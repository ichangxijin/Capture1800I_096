using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageCapturing
{
    public partial class AddBeam : AMRT.ShowBaseControl
    {
        private string oldBeamName = "";
        private List<FormCapture.BeamParaInformation> BeamList = null;
        private FormCapture.BeamParaInformation BeamInfo = null;

        public AddBeam(string title, FormCapture.BeamParaInformation _beamInfo,List<FormCapture.BeamParaInformation> _beamList)
        {
            ShowTitle = title;
            InitializeComponent();
            BeamList = _beamList;
            BeamInfo = _beamInfo;
            if (BeamInfo.ID < 0) //add
            {
                txtBeamName.Text = GetNewBeamNameDefault();
            }
            else //modify
            {
                oldBeamName = BeamInfo.name;
                this.txtBeamName.Text = oldBeamName;
                this.txtAquireGantry.Text = BeamInfo.gantry_angle.ToString();
                this.txtnote.Text = BeamInfo.Note;
            }
            float[] gantryRange = new float[] { 0, 360 };
            this.txtAquireGantry.DigitMinValue = (int)gantryRange[0];
            this.txtAquireGantry.DigitMaxValue = (int)gantryRange[1];
            toolTipBase.SetToolTip(txtAquireGantry, gantryRange[0].ToString() + "~" + gantryRange[1].ToString());
        }

        private string GetNewBeamNameDefault()
        {
            string newName = "Field";
            int num = 0;
            for (int i = 0; i < BeamList.Count; ++i)
            {
                int index = BeamList[i].name.IndexOf(newName);
                if (index == 0 && BeamList[i].name.Length > newName.Length)
                {
                    int numTemp;
                    if (int.TryParse(BeamList[i].name.Substring(newName.Length, BeamList[i].name.Length - newName.Length), out numTemp))
                    {
                        if (num < numTemp) num = numTemp;
                    }
                }
            }
            num++;
            newName += num.ToString();
            return newName;
        }

        protected override void gbtOK_Click(object sender, EventArgs e)
        {
            string name = txtBeamName.Text.Trim();
            if (name == "")
            {
                MessageBox.Show("The name should not be null !");
                txtBeamName.Focus();
                return;
            }
            else
            {
                if (name != oldBeamName && CheckFieldNameExisted(name))
                {
                    MessageBox.Show("The beam name has existed, input a new one please!");
                    txtBeamName.Focus();
                    return;
                }
            }

            float gantry = 0;
            if (!float.TryParse(txtAquireGantry.Text, out gantry))//2010/08/10 wzf add temp for prtected
            {
                MessageBox.Show("Gantry angle should be number!");
                txtAquireGantry.Focus();
                return;
            }

            float sad = 0;
            if (!float.TryParse(CapturePub.readCaptrueValue("Sad"), out sad))
            {
                sad = 0;
            }
            if (BeamInfo.ID < 0)//add
            {
                BeamInfo.TreatmentDeliveryType = "PORTAL";
                BeamInfo.name = name;
                BeamInfo.gantry_angle = gantry;
                BeamInfo.Note = txtnote.Text;
                BeamInfo.SAD = sad;
                BeamInfo.ISOCenterX = float.MinValue;
                BeamInfo.ISOCenterY = float.MinValue;
                BeamInfo.ISOCenterZ = float.MinValue;
                BeamInfo.PitchAngle = 0;
                BeamInfo.RollAngle = 0;
                BeamInfo.RotateAngle = 0;
            }
            else//Edit
            {
                BeamInfo.name = name;
                BeamInfo.gantry_angle = gantry;
                BeamInfo.Note = txtnote.Text;
            }
            base.gbtOK_Click(sender, e);
        }

        private bool CheckFieldNameExisted(string newName)
        {
            for(int i = 0 ; i < BeamList.Count; i++)
            {
                if (BeamList[i].name == newName)
                {
                    return true;
                }
            }
            return false;
        }
  
    }
}
