using System;
using System.Collections.Generic;
using System.Text;

namespace ImageCapturing
{
    public enum Terminal_Address
    {
        PC = 0x01,
        MCU = 0x02, //主控板
        DSP = 0x03, //数字信号处理器
        BeamTrigger = 0x04 //Beam Trigger 系统
    }

    public class Command_Protocol
    {
        public const byte CMD_ConnectRequest = 0x10; //连接请求
        public const byte CMD_BeamInfo = 0x11; //Beam源的配置参数
        public const byte CMD_WorkStatus = 0x12; //工作模式配置
        public const byte CMD_QueryBeamOn = 0x13;//工作模式为响应回传模式时，此命令用于查询当前BeamOn状态

        public const byte DP_Disconnected = 0xff;       //PC内部使用，表示通信中断
    }
}
