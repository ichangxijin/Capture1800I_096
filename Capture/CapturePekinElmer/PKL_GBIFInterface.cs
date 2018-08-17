using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace ImageCapturing
{
    partial class PKL_Interface
    {
        public const int GBIF_IP_MAC_NAME_CHAR_ARRAY_LENGTH = 16;
        public const int HIS_GbIF_IP_STATIC = 1;
        public const int HIS_GbIF_IP_DHCP = 2;
        public const int HIS_GbIF_IP_LLA = 4;
        public const int HIS_GbIF_IP = 1;
        public const int HIS_GbIF_MAC = 2;
        public const int HIS_GbIF_NAME = 3;


        [StructLayout(LayoutKind.Sequential)]
        public struct GBIF_DEVICE_PARAM
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GBIF_IP_MAC_NAME_CHAR_ARRAY_LENGTH)]
            public byte[] ucMacAddress;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GBIF_IP_MAC_NAME_CHAR_ARRAY_LENGTH)]
            public byte[] ucIP;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GBIF_IP_MAC_NAME_CHAR_ARRAY_LENGTH)]
            public byte[] ucSubnetMask;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GBIF_IP_MAC_NAME_CHAR_ARRAY_LENGTH)]
            public byte[] ucGateway;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GBIF_IP_MAC_NAME_CHAR_ARRAY_LENGTH)]
            public byte[] ucAdapterIP;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GBIF_IP_MAC_NAME_CHAR_ARRAY_LENGTH)]
            public byte[] ucAdapterMask;
            public int dwIPCurrentBootOptions;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] cManufacturerName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] cModelName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] cGBIFFirmwareVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] cDeviceName;

            public void WriteToFile(string tag)
            {
                LogFPD.WriteLogStartTag("");
                LogFPD.WriteLog(tag);
                LogFPD.WriteLog("ucMacAddress -- " + CCharToString(ucMacAddress));
                LogFPD.WriteLog("ucIP -- " + CCharToString(ucIP));
                LogFPD.WriteLog("ucSubnetMask -- " + CCharToString(ucSubnetMask));
                LogFPD.WriteLog("ucGateway -- " + CCharToString(ucGateway));
                LogFPD.WriteLog("ucAdapterIP -- " + CCharToString(ucAdapterIP));
                LogFPD.WriteLog("ucAdapterMask -- " + CCharToString(ucAdapterMask));
                LogFPD.WriteLog("cManufacturerName -- " + CCharToString(cManufacturerName));
                LogFPD.WriteLog("cModelName -- " + CCharToString(cModelName));
                LogFPD.WriteLog("cGBIFFirmwareVersion -- " + CCharToString(cGBIFFirmwareVersion)); ;
                LogFPD.WriteLog("cDeviceName -- " + CCharToString(cDeviceName));
            }
        }

        public static string CCharToString(byte[] arrary)
        {
            string str = "";
            if(arrary.Length > 0)
            {
                str = ASCIIEncoding.ASCII.GetString(arrary);
            }
            return str;
        }

        public static byte[] StringToCChar(string str)
        {
            return ASCIIEncoding.ASCII.GetBytes(str);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GBIF_Detector_Properties     //定义结构体
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] cDetectorType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] cManufacturingDate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] cPlaceOfManufacture;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
            public byte[] cDummy;

            public void WriteToFile(string tag)
            {
                LogFPD.WriteLogStartTag("");
                LogFPD.WriteLog(tag);
                LogFPD.WriteLog("cDetectorType -- " + CCharToString(cDetectorType));
                LogFPD.WriteLog("cManufacturingDate -- " + CCharToString(cManufacturingDate));
                LogFPD.WriteLog("cPlaceOfManufacture -- " + CCharToString(cPlaceOfManufacture));
            }
        }

       // [DllImport("XISL.dll")]
        [DllImport(ImportXISL)]
        public unsafe static extern uint Acquisition_GbIF_Init(ref IntPtr phAcqDesc,
                                                        int nChannelNr,
                                                        int bEnableIRQ,
                                                        uint uiRows,
                                                        uint uiColumns,
                                                        int bSelfInit,
                                                        int bAlwaysOpen,
                                                        int lInitType,
                                                        byte* cAddress
);
        [DllImport(ImportXISL)]

        //[DllImport("libxml2.dll")]
        //[DllImport("CANCamGigE.dll")]
        public unsafe static extern uint Acquisition_GbIF_GetDeviceCnt(int* plNrOfboards);


        [DllImport(ImportXISL)]

        public unsafe static extern uint Acquisition_GbIF_GetDeviceList(ref GBIF_DEVICE_PARAM pGBIF_DEVICE_PARAM,
                                                                        int nDeviceCnt
);
        [DllImport(ImportXISL)]

        public unsafe static extern uint Acquisition_GbIF_GetDevice(byte* ucAddress,
                                                                    int dwAddressType,
                                                                    ref GBIF_DEVICE_PARAM pDevice
);

        [DllImport(ImportXISL)]

        public static extern uint Acquisition_GbIF_GetDeviceParams(IntPtr hAcqDesc,
                                                                   ref GBIF_DEVICE_PARAM pDevice
);


        [DllImport(ImportXISL)]

        public static extern unsafe uint Acquisition_GbIF_SetConnectionSettings(byte* cMAC,
                                                                                 uint uiBootOptions,
                                                                                 byte* cDefIP,
                                                                                 byte* cDefSubNetMask,
                                                                                 byte* cStdGateway
);

        [DllImport(ImportXISL)]

        public unsafe static extern uint Acquisition_GbIF_GetConnectionSettings(byte[] ucMAC,
                                                                         ref int uiBootOptions,
                                                                         byte[] ucDefIP,
                                                                         byte[] ucDefSubNetMask,
                                                                         byte[] ucStdGateway
);
        [DllImport(ImportXISL)]

        public unsafe static extern uint Acquisition_GbIF_ForceIP(byte* cMAC,
                                                                  byte* cDefIP,
                                                                  byte* cDefSubNetMask,
                                                                  byte* cStdGateway
);

        [DllImport(ImportXISL)]

        public static extern uint Acquisition_GbIF_SetPacketDelay(IntPtr hAcqDesc, int lPacketdelay);

        [DllImport(ImportXISL)]

        public static extern uint Acquisition_GbIF_GetPacketDelay(IntPtr hAcqDesc, ref int lPacketdelay);

        [DllImport(ImportXISL)]

        public static extern uint Acquisition_GbIF_CheckNetworkSpeed(IntPtr hAcqDesc,
                                                                     ref short wTiming,
                                                                     ref int lPacketDelay,
                                                                     int lMaxNetworkLoadPercent
);

        [DllImport(ImportXISL)]

        public unsafe static extern uint Acquisition_GBIF_GetDetectorProperties(IntPtr hAcqDesc,
                                                                                ref GBIF_Detector_Properties pDetectorProperties
);
        [DllImport(ImportXISL)]
        public unsafe static extern uint Acquisition_SetFrameSyncTimeMode(IntPtr hAcqDesc,
                                                                          uint uiMode,
                                                                          uint dwDelayTime
);
        [DllImport(ImportXISL)]
        public unsafe static extern uint Acquisition_SetCameraTriggerMode(
IntPtr hAcqDesc,
short wMode
);
    }
}