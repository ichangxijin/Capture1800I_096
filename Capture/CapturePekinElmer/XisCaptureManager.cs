using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using AMRT;

namespace XISRead
{
    //
    // Sort of dwCorrection 
    //
    public enum ImageCorrection
    {
        None,
        Offset,
        Gain,
        GainSequence,
        BadPixel
    }

    //
    // Mode of capture image
    //
    public class CaptureConfig
    {
        public uint dwSyncMode = 4;
        public uint dwTimingMode = 0;
        public uint dwIntegrationTime = 2000;
        public ImageCorrection dwCorrection = ImageCorrection.None;
        public uint captureFrame = 1;
        public ushort dwGain =5;
        public uint dwRows = 1024;
        public uint dwColumns = 1024;
        public int DelayTime = 4000;
        public int FrameDelayTime = 0;
    }

    public unsafe class XisCaptureManager : ProgressBase
    {
        private Object thisLock = new Object();

        private bool capturing = false;
        public bool Capturing
        {
            get { lock (thisLock) { return capturing; } }
            set { lock (thisLock) { capturing = value; } }
        }
        public IntPtr hWnd;

        private IntPtr hAcqDesc;
        //private IntPtr hevEndAcq;
        private PanelManager panelManager = new PanelManager();
        public List<ushort[,]> dataList;
        public CaptureConfig capConfig = new CaptureConfig();
        public int angle;//选择的偏转角度 0-0，1-90，2-180，3-270,4-水平翻转，5-垂直翻转

        private ushort* pAcqBuffer;
        private ushort* pOffsetBuffer;
        private ushort* pBrightocBuffer;
        private uint* pGainBuffer;
        private ushort* pGainSeqBuffer;
        private ushort* pGainSeqMedBuffer;
        private uint* pPixelBuffer;
        private uint* pPixelPtr;
        private int* pdwPxlCorrList;
        private int gainSeqFrames;
        private uint acqFrameSN;
        private DateTime TimeCount;

        private int LinkStatus = 0;        
        private bool IsWorking = false;
        private System.Threading.Timer timer = null; 

        private PKL_Interface.OneCallBack FrameCallbackDelegate;
        private PKL_Interface.TwoCallBack AcqCallbackDelegate;

        public XisCaptureManager()
        {
            FrameCallbackDelegate = new PKL_Interface.OneCallBack(OnEndFrameCallback);
            AcqCallbackDelegate = new PKL_Interface.TwoCallBack(OnEndAcqCallback);

            timer = new System.Threading.Timer(new TimerCallback(TimerProc));
            timer.Change(0, 1000);
        }
        
        private void TimerProc(object state)
        {
            if (IsWorking || LinkStatus == 0)
            {
                return;
            }

            int now = panelManager.IsLink() ? 1 : 2;
            if (now != LinkStatus)
            {
                LinkStatus = now;
                Kernel32Interface.PostMessage(this.hWnd, WIN_MSG.WM_HW_STATUS, LinkStatus, 0);
            }
        }

        #region Properities

        private bool linkState = false;
        public bool LinkState
        {
            get
            {
                return linkState;
            }
            set
            {
                linkState = false;
            }
        }

        #endregion

        #region private Function

        #region Callback Events Of Capture Image

        private void OnEndFrameCallback(IntPtr hAcqDesc)
        {
            uint size = capConfig.dwRows * capConfig.dwColumns;
            uint off = size * acqFrameSN;
            ushort* p = pAcqBuffer + off;

            uint AcqData;
            PKL_Interface.Acquisition_GetAcqData(hAcqDesc, out AcqData);
            if (AcqData != ACQ_CONT)
            {
                return;
            }
            
            switch (capConfig.dwCorrection)
            {
                case ImageCorrection.None://none
                    break;

                case ImageCorrection.Offset://offset
                    PKL_Interface.Acquisition_DoOffsetCorrection(p, p, pOffsetBuffer, (int)(capConfig.dwColumns * capConfig.dwRows));
                    break;

                case ImageCorrection.Gain://offset_gain
                    PKL_Interface.Acquisition_DoOffsetGainCorrection(p, p, pOffsetBuffer, pGainBuffer, (int)(capConfig.dwColumns * capConfig.dwRows));
                    break;

                case ImageCorrection.GainSequence://GainSequence
                    PKL_Interface.Acquisition_DoOffsetGainCorrection_Ex (p, p, pOffsetBuffer, pGainSeqBuffer,pGainSeqMedBuffer,
                            (int)(capConfig.dwColumns * capConfig.dwRows), gainSeqFrames);
                    break;

                case ImageCorrection.BadPixel:
                    PKL_Interface.Acquisition_DoOffsetGainCorrection_Ex(p, p, pOffsetBuffer, pGainSeqBuffer, pGainSeqMedBuffer,
                            (int)(capConfig.dwColumns * capConfig.dwRows), gainSeqFrames);
                    PKL_Interface.Acquisition_DoPixelCorrection(p, pdwPxlCorrList);
                    break;
            }
            acqFrameSN++;
            System.Threading.Thread.Sleep(capConfig.FrameDelayTime);
            TextLog.SaveLogOperate("Send message..." + " ......In Function : OnEndFrameCallback"); //log 
            Kernel32Interface.PostMessage(this.hWnd, WIN_MSG.WM_PROGRESS, 1, 0);
            PKL_Interface.Acquisition_GetReady(hAcqDesc);
        }

        private void OnEndAcqCallback(IntPtr hAcqDesc)
        {
            uint AcqData;
            PKL_Interface.Acquisition_GetAcqData(hAcqDesc, out AcqData);

            TextLog.SaveLogOperate("Is ACQ_CONT:" + (AcqData == ACQ_CONT).ToString() + " ......In Function : OnEndAcqCallback"); //log 
            TextLog.SaveLogOperate("Frames: " + capConfig.captureFrame.ToString() + " ......In Function : OnEndAcqCallback");    //log 

            //set data has been acquired consummately
            if (AcqData == ACQ_CONT)
            {
                int Frames = (int)capConfig.captureFrame;
                int Rows = (int)capConfig.dwRows;
                int Cols = (int)capConfig.dwColumns;
                dataList = TypeConvert.BufferToArrayList(pAcqBuffer, Frames, Rows, Cols);
                //if (pAcqBuffer != null)
                //{
                //    Marshal.FreeHGlobal((IntPtr)pAcqBuffer);
                //    Kernel32Interface.CloseHandle((IntPtr)pAcqBuffer);
                //    pAcqBuffer = null;
                //}

                if (dataList != null && dataList.Count >= 1) addAngle(angle);

                TextLog.SaveLogOperate("Callback dataList count:" + dataList.Count.ToString() + "......In Function : OnEndAcqCallback"); //log 
            }

            TextLog.SaveLogOperate("End Acquire Call Back" + " ......In Function : OnEndAcqCallback"); //log

            Kernel32Interface.PostMessage(this.hWnd, WIN_MSG.WM_PROGRESS, -1, 0);

            //Kernel32Interface.SetEvent(hevEndAcq);
        }


        #endregion

        //private void SetCaptureMode()
        //{
        //    PKL_Interface.Acquisition_SetCallbacksAndMessages(hAcqDesc, new IntPtr(), 0, 0,  FrameCallbackDelegate, AcqCallbackDelegate);
        //    PKL_Interface.Acquisition_SetCameraMode(hAcqDesc, capConfig.dwTimingMode);
        //    PKL_Interface.Acquisition_SetCameraGain(hAcqDesc, capConfig.dwGain);
        //    PKL_Interface.Acquisition_SetFrameSyncMode(hAcqDesc, capConfig.dwSyncMode);
        //    if (capConfig.dwSyncMode == HIS_SYNCMODE_INTERNAL_TIMER)
        //    {
        //        PKL_Interface.Acquisition_SetTimerSync(hAcqDesc, ref capConfig.dwIntegrationTime);
        //    }
        //}

        //public void SetReadyParaBeforeCapture()
        //{
        //    uint Frames = capConfig.captureFrame;
        //    uint Rows = capConfig.dwRows;
        //    uint Cols = capConfig.dwColumns;
        //    PKL_Interface.Acquisition_DefineDestBuffers(hAcqDesc, pAcqBuffer, Frames, Rows, Cols);
        //    SetCaptureMode();
        //}

        //public void AcquireImageData()
        //{
        //    acqFrameSN = 0;
        //    //hevEndAcq = Kernel32Interface.CreateEvent(null, 0, 0, null);
        //    PKL_Interface.Acquisition_SetAcqData(hAcqDesc, ACQ_CONT);
        //    TextLog.SaveLogOperate("call Acquisition_Acquire_Image" + "...... by AcquireImageData");
        //    PKL_Interface.Acquisition_Acquire_Image(hAcqDesc, capConfig.captureFrame, 0, HIS_SEQ_ONE_BUFFER, null, null, null);

        //    //TextLog.SaveLogOperate("WaitForSingleObject" + "...... by AcquireImageData");
        //    //Kernel32Interface.WaitForSingleObject(hevEndAcq, INFINITE);
        //    //TextLog.SaveLogOperate("End WaitForSingleObject" + "...... by AcquireImageData");
        //}

        //public void AllotAcquireMemory()
        //{
        //    if ((IntPtr)pAcqBuffer != IntPtr.Zero)
        //    {
        //        Marshal.FreeHGlobal((IntPtr)pAcqBuffer);
        //        Kernel32Interface.CloseHandle((IntPtr)pAcqBuffer);
        //        pAcqBuffer = (ushort*)IntPtr.Zero;
        //    }
        //    uint Frames = capConfig.captureFrame;
        //    uint Rows = capConfig.dwRows;
        //    uint Cols = capConfig.dwColumns;
        //    pAcqBuffer = (ushort*)Marshal.AllocHGlobal((int)(Frames * Rows * Cols * sizeof(short)));
        //}

        //public void LinkOffsetImage(string fname)
        //{
        //    if ((IntPtr)pOffsetBuffer != IntPtr.Zero)
        //    {
        //        Marshal.FreeHGlobal((IntPtr)pOffsetBuffer);
        //        Kernel32Interface.CloseHandle((IntPtr)pOffsetBuffer);
        //        pOffsetBuffer = (ushort*)IntPtr.Zero;
        //    }
        //    List<ushort[,]> list = ReadHis.GetHisData(fname);
        //    pOffsetBuffer = TypeConvert.ArrayListToBuffer(list);
        //}

        //public void LinkGainSequence(string fname)
        //{

        //    if ((IntPtr)pGainSeqBuffer != IntPtr.Zero)
        //    {
        //        Marshal.FreeHGlobal((IntPtr)pGainSeqBuffer);
        //        Kernel32Interface.CloseHandle((IntPtr)pGainSeqBuffer);
        //        pGainSeqBuffer = (ushort*)IntPtr.Zero;
        //    }
        //    List<ushort[,]> list = ReadHis.GetHisData(fname);
        //    gainSeqFrames = list.Count;
        //    int count = list[0].Length;
        //    pGainSeqBuffer = TypeConvert.ArrayListToBuffer(list);
        //    pGainSeqMedBuffer = (ushort*)Marshal.AllocHGlobal((int)(gainSeqFrames * sizeof(short)));
        //    PKL_Interface.Acquisition_CreateGainMap(pGainSeqBuffer, pGainSeqMedBuffer, count, gainSeqFrames);
        //}

        //public void LinkPixelCorrection(string fname)
        //{
        //    if ((IntPtr)pdwPxlCorrList != IntPtr.Zero)
        //    {
        //        Marshal.FreeHGlobal((IntPtr)pdwPxlCorrList);
        //        Kernel32Interface.CloseHandle((IntPtr)pdwPxlCorrList);
        //        pdwPxlCorrList = (int*)IntPtr.Zero;
        //    }
        //    List<ushort[,]> list = ReadHis.GetHisData(fname);
        //    int Frames = list.Count;
        //    int Rows = list[0].GetLength(0);
        //    int Cols = list[0].GetLength(1);
        //    int pCorrListSize = 0;
        //    ushort* pPixelSoure = TypeConvert.ArrayListToBuffer(list);
        //    PKL_Interface.Acquisition_CreatePixelMap(pPixelSoure, Rows, Cols, null, ref pCorrListSize);
        //    pdwPxlCorrList = (int*)Marshal.AllocHGlobal((int)(pCorrListSize * sizeof(int)));
        //    PKL_Interface.Acquisition_CreatePixelMap(pPixelSoure, Rows, Cols, pdwPxlCorrList, ref pCorrListSize);

        //    Marshal.FreeHGlobal((IntPtr)pPixelSoure);
        //    Kernel32Interface.CloseHandle((IntPtr)pPixelSoure);
        //    pPixelSoure = (ushort*)IntPtr.Zero;
        //}

        public void LoadImageData(string file)
        {
            dataList = ReadHis.GetHisData(file);
        }

        #endregion

        #region Public Function

        public bool InitLink()
        {
            linkState = panelManager.Init();
            return linkState;
        }

        //public void addAngle(int SelectedIndex)
        //{
        //    if (SelectedIndex == -1||SelectedIndex == 0) return;
        //    if (dataList == null) return;
        //    int r = dataList[0].GetLength(0);
        //    int c = dataList[0].GetLength(1);
        //    List<int[]> COUNTANGLE = new List<int[]> { 
        //        new int[5] { 1, -1,  1, c - 1,     0 }, 
        //        new int[5] { 0, -1, -1, r - 1, c - 1 }, 
        //        new int[5] { 1,  1, -1,     0, r - 1 }, 
        //        new int[5] { 0,  1, -1,     0, c - 1 }, 
        //        new int[5] { 0, -1,  1, r - 1,     0 }};//[exchange,scaleX,scaleY,offX,offY]
        //    int[] ca = COUNTANGLE[SelectedIndex - 1];
        //    ushort[,] temp = new ushort[r, c];
        //    if (ca[0] == 1)

        //    {
        //        for (int n = 0; n < dataList.Count; n++)
        //        {
        //            for (int i = 0; i < r; i++)
        //            {
        //                for (int j = 0; j < c; j++)
        //                {
        //                    temp[i, j] = dataList[n][j * ca[1] + ca[3], i * ca[2] + ca[4]];                            
        //                }
        //            }
        //            dataList[n] = temp;
        //        }
        //    } 
        //    else
        //    {
        //        for (int n = 0; n < dataList.Count; n++)
        //        {
        //            for (int i = 0; i < r; i++)
        //            {
        //                for (int j = 0; j < c; j++)
        //                {
        //                    temp[i, j] = dataList[n][i * ca[1] + ca[3], j * ca[2] + ca[4]];                        
        //                }
        //            }
        //            dataList[n] = temp;
        //        }
        //    }
        //}
        #endregion

    }
}