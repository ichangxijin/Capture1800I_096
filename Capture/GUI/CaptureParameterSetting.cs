using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageCapturing
{
    public partial class CaptureParameterSetting : Form
    {
        public CaptureParameterSetting()
        {
            InitializeComponent();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            //Capture Parameter
            if (comboBoxWorkMode.SelectedIndex == 0)
            {
                CapturePub.saveCaptrueValue(XmlField.KZCheckMode, ((int)CareRayInterface.CheckMode.MODE_RAD).ToString());
            }
            else if (comboBoxWorkMode.SelectedIndex == 1)
            {
                CapturePub.saveCaptrueValue(XmlField.KZCheckMode, ((int)CareRayInterface.CheckMode.MODE_FLUORO_START + 2).ToString());
            }
            else if (comboBoxWorkMode.SelectedIndex == 2)
            {
                CapturePub.saveCaptrueValue(XmlField.KZCheckMode, ((int)CareRayInterface.CheckMode.MODE_RAD).ToString());
            }
            CapturePub.saveCaptrueValue(XmlField.CareRay_ExposureTime, this.textBoxExposureTime.Text);
            CapturePub.saveCaptrueValue(XmlField.CareRay_ExposureDelay, this.textBoxDelayTime.Text);
            CapturePub.saveCaptrueValue(XmlField.CareRay_ExposureWait, this.textBoxWaitTime.Text);


            string config_file = CapturePub.CareRayPath + "Config.ini";
            CareRayInterface.SaveConfigOptionValue(config_file, "ipAddress",textBoxPanelIP.Text.Trim());



            this.DialogResult = DialogResult.OK;
        }

        private void CaptureParameterSetting_Load(object sender, EventArgs e)
        {
            //Capture Parameter
            string xmlValueString;
            xmlValueString = CapturePub.readCaptrueValue(XmlField.KZCheckMode);
            if (xmlValueString == ((int)CareRayInterface.CheckMode.MODE_RAD).ToString())
            {
                comboBoxWorkMode.SelectedIndex = 0;
            }
            else if (xmlValueString == ((int)CareRayInterface.CheckMode.MODE_FLUORO_START + 2).ToString())
            {
                comboBoxWorkMode.SelectedIndex = 1;
            }
            else if (xmlValueString == ((int)CareRayInterface.CheckMode.MODE_FLUORO_START).ToString())
            {
                comboBoxWorkMode.SelectedIndex = 2;
            }

            this.textBoxExposureTime.Text = CapturePub.readCaptrueValue(XmlField.CareRay_ExposureTime);
            this.textBoxDelayTime.Text = CapturePub.readCaptrueValue(XmlField.CareRay_ExposureDelay);
            this.textBoxWaitTime.Text = CapturePub.readCaptrueValue(XmlField.CareRay_ExposureWait);


            string config_file = CapturePub.CareRayPath + "Config.ini";
            string PanelIP = CareRayInterface.ReadConfigOptionValue(config_file, "ipAddress");

            this.textBoxPanelIP.Text = PanelIP;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
