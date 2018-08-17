using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace ImageCapturing
{
    public class CRC
    {
        public CRC()
        { 
        
        }

        public static byte CalcuXor(byte[] data, int start, int count)
        { 
            byte Xor = 0;
            for(int i = 0; i < count; i++)
            {
                Xor = (byte)(Xor ^ data[start + i]);
            }

            return Xor;
        }

        public static UInt16 CalcuCRC16_1021(byte[] data, int start, int count)
        { 
            int pos = start;
            UInt16 u16_crc = 0;
            while(count-- > 0)
            {
                byte value = data[pos++];
                for(byte i = 0x80; i != 0; i = (byte)(i >> 1))
                {
                    if( (u16_crc & 0x8000) != 0 )
                    {
                        u16_crc = (UInt16)(u16_crc << 1);
                        u16_crc ^= 0x1021;
                    }
                    else
                    {
                        u16_crc = (UInt16)(u16_crc << 1);
                    }

                    if( (value & i) != 0 )
                    {
                        u16_crc ^= 0x1021;
                    }
                }
            }
            return u16_crc;
        }

        public static UInt32 Topslane_CRC(byte[] buff, UInt32 len)
        {
            UInt32 seed = 0x8035;
            UInt32 crc = rtpcrc(buff, len, seed);
            return crc;
        }

        public static UInt32 RTP_CRC(byte[] buff, UInt32 len)
        {
            UInt32 seed = 0x0521;
            UInt32 crc = rtpcrc(buff, len, seed);
            return crc;
        }

        public static UInt32 rtpcrc(byte[] buff, UInt32 len, UInt32 seed)
        {
            int pos = 0;

            UInt32[] crc_tbl =
            {
                0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
                0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
                0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
                0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
                0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,

                0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
                0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
                0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
                0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
                0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,

                0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
                0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
                0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,
                0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
                0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,

                0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
                0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
                0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
                0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
                0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,

                0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
                0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
                0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
                0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
                0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,

                0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
                0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
                0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
                0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,
                0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,

                0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
                0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040,
            };

            while (len != 0)
            {
                seed = crc_tbl[buff[pos] ^ (byte)seed] ^ (seed >> 8);
                pos++;
                len--;
            }
            return (seed);
        }

        public static uint GetTopFileCRC(string topFile)
        {
            byte IO0;

            uint crc = 0;
            FileInfo file = new FileInfo(topFile);
            using (System.IO.Stream s = file.OpenRead())
            {//openFileDialog1.OpenFile();    //用2进制方式读取文件
                byte[] buf = new byte[s.Length];
                for (int i = 0; i < s.Length; )
                {
                    IO0 = (byte)s.ReadByte();
                    if (IO0 != 0x0D)   //回车不读
                    {
                        buf[i] = IO0;
                        i++;
                    }
                }

                crc = GetCRCByBuf(buf);
                s.Close();
            }

            return crc;
        }

        /// <summary>
        /// 读取带有注释的crc
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static uint GetCRCWithoutNote(string topFile, string noteSign, string endSign)
        {
            //string noteSign = "||"; // 注释标示符号            
            uint crc = 0;

            string values = "";
            using (StreamReader sr = new StreamReader(topFile, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (line.IndexOf(noteSign) < 0 && line.Trim() != "")
                    {
                        values += "\n" + line.Trim();
                    }
                }
                sr.Close();
            }

            char[] bufC = values.ToCharArray();
            byte[] buf = new byte[bufC.Length];
            for (int i = 0; i < bufC.Length; ++i)
            {
                buf[i] = (byte)bufC[i];
            }
            crc = GetCRCByBuf(buf, endSign);

            #region Byte Fun
            /*FileInfo file = new FileInfo(topFile);                              
            using (System.IO.Stream s = file.OpenRead())//openFileDialog1.OpenFile();    //用2进制方式读取文件
            {
                byte IO0;
                byte sign = 0x7C;//|
                byte enter = 0x0A;//\n
                int crcIndex = 0; //CRC开始的标示序号
                List<byte> bufList = new List<byte>();
                //byte[] buf = new byte[s.Length-noteIndex];
                //s.Seek(noteIndex, SeekOrigin.Begin);
                for (int i = 0; i < s.Length; )
                {
                    IO0 = (byte)s.ReadByte();
                    if (IO0 != 0x0D)   //回车不读
                    {
                        bufList.Add(IO0);
                        if(bufList.Count > 3)
                        {
                            if (bufList[bufList.Count - 2] == enter)
                            {
                                if(bufList[bufList.Count - 1] == sign)
                                {//读取\n!表示以前的为注释以及下一行也为注释
                                    crcIndex = bufList.Count;
                                }
                                else//回车后读取不是sign，则表示此行为非注释
                                {
                                    if (bufList[bufList.Count - 1] == enter)
                                    {//删除空行
                                        bufList.RemoveAt(bufList.Count - 1);
                                    }
                                }
                            }
                            else if (bufList[bufList.Count - 1] == enter)
                            {
                                if (crcIndex != 0)
                                {//bufList中保留非注释的字符
                                    bufList.RemoveRange(crcIndex-2, bufList.Count - crcIndex);
                                    crcIndex = 0;
                                }
                            }
                        }
                        i++;
                    }
                }

                crc = GetCRCByBuf(bufList.ToArray());
                s.Close();
            }*/
            #endregion

            return crc;
        }

        private static uint GetCRCByBuf(byte[] buf)
        {
            //UInt32 CrcPos = 2;
            //for (CrcPos = 2; CrcPos < buf.Length; CrcPos++)
            //{
            //    if (buf[CrcPos - 2] == 'C' && buf[CrcPos - 1] == 'R' && buf[CrcPos] == 'C')
            //    {
            //        break;
            //    }
            //}
            ////在C#里读文件读到文件末尾有回车(13)和换行(10)，而CBUILD里只有换行(10)，
            ////为了兼容CBUILD下的计划系统导出的文件，所以这个把导入的文件做了一下改变
            ////回车不读
            //uint CRC = Topslane_CRC(buf, CrcPos - 2);

            return GetCRCByBuf(buf, "CRC");
        }

        private static uint GetCRCByBuf(byte[] buf, string sign)
        {
            int CrcPos = 2;
            int index = sign.Length - 1;
            char[] signC = sign.ToCharArray();
            for (CrcPos = index; CrcPos < buf.Length; CrcPos++)
            {
                bool flag = false;
                for (int i = 0; i < sign.Length; ++i)
                {
                    if (buf[CrcPos - index + i] == (byte)signC[i])
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    break;
                }
            }
            //在C#里读文件读到文件末尾有回车(13)和换行(10)，而CBUILD里只有换行(10)，
            //为了兼容CBUILD下的计划系统导出的文件，所以这个把导入的文件做了一下改变
            //回车不读
            uint CRC = Topslane_CRC(buf, (uint)CrcPos - 2);

            return CRC;
        }
    }
}
