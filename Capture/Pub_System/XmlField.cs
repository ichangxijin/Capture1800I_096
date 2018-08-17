using System;
using System.Collections.Generic;
using System.Text;

namespace ImageCapturing
{
    public class XmlField
    {
        public static string CaptureMode = "CaptureMode";
        public static string IntegrationTime = "IntegrationTime";
        public static string CaptureFrameCount = "CaptureFrameCount";
        public static string FrameDelayTime = "FrameDelayTime";
        public static string GainMode = "GainMode";
        public static string BinningMode = "BinningMode";
        public static string ImageCorrection = "ImageCorrection";
        public static string LinkPanel = "LinkPanel";           // 是否连接Panel
        public static string SimImagePanel = "SimImagePanel";   // 是否模拟Panel
        public static string NeedCheckPanelInterface = "NeedCheckPanelInterface"; //是否需要检查dll,当改变接口的时候是真的需要检查
        public static string PanelInterface = "PanelInterface"; // 数据传输接口是否是网络接口
        public static string ShowBeamButton = "ShowBeamButton";
        public static string PanelBrandName = "PanelBrandName";

        public static string OffsetFile = "OffsetFile";
        public static string GainFile = "GainFile";
        public static string GainSeqFile_Image = "GainSeqFile";
        public static string GainSeqFile_Dose = "GainSeqFile_Dose";
        public static string PixelMapFile = "PixelMapFile";
        public static string OpenFilePath = "OpenfilePath";

        public static string TriggerPort = "TriggerPort";
        public static string TriggerDelayTime = "TriggerDelayTime";
        public static string SignalSourceID = "SignalSourceID";
        public static string SignalTimeout = "SignalTimeout";
        public static string SignalInterval = "SignalInterval";
        public static string SignalFeedInterval = "SignalFeedInterval";
        public static string AutoComFeedback = "AutoComFeedback";
        public static string MachineQAMachineData = "MachineQAMachineData";//2011.12.22
        public static string GetArmParameterMode = "GetArmParameterMode";
        public static string TriggerMode = "TriggerMode";
        public static string SortGainFolder = "SortGainFolder";

        public static string CareRay_ExposureTime = "CareRay_ExposureTime";
        public static string CareRay_ExposureDelay = "CareRay_ExposureDelay";
        public static string CareRay_ExposureWait = "CareRay_ExposureWait";
        public static string CareRay_ExposureSycMode = "CareRay_ExposureSycMode";
    }
}
