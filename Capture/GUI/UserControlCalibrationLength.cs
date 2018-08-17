using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace ImageCapturing
{
    public partial class UserControlCalibrationLength : AMRT.ShowBaseControl
    {
        public float length = 0;
        public UserControlCalibrationLength()
        {
            InitializeComponent();
            ShowTitle = "Input Calibration Length";
        }

        protected override void gbtOK_Click(object sender, EventArgs e)
        {
            try
            {
                length = float.Parse(tbLineLength.Text.Trim());
            }
            catch
            { }
            if (length == 0)
            {
                cls_MessageBox.Show("Invalid value.");
                return;
            }
            base.gbtOK_Click(sender, e);
        }
    }
}
