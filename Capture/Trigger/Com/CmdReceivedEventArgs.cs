using System;
using System.Collections.Generic;
using System.Text;

namespace ImageCapturing
{
    public class CmdReceivedEventArgs:EventArgs
    {
        public byte Cmd;
        public byte D0;
        public byte D1;
        public byte[] Data;
        public CmdReceivedEventArgs(byte cmd, byte[] data)
        {
            this.Cmd = cmd;
            if (data.Length < 2) return;
            this.D0 = data[0];
            this.D1 = data[1];
            Data = new byte[data.Length-2];
            for (int i = 0; i < data.Length - 2; i++)       //外部取得的是D2开始的数据区
            {
                this.Data[i] = data[i+2];
            }
        }
    }
}
