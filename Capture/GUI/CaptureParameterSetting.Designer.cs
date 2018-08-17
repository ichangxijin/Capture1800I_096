namespace ImageCapturing
{
    partial class CaptureParameterSetting
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
            this.gbCapSet = new System.Windows.Forms.GroupBox();
            this.comboBoxBinningMode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxGainMode = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxFrameCount = new AMRT.MaskedTextBox();
            this.textBoxFrameDelay = new AMRT.MaskedTextBox();
            this.textIntegrationTime = new AMRT.MaskedTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxPanelInterface = new System.Windows.Forms.ComboBox();
            this.comboBoxTriggerMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panelBottom.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.gbCapSet.SuspendLayout();
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
            this.panelBottom.Location = new System.Drawing.Point(0, 316);
            this.panelBottom.Size = new System.Drawing.Size(502, 42);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(502, 2);
            // 
            // panelProgress
            // 
            this.panelProgress.Size = new System.Drawing.Size(165, 40);
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.gbCapSet);
            this.panelCenter.Size = new System.Drawing.Size(502, 316);
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
            // gbCapSet
            // 
            this.gbCapSet.Controls.Add(this.comboBoxTriggerMode);
            this.gbCapSet.Controls.Add(this.label4);
            this.gbCapSet.Controls.Add(this.comboBoxPanelInterface);
            this.gbCapSet.Controls.Add(this.label2);
            this.gbCapSet.Controls.Add(this.comboBoxBinningMode);
            this.gbCapSet.Controls.Add(this.label3);
            this.gbCapSet.Controls.Add(this.comboBoxGainMode);
            this.gbCapSet.Controls.Add(this.label14);
            this.gbCapSet.Controls.Add(this.textBoxFrameCount);
            this.gbCapSet.Controls.Add(this.textBoxFrameDelay);
            this.gbCapSet.Controls.Add(this.textIntegrationTime);
            this.gbCapSet.Controls.Add(this.label5);
            this.gbCapSet.Controls.Add(this.label46);
            this.gbCapSet.Controls.Add(this.label47);
            this.gbCapSet.Location = new System.Drawing.Point(2, 7);
            this.gbCapSet.Name = "gbCapSet";
            this.gbCapSet.Size = new System.Drawing.Size(497, 299);
            this.gbCapSet.TabIndex = 160;
            this.gbCapSet.TabStop = false;
            this.gbCapSet.Text = "Capture Setting";
            // 
            // comboBoxBinningMode
            // 
            this.comboBoxBinningMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBinningMode.ForeColor = System.Drawing.Color.Blue;
            this.comboBoxBinningMode.FormattingEnabled = true;
            this.comboBoxBinningMode.Items.AddRange(new object[] {
            "Binning 1(default)",
            "Binning 2(x= x/2, y= y/2)"});
            this.comboBoxBinningMode.Location = new System.Drawing.Point(262, 177);
            this.comboBoxBinningMode.Name = "comboBoxBinningMode";
            this.comboBoxBinningMode.Size = new System.Drawing.Size(216, 29);
            this.comboBoxBinningMode.TabIndex = 207;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(17, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(130, 25);
            this.label3.TabIndex = 208;
            this.label3.Text = "Binning Mode";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxGainMode
            // 
            this.comboBoxGainMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGainMode.ForeColor = System.Drawing.Color.Blue;
            this.comboBoxGainMode.FormattingEnabled = true;
            this.comboBoxGainMode.Items.AddRange(new object[] {
            "0.25pF(200μm only)",
            "0.5 pF",
            "1 pF",
            "2 pF",
            "4 pF",
            "8 pF"});
            this.comboBoxGainMode.Location = new System.Drawing.Point(262, 139);
            this.comboBoxGainMode.Name = "comboBoxGainMode";
            this.comboBoxGainMode.Size = new System.Drawing.Size(216, 29);
            this.comboBoxGainMode.TabIndex = 205;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(17, 145);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(130, 25);
            this.label14.TabIndex = 206;
            this.label14.Text = "Gain Mode";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxFrameCount
            // 
            this.textBoxFrameCount.DigitMaxValue = 2.147484E+09F;
            this.textBoxFrameCount.DigitMinValue = 0F;
            this.textBoxFrameCount.ForeColor = System.Drawing.Color.Blue;
            this.textBoxFrameCount.Location = new System.Drawing.Point(262, 28);
            this.textBoxFrameCount.Masked = AMRT.Mask.Digit;
            this.textBoxFrameCount.Name = "textBoxFrameCount";
            this.textBoxFrameCount.Size = new System.Drawing.Size(216, 28);
            this.textBoxFrameCount.TabIndex = 12;
            // 
            // textBoxFrameDelay
            // 
            this.textBoxFrameDelay.DigitMaxValue = 2.147484E+09F;
            this.textBoxFrameDelay.DigitMinValue = 0F;
            this.textBoxFrameDelay.ForeColor = System.Drawing.Color.Blue;
            this.textBoxFrameDelay.Location = new System.Drawing.Point(262, 102);
            this.textBoxFrameDelay.Masked = AMRT.Mask.Digit;
            this.textBoxFrameDelay.Name = "textBoxFrameDelay";
            this.textBoxFrameDelay.Size = new System.Drawing.Size(216, 28);
            this.textBoxFrameDelay.TabIndex = 11;
            // 
            // textIntegrationTime
            // 
            this.textIntegrationTime.DigitMaxValue = 2.147484E+09F;
            this.textIntegrationTime.DigitMinValue = 0F;
            this.textIntegrationTime.ForeColor = System.Drawing.Color.Blue;
            this.textIntegrationTime.Location = new System.Drawing.Point(262, 65);
            this.textIntegrationTime.Masked = AMRT.Mask.Digit;
            this.textIntegrationTime.Name = "textIntegrationTime";
            this.textIntegrationTime.Size = new System.Drawing.Size(216, 28);
            this.textIntegrationTime.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(17, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(215, 28);
            this.label5.TabIndex = 143;
            this.label5.Text = "Integration Time(ms)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label46
            // 
            this.label46.Location = new System.Drawing.Point(17, 106);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(215, 28);
            this.label46.TabIndex = 133;
            this.label46.Text = "Frame Delay Time(ms)";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label47
            // 
            this.label47.Location = new System.Drawing.Point(17, 28);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(215, 28);
            this.label47.TabIndex = 135;
            this.label47.Text = "Capture Frame Count";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(17, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 25);
            this.label2.TabIndex = 214;
            this.label2.Text = "Flat Panel Detector Interface";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxPanelInterface
            // 
            this.comboBoxPanelInterface.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPanelInterface.ForeColor = System.Drawing.Color.Blue;
            this.comboBoxPanelInterface.FormattingEnabled = true;
            this.comboBoxPanelInterface.Items.AddRange(new object[] {
            "Frame Grabber",
            "Network"});
            this.comboBoxPanelInterface.Location = new System.Drawing.Point(262, 215);
            this.comboBoxPanelInterface.Name = "comboBoxPanelInterface";
            this.comboBoxPanelInterface.Size = new System.Drawing.Size(216, 29);
            this.comboBoxPanelInterface.TabIndex = 213;
            // 
            // comboBoxTriggerMode
            // 
            this.comboBoxTriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTriggerMode.ForeColor = System.Drawing.Color.Blue;
            this.comboBoxTriggerMode.FormattingEnabled = true;
            this.comboBoxTriggerMode.Items.AddRange(new object[] {
            "Frame-wise",
            "Data Delivered on Demand"});
            this.comboBoxTriggerMode.Location = new System.Drawing.Point(263, 257);
            this.comboBoxTriggerMode.Name = "comboBoxTriggerMode";
            this.comboBoxTriggerMode.Size = new System.Drawing.Size(216, 29);
            this.comboBoxTriggerMode.TabIndex = 215;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 259);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(239, 25);
            this.label4.TabIndex = 216;
            this.label4.Text = "Trigger Mode";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CaptureParameterSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Name = "CaptureParameterSetting";
            this.Size = new System.Drawing.Size(502, 358);
            this.Load += new System.EventHandler(this.CaptureParameterSetting_Load);
            this.panelBottom.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.gbCapSet.ResumeLayout(false);
            this.gbCapSet.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GraphicButton Button1;
        private GraphicButton btn_Close1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox gbCapSet;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label5;
        private MaskedTextBox textIntegrationTime;
        private MaskedTextBox textBoxFrameCount;
        private MaskedTextBox textBoxFrameDelay;
        private System.Windows.Forms.ComboBox comboBoxBinningMode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxGainMode;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxTriggerMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxPanelInterface;
        private System.Windows.Forms.Label label2;
    }
}