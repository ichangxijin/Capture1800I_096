using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ImageCapturing
{
    public partial class CareRayInterface
    {

        public const int STRLEN = 256;

        [StructLayout(LayoutKind.Sequential)]
        public struct FpgaReg
        {
            public int res1;
            public int res2;
            public int res3;
            public int res4;
            public int res5;
            public int res6;
            public int res7;
            public int res8;
            public int res9;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] res10;
            public float res11;
            public int res12;
            public float res13;
            public int res14;
            public float res15;
            public float res16;
            public float res17;
            public float res18;
            public float res19;
            public int res20;
            public float res21;
            public int res2022;
            public float res23;
            public int res24;
            public int res25;
            public int res26;
            public int readout_cntr;
            public int res27;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] res28;
            public int res29;
            public int res30;
            public int res31;
            public int res32;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] res33;
            public int res34;
            public int res35;
            public int res36;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct ConfigInfo
        {
            public int detectorType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] detectorIp;
            public int port;
            public int xrayDelay;
            public bool syncEnable;
            public int ofstTimeNum;
            public int ofstNumEachTime;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] ofstCalExpTime;
            public int ofstCorrImgNum;
            public int ofstInterval;
            public int gainNum;
            public int defectNum;
            public int doseNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] dose;
            public int updateDefectMap;
            public int maxValue;
            public int baseImage;
            public bool localNSigmaEnable;
            public float localNSigma;
            public bool globalHisNPercentEnable;
            public float globalHisNPercent;
            public bool globalMeanThreholdEnable;
            public float globalMeanUpParam;
            public float globalMeanDownParam;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DetectorInfo
        {
            public int rawImageWidth;
            public int rawImageHeight;
            public int maxPixelValue;
            public int bitsPerPixel;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] hardWareVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] softWareVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] serialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] detectorDescription;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Temperature
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public float[] reserved;
            public float maxTemperature;
            public float aveTemperature;
            public int overhot_flag;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BatteryInfo
        {
            public int manu_access;
            public int alarm_capa;
            public int alarm_time;
            public int mode;
            public int atrate;
            public int atrate_tofull;
            public int atrate_toempty;
            public int atrate_ok;
            public float temperature;
            public float voltage;
            public float current;
            public float ave_current;
            public float max_error;
            public float relative_state_of_charge;
            public float absolute_state_of_charge;
            public int rest_capacity;
            public int full_capacity;
            public int run_time_to_empty;
            public int ave_time_to_empty;
            public float charging_current;
            public float charging_voltage;
            public int battery_status;
            public int cycle_count;
            public int design_capacity;
            public float design_voltage;
            public float cell4_voltage;
            public float cell3_voltage;
            public float cell2_voltage;
            public float cell1_voltage;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WirelessInfo
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] essid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] freq;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] channel;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] bit_rate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public char[] encypt_key;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] security_mode;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] link_quality;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] signal_level;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] noise_level;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] sensitivity;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public char[] reserved1;
            private ulong tx_packets;
            private ulong rx_packets;
            private ulong tx_bytes;
            private ulong rx_bytes;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public int[] reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StatusInfo
        {
            public int checkMode;
            public int detectorState;
            public float frameRate;
            public CareRayInterface.Temperature temperature;
            public CareRayInterface.BatteryInfo batInfo;
            public CareRayInterface.WirelessInfo wireless_info;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ModeInfo
        {
            public int modeId;
            public int acqType;
            public int imageWidth;
            public int imageHeight;
            public int linesPerPixel;
            public int colsPerPixel;
            public int imageSize;
            public float maxFrameRate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] modeDescription;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ExtendModeInfo
        {
            public int modeId;
            public int acqType;
            public int imageWidth;
            public int imageHeight;
            public int linesPerPixel;
            public int colsPerPixel;
            public int imageSize;
            public float maxFrameRate;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] modeDescription;
            public int bitsStored;
            public int bitsAllocated;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Correction
        {
            public bool offset;
            public bool gain;
            public bool defect;
            public bool defaultPortableGain;
            public bool fixedPortableGain;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct UserCorrection
        {
            public bool fixedCorr;
            public bool non_fixedCorr;
            public bool portableCorr;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ExpProgress
        {
            public int expStatus;
            public bool inside_offs_corrflag;
            public bool realtime_offset;
            public int frame_number;
            public bool fetchable;
            public int errorCode;
            public int calComplete;
            public bool expose_flag;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FrameAttr
        {
            public int image_width;
            public int image_height;
            public int pixel_bits;
            public int image_datatype;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CalParams
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] gain_image_dir;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] portable_cal_kv;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] normal_cal_kv;
            public int ofst_cal_num;
            public int linear_dose_num;
            public int linear_num_per_dose;
            public int time_interval;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RealtimeOfstSetting
        {
            public int realtime_ofst_opt;
            public bool realtime_ofst_inner;
        }

        public enum DetectorType
        {
            CareView_1500R,
            CareView_1500Rm,
            CareView_1500P,
            CareView_1500C,
            CareView_1800R,
            CareView_500M,
            CareView_500I,
            CareView_500P,
            CareView_900F,
            CareView_1800I,
            Unkown_Type,
        }

        public enum CheckMode
        {
            MODE_UNDEFINED = 0,
            MODE_RAD = 0x10,
            MODE_FLUORO = 0x11,
            MODE_TEST = 0x14,
            MODE_BIN22 = 0x15,
            MODE_NDT = 0x16,
            MODE_PREV = 0x17,
            MODE_BIN11 = 0x18,
            MODE_FLUORO_START = 100,
            MODE_FLUORO_END = 1000,
            MODE_MAXID,
        }

        private enum DetectorState
        {
            CR_BOOT,
            CR_STANDBY,
            CR_ACQUISTION,
            CR_SLEEP,
            CR_ERROR,
        }

        public enum ExposureStatus
        {
            CR_EXP_ERROR = -1,
            CR_EXP_INIT = 0,
            CR_EXP_READY = 1,
            CR_EXP_WAIT_PERMISSION = 2,
            CR_EXP_PERMITTED = 3,
            CR_EXP_EXPOSE = 4,
            CR_EXP_COMPLETE = 5,
        }

        public enum ProgType
        {
            CR_RAD_PROG,
            CR_CAL_PROG,
        }

        public enum SyncMode
        {
            UNDEF_SYNC,
            EXT_SYNC,
            SOFT_SYNC,
            AUTO_SYNC,
            MANUAL_SYNC,
            SCAN_SYNC,
            AED_SYNC,
            MUTIL_EXT_SYN,
            EXT_TRIGGER = 8, //Exteral Trigger ,used in fluoro mode
            INI_TRIGGER //Internal Trigger,used in fluoro mode
        }

        public enum PowerMode
        {
            PWR_STANDBY = 0,
            PWR_FULL_RUNNING = 1,
            PWR_SMART_RUNNING = 2,
            PWR_DOWN_FE = 4,
            PWR_SLEEPING = 5,
            PWR_DEEP_SLEEPING = 6,
            PWR_SUSPEND = 7,
        }

        public enum ImageDataType
        {
            UINT_16,
            UINT_32,
            INT_16,
            INT_32,
            FLOAT_32,
            DOUBLE_64,
        }

        public enum TempStatus
        {
            IN_NORMAL_TEMP,
            IN_WARN_TEMP,
            OVER_MAX_TEMP_LIMIT,
            OVER_MIN_TEMP_LIMIT,
            INVAILD_TEMP,
        }

        public enum DetectorIndex
        {
            DETECTOR_I,
            DETECTOR_II,
        }

        public enum RealtimeDarkOpt
        {
            NO_REALTIME_DARK,
            PRE_REALTIME_DARK,
            AFT_REALTIME_DARK,
        }


        public enum event_id
        {
            EVT_DISCONNECT,
            EVT_READY,
            EVT_EXP_EN,
            EVT_IMAGE_ARRIVE,
            EVT_AEC_PREV_MODE_READY,
            EVT_AEC_RAD_MODE_READY,
            EVT_AEC_PREV_IMG_ARRIVE,
            EVT_AEC_RAD_IMG_ARRIVE,
            EVT_DETECTOR_ERROR,
            EVT_EXPOSE_FLAG_FALSE,
            EVT_EXPOSE_FLAG_TRUE,
            EVT_INITIAL,
            EVT_UNUPLOADED_IMG_EXIST,
            EVT_CONNECTED,
            EVT_SECOND_PANEL_CONNECTED,
            EVT_SECOND_PANEL_DISCONNECTED,
            EVT_SHS_STATUS_CHANGED,
            EVT_VIDEO_FRAME_INDEX_CHANGED,
            EVT_VIDEO_FRAME_RECV_STAT
        };


        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct EventData
        {
            public int width;
            public int height;
            public int bits;
            public void* data;
        };

        public delegate void EventCallbackDelegate(int eventID, ref EventData eventData);

    }
}
