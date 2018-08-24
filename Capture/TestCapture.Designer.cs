
namespace ImageCapturing
{
    partial class TestCapture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestCapture));
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonoffsetCal = new System.Windows.Forms.Button();
            this.buttongainCal = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.panelAquire = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.labelTriggerNumber = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelCaptureNumber = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.labelProgressInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelImageSize = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelLevelWindow = new System.Windows.Forms.Label();
            this.labelMean = new System.Windows.Forms.Label();
            this.labelImageValue = new System.Windows.Forms.Label();
            this.labelAxis = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelsplit = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelMain = new System.Windows.Forms.Panel();
            this.buttonCapturePara = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panelAquire.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelProgress.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.buttonCapturePara);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.buttonoffsetCal);
            this.panel2.Controls.Add(this.buttongainCal);
            this.panel2.Controls.Add(this.btnCapture);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 513);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(420, 74);
            this.panel2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Font = new System.Drawing.Font("Arial", 9F);
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(150, 8);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button1.Size = new System.Drawing.Size(60, 60);
            this.button1.TabIndex = 12;
            this.button1.Text = "Trigger";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonoffsetCal
            // 
            this.buttonoffsetCal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonoffsetCal.Font = new System.Drawing.Font("Arial", 9F);
            this.buttonoffsetCal.ForeColor = System.Drawing.Color.Black;
            this.buttonoffsetCal.Location = new System.Drawing.Point(8, 8);
            this.buttonoffsetCal.Name = "buttonoffsetCal";
            this.buttonoffsetCal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonoffsetCal.Size = new System.Drawing.Size(60, 60);
            this.buttonoffsetCal.TabIndex = 11;
            this.buttonoffsetCal.Text = "offsCal";
            this.buttonoffsetCal.Click += new System.EventHandler(this.buttonoffsetCal_Click);
            // 
            // buttongainCal
            // 
            this.buttongainCal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttongainCal.Font = new System.Drawing.Font("Arial", 9F);
            this.buttongainCal.ForeColor = System.Drawing.Color.Black;
            this.buttongainCal.Location = new System.Drawing.Point(74, 8);
            this.buttongainCal.Name = "buttongainCal";
            this.buttongainCal.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttongainCal.Size = new System.Drawing.Size(60, 60);
            this.buttongainCal.TabIndex = 10;
            this.buttongainCal.Text = "gainCal";
            this.buttongainCal.Click += new System.EventHandler(this.buttongainCal_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCapture.ForeColor = System.Drawing.Color.Black;
            this.btnCapture.Image = ((System.Drawing.Image)(resources.GetObject("btnCapture.Image")));
            this.btnCapture.Location = new System.Drawing.Point(345, 7);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCapture.Size = new System.Drawing.Size(60, 60);
            this.btnCapture.TabIndex = 2;
            this.btnCapture.Click += new System.EventHandler(this.gbCapture_Click);
            // 
            // panelAquire
            // 
            this.panelAquire.Controls.Add(this.panel3);
            this.panelAquire.Controls.Add(this.panel2);
            this.panelAquire.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAquire.Location = new System.Drawing.Point(0, 0);
            this.panelAquire.Name = "panelAquire";
            this.panelAquire.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panelAquire.Size = new System.Drawing.Size(420, 587);
            this.panelAquire.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.labelTriggerNumber);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.labelCaptureNumber);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(420, 513);
            this.panel3.TabIndex = 9;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(204, 438);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 24);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 445);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(153, 17);
            this.label11.TabIndex = 4;
            this.label11.Text = "Plan Capture Number:";
            // 
            // labelTriggerNumber
            // 
            this.labelTriggerNumber.AutoSize = true;
            this.labelTriggerNumber.Font = new System.Drawing.Font("Arial", 30F);
            this.labelTriggerNumber.ForeColor = System.Drawing.Color.Red;
            this.labelTriggerNumber.Location = new System.Drawing.Point(196, 146);
            this.labelTriggerNumber.Name = "labelTriggerNumber";
            this.labelTriggerNumber.Size = new System.Drawing.Size(42, 45);
            this.labelTriggerNumber.TabIndex = 3;
            this.labelTriggerNumber.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Trigger Number:";
            // 
            // labelCaptureNumber
            // 
            this.labelCaptureNumber.AutoSize = true;
            this.labelCaptureNumber.Font = new System.Drawing.Font("Arial", 30F);
            this.labelCaptureNumber.ForeColor = System.Drawing.Color.Green;
            this.labelCaptureNumber.Location = new System.Drawing.Point(196, 22);
            this.labelCaptureNumber.Name = "labelCaptureNumber";
            this.labelCaptureNumber.Size = new System.Drawing.Size(42, 45);
            this.labelCaptureNumber.TabIndex = 1;
            this.labelCaptureNumber.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Capture Image Number:";
            // 
            // panelProgress
            // 
            this.panelProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProgress.Controls.Add(this.labelProgressInfo);
            this.panelProgress.Controls.Add(this.panel1);
            this.panelProgress.Controls.Add(this.pictureBox1);
            this.panelProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProgress.Location = new System.Drawing.Point(0, 588);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.panelProgress.Size = new System.Drawing.Size(957, 35);
            this.panelProgress.TabIndex = 105;
            // 
            // labelProgressInfo
            // 
            this.labelProgressInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProgressInfo.Font = new System.Drawing.Font("Arial", 8F);
            this.labelProgressInfo.ForeColor = System.Drawing.Color.Black;
            this.labelProgressInfo.Location = new System.Drawing.Point(3, 2);
            this.labelProgressInfo.Name = "labelProgressInfo";
            this.labelProgressInfo.Size = new System.Drawing.Size(257, 30);
            this.labelProgressInfo.TabIndex = 112;
            this.labelProgressInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.labelImageSize);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.labelLevelWindow);
            this.panel1.Controls.Add(this.labelMean);
            this.panel1.Controls.Add(this.labelImageValue);
            this.panel1.Controls.Add(this.labelAxis);
            this.panel1.Location = new System.Drawing.Point(260, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(656, 34);
            this.panel1.TabIndex = 111;
            // 
            // labelImageSize
            // 
            this.labelImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelImageSize.ForeColor = System.Drawing.Color.Black;
            this.labelImageSize.Location = new System.Drawing.Point(6, 1);
            this.labelImageSize.Name = "labelImageSize";
            this.labelImageSize.Size = new System.Drawing.Size(157, 30);
            this.labelImageSize.TabIndex = 13;
            this.labelImageSize.Text = "Image Size:1408x1408";
            this.labelImageSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.BackColor = System.Drawing.Color.Blue;
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.Location = new System.Drawing.Point(2, 2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(2, 31);
            this.label13.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.Color.Blue;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(325, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(2, 31);
            this.label5.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.Blue;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(653, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(2, 31);
            this.label4.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BackColor = System.Drawing.Color.Blue;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(442, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(2, 31);
            this.label6.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.BackColor = System.Drawing.Color.Blue;
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(165, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(2, 31);
            this.label8.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BackColor = System.Drawing.Color.Blue;
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(544, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(2, 31);
            this.label9.TabIndex = 6;
            // 
            // labelLevelWindow
            // 
            this.labelLevelWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLevelWindow.ForeColor = System.Drawing.Color.Black;
            this.labelLevelWindow.Location = new System.Drawing.Point(170, 0);
            this.labelLevelWindow.Name = "labelLevelWindow";
            this.labelLevelWindow.Size = new System.Drawing.Size(153, 30);
            this.labelLevelWindow.TabIndex = 11;
            this.labelLevelWindow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMean
            // 
            this.labelMean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMean.ForeColor = System.Drawing.Color.Black;
            this.labelMean.Location = new System.Drawing.Point(548, 0);
            this.labelMean.Name = "labelMean";
            this.labelMean.Size = new System.Drawing.Size(103, 30);
            this.labelMean.TabIndex = 5;
            this.labelMean.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelImageValue
            // 
            this.labelImageValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelImageValue.ForeColor = System.Drawing.Color.Black;
            this.labelImageValue.Location = new System.Drawing.Point(442, 0);
            this.labelImageValue.Name = "labelImageValue";
            this.labelImageValue.Size = new System.Drawing.Size(101, 30);
            this.labelImageValue.TabIndex = 1;
            this.labelImageValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelAxis
            // 
            this.labelAxis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAxis.ForeColor = System.Drawing.Color.Black;
            this.labelAxis.Location = new System.Drawing.Point(328, 0);
            this.labelAxis.Name = "labelAxis";
            this.labelAxis.Size = new System.Drawing.Size(112, 30);
            this.labelAxis.TabIndex = 3;
            this.labelAxis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(919, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 27);
            this.pictureBox1.TabIndex = 106;
            this.pictureBox1.TabStop = false;
            // 
            // labelsplit
            // 
            this.labelsplit.BackColor = System.Drawing.Color.SkyBlue;
            this.labelsplit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelsplit.Location = new System.Drawing.Point(0, 587);
            this.labelsplit.Name = "labelsplit";
            this.labelsplit.Size = new System.Drawing.Size(957, 1);
            this.labelsplit.TabIndex = 106;
            this.labelsplit.Text = "label1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panelAquire);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelMain);
            this.splitContainer1.Size = new System.Drawing.Size(957, 587);
            this.splitContainer1.SplitterDistance = 420;
            this.splitContainer1.TabIndex = 13;
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(533, 587);
            this.panelMain.TabIndex = 0;
            // 
            // buttonCapturePara
            // 
            this.buttonCapturePara.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCapturePara.Font = new System.Drawing.Font("Arial", 9F);
            this.buttonCapturePara.ForeColor = System.Drawing.Color.Black;
            this.buttonCapturePara.Location = new System.Drawing.Point(216, 7);
            this.buttonCapturePara.Name = "buttonCapturePara";
            this.buttonCapturePara.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.buttonCapturePara.Size = new System.Drawing.Size(60, 60);
            this.buttonCapturePara.TabIndex = 13;
            this.buttonCapturePara.Text = "CapSet";
            this.buttonCapturePara.Click += new System.EventHandler(this.buttonCapturePara_Click);
            // 
            // TestCapture
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(232)))), ((int)(((byte)(207)))));
            this.ClientSize = new System.Drawing.Size(957, 623);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.labelsplit);
            this.Controls.Add(this.panelProgress);
            this.Font = new System.Drawing.Font("Arial", 11F);
            this.KeyPreview = true;
            this.Name = "TestCapture";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCapture_FormClosing);
            this.panel2.ResumeLayout(false);
            this.panelAquire.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelProgress.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }



        #endregion

        private System.Windows.Forms.Panel panelAquire;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCapture;
        protected System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label labelsplit;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelLevelWindow;
        private System.Windows.Forms.Label labelMean;
        private System.Windows.Forms.Label labelImageValue;
        private System.Windows.Forms.Label labelAxis;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonoffsetCal;
        private System.Windows.Forms.Button buttongainCal;
        private System.Windows.Forms.Label labelImageSize;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelProgressInfo;
        private System.Windows.Forms.Label labelCaptureNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTriggerNumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonCapturePara;
    }
}

