using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using TDicom;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AMRT;

namespace ImageCapturing
{
    public partial class FormImagingPerformance : AMRT.BaseForm
    {

        #region Class Members


        string SaveFilePath;
        //private DBAccess dbIVS;
        //private FormCapture.SelectedVerifyInfo stateInformation;
        private List<ImageROI> captureImagesNow = new List<ImageROI>();
        private List<ImageROI> captureImagesGainSeq = new List<ImageROI>();
        private List<ImageROI> captureImagesPixel = new List<ImageROI>();
        ushort[,] pixelMapData = null;
        private TriggerBase Trigger = null;
        private CaptureBase ImgCapture = null;
        private ContextMenuStrip ImageMenuStrip = null;
        private Matrix myMatrix;

        private AssistantCheckGraphTool assistantTool;
        private bool flag = false;
        private Point mouseDownPoint = new Point();
        private int selectROIImageIndex = -1;

        Bitmap srcBitmap = null;
        Point movePoint = Point.Empty;
        /// <summary>
        /// 最大的缩放比例
        /// </summary>
        private float ScaleMax = 10;
        /// <summary>
        /// 最小的缩放比例
        /// </summary>
        private float ScaleMin = 0.1F;

        private Cursor HandCursor = new Cursor(ClassPath.LoadCursor(ClassPath.Handmove));
        private Cursor DistanceCursor = new Cursor(ClassPath.LoadCursor(ClassPath.TP_crDistance));

        #endregion

        #region Constructors

        public FormImagingPerformance(CaptureBase capture, TriggerBase trigger)
        {
            InitializeComponent();
            this.userControlMutiDicomImages.SignImg = TIcon.Change4dDisplay;
            TCursor.LoadAllCursors(System.Windows.Forms.Application.StartupPath);

            ImgCapture = capture;
            Trigger = trigger;
            ImgCapture.RefreshHostHandle(this.Handle);


            this.Shown -= new EventHandler(FormCapture_Shown);
            this.Shown += new EventHandler(FormCapture_Shown);
            GraphicButton.ShowButtonToolTip -= new GraphicButton.ShowButtonTooltip(GraphicButton_ShowButtonToolTip);
            GraphicButton.ShowButtonToolTip += new GraphicButton.ShowButtonTooltip(GraphicButton_ShowButtonToolTip);
            GraphicButton.LeaveButton -= new GraphicButton.ShowButtonTooltip(GraphicButton_LeaveButton);
            GraphicButton.LeaveButton += new GraphicButton.ShowButtonTooltip(GraphicButton_LeaveButton);
            userControlMutiDicomImages.MayCancelSelect = true;
            assistantTool = new AssistantCheckGraphTool();
        }

        private void FormCapture_Shown(object sender, EventArgs e)
        {

            //先显示出默认界面内容
            SetContextMenuStrip();
            userControlMutiDicomImages.DrawImagesInPane(new List<Bitmap>(), false);
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.Black);
            g.Dispose();
            histogramControl1.SetCenterWindow(0, 0);
            histogramControl1.RefreshHistogram(null);
            Refresh();
            Show();
            //刷新数据在界面显示
            userControlMutiDicomImages.ImageNumChanged -= new UserControlMutiImages.EventChanged(userControlMutiDicomImages_ImageNumChanged);
            userControlMutiDicomImages.ImageNumChanged += new UserControlMutiImages.EventChanged(userControlMutiDicomImages_ImageNumChanged);
            //userControlMutiDicomImages.ImageNumChanged -= new UserControlMutiImages.EventChanged(userControlMutiDicomImages_ImageNumChanged);
            //userControlMutiDicomImages.ImageNumChanged += new UserControlMutiImages.EventChanged(userControlMutiDicomImages_ImageNumChanged);
            histogramControl1.LUTChanged -= new HistogramControl.LUTChangedDelegate(cImage_LUTChanged);
            histogramControl1.LUTChanged += new HistogramControl.LUTChangedDelegate(cImage_LUTChanged);
            histogramControl1.EventLutMouseUp -= new HistogramControl.LUTChangedDelegate(cImage_LutMouseUp);
            histogramControl1.EventLutMouseUp += new HistogramControl.LUTChangedDelegate(cImage_LutMouseUp);
            this.KeyUp -= new KeyEventHandler(FormCapture_KeyUp);
            this.KeyUp += new KeyEventHandler(FormCapture_KeyUp);
            tabControlCalibration.SelectedIndex = 0;
            tabControlCalibration.TabPages[0].BackColor = Color.DodgerBlue;
            captureImagesNow = captureImagesGainSeq;
            ImageNumberNow = captureImagesNow.Count;//2012.1.11，ml
            //tabControlCalibration_Selected(null,null);
            Refresh();
            tabControlCalibration.Selected -= new TabControlEventHandler(tabControlCalibration_Selected);
            tabControlCalibration.Selected += new TabControlEventHandler(tabControlCalibration_Selected);
            comboBoxGainMode.SelectedIndex = comboBoxGainMode.Items.Count - 1; ;
            comboBoxGainMode1.SelectedIndex = comboBoxGainMode1.Items.Count - 1;
            ImgCapture.Ready();
            //RefreshProfileFigureAsDefault(zedGraphControl1.GraphPane,true);
        }
        ToolStripSeparator separator1 = new ToolStripSeparator();
        ToolStripSeparator separator2 = new ToolStripSeparator();
        ToolStripSeparator separator3 = new ToolStripSeparator();
        ToolStripMenuItem selectAllImages;
        private void SetContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            selectAllImages = new ToolStripMenuItem("Select all for linear detection", null, SelectAll_Click);
            ToolStripMenuItem deleteOneImage = new ToolStripMenuItem("Remove", null, Delete_Click);
            deleteOneImage.Name = "DELETE";
            ToolStripMenuItem saveImage = new ToolStripMenuItem("Save as...", null, SaveAs_Click);
            deleteOneImage.Name = "SaveAs";
            ToolStripMenuItem linearDetection = new ToolStripMenuItem("Linear detection", null, LinearDetection_Click);
            linearDetection.Name = "LinearDetection";
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { selectAllImages, separator1, deleteOneImage, separator2, linearDetection, separator3, saveImage });
            userControlMutiDicomImages.SetContextMenuStrip(contextMenuStrip);
            contextMenuStrip.Opening -= new CancelEventHandler(smallImagesContextMenuStrip_Opening);
            contextMenuStrip.Opening += new CancelEventHandler(smallImagesContextMenuStrip_Opening);
            ImageMenuStrip = contextMenuStrip;
        }

        private void FormCapture_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ImgCapture.WorkStatus)
            {
                if (cls_MessageBox.Show("Are you sure to stop capturing images", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.DoEvents();
                    ImgCapture.Cancel();
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
            if(ImgCapture is CapturePKI)
            {
                (ImgCapture as CapturePKI).ReadXMLFileCaptureParameter();
                (ImgCapture as CapturePKI).RefreshAcquisitionStructureParameters();

            }
        }

        private void DisposeVariables()
        {
            if (ImgCapture != null)
            {
                ImgCapture.imgList = new List<ImageObject>();
            }
            HandCursor.Dispose();
            DistanceCursor.Dispose();
        }

        #endregion

        #region Control Events

        private void FormCapture_KeyUp(object sender, KeyEventArgs e)
        {
            Keys a = e.KeyCode;
            switch (a)
            {
                case Keys.F6:
                    {
                        if (!ImgCapture.WorkStatus)
                        {
                            gbCapture_Click(null, null);

                        }
                    }
                    break;
                case Keys.F7:
                    {
                        if (ImgCapture.WorkStatus)
                        {
                            gbCapture_Click(null, null);

                        }
                    }
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public List<Bitmap> GetSmallBitmapList(List<ImageROI> imageList)
        {
            List<Bitmap> bmList = new List<Bitmap>();
            if (imageList != null)
            {
                for (int i = 0; i < imageList.Count; ++i)
                {
                    bmList.Add(imageList[i].smallBMP);
                }
            }
            return bmList;
        }

        private void smallImagesContextMenuStrip_Opening(object sender, EventArgs e)
        {
            if (captureImagesNow.Count <= 0 || userControlMutiDicomImages.Num <= 0)
            {
                ImageMenuStrip.Visible = false;
                return;
            }

            int index = userControlMutiDicomImages.SelectedIndex;
            if (index < 0)
            {
                ImageMenuStrip.Visible = false;
                return;
            }
            ImageMenuStrip.Visible = true;
            selectAllImages.Checked = (captureImagesNow.Count == userControlMutiDicomImages.SelectList.Count);
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    在小网格中选择Image保存到数据库
        private void Select_Click(object sender, EventArgs e)
        {
            if (captureImagesNow.Count <= 0)
            {
                return;
            }

            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null) return;
            item.Checked = !item.Checked;
            userControlMutiDicomImages.SetSelectImage(item.Checked);
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            if (captureImagesNow.Count <= 0)
            {
                return;
            }
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null) return;
            item.Checked = !item.Checked;
            userControlMutiDicomImages.SelectAll(item.Checked);
        }

        //private void LinearDetection_Click(object sender, EventArgs e)
        //{
        //    List<ImageROI> imageList = new List<ImageROI>();
        //    foreach (int i in userControlMutiDicomImages.SelectList)
        //    {
        //        imageList.Add(captureImagesNow[i]);
        //    }
        //    if (imageList.Count <= 1)
        //    {
        //        cls_MessageBox.Show("Please select images more than 2.");
        //        return;
        //    }

        //    string detectionAreaString = CapturePub.readCaptrueValue("LinearDetectionArea", false);
        //    int detectionArea = 15;
        //    if (!int.TryParse(detectionAreaString, out detectionArea))
        //    {
        //        detectionArea = 15;
        //    }

        //    SortedDictionary<double, double> muValues = new SortedDictionary<double, double>();
        //    for (int i = 0; i < imageList.Count; i++)
        //    {
        //        ushort[,] imgData = imageList[i].imageData;
        //        int radius = (int)((detectionArea * 10 / imageList[i].pixelSize) / 2.0F);
        //        if (radius <= 0 || radius >= imgData.GetLength(0) / 2)
        //        {
        //            radius = 500;
        //        }
        //        double v = 0;
        //        int upRow = (int)(imageList[i].centerY - radius);
        //        if (upRow < 0 || upRow >= imgData.GetLength(0))
        //        {
        //            upRow = 0;
        //        }
        //        int downRow = (int)(imageList[i].centerY + radius);
        //        if (downRow < 0 || downRow >= imgData.GetLength(0))
        //        {
        //            downRow = imgData.GetLength(0) - 1;
        //        }

        //        int leftCol = (int)(imageList[i].centerX - radius);
        //        if (leftCol < 0 || leftCol >= imgData.GetLength(1))
        //        {
        //            leftCol = 0;
        //        }
        //        int rightCol = (int)(imageList[i].centerX + radius);
        //        if (rightCol < 0 || rightCol >= imgData.GetLength(1))
        //        {
        //            rightCol = imgData.GetLength(1) - 1;
        //        }


        //        for (int row = upRow; row <= downRow; row++)
        //        {
        //            for (int col = leftCol; col <= rightCol; col++)
        //            {
        //                v += imgData[row, col];
        //            }
        //        }
        //        if (!muValues.ContainsKey(imageList[i].mu))
        //        {
        //            muValues.Add(imageList[i].mu, v);
        //        }
        //    }

        //    double[] mus = new double[muValues.Count];
        //    double[] imgVs = new double[muValues.Count];
        //    muValues.Keys.CopyTo(mus, 0);
        //    muValues.Values.CopyTo(imgVs, 0);
        //    double xMin = mus[0];
        //    double xMax = mus[mus.Length - 1];
        //    double yMin = double.MaxValue;
        //    double yMax = double.MinValue;
        //    foreach (double v in imgVs)
        //    {
        //        if (v < yMin)
        //        {
        //            yMin = v;
        //        }
        //        if (v > yMax)
        //        {
        //            yMax = v;
        //        }
        //    }

        //    ZedGraphImageCapturing.GraphPane OARPaneX = zedGraphControl1.GraphPane;
        //    OARPaneX.XAxis.Scale.Max = xMax + 1;
        //    OARPaneX.XAxis.Scale.Min = xMin - 1;
        //    OARPaneX.XAxis.Scale.MajorStep = 1;

        //    OARPaneX.YAxis.Scale.Min = (yMin - 5);
        //    OARPaneX.YAxis.Scale.Max = (yMax + 5);
        //    OARPaneX.YAxis.Scale.MajorStep = 5;



        //    OARPaneX.CurveList.Clear();
        //    OARPaneX.AddCurve("", mus, imgVs, System.Drawing.Color.Red, ZedGraphImageCapturing.SymbolType.None);
        //    //RefreshImage();
        //    zedGraphControl1.Refresh();

        //    //Form1 ctrl = new Form1(imageList, zedGraphControl1);
        //    //ctrl.ShowDialog();
        //}

        private void LinearDetection_Click(object sender, EventArgs e)
        {
            List<ImageROI> imageList = new List<ImageROI>();
            foreach (int i in userControlMutiDicomImages.SelectList)
            {
                imageList.Add(captureImagesNow[i]);
            }
            if (imageList.Count <= 1)
            {
                cls_MessageBox.Show("Please select images more than 2.");
                return;
            }
            LinarDetection ctrl = new LinarDetection(imageList);
            ShowBaseForm.ShowControlDialog(ctrl);
        }

        //private void RefreshProfileFigureAsDefault(ZedGraphImageCapturing.GraphPane OARPane, bool AxisX)
        //{
        //    OARPane.CurveList.Clear();
        //    OARPane.Fill.Color = System.Drawing.Color.Black;
        //    OARPane.Title.FontSpec.Size = 25;
        //    if (AxisX)
        //    {
        //        //OARPane.Title.Text = "Linear ";
        //    }
        //    else
        //    {
        //        //OARPane.Title.Text = "Offset Axis Y Ratio (OAR)";
        //    }
        //    OARPane.Title.FontSpec.FontColor = System.Drawing.Color.Green;

        //    OARPane.XAxis.IsVisible = true;
        //    OARPane.XAxis.Title.Text = "Dose(mu)";
        //    OARPane.XAxis.Title.FontSpec.Size = 18;
        //    OARPane.XAxis.Title.FontSpec.FontColor = System.Drawing.Color.Green;

        //    OARPane.XAxis.MajorGrid.IsVisible = true;
        //    OARPane.XAxis.MajorGrid.Color = System.Drawing.Color.Green;
        //    OARPane.XAxis.MajorGrid.DashOn = 3;

        //    OARPane.XAxis.Scale.IsVisible = true;
        //    OARPane.XAxis.Scale.Max = 15;
        //    OARPane.XAxis.Scale.Min = -15;
        //    OARPane.XAxis.Scale.MajorStep = 1;
        //    OARPane.XAxis.Scale.FontSpec.FontColor = System.Drawing.Color.LimeGreen;
        //    OARPane.XAxis.Scale.FontSpec.Border.Color = System.Drawing.Color.LimeGreen;

        //    OARPane.YAxis.IsVisible = true;
        //    OARPane.YAxis.Title.Text = "Value";//"Relative Dose(%)";
        //    OARPane.YAxis.Title.FontSpec.Size = 18;
        //    OARPane.YAxis.Title.FontSpec.FontColor = System.Drawing.Color.Green;

        //    OARPane.YAxis.MajorGrid.IsVisible = true;
        //    OARPane.YAxis.MajorGrid.Color = System.Drawing.Color.Green;
        //    OARPane.YAxis.MajorGrid.DashOn = 3;

        //    OARPane.YAxis.Scale.IsVisible = true;
        //    OARPane.YAxis.Scale.Max = 100;
        //    OARPane.YAxis.Scale.Min = 0;
        //    OARPane.YAxis.Scale.MajorStep = 5;
        //    OARPane.YAxis.Scale.FontSpec.FontColor = System.Drawing.Color.LimeGreen;
        //    OARPane.YAxis.Scale.FontSpec.Border.Color = System.Drawing.Color.LimeGreen;

        //    OARPane.Legend.Fill.Color = System.Drawing.Color.Black;
        //    OARPane.Legend.FontSpec.FontColor = System.Drawing.Color.Green;
        //    OARPane.Legend.Border = new ZedGraphImageCapturing.Border(System.Drawing.Color.Green, 0.5f);
        //    OARPane.Chart.Fill = new ZedGraphImageCapturing.Fill(new SolidBrush(System.Drawing.Color.Black), true);
        //    OARPane.Chart.Border = new ZedGraphImageCapturing.Border(true, System.Drawing.Color.Green, 2);

        //    OARPane.XAxis.Scale.Format = "f2";
        //    zedGraphControl1.ZoomPane(OARPane, 1.0f, PointF.Empty, false);
        //    //zedGraphControlY.ZoomPane(OARPane, 1.0f, PointF.Empty, false);
        //}
        private int ImageNumberNow = 0;//2012.1.11，ml

        private void Delete_Click(object sender, EventArgs e)
        {
            int originalIndex = userControlMutiDicomImages.SelectedIndex;
         
            if (originalIndex < 0 || captureImagesNow.Count <= 0)
            {
                return; 
            }
            Cursor = Cursors.WaitCursor;
            ImageROI imgROI = captureImagesNow[originalIndex];
           
            captureImagesNow.RemoveAt(originalIndex);
            if (userControlMutiDicomImages.SelectList.Contains(originalIndex))
            {
                userControlMutiDicomImages.SelectList.Remove(originalIndex);
               
            }
            ImageNumberNow = ImageNumberNow - 1;//2012.1.11，ml
            for (int k = 0; k < userControlMutiDicomImages.SelectList.Count; k++)
            {
                if (userControlMutiDicomImages.SelectList[k] > originalIndex)
                {
                    userControlMutiDicomImages.SelectList[k] = userControlMutiDicomImages.SelectList[k] - 1;
                }
            }
           
            userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow), originalIndex);
            Cursor = Cursors.Default;

            if (ImageNumberNow <= 0) //2012.1.11，ml
            {

                SetContextMenuStrip();
                userControlMutiDicomImages.DrawImagesInPane(new List<Bitmap>(), false);
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                g.Clear(Color.Black);
                g.Dispose();
                histogramControl1.SetCenterWindow(0, 0);
                histogramControl1.RefreshHistogram(null);
                Refresh();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                SNRlabel.Text = "";
            }

        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            int originalIndex = userControlMutiDicomImages.SelectedIndex;
            if (originalIndex < 0 || captureImagesNow.Count <= 0)
            {
                return;
            }
            List<ushort[,]> list = new List<ushort[,]>();
            list.Add(captureImagesNow[originalIndex].ImageData);
            using (SaveFileDialog f = new SaveFileDialog())
            {
                f.Title = "Save his file";
                f.Filter = "*.*|*.*|*.his|*.his";
                f.SupportMultiDottedExtensions = false;
                string scanPath = Application.StartupPath;
                try
                {
                    scanPath = CapturePub.readCaptrueValue("SaveLocalHisFilePath", false);
                }
                catch
                {
                    scanPath = Application.StartupPath;
                }
                if (!System.IO.Directory.Exists(scanPath))
                {
                    scanPath = Application.StartupPath;
                }
                f.InitialDirectory = scanPath;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    Refresh();
                    
                    string fileName = f.FileName;
                    if (fileName == null || fileName == "")
                    {
                        return;
                    }
                    try
                    {
                        CapturePub.saveCaptrueValue("SaveLocalHisFilePath", fileName.Substring(0, fileName.LastIndexOf('\\') + 1));
                    }
                    catch (System.Exception ex)
                    {
                    	
                    }
                    if (!fileName.EndsWith(".his"))
                    {
                        fileName += ".his";
                    }


                    int intergrationTime = 1000;
                    if (ImgCapture is CapturePKI)
                    {
                        intergrationTime = (int)(ImgCapture as CapturePKI).pki_config.integrationTime;
                    }
                    SaveasHisFile(fileName, list, intergrationTime);
                    cls_MessageBox.Show("Save his file successfully!");
                }
            }
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        void userControlMutiDicomImages_ImageNumChanged(object Sender, EventArgs e)
        {
            //textBox1.Text = "";
            //textBox2.Text = "";
            //textBox3.Text = "";

            pixelMapData = null;
            int index = userControlMutiDicomImages.SelectedIndex;
            selectROIImageIndex = index;
            ImageROI cImageB = captureImagesNow[index].Clone() as ImageROI;
            histogramControl1.SetCenterWindow(cImageB.level, cImageB.window);
            histogramControl1.SetColorModeConverse(cImageB.converse);
            histogramControl1.SetColorMode(cImageB.colorModeName);
            histogramControl1.RefreshHistogram(cImageB.ImageData);

            labelMedianValue.Text = "Average:" + captureImagesNow[index].averageValue.ToString();
            srcBitmap = cImageB.BMP;
            int minLen = pictureBox1.Width > pictureBox1.Height ? pictureBox1.Height : pictureBox1.Width;
            float scale = (float)minLen / srcBitmap.Width;
            myMatrix = new Matrix();
            int offsetX = (int)((pictureBox1.Width - minLen) / 2.0f);
            int offsetY = (int)((pictureBox1.Height - minLen) / 2.0f);
            myMatrix.Scale(scale, scale);
            myMatrix.Translate(offsetX, offsetY, MatrixOrder.Append);
            if (tabControlCalibration.SelectedIndex == 1)
            {
                pixelMapData = new ushort[cImageB.ImageData.GetLength(0), cImageB.ImageData.GetLength(1)];
                int Average = 0;
                long SumValue = 0;
                string xmlValue = CapturePub.readCaptrueValue("UNIT");
                textBoxUnit.Text = (xmlValue == "" ? "4" : xmlValue);
                int unitlength = int.Parse(textBoxUnit.Text);
                xmlValue = CapturePub.readCaptrueValue("Coefficient");
                textBoxCoefficient.Text = (xmlValue == "" ? "0.9" : xmlValue);
                float percentage = float.Parse(textBoxCoefficient.Text);
                for (int i = 0; i < cImageB.ImageData.GetLength(0) / unitlength; i++)
                {
                    for (int j = 0; j < cImageB.ImageData.GetLength(1) / unitlength; j++)
                    {
                        Average = 0;
                        SumValue = 0;
                        for (int ii = 0; ii < unitlength; ii++)
                        {
                            for (int jj = 0; jj < unitlength; jj++)
                            {
                                SumValue += cImageB.ImageData[i * unitlength + ii, j * unitlength + jj];
                            }
                        }
                        Average = (int)(((double)SumValue / (unitlength * unitlength)) * percentage);
                        for (int ii = 0; ii < unitlength; ii++)
                        {
                            for (int jj = 0; jj < unitlength; jj++)
                            {
                                if (cImageB.ImageData[i * unitlength + ii, j * unitlength + jj] >= Average)
                                {
                                    pixelMapData[i * unitlength + ii, j * unitlength + jj] = 0;
                                }
                                else
                                {
                                    pixelMapData[i * unitlength + ii, j * unitlength + jj] = 65535;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                assistantTool.CheckImageInList(index);
                RefreshROIInformation();
            }
            RefreshImage(cImageB);
            cImageB.Dispose();
            GC.Collect();

            //List<Point> pts = GetBoundaryOfData();
            //GetEvaluateInfo(pts);
        }

        private List<Point> GetBoundaryOfData()
        {
            int index = userControlMutiDicomImages.SelectedIndex;
            ImageROI imgTemp = captureImagesNow[index].Clone() as ImageROI;
            ushort[,] data = imgTemp.ImageData;
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            int sum = 0;
            for (int i = width / 2 - 5; i < width / 2 + 5; i++)
            {
                for (int j = height / 2 - 5; j < height / 2 + 5; j++)
                {
                    sum += data[i, j];
                }
            }
            int average = sum / 100;
            double scale = 0.5;
            try
            {
                scale = double.Parse(CapturePub.readCaptrueValue("EvaluateScaleThreshold"));
            }
            catch (System.Exception ex)
            {
                scale = 0.5;
            }

            int threshold = (int)(average * scale);
            Bitmap bmp = (Bitmap)(imgTemp.BMP.Clone());
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (data[i, j] > threshold)
                    {
                        bmp.SetPixel(j, i, Color.Red);
                    }
                }
            }
            BoundaryTracker bt = new BoundaryTracker();
            if (bt.GetSerializedBoundary(bmp, Color.Red))
            {
                int indexPts = 0;
                for (int i = 0; i < bt.CL.Count;i++ )
                {
                    if (bt.CL[i].Length > bt.CL[indexPts].Length)
                    {
                        indexPts = i;
                    }
                }
                if (bt.CL.Count!= 0)
                {
                    PointF[] pts = bt.CL[indexPts];
                    int Xmin = int.MaxValue;
                    int Xmax = int.MinValue;
                    int Ymin = int.MaxValue;
                    int Ymax = int.MinValue;
                    for (int i = 0; i < pts.Length; i++)
                    {
                        if (Xmin > pts[i].X)
                        {
                            Xmin = (int)pts[i].X;
                        }
                        if (Ymin > pts[i].Y)
                        {
                            Ymin = (int)pts[i].Y;
                        }
                        if (Xmax < pts[i].X)
                        {
                            Xmax = (int)pts[i].X;
                        }
                        if (Ymax < pts[i].Y)
                        {
                            Ymax = (int)pts[i].Y;
                        }
                    }
                    Point pt1 = new Point(Xmin, Ymin);
                    Point pt2 = new Point(Xmax, Ymin);
                    Point pt3 = new Point(Xmax, Ymax);
                    Point pt4 = new Point(Xmin, Ymax);
                    return new List<Point>() { pt1, pt2, pt3, pt4 };
                }
                return new List<Point>() { new Point(0, 0), new Point(data.GetLength(0), 0), 
                new Point(data.GetLength(0), data.GetLength(1)), new Point(0, data.GetLength(1)) };
            }
            return new List<Point>() { new Point(0, 0), new Point(data.GetLength(0), 0), 
                new Point(data.GetLength(0), data.GetLength(1)), new Point(0, data.GetLength(1)) };
        }

        private void GetEvaluateInfo(List<Point> pts)
        {
           

            int Xmin = pts[0].X;
            int Xmax = pts[2].X;
            int Ymin = pts[0].Y;
            int Ymax = pts[2].Y;

            int index = userControlMutiDicomImages.SelectedIndex;
            ImageROI imgTemp = captureImagesNow[index].Clone() as ImageROI;
            ushort[,] data = imgTemp.ImageData;

            int RecNum = 0;

            double sum = 0;
            for (int i = Ymin; i < Ymax; i++)
            {
                for (int j = Xmin; j < Xmax; j++)
                {
                    sum += data[i, j];
                    RecNum++;
                }
            }
            double averageValue = (double)(sum / ((Xmax - Xmin + 1) * (Ymax - Ymin + 1)));
            textBox1.Text = averageValue.ToString("0.00");

            //计算标准差
            double sumVariance = 0;
            for (int i = Ymin; i < Ymax; i++)
            {
                for (int j = Xmin; j < Xmax; j++)
                {
                    sumVariance += Math.Pow((data[i, j] - averageValue), 2);//方差
                }
            }
            double variance = Math.Sqrt(sumVariance / ((Xmax - Xmin + 1) * (Ymax - Ymin + 1)));
            textBox2.Text = variance.ToString("0.00");

            int diffNum = 0;
            int diff = 100;
            try
            {
                diff = (int)(averageValue * double.Parse(CapturePub.readCaptrueValue("EvaluateDiffThreshold")));
            }
            catch (System.Exception ex)
            {
                diff = (int)(averageValue * 0.5);
            }
            List<Point> temp = new List<Point>();
            for (int i = Ymin; i < Ymax - 1; i++)
            {
                for (int j = Xmin; j < Xmax - 1; j++)
                {
                    int diffColumn = data[j, i + 1] - data[j, i];
                    int diffRow = data[j + 1, i] - data[j, i];
                    if (Math.Abs(diffColumn) > diff || Math.Abs(diffRow) > diff)
                    {
                        diffNum++;
                        temp.Add(new Point(j, i));
                    }
                }
            }
            textBox3.Text = diffNum.ToString("0.00");
            if (diffNum > (((Xmax - Xmin + 1) * (Ymax - Ymin + 1)) / 4))
            {
                try
                {
                    string tag = CapturePub.readCaptrueValue("ShowDiffPoint");
                    if (tag == "T")
                    {
                        cls_MessageBox.Show("There too many error point!");
                    }
                }
                catch (System.Exception ex)
                {
                    CapturePub.saveCaptrueValue("ShowDiffPoint", "F");
                }

            }
            else
            {

            }
            if (RecNum > 0)
            {
                SNRlabel.Text = (averageValue / (Math.Sqrt(sumVariance / RecNum))).ToString("0.00");
            }
        }

        protected void tabControlCalibration_Selected(object sender, TabControlEventArgs e)
        {
            for (int i = 0; i < tabControlCalibration.TabPages.Count; i++)
            {
                tabControlCalibration.TabPages[i].BackColor = Color.LightSteelBlue;
                if (tabControlCalibration.SelectedIndex == i)
                {
                    tabControlCalibration.TabPages[i].BackColor = Color.DodgerBlue;
                }
            }
            if (tabControlCalibration.SelectedIndex == 0)
            {
                panelGainSeq.Visible = true;
                panelPixel.Visible = false;
                captureImagesNow = captureImagesGainSeq;
                userControlMutiDicomImages.SingleSelect = false;
                separator1.Visible = true;
                selectAllImages.Visible = true;
                this.pictureBox1.ContextMenuStrip = this.contextMenuStripRectSet;

            }
            else
            {
                panelPixel.Visible = true;
                panelGainSeq.Visible = false;
                captureImagesNow = captureImagesPixel;
                userControlMutiDicomImages.SingleSelect = true;
                separator1.Visible = false;
                selectAllImages.Visible = false;
                this.pictureBox1.ContextMenuStrip = null;
            }
            userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow));
            if (captureImagesNow.Count <= 0)
            {
                Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(bm);
                g.Clear(Color.Black);
                g.Dispose();
                pictureBox1.Image = bm; 
            }
        }

        void cImage_LUTChanged(int ChangedType, object ChangedObject, Color[] LUT)
        {
            if (captureImagesNow.Count <= 0 || userControlMutiDicomImages.SelectedIndex >= captureImagesNow.Count)
            {
                return;
            }

            ImageROI img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
            img.SetLUT(LUT);
            if (ChangedType == 0)
            {
                img.level = ((Point)ChangedObject).X;
                img.window = ((Point)ChangedObject).Y;
            }
            else if (ChangedType == 1)
            {
                img.converse = (bool)ChangedObject;
            }
            else if (ChangedType == 2)
            {
                img.colorModeName = (string)ChangedObject;
            }
            srcBitmap = img.BMP;
            RefreshImage(img);
        }

        void cImage_LutMouseUp(int ChangedType, object ChangedObject, Color[] LUT)
        {
            if (captureImagesNow.Count <= 0 || userControlMutiDicomImages.SelectedIndex >= captureImagesNow.Count)
            {
                return;
            }

            ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
            if (ChangedType == 0)
            {
                img.level = ((Point)ChangedObject).X;
                img.window = ((Point)ChangedObject).Y;
            }
            else if (ChangedType == 1)
            {
                img.converse = (bool)ChangedObject;
            }
            else if (ChangedType == 2)
            {
                img.colorModeName = (string)ChangedObject;
            }

            userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow), false);
        }

        void GraphicButton_ShowButtonToolTip(GraphicButton GB)
        {
            if (GB.Hint != "" && TextToolTipShow.Text != GB.Hint)
            {
                TextToolTipShow.Text = GB.Hint;
                TextToolTipShow.Visible = true;
            }
        }

        void GraphicButton_LeaveButton(GraphicButton GB)
        {
            TextToolTipShow.Text = "";
            TextToolTipShow.Visible = false;
        }

        #endregion

        #region Progress Events

        public void ProgressBegin(string title, int maxStep)
        {
            int CONST_PROGRESSTITLELEN = 150;
            int CONST_PROGRESSBARLEN = 300;

            Graphics g = labelProgressBarTitle.CreateGraphics();
            int strLen = (int)(g.MeasureString(title, labelProgressBarTitle.Font).Width + 0.5) + 4;
            labelProgressBarTitle.Width = (strLen >= CONST_PROGRESSTITLELEN) ? strLen : CONST_PROGRESSTITLELEN;
            if (progressBar.Width > CONST_PROGRESSBARLEN)
            {
                progressBar.Width = CONST_PROGRESSBARLEN;
            }
            labelProgressBarTitle.Text = title;
            labelProgressBarTitle.TextAlign = ContentAlignment.MiddleLeft;
            labelProgressBarTitle.Visible = true;
            if (maxStep > 1)
            {
                progressBar.Maximum = maxStep;
                progressBar.Value = 0;
                progressBar.Visible = true;
                this.progressBar.Top = this.panelStatus.Padding.Top + 2;
            }
            g.Dispose();
            labelProgressBarTitle.Refresh();
            progressBar.Refresh();
        }

        public void ProgressAdd(int step)
        {
            int v = progressBar.Value + step;
            if (v > progressBar.Maximum)
            {
                v = progressBar.Maximum;
            }
            progressBar.Value = v;
        }

        public void ProgressEnd()
        {
            progressBar.Visible = false;
            labelProgressBarTitle.Text = "";
            labelProgressBarTitle.Visible = false;
        }

        #endregion

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~  Call capture image class interface
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WIN_MSG.WM_SHOW_PROGRESS:
                    switch ((int)m.WParam)
                    {
                        case -1:
                            ProgressAdd((int)m.LParam);
                            break;

                        case -2:
                            ProgressEnd();
                            break;

                        default:
                            int num = (int)m.LParam;
                            ProgressBegin(ImgCapture.PopWinMessage((int)m.WParam), num);
                            break;
                    }
                    break;

                case WIN_MSG.WM_TRIGGER_CHANGE:
                    if ((TRIGGER_STATUS)m.WParam == TRIGGER_STATUS.ON)
                    {
                        //界面不可用
                        //SetInterfacebyCapture(true);
                    }
                    else
                    {
                        //SetInterfacebyCapture(false);
                        //Trigger.Stop();
                    }
                    break;

                case WIN_MSG.WM_TRIGGER_STATUS:
                    if ((TRIGGER_STATUS)m.WParam == TRIGGER_STATUS.OFF)
                    {
                        //SetInterfacebyCapture(false);
                    }
                    else if ((TRIGGER_STATUS)m.WParam == TRIGGER_STATUS.ON)
                    {
                        //SetInterfacebyCapture(true);
                    }
                    break;
                case WIN_MSG.WM_CAPTURE_DATA:
                    List<ImageObject> imgs = new List<ImageObject>();
                    switch ((CapturePKI.PanelCaptureMode)m.WParam)
                    {
                        case CapturePKI.PanelCaptureMode.DoubleExposure:
                            imgs.AddRange(ImgCapture.imgList);
                            ImgCapture.imgList.Clear();
                            RefreshDataDisplay(imgs, true, (int)m.LParam);
                            break;
                        case CapturePKI.PanelCaptureMode.Sequence:
                            imgs.AddRange(ImgCapture.imgList);
                            ImgCapture.imgList.Clear();
                            RefreshDataDisplay(imgs, false, (int)m.LParam);
                            break;
                        case CapturePKI.PanelCaptureMode.Continuous:
                            imgs.Add(ImgCapture.imgList[0]);
                            ImgCapture.imgList.RemoveAt(0);
                            RefreshDataDisplay(imgs, false, (int)m.LParam);
                            break;
                    }
                    break;
                case WIN_MSG.WM_CAPTURE_WORKSTATUS:
                    SetInterfacebyCapture((int)m.WParam == 1);
                    break;

                default:
                    base.DefWndProc(ref m);///调用基类函数处理非自定义消息。
                    break;
            }
        }

        private void gbCapture_Click(object sender, EventArgs e)
        {

            switch (ImgCapture.LinkStatus)
            {
                case PanelLinkStatus.NONE:
                    break;
                case PanelLinkStatus.LINK_FAIL:
                    cls_MessageBox.Show("Link error!", "Information");
                    return;
                case PanelLinkStatus.LINK_SUCCESS:
                    break;
                case PanelLinkStatus.LINKING:
                    cls_MessageBox.Show("It is connecting panel,Please wait a few minutes.", "Information");
                    return;
                default:
                    cls_MessageBox.Show("No link status!", "Information");
                    return;
            }
            //if (!(ImgCapture is CapturePKI))
            //{
            //    cls_MessageBox.Show("Do not link panel!", "Information");
            //}


            if (ImgCapture.WorkStatus)
            {
                if (cls_MessageBox.Show("Are you sure to stop capturing images", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.DoEvents();
                    ImgCapture.Cancel();
                }
            }
            else
            {
                if (tabControlCalibration.SelectedIndex == 0)
                {
                    if (tbFrameAmount.Text.Trim() == "" || tbFrameAmount.Text.Trim() == "0")
                    {
                        cls_MessageBox.Show("Please input frame amount.");
                        return;
                    }
                    if (tbIntegrationTime.Text.Trim() == "")
                    {
                        cls_MessageBox.Show("Please input integration time.");
                        return;
                    }
                }
                else
                {
                    if (tbFrameAmount1.Text.Trim() == "" || tbFrameAmount1.Text.Trim() == "0")
                    {
                        cls_MessageBox.Show("Please input frame amount.");
                        return;
                    }
                    if (tbIntegrationTime1.Text.Trim() == "")
                    {
                        cls_MessageBox.Show("Please input integration time.");
                        return;
                    }
                }
                if (ImgCapture is CapturePKI)
                {
                    CapturePKI imgCapturePKI = ImgCapture as CapturePKI;
                    imgCapturePKI.DoseScaleMode = false;
                    if (tabControlCalibration.SelectedIndex == 0)
                    {
                        imgCapturePKI.pki_config.gianMode = (CapturePKI.ElectricCapacityMode)comboBoxGainMode.SelectedIndex;
                        imgCapturePKI.FrameCount = int.Parse(tbFrameAmount.Text.Trim());
                        imgCapturePKI.pki_config.integrationTime = uint.Parse(tbIntegrationTime.Text.Trim()) * 1000;
                    }
                    else
                    {
                        imgCapturePKI.pki_config.gianMode = (CapturePKI.ElectricCapacityMode)comboBoxGainMode1.SelectedIndex;
                        imgCapturePKI.FrameCount = int.Parse(tbFrameAmount1.Text.Trim());
                        imgCapturePKI.pki_config.integrationTime = uint.Parse(tbIntegrationTime1.Text.Trim()) * 1000;
                    }
                    imgCapturePKI.RefreshAcquisitionStructureParameters();
                    SaveFilePath = CapturePub.SaveDFPath + "\\gain" + DateTime.Now.ToString("yyyy.MM.dd.hhmmss");
                }
                //ImgCapture.SetCaptureMode(CapturePKI.PanelCaptureMode.AverageWithGainSequence);
                ImgCapture.Start();

            }
        }

        private void btnLoadLocalFile_Click(object sender, EventArgs e)
        {
            SaveFilePath = CapturePub.SaveDFPath + "\\gain" + DateTime.Now.ToString("yyyy.MM.dd.hhmmss");
            CaptureBase localCapture = new CaptureLocalFile();
            localCapture.CaptureImageData();
            ImageNumberNow += localCapture.imgList.Count;//2012.1.11，ml
            RefreshDataDisplay(localCapture.imgList, false, 0);
            
        }

        private void SetInterfacebyCapture(bool capturingFlag)
        {
            btnBuildGainSeq.Enabled = !capturingFlag;
            btnLocal.Enabled = !capturingFlag;
            btnAverageGain.Enabled = !capturingFlag;
            btnLocal1.Enabled = !capturingFlag;
            btnCreatePixelMap.Enabled = !capturingFlag;

            btnCapture.Image = !capturingFlag ? TIcon.panelCapture : TIcon.cancelPanelCapture;
            btnCapture2.Image = !capturingFlag ? TIcon.panelCapture : TIcon.cancelPanelCapture;
        }

        private void RefreshDataDisplay(List<ImageObject> imageObjs, bool doubleExposure, int beamon_num)
        {
            //ImgCapture.RotateImage(imageObjs);
            foreach (ImageObject imageObj in imageObjs)
            {
                ImgCapture.RotateImage(imageObj);
                ImageROI imgROI = ImageROI.GenerateImageROI(imageObj);
                int intmu = 1;
                if (!int.TryParse(tbLinacMU.Text.Trim(), out intmu))
                {
                    intmu = 1;
                }
                imgROI.mu = intmu;

                int intFrame = 1;
                if (!int.TryParse(tbFrameAmount.Text.Trim(), out intFrame))
                {
                    intFrame = 1;
                }
                imgROI.frameNum = intFrame;

                imgROI.ImageData = imgROI.imageData;
                captureImagesNow.Add(imgROI);
                if (ImgCapture is CapturePKI)
                {
                    CapturePKI imgCapturePKI = ImgCapture as CapturePKI;
                    imgROI.SaveasHisFile(SaveFilePath, (int)imgCapturePKI.pki_config.integrationTime, imgCapturePKI.pki_config.imageCorrection);
                }
            //Get image statistics information Lst 2011.12.16
            if (captureImagesGainSeq != null)
            {
                if (captureImagesGainSeq.Count != 0)
                {
                    assistantTool.imgBase = captureImagesGainSeq;
                    int index = captureImagesGainSeq.Count - 1;
                    assistantTool.CheckImageInList(index);
                    assistantTool.GetBoundaryOfData();
                    assistantTool.GetStatisticsInfomation();
                }
            }
                
                //SNR = Ave / (Math.Sqrt(Variance / (imgROI.ImageData.GetLength(0) * imgROI.ImageData.GetLength(1))));
                //SNRlabel.Text = SNR.ToString("0.00");
            }

            userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow), true);
            if (ImageNumberNow <= 0)//2012.1.11，ml
            {
                Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(bm);
                g.Clear(Color.Black);
                g.Dispose();
                pictureBox1.Image = bm;
            }
        }

        //private void btnBuildGainSeq_Click(object sender, EventArgs e)
        //{
        //    List<ImageObjectBase> images = new List<ImageObjectBase>();
        //    foreach (int i in userControlMutiDicomImages.SelectList)
        //    {
        //        images.Add(captureImagesNow[i]);
        //    }
        //    if (images.Count <= 0)
        //    {
        //        cls_MessageBox.Show("Please select images.");
        //        return;
        //    }

        //    string CorrectFilePath = CapturePub.LinkFilePath + "\\";
        //    string BakCorrectFilePath = CapturePub.LinkFilePath + "\\bak";
        //    //OffsetFile = CorrectFilePath + CapturePub.readCaptrueValue("OffsetFile");
        //    string GainSeqFile = CorrectFilePath + CapturePub.readCaptrueValue("GainSeqFile");
        //    string BakGainSeqFile = BakCorrectFilePath + "\\" + CapturePub.readCaptrueValue("GainSeqFile");
        //    //PixelMapFile = CorrectFilePath + CapturePub.readCaptrueValue("PixelMapFile");
        //    if (!GainSeqFile.EndsWith(".his"))
        //    {
        //        GainSeqFile += ".his";
        //    }
        //    if (!BakGainSeqFile.EndsWith(".his"))
        //    {
        //        BakGainSeqFile += ".his";
        //    }
        //    if (File.Exists(GainSeqFile))
        //    {
        //        if (!Directory.Exists(BakCorrectFilePath))
        //        {
        //            Directory.CreateDirectory(BakCorrectFilePath);
        //        }
        //        if (!File.Exists(BakGainSeqFile))
        //        {
        //            File.Move(GainSeqFile, BakGainSeqFile);
        //        }
        //    }

        //    images.Sort();
        //    List<ushort[,]> dataList = new List<ushort[,]>();
        //    foreach (ImageROI obj in images)
        //    {
        //        dataList.Add(obj.imageData);
        //    } 
        //    int intergrationTime = 1000;
        //    if (ImgCapture is CapturePKI)
        //    {
        //        intergrationTime = (int)(ImgCapture as CapturePKI).pki_config.integrationTime;
        //    }
        //    string NewGainSeqFileName = "gainSeq_" + (intergrationTime / (1000 * 1000)) + "s_" + DateTime.Now.ToString("yyyy.MM.dd.HHmmss") + ".his";
           
        //    CapturePub.saveCaptrueValue("GainSeqFile", NewGainSeqFileName);
        //    string NewGainSeqFile = CorrectFilePath + NewGainSeqFileName; ;//生成文件的名称
        //    SaveasHisFile(NewGainSeqFile, dataList, intergrationTime);
        //    if (ImgCapture is CapturePKI)
        //    {
        //        (ImgCapture as CapturePKI).SetLinkCorrection();
        //    }
        //    cls_MessageBox.Show("Create gain sequence successfully!");

        //}

        //private void btnAverageGain_Click(object sender, EventArgs e)
        //{
        //    List<ImageROI> images = new List<ImageROI>();
        //    foreach (int i in userControlMutiDicomImages.SelectList)
        //    {
        //        images.Add(captureImagesNow[i]);
        //    }
        //    if (images.Count <= 0)
        //    {
        //        cls_MessageBox.Show("Please select images.");
        //        return;
        //    }

        //    string CorrectFilePath = CapturePub.LinkFilePath + "\\";
        //    string BakCorrectFilePath = CapturePub.LinkFilePath + "\\bak";
        //    //OffsetFile = CorrectFilePath + CapturePub.readCaptrueValue("OffsetFile");
        //    string GainSeqFile = CorrectFilePath + CapturePub.readCaptrueValue("GainSeqFile");
        //    string BakGainSeqFile = BakCorrectFilePath + "\\" + CapturePub.readCaptrueValue("GainSeqFile");
        //    //PixelMapFile = CorrectFilePath + CapturePub.readCaptrueValue("PixelMapFile");
        //    if (!GainSeqFile.EndsWith(".his"))
        //    {
        //        GainSeqFile += ".his";
        //    }
        //    if (!BakGainSeqFile.EndsWith(".his"))
        //    {
        //        BakGainSeqFile += ".his";
        //    }
        //    if (File.Exists(GainSeqFile))
        //    {
        //        if (!Directory.Exists(BakCorrectFilePath))
        //        {
        //            Directory.CreateDirectory(BakCorrectFilePath);
        //        }
        //        if (!File.Exists(BakGainSeqFile))
        //        {
        //            File.Move(GainSeqFile, BakGainSeqFile);
        //        }
        //    }

        //    //images.Sort();
        //    List<ushort[,]> dataList = new List<ushort[,]>();
        //    ushort[,] averageData = new ushort[images[0].imageData.GetLength(0), images[0].imageData.GetLength(1)];
        //    for (int Row = 0; Row < averageData.GetLength(0); Row++)
        //    {
        //        for (int Col = 0; Col < averageData.GetLength(1); Col++)
        //        {
        //            long num = 0;

        //            foreach (ImageObjectBase obj in images)
        //            {
        //                num += obj.imageData[Row, Col];
        //            }
        //            averageData[Row, Col] = (ushort)(num / images.Count);
        //        }
        //    }
        //    dataList.Add(averageData);

        //    int intergrationTime = 1000;
        //    if (ImgCapture is CapturePKI)
        //    {
        //        intergrationTime = (int)(ImgCapture as CapturePKI).pki_config.integrationTime;
        //    }
        //    string NewGainSeqFileName = "aveGain_" + (intergrationTime / (1000 * 1000)) + "s_" + DateTime.Now.ToString("yyyy.MM.dd.HHmmss") + ".his";
        //    CapturePub.saveCaptrueValue("GainSeqFile", NewGainSeqFileName);
        //    //Save to his file
        //    string NewGainSeqFile = CorrectFilePath + NewGainSeqFileName; ;//生成文件的名称
        //    SaveasHisFile(NewGainSeqFile, dataList, intergrationTime);
        //    if (ImgCapture is CapturePKI)
        //    {
        //        (ImgCapture as CapturePKI).SetLinkCorrection();
        //    }

        //    ImageROI cimage = new ImageROI();
        //    cimage.centerX = (int)images[0].centerX;
        //    cimage.centerY = (int)images[0].centerY;
        //    cimage.level = (int)images[0].level;
        //    cimage.window = (int)images[0].window;
        //    cimage.pixelSize = images[0].pixelSize;
        //    cimage.converse = true;
        //    cimage.ImageData = averageData;//2010/08/18 保存原始图，避免失真.数据库中除了Data外，其他都是Drr层面的值
        //    long sumValue = 0;
        //    foreach (ushort v in averageData)
        //    {
        //        sumValue += v;
        //    }
        //    cimage.averageValue = (int)(sumValue / averageData.Length);
        //    cimage.imageWidth = cimage.imageData.GetLength(1);
        //    cimage.imageHeight = cimage.imageData.GetLength(0);
        //    cimage.createTime = DateTime.Now;
        //    captureImagesNow.Add(cimage);
        //    userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow), captureImagesNow.Count - 1);

        //    cls_MessageBox.Show("Create average image as gain sequence successfully!");
        //}

        private void Input_KeyUp(object sender, KeyEventArgs e)
        {
            AMRT.MaskedTextBox temp = (sender as AMRT.MaskedTextBox);
            if (temp == null)
            {
                return;
            }
            temp.Focus();
        }

        private void btnCreatePixelMap_Click(object sender, EventArgs e)
        {
            if (pixelMapData == null)
            {
                cls_MessageBox.Show("Do not select value!");
                return;
            }
            string CorrectFilePath = CapturePub.LinkFilePath + "\\";
            string BakCorrectFilePath = CapturePub.LinkFilePath + "\\bak";
            //OffsetFile = CorrectFilePath + CapturePub.readCaptrueValue("OffsetFile");
            string PixelMapFile = CorrectFilePath + CapturePub.readCaptrueValue("PixelMapFile");
            string BakPixelMapFile = BakCorrectFilePath + "\\" + CapturePub.readCaptrueValue("PixelMapFile");
            //PixelMapFile = CorrectFilePath + CapturePub.readCaptrueValue("PixelMapFile");
            if (!PixelMapFile.EndsWith(".his"))
            {
                PixelMapFile += ".his";
            }
            if (!BakPixelMapFile.EndsWith(".his"))
            {
                BakPixelMapFile += ".his";
            }
            if (File.Exists(PixelMapFile))
            {
                if (!Directory.Exists(BakCorrectFilePath))
                {
                    Directory.CreateDirectory(BakCorrectFilePath);
                }
                if (!File.Exists(BakPixelMapFile))
                {
                    File.Move(PixelMapFile, BakPixelMapFile);
                }
            }

            List<ushort[,]> list = new List<ushort[,]>();
            list.Add(pixelMapData);

            int intergrationTime = 1000;
            if (ImgCapture is CapturePKI)
            {
                intergrationTime = (int)(ImgCapture as CapturePKI).pki_config.integrationTime;
            }
            string NewPixelMapFileName = "pxlMap_" + (intergrationTime / (1000 * 1000)) + "s_" + DateTime.Now.ToString("yyyy.MM.dd.HHmmss") + ".his";
            CapturePub.saveCaptrueValue("PixelMapFile", NewPixelMapFileName);
            //Save to his file
            string NewPixelMapFile = CorrectFilePath + NewPixelMapFileName; ;//生成文件的名称
            SaveasHisFile(NewPixelMapFile, list, intergrationTime);
            if (ImgCapture is CapturePKI)
            {
                (ImgCapture as CapturePKI).SetLinkCorrection();
            }
            cls_MessageBox.Show("Create pixel map successfully!");
        }

        private void RefreshImage(ImageObject img)
        {
            Bitmap bmpTemp = ImageAfterZoom(img.LUT[0], srcBitmap);
            pictureBox1.Image = bmpTemp;
            pictureBox1.Refresh();
        }

        private Bitmap ImageAfterZoom(Color backColor, Bitmap img)
        {
            if (pictureBox1.Width < 1 || this.pictureBox1.Height < 1) return null;

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(backColor), new Rectangle(0, 0, bmp.Width, bmp.Height));
            Bitmap cloneBMP = null;
            RectangleF srcRec = new RectangleF(0, 0, (float)srcBitmap.Width, (float)srcBitmap.Height);
            RectangleF zoomSrcRec = TransformRectangle(srcRec, myMatrix);
            RectangleF viewRec = new RectangleF(0, 0, (float)pictureBox1.Width, (float)pictureBox1.Height);
            RectangleF viewZoomRec = RectangleF.Intersect(viewRec, zoomSrcRec);
            Matrix mxInvert = myMatrix.Clone();
            mxInvert.Invert();
            RectangleF viewSrcRec = TransformRectangle(viewZoomRec, mxInvert);
            if (ReviseImageRect(srcBitmap.Size, ref viewSrcRec))
            {
                cloneBMP = srcBitmap.Clone(viewSrcRec, PixelFormat.Format32bppArgb);
                g.DrawImage(cloneBMP, viewZoomRec);
                if (tabControlCalibration.SelectedIndex == 0)
                {
                    assistantTool.DrawGraph(g, myMatrix);
                }
            }
            if (cloneBMP != null) cloneBMP.Dispose();
            mxInvert.Dispose();
            g.Dispose();
            if (pixelMapData != null)
            {
                List<Point> list = new List<Point>();
                for (int i = 0; i < pixelMapData.GetLength(0); i++)
                {
                    for (int j = 0; j < pixelMapData.GetLength(1); j++)
                    {
                        if (pixelMapData[i, j] == 65535)
                        {
                            list.Add(new Point(j, i));
                        }
                    }
                }
                Point[] ps = list.ToArray();
                if (ps.Length > 0)
                {
                    myMatrix.TransformPoints(ps);
                    Rectangle recR = new Rectangle(0, 0, bmp.Width, bmp.Height);
                    for (int kk = 0; kk < ps.Length; kk++)
                    {
                        if (recR.Contains(new Point((int)ps[kk].X, (int)ps[kk].Y)))
                        {
                            bmp.SetPixel((int)ps[kk].X, (int)ps[kk].Y, Color.FromArgb(255, 255, 0, 0));
                        }
                    }
                }
                //g.FillPolygon(new SolidBrush(Color.Red), ps, System.Drawing.Drawing2D.FillMode.Winding);
            }
            return bmp;
        }

        private bool ReviseImageRect(Size bmSize, ref RectangleF cloneRectF)
        {
            if (cloneRectF.X >= bmSize.Width) return false;
            if (cloneRectF.Y >= bmSize.Height) return false;
            if (cloneRectF.Width < 1) return false;
            if (cloneRectF.Height < 1) return false;

            if (cloneRectF.X < 0) cloneRectF.X = 0;
            if (cloneRectF.Y < 0) cloneRectF.Y = 0;

            int lx = (int)(Math.Ceiling(cloneRectF.X));
            if (lx + cloneRectF.Width > bmSize.Width)
            {
                cloneRectF.Width = bmSize.Width - lx;
            }
            int ly = (int)(Math.Ceiling(cloneRectF.Y));
            if (ly + cloneRectF.Height > bmSize.Height)
            {
                cloneRectF.Height = bmSize.Height - ly;
            }

            return true;
        }

        private RectangleF TransformRectangle(RectangleF srcRec, Matrix matrix)
        {
            PointF[] pts = new PointF[] { new PointF(srcRec.X, srcRec.Y), 
                new PointF(srcRec.X + srcRec.Width, srcRec.Y + srcRec.Height) };
            matrix.TransformPoints(pts);
            RectangleF rec = RectangleF.Empty;
            rec.Location = pts[0];
            rec.Size = new SizeF((pts[1].X - pts[0].X), (pts[1].Y - pts[0].Y));
            return rec;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (userControlMutiDicomImages.Num <= 0 || userControlMutiDicomImages.SelectedIndex < 0)
            {
                return;
            }
            if (captureImagesNow != null)
            {
                if (captureImagesNow.Count != 0)
                {
                    ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
                    if (this.pictureBox1.Cursor == this.HandCursor)
                    {
                        float dx = e.X - movePoint.X;
                        float dy = e.Y - movePoint.Y;
                        if (dx == 0 && dy == 0) return;
                        myMatrix.Translate(dx, dy, MatrixOrder.Append);
                        movePoint = e.Location;
                        RefreshImage(img);
                    }
                    if (e.Button == MouseButtons.Left && flag)
                    {
                        assistantTool.MoveHandleTo(e.Location, 0);
                        RefreshROIInformation();
                        assistantTool.imgBase[selectROIImageIndex].rectType = "toolStripMenuItemModify";
                        RefreshImage(img);
                    }
                    else
                    {
                        if (tabControlCalibration.SelectedIndex == 0)
                        {
                            RefreshPOIInformation(e.Location);
                            this.pictureBox1.Cursor = assistantTool.DetectRectangle((float)e.X, (float)e.Y, false);
                        }
                    }
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.pictureBox1.Cursor != Cursors.Default)
            {
                flag = true;
                mouseDownPoint = e.Location;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    this.Cursor = Cursors.Default;
            //}
            flag = false;
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (captureImagesNow.Count <= 0 || userControlMutiDicomImages.SelectedIndex >= captureImagesNow.Count)
            {
                return;
            }
            float scale = myMatrix.Elements[0];
            bool toBig = (e.Delta > 0);
            if ((toBig && scale >= ScaleMax) || (!toBig && scale <= ScaleMin))
            {
                return; //超过缩放限制后不再继续
            }
            float k = toBig ? 1.1f : 0.9f;
            float tempScale = scale * k;
            if (tempScale > ScaleMax)
            {
                tempScale = ScaleMax;
            }
            else if (tempScale < ScaleMin)
            {
                tempScale = ScaleMin;
            }
            //float defaultScale = Math.Min(imgPictureBox.Width / (float)ctWidth, imgPictureBox.Height / (float)ctHeight);
            PointF pCenter = new PointF((float)e.X, (float)e.Y);
            Matrix mxInvert = (Matrix)myMatrix.Clone();
            mxInvert.Invert();
            PointF[] pts = new PointF[1] { pCenter };
            mxInvert.TransformPoints(pts);
            pCenter = pts[0];
            if (pCenter.IsEmpty) return;
            pts = new PointF[1] { pCenter };
            myMatrix.TransformPoints(pts);
            PointF pt1 = pts[0];
            myMatrix.Scale(k, k);
            pts[0] = pCenter;
            myMatrix.TransformPoints(pts);
            PointF pt2 = pts[0];
            float dx = pt1.X - pt2.X;
            float dy = pt1.Y - pt2.Y;
            myMatrix.Translate(dx, dy, MatrixOrder.Append);
            ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
            RefreshImage(img);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private void SaveasHisFile(string hisPath,List<ushort[,]> dataList,int intergrationTime)
        {
            int NrOfFrames = dataList.Count;
            int row = dataList[0].GetLength(0);
            int col = dataList[0].GetLength(1);
            HisHeader h = new HisHeader();
            h.FileID = 0x7000;
            h.HeaderSize = 68;
            h.HeadVerdion = 100;
            h.FileSize = (uint)(32 + 68 + NrOfFrames * row * col * sizeof(ushort));
            h.ImageHeaderSize = 32;
            h.ULX = 1;
            h.BRX = (short)col;
            h.ULY = 1;
            h.BRY = (short)row;
            h.NrOfFrames = (short)NrOfFrames;
            h.Correction = 1;
            try
            {
                h.IntegrationTime = (uint)(intergrationTime * 1000);
            }
            catch (System.Exception ex)
            {
                h.IntegrationTime = 1000 * 1000;
            }
            h.TypeOfNumbers = 4;
            h.X = new byte[34];
            h.ImageHeader = new byte[32];
            HisObject.SaveDataToHIS(ref h, dataList, hisPath);
        }
        //private void pictureBox1_MouseLeave(object sender, System.EventArgs e)
        //{
        //    lbLocationX.Text = "";
        //    lbLocationY.Text = "";
        //    lbGrayValue.Text = "";
        //}
        private void RefreshROIInformation()
        {
            if (captureImagesNow != null)
            {
                if (captureImagesNow.Count != 0)
                {
                    //lbSize.Text = (captureImagesNow[selectROIImageIndex].ROIRectangle.Width.ToString()
                    //+ " * " + captureImagesNow[selectROIImageIndex].ROIRectangle.Height.ToString()).PadLeft(10);
                    textBox1.Text = captureImagesNow[selectROIImageIndex].ROIMeanValue.ToString("F2");
                    textBox2.Text = captureImagesNow[selectROIImageIndex].ROIVar.ToString("F2");
                    textBox3.Text = captureImagesNow[selectROIImageIndex].ROIDiffNumber.ToString();
                    //SNR =  captureImagesNow[selectROIImageIndex].ROIMeanValue / //Ave / (Math.Sqrt(Variance / (imgROI.ImageData.GetLength(0) * imgROI.ImageData.GetLength(1))));
                    SNRlabel.Text = captureImagesNow[selectROIImageIndex].SNR.ToString("F2");

                    if (ImageNumberNow <= 0)//2012.1.11，ml
                    {
                        SetContextMenuStrip();
                        userControlMutiDicomImages.DrawImagesInPane(new List<Bitmap>(), false);
                        pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        Graphics g = Graphics.FromImage(pictureBox1.Image);
                        g.Clear(Color.Black);
                        g.Dispose();
                        histogramControl1.SetCenterWindow(0, 0);
                        histogramControl1.RefreshHistogram(null);
                        Refresh();
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        SNRlabel.Text = "";
                    }
                }

            }

        }
        private void RefreshPOIInformation(Point mousePoint)
        {
            if (ImageNumberNow <= 0)//2012.1.11，ml
            {
                return;
            }
            Matrix mxInvert = (Matrix)myMatrix.Clone();
            mxInvert.Invert();
            Point[] ps = new Point[] { mousePoint };
            mxInvert.TransformPoints(ps);
            int index = userControlMutiDicomImages.SelectedIndex;
            ushort[,] data = captureImagesNow[index].ImageData;
            if (ps[0].X >= 0 && ps[0].Y >= 0 && ps[0].X < data.GetLength(1) && ps[0].Y < data.GetLength(0))
            {
                labelAxis.Text = "X=" + ps[0].X + ",Y=" + ps[0].Y;
                //this.labelAxis.Text = "X=" + ps[0].X.ToString() + "  " + "Y=" + ps[0].Y.ToString();
                this.labelImageValue.Text = "Value:" + data[ps[0].Y, ps[0].X].ToString();
                //lbLocationY.Text = "   " + ps[0].Y.ToString();
            }
            else
            {
                this.labelAxis.Text = ""; //"X:" + ps[0].X.ToString() + "  " + "Y:" + ps[0].Y.ToString();
                this.labelImageValue.Text = ""; //data[ps[0].Y, ps[0].X].ToString();
                //this.labelImageValue.Text = "X:" + "   " + "Y:" + "   ";
            }
        }

        private void graphicButton1_Click(object sender, EventArgs e)
        {
            //List<ImageROI> imageList = new List<ImageROI>();
            //foreach (int i in userControlMutiDicomImages.SelectList)
            //{
            //    imageList.Add(captureImagesNow[i]);
            //}
            //if (imageList.Count <= 1)
            //{
            //    cls_MessageBox.Show("Please select images more than 2.");
            //    return;
            //}

            //string detectionAreaString = CapturePub.readCaptrueValue("LinearDetectionArea", false);
            //int detectionArea = 15;
            //if (!int.TryParse(detectionAreaString, out detectionArea))
            //{
            //    detectionArea = 15;
            //}

            //SortedDictionary<double, double> muValues = new SortedDictionary<double, double>();
            //for (int i = 0; i < imageList.Count; i++)
            //{
            //    ushort[,] imgData = imageList[i].imageData;
            //    int radius = (int)((detectionArea * 10 / imageList[i].pixelSize) / 2.0F);
            //    if (radius <= 0 || radius >= imgData.GetLength(0) / 2)
            //    {
            //        radius = 500;
            //    }
            //    double v = 0;
            //    int upRow = (int)(imageList[i].centerY - radius);
            //    if (upRow < 0 || upRow >= imgData.GetLength(0))
            //    {
            //        upRow = 0;
            //    }
            //    int downRow = (int)(imageList[i].centerY + radius);
            //    if (downRow < 0 || downRow >= imgData.GetLength(0))
            //    {
            //        downRow = imgData.GetLength(0) - 1;
            //    }

            //    int leftCol = (int)(imageList[i].centerX - radius);
            //    if (leftCol < 0 || leftCol >= imgData.GetLength(1))
            //    {
            //        leftCol = 0;
            //    }
            //    int rightCol = (int)(imageList[i].centerX + radius);
            //    if (rightCol < 0 || rightCol >= imgData.GetLength(1))
            //    {
            //        rightCol = imgData.GetLength(1) - 1;
            //    }


            //    for (int row = upRow; row <= downRow; row++)
            //    {
            //        for (int col = leftCol; col <= rightCol; col++)
            //        {
            //            v += imgData[row, col];
            //        }
            //    }
            //    if (!muValues.ContainsKey(imageList[i].mu))
            //    {
            //        muValues.Add(imageList[i].mu, v);
            //    }
            //}

            //double[] mus = new double[muValues.Count];
            //double[] imgVs = new double[muValues.Count];
            //muValues.Keys.CopyTo(mus, 0);
            //muValues.Values.CopyTo(imgVs, 0);
            //double xMin = mus[0];
            //double xMax = mus[mus.Length - 1];
            //double yMin = double.MaxValue;
            //double yMax = double.MinValue;
            //foreach (double v in imgVs)
            //{
            //    if (v < yMin)
            //    {
            //        yMin = v;
            //    }
            //    if (v > yMax)
            //    {
            //        yMax = v;
            //    }
            //}

            //ZedGraphImageCapturing.GraphPane OARPaneX = zedGraphControl1.GraphPane;
            //OARPaneX.XAxis.Scale.Max = xMax + 1;
            //OARPaneX.XAxis.Scale.Min = xMin - 1;
            //OARPaneX.XAxis.Scale.MajorStep = 1;

            //OARPaneX.YAxis.Scale.Min = (yMin - 5);
            //OARPaneX.YAxis.Scale.Max = (yMax + 5);
            //OARPaneX.YAxis.Scale.MajorStep = 5;



            //OARPaneX.CurveList.Clear();
            //OARPaneX.AddCurve("", mus, imgVs, System.Drawing.Color.Red, ZedGraphImageCapturing.SymbolType.None);
            ////RefreshImage();
            //zedGraphControl1.Refresh();

            //Form1 ctrl = new Form1(imageList, zedGraphControl1);
            //ctrl.ShowDialog();
        }

        private void SetRectangle(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            int left = 0;
            int top = 0;
            int width = 0;
            int height = 0;
            string name = "";
            if (captureImagesNow != null)
            {
                if (captureImagesNow.Count != 0)
                {
                    switch (item.Name)
                    {
                        case "toolStripMenuItemAll":
                            name = item.Name;
                            assistantTool.imgBase[selectROIImageIndex].rectType = "toolStripMenuItemAll";
                            left = 0;
                            top = 0;
                            width = 1024;
                            height = 1024;
                            break;
                        case "toolStripMenuItemDefault":
                            name = item.Name;
                            assistantTool.imgBase[selectROIImageIndex].rectType = "toolStripMenuItemDefault";
                            left = assistantTool.imgBase[selectROIImageIndex].BoundaryRect.Left;
                            top = assistantTool.imgBase[selectROIImageIndex].BoundaryRect.Top;
                            width = assistantTool.imgBase[selectROIImageIndex].BoundaryRect.Width;
                            height = assistantTool.imgBase[selectROIImageIndex].BoundaryRect.Height;
                            break;
                        case "toolStripMenuItemFive":
                            name = item.Name;
                            assistantTool.imgBase[selectROIImageIndex].rectType = "toolStripMenuItemFive";
                            left = (int)(assistantTool.imgBase[selectROIImageIndex].centerX - 25 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            top = (int)(assistantTool.imgBase[selectROIImageIndex].centerY - 25 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            width = (int)(50 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            height = (int)(50 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            break;
                        case "toolStripMenuItemTen":
                            assistantTool.imgBase[selectROIImageIndex].rectType = "toolStripMenuItemTen";
                            name = item.Name;
                            left = (int)(assistantTool.imgBase[selectROIImageIndex].centerX - 50 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            top = (int)(assistantTool.imgBase[selectROIImageIndex].centerY - 50 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            width = (int)(100 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            height = (int)(100 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            break;
                        case "toolStripMenuItemTwenty":
                            name = item.Name;
                            assistantTool.imgBase[selectROIImageIndex].rectType = "toolStripMenuItemTwenty";
                            left = (int)(assistantTool.imgBase[selectROIImageIndex].centerX - 100 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            top = (int)(assistantTool.imgBase[selectROIImageIndex].centerY - 100 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            width = (int)(200 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            height = (int)(200 / assistantTool.imgBase[selectROIImageIndex].pixelSize);
                            break;
                        default:
                            break;
                    }
                    for (int i = 0; i < contextMenuStripRectSet.Items.Count; i++)
                    {
                        if (contextMenuStripRectSet.Items[i].Name == name)
                        {
                            ((ToolStripMenuItem)contextMenuStripRectSet.Items[i]).Checked = true;
                        }
                        else
                        {
                            ((ToolStripMenuItem)contextMenuStripRectSet.Items[i]).Checked = false;
                        }
                    }
                    Rectangle rect = new Rectangle(left, top, width, height);
                    assistantTool.SetRectAngle(rect);
                    assistantTool.GetStatisticsInfomation();
                    RefreshROIInformation();
                    RefreshImage(captureImagesNow[userControlMutiDicomImages.SelectedIndex]);
                }
            }
        }
        private void contextMenuStripRectSet_Opening(object sender, CancelEventArgs e)
        {
            if (captureImagesNow != null)
            {
                if (captureImagesNow.Count != 0)
                {
                    for (int i = 0; i < contextMenuStripRectSet.Items.Count; i++)
                    {
                        string name = assistantTool.imgBase[selectROIImageIndex].rectType;
                        if (contextMenuStripRectSet.Items[i].Name == name)
                        {
                            ((ToolStripMenuItem)contextMenuStripRectSet.Items[i]).Checked = true;
                        }
                        else
                        {
                            ((ToolStripMenuItem)contextMenuStripRectSet.Items[i]).Checked = false;
                        }
                    }
                }
            }
        }
    }
}
