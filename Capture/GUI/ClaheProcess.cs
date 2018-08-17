using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using AMRT;

namespace ImageCapturing
{
    public partial class ClaheProcess : UserControl
    {

        public delegate void RefreshClahe(int x, int y, int averageGray, float limit);
        public event RefreshClahe ClaheModelChanged;


        public string NumberX
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public string NumberY
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }
        }

        public string AverageGray
        {
            get { return textBox3.Text; }
            set { textBox3.Text = value; }
        }

        public string ClipLimit
        {
            get { return textBox4.Text; }
            set { textBox4.Text = value; }
        }

        public string Model
        {
            get
            { 
                if (comboBoxClaheModel.SelectedIndex < 0)
                {
                    return "";
                }
                else 
                    return comboBoxClaheModel.SelectedItem.ToString(); 
            }
            set { comboBoxClaheModel.SelectedItem = value; }
        }

        public class ClaheMode
        {
            public string Name;
            public string Paras;
        }

        public Dictionary<string,ClaheMode> ClaheModels;

        public ClaheProcess()
        {
            InitializeComponent();
            LoadClaheModels();
            this.textBox1.TextChanged -= new EventHandler(textBox_TextChanged);
            this.textBox2.TextChanged -= new EventHandler(textBox_TextChanged);
            this.textBox3.TextChanged -= new EventHandler(textBox_TextChanged);
            this.textBox4.TextChanged -= new EventHandler(textBox_TextChanged);
            this.textBox1.TextChanged += new EventHandler(textBox_TextChanged);
            this.textBox2.TextChanged += new EventHandler(textBox_TextChanged);
            this.textBox3.TextChanged += new EventHandler(textBox_TextChanged);
            this.textBox4.TextChanged += new EventHandler(textBox_TextChanged);
        }

        public static string DataPath = ClassPath.GetAppDataSystemPath();
        private static string fileClaheModel = DataPath + "\\ClaheModel.xml";

        private static DataTable GenerateTable_ClaheModel()
        {
            DataTable tb = new DataTable("ClaheModel");
            tb.Columns.Add(new DataColumn("Name", typeof(System.String)));
            tb.Columns.Add(new DataColumn("Para", typeof(System.String)));

            return tb;
        }

        public static DataSet GetClaheModel()
        {
            DataSet ds = CapturePub.GetData(fileClaheModel);
            if (ds.Tables.Count < 1)
            {
                ds.Tables.Add(GenerateTable_ClaheModel());
            }

            return ds;
        }

        public void LoadClaheModels()
        {
            ClaheModels = new Dictionary<string, ClaheMode>();
            DataSet ds = GetClaheModel();
            DataTable table = ds.Tables[0];
            if (table != null)
            {
                for (int j = 0; j < table.Rows.Count; ++j)
                {
                    ClaheMode c = new ClaheMode();
                    DataRow row = table.Rows[j];
                    c.Name = row[0].ToString();
                    c.Paras = row["Para"].ToString();

                    ClaheModels.Add(c.Name,c);
                }
            }
            ClaheMode self = new ClaheMode();
            self.Name = "Self Define";
            self.Paras = "8,8,128,4";
            ClaheModels.Add(self.Name,self);
            comboBoxClaheModel.Items.Clear();
            ClaheMode[] temp = new ClaheMode[ClaheModels.Count];
            ClaheModels.Values.CopyTo(temp, 0);
            for (int i = 0; i < temp.Length; i++)
            {
                comboBoxClaheModel.Items.Add(temp[i].Name);
            }
        }
        
        [DllImport("ClaheLibrary.dll")]
        public static extern int CLAHE(ushort[] pImage, int uiXRes, int uiYRes,
           int Min, int Max, int uiNrX, int uiNrY,
            int uiNrBins, float fCliplimit);
        //0 - NO
        //1 - 16,16,128,16
        //2 - 8 ,8 ,128,4
        //3 - 8 ,8 ,128,0.5
        //4 - SELF
        private void comboBoxClaheModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.TextChanged -= new EventHandler(textBox_TextChanged);
            this.textBox2.TextChanged -= new EventHandler(textBox_TextChanged);
            this.textBox3.TextChanged -= new EventHandler(textBox_TextChanged);
            this.textBox4.TextChanged -= new EventHandler(textBox_TextChanged);
            bool onlyread = comboBoxClaheModel.SelectedItem.ToString() != "Self Define";
            textBox1.ReadOnly = onlyread;
            textBox2.ReadOnly = onlyread;
            textBox3.ReadOnly = onlyread;
            textBox4.ReadOnly = onlyread;
            textBox5.ReadOnly = onlyread;
            btnAddModel.Enabled = !onlyread;
            Color backcolor = onlyread ? Color.LightSteelBlue : Color.White;
            textBox1.BackColor = backcolor;
            textBox2.BackColor = backcolor;
            textBox3.BackColor = backcolor;
            textBox4.BackColor = backcolor;
            textBox5.BackColor = backcolor;

            if (comboBoxClaheModel.SelectedItem.ToString() == "No Process")
            {
                groupBox1.Visible = false;
            }
            else
            {
                groupBox1.Visible = true;
                ClaheMode cm = ClaheModels[comboBoxClaheModel.SelectedItem.ToString()];
                string[] paras = cm.Paras.Split(',');
                textBox1.Text = paras[0];
                textBox2.Text = paras[1];
                textBox3.Text = paras[2];
                textBox4.Text = paras[3];
                textBox5.Text = cm.Name;
                btnDeleteModel.Enabled = CheckISSelfDefine();
                btnModify.Enabled = CheckISSelfDefine();
            }

            if (ClaheModelChanged != null )
            {
                try
                {
                    ClaheModelChanged(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text), float.Parse(textBox4.Text));
                }
                catch (System.Exception ex)
                {
                	
                }
            }
            this.textBox1.TextChanged += new EventHandler(textBox_TextChanged);
            this.textBox2.TextChanged += new EventHandler(textBox_TextChanged);
            this.textBox3.TextChanged += new EventHandler(textBox_TextChanged);
            this.textBox4.TextChanged += new EventHandler(textBox_TextChanged);
        }

        public static ushort[,] CLAHE(string strength, ushort[,] image)
        {
            if (strength == "" )
            {
                return image;
            }
            string[] claheStr = strength.Split('$');
            if (claheStr.Length <= 1)
            {
                return image;
            }
           
            string[] temp = (claheStr[0]).Split('\\');
            if (temp.Length <= 1 || (temp.Length > 1 && temp[0] == "") || (temp.Length > 1 && temp[0] == "Clahe,No Process"))
            {
                return image;
            }
            //try
            //{
            //    int model = int.Parse(temp[0]);
            //}
            //catch (System.Exception ex)
            //{
            //    return image;
            //}
            try
            {
                string[] paras = temp[1].Split(',');

                ushort[,] img = (ushort[,])(image.Clone());

                int uiXRes = image.GetLength(0);
                int uiYRes = image.GetLength(1);
                int[] maxmin = LunImage.FindMaxAndMin(image);

                ushort[] cArray = new ushort[uiXRes * uiYRes];
                for (int i = 0; i < uiXRes; ++i)
                {
                    int it = i * uiYRes;
                    for (int j = 0; j < uiYRes; ++j)
                    {
                        cArray[it + j] = image[i, j];
                    }
                }

                int Min = maxmin[1];
                int Max = maxmin[0];
                int uiNrBins = int.Parse(paras[2]);
                int uiNrX = int.Parse(paras[0]);
                int uiNrY = int.Parse(paras[1]);
                float fCliplimit = float.Parse(paras[3]);
                int cla = CLAHE(cArray, uiXRes, uiYRes, Min, Max, uiNrX, uiNrY, uiNrBins, fCliplimit);

                for (int i = 0; i < uiXRes; ++i)
                {
                    int it = i * uiYRes;
                    for (int j = 0; j < uiYRes; ++j)
                    {
                        img[i, j] = (ushort)cArray[it + j];
                    }
                }
                return img;
            }
            catch (System.Exception ex)
            {
                return image;
            }
            
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
            {
                return;
            }
            float tmp = 0f;
            try
            {
                tmp = float.Parse(tb.Text);
            }
            catch (System.Exception ex)
            {
                return;
            }
            if (ClaheModelChanged != null)
            {
                try
                {
                    ClaheModelChanged(int.Parse(textBox1.Text), int.Parse(textBox2.Text), int.Parse(textBox3.Text), float.Parse(textBox4.Text));
                }
                catch (System.Exception ex)
                {

                }
            }
            if (CheckISSelfDefine())
            {
                string name = this.textBox5.Text.Trim();
                ClaheMode cm = new ClaheMode();
                cm.Name = name;
                cm.Paras = textBox1.Text.Trim() + "," + textBox2.Text.Trim() + "," + textBox3.Text.Trim() + "," + textBox4.Text.Trim();
                ClaheModels[name] = cm;
                SaveModel();
                comboBoxClaheModel.SelectedItem = name;
            }
        }

        private void btnAddModel_Click(object sender, EventArgs e)
        {
            string name = this.textBox5.Text.Trim();
            if (ClaheModels.ContainsKey(name) || name == "Self Define")
            {
                cls_MessageBox.Show("Name existed,please change!");
                return;
            }
            ClaheMode cm = new ClaheMode ();
            cm.Name = name;
            cm.Paras = textBox1.Text.Trim() + "," +textBox2.Text.Trim() + "," +textBox3.Text.Trim() + "," +textBox4.Text.Trim();
            ClaheModels.Add(name, cm);
            SaveModel();
            comboBoxClaheModel.SelectedItem = name;
        }

        private void SaveModel()
        {
            ClaheMode[] temp = new ClaheMode[ClaheModels.Count];
            ClaheModels.Values.CopyTo(temp, 0);
            SaveClaheMode(temp);
            LoadClaheModels();
        }

        public static void SaveClaheMode(ClaheMode[] cModeList)
        {
            DataSet ds = GetClaheModel();
            DataTable table = ds.Tables[0];
            if (table != null)
            {
                table.Clear();

                for (int j = 0; j < cModeList.Length; ++j)
                {
                    ClaheMode c = cModeList[j];
                    if (c.Name != "Self Define")
                    {
                        DataRow row = table.NewRow();
                        row[0] = c.Name;
                        row["Para"] = c.Paras;

                        table.Rows.Add(row);
                    }
                }
            }

            DataSet changeSet = ds.GetChanges();
            if (changeSet != null)
            {
                changeSet.WriteXml(fileClaheModel, XmlWriteMode.WriteSchema);
            }
            ds.Dispose();
        }

        private void btnDeleteModel_Click(object sender, EventArgs e)
        {
            if (!CheckISSelfDefine())
            {
                cls_MessageBox.Show("This model can not delete!");
                return;
            }
            DataSet ds = GetClaheModel();
            DataTable table = ds.Tables[0];
            if (table != null)
            {
                for (int i = 0; i < table.Rows.Count;i++ )
                {
                    if (table.Rows[i][0].ToString() == textBox5.Text)
                    {
                        table.Rows.RemoveAt(i);
                        break;
                    }
                }
            }
            DataSet changeSet = ds.GetChanges();
            if (changeSet != null)
            {
                changeSet.WriteXml(fileClaheModel, XmlWriteMode.WriteSchema);
            }
            ds.Dispose();
            LoadClaheModels();
            comboBoxClaheModel.SelectedItem = "No Process";
        }

        private bool CheckISSelfDefine()
        {
            return (textBox5.Text != "No Process" && textBox5.Text != "High Contrast" && textBox5.Text != "General Contrast"
                    && textBox5.Text != "Low Contrast" && textBox5.Text != "High Contrast(Detail)" && textBox5.Text != "General Contrast(Detail)"
                    && textBox5.Text != "Low Contrast(Detail)" && textBox5.Text != "Self Define");
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
            {
                return;
            }
            tb.Focus();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (!CheckISSelfDefine())
            {
                return;
            }
            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
            textBox4.ReadOnly = false;
            textBox1.BackColor = Color.White;
            textBox2.BackColor = Color.White;
            textBox3.BackColor = Color.White;
            textBox4.BackColor = Color.White;
        }
    }
}
