using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ImageCapturing
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public partial class ControlBase : UserControl
    {
        public ControlBase()
        {
            InitializeComponent();
            mUnitID = Guid.NewGuid().ToString();
        }

       
        private string mUnitID = "";
         /// <summary>
        /// 显示单元构造时生成的唯一标志
        /// </summary>
        public string UnitID
        {
            get { return mUnitID; }
        }

        /// <summary>
        /// 显示单元的名称
        /// </summary>
        protected string mUnitName;
        public string UnitName
        {
            get { return mUnitName; }
            set { mUnitName = value; }
        }

        public void SetLocation(int x, int y, Size size)
        {
            this.Location = new Point(x, y);
            this.Size = size;
        }

        public void SetLocation(Rectangle rec)
        {
            this.Location = rec.Location;
            this.Size = rec.Size;
        }
    }
}
