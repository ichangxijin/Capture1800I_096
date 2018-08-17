namespace ImageCapturing
{
    partial class UserControlCalibrationLength
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.tbLineLength = new MaskedTextBox();
            this.panelBottom.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbtOK
            // 
            this.gbtOK.Location = new System.Drawing.Point(577, 4);
            // 
            // gbtCancel
            // 
            this.gbtCancel.Location = new System.Drawing.Point(623, 4);
            // 
            // panelBottom
            // 
            this.panelBottom.Location = new System.Drawing.Point(0, 115);
            this.panelBottom.Size = new System.Drawing.Size(335, 42);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(335, 2);
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.label2);
            this.panelCenter.Controls.Add(this.tbLineLength);
            this.panelCenter.Size = new System.Drawing.Size(335, 115);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = "Input the real distance(mm)";
            // 
            // tbLineLength
            // 
            this.tbLineLength.DigitMaxValue = 3.402823E+38F;
            this.tbLineLength.DigitMinValue = 0F;
            this.tbLineLength.Location = new System.Drawing.Point(26, 56);
            this.tbLineLength.Masked = Mask.Decimal;
            this.tbLineLength.Name = "tbLineLength";
            this.tbLineLength.Size = new System.Drawing.Size(280, 28);
            this.tbLineLength.TabIndex = 2;
            // 
            // UserControlCalibrationLength
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "UserControlCalibrationLength";
            this.Size = new System.Drawing.Size(335, 157);
            this.panelBottom.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.panelCenter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private AMRT.MaskedTextBox tbLineLength;
    }
}
