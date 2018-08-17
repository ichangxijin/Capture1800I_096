using AMRT;
namespace ImageCapturing
{
    partial class ClaheProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClaheProcess));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnModify = new AMRT.GraphicButton();
            this.btnDeleteModel = new AMRT.GraphicButton();
            this.comboBoxClaheModel = new System.Windows.Forms.ComboBox();
            this.btnAddModel = new AMRT.GraphicButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.btnDeleteModel);
            this.panel1.Controls.Add(this.comboBoxClaheModel);
            this.panel1.Controls.Add(this.btnAddModel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 185);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(370, 44);
            this.panel1.TabIndex = 0;
            // 
            // btnModify
            // 
            this.btnModify.BackColor = System.Drawing.Color.Transparent;
            this.btnModify.BaseColor = System.Drawing.Color.Transparent;
            this.btnModify.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnModify.ButtonText = null;
            this.btnModify.ForeColor = System.Drawing.Color.Black;
            this.btnModify.Hint = "Modify Model";
            this.btnModify.Image = ((System.Drawing.Image)(resources.GetObject("btnModify.Image")));
            this.btnModify.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnModify.Location = new System.Drawing.Point(323, 4);
            this.btnModify.Name = "btnModify";
            this.btnModify.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnModify.Size = new System.Drawing.Size(35, 35);
            this.btnModify.TabIndex = 12;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDeleteModel
            // 
            this.btnDeleteModel.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteModel.BaseColor = System.Drawing.Color.Transparent;
            this.btnDeleteModel.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnDeleteModel.ButtonText = null;
            this.btnDeleteModel.Enabled = false;
            this.btnDeleteModel.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteModel.Hint = "Delete Model";
            this.btnDeleteModel.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteModel.Image")));
            this.btnDeleteModel.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDeleteModel.Location = new System.Drawing.Point(285, 4);
            this.btnDeleteModel.Name = "btnDeleteModel";
            this.btnDeleteModel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnDeleteModel.Size = new System.Drawing.Size(35, 35);
            this.btnDeleteModel.TabIndex = 11;
            this.btnDeleteModel.Click += new System.EventHandler(this.btnDeleteModel_Click);
            // 
            // comboBoxClaheModel
            // 
            this.comboBoxClaheModel.BackColor = System.Drawing.Color.LightSteelBlue;
            this.comboBoxClaheModel.FormattingEnabled = true;
            this.comboBoxClaheModel.Items.AddRange(new object[] {
            "Normalization contrast",
            "High contrast",
            "Generic contrast",
            "Low contrast",
            "New contrast"});
            this.comboBoxClaheModel.Location = new System.Drawing.Point(12, 8);
            this.comboBoxClaheModel.Name = "comboBoxClaheModel";
            this.comboBoxClaheModel.Size = new System.Drawing.Size(226, 29);
            this.comboBoxClaheModel.TabIndex = 0;
            this.comboBoxClaheModel.SelectedIndexChanged += new System.EventHandler(this.comboBoxClaheModel_SelectedIndexChanged);
            // 
            // btnAddModel
            // 
            this.btnAddModel.BackColor = System.Drawing.Color.Transparent;
            this.btnAddModel.BaseColor = System.Drawing.Color.Transparent;
            this.btnAddModel.ButtonColor = System.Drawing.Color.DarkCyan;
            this.btnAddModel.ButtonText = null;
            this.btnAddModel.Enabled = false;
            this.btnAddModel.ForeColor = System.Drawing.Color.Black;
            this.btnAddModel.Hint = "Add Model";
            this.btnAddModel.Image = ((System.Drawing.Image)(resources.GetObject("btnAddModel.Image")));
            this.btnAddModel.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnAddModel.Location = new System.Drawing.Point(246, 4);
            this.btnAddModel.Name = "btnAddModel";
            this.btnAddModel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAddModel.Size = new System.Drawing.Size(35, 35);
            this.btnAddModel.TabIndex = 8;
            this.btnAddModel.Click += new System.EventHandler(this.btnAddModel_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(370, 185);
            this.panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox4);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 185);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Parameter";
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.LightSteelBlue;
            this.textBox5.Location = new System.Drawing.Point(193, 21);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(165, 28);
            this.textBox5.TabIndex = 10;
            this.textBox5.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 22);
            this.label5.TabIndex = 9;
            this.label5.Text = "Name";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.LightSteelBlue;
            this.textBox4.Location = new System.Drawing.Point(193, 149);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(165, 28);
            this.textBox4.TabIndex = 7;
            this.textBox4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 22);
            this.label4.TabIndex = 6;
            this.label4.Text = "Normalized cliplimit";
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.LightSteelBlue;
            this.textBox3.Location = new System.Drawing.Point(193, 117);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(165, 28);
            this.textBox3.TabIndex = 5;
            this.textBox3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 22);
            this.label3.TabIndex = 4;
            this.label3.Text = "Number of greybins";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.LightSteelBlue;
            this.textBox2.Location = new System.Drawing.Point(193, 85);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(165, 28);
            this.textBox2.TabIndex = 3;
            this.textBox2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of regions  Y";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.textBox1.Location = new System.Drawing.Point(193, 53);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(165, 28);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of regions  X";
            // 
            // ClaheProcess
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Tahoma", 13F);
            this.Name = "ClaheProcess";
            this.Size = new System.Drawing.Size(370, 229);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ComboBox comboBoxClaheModel;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox textBox4;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBox3;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.Label label2;
        public GraphicButton btnAddModel;
        public System.Windows.Forms.TextBox textBox5;
        public System.Windows.Forms.Label label5;
        public GraphicButton btnDeleteModel;
        public GraphicButton btnModify;

    }
}
