using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace ImageCapturing
{
    /// <summary>
    /// 显示直方图BMP,以及窗宽窗位的变化控制
    /// </summary>
    public class Histogram_BMP
    {
        Histogram_Data histo_data;

        public Bitmap backBMP;
        public Bitmap WL_BMP;

        int histo_width = 300;
        int histo_height = 200;

        Point startPoint_ref = new Point(0,0); // 图片在控件中的起始坐标 

        /// <summary>
        /// 直方图的颜色
        /// </summary>
        private Color HistogramColor = Color.FromArgb(200, 0, 0, 255);

        /// <summary>
        /// 直方图的背景色
        /// </summary>
        private Color HistogramBackColor = Color.Transparent;

        // 调节窗宽窗位线条的颜色
        private Color WL_Color = Color.Red;

        Point breakPoint;

        public Histogram_BMP(Point p, Size size, Histogram_Data his)
        {
            startPoint_ref = p;
            histo_width = size.Width;
            histo_height = size.Height;
            histo_data = his;
        }


        /// <summary>
        /// 画直方图数据到Bitmap中
        /// </summary>
        public void GenerateBackBMP()
        {
            if (histo_data == null || histo_data.HistogramData == null)
            {
                return;
            }

            Size histoSize = new Size(histo_width,histo_height);
            backBMP = new Bitmap(histoSize.Width,histoSize.Height);
            Graphics gp = Graphics.FromImage(backBMP);

            long[] histogramData_bmp = new long[histoSize.Width];

            int delta = 65536 / histoSize.Width;
            for (int i = 0; i < histoSize.Width; i++)
            {
                for (int j = 0; j < delta; j++)
                {
                    histogramData_bmp[i] += histo_data.HistogramData[i * delta + j];
                }
            }

            long max_bmpValue = 0;
            foreach (long v in histogramData_bmp)
            {
                if (v > max_bmpValue)
                {
                    max_bmpValue = v;
                }
            }
            Point[] DrawPoint = new Point[histoSize.Width];
            for (int i = 0; i < histoSize.Width; i++)
            {
                DrawPoint[i].X = i;
                int y = (int)(histogramData_bmp[i] * histoSize.Height / max_bmpValue);
                if (y > histoSize.Height - 1)
                {
                    y = histoSize.Height - 1;
                }
                DrawPoint[i].Y = histoSize.Height - y;
            }

           
            //draw
            Point[] drawPoint = new Point[DrawPoint.Length + 2];
            for (int i = 0; i < DrawPoint.Length; i++)
            {
                drawPoint[i].X = DrawPoint[i].X;
                drawPoint[i].Y = DrawPoint[i].Y;
            }
            drawPoint[histoSize.Width].X = histoSize.Width - 1;
            drawPoint[histoSize.Width].Y = histoSize.Height - 1;
            drawPoint[histoSize.Width + 1].X = 0;
            drawPoint[histoSize.Width + 1].Y = histoSize.Height - 1;
            gp.FillPolygon(new SolidBrush(HistogramColor), drawPoint, System.Drawing.Drawing2D.FillMode.Winding);
            gp.Dispose();
        }

        public void GenerateWL()
        {
            if (histo_data == null || histo_data.HistogramData == null)
            {
                return;
            }

            WL_BMP = new Bitmap(histo_width, histo_height);
            Graphics gp = Graphics.FromImage(WL_BMP);

            Point p = HisLUT.GetCminCmax(histo_data.windowCenter, histo_data.windowWidth);
            int curMin = GetValuePosition(p.X);
            int curMax = GetValuePosition(p.Y);

            int maxX = WL_BMP.Width - 1;
            if (curMin < 0)
            {
                curMin = 0;
            }
            else if (curMin > maxX)
            {
                curMin = maxX;
            }

            if (curMax < 0)
            {
                curMax = 0;
            }
            else if (curMax > maxX)
            {
                curMax = maxX;
            }
            if (curMax < curMin)
            {
                curMax = curMin;
            }
            Pen WL_pen = new Pen(WL_Color);
            Point p_left_down = new Point(curMin, WL_BMP.Height);
            Point p_left_up = new Point(curMin, 0);
            Point p_right_down = new Point(curMax, WL_BMP.Height);
            Point p_right_up = new Point(curMax, 0);
            Point p_mid_up = new Point((curMin + curMax) / 2, 0);
            Point p_mid_down = new Point((curMin + curMax) / 2, WL_BMP.Height);
            
            gp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gp.DrawLine(WL_pen, p_left_down, p_left_up);
            gp.DrawLine(WL_pen, p_left_down, p_right_up);
            gp.DrawLine(WL_pen, p_right_down, p_right_up);
            gp.DrawLine(WL_pen, p_mid_up, p_mid_down);
            gp.Dispose();

        }

        public int GetValuePosition(int value)
        {
            return (int)(value / 65536.0F * histo_width + 0.5F);
        }

        public int GetPositionValue(int pos)
        {
            return (int)(pos / (float)histo_width * 65536.0F + 0.5F);
        }

        public void MouseDown(PictureBox picturebox, MouseEventArgs e)
        {
            if (histo_data == null || histo_data.HistogramData == null)
            {
                return;
            }

            Point p = new Point(e.X - startPoint_ref.X, e.Y - startPoint_ref.Y);
            if (p.Y > histo_height)
            {
                return;
            }

            Point p_minMax = HisLUT.GetCminCmax(histo_data.windowCenter, histo_data.windowWidth);
            int curMin = GetValuePosition(p_minMax.X);
            int curMax = GetValuePosition(p_minMax.Y);

            if (Math.Abs(p.X - curMin) < 5 && p.Y < histo_height)
            {
                breakPoint.Y = 1;
            }
            else if (((p.X - curMin) > 5) && ((curMax - p.X) > 5) && p.Y < histo_height)
            {
                breakPoint.Y = 2;
            }
            else if (Math.Abs(p.X - curMax) < 5 && p.Y < histo_height)
            {
                breakPoint.Y = 3;
            }
            else
            {
                breakPoint.Y = 0;
            }
            breakPoint.X = p.X;
        }

        public void MouseMove(PictureBox picturebox, MouseEventArgs e)
        {
            if (histo_data == null || histo_data.HistogramData == null )
            {
                return;
            }

            Point p = new Point(e.X - startPoint_ref.X, e.Y - startPoint_ref.Y);
            if (p.Y > histo_height)
            {
                return;
            }

            Point p_minMax = HisLUT.GetCminCmax(histo_data.windowCenter, histo_data.windowWidth);
            int curMin = GetValuePosition(p_minMax.X);
            int curMax = GetValuePosition(p_minMax.Y);

            int error = 3;
            if (e.Button != MouseButtons.Left)
            {
                if (Math.Abs(p.X - curMin) < error && p.Y < histo_height)
                {
                    picturebox.Cursor = Cursors.SizeWE;
                }
                else if (Math.Abs(p.X - curMax) < error && p.Y < histo_height)
                {
                    picturebox.Cursor = Cursors.SizeWE;//TCursor.curRight;
                }
                else if (((p.X - curMin) >= error) && ((curMax - p.X) >= error) && p.Y < histo_height)
                {
                    picturebox.Cursor = Cursors.Hand;
                }
                else 
                {
                    picturebox.Cursor = Cursors.Default;
                }
            }
            else if (breakPoint.Y != 0 && Math.Abs(p.X - breakPoint.X) > 5 && p.Y < histo_height)
            {
                int oldMinInHist = curMin;
                int oldMaxInHist = curMax;

                if (breakPoint.Y == 1)
                {
                    curMin = p.X;
                    breakPoint.X = p.X;
                }
                else if (breakPoint.Y == 3)
                {
                    curMax = e.X; 
                    breakPoint.X = p.X;
                }
                else if (breakPoint.Y == 2)
                {
                    curMax += (p.X - breakPoint.X);   //加上偏移量
                    curMin += (p.X - breakPoint.X);
                    if (curMin < 0 && p.X < (0 + curMax) / 2)
                    {
                        curMax -= (0 - curMin);
                    }
                    if (curMax > histo_width && p.X > (curMin + histo_width - 0 - 1) / 2)
                    {
                        curMin += (curMax - histo_width);
                    }
                    breakPoint.X = p.X;
                }

                if (Math.Abs(oldMinInHist - curMin) < 1 && Math.Abs(oldMaxInHist - curMax) < 1)
                {
                    return;
                }

                int minV = GetPositionValue(curMin);
                int maxV = GetPositionValue(curMax);
                if (minV > maxV)
                {
                    int temp = maxV;
                    maxV = minV;
                    minV = temp;
                }
                histo_data.SetCenterWindow((maxV - minV) / 2 + minV, maxV - minV);
                picturebox.Image = new Bitmap(histo_width,histo_height);
                Graphics g = Graphics.FromImage(picturebox.Image);
                GenerateWL();
                g.DrawImage(backBMP, new Point(0, 0));
                g.DrawImage(WL_BMP, new Point(0, 0));
                picturebox.Refresh();



                //hist1.RefreshLUT();
                //if (LUTChanged != null)
                //{
                //    LUTChanged(0, new Point(hist1.windowCenter, hist1.windowWidth), hist1.LUT);
                //}
            }
        }

        public void MouseUp(PictureBox picturebox, MouseEventArgs e)
        {

            if (histo_data == null || histo_data.HistogramData == null)
            {
                return;
            }

           
        }

        public void RefreshWL(PictureBox picturebox)
        {
            picturebox.Image = new Bitmap(histo_width, histo_height);
            Graphics g = Graphics.FromImage(picturebox.Image);
            GenerateWL();
            g.DrawImage(backBMP, new Point(0, 0));
            g.DrawImage(WL_BMP, new Point(0, 0));
            picturebox.Refresh();
        }
    }
}
