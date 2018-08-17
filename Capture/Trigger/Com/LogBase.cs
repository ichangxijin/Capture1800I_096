using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace ImageCapturing
{
    public partial class Database
    {
        public class LogBase
        {
            #region - 字段 -
            /// <summary>
            /// 文件的最大值
            /// </summary>
            protected const int FileMaxSize = 250000;
            #endregion

            /// <summary>
            /// 检查文件是否过大
            /// </summary>
            /// <param name="file">文件名</param>
            /// <param name="maxSize">最大文件大小</param>
            /// <returns>是否过大</returns>
            protected static bool CheckFileOver(string file, int maxSize)
            {
                FileInfo fi = new FileInfo(file);
                if (!fi.Exists)
                {
                    fi.Create();
                    fi.Refresh();
                }
                long fileSize = fi.Length;
                GC.Collect();

                return (fileSize >= maxSize);
            }

            /// <summary>
            /// 备份Log文件
            /// </summary>
            /// <param name="bakName">备份文件名</param>
            protected static void BackFile(string oldFileName, string bakName)
            {
                File.Copy(oldFileName, bakName);
                File.Delete(oldFileName);

                FileInfo LogSFileInfo = new FileInfo(oldFileName);
                LogSFileInfo.Create();
            }         
        }
    }
}
