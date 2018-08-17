using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AMRT
{
    public partial class ParaSet : ShowBaseControl
    {
        private PortStatus PS;
        public ParaSet(PortStatus ps)
        {
            InitializeComponent();
            PS = ps;
            comboBoxOnValue.Items.AddRange(new string[]{"1","2"});
            combPort.Items.AddRange(new string[] { "278", "378", "3BC" });
            comboBoxInterval.Items.AddRange(new string[] {"1","2","5","10","20" });
            comboBoxTimeOut.Items.AddRange(new string[] { "20", "50", "100", "200", "500", "1000" });
            combPort.SelectedIndex = 0;
            comboBoxOnValue.SelectedIndex = 1;
            comboBoxInterval.SelectedIndex = 1;
            comboBoxTimeOut.SelectedIndex = 1;
        }

        private void gbtOK_Click_1(object sender, EventArgs e)
        {
            if (PS == null)
            {
                MessageBox.Show("Link Error !");
                return;
            }
            PS.SetPort(int.Parse(combPort.SelectedItem.ToString(), System.Globalization.NumberStyles.HexNumber));
            PS.SetOnValue(int.Parse(comboBoxOnValue.SelectedItem.ToString()));
            PS.SetInterval(int.Parse(comboBoxInterval.SelectedItem.ToString()));
            PS.SetTimeout(int.Parse(comboBoxTimeOut.SelectedItem.ToString()));
            this.Close();
        }

        private void gbtCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
