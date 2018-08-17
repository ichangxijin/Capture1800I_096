using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace ImageCapturing
{
    /// <summary>
    /// 路径相关的类
    /// </summary>
    public class ClassPath
    {

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
            return LoadCursorFromFile(ClassPath.GetCursorPath() + "\\" + constFileName);
        }


        private static string demoData = GetStartupPath() + "\\Demo Data";

        private static string GetStartupPath()
        {
            System.IO.DirectoryInfo DirectoryInfo = System.IO.Directory.GetParent(System.Windows.Forms.Application.StartupPath);
            return DirectoryInfo.ToString();
        }
        
        /// <summary>
        /// 得到或设置Demo数据所在的路径
        /// </summary>
        public static string DemoPath
        {
            set
            {
                demoData = value;
            }
            get
            {
                return demoData;
            }
        }

        /// <summary>
        /// 设置默认的Demo数据所在的路径
        /// </summary>
        public static void SetDefaultDemoPath(string path)
        {
            demoData = path;
        }

        /// <summary>
        /// 得到system所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppOutPath()
        {
            return GetStartupPath() + "\\system";
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
        /// 得到data所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataPath()
        {
            string path = GetStartupPath() + "\\" + SelectDataPath();
            if (!Directory.Exists(path))
            {
                path = GetStartupPath() + "\\data";
            }
            return path;
        }

        /// <summary>
        /// 选择data文件夹
        /// </summary>
        /// <returns></returns>
        private static string SelectDataPath()
        {
            string dataFolder = null;
            XmlDocument config = new XmlDocument();
            try
            {
                string dataPath = GetAppOutPath() + "\\data.xml";
                if (File.Exists(dataPath))
                {
                    config.Load(dataPath);
                }
                if (!config.HasChildNodes)
                {
                    XmlNode node = config.CreateNode(XmlNodeType.Element, "data", "");
                    ((XmlElement)node).SetAttribute("Value", "data");
                    config.AppendChild(node);
                    config.Save(dataPath);
                    dataFolder = "data";
                }
                else
                {
                    XmlNode nd = config.SelectSingleNode("data");
                    if (nd != null)
                    {
                        XmlElement ele = nd as XmlElement;
                        dataFolder = ele.GetAttribute("Value");
                    }
                }
            }
            catch (System.Exception ex)
            { }
            if (dataFolder == null || dataFolder == "") dataFolder = "data";
            return dataFolder;
        }

        /// <summary>
        /// 得到XML所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataXmlPath()
        {
            return GetAppDataPath() + "\\Xml";
        }
        
        public static string GetAppDataConfigPath()
        {
            return GetAppDataPath() + "\\Config";
        }
        
        /// <summary>
        /// 得到Temp所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataTempPath()
        {
            string path = GetAppDataPath() + "\\Temp";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        /// <summary>
        /// 得到Data所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataXmlDataPath()
        {
            return GetAppDataXmlPath() + "\\Data";
        }

        /// <summary>
        /// 得到System所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataSystemPath()
        {
            return GetAppDataPath() + "\\System";
        }

        /// <summary>
        /// 得到Machine所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataXmlMachinePath()
        {
            return GetAppDataXmlPath() + "\\Machine";
        }

        /// <summary>
        /// 得到Patient所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataPatientPath()
        {
            return GetAppDataPath() + "\\Patient";
        }

        /// <summary>
        /// 得到Dicom所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataXmlDicomPath()
        {
            return GetAppDataXmlPath() + "\\Dicom";
        }

        /// <summary>
        /// 得到Demo所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetDemoPath()
        {
            return demoData;
        }

        /// <summary>
        /// 得到Sql所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataSqlPath()
        {
            return GetAppDataPath() + "\\Sql";
        }

        /// <summary>
        /// 得到Report所在的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppDataXmlReportPath()
        {
            return GetAppDataPath() + "\\Report";
        }

        /// <summary>
        /// 创建指定Data的路径
        /// </summary>
        /// <param name="ServerName">服务器名</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="StudyID">Study ＩＤ</param>
        public static void CreateXmlDataDirectory(string ServerName, string dbName, string StudyID)
        {
            string DirectoryName = GetDirectory(ServerName, dbName, StudyID);
            DirectoryInfo Dir = new DirectoryInfo(DirectoryName);
            if (!Dir.Exists)
            {
                Dir.Create();
            }
        }

        /// <summary>
        /// 得到指定Data的路径
        /// </summary>
        /// <param name="ServerName">服务器名</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="study">
        /// Data类型
        /// (0://patientsub  1://patienttotal   2: //mdmsub  3://mdmtotal 4://AutoFieldMaxValue 5://TableStucture)
        /// </param>
        /// <returns></returns>
        public static string GetDirectory(string ServerName, string dbName, string study)
        {
            if (study != "")
            {
                return ClassPath.GetAppDataXmlDataPath() + "\\" + ServerName + "\\" + dbName + "\\" + study;
            }
            else
            {
                return ClassPath.GetAppDataXmlDataPath() + "\\" + ServerName + "\\" + dbName;
            }
        }
    }
}
