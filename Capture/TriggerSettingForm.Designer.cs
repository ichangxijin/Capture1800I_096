namespace ImageCapturing
{
    partial class TriggerSettingForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxFramenumber = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBoxFrametime2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFrametime1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDelaytime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.textBoxSoftFrameNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxSoftFrameTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(387, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxSoftFrameNumber);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxSoftFrameTime);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.textBoxFramenumber);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.textBoxFrametime2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxFrametime1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxDelaytime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 279);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Trigger Setting";
            // 
            // textBoxFramenumber
            // 
            this.textBoxFramenumber.Location = new System.Drawing.Point(155, 163);
            this.textBoxFramenumber.Name = "textBoxFramenumber";
            this.textBoxFramenumber.Size = new System.Drawing.Size(142, 21);
            this.textBoxFramenumber.TabIndex = 13;
            this.textBoxFramenumber.Text = "1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "Frame number";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Manual",
            "Static",
            "Dynamic"});
            this.comboBox1.Location = new System.Drawing.Point(155, 38);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(142, 20);
            this.comboBox1.TabIndex = 9;
            // 
            // textBoxFrametime2
            // 
            this.textBoxFrametime2.Location = new System.Drawing.Point(155, 133);
            this.textBoxFrametime2.Name = "textBoxFrametime2";
            this.textBoxFrametime2.Size = new System.Drawing.Size(142, 21);
            this.textBoxFrametime2.TabIndex = 8;
            this.textBoxFrametime2.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "Frame time(SW3 10ms)";
            // 
            // textBoxFrametime1
            // 
            this.textBoxFrametime1.Location = new System.Drawing.Point(155, 99);
            this.textBoxFrametime1.Name = "textBoxFrametime1";
            this.textBoxFrametime1.Size = new System.Drawing.Size(142, 21);
            this.textBoxFrametime1.TabIndex = 6;
            this.textBoxFrametime1.Text = "4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Frame time(SW2 100ms)";
            // 
            // textBoxDelaytime
            // 
            this.textBoxDelaytime.Location = new System.Drawing.Point(155, 70);
            this.textBoxDelaytime.Name = "textBoxDelaytime";
            this.textBoxDelaytime.Size = new System.Drawing.Size(142, 21);
            this.textBoxDelaytime.TabIndex = 4;
            this.textBoxDelaytime.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Delay time(SW1 100ms)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mode";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(387, 74);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "Sim";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBoxSoftFrameNumber
            // 
            this.textBoxSoftFrameNumber.Location = new System.Drawing.Point(155, 244);
            this.textBoxSoftFrameNumber.Name = "textBoxSoftFrameNumber";
            this.textBoxSoftFrameNumber.Size = new System.Drawing.Size(142, 21);
            this.textBoxSoftFrameNumber.TabIndex = 17;
            this.textBoxSoftFrameNumber.Text = "100";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 247);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "Soft Frame number";
            // 
            // textBoxSoftFrameTime
            // 
            this.textBoxSoftFrameTime.Location = new System.Drawing.Point(155, 214);
            this.textBoxSoftFrameTime.Name = "textBoxSoftFrameTime";
            this.textBoxSoftFrameTime.Size = new System.Drawing.Size(142, 21);
            this.textBoxSoftFrameTime.TabIndex = 15;
            this.textBoxSoftFrameTime.Text = "333";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 217);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "Soft Frame time (1ms)";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(387, 114);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 13;
            this.button2.Text = "Soft Sim";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TriggerSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 294);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Name = "TriggerSettingForm";
            this.Text = "TriggerNewForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxFrametime2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFrametime1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDelaytime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBoxFramenumber;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox textBoxSoftFrameNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxSoftFrameTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
    }
}