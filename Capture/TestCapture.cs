using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;

namespace ImageCapturing
{
    public partial class TestCapture : Form
    {
        #region Class Members

        CaptureBase ImgCapture = null;
        ImgBase imgShow = new ImgBase();
        ImageObject imgCurrent = new ImageObject();

        Thread PanelStatusThread = null;

        Thread ImageProcessThread = null;
        Mutex ImageProcessThreadMutex = new Mutex();
        Queue<ImageObject> QueueImages = new Queue<ImageObject>();
        public delegate void RefreshDataDisplayDelegate(ImageObject img);

        TriggerCOM trigger = null;




        #endregion

        #region Constructors

        public TestCapture()
        {
            InitializeComponent();
            panelMain.Controls.Add(imgShow);
            imgShow.Dock = DockStyle.Fill;

            ImgCapture = CaptureBase.GetCapture();
            ImgCapture.RefreshHostHandle(this.Handle);
            ImgCapture.SetTrigger();
            ImgCapture.AsyncInitLink();

            PanelStatusThread = new Thread(new ThreadStart(PanelStatusThreadFun));
            PanelStatusThread.Name = "PanelStatusThread";
            PanelStatusThread.Start();

            ImageProcessThread = new Thread(new ThreadStart(ImageProcessThreadFun));
            ImageProcessThread.Name = "Process image refresh";
            ImageProcessThread.Start();

            this.Shown -= new EventHandler(FormCapture_Shown);
            this.Shown += new EventHandler(FormCapture_Shown);
        }


        void PanelStatusThreadFun()
        {
            while (!Disposing)
            {
                RefreshPanelLinkStatus();
                Thread.Sleep(1000);
            }
        }

        void RefreshPanelLinkStatus()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ThreadStart(RefreshPanelLinkStatus));
            }
            else
            {
                bool link = ImgCapture.LinkStatus == PanelLinkStatus.LINK_SUCCESS;
                Color c = link ? Color.Green : Color.Gray;
                this.pictureBox1.BackColor = c;
            }
        }

        void FormCapture_Shown(object sender, EventArgs e)
        {
            this.KeyUp -= new KeyEventHandler(FormCapture_KeyUp);
            this.KeyUp += new KeyEventHandler(FormCapture_KeyUp);

            imgShow.PositionAndValue -= new ImgBase.PositionAndValueDelegate(ImgShow_PositionAndValue);
            imgShow.PositionAndValue += new ImgBase.PositionAndValueDelegate(ImgShow_PositionAndValue);

            labelImageSize.Text = "";
        }

        void FormCapture_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ImgCapture.WorkStatus)
            {
                ImgCapture.Stop();
                Application.DoEvents();
            }
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

        #endregion


        int TriggerNumber = 0;
        int CaptureNumber = 0;

        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //case WIN_MSG.WM_TRIGGER_CHANGE:
                //case WIN_MSG.WM_TRIGGER_STATUS:
                case WIN_MSG.WM_SHOW_PROGRESS:
                    int msgID = (int)m.WParam;
                    RefreshWindowsInterface(msgID);
                    break;
                case WIN_MSG.WM_CAPTURE_DATA:
                    {
                        CaptureNumber++;

                        labelCaptureNumber.Text = CaptureNumber.ToString();

                        ImageObject img = ImgCapture.imgList.Dequeue();
                        QueueImages.Enqueue(img);
                    }
                    break;
                case WIN_MSG.WM_CAPTURE_WORKSTATUS:
                    SetCaptureStatus((int)m.WParam);
                    break;
                default:
                    base.DefWndProc(ref m);///调用基类函数处理非自定义消息。
                    break;
            }
        }

        void RefreshWindowsInterface(int msgID)
        {
            if (msgID == -2)
            {
                labelProgressInfo.Text = "";
            }
            else
            {
                labelProgressInfo.Text = ImgCapture.PopWinMessage(msgID);
            }
            if (labelProgressInfo.Text == "Preparing irradiation...")
            {
                labelProgressInfo.ForeColor = Color.Green;

                #region 测试软件控制触发信号

                return;

                if (trigger == null)
                {
                    trigger = new TriggerCOM();
                    trigger.Open();
                }

                Console.Clear();
                byte[] triggerdata = new byte[14];
                triggerdata[0] = 0x55;
                triggerdata[1] = 0x66;
                triggerdata[2] = 0x77;
                triggerdata[3] = 0x88;
                triggerdata[4] = 0x02;
                triggerdata[5] = 0x00;
                triggerdata[6] = 0x00;
                triggerdata[7] = 0x00;
                triggerdata[8] = 0x00;
                triggerdata[9] = 0x00;
                triggerdata[10] = 0x00;
                triggerdata[11] = 0x00;
                triggerdata[12] = 0x00;

                //byte Xor = 0x00;
                //for (int i = 0; i < 13; i++)
                //{
                //    Xor = (byte)(Xor ^ triggerdata[i]);
                //}
                //triggerdata[13] = Xor;
                triggerdata[13] = (byte)(0xFF - triggerdata[12]);

                trigger.SendCMD(triggerdata);

                TriggerNumber++;

                labelTriggerNumber.Text = TriggerNumber.ToString();
                #endregion
            }
            else
            {
                labelProgressInfo.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 设置界面可用状态；
        /// </summary>
        /// <param name="CaptureSatus">0,默认不工作状态；1，采集状态；2，offset calibation</param>
        void SetCaptureStatus(int CaptureSatus)
        {
            if (CaptureSatus == 1)
            {
                buttonoffsetCal.Enabled = false;
                buttongainCal.Enabled = false;
                btnCapture.Enabled = true;
                buttonoffsetCal.BackColor = this.BackColor;
                buttongainCal.BackColor = this.BackColor;
                btnCapture.BackColor = Color.Green;
            }
            else if (CaptureSatus == 2)
            {
                buttonoffsetCal.Enabled = true;
                buttongainCal.Enabled = false;
                btnCapture.Enabled = false;
                buttonoffsetCal.BackColor = Color.Green;
                buttongainCal.BackColor = this.BackColor;
                btnCapture.BackColor = this.BackColor;
            }
            else
            {

                buttonoffsetCal.Enabled = true;
                buttongainCal.Enabled = true;
                btnCapture.Enabled = true;
                buttonoffsetCal.BackColor = this.BackColor;
                buttongainCal.BackColor = this.BackColor;
                btnCapture.BackColor = this.BackColor;
            }
        }

        void ImageProcessThreadFun()
        {
            while (!Disposing)
            {
                if (QueueImages.Count > 0)
                {

                    ImageProcessThreadMutex.WaitOne();


                    //DateTime tm = DateTime.Now;

                    ImageObject img = QueueImages.Dequeue();
                    img.GenerateImage();
                    RefreshDataDisplay(img);

                    //Console.WriteLine(DateTime.Now - tm);

                    ImageProcessThreadMutex.ReleaseMutex();

                    Thread.Sleep(10);
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
        }

        void RefreshDataDisplay(ImageObject img)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new RefreshDataDisplayDelegate(RefreshDataDisplay), new object[] { img });
            }
            else
            {

                imgCurrent = img;

                imgShow.InitImageData(img);

                labelMean.Text = "Mean:" + (int)img.AverageValue;
                labelLevelWindow.Text = "L=" + img.imgLevel + ",W=" + img.imgWindow;
                labelImageSize.Text = "Image Size:" + img.imageWidth + "x" + img.imageHeight;


            }

        }

        void gbCapture_Click(object sender, EventArgs e)
        {
            switch (ImgCapture.LinkStatus)
            {
                case PanelLinkStatus.NONE:
                    break;
                case PanelLinkStatus.LINK_FAIL:
                    MessageBox.Show("Link error!", "Information");
                    return;
                case PanelLinkStatus.LINK_SUCCESS:
                    break;
                case PanelLinkStatus.LINKING:
                    MessageBox.Show("It is connecting panel,Please wait a few minutes.", "Information");
                    return;
                default:
                    MessageBox.Show("No link status!", "Information");
                    return;
            }

            if (ImgCapture.WorkStatus)
            {
                if (MessageBox.Show("Are you sure to stop capturing images", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ImgCapture.Cancel();
                    Application.DoEvents();
                    //RefreshListBoxFiles(textBoxDir.Text, "");
                }
            }
            else
            {
                imgShow.ClearImageData();


                TriggerNumber = 0;
                CaptureNumber = 0;

                ImgCapture.acqImageNuber = int.Parse(textBox1.Text);

                ImgCapture.Start();
            }
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


                labelLevelWindow.Text = "L=" + this.imgCurrent.imgLevel + ",W=" + imgCurrent.imgWindow;
            }
        }

        void buttonoffsetCal_Click(object sender, EventArgs e)
        {
            switch (ImgCapture.LinkStatus)
            {
                case PanelLinkStatus.NONE:
                    break;
                case PanelLinkStatus.LINK_FAIL:
                    MessageBox.Show("Link error!", "Information");
                    return;
                case PanelLinkStatus.LINK_SUCCESS:
                    break;
                case PanelLinkStatus.LINKING:
                    MessageBox.Show("It is connecting panel,Please wait a few minutes.", "Information");
                    return;
                default:
                    MessageBox.Show("No link status!", "Information");
                    return;
            }

            if (ImgCapture.WorkStatus)
            {
                if (MessageBox.Show("Are you sure to stop capturing images", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.DoEvents();
                    ImgCapture.Cancel();
                }
            }
            else
            {

                Capture1800I_096Static imgCap = (ImgCapture as Capture1800I_096Static);
                imgCap.Enable1800IGainAlgorithm = false;
                imgCap.ThreadOffsetCalibration();
            }
        }

        void buttongainCal_Click(object sender, EventArgs e)
        {
            List<ImageObject> images = new List<ImageObject>();


            Thread.Sleep(10000);

            CareRayInterface.CalParams cal_params = new CareRayInterface.CalParams();

            int result = CareRayInterface.CR_get_cal_params(ref cal_params);
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
            {
                Console.WriteLine("CR_get_cal_params error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                return;
            }

            int ConfigNumber = cal_params.linear_dose_num * cal_params.linear_num_per_dose;

            if (ConfigNumber != images.Count)
            {
                MessageBox.Show("Please check images.");
                return;
            }

            int v = (ImgCapture as Capture1800I_096Static).performGainCalibrationInTwoSteps(images);
            if (v == 0)
            {

                MessageBox.Show("create gain files successfully.");
            }
            else
            {

                MessageBox.Show("Fail to create gain files.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (trigger == null)
            {
                return;
            }
            TriggerSettingForm frm = new TriggerSettingForm(trigger);
            frm.Show();
        }

        private void buttonCapturePara_Click(object sender, EventArgs e)
        {
            using (CaptureParameterSetting frm = new CaptureParameterSetting())
            {

                DialogResult dir = frm.ShowDialog(this);
                if (dir == DialogResult.OK)
                {
                    ImgCapture.ReadCaptureConfig();
                    ImgCapture.RefreshPanelSettings();
                }
            }
        }

     }
}