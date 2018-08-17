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
    public partial class CaptureParameterSetting : AMRT.ShowBaseControl
    {
        public CaptureParameterSetting()
        {
            InitializeComponent();
        }

        protected override void gbtOK_Click(object sender, EventArgs e)
        {
            //Capture Parameter
            //CapturePub.saveCaptrueValue(XmlField.CaptureMode, comboBoxCaptureMode.SelectedIndex.ToString());
            CapturePub.saveCaptrueValue(XmlField.IntegrationTime, textIntegrationTime.Text);
            CapturePub.saveCaptrueValue(XmlField.CaptureFrameCount, textBoxFrameCount.Text);
            CapturePub.saveCaptrueValue(XmlField.FrameDelayTime, textBoxFrameDelay.Text);
            CapturePub.saveCaptrueValue(XmlField.GainMode, comboBoxGainMode.SelectedIndex.ToString());
            CapturePub.saveCaptrueValue(XmlField.BinningMode, (comboBoxBinningMode.SelectedIndex + 1).ToString());
            //CapturePub.saveCaptrueValue(XmlField.ImageCorrection, comboBoxPanelInterface.SelectedIndex.ToString());
            if (CapturePub.readCaptrueValue(XmlField.PanelInterface, false) != comboBoxPanelInterface.SelectedIndex.ToString())
            {
                CapturePub.saveCaptrueValue(XmlField.PanelInterface, comboBoxPanelInterface.SelectedIndex.ToString());
                CapturePub.saveCaptrueValue(XmlField.NeedCheckPanelInterface, "T");
            }

           
            if (comboBoxTriggerMode.SelectedIndex == 1)
            {
                CapturePub.saveCaptrueValue(XmlField.TriggerMode, "0");
            } 
            else
            {
                CapturePub.saveCaptrueValue(XmlField.TriggerMode, "3");
            }
            base.gbtOK_Click(sender, e);
        }

        private void CaptureParameterSetting_Load(object sender, EventArgs e)
        {
            //Capture Parameter
            int xmlValue;
            string xmlValueString;
            //if (int.TryParse(CapturePub.readCaptrueValue(XmlField.CaptureMode,false), out xmlValue))
            //{
            //    comboBoxCaptureMode.SelectedIndex = xmlValue;
            //}
            //else
            //{
            //    comboBoxCaptureMode.SelectedIndex = 0;
            //}
            xmlValueString = CapturePub.readCaptrueValue(XmlField.CaptureFrameCount);
            textBoxFrameCount.Text = (xmlValueString == "" ? "1" : xmlValueString);
            xmlValueString = CapturePub.readCaptrueValue(XmlField.IntegrationTime);
            textIntegrationTime.Text = (xmlValueString == "" ? "2000" : xmlValueString);
            xmlValueString = CapturePub.readCaptrueValue(XmlField.FrameDelayTime);
            textBoxFrameDelay.Text = (xmlValueString == "" ? "200" : xmlValueString);

            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.GainMode, false), out xmlValue))
            {
                this.comboBoxGainMode.SelectedIndex = xmlValue;
            }
            else
            {
                this.comboBoxGainMode.SelectedIndex = 0;
            }
            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.BinningMode, false), out xmlValue))
            {
                this.comboBoxBinningMode.SelectedIndex = xmlValue - 1;
            }
            else
            {
                comboBoxBinningMode.SelectedIndex = 0;
            }
            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.PanelInterface, false), out xmlValue))
            {
                this.comboBoxPanelInterface.SelectedIndex = xmlValue;
            }
            else
            {
                comboBoxPanelInterface.SelectedIndex = 0;
            }

            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.TriggerMode, false), out xmlValue))
            {
                if (xmlValue == 0)
                {
                    comboBoxTriggerMode.SelectedIndex = 1;
                }
                else
                {
                    comboBoxTriggerMode.SelectedIndex = 0;
                }
            }
            else
            {
                comboBoxTriggerMode.SelectedIndex = 0;
            } 
            //if (int.TryParse(CapturePub.readCaptrueValue(XmlField.ImageCorrection, false), out xmlValue))
            //{
            //    this.comboBoxPanelInterface.SelectedIndex = xmlValue;
            //}
            //else
            //{
            //    comboBoxPanelInterface.SelectedIndex = 0;
            //}

            textBoxFrameCount.SelectAll();
            textBoxFrameCount.Focus();
        }
    }
}
