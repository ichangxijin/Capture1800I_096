namespace ImageCapturing
{
    partial class TriggerParameterSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Button1 = new AMRT.GraphicButton();
            this.btn_Close1 = new AMRT.GraphicButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbComFeedInterval = new AMRT.MaskedTextBox();
            this.tbBeamInterval = new AMRT.MaskedTextBox();
            this.tbHZ = new AMRT.MaskedTextBox();
            this.cbBBeamSource = new System.Windows.Forms.ComboBox();
            this.labelBSource = new System.Windows.Forms.Label();
            this.combPort = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblComFeedInterval = new System.Windows.Forms.Label();
            this.lblBeamInterval = new System.Windows.Forms.Label();
            this.labelHZ = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxSleep = new AMRT.MaskedTextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.panelBottom.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbtOK
            // 
            this.gbtOK.Location = new System.Drawing.Point(396, 5);
            this.gbtOK.TabIndex = 7;
            // 
            // gbtCancel
            // 
            this.gbtCancel.Location = new System.Drawing.Point(446, 5);
            this.gbtCancel.TabIndex = 8;
            // 
            // panelBottom
            // 
            this.panelBottom.Location = new System.Drawing.Point(0, 282);
            this.panelBottom.Size = new System.Drawing.Size(477, 42);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(477, 2);
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.groupBox1);
            this.panelCenter.Size = new System.Drawing.Size(477, 282);
            // 
            // Button1
            // 
            this.Button1.BackColor = System.Drawing.Color.Transparent;
            this.Button1.ButtonText = null;
            this.Button1.Hint = "";
            this.Button1.Location = new System.Drawing.Point(0, 0);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(100, 32);
            this.Button1.TabIndex = 0;
            // 
            // btn_Close1
            // 
            this.btn_Close1.BackColor = System.Drawing.Color.Transparent;
            this.btn_Close1.ButtonText = null;
            this.btn_Close1.Hint = "";
            this.btn_Close1.Location = new System.Drawing.Point(0, 0);
            this.btn_Close1.Name = "btn_Close1";
            this.btn_Close1.Size = new System.Drawing.Size(100, 32);
            this.btn_Close1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Location = new System.Drawing.Point(25, 307);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 1);
            this.panel1.TabIndex = 157;
            // 
            // tbComFeedInterval
            // 
            this.tbComFeedInterval.DigitMaxValue = 2.147484E+09F;
            this.tbComFeedInterval.DigitMinValue = 40F;
            this.tbComFeedInterval.ForeColor = System.Drawing.Color.Blue;
            this.tbComFeedInterval.Location = new System.Drawing.Point(234, 220);
            this.tbComFeedInterval.Masked = AMRT.Mask.Digit;
            this.tbComFeedInterval.Name = "tbComFeedInterval";
            this.tbComFeedInterval.Size = new System.Drawing.Size(216, 28);
            this.tbComFeedInterval.TabIndex = 12;
            this.tbComFeedInterval.Tag = "5";
            // 
            // tbBeamInterval
            // 
            this.tbBeamInterval.DigitMaxValue = 2.147484E+09F;
            this.tbBeamInterval.DigitMinValue = 0F;
            this.tbBeamInterval.ForeColor = System.Drawing.Color.Blue;
            this.tbBeamInterval.Location = new System.Drawing.Point(234, 183);
            this.tbBeamInterval.Masked = AMRT.Mask.Digit;
            this.tbBeamInterval.Name = "tbBeamInterval";
            this.tbBeamInterval.Size = new System.Drawing.Size(216, 28);
            this.tbBeamInterval.TabIndex = 12;
            this.tbBeamInterval.Tag = "4";
            // 
            // tbHZ
            // 
            this.tbHZ.DigitMaxValue = 2.147484E+09F;
            this.tbHZ.DigitMinValue = 0F;
            this.tbHZ.ForeColor = System.Drawing.Color.Blue;
            this.tbHZ.Location = new System.Drawing.Point(234, 146);
            this.tbHZ.Masked = AMRT.Mask.Digit;
            this.tbHZ.Name = "tbHZ";
            this.tbHZ.Size = new System.Drawing.Size(216, 28);
            this.tbHZ.TabIndex = 12;
            this.tbHZ.Tag = "3";
            // 
            // cbBBeamSource
            // 
            this.cbBBeamSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBBeamSource.ForeColor = System.Drawing.Color.Blue;
            this.cbBBeamSource.FormattingEnabled = true;
            this.cbBBeamSource.Items.AddRange(new object[] {
            "CH1",
            "CH2",
            "CH3",
            "CH4"});
            this.cbBBeamSource.Location = new System.Drawing.Point(234, 108);
            this.cbBBeamSource.Name = "cbBBeamSource";
            this.cbBBeamSource.Size = new System.Drawing.Size(216, 29);
            this.cbBBeamSource.TabIndex = 8;
            this.cbBBeamSource.Tag = "2";
            // 
            // labelBSource
            // 
            this.labelBSource.Location = new System.Drawing.Point(20, 109);
            this.labelBSource.Name = "labelBSource";
            this.labelBSource.Size = new System.Drawing.Size(215, 28);
            this.labelBSource.TabIndex = 138;
            this.labelBSource.Text = "Signal Source";
            this.labelBSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // combPort
            // 
            this.combPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combPort.ForeColor = System.Drawing.Color.Blue;
            this.combPort.FormattingEnabled = true;
            this.combPort.Location = new System.Drawing.Point(234, 33);
            this.combPort.Name = "combPort";
            this.combPort.Size = new System.Drawing.Size(216, 29);
            this.combPort.TabIndex = 8;
            this.combPort.Tag = "1";
            this.combPort.SelectedIndexChanged += new System.EventHandler(this.combPort_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 28);
            this.label2.TabIndex = 138;
            this.label2.Text = "Trigger Port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblComFeedInterval
            // 
            this.lblComFeedInterval.Location = new System.Drawing.Point(20, 223);
            this.lblComFeedInterval.Name = "lblComFeedInterval";
            this.lblComFeedInterval.Size = new System.Drawing.Size(215, 28);
            this.lblComFeedInterval.TabIndex = 135;
            this.lblComFeedInterval.Text = "Feed Interval(ms)";
            this.lblComFeedInterval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBeamInterval
            // 
            this.lblBeamInterval.Location = new System.Drawing.Point(20, 185);
            this.lblBeamInterval.Name = "lblBeamInterval";
            this.lblBeamInterval.Size = new System.Drawing.Size(215, 28);
            this.lblBeamInterval.TabIndex = 135;
            this.lblBeamInterval.Text = "Signal Interval(ms)";
            this.lblBeamInterval.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelHZ
            // 
            this.labelHZ.Location = new System.Drawing.Point(20, 147);
            this.labelHZ.Name = "labelHZ";
            this.labelHZ.Size = new System.Drawing.Size(215, 28);
            this.labelHZ.TabIndex = 135;
            this.labelHZ.Text = "Signal Timeout";
            this.labelHZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxSleep);
            this.groupBox1.Controls.Add(this.label45);
            this.groupBox1.Controls.Add(this.tbComFeedInterval);
            this.groupBox1.Controls.Add(this.combPort);
            this.groupBox1.Controls.Add(this.tbBeamInterval);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbHZ);
            this.groupBox1.Controls.Add(this.labelHZ);
            this.groupBox1.Controls.Add(this.lblBeamInterval);
            this.groupBox1.Controls.Add(this.lblComFeedInterval);
            this.groupBox1.Controls.Add(this.labelBSource);
            this.groupBox1.Controls.Add(this.cbBBeamSource);
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(471, 264);
            this.groupBox1.TabIndex = 162;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trigger Setting";
            // 
            // textBoxSleep
            // 
            this.textBoxSleep.DigitMaxValue = 2.147484E+09F;
            this.textBoxSleep.DigitMinValue = 0F;
            this.textBoxSleep.ForeColor = System.Drawing.Color.Blue;
            this.textBoxSleep.Location = new System.Drawing.Point(234, 71);
            this.textBoxSleep.Masked = AMRT.Mask.Digit;
            this.textBoxSleep.Name = "textBoxSleep";
            this.textBoxSleep.Size = new System.Drawing.Size(216, 28);
            this.textBoxSleep.TabIndex = 139;
            // 
            // label45
            // 
            this.label45.Location = new System.Drawing.Point(20, 71);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(215, 28);
            this.label45.TabIndex = 140;
            this.label45.Text = "Beam On Delay Time(ms)";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TriggerParameterSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Name = "TriggerParameterSetting";
            this.Size = new System.Drawing.Size(477, 324);
            this.Load += new System.EventHandler(this.CaptureParameterSetting_Load);
            this.panelBottom.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GraphicButton Button1;
        private GraphicButton btn_Close1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox combPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbBBeamSource;
        private System.Windows.Forms.Label labelBSource;
        private MaskedTextBox tbHZ;
        private System.Windows.Forms.Label labelHZ;
        private MaskedTextBox tbComFeedInterval;
        private System.Windows.Forms.Label lblComFeedInterval;
        private MaskedTextBox tbBeamInterval;
        private System.Windows.Forms.Label lblBeamInterval;
        private System.Windows.Forms.GroupBox groupBox1;
        private MaskedTextBox textBoxSleep;
        private System.Windows.Forms.Label label45;
    }
}