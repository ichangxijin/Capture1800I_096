namespace AMRT
{
    partial class ParaSet
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
            this.btn_Close1 = new AMRT.GraphicButton();
            this.Button1 = new AMRT.GraphicButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxTimeOut = new System.Windows.Forms.ComboBox();
            this.comboBoxInterval = new System.Windows.Forms.ComboBox();
            this.combPort = new System.Windows.Forms.ComboBox();
            this.comboBoxOnValue = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelBottom.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbtOK
            // 
            this.gbtOK.Location = new System.Drawing.Point(290, 3);
            this.toolTipBase.SetToolTip(this.gbtOK, "Apply");
            this.gbtOK.Click += new System.EventHandler(this.gbtOK_Click_1);
            // 
            // gbtCancel
            // 
            this.gbtCancel.Location = new System.Drawing.Point(336, 3);
            this.toolTipBase.SetToolTip(this.gbtCancel, "Cancel");
            this.gbtCancel.Click += new System.EventHandler(this.gbtCancel_Click_1);
            // 
            // panelBottom
            // 
            this.panelBottom.Location = new System.Drawing.Point(0, 252);
            this.panelBottom.Size = new System.Drawing.Size(379, 42);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(379, 2);
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.groupBox1);
            this.panelCenter.Size = new System.Drawing.Size(379, 252);
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
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Location = new System.Drawing.Point(25, 307);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(324, 1);
            this.panel1.TabIndex = 158;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxTimeOut);
            this.groupBox1.Controls.Add(this.comboBoxInterval);
            this.groupBox1.Controls.Add(this.combPort);
            this.groupBox1.Controls.Add(this.comboBoxOnValue);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(20, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(341, 236);
            this.groupBox1.TabIndex = 156;
            this.groupBox1.TabStop = false;
            // 
            // comboBoxTimeOut
            // 
            this.comboBoxTimeOut.FormattingEnabled = true;
            this.comboBoxTimeOut.Location = new System.Drawing.Point(230, 190);
            this.comboBoxTimeOut.Name = "comboBoxTimeOut";
            this.comboBoxTimeOut.Size = new System.Drawing.Size(96, 29);
            this.comboBoxTimeOut.TabIndex = 19;
            // 
            // comboBoxInterval
            // 
            this.comboBoxInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInterval.FormattingEnabled = true;
            this.comboBoxInterval.Location = new System.Drawing.Point(230, 135);
            this.comboBoxInterval.Name = "comboBoxInterval";
            this.comboBoxInterval.Size = new System.Drawing.Size(96, 29);
            this.comboBoxInterval.TabIndex = 17;
            // 
            // combPort
            // 
            this.combPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combPort.FormattingEnabled = true;
            this.combPort.Location = new System.Drawing.Point(230, 25);
            this.combPort.Name = "combPort";
            this.combPort.Size = new System.Drawing.Size(96, 29);
            this.combPort.TabIndex = 15;
            // 
            // comboBoxOnValue
            // 
            this.comboBoxOnValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOnValue.FormattingEnabled = true;
            this.comboBoxOnValue.Location = new System.Drawing.Point(230, 80);
            this.comboBoxOnValue.Name = "comboBoxOnValue";
            this.comboBoxOnValue.Size = new System.Drawing.Size(96, 29);
            this.comboBoxOnValue.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(11, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 22);
            this.label3.TabIndex = 16;
            this.label3.Text = "Sampling interval (ms)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 22);
            this.label2.TabIndex = 12;
            this.label2.Text = "Port";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(11, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(210, 22);
            this.label4.TabIndex = 13;
            this.label4.Text = "Beam On Value";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(11, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(210, 22);
            this.label5.TabIndex = 18;
            this.label5.Text = "Break Delay Time(ms)";
            // 
            // ParaSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Name = "ParaSet";
            this.Size = new System.Drawing.Size(379, 294);
            this.panelBottom.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GraphicButton btn_Close1;
        private GraphicButton Button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxTimeOut;
        private System.Windows.Forms.ComboBox comboBoxInterval;
        private System.Windows.Forms.ComboBox combPort;
        private System.Windows.Forms.ComboBox comboBoxOnValue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}