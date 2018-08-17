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
    public partial class CorrectFile : AMRT.ShowBaseControl
    {
        public CorrectFile()
        {
            InitializeComponent();
        }

        private void CorrectFile_Load(object sender, EventArgs e)
        {
            try
            {
                string ss = CapturePub.readCaptrueValue(XmlField.SortGainFolder, false);
                if (Directory.Exists(ss))
                {

                    dirListBox.Path = ss;
                }
                else
                {
                    dirListBox.Path = CapturePub.SaveDFPath;
                }
            }
            catch
            {
                dirListBox.Path = CapturePub.SaveDFPath;
            }

            this.fileListBox.Path = this.dirListBox.Path + "\\";
            dirListBox.DoubleClick += new EventHandler(dirListBox_DoubleClick);
            string s = CapturePub.readCaptrueValue(XmlField.OffsetFile);
            if (File.Exists(s))
            {
                textBoxOffsetFile.Tag = s.Substring(0,s.LastIndexOf('\\') + 1);
                textBoxOffsetFile.Text = s.Substring(s.LastIndexOf('\\') + 1);
                toolTipBase.SetToolTip(textBoxOffsetFile, s);
            }
            else
            {
                toolTipBase.SetToolTip(textBoxOffsetFile, "");
            }
            s = CapturePub.readCaptrueValue(XmlField.GainFile);
            if (File.Exists(s))
            {
                textBoxGainFile.Tag = s.Substring(0, s.LastIndexOf('\\') + 1);
                textBoxGainFile.Text = s.Substring(s.LastIndexOf('\\') + 1);
                toolTipBase.SetToolTip(textBoxGainFile, s);
            }
            else
            {
                toolTipBase.SetToolTip(textBoxGainFile, "");
            }

            s = CapturePub.readCaptrueValue(XmlField.GainSeqFile_Image);
            if (File.Exists(s))
            {
                textBoxGainSeqFile_Image.Tag = s.Substring(0, s.LastIndexOf('\\') + 1);
                textBoxGainSeqFile_Image.Text = s.Substring(s.LastIndexOf('\\') + 1);
                toolTipBase.SetToolTip(textBoxGainSeqFile_Image, s);
            }
            else
            {
                toolTipBase.SetToolTip(textBoxGainSeqFile_Image, "");
            }
            s = CapturePub.readCaptrueValue(XmlField.GainSeqFile_Dose);
            if (File.Exists(s))
            {
                textBoxGainSeqFile_Dose.Tag = s.Substring(0, s.LastIndexOf('\\') + 1);
                textBoxGainSeqFile_Dose.Text = s.Substring(s.LastIndexOf('\\') + 1);
                toolTipBase.SetToolTip(textBoxGainSeqFile_Dose, s);
            }
            else
            {
                toolTipBase.SetToolTip(textBoxGainSeqFile_Dose, "");
            }

            s = CapturePub.readCaptrueValue(XmlField.PixelMapFile);
            if (File.Exists(s))
            {
                textBoxPixelMapFile.Tag = s.Substring(0, s.LastIndexOf('\\') + 1);
                textBoxPixelMapFile.Text = s.Substring(s.LastIndexOf('\\') + 1);
                toolTipBase.SetToolTip(textBoxPixelMapFile, s);
            }
            else
            {
                toolTipBase.SetToolTip(textBoxPixelMapFile, "");
            }
            s = CapturePub.readCaptrueValue(XmlField.MachineQAMachineData);
            if (File.Exists(s))
            {
                txtMachineDataFile.Tag = s.Substring(0, s.LastIndexOf('\\') + 1);
                txtMachineDataFile.Text = s.Substring(s.LastIndexOf('\\') + 1);
                toolTipBase.SetToolTip(txtMachineDataFile, s);
            }
            else
            {
                toolTipBase.SetToolTip(txtMachineDataFile, "");
            } 
        }

        void dirListBox_DoubleClick(object sender, EventArgs e)
        {
            //if (dirListBox.Path.Length < CapturePub.SaveDFPath.Length)
            //{
            //    dirListBox.Path = fileListBox.Path;
            //    return;
            //}
            dirListBox_SelectedIndexChanged(null, null);
        }

        private void dirListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fileListBox.Path = this.dirListBox.Path + "\\";
        }

        private void btnOffsetFileAdd_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                return;
            }
            textBoxOffsetFile.Tag = dirListBox.Path;
            textBoxOffsetFile.Text = fileListBox.SelectedItem.ToString();
            toolTipBase.SetToolTip(textBoxOffsetFile, textBoxOffsetFile.Tag.ToString() + "\\" + textBoxOffsetFile.Text);
        }

        private void btnOffsetFileRemove_Click(object sender, EventArgs e)
        {
            textBoxOffsetFile.Tag = "";
            textBoxOffsetFile.Text = "";
            toolTipBase.SetToolTip(textBoxOffsetFile, "");
        } 
        
        private void btnGainFileAdd_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                return;
            }
            textBoxGainFile.Tag = dirListBox.Path;
            textBoxGainFile.Text = fileListBox.SelectedItem.ToString();
            toolTipBase.SetToolTip(textBoxGainFile, textBoxGainFile.Tag.ToString() + "\\" + textBoxGainFile.Text);
        }

        private void btnGainFileRemove_Click(object sender, EventArgs e)
        {
            textBoxGainFile.Tag = "";
            textBoxGainFile.Text = "";
            toolTipBase.SetToolTip(textBoxGainFile, "");
        }

        private void btnGainSeqFileAdd_Image_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                return;
            }
            textBoxGainSeqFile_Image.Tag = dirListBox.Path;
            textBoxGainSeqFile_Image.Text = fileListBox.SelectedItem.ToString();
            toolTipBase.SetToolTip(textBoxGainSeqFile_Image, textBoxGainSeqFile_Image.Tag.ToString() + "\\" + textBoxGainSeqFile_Image.Text);
        }

        private void btnGainSeqFileRemove_Image_Click(object sender, EventArgs e)
        {
            textBoxGainSeqFile_Image.Tag = "";
            textBoxGainSeqFile_Image.Text = "";
            toolTipBase.SetToolTip(textBoxGainSeqFile_Image, "");
        }

        private void btnGainSeqFileAdd_Dose_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                return;
            }
            textBoxGainSeqFile_Dose.Tag = dirListBox.Path; 
            textBoxGainSeqFile_Dose.Text = fileListBox.SelectedItem.ToString();
            toolTipBase.SetToolTip(textBoxGainSeqFile_Dose, textBoxGainSeqFile_Dose.Tag.ToString() + "\\" + textBoxGainSeqFile_Dose.Text);
        }

        private void btnGainSeqFileRemove_Dose_Click(object sender, EventArgs e)
        {
            textBoxGainSeqFile_Dose.Tag = "";
            textBoxGainSeqFile_Dose.Text = "";
            toolTipBase.SetToolTip(textBoxGainSeqFile_Dose, "");
        }

        private void btnPixelMapFileAdd_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                return;
            }
            textBoxPixelMapFile.Tag = dirListBox.Path;
            textBoxPixelMapFile.Text = fileListBox.SelectedItem.ToString();

            toolTipBase.SetToolTip(textBoxPixelMapFile, textBoxPixelMapFile.Tag.ToString() + "\\" + textBoxPixelMapFile.Text);
        }

        private void btnPixelMapFileRemove_Click(object sender, EventArgs e)
        {
            textBoxPixelMapFile.Tag = "";
            textBoxPixelMapFile.Text = "";
            toolTipBase.SetToolTip(textBoxPixelMapFile, "");
        }

        protected override void gbtOK_Click(object sender, EventArgs e)
        {
            //CapturePub.saveCaptrueValue(XmlField.OffsetFile, textBoxOffsetFile.Tag.ToString() + "\\" + textBoxOffsetFile.Text);
            //CapturePub.saveCaptrueValue(XmlField.GainFile, textBoxGainFile.Tag.ToString() + "\\" + textBoxGainFile.Text);
            CapturePub.saveCaptrueValue(XmlField.GainSeqFile_Image, textBoxGainSeqFile_Image.Tag.ToString() + "\\" + textBoxGainSeqFile_Image.Text);
            //CapturePub.saveCaptrueValue(XmlField.GainSeqFile_Dose, textBoxGainSeqFile_Dose.Tag.ToString() + "\\" + textBoxGainSeqFile_Dose.Text);
            CapturePub.saveCaptrueValue(XmlField.PixelMapFile, textBoxPixelMapFile.Tag.ToString() + "\\" + textBoxPixelMapFile.Text);
            //2011.12.22
            //CapturePub.saveCaptrueValue(XmlField.MachineQAMachineData, txtMachineDataFile.Tag.ToString() + "\\" + txtMachineDataFile.Text);//
            base.gbtOK_Click(sender, e);
        }

        //2.11.12.22
        private void btnMachineDataAdd_Click(object sender, EventArgs e)
        {
            if (fileListBox.SelectedItem == null)
            {
                return;
            }
            txtMachineDataFile.Tag = dirListBox.Path;
            txtMachineDataFile.Text = fileListBox.SelectedItem.ToString();
            //int offset = txtMachineDataFile.Text.con
            toolTipBase.SetToolTip(txtMachineDataFile, txtMachineDataFile.Tag.ToString() + "\\" + txtMachineDataFile.Text);
        }

        //2.11.12.22
        private void btnMachineDataRemove_Click(object sender, EventArgs e)
        {
            txtMachineDataFile.Tag = "";
            txtMachineDataFile.Text = "";
            toolTipBase.SetToolTip(txtMachineDataFile, "");
        }
    }
}
