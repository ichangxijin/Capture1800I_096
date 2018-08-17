using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace ImageCapturing
{
    public class LogFPD
    {
        public static string Path_LogFPD = CapturePub.LogPath + "\\LogFPD.txt";

        public static void WriteLogStartTag(string tag)
        {
            try
            {
                if (!File.Exists(Path_LogFPD))
                {
                    if (!Directory.Exists(CapturePub.LogPath)) Directory.CreateDirectory(CapturePub.LogPath);
                    File.Create(Path_LogFPD);
                }
                FileStream fs = new FileStream(Path_LogFPD, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("==================================================================");
                if (tag != "")
                {
                    WriteLog(tag);
                }
                sw.Flush();
                fs.Close();
                sw.Close();
                fs.Dispose();
                sw.Dispose();
            }
            catch
            {
            }
        }

        public static void WriteLog(string log)
        {
            try
            {
                string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ff");
                if (!File.Exists(Path_LogFPD))
                {
                    if (!Directory.Exists(CapturePub.LogPath)) Directory.CreateDirectory(CapturePub.LogPath);
                    File.Create(Path_LogFPD);
                }
                FileStream fs = new FileStream(Path_LogFPD, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                log = timeNow + " | " + log;
                sw.WriteLine(log);
                sw.Flush();
                fs.Close();
                sw.Close();
                fs.Dispose();
                sw.Dispose();
            }
            catch
            {
            }
        }
    }
}