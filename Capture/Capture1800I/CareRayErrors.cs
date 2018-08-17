using System;
using System.Collections.Generic;
using System.Text;

namespace ImageCapturing
{


    enum DetectorType
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
        CareView_1500L,
        CareView_1500Cw,
        CareView_300P,
        CareView_750M,
        CareView_1800L,
        CareView_1800RV2,
        CareView_1500PV2,
        CareView_750Cw,
        CareView_0331IF,
        CareView_750I,
        CareView_1800IF,
        Unkown_Type           //未知型号
    };

    /* 
 * Enum for detector mode
 * 
 * CR_RAD_MODE — Rad mode
 * CR_FLUORO_MODE — Fluoro mode
 * CR_LAGFLUORO_MODE — Lag testing mode
 * CR_CUSTFLUORO_MODE — Customized fluoro mode
 */
    enum CheckMode
    {
        MODE_UNDEFINED = 0,
        MODE_RAD = 0x10,
        MODE_FLUORO = 0x11,
        MODE_TEST = 0x14,        //!<used just for self-test
        MODE_BIN22 = 0x15,
        MODE_NDT = 0x16,     //NDT mode, for industry application.
        MODE_PREV = 0x17,      //preview mode, for 500M
        MODE_BIN11 = 0x18,
        MODE_FLUORO_START = 100,
        MODE_FLUORO_END = 1000,
        MODE_MAXID
    };

    //同步模式
    enum SyncMode
    {
        UNDEF_SYNC,		//undefined sync mode
        EXT_SYNC,			//external sync mode (exist sync cable)
        SOFT_SYNC,			//software sync mode
        AUTO_SYNC,			//auto sync mode
        MANUAL_SYNC,		//manual sync mode
        SCAN_SYNC,
        AED_SYNC,
        MUTIL_EXT_SYN, //similar with INT_TRIGGER, would be discard in the future
        EXT_TRIGGER = 8, //External Trigger, used in fluoro mode
        INT_TRIGGER //Internal Trigger, used in fluoro mode
    };




        public enum KZ_ERROR_TYPE
        {
            CR_NO_ERR = 0,
            ERR_DRIVER,
            ERR_OPEN_DETECTOR_FAILED,
            ERR_MMAP_SPACE,
            ERR_MEM_ALLOC,
            ERR_NO_SD_SPACE,		//5
            ERR_NO_FLASH_SPACE,
            ERR_FILE_NAME,
            ERR_FILE_PATH,
            ERR_FILE_READ,
            ERR_FILE_WRITE,		//10
            ERR_XMLFILE,
            ERR_CODER_IDX,		//undefined command ID at codec
            ERR_CODER_VER,		//undefined parser version
            ERR_CODER_PRM,	//command param error
            ERR_IMAGE_PIPE_BROKEN,		//15
            ERR_LOGIC,
            ERR_NO_MODE_SELECTED,
            //ERR_ENCODE_COMM = 1,
            //ERR_DECODE_COMM = 2,
            ERR_SET_DTTVER,
            ERR_UNDEFINED_COMMID,
            ERR_UNDEFINED_MODE,		//20
            ERR_HANDSWITCH_STATE,
            ERR_SET_DTTTIME,
            ERR_ACCESS_FPGA,
            ERR_UPFILE,
            ERR_BAD_PARAM, 	//25
            ERR_BAD_STATE,		//forbid to change mode at current transfer state.
            ERR_BAD_ACTION_IN_CURRENT_MODE,		//
            ERR_LICENSE,
            ERR_SYSTEM_SELFTEST,
            ERR_IOCTL_FAILED,	//30
            ERR_TEST_FRAMES,
            ERR_EXCHANGE_CFGFILE,
            ERR_UNAVAILABLE_OFFSET,		//
            ERR_CREATE_THREAD,
            ERR_READOUT,		//35
            ERR_INVAILD_FEATHER,
            ERR_GET_CORR_IMGCRC, //37
            ERR_SOFTWARE_HAS_EXPIRED,  //38
            ERR_FETCHABLE_NOT_TRUE,
            ERR_INVALID_FE_FRMNUMS,
            ERR_READ_FRAME_ATTR,
            ERR_INVALID_ACQ_OPTION,
            ERR_RDFRM_SQQ_IS_EMPTY,
            ERR_UNAVAILABLE_SYNCMODE,
            ERR_UNAVAILABLE_ACQMODE,

            ERR_REALTIME_DARK_SET,
            ERR_UNDEFINED_CONFIGFILE,
            ERR_NO_CONFIGFILE,
            ERR_FILE_CREATE,
            ERR_FILE_CRC,
            ERR_EXIST_CONFIGFILE,
            ERR_FILE_OPEN,

            //---------Socket Layer--------------
            //Socket layer error most happens in bad network state
            CR_CONN_DETECTOR_ERR = 1000,//API_INDEX,
            CR_DISCONN_DETECTOR_ERR,
            CR_ALREADY_CONN_ERR,
            CR_ALREADY_DISCONN_ERR,
            CR_RECONN_ERROR,
            CR_CLOSE_SOCK_ERROR,
            CR_INIT_ERR,
            CR_CREATE_ERR,
            CR_GETOPT_ERR,
            CR_SETOPT_ERR,
            CR_SEND_ERR,
            CR_RECV_ERR,
            CR_CLOSE_ERR,
            CR_SET_VER_ERR,
            //---------Codec Layer--------------
            CR_COMID_ERR,
            CR_VER_ERR,
            CR_PARAM_ERR,
            //---------V_API Layer--------------
            CR_NULL_BUFFER_ERR,
            CR_READ_CONFIGFILE_ERR,
            CR_WRITE_CONFIGFILE_ERR,
            CR_USER_PARAM_ERR,
            CR_MODE_UNSELECT_ERR,
            CR_LOAD_DLL_ERR,
            CR_FREE_DLL_ERR,
            CR_CREATE_EVENT_ERR,
            CR_CLOSE_EVENT_ERR,
            CR_FUNCID_ERR,
            CR_GET_FUNC_ADDR_ERR,
            CR_CREATE_VTHREAD_ERR,
            CR_CREATE_RTTHREAD_ERR,
            CR_CREATE_HBTHREAD_ERR,
            CR_CREATE_OFSTTHREAD_ERR,
            CR_CREATE_CALTHREAD_ERR,
            CR_CREATE_LOGTHREAD_ERR,
            CR_CLOSE_THREAD_ERR,
            CR_ALCATE_BUFF_ERR,
            CR_VTHREAD_BUSY_ERR,
            CR_GET_IMAGE_ERR,
            CR_FILE_PATH_ERR,
            CR_OPEN_FILE_ERR,
            CR_READ_FILE_ERR,
            CR_WRITE_FILE_ERR,
            CR_EXCHANGEFILE_ERR,
            CR_GRAB_IN_PROCESS_ERR,
            CR_GRAB_NOT_WORK_ERR,
            CR_REC_IN_PROCESS_ERR,
            CR_REC_NOT_WORK_ERR,
            CR_INDEX_OUT_BOUNDARY_ERR,
            CR_GRAB_IMG_ERR,
            CR_REC_IMG_ERR,
            CR_RECV_IMGHEAD_ERR,
            CR_MODE_COMM_NOT_MATCH_ERR,
            CR_FLU_PARAM_UNSET_ERR,
            CR_CAL_IN_PROCESS_ERR,
            CR_OFFSET_THREAD_BUSY_ERR,
            CR_OFFSET_THREAD_STOP_ERR,
            CR_CAL_INTERUPT_ERR,
            CR_STRING_EMPTY,
            CR_GAIN_UNSET,
            CR_MACHINEID_EMPTY,
            //image self test errors
            CR_IMAGE_AVG_ERR,
            CR_IMAGE_STD_ERRO,
            CR_IMAGE_UNIFORM_ERR,
            CR_IMAGE_STRIP_ERR,
            CR_IMAGE_RCN_ERR,//行噪声分析错误
            CR_IMAGE_LINEAR_ERR,//图像线性度错误
            CR_IMAGE_DARK_GRAY_ERR,
            CR_IMAGE_SYNC_ERR,	//图像是未同步下的
            CR_IMAGE_COLLIMATOR_ERR,//束光器设置错误
            CR_IMAGE_TUBE_ERR,//球管未对齐
            //Calibration parameters errors in application's config files
            CR_OFFSETCAL_NUM_ERR,
            CR_LINEAR_DOSENUM_ERR,
            CR_LINEAR_NUM_PERDOSE_ERR,
            CR_PORTABLEKV_ERR,

            CR_ADDPIXEL_MACHINE_ERR,
            CR_SEND_PCFILE_OLD_ERR,
            CR_SEND_PCFILE_MACHINEID_ERR,
            CR_CALIBRATION_FILE_NULL_ERR,

            CR_CREATE_AEC_ACQUIRE_THREAD_ERR,
            CR_ONEKEY_FIT_UNACCEPTABLE_ERR,
            CR_CREATE_MONITORTHREAD_ERR,//1080
            CR_CREATE_CLEARIMAGETHREAD_ERR,
            CR_AUTOSYNC_DARKIMAGE_ERR,
            CR_AUTOSYNC_RADIMAGE_ERR,
            CR_AUTOSYNC_INPUTIMAGES_ERR,
            CR_AUTOSYNC_BOUNDARYINDEXES_ERR,
            CR_POLYFIT_ORDERERROR,
            CR_EXPOSURE_UNIFORM_ERR
        };


        public class CareRayErrors
        {

            const int API_INDEX = 1000;


            //Errors in detector, the indexes start from 0
            //0 means call success
            public static string[] CrErrStrList_dtt = new string[] {
	
	"Normal exit without an error.",

	"Load device driver failed.",
	"Open detector failed.",
	"Map memory  failed.",
	"Allocate memory failed.",
	"No enough space on SD card.",		//5
	"No enough Space on Flash.",
	"File name is invalid, failed to exchange.",
	"File path is Invalid, failed to exchange.",
	"Read file error.",
	"Write file error.",				//10
	"Bad XML file.",
	"Command can not be recognized.",
	"Software version does not match.",
	"Incorrect number of parameters.",
	"Send frame error, transmission pipeline closed.",		//15
	"Execution Logic of API goes wrong.",
	"Have no select mode.",
	"Setting detector version failed.",
	"Undefined commandID.",
	"Undefined mode.",				//20
	"HandSwitch in error state.", 
	"setting detector time failed.",
	"access fpga register failed.",
	"update file failed.",
	"Invalid parameter.",				//25
	"Forbid to change mode at current transfer state.",
	"Forbid this operation at current transfer state.",
	"Invalid license, Please contact this detector manufacturer.",
	"Error happens in the self-test phase.",
	"ioctl calling failed.",	//30
	"frame stability test failed.",
	"Exchange config files error.",
	"Offset is unavailable.",
	"Creat thread failed.",
	"Error happens during readout.",	//35
	"Function is not supported at present.",
	"Read image CRC failed.",
	"Software has expired.",
	"has no fetchable frame.",
	"read frame number from FE is invalid.",
	"read frame attribution failed.",
	"invalid acquisition option.",
	"read-frame sequence is empty.",
	"Current detector doesn't support this sync mode.",
	"Current detector doesn't support this acquisition mode.",

	"Real time Dark setting error.",
	"Undefined config file.",
	"Configure file not exist.",
	"Error to create file.",
	"CRC Check error.",
	"Configure file already exist.",
	"Fail to open file."
};

            //Errors in API, the indexes start from API_INDEX
            public static string[] CrErrStrList_pc = new string[] {
	//---------Socket Layer--------------
	"Unable to connect detector. Possible causes include, but not limited to, employment of incompatible SDK versions, wrong IP settings, network issue.",
	"Unable to disconnect detector.",
	"Warning: job aborted, detector already connected.",
	"Warning: job aborted, detector already disconnected.",
	"Error reconnecting socket.",
	"Error closing socket.",
	"Error initiating Winsock.",
	"Error creating socket.",
	"Error getting socket options.",
	"Error setting socket options.",
	"Error sending data.",//10
	"Error receiving data.",
	"Detector disconnected while receiving data.",
	"Mismatched versions between detector and SDK.",
	//---------Codec Layer--------------
	"Wrong command ID.",
	"Wrong version. This error will be obsoleted in a future release.",
	"Wrong parameter in a command package sent from API to detector.",
	//---------V_API Layer--------------
	"A NULL pointer must not be passed to a function.",
	"Error reading configure file.",
	"Error writing configure file.",
	"Wrong user parameter.",//20
	"Unspecified check mode. Select the check mode as one of RAD|BINNING|PREVIEW| FLUOROSCOPY (for future version).",
	"Error loading DLL files.",
	"Error freeing DLL files.",
	"Error creating an event.",
	"Error terminating an event.",
	"Mismatched function ID sent to the detector and replied back from it.",
	"Error getting function address.",
	"Error creating a thread for image acquisition.",
	"Error creating a real-time thread.",
	"Error creating a heartbeat thread.",//30
	"Error creating a thread for offset image acquisition.",
	"Error creating a thread for image calibration.",
	"Error creating a logger thread.",
	"Error terminating a thread.",
	"Error allocating memory.",
	"Warning: job aborted, image acquisition in process.",
	"Error acquiring an image.",
	"Chinese characters Found in path. Path must not contain Chinese characters.",
	"No calibration file found in the specified path, or wrong path.",
	"Error reading a file.",//40
	"Error writing a file.",
	"Error exchanging files with detector.",
	"Warning: job aborted, fluoroscopic image acquisition in process.",
	"Warning: job aborted, fluoroscopic image acquisition stopped.",
	"Warning: job aborted, fluoroscopic image recording in process.",
	"Warning: job aborted, fluoroscopic image recording stopped.",
	"Index out of boundary.",
	"Error acquiring fluoroscopic images.",
	"Error recording fluoroscopic images.",
	"Error receiving image header.",//50
	"Unspecified check mode. Select the check mode as one of RAD|BINNING|PREVIEW| FLUOROSCOPY (for future version).",
	"Unspecified fluoroscopy mode.",	
	"Warning: job aborted, calibration in process.",
	"Warning: job aborted, the thread for offset image acquisition in process.",
	"Warning: job aborted, the thread for offset image acquisition stopped. This warning will be obsoleted in a future release.",
	"Error: calibration process interrupted.",
	"String or array must not be empty",
	"Unspecified gain image. This error will be obsoleted in a future release.",

	"No machine ID.",

	"Unqualified mean value of the image. This error will be obsoleted in a future release.",//60
	"Unqualified standard deviation of the image.",
	"Unqualified image uniformity.",
	"Unqualified mean values of the image strips.",
	"Unqualified row correlated noise (RCN) of the image.",
	"Unqualified image linearity.",
	"Unqualified mean value of the image. This error will be obsoleted in a future release.",
	"Unsynchronized detector. This error will be obsoleted in a future release.",
	"Part of the detector panel does not receive X-ray. Make sure the tube, the collimator, and the detector are properly aligned, and the generated X-ray illuminates the entire panel.",
	"Tube misaligned.",

	"Offset calibration requires more than 2 (inclusive) offset images.",
	"Gain calibration requires a minimum number of 5 (inclusive) different doses and a maximum number of 9 (inclusive).",
	"Gain calibration requires acquiring more than 2 (inclusive) images at each dose.",
	"Wrong string for setting portable KV.",
	"Information of the bad pixels does not pair with the detector. Bad pixels are not obtained from this detector.",
	"A newer calibration file on detector must not be overwritten with an older version.",
	"Calibration file does not pair with the detector.",
	"No calibration files exist on PC or detector. Calibrate the detector to obtain the calibration files.",
	"Error creating AEC acquire thread.",
	"One key fit result unacceptable",
	"Error creating monitor thread.",
	"Error creating clear image thread.",
	"Auto Sync input dark image error.",
	"Auto Sync input images sequence error.",
	"Auto Sync input images error, can't find reasonable band indexes.",
	"Auto Sync boundary indexes error, can't find reasonable boundary indexes.",
	"The order to do polyfit should not bigger than 6.",
	"Warning:The intensity of X-ray received by detector is not uniform enough."//1087,CR_EXPOSURE_UNIFORM_ERR
};


            public static string CrErrStrList(int index)
            {
                if (index < API_INDEX)
                {
                    return CrErrStrList_dtt[index];
                }
                else if (index >= API_INDEX)
                {
                    return CrErrStrList_pc[index - API_INDEX];
                }
                else
                {
                    return "";
                }
            }

        }
}
