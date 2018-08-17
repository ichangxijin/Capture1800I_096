using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ImageCapturing
{

    public class HisLUT
    {
        public static Color[] RefreshColorLUT(Color[] mColorLUT)
        {
            if (mColorLUT == null)
            {
                mColorLUT = new Color[256];
            }
            for (int i = 0; i <= 255; i++)
            {
                mColorLUT[i] = Color.FromArgb(i, i, i);
            }

            return mColorLUT;
        }

        public static void RefreshLUT(ref Color[] mLUT, Color[] mColorLUT, bool Converse, int level, int window, int lutLength)
        {
            if (mColorLUT == null)
            {
                mColorLUT = RefreshColorLUT(mColorLUT);
            }

            Point minMax = GetCminCmax(level, window);
            int cmin = minMax.X;
            int cmax = minMax.Y;
            
            //Tyh 2008.07.08 modify.将Converse信息加入LUT中，外部不再传递此参数
            Color[] tmp = new Color[256];
            if (Converse)
            {
                for (int i = 0; i < 256; i++)
                {
                    tmp[i] = mColorLUT[255 - i];
                }
            }
            else
            {
                for (int i = 0; i < 256; i++)
                {
                    tmp[i] = mColorLUT[i];
                }
            }

            if (mLUT == null || mLUT.Length != lutLength)
            {
                mLUT = new Color[lutLength];
            }

            
            //if (cmin < min) cmin = min; //2010/10/13 add 防止cmin小于min导致的索引异常。(cmin=0,min=1时候错误)
            if (cmin > cmax) cmax = cmin;
            if (cmax > lutLength) cmax = lutLength;
            if (cmin > lutLength) cmin = lutLength;
            float k = 255.0F / (cmax - cmin + 1);
            for (int i = 0; i < cmin; i++)
            {
                mLUT[i] = tmp[0];
            }
            for (int i = cmin; i < cmax; i++)
            {
                mLUT[i] = tmp[(int)((i - cmin) * k + 0.5)];
            }
            for (int i = cmax; i < mLUT.Length; i++)
            {
                mLUT[i] = tmp[255];
            }
        }

        public static Point GetCminCmax(int level, int window)
        {
            int cmin = (int)(level - window / 2.0 + 0.5);
            int cmax = (int)(level + window / 2.0 + 0.5);
            if (cmin < 0) cmin = 0;
            if (cmin > cmax) cmin = 0;
            return new Point(cmin, cmax);
        }
    }
}
