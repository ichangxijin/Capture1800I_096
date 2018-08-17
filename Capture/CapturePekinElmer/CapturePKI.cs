using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;

namespace ImageCapturing
{
    public unsafe class CapturePKI : CaptureBase
    {
        public enum SeqBufferMode
        {
            // Storage of the sequence into two buffers.
            //Secure image acquisition by separated data transfer and later
            //performed image correction.
            HIS_SEQ_TWO_BUFFERS = 0x1,
            //Storage of the sequence into one buffer.
            //Direct acquisition and linked correction into one buffer.
            HIS_SEQ_ONE_BUFFER = 0x2,
            // All acquired single images are directly added into one buffer and after
            //acquisition divided by the number of frames, including linked
            //correction files.
            HIS_SEQ_AVERAGE = 0x4,
            //Sequence of frames using the same image buffer
            HIS_SEQ_DEST_ONE_FRAME = 0x8,
            //Skip frames after acquiring frames in a ring buffer
            HIS_SEQ_COLLATE = 0x10,
            //Continuous acquisition
            //Frames are continuously acquired into a ring buffer of dwFrames
            HIS_SEQ_CONTINUOUS = 0x100
        }

        // the synchronization mode of the frame grabber.
        public enum CaptureSynchronizationMode
        {
            HIS_SYNCMODE_SOFT_TRIGGER = 1,
            HIS_SYNCMODE_INTERNAL_TIMER = 2,
            HIS_SYNCMODE_EXTERNAL_TRIGGER = 3,
            HIS_SYNCMODE_FREE_RUNNING = 4
        }

        //sort definitions
        public enum PanelSortMode
        {
            HIS_SORT_NOSORT = 0,
            HIS_SORT_QUAD = 1,
            HIS_SORT_COLUMN = 2,
            HIS_SORT_COLUMNQUAD = 3,
            HIS_SORT_QUAD_INVERSE = 4,
            HIS_SORT_QUAD_TILE = 5,
            HIS_SORT_QUAD_TILE_INVERSE = 6,
            HIS_SORT_QUAD_TILE_INVERSE_SCRAMBLE = 7,
            HIS_SORT_OCT_TILE_INVERSE = 8,		//	1640 and 1620
            HIS_SORT_OCT_TILE_INVERSE_BINDING = 9,		//	1680
            HIS_SORT_OCT_TILE_INVERSE_DOUBLE = 10,		//	1620 reverse
            HIS_SORT_HEX_TILE_INVERSE = 11,		//	1621 ADIC
            HIS_SORT_HEX_CS = 12		//	1620/1640 continous scan
        }

        //sequence acquisition options
        public enum PanelAcqOption
        {
            HIS_SEQ_TWO_BUFFERS = 0x1,
            HIS_SEQ_ONE_BUFFER = 0x2,
            HIS_SEQ_AVERAGE = 0x4,
            HIS_SEQ_DEST_ONE_FRAME = 0x8,
            HIS_SEQ_COLLATE = 0x10,
            HIS_SEQ_CONTINUOUS = 0x100,
            HIS_SEQ_LEAKAGE = 0x1000,
            HIS_SEQ_NONLINEAR = 0x2000
        }

        //const int HIS_CAMMODE_SETSYNC = 0x8;
        //const int HIS_CAMMODE_TIMEMASK = 0x7;
        //const int HIS_CAMMODE_FPGA = 0x7F;

        public enum PanelType
        {
            HIS_BOARD_TYPE_NOONE = 0x0,
            HIS_BOARD_TYPE_ELTEC = 0x1,
            HIS_BOARD_TYPE_DIPIX = 0x2,
            HIS_BOARD_TYPE_RS232 = 0x3,
            HIS_BOARD_TYPE_USB = 0x4,
            HIS_BOARD_TYPE_ELTEC_XRD_FGX = 0x8,
            HIS_BOARD_TYPE_ELTEC_XRD_FGE_Opto = 0x16, //The communication interface to the detector is an XRD-FGe Opto frame grabber.
            HIS_BOARD_TYPE_ELTEC_GbIF = 0x32 //The detector communicates over GigabitEthernet with the host PC.
        }

        public enum ReadoutTimingMode
        {
            Timing0 = 0,
            Timing1 = 1,
            Timing2 = 2,
            Timing3 = 3,
            Timing4 = 4,
            Timing5 = 5,
            Timing6 = 6,
            Timing7 = 7
        }

        public enum ImageCorrection
        {
            None = 0,//0
            Offset = 1,// offset
            Gain = 2,// offset + gain + pixel
            GainSequence = 3 // offset +  gainsequence + pixel
        }

        public enum ElectricCapacityMode
        {
            Gain_0o25pF = 0, //0.25pF(1621 only)
            Gain_0o5pF = 1,  //0.5pF
            Gain_1pF = 2,    //1pF
            Gain_2pF = 3,    //2pF
            Gain_4pF = 4,    //4pF
            Gain_8pF = 5     //8pF
        }

        public enum ImageResolution
        {
            Nobinning = 1,        //No Binning
            Binning2x2 = 2,       //2 x 2 binning (averaged or accumulated) Square
            Binning4x4 = 3,       //4x4 binning (averaged or accumulated) Square
            Binning1x2 = 4,       //1 x 2 binning (only accumulated binning) Rectangular
            Binning1x4 = 5,       //1 x 4 binning (only accumulated binning) Rectangular
            BinningAverage = 256, //256 Averaged binning
            BinningAccumulated = 512    //Accumulated binning
        }


        public enum TriggerMode
        {
            DDD = 0,                    //Data Delivered on Demand
            DDD_withoutClearence = 1,   //Data Delivered on Demand without clearence scan
            Linewise = 2,               // (Start / Stop Trigger)
            Framewise = 3               //(default)
        }

        public class PKI_Config
        {
            /// <summary>
            /// Panel 抓图的信号同步模式，默认是内部信号同步模式
            /// </summary>
            public CaptureSynchronizationMode captureSyncMode = CaptureSynchronizationMode.HIS_SYNCMODE_INTERNAL_TIMER;
            /// <summary>
            /// Panel 解析图的时间模式，默认是最快的模式uint
            /// </summary>
            public ReadoutTimingMode readoutTimingMode = ReadoutTimingMode.Timing0;

            /// <summary>
            /// 抓取图片的校正模式，默认是GainSequence
            /// </summary>
            public ImageCorrection imageCorrection = ImageCorrection.GainSequence;
            /// <summary>
            /// Panel 设置的电容大小 16bit ushort,默认是最大的电容8pF
            /// </summary>
            public ElectricCapacityMode gianMode = ElectricCapacityMode.Gain_8pF;
            /// <summary>
            /// Panel 抓图的图像分辨率设置 16bit ushort,默认是最大的分辨率
            /// </summary>
            public ImageResolution binningMode = ImageResolution.Nobinning;

            public TriggerMode triggerMode = TriggerMode.Framewise;

            /// <summary>
            /// Panel 用内部信号模式抓图时，抓取一帧图的时间 ms
            /// </summary>
            public uint integrationTime = 2000 * 1000;
        }

        /// <summary>
        /// 系统连接Panel板连接信息句柄
        /// </summary>
        private IntPtr hAcqDesc;
        private uint pdwNumSensors;
        private int bEnableIRQ = 1;
        private int bAlwaysOpen = 0;

        /// <summary>
        /// Panel抓取图像的行数
        /// </summary>
        public uint imageRows;
        /// <summary>
        /// Panel抓取图像的列数
        /// </summary>
        public uint imageColumns;

        private PKL_Interface.CHwHeaderInfo Info = new PKL_Interface.CHwHeaderInfo();
        private PKL_Interface.CHwHeaderInfoEx InfoEx = new PKL_Interface.CHwHeaderInfoEx();

        public PKI_Config pki_config = new PKI_Config();

        private string OffsetFile = "";
        private string GainFile = "";
        private string GainSeqFile_Image = "";
        private string GainSeqFile_Dose = "";
        private string PixelMapFile = "";
        private PKL_Interface.EndAcquisitionCallBack1 EndFrameCallbackDelegate;
        private PKL_Interface.EndAcquisitionCallBack2 EndAcqCallbackDelegate;

        private ushort* pAcqBuffer = (ushort*)IntPtr.Zero;         //连续抓图和批量抓图模式存储图像的内存块
        private ushort* pAcqBufferAverage = (ushort*)IntPtr.Zero;  //抓取单张平均图像的内存块
        private ushort* pOffsetBuffer = (ushort*)IntPtr.Zero;      // offset校正图像的内存块
        private uint* pGainBuffer = (uint*)IntPtr.Zero;            // gain校正图像的内存块
        private ushort* pGainSeqBuffer_Image = (ushort*)IntPtr.Zero;     // gainsequence校正图像的内存块
        private ushort* pGainSeqMedBuffer_Image = (ushort*)IntPtr.Zero;  // gainsequence校正产生相关变量的内存块
        private int gainSeqFrames_Image;    //用于gainsequence校正的序列图的张数
        private ushort* pGainSeqBuffer_Dose = (ushort*)IntPtr.Zero;     // gainsequence校正图像的内存块
        private ushort* pGainSeqMedBuffer_Dose = (ushort*)IntPtr.Zero;  // gainsequence校正产生相关变量的内存块
        private int gainSeqFrames_Dose;    //用于gainsequence校正的序列图的张数

        private int* pdwPxlCorrList = (int*)IntPtr.Zero;           // pixel校正图像的内存块



        private int dynamicOffsetImageNum = 5;       //动态抓取offset校正图像的张数
        public bool dynamicOffsetPreAcq = true;     //标识是否每次抓图时都抓取Offset本底校正图像
        private bool captureOffsetData = false;      //标识是否正在抓取Offset数据

        private int acqFrameSN;                      //实时统计抓到了多少张图

        private int beamON_Num = 0;                  //记录多次曝光抓图的次数

        public bool DoseScaleMode = false;         //是否是剂量刻度模式

        public bool DoCorrection = true;

        //private System.Timers.Timer linkPanelTimer = new System.Timers.Timer();
        private int continuesNum = 0;

        bool actionTriggerON = false;

        public CapturePKI()
            : base()
        {
            //linkPanelTimer.Elapsed -= new System.Timers.ElapsedEventHandler(linkPanelTimer_Elapsed);
            //linkPanelTimer.Elapsed += new System.Timers.ElapsedEventHandler(linkPanelTimer_Elapsed);
            //linkPanelTimer.Interval = 1000 * 5;

            ////创建接收线程
            //ThreadStart threadDelegate = new ThreadStart(Receive);
            //if (ReceiveThread != null && ReceiveThread.IsAlive)
            //{
            //    ReceiveThread.Abort();
            //}
            //ReceiveThread = new Thread(threadDelegate);

            //this.AsyncInitLink();
            EndFrameCallbackDelegate = new PKL_Interface.EndAcquisitionCallBack1(OnEndFrameCallback);
            EndAcqCallbackDelegate = new PKL_Interface.EndAcquisitionCallBack2(OnEndAcqCallback);
        }

        protected override void InitParam()
        {
            ReadSetupConfig();
            ReadCaptureConfig();
            RefreshAcquisitionStructureParameters();
            SetLinkCorrection();
        }

        public void SetLinkCorrection()
        {
            //string CorrectFilePath = CapturePub.LinkFilePath + "\\";
            OffsetFile = CapturePub.readCaptrueValue(XmlField.OffsetFile);
            GainFile = CapturePub.readCaptrueValue(XmlField.GainFile);
            GainSeqFile_Image = CapturePub.readCaptrueValue(XmlField.GainSeqFile_Image);
            GainSeqFile_Dose = CapturePub.readCaptrueValue(XmlField.GainSeqFile_Dose);
            PixelMapFile = CapturePub.readCaptrueValue(XmlField.PixelMapFile);

            LinkOffsetImage();
            LinkGain();
            LinkGainSequence_Image();
            LinkGainSequence_Dose();
            LinkPixelCorrection();
        }

        //Read capture parameter from xml file
        public void ReadCaptureConfig()
        {

            int capmode;
            int imageCorrection;
            int gianMode;
            int binningMode;
            int integrationTime;
            int xmlFrameNum;
            int xmlTriggerMode;
            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.CaptureMode, false), out capmode))
            {
                captureImageMode = (PanelCaptureMode)capmode;
            }
            else
            {
                captureImageMode = PanelCaptureMode.Sequence;
            }
            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.ImageCorrection, false), out imageCorrection))
            {
                pki_config.imageCorrection = (ImageCorrection)imageCorrection;
            }
            else
            {
                pki_config.imageCorrection = ImageCorrection.GainSequence;
            }
            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.GainMode, false), out gianMode))
            {
                pki_config.gianMode = (ElectricCapacityMode)gianMode;
            }
            else
            {
                pki_config.gianMode = ElectricCapacityMode.Gain_8pF;
            }
            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.BinningMode, false), out binningMode))
            {
                pki_config.binningMode = (ImageResolution)binningMode;
            }
            else
            {
                pki_config.binningMode = ImageResolution.Nobinning;
            }
            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.IntegrationTime, false), out integrationTime))
            {
                pki_config.integrationTime = (uint)(integrationTime * 1000);
            }
            else
            {
                pki_config.integrationTime = 1000 * 1000;
            }

            if (!int.TryParse(CapturePub.readCaptrueValue(XmlField.CaptureFrameCount), out xmlFrameNum))
            {
                xmlFrameNum = 1;
            }
            xmlFrameNum = 1;

            if (int.TryParse(CapturePub.readCaptrueValue(XmlField.TriggerMode, false), out xmlTriggerMode))
            {
                pki_config.triggerMode = (TriggerMode)xmlTriggerMode;
            }
            else
            {
                pki_config.triggerMode = TriggerMode.Framewise;
            }


            if (xmlFrameNum != FrameCount)
            {
                FrameCount = xmlFrameNum;
                if ((IntPtr)pAcqBuffer != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal((IntPtr)pAcqBuffer);
                    Kernel32Interface.CloseHandle((IntPtr)pAcqBuffer);
                    pAcqBuffer = (ushort*)IntPtr.Zero;
                }
            }
        }

        protected override void Trigger_TriggerChanged(TRIGGER_STATUS status)
        {
            if ((TRIGGER_STATUS)status == TRIGGER_STATUS.ON)
            {
                if (captureImageMode != PanelCaptureMode.Continuous)
                {
                    Trigger.Stop();
                }
                ActionTriggerON();
            }
            else if ((TRIGGER_STATUS)status == TRIGGER_STATUS.OFF)
            {
                if (WorkStatus)
                {
                    if (captureImageMode == PanelCaptureMode.Continuous)
                    {
                        Cancel();
                    }
                    CaptureImageData();
                }
            }
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_TRIGGER_CHANGE, (int)status, 0);
        }

        public override void Cancel()
        {
            if (PKL_Interface.Acquisition_IsAcquiringData(hAcqDesc) == 1)
            {
                for (int i = 0; i < 10; i++)
                {
                    Thread thread = new Thread(CancelCapture);
                    thread.Start();
                    System.Threading.Thread.Sleep(100);
                    if (PKL_Interface.Acquisition_IsAcquiringData(hAcqDesc) != 1)
                    {
                        //LogTest.WriteLog(SaveFilePath + "\\log.txt", "Thread abort number: " + (i + 1).ToString());
                        break;
                    }
                    else
                    {
                        thread.Abort();
                    }
                }
            }
            Trigger.Stop();
            WorkStatus = false;
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
            beamON_Num = 0;
            base.Cancel();
        }

        private void CancelCapture()
        {
            PKL_Interface.Acquisition_Abort(hAcqDesc);
        }

        public override void Stop()
        {
            try
            {
                base.Stop();
                if (Trigger != null)
                {
                    Trigger.Stop();
                    Trigger.ProgressBegin -= new ProgressBase.ProgressBeginDelegate(Trigger_ProgressBegin);
                    Trigger.TriggerChanged -= new TriggerBase.TriggerChangedDelegate(Trigger_TriggerChanged);
                    Trigger.TriggerStatus -= new TriggerBase.TriggerChangedDelegate(Trigger_TriggerStatus);
                    //linkPanelTimer.Elapsed -= new System.Timers.ElapsedEventHandler(linkPanelTimer_Elapsed);
                    Trigger = null;
                }
                //if (linkPanelTimer != null)
                //{
                //    linkPanelTimer.Elapsed -= new System.Timers.ElapsedEventHandler(linkPanelTimer_Elapsed);
                //    linkPanelTimer.Stop();
                //}

                //PKL_Interface.Acquisition_Abort(hAcqDesc);
                PKL_Interface.Acquisition_Close(hAcqDesc);
                PKL_Interface.Acquisition_CloseAll();
                EndAcqCallbackDelegate = null;
                EndFrameCallbackDelegate = null;
                FreeMemory();
                WorkStatus = false;
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
            }
            catch
            { }
        }

        //Refresh captrue parameter to base acquisition data structure(c++ library structure)
        public void RefreshAcquisitionStructureParameters()
        {
            uint bakRows = imageRows;

            Console.WriteLine("Refresh captrue parameter to base acquisition data structure(c++ library structure).");

            //PKL_Interface.Acquisition_SetFrameSyncMode(hAcqDesc, (uint)CaptureSynchronizationMode.HIS_SYNCMODE_FREE_RUNNING);
            //PKL_Interface.Acquisition_SetCameraMode(hAcqDesc, (uint)pki_config.readoutTimingMode);
            PKL_Interface.Acquisition_SetCameraGain(hAcqDesc, (ushort)pki_config.gianMode);
            PKL_Interface.Acquisition_SetCameraBinningMode(hAcqDesc, (ushort)pki_config.binningMode);
            uint dwFrames;
            uint dwDataType;
            uint dwSortFlags;
            int bIRQEnabled;
            uint dwAcqType;
            uint dwSystemID;
            uint dwSyncMode;
            uint dwHwAccess;
            PKL_Interface.Acquisition_GetConfiguration(hAcqDesc, out dwFrames, out imageRows,
                out imageColumns, out dwDataType, out dwSortFlags, out bIRQEnabled,
                out dwAcqType, out dwSystemID, out dwSyncMode, out dwHwAccess);
            if (bakRows != imageRows)
            {
                RefreshScale();
                //float scaleToDRR = SAD / SID;
                //pixelSize = phySize * scaleToDRR / imageColumns;
                //imageCenterX = phyCenterX * scaleToDRR / pixelSize;
                //imageCenterY = phyCenterY * scaleToDRR / pixelSize;
            }

            if (pAcqBuffer == (ushort*)IntPtr.Zero || bakRows != imageRows)
            {
                AllocAcquireMemory();

            }
            if (pAcqBufferAverage == (ushort*)IntPtr.Zero || bakRows != imageRows)
            {
                AllocAcquireOneImageMemory();
            }
        }

        
        public void SetImageCorrection(ImageCorrection imgcorrection)
        {
            pki_config.imageCorrection = imgcorrection;
            CapturePub.saveCaptrueValue(XmlField.ImageCorrection, ((int)imgcorrection).ToString());
        }

        public override void Start()
        {
            PKL_Interface.Acquisition_SetFrameSyncMode(hAcqDesc, (uint)CaptureSynchronizationMode.HIS_SYNCMODE_INTERNAL_TIMER);
            PKL_Interface.Acquisition_SetTimerSync(hAcqDesc, ref pki_config.integrationTime);
            PKL_Interface.Acquisition_SetCameraMode(hAcqDesc, (uint)pki_config.readoutTimingMode);


            imgList.Clear();
            WorkStatus = true;
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 1, 0);
            if (dynamicOffsetPreAcq)
            {
                if (captureImageMode == PanelCaptureMode.DoubleExposure && beamON_Num == 1)
                {
                    int msgID = GenerateWinMessage("Preparing irradiation and capturing...");
                    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
                    Trigger.Start();
                }
                else
                {
                    if (DynamicAcqOffsetCorrPreAcq() == false)
                    {
                        Cancel();
                        return;
                    }
                }
            }
            else
            {
                int msgID = GenerateWinMessage("Preparing irradiation and capturing...");
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
                Trigger.Start();
            }
        }

        private bool DynamicAcqOffsetCorrPreAcq()
        {
            captureOffsetData = true;

            int msgID = GenerateWinMessage("Capturing offset images...");


            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, dynamicOffsetImageNum);
            if ((IntPtr)pOffsetBuffer == IntPtr.Zero)
            {
                pOffsetBuffer = (ushort*)Marshal.AllocHGlobal((int)(imageRows * imageColumns * sizeof(short)));
            }

            uint v = PKL_Interface.Acquisition_Acquire_OffsetImage(hAcqDesc, pOffsetBuffer, imageRows, imageColumns, (uint)dynamicOffsetImageNum);
            if (v == 1)
            {
                //int dwHISError = -1;
                //int dwBoardError = -1;
                //uint vR = PKL_Interface.Acquisition_GetErrorCode(hAcqDesc, ref dwHISError, ref dwBoardError);
                //if (vR != 0 || dwHISError == 0)
                //{
                LinkStatus = PanelLinkStatus.LINK_FAIL;
                return false;
                //}
            }
            return true;
        }

        private void ActionTriggerON()
        {
            actionTriggerON = true;
            acqFrameSN = 0;
            RefreshScale();

            if (CapturePub.readCaptrueValue("TriggerON_withoutClearenceScan", false) == "F")
            {
                int msgID = GenerateWinMessage("Receive trigger on signal,start clearence scan...");
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
                PKL_Interface.Acquisition_SetFrameSyncMode(hAcqDesc, (uint)CaptureSynchronizationMode.HIS_SYNCMODE_INTERNAL_TIMER);
                PKL_Interface.Acquisition_SetTimerSync(hAcqDesc, ref pki_config.integrationTime);
                PKL_Interface.Acquisition_SetCameraMode(hAcqDesc, (uint)pki_config.readoutTimingMode);

                PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBuffer, (uint)FrameCount, imageRows, imageColumns);
                PKL_Interface.Acquisition_Acquire_Image(hAcqDesc, (uint)1, 0, (uint)SeqBufferMode.HIS_SEQ_ONE_BUFFER, null, null, null);

            }


            captureImageMode = PanelCaptureMode.Sequence;
            PKL_Interface.Acquisition_SetCameraTriggerMode(hAcqDesc, 3);

            PKL_Interface.Acquisition_SetFrameSyncMode(hAcqDesc, (uint)CaptureSynchronizationMode.HIS_SYNCMODE_SOFT_TRIGGER);
            PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBuffer, (uint)FrameCount, imageRows, imageColumns);
            PKL_Interface.Acquisition_Acquire_Image(hAcqDesc, (uint)FrameCount, 0, (uint)SeqBufferMode.HIS_SEQ_ONE_BUFFER, null, null, null);

        }

        public override void CaptureImageData()
        {
            actionTriggerON = false;
            int msgID = GenerateWinMessage("Receive Trigger off signal,start to capture images...");
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);

            PKL_Interface.Acquisition_SetFrameSync(hAcqDesc);

            //if (pki_config.captureImageMode == PanelCaptureMode.Sequenc
            //{
            //    PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBuffer, (uint)FrameCount, imageRows, imageColumns);
            //    PKL_Interface.Acquisition_Acquire_Image(hAcqDesc, (uint)FrameCount, 0, (uint)SeqBufferMode.HIS_SEQ_ONE_BUFFER, null, null, null);
            //    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, FrameCount);
            //}
            //else if (pki_config.captureImageMode == PanelCaptureMode.Continuous)
            //{
            //    continuesNum = 0;
            //    PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBuffer, (uint)FrameCount, imageRows, imageColumns);
            //    PKL_Interface.Acquisition_Acquire_Image(hAcqDesc, (uint)FrameCount, 0, (uint)SeqBufferMode.HIS_SEQ_CONTINUOUS, null, null, null);
            //    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
            //}
            //else if (pki_config.captureImageMode == PanelCaptureMode.Average)
            //{
            //    PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBufferAverage, (uint)FrameCount, imageRows, imageColumns);
            //    PKL_Interface.Acquisition_Acquire_Image(hAcqDesc, (uint)FrameCount, 0, (uint)SeqBufferMode.HIS_SEQ_AVERAGE, pOffsetBuffer, null, null);
            //    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, FrameCount);
            //}
            //else if (pki_config.captureImageMode == PanelCaptureMode.AverageWithGainSequence)
            //{
            //    PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBufferAverage, (uint)FrameCount, imageRows, imageColumns);
            //    if (DoseScaleMode)
            //    {
            //        PKL_Interface.Acquisition_Acquire_Image_Ex(hAcqDesc, (uint)FrameCount, 0, (uint)SeqBufferMode.HIS_SEQ_AVERAGE, 
            //            pOffsetBuffer, (uint)gainSeqFrames_Dose, pGainSeqBuffer_Dose, pGainSeqMedBuffer_Dose, null, pdwPxlCorrList);
            //    }
            //    else
            //    {
            //        PKL_Interface.Acquisition_Acquire_Image_Ex(hAcqDesc, (uint)FrameCount, 0, (uint)SeqBufferMode.HIS_SEQ_AVERAGE,
            //            pOffsetBuffer, (uint)gainSeqFrames_Image, pGainSeqBuffer_Image, pGainSeqMedBuffer_Image, null, pdwPxlCorrList);
            //    }
            //    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, FrameCount);
            //}
            //else if (pki_config.captureImageMode == PanelCaptureMode.DoubleExposure)
            //{
            //    beamON_Num++;
            //    PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBuffer, (uint)FrameCount, imageRows, imageColumns);
            //    PKL_Interface.Acquisition_Acquire_Image(hAcqDesc, (uint)FrameCount, 0, (uint)SeqBufferMode.HIS_SEQ_ONE_BUFFER, null, null, null);
            //    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
            //}
        }

        private void OnEndFrameCallback(IntPtr hAcqDesc)
        {
            if (captureOffsetData)
            {
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -1, 1); //-1代表进度Step,-2代表END
                return;
            }

            if (actionTriggerON)
            {
                return;
            }

            //System.Threading.Thread.Sleep(frameDelayTime); //软件自定义附加的抓图前的延迟时间
            ushort* p = pAcqBuffer + imageRows * imageColumns * acqFrameSN;
            switch (pki_config.imageCorrection)
            {
                case ImageCorrection.None://none 0
                    break;

                case ImageCorrection.Offset://offset 1
                    try
                    {
                        PKL_Interface.Acquisition_DoOffsetCorrection(p, p, pOffsetBuffer, (int)(imageColumns * imageRows));
                    }
                    catch (Exception e)
                    {
                        DoCorrection = false;
                    }
                    break;
                case ImageCorrection.Gain://offset_gain 2
                    try
                    {
                        PKL_Interface.Acquisition_DoOffsetGainCorrection(p, p, pOffsetBuffer, pGainBuffer, (int)(imageColumns * imageRows));
                        PKL_Interface.Acquisition_DoPixelCorrection(p, pdwPxlCorrList);
                    }
                    catch (Exception e)
                    {
                        DoCorrection = false;
                    }
                    break;
                case ImageCorrection.GainSequence://3
                    try
                    {
                        if (DoseScaleMode)
                        {
                            PKL_Interface.Acquisition_DoOffsetGainCorrection_Ex(p, p, pOffsetBuffer, pGainSeqBuffer_Dose, pGainSeqMedBuffer_Dose,
                                     (int)(imageColumns * imageRows), gainSeqFrames_Dose);
                        }
                        else
                        {
                            PKL_Interface.Acquisition_DoOffsetGainCorrection_Ex(p, p, pOffsetBuffer, pGainSeqBuffer_Image, pGainSeqMedBuffer_Image,
                                    (int)(imageColumns * imageRows), gainSeqFrames_Image);
                        }
                        PKL_Interface.Acquisition_DoPixelCorrection(p, pdwPxlCorrList);
                    }
                    catch (Exception e)
                    {
                        DoCorrection = false;
                    }
                    break;
            }
            acqFrameSN++;
            List<ushort[,]> image = BufferToArrayList(p, 1, (int)imageRows, (int)imageColumns);
            ImageObject imgObj = new ImageObject();
            imgObj.pixelSize = pixelSize;
            imgObj.centerX = imageCenterX;
            imgObj.centerY = imageCenterY;
            imgObj.ImageData = image[0];
            imgObj.createTime = DateTime.Now;
            imgList.Enqueue(imgObj);
            if (captureImageMode == PanelCaptureMode.Continuous)
            {
                int msgID = GenerateWinMessage("Capturing image number：" + (++continuesNum));
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_DATA, (int)captureImageMode, (int)pki_config.imageCorrection);

                if (acqFrameSN == FrameCount)
                {
                    acqFrameSN = 0;
                }
            }
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -1, 1);
            PKL_Interface.Acquisition_GetReady(hAcqDesc);
        }

        private void OnEndAcqCallback(IntPtr hAcqDesc)
        {
            if (captureOffsetData)
            {
                captureOffsetData = false;
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);

                int msgID = GenerateWinMessage("Preparing irradiation and capturing...");
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
                Trigger.Start();

                return;
            }

            if (actionTriggerON)
            {
                actionTriggerON = false;
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                return;
            }
            if (captureImageMode == PanelCaptureMode.Sequence)
            {
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_DATA, (int)captureImageMode, (int)pki_config.imageCorrection);
            }
            else if (captureImageMode == PanelCaptureMode.DoubleExposure)
            {
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_DATA, (int)captureImageMode, beamON_Num);
            }

            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_TRIGGER_STATUS, (int)TRIGGER_STATUS.OFF, 0);
            WorkStatus = false;
            Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_WORKSTATUS, 0, 0);
            if (captureImageMode == PanelCaptureMode.DoubleExposure)
            {
                if (beamON_Num == 2)
                {
                    beamON_Num = 0;
                }
                else
                {
                    Start();
                }
            }
        }

        public unsafe static List<ushort[,]> BufferToArrayList(ushort* Buffer, int Frames, int Row, int Col)
        {
            ushort* p = Buffer;
            List<ushort[,]> list = new List<ushort[,]>();
            for (int n = 0; n < Frames; n++)
            {
                ushort[,] data = new ushort[Row, Col];
                list.Add(data);
                for (int i = 0; i < Row; i++)
                {
                    for (int j = 0; j < Col; j++)
                    {
                        data[i, j] = *p;
                        p++;
                    }
                }
            }
            return list;
        }

        private unsafe static ushort* ArrayListToBuffer(List<ushort[,]> list)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            int Frames = list.Count;
            int Rows = list[0].GetLength(0);
            int Cols = list[0].GetLength(1);
            ushort* Buffer = (ushort*)System.Runtime.InteropServices.Marshal.AllocHGlobal((int)(Frames * Rows * Cols * sizeof(short)));
            ushort* p = Buffer;
            for (int n = 0; n < Frames; n++)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        *p = list[n][i, j];
                        p++;
                    }
                }
            }
            return Buffer;
        }

        public override void AsyncInitLink()
        {
            if (LinkStatus == PanelLinkStatus.LINK_SUCCESS || LinkStatus == PanelLinkStatus.LINKING)
            {
                return;
            }
            LinkStatus = PanelLinkStatus.LINKING;

            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new DoWorkEventHandler(ThreadDoWork);
            bw.RunWorkerAsync();
        }

        private void ThreadDoWork(object sender, DoWorkEventArgs e)
        {
            if (hAcqDesc != IntPtr.Zero)
            {
                try
                {
                    //PKL_Interface.Acquisition_Abort(hAcqDesc);
                    PKL_Interface.Acquisition_Close(hAcqDesc);
                }
                catch
                { }

            }

            bool flag = false;
            while(!flag)
            {
                int msgID = GenerateWinMessage("Connecting panel...");
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);

                //return InitNetworkLink();
                uint nRet;
                const int HIS_ALL_OK = 0;
                nRet = PKL_Interface.Acquisition_EnumSensors(out pdwNumSensors, bEnableIRQ, bAlwaysOpen);
                if (nRet == HIS_ALL_OK && pdwNumSensors > 0)
                {

                    //Iterate through all sensors, and select the last sensor
                    uint Pos = 0;
                    do
                    {
                        nRet = PKL_Interface.Acquisition_GetNextSensor(ref Pos, out hAcqDesc);
                    } while (Pos != 0);

                    if (nRet == HIS_ALL_OK)
                    {
                        flag = true;
                        msgID = GenerateWinMessage("Link successfully.");
                    }
                    else
                    {
                        msgID = GenerateWinMessage("in Fail to search sensor, return value:" + nRet + "sensor Number:" + pdwNumSensors);
                        Console.WriteLine(" in Fail to search sensor, return value:" + nRet + "sensor Number:" + pdwNumSensors);
                    }
                }
                else
                {
                    msgID = GenerateWinMessage(" out Fail to search sensor, return value:" + nRet + "sensor Number:" + pdwNumSensors);
                    Console.WriteLine("out Fail to search sensor, return value:" + nRet + "sensor Number:" + pdwNumSensors);
                }
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, msgID, 0);
                Thread.Sleep(4000);
            }//end while

            LinkStatus = PanelLinkStatus.LINK_SUCCESS;
            EndFrameCallbackDelegate = new PKL_Interface.EndAcquisitionCallBack1(OnEndFrameCallback);
            EndAcqCallbackDelegate = new PKL_Interface.EndAcquisitionCallBack2(OnEndAcqCallback);
            PKL_Interface.Acquisition_SetCallbacksAndMessages(hAcqDesc, new IntPtr(), 0, 0, EndFrameCallbackDelegate, EndAcqCallbackDelegate);
            InitParam();
        }

        private bool InitNetworkLink()
        {
            Console.WriteLine("Start to initilize network communication Flat detecter panel.");
            uint nRet;
            const int HIS_ALL_OK = 0;
            int plNrOfboards = 0;

            bool searchDevice = false;

            string panelMac = CapturePub.readCaptrueValue("PanelMacAddress", false);
            byte[] byteMac = PKL_Interface.StringToCChar(panelMac);
            byte* addressBuffer = (byte*)Marshal.AllocHGlobal(16 * sizeof(byte));
        SearchDevice:
            if (byteMac.Length != 16)
            {
                Console.WriteLine("Try to find out network communication Flat detecter panel.");
                searchDevice = true;
                nRet = PKL_Interface.Acquisition_GbIF_GetDeviceCnt(&plNrOfboards);
                if (nRet != 0 || plNrOfboards == 0)
                {
                    WriteError("Fail to get device number(Acquisition_GbIF_GetDeviceCnt).  @@ " + nRet + "(Return Value) " + plNrOfboards + "(Number of Flat detecter panel)");
                    return false;
                }
                else
                {
                    Console.WriteLine("Get device number(Acquisition_GbIF_GetDeviceCnt).  @@ " + nRet + "(Return Value) " + plNrOfboards + "(Number of Flat detecter panel)");
                }
                PKL_Interface.GBIF_DEVICE_PARAM[] deviceParameter = new PKL_Interface.GBIF_DEVICE_PARAM[plNrOfboards];
                for (int i = 0; i < plNrOfboards; i++)
                {
                    deviceParameter[i] = new PKL_Interface.GBIF_DEVICE_PARAM();
                }
                nRet = PKL_Interface.Acquisition_GbIF_GetDeviceList(ref deviceParameter[0], plNrOfboards);

                if (nRet != 0 || deviceParameter.Length == 0)
                {
                    WriteError("Fail to get device list(Acquisition_GbIF_GetDeviceList).  @@ " + nRet + "(Return Value) " + deviceParameter.Length + "(Number of Flat detecter panel)");
                    return false;
                }
                else
                {
                    Console.WriteLine("Get device list(Acquisition_GbIF_GetDeviceList).  @@ " + nRet + "(Return Value) " + deviceParameter.Length + "(Number of Flat detecter panel)");
                }
                byteMac = deviceParameter[0].ucMacAddress;
                CapturePub.saveCaptrueValue("PanelMacAddress", PKL_Interface.CCharToString(byteMac));
            }
            byte* pTemp = addressBuffer;
            for (int i = 0; i < 16; i++)
            {
                *pTemp = byteMac[i];
                pTemp++;
            }
            nRet = PKL_Interface.Acquisition_GbIF_Init(ref hAcqDesc, 0, 0, 2048, 2048,
                                                          1, 1, PKL_Interface.HIS_GbIF_MAC, addressBuffer);

            if (nRet != 0)
            {
                if (!searchDevice)
                {
                    byteMac = new byte[] { };
                    goto SearchDevice;
                }
                else
                {
                    WriteError("Fail to initilize flat detecter(Acquisition_GbIF_Init).  @@ " + nRet + "(Return Value) ");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Initilize detecter successfully(Acquisition_GbIF_Init).  @@ " + nRet + "(Return Value) ");
            }
            //PKL_Interface.GBIF_DEVICE_PARAM deviceParameter1 = new PKL_Interface.GBIF_DEVICE_PARAM();
            //nRet = PKL_Interface.Acquisition_GbIF_GetDevice(addressBuffer, PKL_Interface.HIS_GbIF_MAC, ref deviceParameter1);
            //if (nRet != 0)
            //{
            //    return false;
            //}WriteToFile
            //deviceParameter1.("Get device information by address...");//log

            if ((IntPtr)addressBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)addressBuffer);
                Kernel32Interface.CloseHandle((IntPtr)addressBuffer);
                addressBuffer = (byte*)IntPtr.Zero;
            }
            //ask for communication device type and its number
            //if (PKL_Interface.Acquisition_GetCommChannel(hAcqDesc, out nChannelType, out nChannelNr) != HIS_ALL_OK)
            //{
            //    return false;
            //}
            //if (CapturePub.readCaptrueValue("FPDInterface", false) == "2")
            //{
            //    PKL_Interface.Acquisition_SetCameraBinningMode(hAcqDesc, dwBinning);
            //}
            //ask for data organization of all sensors
            //if ((nRet = PKL_Interface.Acquisition_GetConfiguration(hAcqDesc, out dwFrames, out dwRows,
            //    out dwColumns, out dwDataType, out dwSortFlags, out bEnableIRQ,
            //    out dwAcqType, out dwSystemID, out dwSyncMode, out dwHwAccess)) != HIS_ALL_OK)
            //{
            //    return false;
            //}



            //LogFPD.WriteLog("Column: " + dwColumns);
            //LogFPD.WriteLog("Row: " + dwRows);
            //string strPKI = "Column: " + dwColumns + "Row: " + dwRows;
            //LogTest.WriteLog(SaveFilePath + "\\log.txt", strPKI);

            ////if ((nRet = PKL_Interface.Acquisition_GetHwHeaderInfoEx(hAcqDesc, ref Info, ref InfoEx)) == HIS_ALL_OK)
            ////{
            ////    dwGain = InfoEx.wGain;
            ////}

            ////PKL_Interface.GetCameraBinningMode(hAcqDesc, out dwBinning);
            //PKL_Interface.GBIF_DEVICE_PARAM deviceParameter2 = new PKL_Interface.GBIF_DEVICE_PARAM();
            //nRet = PKL_Interface.Acquisition_GbIF_GetDeviceParams(hAcqDesc, ref deviceParameter2);
            //if (nRet != 0)
            //{
            //    return false;
            //}
            //deviceParameter2.WriteToFile("Get device information by open information...");//log

            //int uiBootOptions = PKL_Interface.HIS_GbIF_IP_LLA;
            //byte[] cDefIP = new byte[16];
            //byte[] cDefSubNetMask = new byte[16];
            //byte[] cStdGateway = new byte[16];
            //nRet = PKL_Interface.Acquisition_GbIF_GetConnectionSettings(addressTemp, ref uiBootOptions, cDefIP, cDefSubNetMask, cStdGateway);
            //if (nRet == 0)
            //{
            //    LogFPD.WriteLogStartTag("Connection settings...");
            //    LogFPD.WriteLog("addressTemp -- " + PKL_Interface.CCharToString(addressTemp));
            //    LogFPD.WriteLog("uiBootOptions -- " + uiBootOptions.ToString());
            //    LogFPD.WriteLog("cDefIP -- " + PKL_Interface.CCharToString(cDefIP));
            //    LogFPD.WriteLog("cDefSubNetMask -- " + PKL_Interface.CCharToString(cDefSubNetMask));
            //    LogFPD.WriteLog("cStdGateway -- " + PKL_Interface.CCharToString(cStdGateway));
            //}
            //short wTiming = 0;
            //int lPacketDelay = 0;
            //int lMaxNetworkLoadPercent = 100;
            //nRet = PKL_Interface.Acquisition_GbIF_CheckNetworkSpeed(hAcqDesc, ref wTiming, ref lPacketDelay, lMaxNetworkLoadPercent);
            //if (nRet == 0)
            //{
            //    LogFPD.WriteLogStartTag("Network Speed...");
            //    LogFPD.WriteLog("wTiming -- " + wTiming.ToString());
            //    LogFPD.WriteLog("lPacketDelay -- " + lPacketDelay.ToString());
            //    LogFPD.WriteLog("lMaxNetworkLoadPercent -- " + lMaxNetworkLoadPercent.ToString());
            //}

            //lPacketDelay = 0;
            //nRet = PKL_Interface.Acquisition_GbIF_GetPacketDelay(hAcqDesc, ref lPacketDelay);
            //if (nRet == 0)
            //{
            //    LogFPD.WriteLogStartTag("Get packet delay by open information...");
            //    LogFPD.WriteLog("lPacketDelay -- " + lPacketDelay.ToString());
            //}

            ////PKL_Interface.GBIF_Detector_Properties properties = new PKL_Interface.GBIF_Detector_Properties();
            ////nRet = PKL_Interface.Acquisition_GBIF_GetDetectorProperties(hAcqDesc, ref properties);
            ////if (nRet == 0)
            ////{
            ////    properties.WriteToFile("Get detector properties by open information...");
            ////}

            return true;
        }

        private void AllocAcquireMemory()
        {
            if ((IntPtr)pAcqBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pAcqBuffer);
                Kernel32Interface.CloseHandle((IntPtr)pAcqBuffer);
                pAcqBuffer = (ushort*)IntPtr.Zero;
            }
            pAcqBuffer = (ushort*)Marshal.AllocHGlobal((int)(FrameCount * imageRows * imageColumns * sizeof(short)));
        }

        private void AllocAcquireOneImageMemory()
        {
            if ((IntPtr)pAcqBufferAverage != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pAcqBufferAverage);
                Kernel32Interface.CloseHandle((IntPtr)pAcqBufferAverage);
                pAcqBufferAverage = (ushort*)IntPtr.Zero;
            }
            pAcqBufferAverage = (ushort*)Marshal.AllocHGlobal((int)(imageRows * imageColumns * sizeof(short)));
        }

        private void LinkOffsetImage()
        {
            if ((IntPtr)pOffsetBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pOffsetBuffer);
                Kernel32Interface.CloseHandle((IntPtr)pOffsetBuffer);
                pOffsetBuffer = (ushort*)IntPtr.Zero;
            }
            if (File.Exists(OffsetFile))
            {
                List<ushort[,]> list = HisObject.OpenFile(OffsetFile).dataList;
                pOffsetBuffer = ArrayListToBuffer(list);
            }
        }

        private void LinkGain()
        {
            if ((IntPtr)pGainBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainBuffer);
                Kernel32Interface.CloseHandle((IntPtr)pGainBuffer);
                pGainBuffer = (uint*)IntPtr.Zero;
            }
            if (File.Exists(GainFile))
            {
                List<ushort[,]> list = HisObject.OpenFile(OffsetFile).dataList;
                pGainBuffer = (uint*)ArrayListToBuffer(list);
            }
        }

        private void LinkGainSequence_Image()
        {
            if ((IntPtr)pGainSeqBuffer_Image != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainSeqBuffer_Image);
                Kernel32Interface.CloseHandle((IntPtr)pGainSeqBuffer_Image);
                pGainSeqBuffer_Image = (ushort*)IntPtr.Zero;
            }
            if ((IntPtr)pGainSeqMedBuffer_Image != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainSeqMedBuffer_Image);
                Kernel32Interface.CloseHandle((IntPtr)pGainSeqMedBuffer_Image);
                pGainSeqMedBuffer_Image = (ushort*)IntPtr.Zero;
            }
            gainSeqFrames_Image = 0;

            if (File.Exists(GainSeqFile_Image))
            {
                List<ushort[,]> list = HisObject.OpenFile(GainSeqFile_Image).dataList;
                gainSeqFrames_Image = list.Count;
                int count = list[0].Length;
                pGainSeqBuffer_Image = ArrayListToBuffer(list);
                pGainSeqMedBuffer_Image = (ushort*)Marshal.AllocHGlobal((int)(gainSeqFrames_Image * sizeof(short)));
                PKL_Interface.Acquisition_CreateGainMap(pGainSeqBuffer_Image, pGainSeqMedBuffer_Image, count, gainSeqFrames_Image);
            }
        }

        private void LinkGainSequence_Dose()
        {
            if ((IntPtr)pGainSeqBuffer_Dose != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainSeqBuffer_Dose);
                Kernel32Interface.CloseHandle((IntPtr)pGainSeqBuffer_Dose);
                pGainSeqBuffer_Dose = (ushort*)IntPtr.Zero;
            }
            if ((IntPtr)pGainSeqMedBuffer_Dose != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainSeqMedBuffer_Dose);
                Kernel32Interface.CloseHandle((IntPtr)pGainSeqMedBuffer_Dose);
                pGainSeqMedBuffer_Dose = (ushort*)IntPtr.Zero;
            }
            gainSeqFrames_Dose = 0;

            if (File.Exists(GainSeqFile_Dose))
            {
                List<ushort[,]> list = HisObject.OpenFile(GainSeqFile_Dose).dataList;
                gainSeqFrames_Dose = list.Count;
                int count = list[0].Length;
                pGainSeqBuffer_Dose = ArrayListToBuffer(list);
                pGainSeqMedBuffer_Dose = (ushort*)Marshal.AllocHGlobal((int)(gainSeqFrames_Dose * sizeof(short)));
                PKL_Interface.Acquisition_CreateGainMap(pGainSeqBuffer_Dose, pGainSeqMedBuffer_Dose, count, gainSeqFrames_Dose);
            }
        }

        private void LinkPixelCorrection()
        {
            if ((IntPtr)pdwPxlCorrList != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pdwPxlCorrList);
                Kernel32Interface.CloseHandle((IntPtr)pdwPxlCorrList);
                pdwPxlCorrList = (int*)IntPtr.Zero;
            }
            if (File.Exists(PixelMapFile))
            {
                List<ushort[,]> list = HisObject.OpenFile(PixelMapFile).dataList;
                int Frames = list.Count;
                int Rows = list[0].GetLength(0);
                int Cols = list[0].GetLength(1);
                int pCorrListSize = 0;
                ushort* pPixelSoure = ArrayListToBuffer(list);
                PKL_Interface.Acquisition_CreatePixelMap(pPixelSoure, Rows, Cols, null, ref pCorrListSize);
                pdwPxlCorrList = (int*)Marshal.AllocHGlobal((int)(pCorrListSize * sizeof(int)));
                PKL_Interface.Acquisition_CreatePixelMap(pPixelSoure, Rows, Cols, pdwPxlCorrList, ref pCorrListSize);

                Marshal.FreeHGlobal((IntPtr)pPixelSoure);
                Kernel32Interface.CloseHandle((IntPtr)pPixelSoure);
                pPixelSoure = (ushort*)IntPtr.Zero;
            }
        }

        private void FreeMemory()
        {
            if ((IntPtr)pOffsetBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pOffsetBuffer);
                Kernel32Interface.CloseHandle((IntPtr)pOffsetBuffer);
                pOffsetBuffer = (ushort*)IntPtr.Zero;
            }
            if ((IntPtr)pGainBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainBuffer);
                Kernel32Interface.CloseHandle((IntPtr)pGainBuffer);
                pGainBuffer = (uint*)IntPtr.Zero;
            }
            if ((IntPtr)pGainSeqBuffer_Image != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainSeqBuffer_Image);
                Kernel32Interface.CloseHandle((IntPtr)pGainSeqBuffer_Image);
                pGainSeqBuffer_Image = (ushort*)IntPtr.Zero;
            }
            if ((IntPtr)pGainSeqMedBuffer_Image != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainSeqMedBuffer_Image);
                Kernel32Interface.CloseHandle((IntPtr)pGainSeqMedBuffer_Image);
                pGainSeqMedBuffer_Image = (ushort*)IntPtr.Zero;
            }
            if ((IntPtr)pGainSeqBuffer_Dose != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainSeqBuffer_Dose);
                Kernel32Interface.CloseHandle((IntPtr)pGainSeqBuffer_Dose);
                pGainSeqBuffer_Dose = (ushort*)IntPtr.Zero;
            }
            if ((IntPtr)pGainSeqMedBuffer_Dose != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pGainSeqMedBuffer_Dose);
                Kernel32Interface.CloseHandle((IntPtr)pGainSeqMedBuffer_Dose);
                pGainSeqMedBuffer_Dose = (ushort*)IntPtr.Zero;
            }
            if ((IntPtr)pdwPxlCorrList != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pdwPxlCorrList);
                Kernel32Interface.CloseHandle((IntPtr)pdwPxlCorrList);
                pdwPxlCorrList = (int*)IntPtr.Zero;
            }
            if ((IntPtr)pAcqBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pAcqBuffer);
                Kernel32Interface.CloseHandle((IntPtr)pAcqBuffer);
                pAcqBuffer = (ushort*)IntPtr.Zero;
            }
            if ((IntPtr)pAcqBufferAverage != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pAcqBufferAverage);
                Kernel32Interface.CloseHandle((IntPtr)pAcqBufferAverage);
                pAcqBufferAverage = (ushort*)IntPtr.Zero;
            }
            gainSeqFrames_Image = 0;
            gainSeqFrames_Dose = 0;
        }

        public ushort[,] GetOffsetData()
        {
            if ((IntPtr)pOffsetBuffer != IntPtr.Zero)
            {
                List<ushort[,]> list = CapturePKI.BufferToArrayList(pOffsetBuffer, 1, (int)imageRows, (int)imageColumns);
                if (list.Count > 0)
                {
                    return list[0];
                }
            }
            return null;
        }

        #region Log(log4net)

        private static void WriteLog(string s)
        {

        }

        private static void WriteError(string s)
        {

        }

        private static void WriteWarn(string s)
        {

        }

        #endregion
    }
}
