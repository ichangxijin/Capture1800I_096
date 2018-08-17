using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ImageCapturing;

namespace ImageCapturing
{
    /// <summary>
    /// 用于直方图的统计显示和控制
    /// </summary>
    public class AverageHistogram
    {
        #region Class Members

        /// <summary>
        /// 直方图数据的长度
        /// </summary>
        private int lenValue;
        public int LenValue
        {
            get { return lenValue; }
            //set { lenValue = value; }
        }

        /// <summary>
        /// 图像中像素值最多的像素的个数
        /// </summary>
        public int numMostValue;

        /// <summary>
        /// 直方图数据——统计图像中像素值的个数
        /// </summary>
        private int[] histogramData = null;

        private int maxValue;
        public int MaxValue
        {
            get { return maxValue; }
            //set { maxValue = value; }
        }

        private int minValue;
        public int MinValue
        {
            get { return minValue; }
            //set { maxValue = value; }
        }

        private int winCenter;
        public int WinCenter
        {
            get { return winCenter; }
        }

        private int winWidth;
        public int WinWidth
        {
            get { return winWidth; }
        }

        private Size DrawSize = Size.Empty;
        private Point[] DrawPoint = null;

        #endregion

        public AverageHistogram(ushort[,] data)
        {
            ComputeHistogram(data);
        }

        #region Functions

        /// <summary>
        /// 计算图像的直方图
        /// </summary>
        private void ComputeHistogram(ushort[,] data)
        {
            if (data == null)
            {
                lenValue = 0;
                histogramData = null;
                return;
            }

            int[] rdata = LunImage.FindMaxAndMin(data);
            maxValue = rdata[0];
            minValue = rdata[1];

            lenValue = maxValue - minValue + 1;
            histogramData = new int[lenValue];

            foreach (int e in data)
            {
                histogramData[e - minValue]++;
            }

            //均值法
            int all = data.Length;
            all = all - histogramData[0] - histogramData[lenValue - 1]; //避免直方图首尾值太大影响显示比例.
            histogramData[0] = 0;
            histogramData[maxValue - minValue] = 0;

            const float factor = 2;//修正参数            
            numMostValue = (int)(all * factor / lenValue + 0.5);
            ComputeDefaultLW();

        }

        public int GetValuePosition(int value)
        {
            return (int)((value - minValue) * DrawSize.Width / (float)lenValue + 0.5);
        }

        public int GetPositionValue(int pos)
        {
            return (int)(pos * lenValue / DrawSize.Width + minValue + 0.5);
        }

        /// <summary>
        /// 画直方图数据到Bitmap中
        /// </summary>
        public Bitmap DrawHistBmp(Size ShowSize, Color DrawColor, Color BackColor, Color axisColor)
        {
            DrawSize = ShowSize;
            if (numMostValue < 1 || lenValue <= 1) return null;

            Bitmap histBmp = TypeConvert.ConvertToBitmap(ShowSize, BackColor);
            Graphics gp = Graphics.FromImage(histBmp);

            float k_width = (float)(ShowSize.Width) / lenValue;
            DrawPoint = new Point[ShowSize.Width];
            float idx = 0;
            int sn = 0;
            int max = 0;
            float unit = lenValue / (float)ShowSize.Width;
            for (int i = 0; i < ShowSize.Width; i++)
            {
                int y = 0;
                while (idx < i + 1 && sn < lenValue)
                {
                    idx += k_width;
                    y += histogramData[sn++];
                }
                DrawPoint[i].Y = y;
                if (max < y)
                {
                    max = y;
                }
            }

            //float k_height = (float)(ShowSize.Height) / max * 4;
            float k_height = (float)(ShowSize.Height / (float)numMostValue);
            for (int i = 0; i < ShowSize.Width; i++)
            {
                DrawPoint[i].X = i;
                int y = (int)(DrawPoint[i].Y / unit * k_height + 0.5);
                if (y > ShowSize.Height - 1)
                {
                    y = ShowSize.Height - 1;
                }
                DrawPoint[i].Y = ShowSize.Height - y;
            }

            //draw
            Point[] drawPoint = new Point[DrawPoint.Length + 2];
            for (int i = 0; i < DrawPoint.Length; i++)
            {
                drawPoint[i].X = DrawPoint[i].X;
                drawPoint[i].Y = DrawPoint[i].Y;
            }
            drawPoint[ShowSize.Width].X = ShowSize.Width - 1;
            drawPoint[ShowSize.Width].Y = ShowSize.Height - 1;
            drawPoint[ShowSize.Width + 1].X = 0;
            drawPoint[ShowSize.Width + 1].Y = ShowSize.Height - 1;
            gp.FillPolygon(new SolidBrush(DrawColor), drawPoint, System.Drawing.Drawing2D.FillMode.Winding);

            //Axis
            Point p0 = new Point(0, ShowSize.Height);
            Point px = new Point(ShowSize.Width, ShowSize.Height);
            Point py = new Point(0, 0);
            Pen pen = new Pen(axisColor);
            gp.DrawLine(pen, p0, px);
            gp.DrawLine(pen, p0, py);
            gp.Dispose();

            return histBmp;
        }

        /// <summary>
        /// 画直方图数据到Bitmap中
        /// </summary>
        public Bitmap RefreshBackGroundImage(Size ShowSize, Color DrawColor, Color BackColor, Color axisColor, int cmin, int cmax, int OffX)
        {
            if (numMostValue <= 1 || lenValue <= 1) return null;

            Bitmap histBmp = TypeConvert.ConvertToBitmap(ShowSize, BackColor);
            Graphics gp = Graphics.FromImage(histBmp);

            int c1 = GetValuePosition(cmin);
            int c2 = GetValuePosition(cmax);
            Point[] tmp = new Point[c1 + 2];
            for (int i = 0; i < c1; i++)
            {
                tmp[i].X = DrawPoint[i].X;
                tmp[i].Y = DrawPoint[i].Y;
            }
            tmp[c1].X = c1 - 1;
            tmp[c1].Y = ShowSize.Height - 1;
            tmp[c1 + 1].X = 0;
            tmp[c1 + 1].Y = ShowSize.Height - 1;
            gp.FillPolygon(new SolidBrush(Color.WhiteSmoke), tmp, System.Drawing.Drawing2D.FillMode.Winding);

            tmp = new Point[c2 - c1 + 2];
            for (int i = c1; i < c2; i++)
            {
                tmp[i - c1].X = DrawPoint[i].X;
                tmp[i - c1].Y = DrawPoint[i].Y;
            }
            tmp[c2 - c1].X = c2 - 1;
            tmp[c2 - c1].Y = ShowSize.Height - 1;
            tmp[c2 - c1 + 1].X = c1;
            tmp[c2 - c1 + 1].Y = ShowSize.Height - 1;
            gp.FillPolygon(new SolidBrush(DrawColor), tmp, System.Drawing.Drawing2D.FillMode.Winding);

            if (DrawSize.Width > c2)
            {
                tmp = new Point[DrawSize.Width - c2 + 1];
                for (int i = c2 + 1; i < DrawSize.Width; i++)
                {
                    tmp[i - c2 - 1].X = DrawPoint[i].X;
                    tmp[i - c2 - 1].Y = DrawPoint[i].Y;
                }
                tmp[DrawSize.Width - c2 - 1].X = ShowSize.Width - 1;
                tmp[DrawSize.Width - c2 - 1].Y = ShowSize.Height - 1;
                tmp[DrawSize.Width - c2].X = c2;
                tmp[DrawSize.Width - c2].Y = ShowSize.Height - 1;
                gp.FillPolygon(new SolidBrush(Color.WhiteSmoke), tmp, System.Drawing.Drawing2D.FillMode.Winding);

            }

            //Axis
            Point p0 = new Point(0, ShowSize.Height);
            Point px = new Point(ShowSize.Width, ShowSize.Height);
            Point py = new Point(0, 0);
            Pen pen = new Pen(axisColor);
            gp.DrawLine(pen, p0, px);
            gp.DrawLine(pen, p0, py);
            gp.Dispose();

            return histBmp;
        }

        private void ComputeDefaultLW()
        {
            winCenter = histogramData.Length / 2 + minValue;
            winWidth = histogramData.Length;
        }
        #endregion
    }
}
