using AMRT;
namespace ImageCapturing
{
    partial class LinarDetection
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelBottom.SuspendLayout();
            this.panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gbtOK
            // 
            this.gbtOK.Location = new System.Drawing.Point(396, 5);
            this.gbtOK.TabIndex = 7;
            this.gbtOK.Visible = false;
            // 
            // gbtCancel
            // 
            this.gbtCancel.Location = new System.Drawing.Point(446, 5);
            this.gbtCancel.TabIndex = 8;
            // 
            // panelBottom
            // 
            this.panelBottom.Location = new System.Drawing.Point(0, 578);
            this.panelBottom.Size = new System.Drawing.Size(750, 42);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(750, 2);
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.pictureBox1);
            this.panelCenter.Size = new System.Drawing.Size(750, 578);
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
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(750, 578);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // LinarDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.Name = "LinarDetection";
            this.Size = new System.Drawing.Size(750, 620);
            this.panelBottom.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GraphicButton Button1;
        private GraphicButton btn_Close1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}