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
    public partial class FormCaptureSetupHight : AMRT.BaseForm
    {

        #region Class Members
        private List<ImageObject> captureImagesNow = new List<ImageObject>();
        private TriggerBase Trigger = null;
        private CaptureBase ImgCapture = null;
        private ContextMenuStrip ImageMenuStrip = null;
        Matrix myMatrix = new Matrix();
        Bitmap srcBitmap = null;
        Point startP = Point.Empty;
        Point endP = Point.Empty;
        Point movePoint = Point.Empty;
        string CalibrationType = "NONE";
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

        public FormCaptureSetupHight(CaptureBase capture, TriggerBase trigger)
        {
            InitializeComponent();
            this.userControlMutiDicomImages.SignImg = TIcon.Change4dDisplay;
            TCursor.LoadAllCursors(System.Windows.Forms.Application.StartupPath);

            ImgCapture = capture;
            Trigger = trigger;
            ImgCapture.RefreshHostHandle(this.Handle);

            this.Shown -= new EventHandler(FormCapture_Shown);
            this.Shown += new EventHandler(FormCapture_Shown);

            bool showBeamButton = CapturePub.readCaptrueValue("ShowBeamButton", false) == "T";


            bool showCaptureButton = CapturePub.readCaptrueValue("HaveImagePanel") == "T";
            showCaptureButton = showCaptureButton || (CapturePub.readCaptrueValue("SimImagePanel") == "T");
            //btnCapture.Visible = showCaptureButton;
            //btnOverlay.Visible = showCaptureButton;
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
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.Black);
            g.Dispose();
            histogramControl1.SetCenterWindow(0, 0);
            histogramControl1.RefreshHistogram(null);
            //Setup Parameter
            string xmlValue = CapturePub.readCaptrueValue("SetupAngle");
            int idx = cbAngle.Items.IndexOf(xmlValue);
            if (idx < 0)
            {
                idx = 0;
            }
            cbAngle.SelectedIndex = idx;
            xmlValue = CapturePub.readCaptrueValue("SetupHeight");
            textBoxSetupHeight.Text = (xmlValue == "" ? "1600" : xmlValue);
            xmlValue = CapturePub.readCaptrueValue("SAD");
            textBoxSAD.Text = (xmlValue == "" ? "1000" : xmlValue);
            xmlValue = CapturePub.readCaptrueValue("PhysicsWidth");
            textBoxPhysicsWidth.Text = (xmlValue == "" ? "410" : xmlValue);
            xmlValue = CapturePub.readCaptrueValue("PhysicsHeight");
            textBoxPhysicsHeight.Text = (xmlValue == "" ? "410" : xmlValue);
            xmlValue = CapturePub.readCaptrueValue("ImageCenterX");
            textBoxImageCenterX.Text = (xmlValue == "" ? "0" : xmlValue);
            xmlValue = CapturePub.readCaptrueValue("ImageCenterY");
            textBoxImageCenterY.Text = (xmlValue == "" ? "0" : xmlValue);

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
            //tabControlCalibration_Selected(null,null);
            Refresh();
            ImgCapture.Ready();
        }
        ToolStripSeparator separator1 = new ToolStripSeparator();
        private void SetContextMenuStrip()
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem deleteOneImage = new ToolStripMenuItem("Remove", null, Delete_Click);
            deleteOneImage.Name = "DELETE";
            ToolStripMenuItem saveImage = new ToolStripMenuItem();//"Save as...", null, null);
            deleteOneImage.Name = "SaveAs";
            contextMenuStrip.Items.AddRange(new ToolStripItem[] { deleteOneImage, separator1, saveImage });
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


            //bool NeedSave = false;

            //if (NeedSave)
            //{
            //    string msg = "There are images without saving,are you sure to close?";
            //    DialogResult dr = cls_MessageBox.Show(msg, "Confirm", MessageBoxButtons.YesNo);
            //    if (dr != DialogResult.Yes)
            //    {
            //        e.Cancel = true;
            //        return;
            //    }
            //}
        }

        private void DisposeVariables()
        {
            if (ImgCapture != null)
            {
                ImgCapture.imgList.Clear();
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
            userControlMutiDicomImages.SelectAll(item.Text == "Select All to Save");
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            int originalIndex = userControlMutiDicomImages.SelectedIndex;
            if (originalIndex < 0 || captureImagesNow.Count <= 0)
            {
                return;
            }
            Cursor = Cursors.WaitCursor;
            captureImagesNow[originalIndex].Dispose();
            captureImagesNow[originalIndex] = null;
            captureImagesNow.RemoveAt(originalIndex);
            userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow), originalIndex);
            Cursor = Cursors.Default;
        }


        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        void userControlMutiDicomImages_ImageNumChanged(object Sender, EventArgs e)
        {
            int index = userControlMutiDicomImages.SelectedIndex;
            if (captureImagesNow[index] == null || captureImagesNow[index].imageData == null)
            {
                return;
            }
            ImageObject cImageB = captureImagesNow[index].Clone() as ImageObject;
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
            RefreshImage(cImageB);
            cImageB.Dispose();
            GC.Collect();
        }

        void cImage_LUTChanged(int ChangedType, object ChangedObject, Color[] LUT)
        {
            if (captureImagesNow.Count <= 0 || userControlMutiDicomImages.SelectedIndex >= captureImagesNow.Count)
            {
                return;
            }

            ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
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
            //progressBar.Left = panelProgress.Padding.Horizontal + labelTitle.Width;
            //labelTitle.Left = panelProgress.Padding.Left;
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

        string SaveFilePath;
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
                if (ImgCapture is CapturePKI)
                {
                    CapturePKI imgCapturePKI = ImgCapture as CapturePKI;
                    SaveFilePath = CapturePub.SaveDFPath + "\\setup" + DateTime.Now.ToString("yyyy.MM.dd.hhmmss");
                    //imgCapturePKI.pki_config.captureImageMode = CapturePKI.PanelCaptureMode.Sequence;
                    imgCapturePKI.DoseScaleMode = false;
                    imgCapturePKI.RefreshAcquisitionStructureParameters();
                }
                ImgCapture.Start();
            }
        }

        private void btnLoadLocalFile_Click(object sender, EventArgs e)
        {
            SaveFilePath = CapturePub.SaveDFPath + "\\setup" + DateTime.Now.ToString("yyyy.MM.dd.hhmmss");
            CaptureBase localCapture = new CaptureLocalFile();
            localCapture.CaptureImageData();
            RefreshDataDisplay(localCapture.imgList, false, 0);
        }

        private void SetInterfacebyCapture(bool capturingFlag)
        {
            btnLocal.Enabled = !capturingFlag;
            btnRuler.Enabled = !capturingFlag;
            btncenter.Enabled = !capturingFlag;
            btnDefaultCursor.Enabled = !capturingFlag;
            groupBox1.Enabled = !capturingFlag;
            btnCapture.Image = !capturingFlag ? TIcon.panelCapture : TIcon.cancelPanelCapture;
        }

        private void RefreshDataDisplay(List<ImageObject> imageObjs, bool doubleExposure, int beamon_num)
        {
            //ImgCapture.RotateImage(imageObjs);
            foreach (ImageObject imageObj in imageObjs)
            {
                ImgCapture.RotateImage(imageObj);
                ImageObject imgROI = ImageObject.GenerateImageROI(imageObj);
                imgROI.ImageData = imgROI.imageData;
                captureImagesNow.Add(imgROI);
                if (ImgCapture is CapturePKI)
                {
                    CapturePKI imgCapturePKI = ImgCapture as CapturePKI;
                    imgROI.SaveasHisFile(SaveFilePath, (int)imgCapturePKI.pki_config.integrationTime, imgCapturePKI.pki_config.imageCorrection);
                }
            }

            userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow), true);
            if (captureImagesNow.Count <= 0)
            {
                Bitmap bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(bm);
                g.Clear(Color.Black);
                g.Dispose();
                pictureBox1.Image = bm;
            }
        }

        private void btnRuler_Click(object sender, EventArgs e)
        {
            if (captureImagesNow.Count <= 0 || userControlMutiDicomImages.SelectedIndex >= captureImagesNow.Count)
            {
                return;
            }
            if (CalibrationType == "LENGTH")
            {
                CalibrationType = "NONE";
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = DistanceCursor;
                CalibrationType = "LENGTH";

            }
            ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
            RefreshImage(img);
        }

        private void btncenter_Click(object sender, EventArgs e)
        {
            if (captureImagesNow.Count <= 0 || userControlMutiDicomImages.SelectedIndex >= captureImagesNow.Count)
            {
                return;
            }
            if (CalibrationType == "CENTER")
            {
                this.Cursor = Cursors.Default;
                CalibrationType = "NONE";
            }
            else
            {
                this.Cursor = Cursors.Cross;
                CalibrationType = "CENTER";

            }
            ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
            RefreshImage(img);
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
            }
            if (cloneBMP != null) cloneBMP.Dispose();
            mxInvert.Dispose();
            g.Dispose();
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
            Point p = e.Location;
            Matrix mxInvert = (Matrix)myMatrix.Clone();
            mxInvert.Invert();
            Point[] ps = new Point[] { p };
            mxInvert.TransformPoints(ps);
            labelAxis.Text = "X=" + ps[0].X + ",Y=" + ps[0].Y;
            int index = userControlMutiDicomImages.SelectedIndex;
            ushort[,] data = captureImagesNow[index].ImageData;
            if (ps[0].X < 0 || ps[0].Y < 0 || ps[0].X >= data.GetLength(1) || ps[0].Y >= data.GetLength(0))
            {
                labelAxis.Text = "";
                labelImageValue.Text = "";
                return;
            }
            ushort v = data[ps[0].Y, ps[0].X];
            //labelImageValue.Text = v.ToString();
            labelImageValue.Text = " Value:" + v.ToString();

            if (e.Button == MouseButtons.Left && startP != Point.Empty && CalibrationType == "LENGTH")
            {
                ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
                Bitmap bmpTemp = ImageAfterZoom(img.LUT[0], srcBitmap);

                Graphics g = Graphics.FromImage(bmpTemp);
                Brush brush = new SolidBrush(Color.Red);
                Pen pen = new Pen(Color.Red, 1);
                Point[] Pt = new Point[] { new Point(startP.X, startP.Y) };
                myMatrix.TransformPoints(Pt);
                g.DrawLine(pen, Pt[0], e.Location);
                pictureBox1.Image = bmpTemp;
                g.Dispose();
                pictureBox1.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
                float dx = e.X - movePoint.X;
                float dy = e.Y - movePoint.Y;
                if (dx == 0 && dy == 0) return;
                myMatrix.Translate(dx, dy, MatrixOrder.Append);
                movePoint = e.Location;
                RefreshImage(img);
            }
            else if (CalibrationType == "CENTER")
            {
                ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
                Bitmap bmpTemp = ImageAfterZoom(img.LUT[0], srcBitmap);

                Graphics g = Graphics.FromImage(bmpTemp);
                Brush brush = new SolidBrush(Color.Red);
                Pen pen = new Pen(Color.Red, 1);
                g.DrawLine(pen, new Point(0, e.Location.Y), new Point(bmpTemp.Width, e.Location.Y));
                g.DrawLine(pen, new Point(e.Location.X, 0), new Point(e.Location.X, bmpTemp.Height));
                pictureBox1.Image = bmpTemp;
                g.Dispose();
                pictureBox1.Refresh();
            }

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                switch (CalibrationType)
                {
                    case "LENGTH":
                        Point p = e.Location;
                        Matrix mxInvert = (Matrix)myMatrix.Clone();
                        mxInvert.Invert();
                        Point[] ps = new Point[] { p };
                        mxInvert.TransformPoints(ps);
                        startP = new Point(ps[0].X, ps[0].Y);
                        break;
                    case "CENTER":
                        if (userControlMutiDicomImages.Num <= 0 || userControlMutiDicomImages.SelectedIndex < 0)
                        {
                            return;
                        }
                        ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];

                        Bitmap bmpTemp = ImageAfterZoom(img.LUT[0], srcBitmap);
                        Graphics g = Graphics.FromImage(bmpTemp);
                        Brush brush = new SolidBrush(Color.Red);
                        Pen pen = new Pen(Color.Red, 1);
                        Point pCenter = e.Location;
                        g.DrawLine(pen, new Point(e.Location.X - 10, e.Location.Y), new Point(e.Location.X + 10, e.Location.Y));
                        g.DrawLine(pen, new Point(e.Location.X, e.Location.Y - 10), new Point(e.Location.X, e.Location.Y + 10));
                        pictureBox1.Image = bmpTemp;
                        g.Dispose();
                        pictureBox1.Refresh();
                        break;
                    case "NONE":
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                movePoint = e.Location;
                this.Cursor = HandCursor;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                switch (CalibrationType)
                {
                    case "LENGTH":
                        {
                            if (captureImagesNow.Count <= 0 || userControlMutiDicomImages.SelectedIndex >= captureImagesNow.Count)
                            {
                                return;
                            }

                            ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
                            Point p = e.Location;
                            Matrix mxInvert = (Matrix)myMatrix.Clone();
                            mxInvert.Invert();
                            Point[] ps = new Point[] { p };
                            mxInvert.TransformPoints(ps);
                            endP = new Point(ps[0].X, ps[0].Y);
                            UserControlCalibrationLength ctrl = new UserControlCalibrationLength();
                            if (ShowBaseForm.ShowControlDialog(ctrl) == DialogResult.OK)
                            {
                                float len = ctrl.length;
                                float pixelLen = (float)Math.Sqrt((float)((startP.X - endP.X) * (startP.X - endP.X) + (startP.Y - endP.Y) * (startP.Y - endP.Y)));
                                float SAD = float.Parse(textBoxSAD.Text.Trim());
                                float panelSize = float.Parse(textBoxPhysicsWidth.Text.Trim());
                                float setupHeight = (float)((SAD * (panelSize / img.imageWidth) * pixelLen) / len);
                                setupHeight = float.Parse(setupHeight.ToString("0.0"));
                                if (cls_MessageBox.Show("Will you apply setup height " + setupHeight + "(mm)?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    textBoxSetupHeight.Text = setupHeight.ToString();
                                    CapturePub.saveCaptrueValue("SetupHeight", setupHeight.ToString());
                                }
                            }
                        }
                        break;
                    case "CENTER":
                        {
                            if (captureImagesNow.Count <= 0 || userControlMutiDicomImages.SelectedIndex >= captureImagesNow.Count)
                            {
                                return;
                            }
                            ImageObject img = captureImagesNow[userControlMutiDicomImages.SelectedIndex];
                            Point p = e.Location;
                            Matrix mxInvert = (Matrix)myMatrix.Clone();
                            mxInvert.Invert();
                            Point[] ps = new Point[] { p };
                            mxInvert.TransformPoints(ps);
                            Point originalPoint = new Point(ps[0].X, ps[0].Y);
                            Point imageCenter = new Point(512, 512);
                            float offsetX = (originalPoint.X - imageCenter.X) * (float.Parse(textBoxPhysicsWidth.Text.Trim()) / img.imageWidth); // panel上的偏移距离
                            float offsetY = (originalPoint.Y - imageCenter.Y) * (float.Parse(textBoxPhysicsWidth.Text.Trim()) / img.imageWidth);
                            offsetX = float.Parse(offsetX.ToString("0.0"));
                            offsetY = float.Parse(offsetY.ToString("0.0"));
                            if (cls_MessageBox.Show("Will you apply center offset value X:" + offsetX + ",Y:" + offsetY + "(mm)?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                textBoxImageCenterX.Text = offsetX.ToString();
                                CapturePub.saveCaptrueValue("ImageCenterX", offsetX.ToString());
                                textBoxImageCenterY.Text = offsetY.ToString();
                                CapturePub.saveCaptrueValue("ImageCenterY", offsetY.ToString());
                            }
                        }
                        break;
                    case "NONE":
                        break;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (CalibrationType == "LENGTH")
                {
                    this.Cursor = DistanceCursor;
                }
                else if (CalibrationType == "CENTER")
                {
                    this.Cursor = Cursors.Cross;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
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
            Bitmap bmpTemp = ImageAfterZoom(img.LUT[0], srcBitmap);
            if (CalibrationType == "CENTER")
            {
                Graphics g = Graphics.FromImage(bmpTemp);
                Brush brush = new SolidBrush(Color.Red);
                Pen pen = new Pen(Color.Red, 1);
                g.DrawLine(pen, new Point(0, e.Location.Y), new Point(bmpTemp.Width, e.Location.Y));
                g.DrawLine(pen, new Point(e.Location.X, 0), new Point(e.Location.X, bmpTemp.Height));
                g.Dispose();
            }
            pictureBox1.Image = bmpTemp;
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private void btnDefaultCursor_Click(object sender, EventArgs e)
        {
            CalibrationType = "NONE";
            this.Cursor = Cursors.Default;
        }

        //private void ModifyLabel_MouseEnter(object sender, EventArgs e)
        //{
        //    TextBox Temp = sender as TextBox;
        //    if (Temp.ReadOnly && Temp.BorderStyle != BorderStyle.FixedSingle)
        //    {
        //        Temp.BorderStyle = BorderStyle.FixedSingle;
        //    }
        //}

        //private void ModifyLabel_MouseLeave(object sender, EventArgs e)
        //{
        //    TextBox Temp = sender as TextBox;
        //    if (Temp.ReadOnly)
        //    {
        //        Temp.BorderStyle = BorderStyle.None;
        //    }
        //}

        //private void ModifyLabel_MouseDown(object sender, MouseEventArgs e)
        //{
        //    TextBox Temp = sender as TextBox;
        //    if (Temp.ReadOnly && Temp.Focused && e.Button == MouseButtons.Left)
        //    {
        //        string s = Temp.Text;
        //        Temp.BorderStyle = BorderStyle.Fixed3D;
        //        Temp.BackColor = Color.White;
        //        Temp.ReadOnly = false;
        //        Temp.Text = s;
        //    }
        //}

        private void ModifyLabel_Validating(object sender, EventArgs e)
        {
            
            TextBox Temp = sender as TextBox;
            if (!Temp.ReadOnly)
            {
                if (Temp.Name == textBoxSAD.Name)
                {
                    if (Temp.Text.Trim() == "")
                    {
                        Temp.Text = CapturePub.readCaptrueValue("SAD", false);
                    }
                    else
                    {
                        CapturePub.saveCaptrueValue("SAD", textBoxSAD.Text.Trim());
                    }
                }
                else if (Temp.Name == textBoxPhysicsWidth.Name)
                {
                    if (Temp.Text.Trim() == "")
                    {
                        Temp.Text = CapturePub.readCaptrueValue("PhysicsWidth", false);
                    }
                    else
                    {
                        CapturePub.saveCaptrueValue("PhysicsWidth", textBoxPhysicsWidth.Text.Trim());
                    }
                }
                else if (Temp.Name == textBoxPhysicsHeight.Name)
                {
                    if (Temp.Text.Trim() == "")
                    {
                        Temp.Text = CapturePub.readCaptrueValue("PhysicsHeight", false);
                    }
                    else
                    {
                        CapturePub.saveCaptrueValue("PhysicsHeight", textBoxPhysicsHeight.Text.Trim());
                    }
                }
            }
            //Temp.BorderStyle = BorderStyle.None;
            //Temp.BackColor = Color.LightSteelBlue;
            //Temp.ReadOnly = true;
        }

        private void cbAngle_SelectedIndexChanged(object sender, EventArgs e)
        {
            string xmlValue = CapturePub.readCaptrueValue("SetupAngle");
            int idx1 = cbAngle.Items.IndexOf(xmlValue);
            if (idx1 < 0)
            {
                idx1 = 0;
            }
            int SetupAngle = (cbAngle.SelectedIndex - idx1) * 90;
            ImgCapture.SetupAngle = int.Parse(cbAngle.Text.Trim());
            CapturePub.saveCaptrueValue("SetupAngle", cbAngle.Text);

            if (captureImagesNow == null || captureImagesNow.Count <= 0)
            {
                return;
            }
            if (SetupAngle % 90 != 0 || SetupAngle % 360 == 0)
            {
                return;
            }

            while (SetupAngle < 0)
            {
                SetupAngle += 360;
            }

            for (int n = 0; n < captureImagesNow.Count; n++)
            {
                int idx = SetupAngle / 90;
                int r = captureImagesNow[n].ImageData.GetLength(0);
                int c = captureImagesNow[n].ImageData.GetLength(1);
                List<int[]> COUNTANGLE = new List<int[]> { 
                new int[5] { 1, -1,  1, c - 1,     0 }, 
                new int[5] { 0, -1, -1, r - 1, c - 1 }, 
                new int[5] { 1,  1, -1,     0, r - 1 }, 
                new int[5] { 0,  1, -1,     0, c - 1 }, 
                new int[5] { 0, -1,  1, r - 1,     0 }};//[exchange,scaleX,scaleY,offX,offY]
                int[] ca = COUNTANGLE[idx - 1];
                ushort[,] temp = new ushort[r, c];
                if (ca[0] == 1)
                {
                    for (int i = 0; i < r; i++)
                    {
                        for (int j = 0; j < c; j++)
                        {
                            temp[i, j] = captureImagesNow[n].ImageData[j * ca[1] + ca[3], i * ca[2] + ca[4]];
                        }
                    }
                    captureImagesNow[n].ImageData = (ushort[,])temp.Clone();
                }
                else
                {
                    for (int i = 0; i < r; i++)
                    {
                        for (int j = 0; j < c; j++)
                        {
                            temp[i, j] = captureImagesNow[n].ImageData[i * ca[1] + ca[3], j * ca[2] + ca[4]];
                        }
                    }
                    captureImagesNow[n].ImageData = (ushort[,])temp.Clone();
                }
            }
            if (userControlMutiDicomImages.Num <= 0)
            {
                return;
            }
            else
            {
                int index = userControlMutiDicomImages.SelectedIndex;
                if (index >= userControlMutiDicomImages.Num || index < 0)
                {
                    index = 0;
                }
                userControlMutiDicomImages.DrawImagesInPane(GetSmallBitmapList(captureImagesNow), index);
                ImageObject img = captureImagesNow[index];
                srcBitmap = img.BMP;
                RefreshImage(img);
            }
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
    }
}
