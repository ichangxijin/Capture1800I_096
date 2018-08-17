using System;
using System.Collections.Generic;
using System.Text;

namespace ImageCapturing
{
    class ComFun
    {
        public static byte[] GetByteArray2(UInt16 data)
        {
            byte[] temp = new byte[2];
            temp[0] = getLoBit(data);
            temp[1] = getHiBit(data);

            return temp;
        }

        public static byte getHiBit(UInt16 data)
        {
            return (byte)((data >> 8) & 0xFF);
        }

        public static byte getLoBit(UInt16 data)
        {
            return (byte)(data & 0xFF);
        }

        public static short GetShortByD0D1(byte d0, byte d1)
        {
            return (short)((d1 << 8) + d0);
        }
    }
}
