using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;

namespace ImageCapturing
{
    public unsafe class Capture1800I_096Dynamic : CaptureBase
    {
        private CareRayInterface.DetectorInfo detectorInfo = new CareRayInterface.DetectorInfo();
        private CareRayInterface.StatusInfo statusInfo = new CareRayInterface.StatusInfo();
        private CareRayInterface.ModeInfo modeInfo = new CareRayInterface.ModeInfo();
        private CareRayInterface.ExpProgress expProg = new CareRayInterface.ExpProgress();
        private CareRayInterface.ExpProgress calProg = new CareRayInterface.ExpProgress();
        private CareRayInterface.UserCorrection userCorrection = new CareRayInterface.UserCorrection();
        private CareRayInterface.FrameAttr obj = new CareRayInterface.FrameAttr();
        private CareRayInterface.CalParams cal_params = new CareRayInterface.CalParams();
        private CareRayInterface.FpgaReg fpga = new CareRayInterface.FpgaReg();

        /// <summary>
        /// 执行采集的后台线程
        /// </summary>
        BackgroundWorker BackgroundWorkerCaptureImageData = new BackgroundWorker();
        /// <summary>
        /// 存储图像数据的连续内存块
        /// </summary>
        ushort* pAcqBuffer = (ushort*)IntPtr.Zero;
        /// <summary>
        /// 统计采集到的亮场图像数量；
        /// </summary>
        int acqBrightImageNumber = 0;
        /// <summary>
        /// 需要采集的帧数；
        /// </summary>
        public int acqImageNuber = 100;
        /// <summary>
        /// 执行后台采集线程的唯一标识符号
        /// </summary>
        string identificationStr = "";
        /// <summary>
        /// 连接Panel的线程；
        /// </summary>
        Thread LinkPanelThread = null;
        /// <summary>
        /// 连接Panel线程使用的同步变量
        /// </summary>
        Mutex LinkPanelMutex = new Mutex();
        /// <summary>
        /// 采集图像的回调函数委托
        /// </summary>
        CareRayInterface.EventCallbackDelegate EventCallbak = null;
        /// <summary>
        /// 丢帧跟踪ID
        /// </summary>
        int ImageTrackID = 0;
        /// <summary>
        /// 平板探测器的IP
        /// </summary>
        string PanelIP = "192.168.68.1";
        /// <summary>
        /// 是否后台监测Panel网络连接
        /// </summary>
        bool MonitorPanelNetwork_Background = false;

        #region 采集配置相关参数
        /// <summary>
        /// 探测器的工作模式
        /// </summary>
        CareRayInterface.CheckMode checkMode = CareRayInterface.CheckMode.MODE_RAD;
        /// <summary>
        /// Trigger 同步模式
        /// </summary>
        CareRayInterface.SyncMode TriggerSyncMode = CareRayInterface.SyncMode.INI_TRIGGER;
        /// <summary>
        /// Gain值
        /// </summary>
        int GainValue = 7;//可以设置为1`7;
        /// <summary>
        /// 曝光时间，单位毫秒ms
        /// </summary>
        int ExposureTime = 2000;
        /// <summary>
        /// 延迟时间，单位毫秒ms
        /// </summary>
        int DelayTime = 100;
        /// <summary>
        /// 等待时间，单位毫秒ms
        /// </summary>
        int WaitTime = 5;
        /// <summary>
        /// 1800I Gain算法是否启用
        /// </summary>
        public bool Enable1800IGainAlgorithm = true;
        /// <summary>
        /// Raw文件的文件头
        /// </summary>
        public int RawFileHeadSize = 4;

        #endregion

        #region 查询Panel状态的函数

        //Show detector information
        int showDetectorInfo()
        {
            Console.WriteLine("\nCalling CR_get_detector_info:\n");
            int result = CareRayInterface.CR_get_detector_info(ref detectorInfo);
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR == result)
            {
                Console.WriteLine("\t\tRawImageWidth is: {0}\n", detectorInfo.rawImageWidth);
                Console.WriteLine("\t\tRawImageHeight is: {0}\n", detectorInfo.rawImageHeight);
                Console.WriteLine("\t\tMaxPixelValue is: {0}\n", detectorInfo.maxPixelValue);
                Console.WriteLine("\t\tBitsPerPixel is: {0}\n", detectorInfo.bitsPerPixel);
                Console.WriteLine("\t\tHardWareVerion is: {0}\n", detectorInfo.hardWareVersion);
                Console.WriteLine("\t\tSoftWareVerion is: {0}\n", detectorInfo.softWareVersion);
                Console.WriteLine("\t\tSerialNumber is: {0}\n", detectorInfo.serialNumber);
                Console.WriteLine("\t\tDetectorDescription is: {0}\n", detectorInfo.detectorDescription);
            }
            return result;
        }

        int showStatusInfo()
        {
            Console.WriteLine("\nCalling CR_get_status_info:\n");
            int result = CareRayInterface.CR_get_status_info(ref statusInfo);
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR == result)
            {
                Console.WriteLine("\t\tcheckMode: {0}\n", statusInfo.checkMode);
                Console.WriteLine("\t\tframeRate: {0}\n", statusInfo.frameRate);
                Console.WriteLine("\t\tdetectorState: {0}\n", statusInfo.detectorState);
                Console.WriteLine("\t\taveTemperature: {0}\n", statusInfo.temperature.aveTemperature);
                Console.WriteLine("\t\tmaxTemperature: {0}\n", statusInfo.temperature.maxTemperature);
                //overhot_flag: see enum TempStatus
                Console.WriteLine("\t\toverhot: {0}\n", statusInfo.temperature.overhot_flag);
                
            }
            return result;
        }

        //Show current mode information
        int showModeInfo()
        {
            Console.WriteLine("\nCalling CR_get_detector_info:\n");
            int result = CareRayInterface.CR_get_mode_info((int)checkMode, ref modeInfo);
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR == result)
            {
                Console.WriteLine("\t\tModeId={0}\n", modeInfo.modeId);
                Console.WriteLine("\t\tAcqType={0}\n", modeInfo.acqType);
                Console.WriteLine("\t\tImageWidth={0}, ImageHeight={0}\n", modeInfo.imageWidth, modeInfo.imageHeight);
                Console.WriteLine("\t\tLinesPerPixel={0}, ColsPerPixel={0}\n", modeInfo.linesPerPixel, modeInfo.colsPerPixel);
                Console.WriteLine("\t\tImageSize(in Byte):{0}\n", modeInfo.imageSize);
                Console.WriteLine("\t\tMaxFrameRate:%f\n", modeInfo.maxFrameRate);
                Console.WriteLine("\t\tModeDescription:{0}\n", modeInfo.modeDescription);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// 刷新Panel连接之后固定的配置；
        /// 一般在Panel连接之后调用一次；
        /// </summary>
        void RefreshFixedPanelSettings()
        {
            try
            {
                //when detected the DROC disconnected with detector by calling
                //CR_getConnState(), only call CR_connect_detector to reconnect, and don't
                //call CR_reset_detector any more.
                int result = 0;
                result = CareRayInterface.CR_reset_detector(0);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_reset_detector error,reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }

                //输出探测器信息；
                result = showDetectorInfo();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    //showDetectorInfo()有注释
                }

                //输出探测器状态信息；
                //Attention the Temperature, overhot_flag indicates the temperature 
                //status in current detector!!!!!!!
                result = showStatusInfo();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    //showStatusInfo()有注释
                }

                //输出探测器型号；
                int currentDetectorType = CareRayInterface.CR_get_detector_type();
                Console.WriteLine("Detector type:{0}", new object[] { ((CareRayInterface.DetectorType)currentDetectorType).ToString() });

                //设置探测器的工作模式；Rad模式；
                result = CareRayInterface.CR_set_check_mode((int)checkMode);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("Set mode error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }
                //输出探测器的模式信息；
                result = showModeInfo();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("Show mode info error, reason: %s\n", CareRayErrors.CrErrStrList(result));
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("RefreshFixedPanelSettings()...catch(1)..." + ex.Message);
            }


            try
            {
                int result;
                //设置Gain值；
                fpga = new CareRayInterface.FpgaReg();
                result = CareRayInterface.CR_get_fpga_reg(ref fpga);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.Write("CR_get_fpga_reg error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }

                int readoutCntr = fpga.readout_cntr;
                int gainId = (readoutCntr >> 4) & 0x00000007;
                Console.WriteLine("Gain Value read from FPGA =" + gainId);

                gainId = GainValue;

                Console.WriteLine("Set Gain ID =" + gainId);

                int readout = fpga.readout_cntr;
                int readoutCntrPrev = (readout & (~(7 << 4))) | (gainId << 4);
                fpga.readout_cntr = readoutCntrPrev;
                //int prmId = (int)((uint)(&fpga.readout_cntr) - (uint)(&fpga.res1));
                int prmId = 160;
                result = CareRayInterface.CR_set_detector_iparam(prmId, readoutCntrPrev);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.Write("CR_set_detector_iparam error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }

                fpga = new CareRayInterface.FpgaReg();
                result = CareRayInterface.CR_get_fpga_reg(ref fpga);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.Write("CR_get_fpga_reg error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }

                readoutCntr = fpga.readout_cntr;
                gainId = (readoutCntr >> 4) & 0x00000007;
                Console.WriteLine("Gain ID read 2 from FPGA=" + gainId);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("RefreshFixedPanelSettings()...catch(2)..." + ex.Message);
            }

            int res = CareRayInterface.CR_register_callback(EventCallbak);
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR != res)
            {
                Console.Write("CR_register_callback error, reason: {0}\n", CareRayErrors.CrErrStrList(res));
            }
        }


        public Capture1800I_096Dynamic()
            : base()
        {
            //是否后台监测Panel网络连接
            MonitorPanelNetwork_Background = CapturePub.readCaptrueValue(XmlField.MonitorPanelNetwork_Background) == "T";
            //读取Panel IP地址
            string config_file = CapturePub.CareRayPath + "Config.ini";
            PanelIP = CareRayInterface.ReadConfigOptionValue(config_file,"ipAddress");

            LinkPanelThread = new Thread(LinkPanelThreadFun);
            LinkPanelThread.IsBackground = true;

            EventCallbak = new CareRayInterface.EventCallbackDelegate(drocEventCallback);
        }


        protected override void InitParam()
        {
            ReadSetupConfig();

            ReadCaptureConfig();

            RefreshFixedPanelSettings();

            AllocAcquireMemory();
        }

        /// <summary>
        /// 从本地文件读取采集配置参数
        /// </summary>
        void ReadCaptureConfig()
        {
            //1,工作模式；
            checkMode = CareRayInterface.CheckMode.MODE_FLUORO_START + 2;
            //2,采集同步模式；
            TriggerSyncMode = CareRayInterface.SyncMode.EXT_TRIGGER;
            //3,采集曝光时间，单位ms；
            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.CareRay_ExposureTime), out ExposureTime))
            {
                ExposureTime = 333;
                CapturePub.saveCaptrueValue(XmlField.CareRay_ExposureTime, ExposureTime.ToString());
            }
            //4,采集延迟时间，单位ms；
            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.CareRay_ExposureDelay), out DelayTime))
            {
                DelayTime = 1;
                CapturePub.saveCaptrueValue(XmlField.CareRay_ExposureDelay, DelayTime.ToString());
            }
            //5,采集等待时间，单位ms；
            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.CareRay_ExposureWait), out WaitTime))
            {
                WaitTime = 1;
                CapturePub.saveCaptrueValue(XmlField.CareRay_ExposureWait, WaitTime.ToString());
            }
            //6,是否启用Gain校正算法
            Enable1800IGainAlgorithm = CapturePub.readCaptrueValue(XmlField.Enable1800IGainAlgorithm) == "T";
            //7,Sequence采集的帧数
            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.CaptureFrameCount), out SeqFrameCount))
            {
                SeqFrameCount = 1;
                CapturePub.saveCaptrueValue(XmlField.CaptureFrameCount, SeqFrameCount.ToString());
            }
            //8,Gain值
            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.CareRay_GainValue), out GainValue))
            {
                GainValue = 1;
                CapturePub.saveCaptrueValue(XmlField.CareRay_GainValue, GainValue.ToString());
            }
        }

        /// <summary>
        /// 刷新Panel的设置
        /// 静态模式有对应的积分时间校正文件，设置Gain校正文件是否调用的函数快速返回，不然会设置错误，导致时间很长
        /// 所以这里设置积分时间，再设置是否调用Gain校正文件
        /// </summary>
        void RefreshPanelSettings()
        {
            try
            {
                int result;

                //1，设置Trigger同步模式
                result = CareRayInterface.CR_set_sync_mode((int)TriggerSyncMode);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("Set sync mode error, reason: {0}\n " + CareRayErrors.CrErrStrList(result));
                }
                //2,设置探测器的曝光时间，延迟时间，等待时间
                result = CareRayInterface.CR_set_cycle_time(ExposureTime, DelayTime, WaitTime);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("Set cycle time error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }
                //3，设置输出1800I的Gain校正算法
                UseCalibrationConfig(Enable1800IGainAlgorithm);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("RefreshFixedPanelSettings()...catch..." + ex.Message);
            }
        }

        public override void Start()
        {
            if (WorkStatus)
            {
                Console.WriteLine("Can not acquire image because of acquiring image now...WorkStatus=" + WorkStatus);
                return;
            }

            WorkStatus = true;
            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 1, 0);

            //跟踪丢帧的变量
            ImageTrackID = 0;

            //重置统计亮场帧数标记；
            acqBrightImageNumber = 0;

            try
            {
                while (true)
                {
                    if (BackgroundWorkerCaptureImageData.IsBusy)
                    {
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        BackgroundWorkerCaptureImageData.WorkerSupportsCancellation = true;
                        BackgroundWorkerCaptureImageData.DoWork -= new DoWorkEventHandler(BackgroundWorkerCaptureImageData_DoWork);
                        BackgroundWorkerCaptureImageData.DoWork += new DoWorkEventHandler(BackgroundWorkerCaptureImageData_DoWork);

                        //BackgroundWorkerCaptureImageData.RunWorkerCompleted -= BackgroundWorkerCaptureImageData_RunWorkerCompleted;
                        //BackgroundWorkerCaptureImageData.RunWorkerCompleted += BackgroundWorkerCaptureImageData_RunWorkerCompleted;

                        identificationStr = Guid.NewGuid().ToString();
                        BackgroundWorkerCaptureImageData.RunWorkerAsync(identificationStr);
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Start():back thread capturing image error:" + ex.Message);
            }
        }

        
        /// <summary>
        /// 是否启用Gain校正算法
        /// </summary>
        /// <param name="gain_cal"></param>
        void UseCalibrationConfig(bool gain_cal)
        {
            Console.WriteLine("UseCalibrationConfig(gain_cal={0})", new object[] { gain_cal });

            int result;

            try
            {
                userCorrection.fixedCorr = gain_cal;
                userCorrection.non_fixedCorr = false;
                userCorrection.portableCorr = false;

                result = CareRayInterface.CR_set_user_correction(ref userCorrection);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_set_user_correction error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }
                else
                {
                    Console.WriteLine("set gain successfully");
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("UseCalibrationConfig()...catch error:" + ex.Message);
            }
        }

        void drocEventCallback(int eventID, ref CareRayInterface.EventData eventData)
        {
            Console.WriteLine("Event ID:" + eventID);

            switch (eventID)
            {
                case (int)CareRayInterface.event_id.EVT_DISCONNECT:
                case (int)CareRayInterface.event_id.EVT_DETECTOR_ERROR:
                    {
                        int result = CareRayInterface.CR_stop_acq_frame();
                        if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                        {
                            Console.WriteLine("CR_stop_acq_frame error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                        }
                    }
                    break;
                case (int)CareRayInterface.event_id.EVT_IMAGE_ARRIVE:
                case (int)CareRayInterface.event_id.EVT_VIDEO_FRAME_INDEX_CHANGED:
                    {
                        //外部中断Cancel();立即退出；
                        if (!WorkStatus)
                        {
                            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                            WorkStatus = false;
                            return;
                        }

                        //用于判断采集是否结束
                        acqImageNuber--;

                        //改变采集到的帧数统计；
                        acqBrightImageNumber++;
                        //输出亮场帧数统计信息；
                        Console.WriteLine("Image Number=" + acqBrightImageNumber);

                        //Windows界面输出采集进度；
                        int msgID = GenerateWinMessage("Capturing image number:" + acqBrightImageNumber);
                        Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);


                        #region Step(1):TCP/IP协议网络从Panel缓冲区取出图像；

                        DateTime tm = DateTime.Now; //log
                        int imageSize = eventData.width * eventData.height * eventData.bits / 8;
                        int imageRows = eventData.height;
                        int imageColumns = eventData.width;

                        ushort* imageData = (ushort*)eventData.data;

#region 验证是否丢帧

                        IntPtr checkPtr = (IntPtr)imageData;
                        int imageID = Marshal.ReadInt32(checkPtr, 0);
                        if (imageID != ImageTrackID)
                        {
                            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!lost frame ID=" + imageID);
                        }

                        ImageTrackID = imageID;
                        ImageTrackID++;

#endregion

                      

                        Console.WriteLine("Step(1):(TCP/IP)Transfer image time=" + (DateTime.Now - tm));//log
                        #endregion

                        #region Step(2)封装数据到对象ImageObject

                        tm = DateTime.Now;

                        RefreshScale();

                        ImageObject imageObjectBase = new ImageObject();

                        ushort[,] imagedata = BufferToArray(imageData, RawFileHeadSize, imageRows, imageColumns);

                        imageObjectBase.pixelSize = this.pixelSize;
                        imageObjectBase.centerX = this.imageCenterX;
                        imageObjectBase.centerY = this.imageCenterY;
                        imageObjectBase.ImageData = imagedata;
                        imageObjectBase.createTime = DateTime.Now;

                        Console.WriteLine("Encapsulate image data to ImageROI time=" + (DateTime.Now - tm));//log
                        #endregion



                        #region Step(3):将采集到的数据ImageROI对象压入到堆栈中供异步处理

                        //压入堆栈；
                        imgList.Enqueue(imageObjectBase);
                        //发送消息到采集界面；保存数据库；  
                        Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_DATA, (int)captureImageMode, acqBrightImageNumber);
                        #endregion

                        //判断采集是否结束
                        if (acqImageNuber == 0)
                        {
                            Cancel();
                            return;
                        }

                    }
                    break;
                default:
                    Console.WriteLine("Rad image transmission complete default");
                    break;
            }
        }

        public override void CaptureImageData()
        {
            int result;

            #region Step(1):设置采集的帧数

            acqImageNuber = 3;

            #endregion//End Step(1)

            #region Step(2):设置工作电压

            try
            {
                result = CareRayInterface.CR_set_normal_power();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_set_normal_power error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                    WorkStatus = false;
                    return;
                }
                else
                {
                    Console.WriteLine("Set panel working voltage successfully.");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("CR_set_normal_power error, reason: {0}\n", ex.Message);
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                WorkStatus = false;
                return;
            }
            #endregion//End Step(2)

            #region Step(3):刷新Panel的参数设置；

            Console.WriteLine("Step(3):Refresh panel settings...");
            RefreshPanelSettings();

            #endregion//End Step(3)

            #region Step(4):采集之前本底丢弃，刷新探测器的lag

            //CaptureDarkImage_DiscardDarkImage_BeforeCapture();
            #endregion//End Step(4)

            #region Step(5):动态本底校正；

            //....
            #endregion//End Step(5)

            #region  Step(6):是否内触发采集本底

            //...
            #endregion//End Step(6)


            //设置状态栏进度信息；
            int msgID2 = GenerateWinMessage("Preparing irradiation...");
            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID2, 0);

            //调用函数开始采集；
            result = CareRayInterface.CR_start_acq_full_image();
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
            {
                Console.WriteLine("Step(7):CR_start_acq_image_full error, reason: " + CareRayErrors.CrErrStrList(result));
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                WorkStatus = false;
                return;
            }
            else
            {
                Console.WriteLine("1800I 0.96 start acquisition successfully.");
            }
        }

        void BackgroundWorkerCaptureImageData_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!e.Argument.Equals(identificationStr))
            {
                Console.WriteLine("BackgroundWorkerCaptureImageData_DoWork():Thread conflict.");
                return;
            }
            CaptureImageData();
        }


        public override void Cancel()
        {
            if (WorkStatus)
            {
                //强制中断采集
                int result = CareRayInterface.CR_stop_acq_frame();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_stop_acq_frame error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }
            }

            WorkStatus = false;
            Kernel32Interface.PostMessage(this.HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
            Trigger.Stop();
        }

        public override void Stop()
        {
            try
            {
                base.Stop();
                FreeMemory();
                WorkStatus = false;
                LinkStatus = PanelLinkStatus.NONE;
            }
            catch
            { }
        }

        #region 连接1800I Panel

        public override void AsyncInitLink()
        {
            LinkPanelThread.Start();
        }

        private void LinkPanelThreadFun()
        {
            while (true)
            {
                bool v = PingIpOrDomainName(PanelIP);
                if (v)
                {
                    InitLink();
                }
                else
                {
                    v = PingIpOrDomainName(PanelIP);
                    if (!v)
                    {
                        LinkStatus = PanelLinkStatus.NONETWORK;
                    }
                }

                //是否后台监测平板的网络连接
                if (!MonitorPanelNetwork_Background)
                {
                    if (LinkStatus == PanelLinkStatus.LINK_SUCCESS)
                    {
                        break;
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public override void InitLink()
        {
            LinkPanelMutex.WaitOne();

            if (LinkStatus != PanelLinkStatus.LINK_SUCCESS)
            {
                bool v = TryLink();
                if (v)
                {
                    LinkStatus = PanelLinkStatus.LINK_SUCCESS;

                    InitParam();
                }
                else
                {
                    LinkStatus = PanelLinkStatus.LINK_FAIL;
                }
            }

            LinkPanelMutex.ReleaseMutex();
        }

        private bool TryLink()
        {
            Console.WriteLine("0.96 Static try link...");

            int result;

            byte* path = (byte*)Marshal.StringToCoTaskMemAnsi(CapturePub.CareRayPath);
            result = CareRayInterface.CR_connect_detector(path);

            bool v = result == (int)KZ_ERROR_TYPE.CR_NO_ERR;
            if (!v)
            {
                Console.WriteLine("Connect default detector (IP:192.168.68.1) error, reason: %s\n", CareRayErrors.CrErrStrList(result));
                Console.WriteLine("For dual_system, try to connect another detector (IP:192.168.68.2)...\n\n");
            }
            else
            {
                Console.WriteLine("Connect detector successfully");
            }


            if ((IntPtr)path != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem((IntPtr)path);
            }

            return v;
        }

        #endregion


        void AllocAcquireMemory()
        {
            if ((IntPtr)((void*)this.pAcqBuffer) != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)((void*)this.pAcqBuffer));
                this.pAcqBuffer = (ushort*)(void*)IntPtr.Zero;
            }
            this.pAcqBuffer = (ushort*)(void*)Marshal.AllocHGlobal(2816 * 2816 * 2 + RawFileHeadSize);
        }

        
        void FreeMemory()
        {
            if ((IntPtr)this.pAcqBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)((void*)this.pAcqBuffer));
                this.pAcqBuffer = (ushort*)(void*)IntPtr.Zero;
            }
        }


        public static ushort[,] BufferToArray(ushort* Buffer, int headsize, int Row, int Col)
        {
            int head = Marshal.ReadInt32((IntPtr)Buffer);

            byte* p = (byte*)Buffer;
            p = p + headsize;

            ushort* p1 = (ushort*)p;

            ushort[,] imgdata = new ushort[Row, Col];
            for (int i = 0; i < Row; ++i)
            {
                for (int j = 0; j < Col; ++j)
                {
                    imgdata[i, j] = *p1;
                    ++p1;
                }
            }
            return imgdata;
        }

    }
}
