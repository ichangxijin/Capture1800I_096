using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageCapturing
{

    public enum PanelLinkStatus
    {
        NONE,          //不连接硬件
        LINK_SUCCESS,  //连接成功
        LINK_FAIL,     //连接失败
        LINKING        //正在连接中
    }


    public enum PanelCaptureMode
    {
        Single = 0, //使用Trigger触发只抓一张图
        Sequence = 1,       //序列
        DoubleExposure = 2,  //双曝光抓图模式
        Continuous = 3     //连续抓图 
    }

    public class CaptureBase
    {
        public string SaveImagesPath = "";

        protected TriggerBase Trigger = null;

        /// <summary>
        /// Panel的物理尺寸 mm
        /// </summary>
        protected float phySize = 410.0f;
        /// <summary>
        /// Panel抓图的射野中心X方向的位置，mm
        /// </summary>
        protected float phyCenterX = 0;
        /// <summary>
        /// Panel抓图的射野中心Y方向的位置，mm
        /// </summary>
        protected float phyCenterY = 0;

        protected float SID = 0;

        protected float SAD = 0;
        /// <summary>
        /// 等中心位置图像的像素大小, mm
        /// </summary>
        public float pixelSize = 1;
        /// <summary>
        /// 等中心位置图像的中心点在X方向的像素值
        /// </summary>
        public float imageCenterX = 0;
        /// <summary>
        /// 等中心位置图像的中心点在Y方向的像素值
        /// </summary>
        public float imageCenterY = 0;
        /// <summary>
        /// Panel的安装角度
        /// </summary>
        public int SetupAngle = 0;
        /// <summary>
        /// 序列、循环抓图在内存中开辟的帧数
        /// </summary>
        public int FrameCount = 1;

        protected IntPtr HostHandle = IntPtr.Zero;
        protected IntPtr MainHandle = IntPtr.Zero;

        public Queue<ImageObject> imgList = null;

        public bool WorkStatus = false;
        public PanelLinkStatus LinkStatus = PanelLinkStatus.NONE;
        private System.Threading.Timer timer = null;

        private Dictionary<int, string> WM_msgs = new Dictionary<int, string>();
        private Random ramdomGenerater = new Random(); //和消息对应的随机数，用来传递window消息
        /// <summary>
        /// 默认是抓取序列模式
        /// </summary>
        public PanelCaptureMode captureImageMode = PanelCaptureMode.Sequence;

        public static CaptureBase GetCapture()
        {
            string linkPanelStr = CapturePub.readCaptrueValue(XmlField.LinkPanel,false);
            bool linkPanel = linkPanelStr.ToUpper() != "F";

            CaptureBase capture = null;
            if (linkPanel)
            {
                //string panelBrandName = CapturePub.readCaptrueValue(XmlField.PanelBrandName, false);
                //if (panelBrandName.ToUpper() == "CareRay".ToUpper())
                //{
                //    capture = new CaptureCareRay();
                //}
                //else
                //{
                    capture = new Capture1800I_096Static();
                //}
            }
            else
            {
                //capture = new CaptureSim();
                //capture.InitParam();
            }
            return capture;
        }

        public CaptureBase()
        {
            imgList = new Queue<ImageObject>();
        }

        public void SetTrigger()
        {
            if (Trigger != null)
            {
                Trigger.ProgressBegin -= new ProgressBase.ProgressBeginDelegate(Trigger_ProgressBegin);
                Trigger.TriggerChanged -= new TriggerBase.TriggerChangedDelegate(Trigger_TriggerChanged);
                Trigger.TriggerStatus -= new TriggerBase.TriggerChangedDelegate(Trigger_TriggerStatus);
            }
            Trigger = TriggerBase.GetTrigger();

            Trigger.ProgressBegin -= new ProgressBase.ProgressBeginDelegate(Trigger_ProgressBegin);
            Trigger.TriggerChanged -= new TriggerBase.TriggerChangedDelegate(Trigger_TriggerChanged);
            Trigger.TriggerStatus -= new TriggerBase.TriggerChangedDelegate(Trigger_TriggerStatus);

            Trigger.ProgressBegin += new ProgressBase.ProgressBeginDelegate(Trigger_ProgressBegin);
            Trigger.TriggerChanged += new TriggerBase.TriggerChangedDelegate(Trigger_TriggerChanged);
            Trigger.TriggerStatus += new TriggerBase.TriggerChangedDelegate(Trigger_TriggerStatus);
        }
        
        protected void Trigger_ProgressBegin(string info, int progressValue)
        {
            int msgID = GenerateWinMessage(info);
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, progressValue);
        }

        /// <summary>
        /// 创建一个WIN消息的ID
        /// </summary>
        protected int GenerateWinMessage(string info)
        {
            int msgID = ramdomGenerater.Next(60000);
            int i = 0;
            while (i < 10 && (msgID == 0 || WM_msgs.ContainsKey(msgID)))
            {
                msgID = ramdomGenerater.Next(60000);
                i++;
            }
            if (!WM_msgs.ContainsKey(msgID))
            {
                try
                {
                    WM_msgs.Add(msgID, info);
                }
                catch (System.Exception ex)
                {
                	
                }
            }
            return msgID;
        }

        public string PopWinMessage(int msgID)
        {
            if (WM_msgs.ContainsKey(msgID))
            {
                string msg = WM_msgs[msgID];
                WM_msgs.Remove(msgID);
                return msg;
            }
            else
            {
                return "";
            }
        }

        protected virtual void  Trigger_TriggerChanged(TRIGGER_STATUS status)
        {
            if ((TRIGGER_STATUS)status == TRIGGER_STATUS.ON)
            {
                Trigger.Stop();
                CaptureImageData();
            }
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_TRIGGER_CHANGE, (int)status, 0);
        }

        protected void Trigger_TriggerStatus(TRIGGER_STATUS status)
        {
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_TRIGGER_STATUS, (int)status, 0);
        }

     
        public virtual void AsyncInitLink()
        {
            
        }

        public virtual void InitLink()
        {
            this.LinkStatus = PanelLinkStatus.NONE;
        }


        public void RefreshMainHandle(IntPtr handle)
        {
            MainHandle = handle;
        }

        public void RefreshHostHandle(IntPtr handle)
        {
            HostHandle = handle;
        }

        /// <summary>
        /// 基类中的配置主要是一些基本的配置：
        /// 包括抓图的帧数，
        /// 以及图像的物理尺寸标定参数
        /// </summary>
        protected virtual void InitParam()
        {
            ReadSetupConfig();
        }

        protected void  ReadSetupConfig()
        {
            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.CaptureFrameCount), out FrameCount))
            {
                FrameCount = 1;
            }
            FrameCount = 1;

            if (!int.TryParse(CapturePub.readCaptrueValue("SetupAngle"), out SetupAngle))
            {
                SetupAngle = 0;
            }

            float sad, sid, phyW, phyH, offsetX, offsetY;
            if (!float.TryParse(CapturePub.readCaptrueValue("Sad"), out sad))
            {
                sad = 1000;//mm
            }
            if (!float.TryParse(CapturePub.readCaptrueValue("SetupHeight"), out sid))
            {
                sid = 1600;//mm
            }
            if (!float.TryParse(CapturePub.readCaptrueValue("PhysicsWidth"), out phyW))
            {
                phyW = 410;//mm
            }
            if (!float.TryParse(CapturePub.readCaptrueValue("PhysicsHeight"), out phyH))
            {
                phyH = 410;//mm
            }
            if (!float.TryParse(CapturePub.readCaptrueValue("OffsetPanelCenterX"), out offsetX))
            {
                offsetX = 0;
            }
            if (!float.TryParse(CapturePub.readCaptrueValue("OffsetPanelCenterY"), out offsetY))
            {
                offsetY = 0;
            }
            SAD = sad;
            SID = sid;

            phySize = phyW;
            phyCenterX = phyW / 2.0f + offsetX;
            phyCenterY = phyH / 2.0f + offsetY;
        }


        public virtual void RefreshScale()
        {
            //float scaleToDRR = SAD / SID;
            //pixelSize = phySize * scaleToDRR / imageColumns;
            //imageCenterX = phyCenterX * scaleToDRR / pixelSize;
            //imageCenterY = phyCenterY * scaleToDRR / pixelSize;
        }


        public virtual void Start()
        {
            imgList.Clear();
            Trigger.Start();
        }

        public virtual void CaptureImageData()
        {

        }

        public virtual void Stop()
        {
            Trigger.Dispose();
        }

        public virtual void Cancel()
        {
            Trigger.Stop();
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
        }

        public void RotateImage(ImageObject image)
        {
            if (image == null)
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
            int idx = SetupAngle / 90;
            int r = image.ImageData.GetLength(0);
            int c = image.ImageData.GetLength(1);
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
                        temp[i, j] = image.ImageData[j * ca[1] + ca[3], i * ca[2] + ca[4]];
                    }
                }
                image.ImageData = temp;
            }
            else
            {
                for (int i = 0; i < r; i++)
                {
                    for (int j = 0; j < c; j++)
                    {
                        temp[i, j] = image.ImageData[i * ca[1] + ca[3], j * ca[2] + ca[4]];
                    }
                }
                image.ImageData = temp;
            }
        }

        // 把Bitmap转化为Ushort数组
        public static ushort[,] BitmapToUShort(Bitmap bm)
        {
            int height = bm.Height, width = bm.Width;
            ushort[,] data = new ushort[height, width];
            //Bitmap resultBmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height),
                ImageLockMode.ReadWrite, bm.PixelFormat);//图像的属性
            
            int off = 4;
            if (bm.PixelFormat.ToString().Contains("24"))
            {
                off = 3;
            }
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            int nOffset = stride - bm.Width * off;// 4;//*3
            unsafe
            {
                byte* pbmpdata = (byte*)(void*)Scan0;
                try
                {
                    for (int y = 0; y < height; ++y)
                    {
                        for (int x = 0; x < width; ++x)
                        {
                            data[y, x] = pbmpdata[0];
                            pbmpdata += off;// 4;
                        }
                        pbmpdata += nOffset;
                    }
                }
                catch (Exception ex)
                {
                    String mes = ex.ToString();
                    bm.UnlockBits(bmData);
                    return data;
                }
            }
            bm.UnlockBits(bmData);

            return data;
        }
    }
}
