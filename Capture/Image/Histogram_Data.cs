using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;

namespace ImageCapturing
{
    /// 图像数据的直方图的统计
    public class Histogram_Data
    {

        /// 直方图数据——统计图像中像素值的个数,从0开始到最大像素点的数组长度
        /// 数组的索引代表像素值，值代表特定像素个数统计
        public int[] HistogramData = null;

        public ushort[,] imageData = null;

        //图像像素的最大值
        public int MaxValue;

        //图像像素的最小值
        public int MinValue;
        
        //整幅图像像素的均值
        public float AverageValue;

        public Color[] LUT;

        public int windowCenter;
        public int windowWidth;
        public bool converse = false;
        public ColorMode selectedColorMode;

        public ImageWindow[] wlModels = new ImageWindow[] { };
        public ColorMode[] colorModels = new ColorMode[] { };

        public delegate void LUTChangedDelegate(Color[] LUT);
        public event LUTChangedDelegate LUTChanged;


        public Histogram_Data()
        {
            //SetDefaultLUT();
        }
      
        /// 计算图像的直方图
        public unsafe void ComputeHistogram(ushort[,] data)
        {
            if(data == null)
            {
                imageData = data;
                HistogramData = null;
                return;
            }
            MaxValue = 65535;// rdata[0];
            MinValue = 0;// rdata[1];

            HistogramData = new int[65536];
            long sumValue = 0;
            int len = data.Length;
            fixed (ushort* p = &data[0, 0])
            {
                ushort* tmp = p;
                for (int i = 0; i < len; i++)
                {
                    sumValue += *tmp;
                    HistogramData[*tmp]++;
                    tmp++;
                }
            }
            AverageValue = (float)(sumValue / data.Length);

            long area_Points_per = (long)(data.Length * 0.05);
            int idx1 = 0;
            int idx2 = 0;
            int hcount = 0;
            for (int i = 0; i < HistogramData.Length; i++)
            {
                hcount += HistogramData[i];
                if (hcount > area_Points_per)
                {
                    idx1 = i;
                    break;
                }
            }
            hcount = 0;
            for (int i = HistogramData.Length - 1; i > 0; i--)
            {
                hcount += HistogramData[i];
                if (hcount > area_Points_per)
                {
                    idx2 = i;
                    break;
                }
            }

            windowCenter = (idx2 - idx1) / 2 + idx1;
            windowWidth =   idx2 - idx1; 
        }



       
    }
}
