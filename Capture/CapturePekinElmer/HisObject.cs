using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ImageCapturing
{
   public class HisHeader
   {
       public short FileID;// = 0x7000;
       public short HeaderSize;// = 68;
       public short HeadVerdion;// = 0;
       public uint FileSize;
       public short ImageHeaderSize;// = 32;
       public short ULX;
       public short ULY;
       public short BRX;
       public short BRY;
       public short NrOfFrames;
       public short Correction;
       public double IntegrationTime; // 毫秒
       public short TypeOfNumbers;
       public byte[] X;
       public byte[] ImageHeader;  
   }

    public class HisObject
    {
        public HisHeader hisHeader = new HisHeader();
        public List<ushort[,]>  dataList = null;

        /// <summary>
        /// Array Of Bytes to short
        /// </summary>
        /// <param name="offset">b 中的字节偏移量，将在此处开始读取字节</param>
        /// <param name="b">low byte is before </param>
        /// <returns></returns>
        public static short BytesToShort(ref int offset, byte[] b)
        {
            int i = 0;
            i += b[++offset];
            i += (b[++offset] << 8);
            return (short)i;
        }

        /// <summary>
        /// Array Of Bytes to short
        /// </summary>
        /// <param name="offset">b 中的字节偏移量，将在此处开始读取字节</param>
        /// <param name="b">low byte is before </param>
        /// <returns></returns>
        private static ushort BytesToUshort(ref int offset, byte[] b)
        {
            int i = 0;
            i += b[++offset];
            i += (b[++offset] << 8);
            return (ushort)i;
        }

        /// <summary>
        /// Array Of Bytes to uint
        /// </summary>
        /// <param name="b"> low byte is before </param>
        /// <returns></returns>
        private static double BytesToDouble(ref int offset, byte[] b)
        {
            double i = BitConverter.ToDouble(b, ++offset);
            offset += 7;
            return i;

            //long i = 0;
            //for (int j = 0; j < 8; j++)
            //{
            //    i += (b[++offset] << (j * 8));
            //}
            //return (double)i;
        }
        
        /// <summary>
        /// Array Of Bytes to uint
        /// </summary>
        /// <param name="b"> low byte is before </param>
        /// <returns></returns>
        public static uint BytesToUint(ref int offset, byte[] b)
        {
            long i = 0;
            i += b[++offset];
            i += (b[++offset] << 8);
            i += (b[++offset] << 16);
            i += (b[++offset] << 24);
            return (uint)i;
        }

        /// <summary>
        /// Array Of Bytes to int
        /// </summary>
        /// <param name="b"> low byte is before </param>
        /// <returns></returns>
        public static int BytesToInt(ref int offset, byte[] b)
        {
            int i = 0;
            i += b[++offset];
            i += (b[++offset] << 8);
            i += (b[++offset] << 16);
            i += (b[++offset] << 24);
            return i;
        }
        
        public static HisObject OpenFile(string file)
        {
            HisObject his = new HisObject();
            if(his.LoadDataFromFile(file))
            {
                return his;
            }
            else
            {
                return null;
            }
        }

        public bool LoadDataFromFile(string fname)
        {
            FileStream fs = new FileStream(fname, FileMode.Open);
            if (fs == null) return false;
            byte[] bytesOfHis = new byte[fs.Length];
            int len = fs.Read(bytesOfHis, 0, bytesOfHis.Length);
            if (len != bytesOfHis.Length || bytesOfHis.Length < 100)
            {
                //cls_MessageBox.Show("Error");
                fs.Close();
                fs.Dispose();
                return false;
            }

            int offset = -1;
            hisHeader.FileID = BytesToShort(ref offset, bytesOfHis);
            hisHeader.HeaderSize = BytesToShort(ref offset, bytesOfHis);
            if (hisHeader.FileID != 0x7000 && hisHeader.HeaderSize != 68)
            {
                fs.Close();
                fs.Dispose();
                return false;
            }
            hisHeader.HeadVerdion = BytesToShort(ref offset, bytesOfHis);
            hisHeader.FileSize = BytesToUint(ref offset, bytesOfHis);
            hisHeader.ImageHeaderSize = BytesToShort(ref offset, bytesOfHis);
            hisHeader.ULX = BytesToShort(ref offset, bytesOfHis);
            hisHeader.ULY = BytesToShort(ref offset, bytesOfHis);
            hisHeader.BRX = BytesToShort(ref offset, bytesOfHis);
            hisHeader.BRY = BytesToShort(ref offset, bytesOfHis);
            hisHeader.NrOfFrames = BytesToShort(ref offset, bytesOfHis);
            hisHeader.Correction = BytesToShort(ref offset, bytesOfHis);
            hisHeader.IntegrationTime = BytesToDouble(ref offset, bytesOfHis);
            hisHeader.TypeOfNumbers = BytesToShort(ref offset, bytesOfHis);

            dataList = new List<ushort[,]>(hisHeader.NrOfFrames);
            ushort[,] data = null;
            offset = 68 + 32 - 1;
            for (int n = 0; n < hisHeader.NrOfFrames; n++)
            {
                int row = hisHeader.BRY - hisHeader.ULY + 1;
                int col = hisHeader.BRX - hisHeader.ULX + 1;
                data = new ushort[row, col];
                for (int i = 0; i < row; ++i)
                {
                    for (int j = 0; j < col; ++j)
                    {
                        data[i, j] = BytesToUshort(ref offset, bytesOfHis);
                    }
                }
                dataList.Add(data);
            }
            fs.Close();
            fs.Dispose();
            fs = null;
            return true;
        }

        public static void SaveDataToHIS(ref HisHeader header, List<ushort[,]> dataList, string fname)
        {
            if (!fname.EndsWith(".his"))
            {
                fname += ".his";
            }
            FileInfo finfo = new FileInfo(fname);
            if (!finfo.Directory.Exists)
            {
                finfo.Directory.Create();
            }

            FileStream fs = new FileStream(fname, FileMode.Create, FileAccess.Write);
            fs.Write(BitConverter.GetBytes(header.FileID), 0, 2);
            fs.Write(BitConverter.GetBytes(header.HeaderSize), 0, 2);
            fs.Write(BitConverter.GetBytes(header.HeadVerdion), 0, 2);
            fs.Write(BitConverter.GetBytes(header.FileSize), 0, 4);
            fs.Write(BitConverter.GetBytes(header.ImageHeaderSize), 0, 2);
            fs.Write(BitConverter.GetBytes(header.ULX), 0, 2);
            fs.Write(BitConverter.GetBytes(header.ULY), 0, 2);
            fs.Write(BitConverter.GetBytes(header.BRX), 0, 2);
            fs.Write(BitConverter.GetBytes(header.BRY), 0, 2);
            fs.Write(BitConverter.GetBytes(header.NrOfFrames), 0, 2);
            fs.Write(BitConverter.GetBytes(header.Correction), 0, 2);
            fs.Write(BitConverter.GetBytes(header.IntegrationTime), 0, 8);
            fs.Write(BitConverter.GetBytes(header.TypeOfNumbers), 0, 2);
            fs.Write(header.X, 0, header.X.Length);
            fs.Write(header.ImageHeader, 0, header.ImageHeader.Length);
            int count = dataList.Count;
            int barLen = count + 1;
            for (int n = 0; n < dataList.Count; n++)
            {
                ushort[,] data = dataList[n];
                int row = data.GetLength(0);
                int col = data.GetLength(1);
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        fs.Write(BitConverter.GetBytes(data[i, j]), 0, 2);
                    }
                }
            }
            fs.Flush();
            fs.Dispose();
        }

    }
}