using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace ImageCapturing
{
    public class CapturePub
    {
        public static string DataPath = Directory.GetParent(Application.StartupPath).ToString() + "\\data";
        public static string SystemPath = Directory.GetParent(Application.StartupPath).ToString() + "\\system";
        
        private static string CaptrueOpsFile
        {
            get
            {
                return DataPath + "\\System\\Capture.xml";
            }
        }

        public static string CareRayPath
        {
            get
            {
                string Folder = SystemPath + @"\CareRay\";
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }
                return Folder;
            }
        }

        public static string LogPath
        {
            get
            {
                string Folder = DataPath + "\\Log";
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }
                return Folder;
            }
        }

        public static string LinkFilePath
        {
            get
            {
                string Folder = DataPath + "\\Config";
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }
                return Folder;
            }
        }

        public static string SaveDFPath
        {
            get
            {
                string Folder = DataPath + "\\DF";
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }
                return Folder;
            }
        }
        
        public static DataSet GetData(string fileName)
        {
            DataSet ds = new DataSet();
            if (!File.Exists(fileName))
            {
                string defaultName = fileName.Insert(fileName.LastIndexOf('\\'), "\\Default");
                if (File.Exists(defaultName))
                {
                    File.Copy(defaultName, fileName, false);
                }
            }
            if (File.Exists(fileName))
            {
                try
                {
                    ds.ReadXml(fileName);
                }
                catch (System.Exception ex)
                {
                    if (ex is XmlException)
                    {
                        string defaultName = fileName.Insert(fileName.LastIndexOf('\\'), "\\Default");
                        if (File.Exists(defaultName))
                        {
                            File.Copy(defaultName, fileName, true);
                        }
                        ds = GetData(fileName);
                    }
                }
            }
            return ds;
        }

        public static void WriteData(DataSet ds, string fileName)
        {
            DataSet changeSet = ds.GetChanges();
            changeSet.WriteXml(fileName, XmlWriteMode.WriteSchema);
        }
        
        /// <summary>
        /// 保存参数选项的值
        /// </summary>
        /// <param name="opt"></param>
        /// <param name="val"></param>
        public static void saveCaptrueValue(string opt, string val)
        {
            DataSet ds = GetData(CaptrueOpsFile);
            DataTable db = ds.Tables["ROW"];

            int pos = -1;
            for (int i = 0; i < db.Rows.Count; i++)
            {
                if (db.Rows[i]["Option"].ToString().ToLower() == opt.ToLower())
                {
                    pos = 0;
                    db.Rows[i]["Value"] = val;
                }
            }
            if (pos == -1)
            {
                DataRow dr = db.NewRow();
                dr["Option"] = opt;
                dr["Value"] = val;
                dr["Default_Value"] = val;
                db.Rows.Add(dr);
            }
            WriteData(ds, CaptrueOpsFile);
        }

        public static string readCaptrueValue(string opt)
        {
            return readCaptrueValue(opt, false);
        }

        /// <summary>
        /// 从sysOps.xml读取对应的数据
        /// </summary>
        /// <param name="opt">读取的字段</param>
        /// <param name="isDefault">是否读取缺省值</param>
        /// <returns></returns>
        public static string readCaptrueValue(string opt, bool isDefault)
        {
            string fd = (isDefault) ? "Default_Value" : "Value";
            DataSet ds = GetData(CaptrueOpsFile);
            DataTable db = ds.Tables["ROW"];
            string tmp = "";
            string UNIT = (isDefault) ? "Default_Value" : "4";
            string Coefficient=(isDefault) ? "Default_Value" : "0.9";
            try
            {
                DataRow[] dr = db.Select("Option='" + opt + "'");
                if (dr.Length > 0)
                {
                    tmp = dr[0][fd].ToString();
                }
                else
                {
                    DataRow drNew = db.NewRow();
                    drNew[0] = opt;
                    db.Rows.Add(drNew);
                    WriteData(ds, CaptrueOpsFile);//可能存在问题
                }
            }
            catch (System.Exception ex)
            {
            }
            return tmp;
        }

        /// <summary>
        /// 调用系统DLL，支持彩色光标
        /// </summary>
        /// <param name="Filename"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string Filename);

        public const string TP_crDistance = "TP_crDistance.cur";
        public const string Handmove = "Handmove.cur";

        /// <summary>
        /// 封装的调用光标函数
        /// </summary>
        /// <param name="constFileName"></param>
        /// <returns></returns>
        public static IntPtr LoadCursor(string constFileName)
        {
            return LoadCursorFromFile(GetCursorPath() + "\\" + constFileName);
        }

        /// <summary>
        /// 得到Cursor所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetCursorPath()
        {
            return GetAppOutPath() + "\\Icon\\Cursor";
        }

        /// <summary>
        /// 得到system所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppOutPath()
        {
            return GetStartupPath() + "\\system";
        }

        private static string GetStartupPath()
        {
            System.IO.DirectoryInfo DirectoryInfo = System.IO.Directory.GetParent(System.Windows.Forms.Application.StartupPath);
            return DirectoryInfo.ToString();
        }

        public static bool GenerateInterfaceFilePath(string dirString)
        {
            string sharePath;
            if (dirString.EndsWith("\\"))
            {
                sharePath = dirString + "Share";
            }
            else
            {
                sharePath = dirString + "\\Share";
            }
            if (!Directory.Exists(sharePath))
            {
                //DirectorySecurity security = new DirectorySecurity(sharePath, AccessControlSections.None);
                Directory.CreateDirectory(sharePath);
            }
            string macthResultPath = sharePath + "\\MatchResult";
            if (!Directory.Exists(macthResultPath))
            {
                //DirectorySecurity security = new DirectorySecurity(sharePath, AccessControlSections.None);
                Directory.CreateDirectory(macthResultPath);
            }
            string imagePath = sharePath + "\\Images";
            if (!Directory.Exists(imagePath))
            {
                //DirectorySecurity security = new DirectorySecurity(sharePath, AccessControlSections.None);
                Directory.CreateDirectory(imagePath);
            }
            //string machinePath = sharePath + "\\Machine";
            //if (!Directory.Exists(machinePath))
            //{
            //    //DirectorySecurity security = new DirectorySecurity(sharePath, AccessControlSections.None);
            //    Directory.CreateDirectory(machinePath);
            //}
            return true;
            //if (ShareNetFolder(sharePath, "Share", "Share_IVS") == -1)
            //{
            //    cls_MessageBox.Show("Unable to share directory.");
            //}
            //else
            //{
            //    cls_MessageBox.Show("Share directory successfully.");
            //}

        }
    }
}
