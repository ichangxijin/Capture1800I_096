using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.Collections;
using System.IO;

namespace ImageCapturing
{
    public class ComPort
    {
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
        #region private variable for receive

        private byte[] ByteBuf = null;
        private byte[] ByteBuf2 = null;
        private Thread ReceiveThread;
        public delegate void CmdReceivedEventHandler(object sender, CmdReceivedEventArgs e);
        public event CmdReceivedEventHandler CmdReceived;
        #endregion
        #endregion

        #region Public Functions

        public ComPort(string com) 
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

            ByteBuf = new byte[ComBufferSize];
            ByteBuf2 = new byte[ComBufferSize];


            //创建接收线程
            ThreadStart threadDelegate = new ThreadStart(Receive);
            ReceiveThread = new Thread(threadDelegate);

            //打开串口
            try
            {
                PortPtr = new SerialPort(com, BaudRate);
                PortPtr.ReadBufferSize = ComBufferSize;
                PortPtr.WriteBufferSize = ComBufferSize;
                PortPtr.WriteTimeout = ComPortWriteTimeOut;
                PortPtr.ReadTimeout = ComPortReadTimeOut;
                PortPtr.DataReceived += new SerialDataReceivedEventHandler(Receive);
                PortPtr.ErrorReceived += new SerialErrorReceivedEventHandler(ErrorReceive);
                PortPtr.PinChanged += new SerialPinChangedEventHandler(PinChange);

                Open(com);
            }
            catch (Exception ex)
            {
                Database.LogDB.SaveTxtLog(ex.ToString());
            }
        }


        List<byte> mBuffers = new List<byte>();
        bool RecognizeCommandHead = false;


        void CompareCommand()
        {
            //Trigger 设置
            //if (command == 0x01 
            //    || command == 0x02
            //    || command ==  0x81)
            if(RecognizeCommandHead)
            {
                if(mBuffers.Count >= 14)
                {
                    byte Xor = 0x00;
                    for(int i =0; i < mBuffers.Count - 1;i++)
                    {
                        Xor =  (byte)(Xor ^ mBuffers[i]);
                    }
                    if(Xor == mBuffers[mBuffers.Count - 1])
                    {
                        Console.WriteLine("Correct feedback...");
                    }
                    else
                    {
                        Console.WriteLine("Error feedback...");
                    }

                    for (int i = 0; i < mBuffers.Count - 1; i++)
                    {
                        Console.Write("{0:X}", mBuffers[i]);//十六进制大写输出  
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
            else if(mBuffers.Count >= 4)
            {
                bool v = mBuffers[mBuffers.Count - 1] == 0x88
                && mBuffers[mBuffers.Count - 2] == 0x77
                && mBuffers[mBuffers.Count - 3] == 0x66
                && mBuffers[mBuffers.Count - 4] == 0x55;

                RecognizeCommandHead = true;
                mBuffers.RemoveRange(0,mBuffers.Count - 4);
            }
        }




        private void Receive(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("Receive:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            while (PortPtr.BytesToRead > 0)
            {
                byte b = (byte)PortPtr.ReadByte();
                mBuffers.Add(b);
                CompareCommand();
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

        public void Open(string com)
        {
            if (PortPtr != null && PortPtr.IsOpen)
            {
                PortPtr.Close();
            }

            PortPtr.Open();

            Console.WriteLine("open com:" + PortPtr.PortName);

            //ReceiveThread.Start();

        }

        /// <summary>
        /// 关闭端口句柄
        /// </summary>
        public void ClosePortptr()
        {
            if (ReceiveThread != null)
            {
                ReceiveThread.Abort();
            }
            if (PortPtr != null)
            {
                PortPtr.DataReceived -= new SerialDataReceivedEventHandler(Receive);
                PortPtr.ErrorReceived -= new SerialErrorReceivedEventHandler(ErrorReceive);
                PortPtr.PinChanged -= new SerialPinChangedEventHandler(PinChange);
                PortPtr.Close();
                Console.WriteLine("close port.");
            }
        }



        public bool SendCmdNew(byte[] data)
        {
            PortPtr.Write(data, 0, data.Length);

            return true;
        }
 

        /// <summary>
        /// 关闭摧毁线程
        /// </summary>
        public void Dispose()
        {
            ClosePortptr();
        }

        #endregion

        #region private function


        private void Receive()
        {
            while (true)
            {
                if (!PortPtr.IsOpen)
                {
                    continue;
                }
                int num = PortPtr.BytesToRead > ComBufferSize ? ComBufferSize : PortPtr.BytesToRead;
                int count = PortPtr.Read(ByteBuf, 0, num);

                if (count > 0)
                {
                    string str = (ASCIIEncoding.ASCII.GetString(ByteBuf, 0, count));

                    Console.WriteLine(str);
                }
            }
        }


        #endregion
    }
}