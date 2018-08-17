namespace ImageCapturing
{
    partial class MultipleSelectedSmallImages
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultipleSelectedSmallImages));
            this.ControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImages)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPreviousLine1
            // 
            this.btnPreviousLine1.FlatAppearance.BorderSize = 0;
            this.btnPreviousLine1.Location = new System.Drawing.Point(8, 8);
            // 
            // btnNextLine1
            // 
            this.btnNextLine1.FlatAppearance.BorderSize = 0;
            this.btnNextLine1.Location = new System.Drawing.Point(8, 27);
            // 
            // btnPreviousLine2
            // 
            this.btnPreviousLine2.FlatAppearance.BorderSize = 0;
            this.btnPreviousLine2.Location = new System.Drawing.Point(468, 86);
            // 
            // btnNextLine2
            // 
            this.btnNextLine2.FlatAppearance.BorderSize = 0;
            this.btnNextLine2.Location = new System.Drawing.Point(468, 105);
            // 
            // pictureBoxImages
            // 
            this.pictureBoxImages.Size = new System.Drawing.Size(498, 131);
            // 
            // MultipleSelectedSmallImages
            // 
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Name = "MultipleSelectedSmallImages";
            this.SingleSelect = false;
            this.ControlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}
