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
    public unsafe class Capture1800I_096Static : CaptureBase
    {
        private CareRayInterface.DetectorInfo detectorInfo = new CareRayInterface.DetectorInfo();
        private CareRayInterface.StatusInfo statusInfo = new CareRayInterface.StatusInfo();
        private CareRayInterface.ModeInfo modeInfo = new CareRayInterface.ModeInfo();
        private CareRayInterface.ExtendModeInfo extendModeInfo = new CareRayInterface.ExtendModeInfo();
        private CareRayInterface.ExpProgress expProg = new CareRayInterface.ExpProgress();
        private CareRayInterface.ExpProgress calProg = new CareRayInterface.ExpProgress();
        private CareRayInterface.ConfigInfo configInfo = new CareRayInterface.ConfigInfo();
        private CareRayInterface.UserCorrection userCorrection = new CareRayInterface.UserCorrection();
        private CareRayInterface.FrameAttr obj = new CareRayInterface.FrameAttr();
        private CareRayInterface.CalParams cal_params = new CareRayInterface.CalParams();

        /// <summary>
        /// 1800I 当前使用的工作模式；
        /// </summary>
        int checkMode = (int)CareRayInterface.CheckMode.MODE_RAD;

        public bool Enable1800IGainAlgorithm = false;

        /// <summary>
        /// 后台采集线程；
        /// </summary>
        BackgroundWorker BackgroundWorkerCaptureImageData = new BackgroundWorker();
        string identificationStr = "";
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
        /// 连接Panel的线程；
        /// </summary>
        Thread LinkPanelThread = null;
        /// <summary>
        /// 连接Panel线程使用的同步变量
        /// </summary>
        Mutex LinkPanelMutex = new Mutex();

        public Capture1800I_096Static()
        {
            LinkPanelThread = new Thread(LinkPanelThreadFun);
            LinkPanelThread.IsBackground = true;
        }

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
            int result = CareRayInterface.CR_get_mode_info(checkMode, ref modeInfo);
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

        //Show current mode information
        int showExtendModeInfo()
        {
            Console.WriteLine("\nCalling CR_get_extend_mode_info:\n");
            int result = CareRayInterface.CR_get_extend_mode_info(checkMode, ref extendModeInfo);
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR == result)
            {
                Console.WriteLine("\t\tModeId={0}\n", extendModeInfo.modeId);
                Console.WriteLine("\t\tAcqType={0}\n", extendModeInfo.acqType);
                Console.WriteLine("\t\tImageWidth={0}, ImageHeight={0}\n", extendModeInfo.imageWidth, extendModeInfo.imageHeight);
                Console.WriteLine("\t\tLinesPerPixel={0}, ColsPerPixel={0}\n", extendModeInfo.linesPerPixel, extendModeInfo.colsPerPixel);
                Console.WriteLine("\t\tImageSize(in Byte):{0}\n", extendModeInfo.imageSize);
                Console.WriteLine("\t\tMaxFrameRate:%f\n", extendModeInfo.maxFrameRate);
                Console.WriteLine("\t\tModeDescription:{0}\n", extendModeInfo.modeDescription);
                Console.WriteLine("\t\tBitStored:{0}\n", extendModeInfo.bitsStored);
                Console.WriteLine("\t\tBitAllocated:{0}\n", extendModeInfo.bitsAllocated);
            }
            return result;
        }

        #endregion

        protected override void InitParam()
        {
            ReadSetupConfig();
            ReadCaptureConfig();
            RefreshFixedPanelSettings();
            AllocAcquireMemory();
        }

        void ReadCaptureConfig()
        {
            checkMode = (int)CareRayInterface.CheckMode.MODE_RAD;

            Enable1800IGainAlgorithm = false;
        }

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
                    Console.WriteLine("Show detector info error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }

                //输出探测器状态信息；
                //Attention the Temperature, overhot_flag indicates the temperature 
                //status in current detector!!!!!!!
                result = showStatusInfo();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("Show status info error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }

                //输出探测器型号；
                int currentDetectorType = CareRayInterface.CR_get_detector_type();
                Console.WriteLine("Detector type:{0}", new object[] { ((CareRayInterface.DetectorType)currentDetectorType).ToString() });

                //设置探测器的工作模式；Rad模式；
                result = CareRayInterface.CR_set_check_mode(checkMode);
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

                
 
                //int KZSynMode = (int)SyncMode.EXT_SYNC;
                //result = CareRayInterface.CR_set_sync_mode(KZSynMode);
                //if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result && (int)KZ_ERROR_TYPE.ERR_NO_CONFIGFILE != result)
                //{
                //    Console.WriteLine("Set sync mode error, reason: {0}\n " + CareRayErrors.CrErrStrList(result));
                //}

                //int expTime = 2000;
                //int delayTime = 100;
                //int waitTime = 5;
                //result = CareRayInterface.CR_set_cycle_time(expTime, delayTime, waitTime);
                //if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                //{
                //    Console.WriteLine("Set cycle time error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                //}

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("RefreshFixedPanelSettings()...catch(1)..." + ex.Message);
            }


            try
            {
                int result;
                //设置Gain值；
                CareRayInterface.FpgaReg fpga = new CareRayInterface.FpgaReg();
                result = CareRayInterface.CR_get_fpga_reg(ref fpga);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.Write("CR_get_fpga_reg error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }

                int readoutCntr = fpga.readout_cntr;
                int gainId = (readoutCntr >> 4) & 0x00000007;
                Console.WriteLine("Gain ID =" + gainId.ToString());

                gainId = 7;

                Console.WriteLine("Set Gain ID =" + gainId.ToString());
                int readout = fpga.readout_cntr;
                int readoutCntrPrev = (readout & (~(7 << 4))) | (gainId << 4);
                fpga.readout_cntr = readoutCntrPrev;
                int prmId = (int)((uint)(&fpga.readout_cntr) - (uint)(&fpga.res1));
                prmId = 160;
                result = CareRayInterface.CR_set_detector_iparam(160, readoutCntrPrev);
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
                Console.WriteLine("Gain ID after setting =" + gainId.ToString());

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("RefreshFixedPanelSettings()...catch(2)..." + ex.Message);
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

                        BackgroundWorkerCaptureImageData.RunWorkerCompleted -= BackgroundWorkerCaptureImageData_RunWorkerCompleted;
                        BackgroundWorkerCaptureImageData.RunWorkerCompleted += BackgroundWorkerCaptureImageData_RunWorkerCompleted;

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

        void BackgroundWorkerCaptureImageData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                int result;
                result = CareRayInterface.CR_set_save_power();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_set_save_power error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }
                else
                {
                    Console.WriteLine("Set panel save power voltage successfully.");
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("CR_set_save_power():" + ex.Message);
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
        
        /// <summary>
        /// Gain算法参数设置；
        /// </summary>
        /// <param name="gain_cal"></param>
        void UseCalibrationConfig(bool gain_cal)
        {
            Console.WriteLine("UseCalibrationConfig(gain_cal={0})",new object[]{gain_cal});

            //CareRayInterface.UserCorrection userCorrection = new CareRayInterface.UserCorrection();
            try
            {
                userCorrection.fixedCorr = gain_cal;
                userCorrection.non_fixedCorr = false;
                userCorrection.portableCorr = false;

                int result = CareRayInterface.CR_set_user_correction(ref userCorrection);
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

        void RefreshPanelSettings()
        {
            try
            {
                int result;

                int KZSynMode = (int)SyncMode.EXT_SYNC;
                result = CareRayInterface.CR_set_sync_mode(KZSynMode);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result && (int)KZ_ERROR_TYPE.ERR_NO_CONFIGFILE != result)
                {
                    Console.WriteLine("Set sync mode error, reason: {0}\n " + CareRayErrors.CrErrStrList(result));
                }

                int expTime = 2000;
                int delayTime = 100;
                int waitTime = 5;
                result = CareRayInterface.CR_set_cycle_time(expTime, delayTime, waitTime);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("Set cycle time error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }



                UseCalibrationConfig(Enable1800IGainAlgorithm);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("RefreshFixedPanelSettings()...catch(1)..." + ex.Message);
            }
        }

        void RefreshPanelSettings_DiscardDarkImage()
        {
            try
            {
                int result;

                int KZSynMode = (int)SyncMode.EXT_SYNC;
                result = CareRayInterface.CR_set_sync_mode(KZSynMode);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result && (int)KZ_ERROR_TYPE.ERR_NO_CONFIGFILE != result)
                {
                    Console.WriteLine("Set sync mode error, reason: {0}\n " + CareRayErrors.CrErrStrList(result));
                }



                UseCalibrationConfig(false);

                int expTime = 1;
                int delayTime = 1;
                int waitTime = 1;
                result = CareRayInterface.CR_set_cycle_time(expTime, delayTime, waitTime);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("Set cycle time error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }



            }
            catch (System.Exception ex)
            {
                Console.WriteLine("RefreshPanelSettings_DiscarkDarkImage()...catch(1)..." + ex.Message);
            }
        }

        public override void CaptureImageData()
        {
            int result;

            //设置采集的帧数；
            //int acqImageNuber = int.MaxValue;
            //设置工作电压；
            result = CareRayInterface.CR_set_normal_power();
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
            {
                Console.WriteLine("CR_set_normal_power error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                InitLink();
            }
            else
            {
                Console.WriteLine("Set panel working voltage successfully.");
            }

            //刷新Panel的参数设置；
            RefreshPanelSettings();

            while (acqImageNuber > 0 && WorkStatus)
            {
            CaptureDarkImageInstead:

                Console.WriteLine();
                Console.WriteLine();

                acqImageNuber--;

                //调用函数开始采集；
                result = CareRayInterface.CR_start_acq_full_image();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_start_acq_image_full error, reason: " + CareRayErrors.CrErrStrList(result));
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                    WorkStatus = false;
                    return;
                }

                expProg = new CareRayInterface.ExpProgress(); //结构体：存放查询到的采集状态；
                int awaitOver = 0;
                int curExpStatus = 10000;

                DateTime tm = DateTime.Now; //log
                DateTime tmTotal = DateTime.Now; //log
                DateTime tmNoReady = DateTime.Now;//计算采集是否超时；
                double NoReady_Elapse_ms = 0;  //计算采集是否超时；        

                do
                {
                    //此函数获取扫描状态；
                    result = CareRayInterface.CR_query_prog_info((int)CareRayInterface.ProgType.CR_RAD_PROG, ref expProg);
                    if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                    {
                        Console.WriteLine("CR_query_prog_info error, reason: " + CareRayErrors.CrErrStrList(result));
                        Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                        Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                        WorkStatus = false;
                        return;
                    }

                    //采集过程时序状态改变，记录状态持续时间；
                    if (curExpStatus != expProg.expStatus)
                    {
                        curExpStatus = expProg.expStatus;
                        Console.WriteLine((CareRayInterface.ExposureStatus)curExpStatus + ":" + (DateTime.Now - tm));
                        tm = DateTime.Now;

                        if (curExpStatus == (int)CareRayInterface.ExposureStatus.CR_EXP_READY)
                        {
                            //设置状态栏进度信息；
                            int msgID2 = GenerateWinMessage("Preparing irradiation...");
                            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID2, 0);
                        }
                        else if (curExpStatus == (int)CareRayInterface.ExposureStatus.CR_EXP_WAIT_PERMISSION)
                        {
                            tmTotal = DateTime.Now; //开始采集计算时间；
                        }
                    }


                    //Thread.Sleep(50);延迟不能放在此处，不然进入死循环，永远改变不了状态（永远调用不了CR_permit_exposure()）

                    #region 设置可曝光函数；外触发必须设置状态；
                    if (expProg.expStatus == (int)CareRayInterface.ExposureStatus.CR_EXP_WAIT_PERMISSION)
                    {
                        if (0 == awaitOver)
                        {
                            Console.WriteLine("CR_permit_exposure");
                            //only permit once
                            result = CareRayInterface.CR_permit_exposure();
                            if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                            {
                                Console.WriteLine("CR_permit_exposure error, reason: " + CareRayErrors.CrErrStrList(result));
                                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                                WorkStatus = false;
                                return;
                            }

                            Console.WriteLine("awaitOver =" + awaitOver);
                            awaitOver++;
                            Console.WriteLine("awaitOver =" + awaitOver);
                        }
                    }
                    #endregion

                    #region 采集超时8s处理


                    if (curExpStatus == (int)CareRayInterface.ExposureStatus.CR_EXP_READY)
                    {
                        tmNoReady = DateTime.Now;//外触发模式的准备采集状态，不能算在超时统计里。
                    }
                    else
                    {
                        NoReady_Elapse_ms = (DateTime.Now - tmNoReady).TotalMilliseconds;
                        if (NoReady_Elapse_ms > 8000)
                        {
                            Console.WriteLine("[ERROR] Reset 1800I ready,current Panel Status:");
                            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff:") + (CareRayInterface.ExposureStatus)expProg.expStatus);
                            Console.WriteLine("awaitOver:" + awaitOver);

                            if (awaitOver == 0)
                            {
                                Console.WriteLine("Reset Ready Status...");

                                result = CareRayInterface.CR_stop_acq_frame();
                                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                                {
                                    Console.WriteLine("CR_stop_acq_frame_cleanup error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                                }
                                else
                                {
                                    //等待1s,让中断采集完成；
                                    Thread.Sleep(1000);
                                    //调用函数开始采集；
                                    result = CareRayInterface.CR_start_acq_full_image();
                                    if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                                    {
                                        Console.WriteLine("CR_start_acq_image_full error, reason: " + CareRayErrors.CrErrStrList(result));
                                        Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                                        Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                                        WorkStatus = false;
                                        return;
                                    }
                                }
                                awaitOver = 0;
                                tmNoReady = DateTime.Now;
                            }
                            else
                            {
                                Console.WriteLine("Capture a DarkImage instead.");

                                result = CareRayInterface.CR_stop_acq_frame();
                                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                                {
                                    Console.WriteLine("CR_stop_acq_frame error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                                }
                                else
                                {
                                    //等待1s,让中断采集完成；
                                    Thread.Sleep(1000);
                                    CaptureDarkImage_ContinuousMode(1);

                                    awaitOver = 0;
                                    tmNoReady = DateTime.Now;

                                    goto CaptureDarkImageInstead;

                                }
                            }
                        }

                    }


                    #endregion

                    //如果扫描结束；退出轮询状态循环；
                    if (expProg.fetchable)
                    {
                        break;
                    }


                    Thread.Sleep(50);//等待时间；

                } while (WorkStatus);
                //外部中断Cancel();立即退出；
                if (!WorkStatus)
                {
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                    WorkStatus = false;
                    return;
                }

                //改变采集到的帧数统计；
                acqBrightImageNumber++;
                //输出亮场帧数统计信息；
                Console.WriteLine("Image Number=" + acqBrightImageNumber);

                //Windows界面输出采集进度；
                int msgID = GenerateWinMessage("Capturing image number:" + acqBrightImageNumber);
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);

                //TCP/IP协议网络从Panel缓冲区取出图像；
                tm = DateTime.Now; //log
                result = CareRayInterface.CR_get_image_attr(ref this.obj);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_get_image_attr error, reason: " + CareRayErrors.CrErrStrList(result));
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                    WorkStatus = false;
                    return;
                }
                Console.WriteLine("image_width={0},image_heigth={1},pixel_bits={2}", new object[] { obj.image_width, obj.image_height, obj.pixel_bits });

                int RawFileHeadsize = 65536;
                int imageSize = this.obj.image_width * this.obj.image_height * this.obj.pixel_bits / 8 + RawFileHeadsize;
                int imageRows = obj.image_height;
                int imageColumns = obj.image_width;
                bool with_head = RawFileHeadsize > 0;

                result = CareRayInterface.CR_get_image(imageSize, with_head, this.pAcqBuffer);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_get_image error, reason: " + CareRayErrors.CrErrStrList(result));
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                    WorkStatus = false;
                    return;
                }

                Console.WriteLine("CR_get_image():(TCP/IP)Transfer image time=" + (DateTime.Now - tm));//log

                //封装数据到对象ImageObject;

                tm = DateTime.Now;

                ushort[,] imagedata = BufferToArray(this.pAcqBuffer, RawFileHeadsize, imageRows, imageColumns);
                RefreshScale();
                ImageObject imageObjectBase = new ImageObject();
                imageObjectBase.pixelSize = this.pixelSize;
                imageObjectBase.centerX = this.imageCenterX;
                imageObjectBase.centerY = this.imageCenterY;
                imageObjectBase.ImageData = imagedata;
                imageObjectBase.createTime = DateTime.Now;

                Console.WriteLine("Encapsulate image data to ImageROI time=" + (DateTime.Now - tm));//log


                Console.WriteLine("Rad image total time=" + (DateTime.Now - tmTotal));//log


                //压入堆栈；
                imgList.Enqueue(imageObjectBase);
                //发送消息到采集界面；保存数据库；  
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_DATA, (int)captureImageMode, acqBrightImageNumber);

                //采集本底丢弃；
                CaptureDarkImage_DiscardDarkImage();

            }//while frame;

            //结束设置；
            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
            WorkStatus = false;
        }

        public override void Cancel()
        {
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

        public static bool PingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(strIpOrDName, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                return false;
            }
        }

        public override void AsyncInitLink()
        {
            LinkPanelThread.Start();
        }

        private void LinkPanelThreadFun()
        {
            string PanelIP = "192.168.68.1";
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
                        LinkStatus = PanelLinkStatus.LINK_FAIL;
                    }
                }

                Thread.Sleep(200);
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
            Console.WriteLine(1);
            int result = (int)KZ_ERROR_TYPE.CR_NO_ERR;
            byte* path = (byte*)Marshal.StringToCoTaskMemAnsi(CapturePub.CareRayPath);
            result = CareRayInterface.CR_connect_detector(path);
            if (result != (int)KZ_ERROR_TYPE.CR_NO_ERR)
            {
                Console.WriteLine("Connect default detector (IP:192.168.68.1) error, reason: %s\n", CareRayErrors.CrErrStrList(result));
                Console.WriteLine("For dual_system, try to connect another detector (IP:192.168.68.2)...\n\n");
                return false;
            }
            else
            {
                Console.WriteLine("Connect detector successfully");
                return true;
            }
        }

        #endregion

        public void ThreadOffsetCalibration()
        {
            BackgroundWorker BackGroundWorkerOffsetCalibration = new BackgroundWorker();
            BackGroundWorkerOffsetCalibration.WorkerSupportsCancellation = true;
            BackGroundWorkerOffsetCalibration.DoWork -= BackGroundWorkerOffsetCalibration_DoWork;
            BackGroundWorkerOffsetCalibration.DoWork += BackGroundWorkerOffsetCalibration_DoWork;
            BackGroundWorkerOffsetCalibration.RunWorkerCompleted -= BackGroundWorkerOffsetCalibration_RunWorkerCompleted;
            BackGroundWorkerOffsetCalibration.RunWorkerCompleted += BackGroundWorkerOffsetCalibration_RunWorkerCompleted;
            BackGroundWorkerOffsetCalibration.RunWorkerAsync();
        }

        void BackGroundWorkerOffsetCalibration_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkStatus = true;
            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 2, 0);

            RefreshPanelSettings();

            int v = performOffsetCalibration();
            e.Result = v;

            WorkStatus = false;
            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
        }

        void BackGroundWorkerOffsetCalibration_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((int)e.Result == 0)
            {
                Console.WriteLine("Offset calibration successfully.");
            }
            else
            {
                Console.WriteLine("Fail to offset calibration.");
            }

        }

        public int performOffsetCalibration()
        {
            int result;

            int offsetFrameNumber = 32;
            cal_params = new CareRayInterface.CalParams(); //必须重新new
            result = CareRayInterface.CR_get_cal_params(ref cal_params);
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
            {
                Console.WriteLine("CR_get_cal_params error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
            }

            offsetFrameNumber = cal_params.ofst_cal_num;

            int msgID = GenerateWinMessage("Offset Calibration...");
            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);

            result = CareRayInterface.CR_stop_cal_procedure(0);
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
            {
                Console.WriteLine("CR_stop_cal_procedure error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
            }

            result = CareRayInterface.CR_cal_offset(checkMode);

            if (result == 1053)
            {
                Console.WriteLine("offset calibration ERROR 1053,reset CR_cal_offset");

                result = CareRayInterface.CR_stop_cal_procedure(1);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_stop_cal_procedure error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }


                result = CareRayInterface.CR_stop_acq_frame_cleanup();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_stop_acq_frame_cleanup error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }

                Thread.Sleep(1000);

                result = CareRayInterface.CR_stop_cal_procedure(0);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_stop_cal_procedure error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                }

                Console.WriteLine("Reset offset calibration...");
                result = CareRayInterface.CR_cal_offset(checkMode);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_cal_offset error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                }
            }
            else if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
            {
                Console.WriteLine("CR_cal_offset error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
            }

            calProg = new CareRayInterface.ExpProgress();
            DateTime tm = DateTime.Now;
            int frame_num = 0;

            do
            {
                result = CareRayInterface.CR_query_prog_info((int)CareRayInterface.ProgType.CR_CAL_PROG, ref calProg);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_query_prog_info error, reason: " + CareRayErrors.CrErrStrList(result));
                    break;
                }

                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != calProg.errorCode)
                {
                    Console.WriteLine("Offset calibration process error code =" + calProg.errorCode);
                    break;
                }

                if (calProg.frame_number > frame_num)
                {
                    frame_num = calProg.frame_number;
                    Console.WriteLine("Offset calibration...received dark images:" + frame_num);


                    msgID = GenerateWinMessage("Offset Calibration..." + frame_num);
                    Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
                }
                Thread.Sleep(100);

            } while (calProg.calComplete == 0 && WorkStatus);

            if (1 == calProg.calComplete)
            {
                Console.WriteLine("Offset calibration successfully.");
            }
            else
            {
                Console.WriteLine("Error occured in offset calibration");
                result = -1;
            }

            Console.WriteLine("Offset cal total time = " + (DateTime.Now  - tm));

            Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);

            return result;

        }

        public int performGainCalibrationInTwoSteps(List<ImageObject> listImages)
        {
            cal_params = new CareRayInterface.CalParams();

            int result = CareRayInterface.CR_get_cal_params(ref cal_params);
            if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
            {
                Console.WriteLine("CR_get_cal_params error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                return result;
            }

            UseCalibrationConfig(false);

            string GainPath = new string(cal_params.gain_image_dir);

            int idx = GainPath.IndexOf('\0');
            if (idx != -1)
            {
                GainPath = GainPath.Remove(idx);
            }

            string kv = "70";
            if (true)
            {
                kv = new string(cal_params.normal_cal_kv);
            }
            else
            {
                kv = new string(cal_params.portable_cal_kv);
            }

            idx = kv.IndexOf('\0');
            if (idx != -1)
            {
                kv = kv.Remove(idx);
            }



            //2 

            Console.WriteLine("Gain path:" + GainPath);

            DirectoryInfo dir = new DirectoryInfo(GainPath);
            FileInfo[] files = dir.GetFiles("*.raw", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length;i++ )
            {
                files[i].Delete();
            }

            int linear_dose_num = cal_params.linear_dose_num;
            int linear_num_per_dose = cal_params.linear_num_per_dose;

            for (int i = 0; i < linear_num_per_dose; i++)
            {
                for (int j = 0; j < linear_num_per_dose; j++)
                {
                    string path = string.Format(@"{0}\{1}kv-{2}-{3}.raw", new object[] { GainPath.TrimEnd('\\'), kv, (i + 1), (j + 1) });
                    listImages[i * linear_num_per_dose + j].SaveRawFile(path, 65536, pAcqBuffer);
                }
            }

            if (!true)
            {
                result = CareRayInterface.CR_execute_portable_cal();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_execute_portable_cal error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                    return result;
                }
            }
            else
            {

                result = CareRayInterface.CR_execute_linear_cal();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_execute_linear_cal error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                    return result;
                }
            }
            calProg = new CareRayInterface.ExpProgress();
            do
            {
                result = CareRayInterface.CR_query_prog_info((int)CareRayInterface.ProgType.CR_CAL_PROG, ref calProg);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_query_prog_info error, reason: " + CareRayErrors.CrErrStrList(result));
                    return result;
                }
                if (1 == expProg.calComplete)
                {
                    break;
                }

                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != calProg.errorCode)
                {
                    Console.WriteLine("Offset calibration process error code =" + calProg.errorCode);
                    result = calProg.errorCode;
                    break;
                }
            } while (true);

            Console.WriteLine("Gain calibration finished");


            return result;
        }

        void CaptureDarkImage_ContinuousMode(int DarkImageNumber)
        {
            Console.WriteLine();

            int result;

            for (int i = 0; i < DarkImageNumber && WorkStatus; i++)
            {
                Console.WriteLine("dark image idx = " + i);

                //调用函数开始采集；
                result = CareRayInterface.CR_start_acq_dark_full_image();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_start_acq_dark_full_image error, reason: " + CareRayErrors.CrErrStrList(result));
                    return;
                }

                expProg = new CareRayInterface.ExpProgress(); //结构体：存放查询到的采集状态；
                int curExpStatus = 10000;


                DateTime tm = DateTime.Now;
                DateTime tmTotal = DateTime.Now;
                DateTime tmNoReady = DateTime.Now;
                double NoReady_Elapse_ms = 0;

                do
                {
                    //此函数获取扫描状态；
                    result = CareRayInterface.CR_query_prog_info((int)CareRayInterface.ProgType.CR_RAD_PROG, ref expProg);
                    if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                    {
                        Console.WriteLine("CR_query_prog_info error, reason: " + CareRayErrors.CrErrStrList(result));
                        return;
                    }
                     if (curExpStatus != expProg.expStatus)
                    {
                        curExpStatus = expProg.expStatus;

                        Console.WriteLine((CareRayInterface.ExposureStatus)curExpStatus + ":" + (DateTime.Now - tm));
                        tm = DateTime.Now;
                    }

                     NoReady_Elapse_ms = (DateTime.Now - tmNoReady).TotalMilliseconds;
                     if (NoReady_Elapse_ms > 8000)
                     {
                         Console.WriteLine("[ERROR] Reset 1800I DarkImage ready,current Panel Status:");
                         Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff:") + (CareRayInterface.ExposureStatus)expProg.expStatus);

                         result = CareRayInterface.CR_stop_acq_frame();
                         if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                         {
                             Console.WriteLine("CR_stop_acq_frame_cleanup error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                         }
                         else
                         {
                             Thread.Sleep(1000);
                             //调用函数开始采集；
                             result = CareRayInterface.CR_start_acq_dark_full_image();
                             if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                             {
                                 Console.WriteLine("CR_start_acq_dark_full_image error, reason: " + CareRayErrors.CrErrStrList(result));
                                 return;
                             }

                             tmNoReady = DateTime.Now;

                         }
                     }


                  

                    //如果扫描结束；退出轮询状态循环；
                    if (expProg.fetchable)
                    {
                        break;
                    }


                    Thread.Sleep(50);//等待时间；

                } while (WorkStatus);
                //外部中断Cancel();立即退出；
                if (!WorkStatus)
                {
                    return;
                }

                //改变采集到的帧数统计；
                acqBrightImageNumber++;
                //输出亮场帧数统计信息；
                Console.WriteLine("Image Number=" + acqBrightImageNumber);

                //Windows界面输出采集进度；
                int msgID = GenerateWinMessage("Capturing image number:" + acqBrightImageNumber);
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);

                //TCP/IP协议网络从Panel缓冲区取出图像；
                tm = DateTime.Now; //log
                result = CareRayInterface.CR_get_image_attr(ref this.obj);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_get_image_attr error, reason: " + CareRayErrors.CrErrStrList(result));
                    return;
                }
                Console.WriteLine("image_width={0},image_heigth={1},pixel_bits={2}", new object[] { obj.image_width, obj.image_height, obj.pixel_bits });

                int RawFileHeadsize = 65536;
                int imageSize = this.obj.image_width * this.obj.image_height * this.obj.pixel_bits / 8 + RawFileHeadsize;
                int imageRows = obj.image_height;
                int imageColumns = obj.image_width;
                bool with_head = RawFileHeadsize > 0;

                result = CareRayInterface.CR_get_image(imageSize, with_head, this.pAcqBuffer);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    return;
                }

                Console.WriteLine("CR_get_image():(TCP/IP)Transfer image time=" + (DateTime.Now - tm));//log

                //封装数据到对象ImageObject;

                tm = DateTime.Now;

                ushort[,] imagedata = BufferToArray(this.pAcqBuffer, RawFileHeadsize, imageRows, imageColumns);
                RefreshScale();
                ImageObject imageObjectBase = new ImageObject();
                imageObjectBase.pixelSize = this.pixelSize;
                imageObjectBase.centerX = this.imageCenterX;
                imageObjectBase.centerY = this.imageCenterY;
                imageObjectBase.ImageData = imagedata;
                imageObjectBase.createTime = DateTime.Now;

                Console.WriteLine("Encapsulate image data to ImageROI time=" + (DateTime.Now - tm));//log


                Console.WriteLine("DarkImage total time=" + (DateTime.Now - tmTotal));//log
                //压入堆栈；
                imgList.Enqueue(imageObjectBase);
                //发送消息到采集界面；保存数据库；  
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_CAPTURE_DATA, (int)captureImageMode, acqBrightImageNumber);

            }
        }

        void CaptureDarkImage_DiscardDarkImage()
        {
            Console.WriteLine();

            int DarkImageNumber = 3;
            RefreshPanelSettings_DiscardDarkImage();
            int result;

            for (int i = 0; i < DarkImageNumber && WorkStatus; i++)
            {
                Console.WriteLine("dark image idx = " + i);

                //调用函数开始采集；
                result = CareRayInterface.CR_start_acq_dark_full_image();
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_start_acq_dark_full_image error, reason: " + CareRayErrors.CrErrStrList(result));
                    return;
                }

                expProg = new CareRayInterface.ExpProgress(); //结构体：存放查询到的采集状态；
                int curExpStatus = 10000;


                DateTime tm = DateTime.Now;
                DateTime tmTotal = DateTime.Now;
                DateTime tmNoReady = DateTime.Now;
                double NoReady_Elapse_ms = 0;


                do
                {
                    //此函数获取扫描状态；
                    result = CareRayInterface.CR_query_prog_info((int)CareRayInterface.ProgType.CR_RAD_PROG, ref expProg);
                    if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                    {
                        Console.WriteLine("CR_query_prog_info error, reason: " + CareRayErrors.CrErrStrList(result));
                        return;
                    }

                    if (curExpStatus != expProg.expStatus)
                    {
                        curExpStatus = expProg.expStatus;

                        Console.WriteLine((CareRayInterface.ExposureStatus)curExpStatus + ":" + (DateTime.Now - tm));
                        tm = DateTime.Now;
                    }

                    NoReady_Elapse_ms = (DateTime.Now - tmNoReady).TotalMilliseconds;
                    if (NoReady_Elapse_ms > 8000)
                    {
                        Console.WriteLine("[ERROR] Reset 1800I DarkImage ready,current Panel Status:");
                        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff:") + (CareRayInterface.ExposureStatus)expProg.expStatus);


                        result = CareRayInterface.CR_stop_acq_frame();
                        if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                        {
                            Console.WriteLine("CR_stop_acq_frame_cleanup error, reason: {0}\n", CareRayErrors.CrErrStrList(result));
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            //调用函数开始采集；
                            result = CareRayInterface.CR_start_acq_dark_full_image();
                            if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                            {
                                Console.WriteLine("CR_start_acq_dark_full_image error, reason: " + CareRayErrors.CrErrStrList(result));
                                return;
                            }

                            tmNoReady = DateTime.Now;
                        }
                    }



                    //如果扫描结束；退出轮询状态循环；
                    if (expProg.fetchable)
                    {
                        break;
                    }


                    Thread.Sleep(50);//等待时间；

                } while (WorkStatus);
                //外部中断Cancel();立即退出；
                if (!WorkStatus)
                {
                    return;
                }
                //TCP/IP协议网络从Panel缓冲区取出图像；
                tm = DateTime.Now; //log
                result = CareRayInterface.CR_get_image_attr(ref this.obj);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    Console.WriteLine("CR_get_image_attr error, reason: " + CareRayErrors.CrErrStrList(result));
                    return;
                }
                Console.WriteLine("image_width={0},image_heigth={1},pixel_bits={2}", new object[] { obj.image_width, obj.image_height, obj.pixel_bits });

                int RawFileHeadsize = 65536;
                int imageSize = this.obj.image_width * this.obj.image_height * this.obj.pixel_bits / 8 + RawFileHeadsize;
                int imageRows = obj.image_height;
                int imageColumns = obj.image_width;
                bool with_head = RawFileHeadsize > 0;

                result = CareRayInterface.CR_get_image(imageSize, with_head, this.pAcqBuffer);
                if ((int)KZ_ERROR_TYPE.CR_NO_ERR != result)
                {
                    return;
                }

                Console.WriteLine("CR_get_image():(TCP/IP)Transfer image time=" + (DateTime.Now - tm));//log
                
                Console.WriteLine("DarkImage total time=" + (DateTime.Now - tmTotal));//log


                //Windows界面输出采集进度；
                int msgID = GenerateWinMessage("Capturing dark images and discarding:" + (i + 1));
                Kernel32Interface.PostMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
            }


            RefreshPanelSettings();

        }

        void AllocAcquireMemory()
        {
            if ((IntPtr)((void*)this.pAcqBuffer) != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)((void*)this.pAcqBuffer));
                Kernel32Interface.CloseHandle((IntPtr)((void*)this.pAcqBuffer));
                this.pAcqBuffer = (ushort*)(void*)IntPtr.Zero;
            }
            this.pAcqBuffer = (ushort*)(void*)Marshal.AllocHGlobal(2816 * 2816 * 2 + 65536);
        }

        void FreeMemory()
        {
            if ((IntPtr)this.pAcqBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)((void*)this.pAcqBuffer));
                Kernel32Interface.CloseHandle((IntPtr)((void*)this.pAcqBuffer));
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
