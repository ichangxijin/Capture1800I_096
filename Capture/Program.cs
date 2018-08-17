using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ImageCapturing;

namespace ImageCapturing
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //TCursor.LoadAllCursors(System.Windows.Forms.Application.StartupPath);

            Application.Run(new TestCapture());
        }
    }
}
