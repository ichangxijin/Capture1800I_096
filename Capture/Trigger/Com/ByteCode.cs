using System;
using System.Collections.Generic;
using System.Text;

namespace ImageCapturing
{
    /// <summary>
    /// 按通信协议将命令和数据编码到字节数组ByteBuf
    /// </summary>
    class EnCode
    {
        /// <summary>
        /// EnCode的构造函数
        /// </summary>
        public EnCode()
        {
            byteBuf = new byte[BufferSize];
        }

        #region 属性
        /// <summary>
        /// 按协议封装好的命令数据帧
        /// </summary>
        public byte[] ByteBuf
        {
            get { return byteBuf; }
        }

        /// <summary>
        /// 整个数据帧的长度
        /// </summary>
        public int BufCount
        {
            get { return bufCount; }
        }
        #endregion

        #region 私有变量
        private byte[] byteBuf;
        private int bufCount;
        private byte Cmd;
        private int BufferSize = 256;
        private Terminal_Address DestAddr;
        #endregion

        #region 公有函数
        /// <summary>
        /// 按通信协议将命令和数据封装进字节数组ByteBuf
        /// </summary>
        /// <param name="cmd">要写入的命令号</param>
        /// <param name="data">要写入的数据</param>
        public void CmdDataToBuffer(byte cmd, byte[] data)
        {
            CmdDataToBuffer(cmd, data, Terminal_Address.BeamTrigger);
        }
        #endregion

        #region 私有函数
        private void AddFrameHead()
        {
            int L = bufCount - 8 + 5;
            byteBuf[0] = 0xff;
            byteBuf[1] = 0xff;
            byteBuf[2] = 0xff;
            byteBuf[3] = 0xff;
            byteBuf[4] = (byte)~L;
            byteBuf[5] = (byte)L;
            byteBuf[6] = (byte)(((int)DestAddr << 4) | (int)Terminal_Address.PC);//0x41;
            byteBuf[7] = Cmd;
        }

        private void AddDataCheck()
        {
            //UInt16 CheckCRC = CRC.CalcuCRC16_1021(ByteBuf, 8, BufCount - 8);           
            //byteBuf[bufCount++] = 0;//byteBuf[BufCount++] = PubFun.getLoBit(CheckCRC);
            //byteBuf[bufCount++] = 0;//byteBuf[BufCount++] = PubFun.getHiBit(CheckCRC);
            byte xor = CRC.CalcuXor(byteBuf, 4, bufCount - 4);
            byteBuf[bufCount++] = xor;
        }

        private void CmdDataToBuffer(byte cmd, byte[] data, Terminal_Address Dest)
        {
            bufCount = 8; //前面留8个字节填帧头信息
            Cmd = cmd;
            if (data != null)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    byteBuf[bufCount++] = data[i];
                }
            }
            //if (Cmd == PDC_ConnectRequest)   //添加版本信息，便于底层识别
            //{
            //    byteBuf[BufCount++] = PrimaryVersion;   //主版本号
            //    byteBuf[BufCount++] = SlaveVersion;     //子版本号
            //}
            DestAddr = Dest;
            AddFrameHead();
            AddDataCheck();
        }
        #endregion
    }

    /// <summary>
    /// 按通信协议将字节数组解码到对应的命令号和数据
    /// </summary>
    class DeCode
    {
        /// <summary>
        /// DeCode的构造函数
        /// </summary>
        public DeCode()
        {

        }

        #region 属性
        /// <summary>
        /// 从字节数组中读到的命令号
        /// </summary>
        public byte Cmd
        {
            get { return mCmd; }
        }

        /// <summary>
        /// 从字节数组中读到的数据
        /// </summary>
        public byte[] Data
        {
            get { return mData; }
        }
        #endregion

        private const int MinDataFrameLength = 9;

        #region 私有变量
        private byte mCmd;
        private byte[] mData;
        private byte[] ByteBuf;
        private int ReadPos;
        private int BufSize;
        private byte[] IOValue = new byte[9];
        private byte[] ReceiveBuffer = new byte[4096];
        private bool bIsRun = false;
        private byte Length, CheckSum;
        private int uDataCount = 0;
        #endregion

        #region 公有函数

        public void Init()
        {
            uDataCount = 0;
        }

        private void DelBufData(int DelCount)
        {
            for (int i = 0; i < uDataCount - DelCount; i++)
            {
                ReceiveBuffer[i] = ReceiveBuffer[i + DelCount];
            }
            uDataCount -= DelCount;
        }

        public bool ByteToCmdData(byte[] ReadBytes, int count, out byte ErrorCode)
        {
            ErrorCode = 0x00;
                
            //add to buffer.
            for (int i = 0; i < count; i++)
            {
                ReceiveBuffer[uDataCount++] = ReadBytes[i];
            }

            //try to read frame data.
            if (uDataCount < MinDataFrameLength)
            {
                return false;
            }

            int sn = 0;
            int DelCount = 0;
            while (sn < uDataCount - MinDataFrameLength)
            {
                if (ReceiveBuffer[sn] == 0xFF
                    && ReceiveBuffer[sn + 1] == 0xFF
                    && ReceiveBuffer[sn + 2] == 0xFF
                    && ReceiveBuffer[sn + 3] == 0xFF
                    && (ReceiveBuffer[sn + 4] ^ ReceiveBuffer[sn + 5]) == 0xFF)
                {//read frame.
                    Length = ReceiveBuffer[sn + 5];
                    if (Length + 6 > uDataCount)
                    {//need to continue read com buffer.
                        DelBufData(sn);
                        return false;
                    }

                    if ((ReceiveBuffer[sn + 6] >> 4) != (int)Terminal_Address.PC)
                    {//frame data error, need clear some data.
                        DelBufData(sn + 4);
                        ErrorCode = 0x01;
                        return false;
                    }

                    CheckSum = 0x00;
                    for (int i = 0; i < Length; i++)
                    {
                        CheckSum ^= ReceiveBuffer[sn + 6 + i];
                    }
                    if (CheckSum != 0xFF)
                    {//CheckSum error, need clear some data.
                        DelBufData(sn + 4);
                        ErrorCode = 0x02;
                        return false;
                    }

                    //0xff,0xff,0xff,0xff,~L,L,dst&src,cmd,data[n],xor. ( length = n+3 )
                    mCmd = ReceiveBuffer[sn + 7];
                    mData = new byte[Length - 3];
                    for (int i = 0; i < mData.Length; i++)
                    {
                        mData[i] = ReceiveBuffer[sn + 8 + i];
                    }
                    DelBufData(sn + Length + 6);
                    return true;
                }
                else
                {
                    sn++;
                }
            }
            return false;
        }
        
        // 0xff,0xff,0xff,0xff,~L,L,dst&src,cmd,data[n],xor. ( length = n+3 )
        public bool ByteToCmdData(byte ReadByte, out byte ErrorCode)
        {
            bool GetFrame = false;
            ErrorCode = 0x0;
            for (int i = 8; i > 0; i--) IOValue[i] = IOValue[i - 1];

            IOValue[0] = ReadByte;

            byte tmp = (byte)(IOValue[8] & IOValue[7] & IOValue[6] & IOValue[5]);
            if (tmp == 0xff && IOValue[4] != 0xff)
            {
                if ((IOValue[4] ^ IOValue[3]) == 0xff)
                {
                    //if(!bIsRun)   // 允许标示头之后的数据之间存在伪标示头作为数据
                    Length = IOValue[3];
                    CheckSum = (byte)(~(IOValue[2] ^ IOValue[1]));
                    uDataCount = 4;
                    bIsRun = true;
                    for (int i = 0; i < 4; i++) ReceiveBuffer[i] = IOValue[4 - i];
                }
            }
            if (bIsRun)
            {
                ReceiveBuffer[uDataCount++] = IOValue[0];             // ReceiveBuffer[0]为~L,ReceiveBuffer[4]为data[0],
                CheckSum ^= IOValue[0];
                if (uDataCount > Length + 1)                           // 接收特定长度的数据后，判断校验和对否？
                {
                    if (CheckSum == 0x00)                                                 // 协议决定的
                    {
                        if ((ReceiveBuffer[2] >> 4) == (int)Terminal_Address.PC)            // 目标板来的数据
                        {
                            mCmd = ReceiveBuffer[3];
                            mData = new byte[Length - 3];
                            for (int i = 0; i < mData.Length; i++)
                            {
                                mData[i] = ReceiveBuffer[4 + i];
                            }
                            GetFrame = true;
                        }
                        else
                        {
                            ErrorCode = 0x01;
                        }
                    }
                    else
                    {
                        ErrorCode = 0x02;
                    }
                    bIsRun = false;
                }
            }
            return GetFrame;
        }
        #endregion
    }
}