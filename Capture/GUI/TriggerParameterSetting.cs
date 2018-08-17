using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace ImageCapturing
{
    public partial class TriggerParameterSetting : AMRT.ShowBaseControl
    {
        public TriggerParameterSetting()
        {
            InitializeComponent();
        }

        protected override void gbtOK_Click(object sender, EventArgs e)
        {
            if (combPort.SelectedIndex < 0 || combPort.SelectedItem.ToString() == "")
            {
                cls_MessageBox.Show("Please check Trigger Port !");
                return;
            }

            
            //Trigger Parameter
            CapturePub.saveCaptrueValue(XmlField.TriggerPort, combPort.Text.Trim());
            CapturePub.saveCaptrueValue(XmlField.TriggerDelayTime, textBoxSleep.Text);
            CapturePub.saveCaptrueValue(XmlField.SignalSourceID, (cbBBeamSource.SelectedIndex + 1).ToString());
            CapturePub.saveCaptrueValue(XmlField.SignalTimeout, tbHZ.Text.Trim());
            CapturePub.saveCaptrueValue(XmlField.SignalInterval, tbBeamInterval.Text.Trim());
            CapturePub.saveCaptrueValue(XmlField.SignalFeedInterval, tbComFeedInterval.Text.Trim());
            base.gbtOK_Click(sender, e);
        }

        private void CaptureParameterSetting_Load(object sender, EventArgs e)
        {
            
            //Trigger Parameter
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();//获取机器的COM口信息
            List<string> portlist = new List<string>();
            portlist.Add("NONE");
            for (int i = 0; i < ports.Length; ++i)
            {
                if (!portlist.Contains(ports[i]))
                {
                    portlist.Add(ports[i]);
                }
            }
            combPort.Items.AddRange(portlist.ToArray());
            string xmlValue = CapturePub.readCaptrueValue(XmlField.TriggerPort).Trim();
            if (xmlValue != "") combPort.Text = xmlValue;
            xmlValue = CapturePub.readCaptrueValue(XmlField.TriggerDelayTime);
            textBoxSleep.Text = (xmlValue == "" ? "5000" : xmlValue);
            xmlValue = CapturePub.readCaptrueValue(XmlField.SignalSourceID);
            if (xmlValue == "" || xmlValue == null)
            {
                cbBBeamSource.SelectedIndex = 0;
            }
            else
            {
                cbBBeamSource.Text = "CH" + xmlValue;
            }
            //this.cbBBeamSource.Text = (xmlValue == "" ? "1" : xmlValue);
            xmlValue = CapturePub.readCaptrueValue(XmlField.SignalTimeout);
            this.tbHZ.Text = (xmlValue == "" ? "0" : xmlValue);
            xmlValue = CapturePub.readCaptrueValue(XmlField.SignalInterval);//BeamOn信号认为消失的时间间隔
            this.tbBeamInterval.Text = (xmlValue == "" ? "20" : xmlValue);
            xmlValue = CapturePub.readCaptrueValue(XmlField.SignalFeedInterval);
            this.tbComFeedInterval.Text = (xmlValue == "" ? "100" : xmlValue);

            combPort.Focus();
        }

        private void combPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isCom = combPort.Text.ToLower().Contains("com");
            //this.labelBSource.Enabled = isCom;
            this.cbBBeamSource.Enabled = isCom;
            //labelHZ.Enabled = isCom;
            tbHZ.Enabled = isCom;
            //lblBeamInterval.Enabled = isCom;
            tbBeamInterval.Enabled = isCom;
            //lblComFeedInterval.Enabled = isCom;
            tbComFeedInterval.Enabled = isCom;
        }
    }
}
