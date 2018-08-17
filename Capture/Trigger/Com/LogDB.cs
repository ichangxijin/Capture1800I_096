using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace ImageCapturing
{
    public partial class Database
    {
        public class LogDB : LogBase
        {
            #region - 字段 -
            public static string LogSFile
            {
                get
                {
                    return CapturePub.LogPath + "\\LogS.dat";
                }
            }

            public static string LogRFile
            {
                get
                {
                    return CapturePub.LogPath + "\\LogR.dat";
                }
            }

            public static string TxtLogName
            {
                get
                {
                    return CapturePub.LogPath + "\\LogALL.txt";
                }
            }
                

            private static string PIDLogFileName = "";
            public static bool WriteBinLog = false;

            private static string SplitS = " | ";
            public static bool WriteTxtLog = false;           
            
            /// <summary>
            /// Log数据表
            /// </summary>
            public static DataTable dt = new DataTable("ComLog");

            #endregion

            /// <summary>
            /// 创建Log文件
            /// </summary>
            public static void Create()
            {
                string FullPath = CapturePub.LogPath;
                if (!Directory.Exists(FullPath))
                {
                    Directory.CreateDirectory(FullPath);
                }
                PIDLogFileName = FullPath;
                bool IsBig = CheckFileOver(LogSFile, FileMaxSize) 
                             || CheckFileOver(LogRFile, 4 * FileMaxSize)
                             || CheckFileOver(TxtLogName, FileMaxSize);
                if (IsBig)
                {
                    string bakName = DateTime.Now.ToString("yyyy_MM_dd_HHmmss") + ".dat";
                    BackLogFile(bakName);
                }
                CreateTable();
            }

            private static void CreateTable()
            {
                if (dt.Columns.Count == 0)
                {
                    dt.Columns.Add("Time", typeof(DateTime));
                    dt.Columns.Add("IsSend", typeof(bool));
                    dt.Columns.Add("Cmd", typeof(string));
                    dt.Columns.Add("Data", typeof(string));
                }
            }

            public static void SaveTxtLog(string log)
            {
                if (!WriteTxtLog)
                {
                    return;
                }

                try
                {
                    string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
                    if (!File.Exists(TxtLogName))
                    {
                        if (!Directory.Exists(CapturePub.LogPath)) Directory.CreateDirectory(CapturePub.LogPath);
                        File.Create(TxtLogName);
                    }

                    using (StreamWriter sw = File.AppendText(TxtLogName))//, FileMode.Append, FileAccess.Write, FileShare.Delete))
                    {
                        log = timeNow + SplitS + log + "\r\n";
                        sw.Write(log);
                        //sw.WriteLine(log);//空一行,用于和其他次记录区分开
                        sw.Close();
                    }
                }
                catch
                {
                }
            }

            /// <summary>
            /// 保存Log
            /// </summary>
            /// <param name="cmd">命令</param>
            /// <param name="data">数据</param>
            /// <param name="IsSend">是否是发送数据</param>
            public static void SaveBinLog(byte cmd, byte[] data, bool IsSend)
            {
                if (!WriteBinLog)
                {
                    return;
                }

                try
                {
                    string file = IsSend ? LogSFile : LogRFile;
                    byte[] Time = BitConverter.GetBytes(DateTime.Now.ToBinary());
                    using (BinaryWriter BW = new BinaryWriter(File.Open(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                    {
                        byte[] Flag = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                        BW.Write(Flag);
                        BW.Write(Time);
                        BW.Write(cmd);
                        if (data != null)
                        {
                            BW.Write((byte)data.Length);
                            BW.Write(data);
                        }
                        else
                        {
                            BW.Write(0x00);
                        }
                        BW.Close();
                    }
                }
                catch (System.Exception ex)
                {
                    Database.LogDB.SaveTxtLog(ex.ToString());
                }                
            }

            public static void CreateNewPIDLogFile()
            {
                if(!File.Exists(PIDLogFileName)) PIDLogFileName += "\\PIDLog_" + DateTime.Now.ToString("yyyymmddhhMMss");
            }

            private static void SavePIDLog(byte cmd, byte[] data) //temp add 2010/05/15
            {
                string file = PIDLogFileName;
                if (!File.Exists(file)) CreateNewPIDLogFile();// return;
                file = PIDLogFileName;
                byte[] Time = BitConverter.GetBytes(DateTime.Now.ToBinary());
                using (BinaryWriter BW = new BinaryWriter(File.Open(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    byte[] Flag = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    BW.Write(Flag);
                    BW.Write(Time);
                    BW.Write(cmd);
                    if (data != null)
                    {
                        BW.Write((byte)data.Length);
                        BW.Write(data);
                    }
                    else
                    {
                        BW.Write(0x00);
                    }
                    BW.Close();
                }
            }

            /// <summary>
            /// 读取Log数据表
            /// </summary>
            /// <returns></returns>
            public static DataTable ReadBinLog(string file)
            {
                string s = LogSFile;
                string r = LogRFile;
                if (file != "")
                {
                    CreateTable();

                    int pos = file.LastIndexOf("\\");
                    string path = file.Substring(0, pos + 1);
                    file = file.Remove(0, pos + 5);
                    s = path + "LogS" + file;
                    r = path + "LogR" + file;
                }
                ReadLogToTable(s, dt, true);
                ReadLogToTable(r, dt, false);
                return dt;
            }

            public static DataTable ReadBinLogByFile(string file)
            {
                if (file != "")
                {
                    CreateTable();

                    int pos = file.LastIndexOf("\\");
                    string name = file.Substring(pos + 1, file.Length - pos - 1);
                    ReadLogToTable(file, dt, name.ToUpper().Contains("LOGS"));
                }

                return dt;
            }

            /// <summary>
            /// 备份Log文件
            /// </summary>
            /// <param name="bakName">备份文件名</param>
            private static void BackLogFile(string bakName)
            {
                string FullPath = CapturePub.LogPath;
                BackFile(LogSFile, FullPath + "\\LogS_" + bakName);
                BackFile(LogRFile, FullPath + "\\LogR_" + bakName);
                BackFile(TxtLogName, FullPath + "\\LogALL_" + bakName);
            }

            /// <summary>
            /// 读取Log数据到数据表
            /// </summary>
            /// <param name="file">文件名</param>
            /// <param name="dt">数据表</param>
            /// <param name="IsSend">是否是发送数据</param>
            private static void ReadLogToTable(string file, DataTable dt, bool IsSend)
            {
                long FileLen = 0;
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    FileLen = fs.Length;
                    fs.Dispose();
                }
                int index = 0;
                //if (IsSend) index = indexS;
                //else index = indexR;

                using (BinaryReader BR = new BinaryReader(File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
                {
                    if (index < FileLen) BR.ReadBytes(index);    //如果有新数据进来，就把前面的数据读走。
                    while (index < FileLen)
                    {
                        index++;
                        if (BR.ReadByte() != 0xff) { continue; }
                        index++;
                        if (BR.ReadByte() != 0xff) { continue; }
                        index++;
                        if (BR.ReadByte() != 0xff) { continue; }
                        index++;
                        if (BR.ReadByte() != 0xff) { continue; }

                        byte[] t = new byte[8];
                        BR.Read(t, 0, 8);
                        index += 8;
                        DateTime time = DateTime.FromBinary(BitConverter.ToInt64(t, 0));


                        byte[] cmd = new byte[1];
                        BR.Read(cmd, 0, 1);
                        index++;

                        byte[] len = new byte[1];
                        BR.Read(len, 0, 1);
                        index++;

                        string data = "";
                        if (len[0] > 0)
                        {
                            index += len[0];
                            for (int i = 0; i < len[0]; i++)
                            {
                                byte tmp = BR.ReadByte();
                                //if (tmp < 16) data += "0" + tmp.ToString("X");
                                //else data += tmp.ToString("X");
                                data += tmp.ToString("X2");
                                data += " ";
                            }
                        }
                        else
                        {
                            BR.ReadBytes(3);
                            index += 3;
                        }

                        DataRow dr = dt.NewRow();
                        dr["Time"] = time;
                        dr["IsSend"] = IsSend;
                        dr["Cmd"] = cmd[0].ToString("X");
                        dr["Data"] = data;
                        dt.Rows.Add(dr);

                    }
                    BR.Close();
                }

                //if (IsSend) indexS = index;
                //else indexR = index;
            }
        }
    }
}