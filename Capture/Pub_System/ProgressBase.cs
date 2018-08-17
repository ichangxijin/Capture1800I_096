using System;
using System.Collections.Generic;
using System.Text;

namespace ImageCapturing
{
    /// <summary>
    /// 用来设置进度的基础参数和事件
    /// </summary>
    public class ProgressBase
    {
        #region Class Members

        /// <summary>
        /// 设置任务数委托
        /// </summary>
        /// <param name="count">进度增加数</param>
        public delegate void ProgressAddDelegate(int count);
        /// <summary>
        /// 进度初始
        /// </summary>
        /// <param name="info"></param>
        /// <param name="maxLen"></param>
        public delegate void ProgressBeginDelegate(string info, int maxLen);
        /// <summary>
        /// 进度结束
        /// </summary>
        public delegate void ProgressEndDelegate();
        /// <summary>
        /// 启动进度显示
        /// </summary>
        public event ProgressBeginDelegate ProgressBegin;
        /// <summary>
        /// 进度推进
        /// </summary>
        public event ProgressAddDelegate ProgressAdd;
        /// <summary>
        /// 进度结束
        /// </summary>
        public event ProgressEndDelegate ProgressEnd;

        #endregion

        #region Functions

        protected void SetProgressBegin(string info, int maxLen)
        {
            if (ProgressBegin != null)
            {
                ProgressBegin(info, maxLen);
            }
        }

        /// <summary>
        /// 设置进度
        /// </summary>
        /// <param name="value">进度增加的值</param>
        protected void SetProgressAdd(int value)
        {
            if (ProgressAdd != null)
            {
                ProgressAdd(value);
            }
        }

        protected void SetProgressEnd()
        {
            if (ProgressEnd != null)
            {
                ProgressEnd();
            }
        }

        #endregion


        #region Write Log Information(log4net)

        public static void WriteInfo(string logString)
        {

        }

        public static void WriteDebug(string logString)
        {

        }

        public static void WriteWarn(string logString)
        {

        }

        public static void WriteError(string logString)
        {

        }

        public static void WriteFatal(string logString)
        {

        }

        #endregion
    }
}
