using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Timers;

namespace ImageCapturing
{
    public class CaptureSim : CaptureBase
    {
        /// <summary>
        /// Panel抓取图像的行数
        /// </summary>
        private int imageRows;
        /// <summary>
        /// Panel抓取图像的列数
        /// </summary>
        private int imageColumns;

        /// <summary>
        /// 默认是抓取序列模式
        /// </summary>
        public CapturePKI.PanelCaptureMode captureImageMode = CapturePKI.PanelCaptureMode.Continuous;

        Timer timerGenerateImage = new Timer();

        int seqenceNum = 0;

        public CaptureSim()
            : base()
        {
            imageRows = 1024;
            imageColumns = 1024;
            timerGenerateImage.Interval = 1000;
            timerGenerateImage.Elapsed -= new ElapsedEventHandler(timerGenerateImage_Elapsed);
            timerGenerateImage.Elapsed += new ElapsedEventHandler(timerGenerateImage_Elapsed);
            int mode;
            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.CaptureMode, false), out mode))
            {
                captureImageMode = (CapturePKI.PanelCaptureMode)mode;
            }
            else
            {
                captureImageMode = CapturePKI.PanelCaptureMode.Sequence;
            }
        }
        public override void InitParam()
        {
            base.InitParam();
            RefreshScale();
           
        }

        public override void SetCaptureMode(CapturePKI.PanelCaptureMode captureMode)
        {
            captureImageMode = captureMode;
        }

        public override void Cancel()
        {
            WorkStatus = false;
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
            timerGenerateImage.Stop();
            Trigger.Stop();
            base.Cancel();
        }

        public override void Start()
        {
            WorkStatus = true;
            int msgID = GenerateWinMessage("Preparing irradiation and capturing...");
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 1, 0);
            base.Start();
        }

        protected override void Trigger_TriggerChanged(TRIGGER_STATUS status)
        {
            if ((TRIGGER_STATUS)status == TRIGGER_STATUS.ON)
            {
                seqenceNum = 0;
                int msgID = GenerateWinMessage("Simulate capturing images...");
                RefreshScale();
                if (captureImageMode == CapturePKI.PanelCaptureMode.Sequence)
                {
                    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, FrameCount);
                }
                else if (captureImageMode == CapturePKI.PanelCaptureMode.Continuous)
                {
                    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
                }
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_TRIGGER_CHANGE, (int)status, 0);
                timerGenerateImage.Start();
                //Trigger.Stop();
                //int msgID = GenerateWinMessage("Capturing images...");
            }
            else if ((TRIGGER_STATUS)status == TRIGGER_STATUS.OFF)
            {
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_TRIGGER_CHANGE, (int)status, 0);
                timerGenerateImage.Stop();
                Trigger.Stop();
            }
        }


        public override void RefreshScale()
        {
            float sid = SID + GetMachinePara.GetPanelOffsetZ() * 10;
            float scaleToDRR = SAD / sid;

            pixelSize = phySize * scaleToDRR / imageColumns;
            imageCenterX = (phyCenterX + GetMachinePara.GetPanelOffsetX() * 10) * scaleToDRR / pixelSize;
            imageCenterY = (phyCenterY + GetMachinePara.GetPanelOffsetY() * 10) * scaleToDRR / pixelSize;
        }


        private void timerGenerateImage_Elapsed(object sender, ElapsedEventArgs e)
        {
            seqenceNum++;
            if (captureImageMode == CapturePKI.PanelCaptureMode.Sequence)
            {
                
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -1, 1);
                Bitmap bm = new Bitmap(imageColumns, imageRows);
                Graphics g = Graphics.FromImage(bm);
                g.FillRectangle(new SolidBrush(Color.Black), 0, 0, bm.Width, bm.Height);
                Font font = new Font(FontFamily.GenericSansSerif, 800, FontStyle.Bold);
                g.DrawString(seqenceNum.ToString(), font, new SolidBrush(Color.White), new PointF(0, 0));

                ImageObject image = new ImageObject();
                image.pixelSize = pixelSize;
                image.imageData = BitmapToUShort(bm);
                image.imageWidth = image.imageData.GetLength(1);
                image.imageHeight = image.imageData.GetLength(0);
                image.centerX = imageCenterX;
                image.centerY = imageCenterY;
                image.createTime = DateTime.Now;
                imgList.Add(image);
                if (seqenceNum == FrameCount)
                {
                    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_DATA, (int)captureImageMode, 0);
                    timerGenerateImage.Stop();
                    WorkStatus = false;
                    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                }
            }
            else if (captureImageMode == CapturePKI.PanelCaptureMode.Continuous)
            {
                Bitmap bm = new Bitmap(imageColumns, imageRows);
                Graphics g = Graphics.FromImage(bm);
                g.FillRectangle(new SolidBrush(Color.Black), 0, 0, bm.Width, bm.Height);
                Font font = new Font(FontFamily.GenericSansSerif, 800, FontStyle.Bold);
                g.DrawString(seqenceNum.ToString(), font, new SolidBrush(Color.White), new PointF(0, 0));

                ImageObject image = new ImageObject();
                image.pixelSize = pixelSize;
                image.imageData = BitmapToUShort(bm);
                image.imageWidth = image.imageData.GetLength(1);
                image.imageHeight = image.imageData.GetLength(0);
                image.centerX = imageCenterX;
                image.centerY = imageCenterY;
                image.createTime = DateTime.Now;
                imgList.Add(image);
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_DATA, (int)captureImageMode, 0);
            }
            //else if (captureImageMode == CapturePKI.PanelCaptureMode.Average)
            //{
            //    //PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBufferAverage, (uint)FrameCount, imageRows, imageColumns);
            //    //PKL_Interface.Acquisition_Acquire_Image(hAcqDesc, (uint)FrameCount, 0, (uint)SeqBufferMode.HIS_SEQ_AVERAGE, pOffsetBuffer, null, null);
            //    //Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, FrameCount);
            //}
            else if (captureImageMode == CapturePKI.PanelCaptureMode.DoubleExposure)
            {
                //beamON_Num++;
                //PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBuffer, (uint)FrameCount, imageRows, imageColumns);
                //PKL_Interface.Acquisition_Acquire_Image(hAcqDesc, (uint)FrameCount, 0, (uint)SeqBufferMode.HIS_SEQ_ONE_BUFFER, null, null, null);
                //Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
            }
        }

        public override void Stop()
        {
            timerGenerateImage.Stop();
            base.Stop();
        }
    }
}
