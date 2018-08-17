using AMRT;
namespace ImageCapturing
{
    partial class FormGainSeqCapture
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
                DisposeVariables();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGainSeqCapture));
            this.contextMenuStripRectSet = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemFive = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTwenty = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxGainMode_Image = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbDoseRate_Image = new AMRT.MaskedTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbLinacMU_Image = new AMRT.MaskedTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCreatePixelMap = new AMRT.GraphicButton();
            this.btnBuildAverage_Image = new AMRT.GraphicButton();
            this.btnLocalImage_Image = new AMRT.GraphicButton();
            this.btnBuildGainSeq_Image = new AMRT.GraphicButton();
            this.btnCapture_Image = new AMRT.GraphicButton();
            this.panelAquire = new System.Windows.Forms.Panel();
            this.panelGainSeqImage = new System.Windows.Forms.Panel();
            this.userControlMutiDicomImages = new ImageCapturing.MultipleSelectedSmallImages();
            this.TextToolTipShow = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelProgress = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelWL = new System.Windows.Forms.Label();
            this.labelMedianValue = new System.Windows.Forms.Label();
            this.labelImageValue = new System.Windows.Forms.Label();
            this.labelAxis = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelTitle = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelMain = new System.Windows.Forms.Panel();
            this.labelsplit = new System.Windows.Forms.Label();
            this.contextMenuStripRectSet.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelAquire.SuspendLayout();
            this.panelGainSeqImage.SuspendLayout();
            this.panelProgress.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripRectSet
            // 
            this.contextMenuStripRectSet.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAll,
            this.toolStripMenuItemDefault,
            this.toolStripMenuItemFive,
            this.toolStripMenuItemTen,
            this.toolStripMenuItemTwenty});
            this.contextMenuStripRectSet.Name = "contextMenuStrip1";
            this.contextMenuStripRectSet.Size = new System.Drawing.Size(153, 114);
            // 
            // toolStripMenuItemAll
            // 
            this.toolStripMenuItemAll.Name = "toolStripMenuItemAll";
            this.toolStripMenuItemAll.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemAll.Text = "All";
            // 
            // toolStripMenuItemDefault
            // 
            this.toolStripMenuItemDefault.Name = "toolStripMenuItemDefault";
            this.toolStripMenuItemDefault.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemDefault.Text = "Default";
            // 
            // toolStripMenuItemFive
            // 
            this.toolStripMenuItemFive.Name = "toolStripMenuItemFive";
            this.toolStripMenuItemFive.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemFive.Text = "5cm x 5cm";
            // 
            // toolStripMenuItemTen
            // 
            this.toolStripMenuItemTen.Name = "toolStripMenuItemTen";
            this.toolStripMenuItemTen.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemTen.Text = "10cm x 10cm";
            // 
            // toolStripMenuItemTwenty
            // 
            this.toolStripMenuItemTwenty.Name = "toolStripMenuItemTwenty";
            this.toolStripMenuItemTwenty.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItemTwenty.Text = "20cm x 20cm";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxGainMode_Image);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbDoseRate_Image);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbLinacMU_Image);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 120);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 557);
            this.groupBox1.TabIndex = 206;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Capture Parameter ";
            // 
            // comboBoxGainMode_Image
            // 
            this.comboBoxGainMode_Image.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGainMode_Image.ForeColor = System.Drawing.Color.Blue;
            this.comboBoxGainMode_Image.FormattingEnabled = true;
            this.comboBoxGainMode_Image.Items.AddRange(new object[] {
            "0.25pF(200μm only)",
            "0.5 pF",
            "1 pF",
            "2 pF",
            "4 pF",
            "8 pF"});
            this.comboBoxGainMode_Image.Location = new System.Drawing.Point(198, 113);
            this.comboBoxGainMode_Image.Name = "comboBoxGainMode_Image";
            this.comboBoxGainMode_Image.Size = new System.Drawing.Size(150, 25);
            this.comboBoxGainMode_Image.TabIndex = 24;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(20, 112);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(130, 25);
            this.label14.TabIndex = 204;
            this.label14.Text = "Gain Mode";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(20, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(169, 22);
            this.label7.TabIndex = 200;
            this.label7.Text = "Dose Rate(cGy/Min)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbDoseRate_Image
            // 
            this.tbDoseRate_Image.DigitMaxValue = 65535F;
            this.tbDoseRate_Image.DigitMinValue = 0F;
            this.tbDoseRate_Image.ForeColor = System.Drawing.Color.Blue;
            this.tbDoseRate_Image.Location = new System.Drawing.Point(200, 35);
            this.tbDoseRate_Image.Masked = AMRT.Mask.Digit;
            this.tbDoseRate_Image.Name = "tbDoseRate_Image";
            this.tbDoseRate_Image.Size = new System.Drawing.Size(150, 24);
            this.tbDoseRate_Image.TabIndex = 21;
            this.tbDoseRate_Image.Text = "200";
            this.tbDoseRate_Image.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Input_KeyUp);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(20, 74);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 22);
            this.label10.TabIndex = 201;
            this.label10.Text = "LINAC MU";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbLinacMU_Image
            // 
            this.tbLinacMU_Image.DigitMaxValue = 65535F;
            this.tbLinacMU_Image.DigitMinValue = 0F;
            this.tbLinacMU_Image.ForeColor = System.Drawing.Color.Blue;
            this.tbLinacMU_Image.Location = new System.Drawing.Point(200, 74);
            this.tbLinacMU_Image.Masked = AMRT.Mask.Digit;
            this.tbLinacMU_Image.Name = "tbLinacMU_Image";
            this.tbLinacMU_Image.Size = new System.Drawing.Size(150, 24);
            this.tbLinacMU_Image.TabIndex = 22;
            this.tbLinacMU_Image.Text = "2";
            this.tbLinacMU_Image.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Input_KeyUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCreatePixelMap);
            this.panel2.Controls.Add(this.btnBuildAverage_Image);
            this.panel2.Controls.Add(this.btnLocalImage_Image);
            this.panel2.Controls.Add(this.btnBuildGainSeq_Image);
            this.panel2.Controls.Add(this.btnCapture_Image);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 677);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(360, 55);
            this.panel2.TabIndex = 2;
            // 
            // btnCreatePixelMap
            // 
            this.btnCreatePixelMap.BackColor = System.Drawing.Color.Transparent;
            this.btnCreatePixelMap.BaseColor = System.Drawing.Color.Transparent;
            this.btnCreatePixelMap.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnCreatePixelMap.ButtonText = null;
            this.btnCreatePixelMap.ForeColor = System.Drawing.Color.Black;
            this.btnCreatePixelMap.Hint = "Create and Save Pixel Map";
            this.btnCreatePixelMap.Image = ((System.Drawing.Image)(resources.GetObject("btnCreatePixelMap.Image")));
            this.btnCreatePixelMap.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCreatePixelMap.Location = new System.Drawing.Point(302, 9);
            this.btnCreatePixelMap.Name = "btnCreatePixelMap";
            this.btnCreatePixelMap.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCreatePixelMap.Size = new System.Drawing.Size(45, 40);
            this.btnCreatePixelMap.TabIndex = 31;
            this.btnCreatePixelMap.Click += new System.EventHandler(this.btnCreatePixelMap_Click);
            // 
            // btnBuildAverage_Image
            // 
            this.btnBuildAverage_Image.BackColor = System.Drawing.Color.Transparent;
            this.btnBuildAverage_Image.BaseColor = System.Drawing.Color.Transparent;
            this.btnBuildAverage_Image.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnBuildAverage_Image.ButtonText = null;
            this.btnBuildAverage_Image.ForeColor = System.Drawing.Color.Black;
            this.btnBuildAverage_Image.Hint = "Average and Save as Gain Sequence";
            this.btnBuildAverage_Image.Image = ((System.Drawing.Image)(resources.GetObject("btnBuildAverage_Image.Image")));
            this.btnBuildAverage_Image.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBuildAverage_Image.ImageSize = new System.Drawing.Size(33, 33);
            this.btnBuildAverage_Image.Location = new System.Drawing.Point(252, 9);
            this.btnBuildAverage_Image.Name = "btnBuildAverage_Image";
            this.btnBuildAverage_Image.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnBuildAverage_Image.Size = new System.Drawing.Size(45, 40);
            this.btnBuildAverage_Image.TabIndex = 30;
            this.btnBuildAverage_Image.Click += new System.EventHandler(this.btnAverageGain_Click);
            // 
            // btnLocalImage_Image
            // 
            this.btnLocalImage_Image.BackColor = System.Drawing.Color.Transparent;
            this.btnLocalImage_Image.BaseColor = System.Drawing.Color.Transparent;
            this.btnLocalImage_Image.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnLocalImage_Image.ButtonText = null;
            this.btnLocalImage_Image.ForeColor = System.Drawing.Color.Black;
            this.btnLocalImage_Image.Hint = "Get Images From Local Path";
            this.btnLocalImage_Image.Image = ((System.Drawing.Image)(resources.GetObject("btnLocalImage_Image.Image")));
            this.btnLocalImage_Image.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnLocalImage_Image.Location = new System.Drawing.Point(102, 9);
            this.btnLocalImage_Image.Name = "btnLocalImage_Image";
            this.btnLocalImage_Image.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnLocalImage_Image.Size = new System.Drawing.Size(45, 40);
            this.btnLocalImage_Image.TabIndex = 28;
            this.btnLocalImage_Image.Click += new System.EventHandler(this.btnLoadLocalFile_Click);
            // 
            // btnBuildGainSeq_Image
            // 
            this.btnBuildGainSeq_Image.BackColor = System.Drawing.Color.Transparent;
            this.btnBuildGainSeq_Image.BaseColor = System.Drawing.Color.Transparent;
            this.btnBuildGainSeq_Image.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnBuildGainSeq_Image.ButtonText = null;
            this.btnBuildGainSeq_Image.ForeColor = System.Drawing.Color.Black;
            this.btnBuildGainSeq_Image.Hint = "Bulid and Save Gain Sequence";
            this.btnBuildGainSeq_Image.Image = ((System.Drawing.Image)(resources.GetObject("btnBuildGainSeq_Image.Image")));
            this.btnBuildGainSeq_Image.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBuildGainSeq_Image.Location = new System.Drawing.Point(202, 9);
            this.btnBuildGainSeq_Image.Name = "btnBuildGainSeq_Image";
            this.btnBuildGainSeq_Image.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnBuildGainSeq_Image.Size = new System.Drawing.Size(45, 40);
            this.btnBuildGainSeq_Image.TabIndex = 27;
            this.btnBuildGainSeq_Image.Click += new System.EventHandler(this.btnBuildGainSeq_Click);
            // 
            // btnCapture_Image
            // 
            this.btnCapture_Image.BackColor = System.Drawing.Color.Transparent;
            this.btnCapture_Image.BaseColor = System.Drawing.Color.Transparent;
            this.btnCapture_Image.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnCapture_Image.ButtonText = null;
            this.btnCapture_Image.ForeColor = System.Drawing.Color.Black;
            this.btnCapture_Image.Hint = "Capture Image (Capture F6,Cancel F7)";
            this.btnCapture_Image.Image = ((System.Drawing.Image)(resources.GetObject("btnCapture_Image.Image")));
            this.btnCapture_Image.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnCapture_Image.Location = new System.Drawing.Point(152, 9);
            this.btnCapture_Image.Name = "btnCapture_Image";
            this.btnCapture_Image.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCapture_Image.Size = new System.Drawing.Size(45, 40);
            this.btnCapture_Image.TabIndex = 26;
            this.btnCapture_Image.Click += new System.EventHandler(this.gbCapture_Click);
            // 
            // panelAquire
            // 
            this.panelAquire.Controls.Add(this.panelGainSeqImage);
            this.panelAquire.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAquire.Location = new System.Drawing.Point(0, 0);
            this.panelAquire.Name = "panelAquire";
            this.panelAquire.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panelAquire.Size = new System.Drawing.Size(360, 732);
            this.panelAquire.TabIndex = 2;
            // 
            // panelGainSeqImage
            // 
            this.panelGainSeqImage.Controls.Add(this.groupBox1);
            this.panelGainSeqImage.Controls.Add(this.userControlMutiDicomImages);
            this.panelGainSeqImage.Controls.Add(this.panel2);
            this.panelGainSeqImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGainSeqImage.Location = new System.Drawing.Point(0, 0);
            this.panelGainSeqImage.Name = "panelGainSeqImage";
            this.panelGainSeqImage.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panelGainSeqImage.Size = new System.Drawing.Size(360, 732);
            this.panelGainSeqImage.TabIndex = 3;
            // 
            // userControlMutiDicomImages
            // 
            this.userControlMutiDicomImages.BackColor = System.Drawing.SystemColors.ControlText;
            this.userControlMutiDicomImages.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("userControlMutiDicomImages.BackgroundImage")));
            this.userControlMutiDicomImages.Column = 3;
            this.userControlMutiDicomImages.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlMutiDicomImages.Font = new System.Drawing.Font("Tahoma", 9F);
            this.userControlMutiDicomImages.ForeColor = System.Drawing.SystemColors.ControlText;
            this.userControlMutiDicomImages.ImageBackColor = System.Drawing.Color.Black;
            this.userControlMutiDicomImages.Location = new System.Drawing.Point(0, 0);
            this.userControlMutiDicomImages.Margin = new System.Windows.Forms.Padding(0);
            this.userControlMutiDicomImages.MouseWheelEnable = true;
            this.userControlMutiDicomImages.Name = "userControlMutiDicomImages";
            this.userControlMutiDicomImages.Row = 1;
            this.userControlMutiDicomImages.SelectedIndex = 0;
            this.userControlMutiDicomImages.SelectGridColor = System.Drawing.Color.Green;
            this.userControlMutiDicomImages.SelectPenWidth = 2;
            this.userControlMutiDicomImages.ShowBackColor = System.Drawing.SystemColors.ControlText;
            this.userControlMutiDicomImages.ShowImageIndex = true;
            this.userControlMutiDicomImages.SingleSelect = false;
            this.userControlMutiDicomImages.Size = new System.Drawing.Size(360, 120);
            this.userControlMutiDicomImages.TabIndex = 0;
            this.userControlMutiDicomImages.TabStop = false;
            // 
            // TextToolTipShow
            // 
            this.TextToolTipShow.Dock = System.Windows.Forms.DockStyle.Left;
            this.TextToolTipShow.Location = new System.Drawing.Point(0, 0);
            this.TextToolTipShow.Name = "TextToolTipShow";
            this.TextToolTipShow.Size = new System.Drawing.Size(454, 36);
            this.TextToolTipShow.TabIndex = 107;
            this.TextToolTipShow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Blue;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(281, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(2, 32);
            this.label3.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Blue;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(127, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(2, 32);
            this.label2.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Blue;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(405, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(2, 32);
            this.label1.TabIndex = 6;
            // 
            // panelProgress
            // 
            this.panelProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProgress.Controls.Add(this.panel1);
            this.panelProgress.Controls.Add(this.pictureBox1);
            this.panelProgress.Controls.Add(this.progressBar1);
            this.panelProgress.Controls.Add(this.labelTitle);
            this.panelProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProgress.Location = new System.Drawing.Point(0, 733);
            this.panelProgress.Name = "panelProgress";
            this.panelProgress.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.panelProgress.Size = new System.Drawing.Size(1024, 35);
            this.panelProgress.TabIndex = 114;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.labelWL);
            this.panel1.Controls.Add(this.labelMedianValue);
            this.panel1.Controls.Add(this.labelImageValue);
            this.panel1.Controls.Add(this.labelAxis);
            this.panel1.Location = new System.Drawing.Point(452, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(530, 34);
            this.panel1.TabIndex = 110;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Blue;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(199, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(2, 31);
            this.label5.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Blue;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(527, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(2, 31);
            this.label4.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Blue;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(316, 1);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(2, 31);
            this.label6.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Blue;
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(39, 1);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(2, 31);
            this.label8.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Blue;
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(418, 1);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(2, 31);
            this.label9.TabIndex = 6;
            // 
            // labelWL
            // 
            this.labelWL.ForeColor = System.Drawing.Color.Black;
            this.labelWL.Location = new System.Drawing.Point(44, 0);
            this.labelWL.Name = "labelWL";
            this.labelWL.Size = new System.Drawing.Size(153, 30);
            this.labelWL.TabIndex = 11;
            this.labelWL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMedianValue
            // 
            this.labelMedianValue.ForeColor = System.Drawing.Color.Black;
            this.labelMedianValue.Location = new System.Drawing.Point(422, 0);
            this.labelMedianValue.Name = "labelMedianValue";
            this.labelMedianValue.Size = new System.Drawing.Size(103, 30);
            this.labelMedianValue.TabIndex = 5;
            this.labelMedianValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelImageValue
            // 
            this.labelImageValue.ForeColor = System.Drawing.Color.Black;
            this.labelImageValue.Location = new System.Drawing.Point(316, 0);
            this.labelImageValue.Name = "labelImageValue";
            this.labelImageValue.Size = new System.Drawing.Size(101, 30);
            this.labelImageValue.TabIndex = 1;
            this.labelImageValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelAxis
            // 
            this.labelAxis.ForeColor = System.Drawing.Color.Black;
            this.labelAxis.Location = new System.Drawing.Point(202, 0);
            this.labelAxis.Name = "labelAxis";
            this.labelAxis.Size = new System.Drawing.Size(112, 30);
            this.labelAxis.TabIndex = 3;
            this.labelAxis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(986, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 27);
            this.pictureBox1.TabIndex = 106;
            this.pictureBox1.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(44, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(215, 30);
            this.progressBar1.TabIndex = 104;
            this.progressBar1.Visible = false;
            // 
            // labelTitle
            // 
            this.labelTitle.Location = new System.Drawing.Point(0, 2);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(44, 30);
            this.labelTitle.TabIndex = 105;
            this.labelTitle.Text = "Title";
            this.labelTitle.Visible = false;
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
            this.splitContainer1.Size = new System.Drawing.Size(1024, 732);
            this.splitContainer1.SplitterDistance = 360;
            this.splitContainer1.TabIndex = 115;
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(660, 732);
            this.panelMain.TabIndex = 0;
            // 
            // labelsplit
            // 
            this.labelsplit.BackColor = System.Drawing.Color.SkyBlue;
            this.labelsplit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelsplit.Location = new System.Drawing.Point(0, 732);
            this.labelsplit.Name = "labelsplit";
            this.labelsplit.Size = new System.Drawing.Size(1024, 1);
            this.labelsplit.TabIndex = 116;
            this.labelsplit.Text = "label1";
            // 
            // FormGainSeqCapture
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(232)))), ((int)(((byte)(207)))));
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.labelsplit);
            this.Controls.Add(this.panelProgress);
            this.Font = new System.Drawing.Font("Arial", 11F);
            this.KeyPreview = true;
            this.Name = "FormGainSeqCapture";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowInTaskbar = false;
            this.Text = "Gain Correction";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCapture_FormClosing);
            this.contextMenuStripRectSet.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panelAquire.ResumeLayout(false);
            this.panelGainSeqImage.ResumeLayout(false);
            this.panelProgress.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }



        #endregion

        private MultipleSelectedSmallImages userControlMutiDicomImages;
        private System.Windows.Forms.Panel panelAquire;
        private System.Windows.Forms.Panel panel2;
        private GraphicButton btnCapture_Image;
        private GraphicButton btnBuildGainSeq_Image;
        private System.Windows.Forms.Label TextToolTipShow;
        private System.Windows.Forms.Panel panelGainSeqImage;
        private System.Windows.Forms.Label label10;
        private MaskedTextBox tbLinacMU_Image;
        private System.Windows.Forms.GroupBox groupBox1;
        private GraphicButton btnLocalImage_Image;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private MaskedTextBox tbDoseRate_Image;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox comboBoxGainMode_Image;
        private GraphicButton btnBuildAverage_Image;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRectSet;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAll;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDefault;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFive;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTen;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTwenty;
        protected System.Windows.Forms.Panel panelProgress;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label labelsplit;
        private GraphicButton btnCreatePixelMap;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelWL;
        private System.Windows.Forms.Label labelMedianValue;
        private System.Windows.Forms.Label labelImageValue;
        private System.Windows.Forms.Label labelAxis;
    }
}

