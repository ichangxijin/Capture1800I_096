using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;


namespace ImageCapturing
{
    partial class PKL_Interface
    {
        static bool CHECK_XISL_DLL = (CapturePub.readCaptrueValue(XmlField.NeedCheckPanelInterface, false) == "F");
        static PKL_Interface()
        {
            if (!CHECK_XISL_DLL)
            {
                if (CapturePub.readCaptrueValue(XmlField.PanelInterface, false) == "0")
                {
                    string XislFilePath = CapturePub.LinkFilePath + "\\xisl\\XISL(3.2.0.14).dll";
                    string newXislFilePath = System.Windows.Forms.Application.StartupPath.ToString() + "\\xisl.dll";
                    if (System.IO.File.Exists(XislFilePath))
                    {
                        File.Copy(XislFilePath, newXislFilePath, true);
                        CapturePub.saveCaptrueValue(XmlField.NeedCheckPanelInterface, "F");
                        CapturePub.saveCaptrueValue(XmlField.PanelInterface, "0");
                    }
                }
                else
                {
                    string XislFilePath = CapturePub.LinkFilePath + "\\xisl\\XISL(3.3.3.32).dll";
                    string newXislFilePath = System.Windows.Forms.Application.StartupPath.ToString() + "\\xisl.dll";
                    if (File.Exists(XislFilePath))
                    {
                        File.Copy(XislFilePath, newXislFilePath, true);
                        CapturePub.saveCaptrueValue(XmlField.NeedCheckPanelInterface, "F");
                        CapturePub.saveCaptrueValue(XmlField.PanelInterface, "1");
                    }
                }
                CHECK_XISL_DLL = true;
            }
        }


        private const string ImportXISL = "XISL.dll";

        [StructLayout(LayoutKind.Sequential)]
        public struct WinHeaderType
        {
            public ushort FileType;			// File ID (0x7000)
            public ushort HeaderSize;		// Size of this file header in Bytes
            public ushort HeaderVersion;	// yy.y
            public uint FileSize;			// Size of the whole file in Bytes
            public ushort ImageHeaderSize;	// Size of the image header in Bytes
            public ushort ULX, ULY, BRX, BRY;// bounding rectangle of the image
            public ushort NrOfFrames;		// self explanatory
            public ushort Correction;		// 0 = none, 1 = offset, 2 = gain, 4 = bad pixel, (ored)
            public double IntegrationTime;	// frame time in microseconds
            public ushort TypeOfNumbers;		// short, long integer, float, signed/unsigned, inverted, 
            // fault map, offset/gain dwCorrection data, badpixel dwCorrection data
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 34)]
            public byte[] x;		// fill up to 68 byte
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CHwHeaderInfo     //定义结构体
        {
            public uint dwPROMID;
            public uint dwHeaderID;
            public int bAddRow;
            public int bPwrSave;
            public uint dwNrRows;
            public uint dwNrColumns;
            public uint dwZoomULRow;
            public uint dwZoomULColumn;
            public uint dwZoomBRRow;
            public uint dwZoomBRColumn;
            public uint dwFrmNrRows;
            public uint dwFrmRowType;
            public uint dwFrmFillRowIntervalls;
            public uint dwNrOfFillingRows;
            public uint dwDataType;
            public uint dwDataSorting;
            public uint dwTiming;
            public uint dwAcqMode;
            public uint dwGain;
            public uint dwOffset;
            public uint dwAccess;
            public int bSyncMode;
            public uint dwBias;
            public uint dwLeakRows;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CHwHeaderInfoEx  //定义结构体
        {
            public ushort wHeaderID;
            public ushort wPROMID;
            public ushort wResolutionX;
            public ushort wResolutionY;
            public ushort wNrRows;
            public ushort wNrColumns;
            public ushort wZoomULRow;
            public ushort wZoomULColumn;
            public ushort wZoomBRRow;
            public ushort wZoomBRColumn;
            public ushort wFrmNrRows;
            public ushort wFrmRowType;
            public ushort wRowTime;
            public ushort wClock;
            public ushort wDataSorting;
            public ushort wTiming;
            public ushort wGain;
            public ushort wLeakRows;
            public ushort wAccess;
            public ushort wBias;
            public ushort wUgComp;
            public ushort wCameratype;
            public ushort wFrameCnt;
            public ushort wBinningMode;
            public ushort wRealInttime_milliSec;
            public ushort wRealInttime_microSec;
            public ushort wStatus;
        }

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_Init(IntPtr phAcqDesc,  
                              int dwBoardType,
                              int nChannelNr,
                              int bEnableIRQ,
                              uint Rows, 
                              uint Columns,
                              uint dwSortFlags,
                              int bSelfInit,
                              int bInitAlways);

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_EnumSensors(out uint pdwNumSensors, int bEnableIRQ, int bInitAlways);

        [DllImport(ImportXISL)]
        //You can use this function to iterate through all recognized sensors in the system.
        public static extern uint Acquisition_GetNextSensor(
                                  //Pointer to an unsigned 4 byte integer that receives information’s that are needed for subsequent calls of this function.
                                  //To receive the acquisition descriptor (HACQDESC) for the first recognized sensor set Pos to NULL.
                                  ref uint Pos, 
                                  //Handle of a structure that contains all needed parameters for acquisition (HACQDESC).
                                  //If you call Acquisition_Init the first time set hAcqDesc to NULL, in subsequent calls use the former returned value.
                                  out IntPtr phAcqDesc);

        public delegate void EndAcquisitionCallBack1(IntPtr phAcqDesc);
        public delegate void EndAcquisitionCallBack2(IntPtr phAcqDesc);

        [DllImport(ImportXISL)]//defines callback functions to react on acquisition status changes.
        
        public static extern uint Acquisition_SetCallbacksAndMessages(IntPtr hAcqDesc,
                                  IntPtr hWnd, 
                                  //Defines a user message that is posted to hWnd 
                                  //if an error occurs during acquisition.
                                  uint dwErrorMsg, 
                                  //Defines a user message that is posted to hWnd 
                                  //if Acquisition_SetReady wasn't called by the application at the end of sorting.
                                  uint dwLoosingFramesMsg,
                                  EndAcquisitionCallBack1 EndFrameCallback, //抓取一帧图像以后的回调函数委托
                                  EndAcquisitionCallBack2 EndAcqCallback);  //抓完图以后的回调函数委托


        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_GetCommChannel(IntPtr hAcqDesc,
            out  uint pdwChannelType,
            out  int pnChannelNr);


        [DllImport(ImportXISL)] 

        public static extern unsafe uint Acquisition_GetHwHeaderInfoEx(IntPtr hAcqDesc,
          ref  CHwHeaderInfo pInfo,
          ref  CHwHeaderInfoEx pInfoEx);

        //This function sets the acquisition mode of the detector.
        //Currently eight fixed frame times of the detector are provided.
        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_SetCameraMode(IntPtr hAcqDesc,
                                  //Must be a number between 0 and 7. The corresponding frame time depends on the used PROM
                                  uint dwMode);

        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_Acquire_OffsetImage(IntPtr hAcqDesc, 
                                        ushort* pOffsetData,
                                        uint nRows, 
                                        uint nCols,
                                        uint nFrames);

        [DllImport(ImportXISL)] 
   
        public static extern uint Acquisition_SetCameraGain(IntPtr hAcqDesc,
            //Gain factor to set.
            //For the AM-Type the values of all capacities are added. All bitwise combinations are valid. For example : 3 => 1.3pF.
            ushort wMode);

        [DllImport(ImportXISL)] 
 
        public static extern uint Acquisition_SetCameraBinningMode(IntPtr hAcqDesc,
            //Binning Mode to be set (Bitwise)
            //Cameratype 1:
            //0 active : no binning (default )
            //1 active: 2x2 binning

            //Cameratype 2:
            //as in Cameratype 1 with additional
            //8 active for AVG Mode
            //9 active for Sum Mode
           ushort wMode);

        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_Acquire_Image(IntPtr hAcqDesc,
                    uint dwFrames,
                    uint dwSkipFrames,
                    uint dwOpt,
                    ushort* pwOffsetData,
                    uint* pdwGainData,
                    int* pdwPixelData);

        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_Acquire_Image_Ex(IntPtr hAcqDesc, 
                    uint dwFrames, 
                    uint dwSkipFrms, 
                    uint dwOpt,
                    ushort* pwOffsetData,
                    uint dwGainFrames,
                    ushort* pwGainSeqData,
                    ushort* pwGainAvgData, 
                    uint* pdwGainData, 
                    int* pdwPxlCorrList);

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_Abort(IntPtr hAcqDesc);

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_AbortCurrentFrame(IntPtr hAcqDesc);

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_GetLatestFrameHeader(IntPtr hAcqDesc,
            ref CHwHeaderInfo pInfo,
            ref CHwHeaderInfoEx pInfoEx);

        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_DefineDestBuffers(
               IntPtr hAcqDesc,
               ushort* pProcessedData,
               uint nFrames,
               uint nRows,
               uint nColumns);

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_Acquire_Image_PreloadCorr(IntPtr hAcqDesc,
                uint nFrames,
                uint dwSkipFrms,
                uint dwOpt);

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_SetCorrData(
               IntPtr hAcqDesc,
               ref  int pOffsetData,
               ref  int pGainData,
               ref  int pPixelCorrList);

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_Acquire_OffsetImage_PreloadCorr(
                    IntPtr hAcqDesc,
                    ref   ushort pwOffsetData,
                    uint nRows,
                    uint nColumns,
                    uint nFrames,
                    uint dwOpt);

        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_Acquire_GainImage(
                        IntPtr hAcqDesc,
                        ushort* pOffsetData,
                        uint* pGainData,
                        uint nRows,
                        uint nCols,
                        uint nFrames);

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_Acquire_GainImage_PreloadCorr(
                               IntPtr hAcqDesc,
                               ref int pGainData,
                               uint nRows,
                               uint nCols,
                               uint nFrames);

        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_CreateGainMap(ushort* pGainData, ushort* pGainAVG, int nCount, int nFrame);


        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_CreatePixelMap(
                            ushort* pData,
                            int nDataRows,
                            int nDataColumns,
                            int* pCorrList,
                            ref int pnCorrListSize);

        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_DoOffsetCorrection(
                           ushort* pSource,
                           ushort* pDest,
                           ushort* pOffsetData,
                           int nCount);

        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_DoGainCorrection(IntPtr hAcqDesc, ref int pGainData);

        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_DoOffsetGainCorrection( //pdf好象有问题!!与上个函数同名!!
                      ushort* pSource,
                      ushort* pDest,
                      ushort* pOffsetData,
                      uint* pGainData,
                      int nCount);

        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_DoOffsetGainCorrection_Ex(
                      ushort* pSource,
                      ushort* pDest,
                      ushort* pOffsetData,
                      ushort* pGainData,
                      ushort* pGainAVG,
                      int nCount,
                      int nFrame);
        [DllImport(ImportXISL)] 

        public unsafe static extern uint Acquisition_DoPixelCorrection(ushort* pData, int* pCorrList);


        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_IsAcquiringData(IntPtr hAcqDesc);


        [DllImport(ImportXISL)] 

        public static extern uint Acquisition_GetErrorCode(
                                    IntPtr hAcqDesc,
                                    ref int dwHISError,
                                    ref int dwBoardError );

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_Close(IntPtr hAcqDesc);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_CloseAll();

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_SetReady(IntPtr hAcqDesc, int bFlag);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_GetReady(IntPtr hAcqDesc);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_GetConfiguration(
                 IntPtr hAcqDesc,
                 out uint dwFrames,
                 out uint dwRows,
                 out uint dwColumns,
                 out uint dwDataType,
                 out uint dwSortFlags,
                 out int bIRQEnabled,
                 out uint dwAcqType,
                 out uint dwSystemID,
                 out uint dwSyncMode,
                 out uint dwHwAccess);

        [DllImport(ImportXISL)] 
        unsafe public static extern uint Acquisition_GetIntTimes(
                      IntPtr hAcqDesc,
                      double* pdblIntTime,
                      ref int nIntTimes);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_SetAcqData(IntPtr hAcqDesc, uint dwData);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_GetAcqData(IntPtr hAcqDesc, out uint dwData);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_GetActFrame(
                         IntPtr hAcqDesc,
                         out  uint dwActAcqFrame,
                         out uint dwActBuffFrame);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_SetFrameSyncMode(IntPtr hAcqDesc, uint dwMode);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_SetFrameSync(IntPtr hAcqDesc);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_SetTimerSync(IntPtr hAcqDesc, ref uint dwCycleTime);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_GetHwHeaderInfo(IntPtr hAcqDesc, ref CHwHeaderInfo pInfo);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_ResetFrameCnt(IntPtr hAcqDesc);

        [DllImport(ImportXISL)] 
        public static extern uint Acquisition_GetCameraBinningMode(IntPtr hAcqDesc, out ushort wMode);
    }
}