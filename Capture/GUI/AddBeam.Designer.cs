using AMRT;
namespace ImageCapturing
{
    partial class AddBeam
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtAquireGantry = new MaskedTextBox();
            this.txtBeamName = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.txtnote = new System.Windows.Forms.TextBox();
            this.panelBottom.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbtOK
            // 
            this.gbtOK.Location = new System.Drawing.Point(290, 3);
            // 
            // gbtCancel
            // 
            this.gbtCancel.Location = new System.Drawing.Point(336, 3);
            // 
            // panelBottom
            // 
            this.panelBottom.Location = new System.Drawing.Point(0, 208);
            this.panelBottom.Size = new System.Drawing.Size(380, 42);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(380, 2);
            // 
            // panelProgress
            // 
            this.panelProgress.Size = new System.Drawing.Size(256, 40);
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.groupBox1);
            this.panelCenter.Size = new System.Drawing.Size(380, 208);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(380, 250);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAquireGantry);
            this.groupBox1.Controls.Add(this.txtBeamName);
            this.groupBox1.Controls.Add(this.label49);
            this.groupBox1.Controls.Add(this.label41);
            this.groupBox1.Controls.Add(this.txtnote);
            this.groupBox1.Location = new System.Drawing.Point(13, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 191);
            this.groupBox1.TabIndex = 170;
            this.groupBox1.TabStop = false;
            // 
            // txtAquireGantry
            // 
            this.txtAquireGantry.DigitMaxValue = 360F;
            this.txtAquireGantry.DigitMinValue = -360F;
            this.txtAquireGantry.Location = new System.Drawing.Point(141, 60);
            this.txtAquireGantry.Masked = Mask.Digit;
            this.txtAquireGantry.Name = "txtAquireGantry";
            this.txtAquireGantry.Size = new System.Drawing.Size(195, 28);
            this.txtAquireGantry.TabIndex = 1;
            // 
            // txtBeamName
            // 
            this.txtBeamName.Location = new System.Drawing.Point(141, 22);
            this.txtBeamName.Multiline = true;
            this.txtBeamName.Name = "txtBeamName";
            this.txtBeamName.Size = new System.Drawing.Size(195, 28);
            this.txtBeamName.TabIndex = 176;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Font = new System.Drawing.Font("Tahoma", 13F);
            this.label49.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(86)))), ((int)(((byte)(213)))));
            this.label49.Location = new System.Drawing.Point(17, 25);
            this.label49.Name = "label49";
            this.label49.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label49.Size = new System.Drawing.Size(111, 22);
            this.label49.TabIndex = 175;
            this.label49.Text = "Beam Name:";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Tahoma", 13F);
            this.label41.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(86)))), ((int)(((byte)(213)))));
            this.label41.Location = new System.Drawing.Point(17, 63);
            this.label41.Name = "label41";
            this.label41.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label41.Size = new System.Drawing.Size(118, 22);
            this.label41.TabIndex = 172;
            this.label41.Text = "Gantry Angle:";
            // 
            // txtnote
            // 
            this.txtnote.Location = new System.Drawing.Point(21, 98);
            this.txtnote.Multiline = true;
            this.txtnote.Name = "txtnote";
            this.txtnote.Size = new System.Drawing.Size(315, 82);
            this.txtnote.TabIndex = 170;
            // 
            // AddBeam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "AddBeam";
            this.Size = new System.Drawing.Size(380, 250);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panelBottom, 0);
            this.Controls.SetChildIndex(this.panelCenter, 0);
            this.panelBottom.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtBeamName;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.TextBox txtnote;
        private MaskedTextBox txtAquireGantry;


    }
}
