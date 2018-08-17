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
using System.Runtime.InteropServices;

namespace ImageCapturing
{
    public partial class FormGainSeqCapture : Form
    {

        #region Class Members

        private List<ImageObject> captureImagesNow = new List<ImageObject>();
        ushort[,] pixelMapData = null;
        private TriggerBase Trigger = null;
        private CaptureBase ImgCapture = null;
        private ContextMenuStrip ImageMenuStrip = null;

        ImageObject imgROI = new ImageObject();
        Histogram_Data his = new Histogram_Data();
        ImgBase imgShow = new ImgBase();

        private System.Timers.Timer timerStatus = new System.Timers.Timer();

        private Bitmap linkImage;
        private Bitmap grayImage;
        #endregion

        #region Constructors

        public FormGainSeqCapture(CaptureBase capture)
        {
            InitializeComponent();
            this.userControlMutiDicomImages.SignImg = TIcon.Change4dDisplay;
            imgShow.Dock = DockStyle.Fill;
            this.panelMain.Controls.Add(imgShow);
            try
            {
                string s = CapturePub.readCaptrueValue("FORM_SIZE", false);
                if (s != "")
                {
                    string[] s_ = s.Split(',');
                    this.Size = new Size(int.Parse(s_[0]), int.Parse(s_[1]));
                }
            }
            catch
            { }

            ImgCapture = capture;
            Trigger = capture.GetTrigger();
            ImgCapture.RefreshHostHandle(this.Handle);

            int size = pictureBox1.Size.Height;
            linkImage = new Bitmap(size, size);
            grayImage = new Bitmap(size, size);
            Graphics g = Graphics.FromImage(linkImage);
            int radis = (int)((size - 2) / 2.0F);
            g.DrawEllipse(Pens.LightSteelBlue, new Rectangle(size / 2 - radis, size / 2 - radis, radis * 2, radis * 2));
            g.FillEllipse(Brushes.Green, new Rectangle(size / 2 - radis, size / 2 - radis, radis * 2, radis * 2));
            g = Graphics.FromImage(grayImage);
            //g.Clear(Color.Black);
            g.DrawEllipse(Pens.LightSteelBlue, new Rectangle(size / 2 - radis, size / 2 - radis, radis * 2, radis * 2));
            g.FillEllipse(Brushes.Gray, new Rectangle(size / 2 - radis, size / 2 - radis, radis * 2, radis * 2));

            timerStatus.Interval = 500;
            timerStatus.Elapsed -= new System.Timers.ElapsedEventHandler(timerStatus_Elapsed);
            timerStatus.Elapsed += new System.Timers.ElapsedEventHandler(timerStatus_Elapsed);
            timerStatus.Start();


            this.Shown -= new EventHandler(FormCapture_Shown);
            this.Shown += new EventHandler(FormCapture_Shown);
            GraphicButton.ShowButtonToolTip -= new GraphicButton.ShowButtonTooltip(GraphicButton_ShowButtonToolTip);
            GraphicButton.ShowButtonToolTip += new GraphicButton.ShowButtonTooltip(GraphicButton_ShowButtonToolTip);
            GraphicButton.LeaveButton -= new GraphicButton.ShowButtonTooltip(GraphicButton_LeaveButton);
            GraphicButton.LeaveButton += new GraphicButton.ShowButtonTooltip(GraphicButton_LeaveButton);
            userControlMutiDicomImages.MayCancelSelect = true;
        }

        private void FormCapture_Shown(object sender, EventArgs e)
        {

            //先显示出默认界面内容
            SetContextMenuStrip();
            userControlMutiDicomImages.DrawImagesInPane(new List<Bitmap>(), false);
            Refresh();
            Show();
            //刷新数据在界面显示
            userControlMutiDicomImages.ImageNumChanged -= new UserControlMutiImages.EventChanged(userControlMutiDicomImages_ImageNumChanged);
            userControlMutiDicomImages.ImageNumChanged += new UserControlMutiImages.EventChanged(userControlMutiDicomImages_ImageNumChanged);
            this.KeyUp -= new KeyEventHandler(FormCapture_KeyUp);
            this.KeyUp += new KeyEventHandler(FormCapture_KeyUp);
            his.LUTChanged -= new Histogram_Data.LUTChangedDelegate(his_LUTChanged);
            his.LUTChanged += new Histogram_Data.LUTChangedDelegate(his_LUTChanged);

            Refresh();
            comboBoxGainMode_Image.SelectedIndex = comboBoxGainMode_Image.Items.Count - 1;
            ImgCapture.Ready();

            imgShow.PositionAndValue -= new ImgBase.PositionAndValueDelegate(ImgShow_PositionAndValue);
            imgShow.PositionAndValue += new ImgBase.PositionAndValueDelegate(ImgShow_PositionAndValue);
        }

        void timerStatus_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!Disposing)
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new System.Timers.ElapsedEventHandler(timerStatus_Elapsed), new object[] { sender, e });
                }
                else
                {
                    bool link = ImgCapture.LinkStatus == PanelLinkStatus.LINK_SUCCESS;
                    System.Drawing.Image img = link ? linkImage : grayImage;
                    this.pictureBox1.BackgroundImage = img;
                }
            }
        }


        ToolStripSeparator separator1 = new ToolStripSeparator();
        ToolStripMenuItem selectAllImages;
        ToolStripMenuItem showPixelMap;
        private void SetContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            selectAllImages = new ToolStripMenuItem("Select all to create gain sequence", null, SelectAll_Click);
            ToolStripMenuItem deleteOneImage = new ToolStripMenuItem("Remove current image", null, Delete_Click);
            deleteOneImage.Name = "DELETE";
            showPixelMap = new ToolStripMenuItem("Show pixel map", null, showPixelMap_Click);
            showPixelMap.Name = "PixelMap";
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { selectAllImages, separator1, deleteOneImage,new ToolStripSeparator(), showPixelMap});
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
            if (ImgCapture is CapturePKI)
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

            timerStatus.Elapsed -= new System.Timers.ElapsedEventHandler(timerStatus_Elapsed);
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
                case Keys.H:
                    {
                        imgShow.ShowHistogram = !imgShow.ShowHistogram;
                        imgShow.RefreshView();
                    }
                    break;
            }
        }

        public List<Bitmap> GetSmallBitmapList(List<ImageObject> imageList)
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
            showPixelMap.Checked = imgShow.ShowPixelMap;

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

        private void Delete_Click(object sender, EventArgs e)
        {
            int originalIndex = userControlMutiDicomImages.SelectedIndex;
            if (originalIndex < 0 || captureImagesNow.Count <= 0)
            {
                return;
            }
            Cursor = Cursors.WaitCursor;

            ImageObject imgROI = captureImagesNow[originalIndex];
            captureImagesNow.RemoveAt(originalIndex);
            if (userControlMutiDicomImages.SelectList.Contains(originalIndex))
            {
                userControlMutiDicomImages.SelectList.Remove(originalIndex);
            }
            for (int k = 0; k < userControlMutiDicomImages.SelectList.Count; k++)
            {
                if (userControlMutiDicomImages.SelectList[k] > originalIndex)
                {
                    userControlMutiDicomImages.SelectList[k] = userControlMutiDicomImages.SelectList[k] - 1;
                }
            }
            userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow), originalIndex);
            Cursor = Cursors.Default;

            if (userControlMutiDicomImages.Num <= 0)
            {

            }
        }

        private void showPixelMap_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item == null) return;
            item.Checked = !item.Checked;
            imgShow.ShowPixelMap = item.Checked;
            imgShow.RefreshView();
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        void userControlMutiDicomImages_ImageNumChanged(object Sender, EventArgs e)
        {
            if (captureImagesNow.Count <= 0)
            {
                return;
            }
            pixelMapData = null;
            int index = userControlMutiDicomImages.SelectedIndex;
            imgROI = captureImagesNow[index];

            pixelMapData = new ushort[imgROI.ImageData.GetLength(0), imgROI.ImageData.GetLength(1)];
            int Average = 0;
            long SumValue = 0;
            string xmlValue = CapturePub.readCaptrueValue("UNIT");
            string unit = (xmlValue == "" ? "4" : xmlValue);
            int unitlength = int.Parse(unit);
            xmlValue = CapturePub.readCaptrueValue("Coefficient");
            string coefficient = (xmlValue == "" ? "0.9" : xmlValue);
            float percentage = float.Parse(coefficient);
            for (int i = 0; i < imgROI.ImageData.GetLength(0) / unitlength; i++)
            {
                for (int j = 0; j < imgROI.ImageData.GetLength(1) / unitlength; j++)
                {
                    Average = 0;
                    SumValue = 0;
                    for (int ii = 0; ii < unitlength; ii++)
                    {
                        for (int jj = 0; jj < unitlength; jj++)
                        {
                            SumValue += imgROI.ImageData[i * unitlength + ii, j * unitlength + jj];
                        }
                    }
                    Average = (int)(((double)SumValue / (unitlength * unitlength)) * percentage);
                    for (int ii = 0; ii < unitlength; ii++)
                    {
                        for (int jj = 0; jj < unitlength; jj++)
                        {
                            if (imgROI.ImageData[i * unitlength + ii, j * unitlength + jj] >= Average)
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

            his.ComputeHistogram(imgROI.ImageData);
            imgROI.GenerateBMP(his.LUT);
            imgShow.InitImageData(imgROI, his, pixelMapData);
            labelMedianValue.Text = "Ave:" + (int)his.AverageValue;
            labelWL.Text = "L=" + his.windowCenter + ",W=" + his.windowWidth;
        }

        #endregion

        #region Progress Events

        bool progressBeinFlag = false;

        public void ProgressBegin(string title, int maxStep)
        {
            progressBeinFlag = true;
            int CONST_PROGRESSTITLELEN = 150;
            int CONST_PROGRESSBARLEN = 300;

            Graphics g = labelTitle.CreateGraphics();
            int strLen = (int)(g.MeasureString(title, labelTitle.Font).Width + 0.5) + 4;
            labelTitle.Width = (strLen >= CONST_PROGRESSTITLELEN) ? strLen : CONST_PROGRESSTITLELEN;
            if ((panelProgress.Width - panelProgress.Padding.Horizontal - labelTitle.Width) > CONST_PROGRESSBARLEN)
            {
                progressBar1.Width = CONST_PROGRESSBARLEN;
            }
            else
            {
                progressBar1.Width = panelProgress.Width - panelProgress.Padding.Horizontal - labelTitle.Width;
            }
            progressBar1.Left = panelProgress.Padding.Horizontal + labelTitle.Width;
            labelTitle.Left = panelProgress.Padding.Left;
            labelTitle.Text = title;
            labelTitle.TextAlign = ContentAlignment.MiddleLeft;
            labelTitle.Visible = true;
            if (maxStep > 1)
            {
                progressBar1.Maximum = maxStep;
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                this.progressBar1.Top = this.panelProgress.Padding.Top;
                progressBar1.Height = this.panelProgress.Height - this.panelProgress.Padding.Vertical;
            }
            g.Dispose();
            //this.panelProgress.Visible = true;
            panelProgress.Refresh();
        }

        public void ProgressAdd(int step)
        {
            int v = progressBar1.Value + step;
            if (v > progressBar1.Maximum)
            {
                v = progressBar1.Maximum;
            }
            progressBar1.Value = v;
        }

        public void ProgressEnd()
        {
            progressBar1.Visible = false;
            labelTitle.Text = "";
            labelTitle.Visible = false;
            //this.panelProgress.Visible = false;

            progressBeinFlag = false;
        }

        void GraphicButton_ShowButtonToolTip(GraphicButton GB)
        {
            if (!progressBeinFlag && GB.Hint != "" && labelTitle.Text != GB.Hint)
            {
                Graphics g = labelTitle.CreateGraphics();
                int strLen = (int)(g.MeasureString(GB.Hint, labelTitle.Font).Width + 0.5) + 4;
                labelTitle.Width = strLen;

                labelTitle.Left = panelProgress.Padding.Left;
                labelTitle.Text = GB.Hint;
                labelTitle.TextAlign = ContentAlignment.MiddleLeft;
                labelTitle.Visible = true;
                g.Dispose();
            }
        }

        void GraphicButton_LeaveButton(GraphicButton GB)
        {
            if (!progressBeinFlag)
            {
                labelTitle.Text = "";
                labelTitle.Visible = false;
            }
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
                    switch ((PanelCaptureMode)m.WParam)
                    {
                        case PanelCaptureMode.DoubleExposure:
                            imgs.AddRange(ImgCapture.imgList);
                            ImgCapture.imgList.Clear();
                            RefreshDataDisplay(imgs, true, (int)m.LParam);
                            break;
                        case PanelCaptureMode.Sequence:
                        case PanelCaptureMode.Single:
                            imgs.AddRange(ImgCapture.imgList);
                            ImgCapture.imgList.Clear();
                            RefreshDataDisplay(imgs, false, (int)m.LParam);
                            break;
                        case PanelCaptureMode.Continuous:
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

                if (ImgCapture is CapturePKI)
                {
                    CapturePKI imgCapturePKI = ImgCapture as CapturePKI;

                    imgCapturePKI.pki_config.gianMode = (CapturePKI.ElectricCapacityMode)comboBoxGainMode_Image.SelectedIndex;
                    //imgCapturePKI.FrameCount = 1;
                    imgCapturePKI.RefreshAcquisitionStructureParameters();
                    imgCapturePKI.pki_config.imageCorrection = CapturePKI.ImageCorrection.Offset;
                }
                if (this.ImgCapture is CaptureCareRay)
                {
                    this.ImgCapture.SetCaptureMode(PanelCaptureMode.Single);
                    (this.ImgCapture as CaptureCareRay).SetCorrection(true);
                }
                ImgCapture.Start();
            }
        }

        private void btnLoadLocalFile_Click(object sender, EventArgs e)
        {
            this.Refresh();
            CaptureBase bakCapture = new CaptureLocalFile();
            //bakCapture.RefreshHostHandle(this.Handle);
            bakCapture.CaptureImageData();
            RefreshDataDisplay(bakCapture.imgList, false, 0);
            //if (bakCapture.imgList.Count > 0)
            //{
            //    imgROI = bakCapture.imgList[0];
            //    his.ComputeHistogram(imgROI.ImageData);
            //    imgROI.GenerateBMP(his.LUT);
            //    imgShow.InitImageData(imgROI, his);
            //    labelMedianValue.Text = "Ave:" + (int)his.AverageValue;
            //    labelWL.Text = "L=" + his.windowCenter + ",W=" + his.windowWidth;
            //}
        }

        private void SetInterfacebyCapture(bool capturingFlag)
        {
            btnCreatePixelMap.Enabled = !capturingFlag;
            btnBuildGainSeq_Image.Enabled = !capturingFlag;
            btnBuildAverage_Image.Enabled = !capturingFlag;
            btnLocalImage_Image.Enabled = !capturingFlag;
            btnCapture_Image.Image = !capturingFlag ? TIcon.panelCapture : TIcon.cancelPanelCapture;
        }

        private void RefreshDataDisplay(List<ImageObject> imageObjs, bool doubleExposure, int beamon_num)
        {
            if (imageObjs.Count > 0)
            {
                his.ComputeHistogram(imageObjs[0].ImageData);
                writeImageToDisk("D:\\img.raw", imageObjs[0].ImageData);
                imageObjs[0].GenerateBMP(his.LUT);
                captureImagesNow.AddRange(imageObjs);
                userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow), true);
            }
        }

        private void btnBuildGainSeq_Click(object sender, EventArgs e)
        {
            if (tbDoseRate_Image.Text.Trim() == "")
            {
                cls_MessageBox.Show("Please input dose rate.");
                return;
            }

            List<ImageObject> images = new List<ImageObject>();
            foreach (int i in userControlMutiDicomImages.SelectList)
            {
                images.Add(captureImagesNow[i]);
            }
            if (images.Count <= 0)
            {
                cls_MessageBox.Show("Please select images.");
                return;
            }



            if (this.ImgCapture is CaptureCareRay)
            {
                //string path = new string((this.ImgCapture as CaptureCareRay).cal_params.gain_image_dir);
                string path = @"D:\CareRayCalImgs\C12140510-003\GainImage\";
                //Console.WriteLine(path);
                //path = path.Replace("\\","\\\\");
                Console.WriteLine(path);
                if (!Directory.Exists(path))
                {
                    //Console.WriteLine("delete folder");
                    //Directory.Delete(path, true);
                    Console.WriteLine("create folder");
                    Directory.CreateDirectory(path);
                }
                
                for (int index = 0; index < images.Count; ++index)
                    this.writeImageToDisk(path + (object)(index + 1) + ".raw", images[index].ImageData);
                int index1 = CareRayInterface.CR_linatech_calibration();
                if (index1 != 0)
                {
                    Console.WriteLine("CR_linear_calibration error,  reason: " + CareRayErrors.CrErrStrList(index1));
                }
                else
                {
                    CareRayInterface.ExpProgress expProg = new CareRayInterface.ExpProgress();
                    do
                    {
                        int result = CareRayInterface.CR_query_prog_info((int)CareRayInterface.ProgType.CR_CAL_PROG, ref expProg);
                        if (0 == result)
                        {
                            if (1 == expProg.calComplete)
                            {
                                Console.WriteLine("gain cal successfully");
                                break;
                            }
                            if (0 != expProg.errorCode)
                            {
                                result = expProg.errorCode;
                                Console.WriteLine("fail to gain");
                                break;
                            }
                        }
                    } while (true);
                }
            }

            return;


            string sortgainfolder = CapturePub.SaveDFPath;// +DateTime.Now.ToString("yyyy.MM.dd.HHmmss");
            CapturePub.saveCaptrueValue(XmlField.SortGainFolder, sortgainfolder);
            string gainFolder = sortgainfolder + "\\gain";
            if (!Directory.Exists(gainFolder))
            {
                Directory.CreateDirectory(gainFolder);
            }
            if (!Directory.Exists(CapturePub.SaveDFPath+"\\offset"))
            {
                Directory.CreateDirectory(CapturePub.SaveDFPath+"\\offset");
            }
            images.Sort();
            List<ushort[,]> dataList = new List<ushort[,]>();
            foreach (ImageObject obj in images)
            {
                dataList.Add(obj.ImageData);
            }
            string doserate = tbDoseRate_Image.Text.Trim();
            string gainFile = "gain_" + DateTime.Now.ToString("yyyy.MM.dd.HHmmss")
                            + "_" + doserate + "cGy(min)_"
                            + getGainString((CapturePKI.ElectricCapacityMode)comboBoxGainMode_Image.SelectedIndex)
                            + "_" + dataList[0].GetLength(0) + "x" + dataList[0].GetLength(1) + ".his";
            string gain_fullPath = gainFolder + "\\" + gainFile;
            SaveasHisFile(gain_fullPath, dataList, 1000);
            CapturePub.saveCaptrueValue(XmlField.GainSeqFile_Image, gain_fullPath);

            if (ImgCapture is CapturePKI)
            {
                (ImgCapture as CapturePKI).SetLinkCorrection();
            }
            cls_MessageBox.Show("Create gain sequence successfully!");
        }


        private unsafe void writeImageToDisk(string fname, ushort[,] data)
        {

            if (ImgCapture is CaptureCareRay)
            {
                Console.Write("save file to ..." + fname);
                CaptureCareRay imgcapture = ImgCapture as CaptureCareRay;
                fixed (ushort* numPtr = &data[0, 0])
                {
                    FileStream fileStream = new FileStream(fname, FileMode.OpenOrCreate, FileAccess.Write);
                    byte[] numArray = new byte[data.Length * 2 + imgcapture.Headsize];
                    ushort* acq = (ushort*)imgcapture.pAcqBuffer;
                    Marshal.Copy((IntPtr)((void*)acq), numArray, 0, imgcapture.Headsize);
                    Marshal.Copy((IntPtr)((void*)numPtr), numArray, imgcapture.Headsize, numArray.Length - imgcapture.Headsize);
                    fileStream.Write(numArray, 0, numArray.Length);
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
        }

        string getGainString(CapturePKI.ElectricCapacityMode gain)
        {
            string s = "8pF";
            switch (gain)
            {
                case CapturePKI.ElectricCapacityMode.Gain_0o25pF:
                    s = "0.25pF";
                    break;
                case CapturePKI.ElectricCapacityMode.Gain_0o5pF:
                    s = "0.5pF";
                    break;
                case CapturePKI.ElectricCapacityMode.Gain_1pF:
                    s = "1pF";
                    break;
                case CapturePKI.ElectricCapacityMode.Gain_2pF:
                    s = "2pF";
                    break;
                case CapturePKI.ElectricCapacityMode.Gain_4pF:
                    s = "4pF";
                    break;
                case CapturePKI.ElectricCapacityMode.Gain_8pF:
                    s = "8pF";
                    break;
            }
            return s;
        }

        private void btnAverageGain_Click(object sender, EventArgs e)
        {
            //if (this.ImgCapture is CaptureCareRay)
            //{
            //    int res = (ImgCapture as CaptureCareRay).performOffsetCalibration();
            //    if (res != 0)
            //    {
            //        Console.WriteLine("fail to offset");
            //    }
            //}
            //return;


            if (tbDoseRate_Image.Text.Trim() == "")
            {
                cls_MessageBox.Show("Please input dose rate.");
                return;
            }

            List<ImageObject> images = new List<ImageObject>();
            foreach (int i in userControlMutiDicomImages.SelectList)
            {
                images.Add(captureImagesNow[i]);
            }
            if (images.Count <= 0)
            {
                cls_MessageBox.Show("Please select images.");
                return;
            }

            string sortgainfolder = CapturePub.SaveDFPath + "\\" + DateTime.Now.ToString("yyyy.MM.dd.HHmmss");
            CapturePub.saveCaptrueValue(XmlField.SortGainFolder,sortgainfolder);
            string gainFolder = sortgainfolder + "\\gain";
            if (!Directory.Exists(gainFolder))
            {
                Directory.CreateDirectory(gainFolder);
            }

            List<ushort[,]> dataList = new List<ushort[,]>();
            ushort[,] averageData = new ushort[images[0].ImageData.GetLength(0), images[0].ImageData.GetLength(1)];
            for (int Row = 0; Row < averageData.GetLength(0); Row++)
            {
                for (int Col = 0; Col < averageData.GetLength(1); Col++)
                {
                    long num = 0;

                    foreach (ImageObject obj in images)
                    {
                        if (obj.ImageData.GetLength(0) == averageData.GetLength(0) && obj.ImageData.GetLength(1) == averageData.GetLength(1))
                        {
                            num += obj.ImageData[Row, Col];
                        }
                    }
                    averageData[Row, Col] = (ushort)(num / images.Count);
                }
            }
            dataList.Add(averageData);

            string doserate = tbDoseRate_Image.Text.Trim();
            string gainFile = "aveage_" + DateTime.Now.ToString("yyyy.MM.dd.HHmmss")
                            + "_" + doserate + "cGy(min)_"
                            + getGainString((CapturePKI.ElectricCapacityMode)comboBoxGainMode_Image.SelectedIndex)
                            + "_" + dataList[0].GetLength(0) + "x" + dataList[0].GetLength(1) + ".his";
            string gain_fullPath = gainFolder + "\\" + gainFile;
            SaveasHisFile(gain_fullPath, dataList, 1000);
            CapturePub.saveCaptrueValue(XmlField.GainSeqFile_Image, gain_fullPath);

            if (ImgCapture is CapturePKI)
            {
                (ImgCapture as CapturePKI).SetLinkCorrection();
            }

            cls_MessageBox.Show("Create average image as gain sequence successfully!");
        }

        private void btnCreatePixelMap_Click(object sender, EventArgs e)
        {
            if (pixelMapData == null)
            {
                cls_MessageBox.Show("No pixel map data.");
                return;
            }

            if (!Directory.Exists(CapturePub.SaveDFPath+"\\pixelmap"))
            {
                Directory.CreateDirectory(CapturePub.SaveDFPath+"\\pixelmap");
            }
            if (!Directory.Exists(CapturePub.SaveDFPath+"\\offset"))
            {
                Directory.CreateDirectory(CapturePub.SaveDFPath+"\\offset");
            }


            List<ushort[,]> dataList = new List<ushort[,]>();
            dataList.Add(pixelMapData);

            string pxlmapFile = "pxlmap_" + DateTime.Now.ToString("yyyy.MM.dd.HHmmss")
                            + "_" + getGainString((CapturePKI.ElectricCapacityMode)comboBoxGainMode_Image.SelectedIndex)
                            + "_" + dataList[0].GetLength(0) + "x" + dataList[0].GetLength(1) + ".his";
            string pxl_fullPath = CapturePub.SaveDFPath + "\\pixelmap\\" + pxlmapFile;
            SaveasHisFile(pxl_fullPath, dataList, 1000);
            CapturePub.saveCaptrueValue(XmlField.PixelMapFile, pxl_fullPath);

            if (ImgCapture is CapturePKI)
            {
                (ImgCapture as CapturePKI).SetLinkCorrection();
            }
            cls_MessageBox.Show("Create pixel map successfully!");
        }

        private void Input_KeyUp(object sender, KeyEventArgs e)
        {
            AMRT.MaskedTextBox temp = (sender as AMRT.MaskedTextBox);
            if (temp == null)
            {
                return;
            }
            temp.Focus();
        }

        private void SaveasHisFile(string hisPath, List<ushort[,]> dataList, int intergrationTime)
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

        void his_LUTChanged(Color[] LUT)
        {
            if (imgROI == null || imgROI.ImageData == null)
            {
                return;
            }

            imgROI.GenerateBMP(LUT);
            imgShow.RefreshView();
            labelWL.Text = "L=" + his.windowCenter + ",W=" + his.windowWidth;
        }


        void ImgShow_PositionAndValue(Point p, int v)
        {
            if (p == Point.Empty)
            {
                labelAxis.Text = "";
                labelImageValue.Text = "";
            }
            else
            {
                labelAxis.Text = "X=" + p.X + ",Y=" + p.Y;
                labelImageValue.Text = " Value:" + v;
            }
        }
    }
}
