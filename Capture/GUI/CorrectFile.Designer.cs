namespace ImageCapturing
{
    partial class CorrectFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CorrectFile));
            this.dirListBox = new Microsoft.VisualBasic.Compatibility.VB6.DirListBox();
            this.fileListBox = new Microsoft.VisualBasic.Compatibility.VB6.FileListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOffsetFileRemove = new AMRT.GraphicButton();
            this.btnOffsetFileAdd = new AMRT.GraphicButton();
            this.btnGainSeqFileRemove_Image = new AMRT.GraphicButton();
            this.btnGainSeqFileAdd_Image = new AMRT.GraphicButton();
            this.btnPixelMapFileRemove = new AMRT.GraphicButton();
            this.btnPixelMapFileAdd = new AMRT.GraphicButton();
            this.textBoxOffsetFile = new System.Windows.Forms.TextBox();
            this.textBoxGainSeqFile_Image = new System.Windows.Forms.TextBox();
            this.textBoxPixelMapFile = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.txtMachineDataFile = new System.Windows.Forms.TextBox();
            this.btnMachineDataRemove = new AMRT.GraphicButton();
            this.btnMachineDataAdd = new AMRT.GraphicButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnGainSeqFileAdd_Dose = new AMRT.GraphicButton();
            this.btnGainSeqFileRemove_Dose = new AMRT.GraphicButton();
            this.textBoxGainSeqFile_Dose = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnGainFileAdd = new AMRT.GraphicButton();
            this.btnGainFileRemove = new AMRT.GraphicButton();
            this.textBoxGainFile = new System.Windows.Forms.TextBox();
            this.panelBottom.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbtOK
            // 
            this.gbtOK.Location = new System.Drawing.Point(1023, 5);
            // 
            // gbtCancel
            // 
            this.gbtCancel.Location = new System.Drawing.Point(1069, 5);
            // 
            // panelBottom
            // 
            this.panelBottom.Location = new System.Drawing.Point(0, 609);
            this.panelBottom.Size = new System.Drawing.Size(1123, 42);
            // 
            // label1
            // 
            this.label1.Size = new System.Drawing.Size(1123, 2);
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.panel2);
            this.panelCenter.Controls.Add(this.panel1);
            this.panelCenter.Size = new System.Drawing.Size(1123, 609);
            // 
            // dirListBox
            // 
            this.dirListBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.dirListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dirListBox.Font = new System.Drawing.Font("Tahoma", 11F);
            this.dirListBox.FormattingEnabled = true;
            this.dirListBox.IntegralHeight = false;
            this.dirListBox.Location = new System.Drawing.Point(0, 0);
            this.dirListBox.Name = "dirListBox";
            this.dirListBox.Size = new System.Drawing.Size(531, 365);
            this.dirListBox.TabIndex = 4;
            this.dirListBox.SelectedIndexChanged += new System.EventHandler(this.dirListBox_SelectedIndexChanged);
            // 
            // fileListBox
            // 
            this.fileListBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fileListBox.Font = new System.Drawing.Font("Tahoma", 10F);
            this.fileListBox.FormattingEnabled = true;
            this.fileListBox.HorizontalScrollbar = true;
            this.fileListBox.Location = new System.Drawing.Point(0, 365);
            this.fileListBox.Name = "fileListBox";
            this.fileListBox.Pattern = "*.*";
            this.fileListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.fileListBox.Size = new System.Drawing.Size(531, 244);
            this.fileListBox.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dirListBox);
            this.panel1.Controls.Add(this.fileListBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(531, 609);
            this.panel1.TabIndex = 8;
            // 
            // btnOffsetFileRemove
            // 
            this.btnOffsetFileRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnOffsetFileRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOffsetFileRemove.BaseColor = System.Drawing.Color.Transparent;
            this.btnOffsetFileRemove.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnOffsetFileRemove.ButtonText = "";
            this.btnOffsetFileRemove.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOffsetFileRemove.ForeColor = System.Drawing.Color.Black;
            this.btnOffsetFileRemove.Hint = "Remove offset file";
            this.btnOffsetFileRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnOffsetFileRemove.Image")));
            this.btnOffsetFileRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOffsetFileRemove.ImageSize = new System.Drawing.Size(25, 22);
            this.btnOffsetFileRemove.Location = new System.Drawing.Point(16, 72);
            this.btnOffsetFileRemove.Name = "btnOffsetFileRemove";
            this.btnOffsetFileRemove.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnOffsetFileRemove.Size = new System.Drawing.Size(44, 36);
            this.btnOffsetFileRemove.TabIndex = 48;
            this.btnOffsetFileRemove.Tag = "203";
            this.btnOffsetFileRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOffsetFileRemove.Click += new System.EventHandler(this.btnOffsetFileRemove_Click);
            // 
            // btnOffsetFileAdd
            // 
            this.btnOffsetFileAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnOffsetFileAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOffsetFileAdd.BaseColor = System.Drawing.Color.Transparent;
            this.btnOffsetFileAdd.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnOffsetFileAdd.ButtonText = "";
            this.btnOffsetFileAdd.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOffsetFileAdd.ForeColor = System.Drawing.Color.Black;
            this.btnOffsetFileAdd.Hint = "Add offset file";
            this.btnOffsetFileAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnOffsetFileAdd.Image")));
            this.btnOffsetFileAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnOffsetFileAdd.ImageSize = new System.Drawing.Size(25, 22);
            this.btnOffsetFileAdd.Location = new System.Drawing.Point(16, 30);
            this.btnOffsetFileAdd.Name = "btnOffsetFileAdd";
            this.btnOffsetFileAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnOffsetFileAdd.Size = new System.Drawing.Size(44, 36);
            this.btnOffsetFileAdd.TabIndex = 47;
            this.btnOffsetFileAdd.Tag = "203";
            this.btnOffsetFileAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOffsetFileAdd.Click += new System.EventHandler(this.btnOffsetFileAdd_Click);
            // 
            // btnGainSeqFileRemove_Image
            // 
            this.btnGainSeqFileRemove_Image.BackColor = System.Drawing.Color.Transparent;
            this.btnGainSeqFileRemove_Image.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGainSeqFileRemove_Image.BaseColor = System.Drawing.Color.Transparent;
            this.btnGainSeqFileRemove_Image.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnGainSeqFileRemove_Image.ButtonText = "";
            this.btnGainSeqFileRemove_Image.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGainSeqFileRemove_Image.ForeColor = System.Drawing.Color.Black;
            this.btnGainSeqFileRemove_Image.Hint = "Remove gain seq file(image)";
            this.btnGainSeqFileRemove_Image.Image = ((System.Drawing.Image)(resources.GetObject("btnGainSeqFileRemove_Image.Image")));
            this.btnGainSeqFileRemove_Image.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnGainSeqFileRemove_Image.ImageSize = new System.Drawing.Size(25, 22);
            this.btnGainSeqFileRemove_Image.Location = new System.Drawing.Point(16, 71);
            this.btnGainSeqFileRemove_Image.Name = "btnGainSeqFileRemove_Image";
            this.btnGainSeqFileRemove_Image.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGainSeqFileRemove_Image.Size = new System.Drawing.Size(44, 36);
            this.btnGainSeqFileRemove_Image.TabIndex = 50;
            this.btnGainSeqFileRemove_Image.Tag = "203";
            this.btnGainSeqFileRemove_Image.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGainSeqFileRemove_Image.Click += new System.EventHandler(this.btnGainSeqFileRemove_Image_Click);
            // 
            // btnGainSeqFileAdd_Image
            // 
            this.btnGainSeqFileAdd_Image.BackColor = System.Drawing.Color.Transparent;
            this.btnGainSeqFileAdd_Image.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGainSeqFileAdd_Image.BaseColor = System.Drawing.Color.Transparent;
            this.btnGainSeqFileAdd_Image.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnGainSeqFileAdd_Image.ButtonText = "";
            this.btnGainSeqFileAdd_Image.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGainSeqFileAdd_Image.ForeColor = System.Drawing.Color.Black;
            this.btnGainSeqFileAdd_Image.Hint = "Add gain seq file(image)";
            this.btnGainSeqFileAdd_Image.Image = ((System.Drawing.Image)(resources.GetObject("btnGainSeqFileAdd_Image.Image")));
            this.btnGainSeqFileAdd_Image.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnGainSeqFileAdd_Image.ImageSize = new System.Drawing.Size(25, 22);
            this.btnGainSeqFileAdd_Image.Location = new System.Drawing.Point(16, 29);
            this.btnGainSeqFileAdd_Image.Name = "btnGainSeqFileAdd_Image";
            this.btnGainSeqFileAdd_Image.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGainSeqFileAdd_Image.Size = new System.Drawing.Size(44, 36);
            this.btnGainSeqFileAdd_Image.TabIndex = 49;
            this.btnGainSeqFileAdd_Image.Tag = "203";
            this.btnGainSeqFileAdd_Image.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGainSeqFileAdd_Image.Click += new System.EventHandler(this.btnGainSeqFileAdd_Image_Click);
            // 
            // btnPixelMapFileRemove
            // 
            this.btnPixelMapFileRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnPixelMapFileRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPixelMapFileRemove.BaseColor = System.Drawing.Color.Transparent;
            this.btnPixelMapFileRemove.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnPixelMapFileRemove.ButtonText = "";
            this.btnPixelMapFileRemove.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPixelMapFileRemove.ForeColor = System.Drawing.Color.Black;
            this.btnPixelMapFileRemove.Hint = "Remove pixel map file";
            this.btnPixelMapFileRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnPixelMapFileRemove.Image")));
            this.btnPixelMapFileRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPixelMapFileRemove.ImageSize = new System.Drawing.Size(25, 22);
            this.btnPixelMapFileRemove.Location = new System.Drawing.Point(16, 71);
            this.btnPixelMapFileRemove.Name = "btnPixelMapFileRemove";
            this.btnPixelMapFileRemove.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPixelMapFileRemove.Size = new System.Drawing.Size(44, 36);
            this.btnPixelMapFileRemove.TabIndex = 52;
            this.btnPixelMapFileRemove.Tag = "203";
            this.btnPixelMapFileRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPixelMapFileRemove.Click += new System.EventHandler(this.btnPixelMapFileRemove_Click);
            // 
            // btnPixelMapFileAdd
            // 
            this.btnPixelMapFileAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnPixelMapFileAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPixelMapFileAdd.BaseColor = System.Drawing.Color.Transparent;
            this.btnPixelMapFileAdd.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnPixelMapFileAdd.ButtonText = "";
            this.btnPixelMapFileAdd.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPixelMapFileAdd.ForeColor = System.Drawing.Color.Black;
            this.btnPixelMapFileAdd.Hint = "Add pixel map file";
            this.btnPixelMapFileAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnPixelMapFileAdd.Image")));
            this.btnPixelMapFileAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnPixelMapFileAdd.ImageSize = new System.Drawing.Size(25, 22);
            this.btnPixelMapFileAdd.Location = new System.Drawing.Point(16, 29);
            this.btnPixelMapFileAdd.Name = "btnPixelMapFileAdd";
            this.btnPixelMapFileAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPixelMapFileAdd.Size = new System.Drawing.Size(44, 36);
            this.btnPixelMapFileAdd.TabIndex = 51;
            this.btnPixelMapFileAdd.Tag = "203";
            this.btnPixelMapFileAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPixelMapFileAdd.Click += new System.EventHandler(this.btnPixelMapFileAdd_Click);
            // 
            // textBoxOffsetFile
            // 
            this.textBoxOffsetFile.Location = new System.Drawing.Point(72, 53);
            this.textBoxOffsetFile.Multiline = true;
            this.textBoxOffsetFile.Name = "textBoxOffsetFile";
            this.textBoxOffsetFile.ReadOnly = true;
            this.textBoxOffsetFile.Size = new System.Drawing.Size(506, 33);
            this.textBoxOffsetFile.TabIndex = 53;
            // 
            // textBoxGainSeqFile_Image
            // 
            this.textBoxGainSeqFile_Image.Location = new System.Drawing.Point(72, 52);
            this.textBoxGainSeqFile_Image.Multiline = true;
            this.textBoxGainSeqFile_Image.Name = "textBoxGainSeqFile_Image";
            this.textBoxGainSeqFile_Image.ReadOnly = true;
            this.textBoxGainSeqFile_Image.Size = new System.Drawing.Size(506, 33);
            this.textBoxGainSeqFile_Image.TabIndex = 54;
            // 
            // textBoxPixelMapFile
            // 
            this.textBoxPixelMapFile.Location = new System.Drawing.Point(72, 54);
            this.textBoxPixelMapFile.Multiline = true;
            this.textBoxPixelMapFile.Name = "textBoxPixelMapFile";
            this.textBoxPixelMapFile.ReadOnly = true;
            this.textBoxPixelMapFile.Size = new System.Drawing.Size(506, 33);
            this.textBoxPixelMapFile.TabIndex = 55;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOffsetFileAdd);
            this.groupBox1.Controls.Add(this.btnOffsetFileRemove);
            this.groupBox1.Controls.Add(this.textBoxOffsetFile);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(592, 122);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Offset File(image && dose)";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Controls.Add(this.groupBox6);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(531, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(592, 609);
            this.panel2.TabIndex = 57;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.txtMachineDataFile);
            this.groupBox5.Controls.Add(this.btnMachineDataRemove);
            this.groupBox5.Controls.Add(this.btnMachineDataAdd);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(0, 488);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(592, 118);
            this.groupBox5.TabIndex = 59;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Machine Data File(dose)";
            // 
            // txtMachineDataFile
            // 
            this.txtMachineDataFile.Location = new System.Drawing.Point(74, 52);
            this.txtMachineDataFile.Multiline = true;
            this.txtMachineDataFile.Name = "txtMachineDataFile";
            this.txtMachineDataFile.ReadOnly = true;
            this.txtMachineDataFile.Size = new System.Drawing.Size(506, 33);
            this.txtMachineDataFile.TabIndex = 2;
            // 
            // btnMachineDataRemove
            // 
            this.btnMachineDataRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnMachineDataRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMachineDataRemove.BaseColor = System.Drawing.Color.Transparent;
            this.btnMachineDataRemove.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnMachineDataRemove.ButtonText = "";
            this.btnMachineDataRemove.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMachineDataRemove.ForeColor = System.Drawing.Color.Black;
            this.btnMachineDataRemove.Hint = "Remove machine data file(dose)";
            this.btnMachineDataRemove.Image = ((System.Drawing.Image)(resources.GetObject("btnMachineDataRemove.Image")));
            this.btnMachineDataRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnMachineDataRemove.ImageSize = new System.Drawing.Size(25, 22);
            this.btnMachineDataRemove.Location = new System.Drawing.Point(16, 76);
            this.btnMachineDataRemove.Name = "btnMachineDataRemove";
            this.btnMachineDataRemove.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnMachineDataRemove.Size = new System.Drawing.Size(44, 36);
            this.btnMachineDataRemove.TabIndex = 1;
            this.btnMachineDataRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMachineDataRemove.Click += new System.EventHandler(this.btnMachineDataRemove_Click);
            // 
            // btnMachineDataAdd
            // 
            this.btnMachineDataAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnMachineDataAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMachineDataAdd.BaseColor = System.Drawing.Color.Transparent;
            this.btnMachineDataAdd.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnMachineDataAdd.ButtonText = "";
            this.btnMachineDataAdd.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMachineDataAdd.ForeColor = System.Drawing.Color.Black;
            this.btnMachineDataAdd.Hint = "Add machine data file(dose)";
            this.btnMachineDataAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnMachineDataAdd.Image")));
            this.btnMachineDataAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnMachineDataAdd.ImageSize = new System.Drawing.Size(25, 22);
            this.btnMachineDataAdd.Location = new System.Drawing.Point(16, 29);
            this.btnMachineDataAdd.Name = "btnMachineDataAdd";
            this.btnMachineDataAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnMachineDataAdd.Size = new System.Drawing.Size(44, 36);
            this.btnMachineDataAdd.TabIndex = 0;
            this.btnMachineDataAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMachineDataAdd.Click += new System.EventHandler(this.btnMachineDataAdd_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnGainSeqFileAdd_Dose);
            this.groupBox6.Controls.Add(this.btnGainSeqFileRemove_Dose);
            this.groupBox6.Controls.Add(this.textBoxGainSeqFile_Dose);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox6.Location = new System.Drawing.Point(0, 366);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(592, 122);
            this.groupBox6.TabIndex = 60;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Gain Seq File(dose)";
            // 
            // btnGainSeqFileAdd_Dose
            // 
            this.btnGainSeqFileAdd_Dose.BackColor = System.Drawing.Color.Transparent;
            this.btnGainSeqFileAdd_Dose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGainSeqFileAdd_Dose.BaseColor = System.Drawing.Color.Transparent;
            this.btnGainSeqFileAdd_Dose.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnGainSeqFileAdd_Dose.ButtonText = "";
            this.btnGainSeqFileAdd_Dose.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGainSeqFileAdd_Dose.ForeColor = System.Drawing.Color.Black;
            this.btnGainSeqFileAdd_Dose.Hint = "Add gain seq file(dose)";
            this.btnGainSeqFileAdd_Dose.Image = ((System.Drawing.Image)(resources.GetObject("btnGainSeqFileAdd_Dose.Image")));
            this.btnGainSeqFileAdd_Dose.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnGainSeqFileAdd_Dose.ImageSize = new System.Drawing.Size(25, 22);
            this.btnGainSeqFileAdd_Dose.Location = new System.Drawing.Point(16, 29);
            this.btnGainSeqFileAdd_Dose.Name = "btnGainSeqFileAdd_Dose";
            this.btnGainSeqFileAdd_Dose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGainSeqFileAdd_Dose.Size = new System.Drawing.Size(44, 36);
            this.btnGainSeqFileAdd_Dose.TabIndex = 49;
            this.btnGainSeqFileAdd_Dose.Tag = "203";
            this.btnGainSeqFileAdd_Dose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGainSeqFileAdd_Dose.Click += new System.EventHandler(this.btnGainSeqFileAdd_Dose_Click);
            // 
            // btnGainSeqFileRemove_Dose
            // 
            this.btnGainSeqFileRemove_Dose.BackColor = System.Drawing.Color.Transparent;
            this.btnGainSeqFileRemove_Dose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGainSeqFileRemove_Dose.BaseColor = System.Drawing.Color.Transparent;
            this.btnGainSeqFileRemove_Dose.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnGainSeqFileRemove_Dose.ButtonText = "";
            this.btnGainSeqFileRemove_Dose.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGainSeqFileRemove_Dose.ForeColor = System.Drawing.Color.Black;
            this.btnGainSeqFileRemove_Dose.Hint = "Remove gain seq file(dose)";
            this.btnGainSeqFileRemove_Dose.Image = ((System.Drawing.Image)(resources.GetObject("btnGainSeqFileRemove_Dose.Image")));
            this.btnGainSeqFileRemove_Dose.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnGainSeqFileRemove_Dose.ImageSize = new System.Drawing.Size(25, 22);
            this.btnGainSeqFileRemove_Dose.Location = new System.Drawing.Point(16, 71);
            this.btnGainSeqFileRemove_Dose.Name = "btnGainSeqFileRemove_Dose";
            this.btnGainSeqFileRemove_Dose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGainSeqFileRemove_Dose.Size = new System.Drawing.Size(44, 36);
            this.btnGainSeqFileRemove_Dose.TabIndex = 50;
            this.btnGainSeqFileRemove_Dose.Tag = "203";
            this.btnGainSeqFileRemove_Dose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGainSeqFileRemove_Dose.Click += new System.EventHandler(this.btnGainSeqFileRemove_Dose_Click);
            // 
            // textBoxGainSeqFile_Dose
            // 
            this.textBoxGainSeqFile_Dose.Location = new System.Drawing.Point(72, 52);
            this.textBoxGainSeqFile_Dose.Multiline = true;
            this.textBoxGainSeqFile_Dose.Name = "textBoxGainSeqFile_Dose";
            this.textBoxGainSeqFile_Dose.ReadOnly = true;
            this.textBoxGainSeqFile_Dose.Size = new System.Drawing.Size(506, 33);
            this.textBoxGainSeqFile_Dose.TabIndex = 54;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnGainSeqFileAdd_Image);
            this.groupBox2.Controls.Add(this.btnGainSeqFileRemove_Image);
            this.groupBox2.Controls.Add(this.textBoxGainSeqFile_Image);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 244);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(592, 122);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Gain Seq File(image)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnPixelMapFileAdd);
            this.groupBox3.Controls.Add(this.btnPixelMapFileRemove);
            this.groupBox3.Controls.Add(this.textBoxPixelMapFile);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 122);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(592, 122);
            this.groupBox3.TabIndex = 58;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pixel Map File(image && dose)";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnGainFileAdd);
            this.groupBox4.Controls.Add(this.btnGainFileRemove);
            this.groupBox4.Controls.Add(this.textBoxGainFile);
            this.groupBox4.Location = new System.Drawing.Point(0, 122);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(346, 122);
            this.groupBox4.TabIndex = 58;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Gain File";
            this.groupBox4.Visible = false;
            // 
            // btnGainFileAdd
            // 
            this.btnGainFileAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnGainFileAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGainFileAdd.BaseColor = System.Drawing.Color.Transparent;
            this.btnGainFileAdd.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnGainFileAdd.ButtonText = "";
            this.btnGainFileAdd.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGainFileAdd.ForeColor = System.Drawing.Color.Black;
            this.btnGainFileAdd.Hint = "Add gain seq file";
            this.btnGainFileAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnGainFileAdd.ImageSize = new System.Drawing.Size(25, 22);
            this.btnGainFileAdd.Location = new System.Drawing.Point(21, 29);
            this.btnGainFileAdd.Name = "btnGainFileAdd";
            this.btnGainFileAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGainFileAdd.Size = new System.Drawing.Size(44, 36);
            this.btnGainFileAdd.TabIndex = 49;
            this.btnGainFileAdd.Tag = "203";
            this.btnGainFileAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGainFileAdd.Click += new System.EventHandler(this.btnGainFileAdd_Click);
            // 
            // btnGainFileRemove
            // 
            this.btnGainFileRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnGainFileRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGainFileRemove.BaseColor = System.Drawing.Color.Transparent;
            this.btnGainFileRemove.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnGainFileRemove.ButtonText = "";
            this.btnGainFileRemove.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGainFileRemove.ForeColor = System.Drawing.Color.Black;
            this.btnGainFileRemove.Hint = "Remove gain seq file";
            this.btnGainFileRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnGainFileRemove.ImageSize = new System.Drawing.Size(25, 22);
            this.btnGainFileRemove.Location = new System.Drawing.Point(21, 71);
            this.btnGainFileRemove.Name = "btnGainFileRemove";
            this.btnGainFileRemove.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGainFileRemove.Size = new System.Drawing.Size(44, 36);
            this.btnGainFileRemove.TabIndex = 50;
            this.btnGainFileRemove.Tag = "203";
            this.btnGainFileRemove.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGainFileRemove.Click += new System.EventHandler(this.btnGainFileRemove_Click);
            // 
            // textBoxGainFile
            // 
            this.textBoxGainFile.Location = new System.Drawing.Point(71, 52);
            this.textBoxGainFile.Multiline = true;
            this.textBoxGainFile.Name = "textBoxGainFile";
            this.textBoxGainFile.ReadOnly = true;
            this.textBoxGainFile.Size = new System.Drawing.Size(241, 33);
            this.textBoxGainFile.TabIndex = 54;
            // 
            // CorrectFile
            // 
            this.Name = "CorrectFile";
            this.Size = new System.Drawing.Size(1123, 651);
            this.Load += new System.EventHandler(this.CorrectFile_Load);
            this.panelBottom.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.VisualBasic.Compatibility.VB6.DirListBox dirListBox;
        private System.Windows.Forms.Panel panel1;
        private Microsoft.VisualBasic.Compatibility.VB6.FileListBox fileListBox;
        private GraphicButton btnPixelMapFileRemove;
        private GraphicButton btnPixelMapFileAdd;
        private GraphicButton btnGainSeqFileRemove_Image;
        private GraphicButton btnGainSeqFileAdd_Image;
        private GraphicButton btnOffsetFileRemove;
        private GraphicButton btnOffsetFileAdd;
        private System.Windows.Forms.TextBox textBoxPixelMapFile;
        private System.Windows.Forms.TextBox textBoxGainSeqFile_Image;
        private System.Windows.Forms.TextBox textBoxOffsetFile;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private GraphicButton btnGainFileAdd;
        private GraphicButton btnGainFileRemove;
        private System.Windows.Forms.TextBox textBoxGainFile;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox txtMachineDataFile;
        private GraphicButton btnMachineDataRemove;
        private GraphicButton btnMachineDataAdd;
        private System.Windows.Forms.GroupBox groupBox6;
        private GraphicButton btnGainSeqFileAdd_Dose;
        private GraphicButton btnGainSeqFileRemove_Dose;
        private System.Windows.Forms.TextBox textBoxGainSeqFile_Dose;
    }
}
