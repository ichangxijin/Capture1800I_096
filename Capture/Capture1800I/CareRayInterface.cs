using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ImageCapturing
{
    public unsafe partial class CareRayInterface
    {
        public const string API_1800I_DLL = "CrApi.dll";

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_fpga_reg(ref FpgaReg fpga);
        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_detector_iparam(int prmId, int value);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_gain_id();
        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_gain_id(int value);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_connect_detector(byte* workDirectory);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_disconnect_detector();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_detector_type();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_status_info(ref CareRayInterface.StatusInfo StatusInfo);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_correction(ref CareRayInterface.Correction corr);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_correction(ref CareRayInterface.Correction corr);

        //[DllImport(API_1800I_DLL)]
        //public static extern int CR_set_userCorrection(ref CareRayInterface.UserCorrection corr);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_userCorrection(ref CareRayInterface.UserCorrection corr);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_ApiVersion(char* versting);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_detector_info(ref CareRayInterface.DetectorInfo detectorInfo);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_mode_info(int modeid, ref CareRayInterface.ModeInfo modeInfo);

        /** get mode information */
        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_extend_mode_info(int modeid, ref CareRayInterface.ExtendModeInfo modeInfo);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_check_mode(int modeid);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_current_mode(int modeid);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_current_mode(ref int modeid);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_sync_mode(int syncMode);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_cycle_time(int exp_time, int delay_time, int wait_time);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_user_correction(ref CareRayInterface.UserCorrection userCorr);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_normal_power();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_save_power();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_start_acq_full_image();
        [DllImport(API_1800I_DLL)]
        public static extern int CR_start_acq_dark_full_image();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_query_prog_info(int ProgType, ref CareRayInterface.ExpProgress expProg);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_permit_exposure();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_reset_detector(int resetboot);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_image_attr(ref CareRayInterface.FrameAttr attr);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_image(int imageSize, bool with_head, ushort* pImage);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_stop_acq_frame_cleanup();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_stop_acq_frame();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_stop_cal_procedure(int bl);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_execute_linear_cal_acqImg(byte* ImgDir);
        [DllImport(API_1800I_DLL)]
        public static extern int CR_execute_linear_cal_calcGain();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_execute_linear_cal();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_execute_portable_cal();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_inpaint_bad_pixels(ushort* pimage);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_detector_config(int i, int i2);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_cal_thread(int boolval);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_cal_params(ref CareRayInterface.CalParams cal_params);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_cal_offset(int currentMode);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_linear_calibration();
        [DllImport(API_1800I_DLL)]
        public static extern int CR_linatech_calibration();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_xray_io_active(bool active);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_sync_active(bool active);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_set_nosync_exp_prepare();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_get_config_info(ref CareRayInterface.ConfigInfo configInfo);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_rad_get_image(int currentMode, int imageType, int imageSize, ushort* pAcq);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_rad_permit_exp();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_rad_query_prog_info(int progType, ref CareRayInterface.ExpProgress calProg);

        //soft sync API
        [DllImport(API_1800I_DLL)]
        public static extern int CR_register_callback(EventCallbackDelegate EventCallback);

        [DllImport(API_1800I_DLL)]
        public static extern int CR_send_exp_request();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_ready_state_request();

        [DllImport(API_1800I_DLL)]
        public static extern int CR_start_soft_acquisition();

    }
}
