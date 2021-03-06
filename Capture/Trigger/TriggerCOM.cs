﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.IO;

namespace ImageCapturing
{
    public class TriggerCOM : TriggerBase
    {
         public TriggerCOM()
            : base()
        {
            
        }

        public override void InitParam()
        {
            base.InitParam();

            ReadCOMConfig();
        }


        #region Members
        const int BaudRate = 9600;//38400;
        private int ComBufferSize = 2048;
        public bool AutoConnect = true;
        private bool HaveSendData = false;
        private byte bakSendCmd;
        private byte[] bakSendData;

        private byte TimeOutCount = 0;
        private byte NoReplyCount = 0;
        /// <summary>
        /// the max reply count
        /// </summary>
        private int MaxReplyCount = 3;
        private int ComPortWriteTimeOut = 1000;
        private int ComPortReadTimeOut = 2000;
        private int ComErrorSleepTime = 500;
       
        private SerialPort PortPtr;

        private string COMName = "";
        #region private variable for receive

        private byte[] ByteBuf = null;
        private byte[] ByteBuf2 = null;


        List<byte> mBuffers = new List<byte>();
        bool RecognizeCommandHead = false;


        #endregion
        #endregion

 
        void ReadCOMConfig() 
        {

            if (!int.TryParse(CapturePub.readCaptrueValue("ComBufferSize"), out ComBufferSize))
            {
                ComBufferSize = 2048;
            }
            if (!int.TryParse(CapturePub.readCaptrueValue("MaxReplyCount"), out MaxReplyCount))
            {
                MaxReplyCount = 3;
            }
            if (!int.TryParse(CapturePub.readCaptrueValue("ComPortWriteTimeOut"), out ComPortWriteTimeOut))
            {
                ComPortWriteTimeOut = 1000;
            }
            if (!int.TryParse(CapturePub.readCaptrueValue("ComPortReadTimeOut"), out ComPortReadTimeOut))
            {
                ComPortReadTimeOut = 2000;
            }
            if (!int.TryParse(CapturePub.readCaptrueValue("ComErrorSleepTime"), out ComErrorSleepTime))
            {
                ComErrorSleepTime = 500;
            }

            COMName = CapturePub.readCaptrueValue("ComName");

            ByteBuf = new byte[ComBufferSize];
            ByteBuf2 = new byte[ComBufferSize];

        }


        void Receive(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("Receive:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            while (PortPtr.BytesToRead > 0)
            {
                byte b = (byte)PortPtr.ReadByte();
                mBuffers.Add(b);
                CompareCommandData();
            }

            //Console.WriteLine(PortPtr.BytesToRead);

            //int num = PortPtr.BytesToRead > ComBufferSize ? ComBufferSize : PortPtr.BytesToRead;
            //int count = PortPtr.Read(ByteBuf2, 0, num);

            ////Console.WriteLine(num);
            //Console.WriteLine(count);

            //string str = (ASCIIEncoding.ASCII.GetString(ByteBuf2, 0, count));

            //Console.WriteLine(str);

        }

        void ErrorReceive(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("ErrorReceive:" + DateTime.Now);
            Console.WriteLine(e.EventType);
        }
       
        void PinChange(object sender, SerialPinChangedEventArgs e)
        {
            Console.WriteLine("PinChange:" + DateTime.Now);
            Console.WriteLine(PortPtr.BytesToRead);
            Console.WriteLine(e.EventType);
        }

        void CompareCommandData()
        {
            //Trigger 设置
            //if (command == 0x01 
            //    || command == 0x02
            //    || command ==  0x81)
            if (RecognizeCommandHead)
            {
                if (mBuffers.Count >= 14)
                {
                    byte Xor = 0x00;
                    for (int i = 0; i < mBuffers.Count - 1; i++)
                    {
                        Xor = (byte)(Xor ^ mBuffers[i]);
                    }

                    if (Xor == mBuffers[mBuffers.Count - 1])
                    {
                        Console.WriteLine("Correct feedback...");
                    }
                    else
                    {
                        Console.WriteLine("Error feedback...");
                    }

                    for (int i = 0; i < mBuffers.Count; i++)
                    {
                        Console.Write("{0:X2} ", mBuffers[i]);//十六进制大写输出  
                    }
                    Console.WriteLine();
                    Console.WriteLine();

                    //清空状态
                    RecognizeCommandHead = false;
                    mBuffers.Clear();
                }
                else
                {
                    //不处理
                }

            }
            else if (mBuffers.Count >= 4)
            {
                bool v = mBuffers[mBuffers.Count - 1] == 0x88
                && mBuffers[mBuffers.Count - 2] == 0x77
                && mBuffers[mBuffers.Count - 3] == 0x66
                && mBuffers[mBuffers.Count - 4] == 0x55;

                RecognizeCommandHead = true;
                mBuffers.RemoveRange(0, mBuffers.Count - 4);
            }
        }

        public void Open()
        {
            //打开串口
            try
            {
                if (PortPtr != null && PortPtr.IsOpen)
                {
                    PortPtr.Close();
                }

                PortPtr = new SerialPort(COMName, BaudRate);
                PortPtr.ReadBufferSize = ComBufferSize;
                PortPtr.WriteBufferSize = ComBufferSize;
                PortPtr.WriteTimeout = ComPortWriteTimeOut;
                PortPtr.ReadTimeout = ComPortReadTimeOut;
                PortPtr.DataReceived += new SerialDataReceivedEventHandler(Receive);
                PortPtr.ErrorReceived += new SerialErrorReceivedEventHandler(ErrorReceive);
                PortPtr.PinChanged += new SerialPinChangedEventHandler(PinChange);

                PortPtr.Open();


                Console.WriteLine("open com:" + PortPtr.PortName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


        }

        /// <summary>
        /// 关闭端口句柄
        /// </summary>
        public void Close()
        {
            if (PortPtr != null)
            {
                PortPtr.DataReceived -= new SerialDataReceivedEventHandler(Receive);
                PortPtr.ErrorReceived -= new SerialErrorReceivedEventHandler(ErrorReceive);
                PortPtr.PinChanged -= new SerialPinChangedEventHandler(PinChange);
                PortPtr.Close();
                Console.WriteLine("close port.");
            }
        }


        public override void Dispose()
        {
            base.Dispose();

            TriggerTime.Stop();
            TriggerTime.Close();

            Close();

        }


        public void SendCMD(byte[] data)
        {
            PortPtr.Write(data, 0, data.Length);
        }
 



    }
}