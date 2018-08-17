namespace ImageCapturing
{
    /// <summary>
    /// 多网格小图控件
    /// </summary>
    partial class UserControlMutiImages
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
                this.SizeChanged -= new System.EventHandler(this.UserControlMutiImages_SizeChanged);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlMutiImages));
            this.ControlPanel = new System.Windows.Forms.Panel();
            this.panelImages = new System.Windows.Forms.Panel();
            this.btnPreviousLine1 = new System.Windows.Forms.Button();
            this.btnNextLine1 = new System.Windows.Forms.Button();
            this.btnPreviousLine2 = new System.Windows.Forms.Button();
            this.btnNextLine2 = new System.Windows.Forms.Button();
            this.pictureBoxImages = new System.Windows.Forms.PictureBox();
            this.ControlPanel.SuspendLayout();
            this.panelImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImages)).BeginInit();
            this.SuspendLayout();
            // 
            // ControlPanel
            // 
            this.ControlPanel.BackColor = System.Drawing.SystemColors.ControlText;
            this.ControlPanel.Controls.Add(this.btnPreviousLine1);
            this.ControlPanel.Controls.Add(this.btnNextLine1);
            this.ControlPanel.Controls.Add(this.btnPreviousLine2);
            this.ControlPanel.Controls.Add(this.btnNextLine2);
            this.ControlPanel.Controls.Add(this.panelImages);
            this.ControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ControlPanel.Location = new System.Drawing.Point(0, 0);
            this.ControlPanel.Name = "ControlPanel";
            this.ControlPanel.Size = new System.Drawing.Size(498, 131);
            this.ControlPanel.TabIndex = 0;
            // 
            // panelImages
            // 
            this.panelImages.Controls.Add(this.pictureBoxImages);
            this.panelImages.Location = new System.Drawing.Point(51, 0);
            this.panelImages.Name = "panelImages";
            this.panelImages.Size = new System.Drawing.Size(388, 131);
            this.panelImages.TabIndex = 3;
            // 
            // btnPreviousLine1
            // 
            this.btnPreviousLine1.BackColor = System.Drawing.Color.Transparent;
            this.btnPreviousLine1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPreviousLine1.BackgroundImage")));
            this.btnPreviousLine1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPreviousLine1.FlatAppearance.BorderSize = 0;
            this.btnPreviousLine1.Location = new System.Drawing.Point(0, 0);
            this.btnPreviousLine1.Name = "btnPreviousLine1";
            this.btnPreviousLine1.Size = new System.Drawing.Size(22, 18);
            this.btnPreviousLine1.TabIndex = 4;
            this.btnPreviousLine1.UseVisualStyleBackColor = false;
            this.btnPreviousLine1.Visible = false;
            this.btnPreviousLine1.Click += new System.EventHandler(this.btnPreviousLine_Click);
            this.btnPreviousLine1.Enter += new System.EventHandler(this.btnDirection_Enter);
            // 
            // btnNextLine1
            // 
            this.btnNextLine1.BackColor = System.Drawing.Color.Transparent;
            this.btnNextLine1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNextLine1.BackgroundImage")));
            this.btnNextLine1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextLine1.FlatAppearance.BorderSize = 0;
            this.btnNextLine1.Location = new System.Drawing.Point(0, 19);
            this.btnNextLine1.Name = "btnNextLine1";
            this.btnNextLine1.Size = new System.Drawing.Size(22, 18);
            this.btnNextLine1.TabIndex = 4;
            this.btnNextLine1.UseVisualStyleBackColor = false;
            this.btnNextLine1.Visible = false;
            this.btnNextLine1.Click += new System.EventHandler(this.btnNextLine_Click);
            this.btnNextLine1.Enter += new System.EventHandler(this.btnDirection_Enter);
            // 
            // btnPreviousLine2
            // 
            this.btnPreviousLine2.BackColor = System.Drawing.Color.Transparent;
            this.btnPreviousLine2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPreviousLine2.BackgroundImage")));
            this.btnPreviousLine2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPreviousLine2.FlatAppearance.BorderSize = 0;
            this.btnPreviousLine2.Location = new System.Drawing.Point(456, 73);
            this.btnPreviousLine2.Name = "btnPreviousLine2";
            this.btnPreviousLine2.Size = new System.Drawing.Size(22, 18);
            this.btnPreviousLine2.TabIndex = 4;
            this.btnPreviousLine2.UseVisualStyleBackColor = false;
            this.btnPreviousLine2.Visible = false;
            this.btnPreviousLine2.Click += new System.EventHandler(this.btnPreviousLine_Click);
            this.btnPreviousLine2.Enter += new System.EventHandler(this.btnDirection_Enter);
            // 
            // btnNextLine2
            // 
            this.btnNextLine2.BackColor = System.Drawing.Color.Transparent;
            this.btnNextLine2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNextLine2.BackgroundImage")));
            this.btnNextLine2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextLine2.FlatAppearance.BorderSize = 0;
            this.btnNextLine2.Location = new System.Drawing.Point(456, 92);
            this.btnNextLine2.Name = "btnNextLine2";
            this.btnNextLine2.Size = new System.Drawing.Size(22, 18);
            this.btnNextLine2.TabIndex = 4;
            this.btnNextLine2.UseVisualStyleBackColor = false;
            this.btnNextLine2.Visible = false;
            this.btnNextLine2.Click += new System.EventHandler(this.btnNextLine_Click);
            this.btnNextLine2.Enter += new System.EventHandler(this.btnDirection_Enter);
            // 
            // pictureBoxImages
            // 
            this.pictureBoxImages.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.pictureBoxImages.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxImages.Name = "pictureBoxImages";
            this.pictureBoxImages.Size = new System.Drawing.Size(364, 177);
            this.pictureBoxImages.TabIndex = 0;
            this.pictureBoxImages.TabStop = false;
            this.pictureBoxImages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImages_MouseMove);
            this.pictureBoxImages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxImages_MouseDown);
            // 
            // UserControlMutiImages
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.ControlPanel);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UserControlMutiImages";
            this.Size = new System.Drawing.Size(498, 131);
            this.Load += new System.EventHandler(this.UserControlMutiImages_Load);
            this.SizeChanged += new System.EventHandler(this.UserControlMutiImages_SizeChanged);
            this.ControlPanel.ResumeLayout(false);
            this.panelImages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel ControlPanel;
        private System.Windows.Forms.Panel panelImages;
        protected System.Windows.Forms.Button btnPreviousLine1;
        protected System.Windows.Forms.Button btnNextLine1;
        protected System.Windows.Forms.Button btnPreviousLine2;
        protected System.Windows.Forms.Button btnNextLine2;
        protected System.Windows.Forms.PictureBox pictureBoxImages;
    }
}
