using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AMRT;

namespace ImageCapturing
{
    public partial class LinarDetection : AMRT.ShowBaseControl
    {
        public List<ImageROI> imageList;

        public LinarDetection(List<ImageROI> images)
        {
            this.ShowTitle = "Image Linear";
            InitializeComponent();
            imageList = images;
        }

        //初始化对象的信息
        public override void UserCtrlShown()
        {
            base.UserCtrlShown();
            //先将界面元素显示出来
            //....................

            string detectionAreaString = CapturePub.readCaptrueValue("LinearDetectionArea", false);
            int detectionArea = 15;
            if (!int.TryParse(detectionAreaString, out detectionArea))
            {
                detectionArea = 15;
            }

            double[] mus = new double[imageList.Count];
            double[] imgVs = new double[imageList.Count];

            for (int i = 0; i < imageList.Count; i++)
            {
                //ushort[,] imgData = imageList[i].imageData;
                //int radius = (int)((detectionArea * 10 / imageList[i].pixelSize) / 2.0F);
                //if (radius <= 0 || radius >= imgData.GetLength(0) / 2)
                //{
                //    radius = 500;
                //}
                //double v = 0;
                //int upRow = (int)(imageList[i].centerY - radius);
                //if (upRow < 0 || upRow >= imgData.GetLength(0))
                //{
                //    upRow = 0;
                //}
                //int downRow = (int)(imageList[i].centerY + radius);
                //if (downRow < 0 || downRow >= imgData.GetLength(0))
                //{
                //    downRow = imgData.GetLength(0) - 1;
                //}

                //int leftCol = (int)(imageList[i].centerX - radius);
                //if (leftCol < 0 || leftCol >= imgData.GetLength(1))
                //{
                //    leftCol = 0;
                //}
                //int rightCol = (int)(imageList[i].centerX + radius);
                //if (rightCol < 0 || rightCol >= imgData.GetLength(1))
                //{
                //    rightCol = imgData.GetLength(1) - 1;
                //}


                //int recNum = 0;
                //for (int row = upRow; row <= downRow; row++)
                //{
                //    for (int col = leftCol; col <= rightCol; col++)
                //    {
                //        v += imgData[row, col];
                //        recNum++;
                //    }
                //}
                mus[i] = imageList[i].mu;
                imgVs[i] = imageList[i].frameNum * imageList[i].ROIMeanValue;
            }

            double xMin = mus[0];
            double xMax = mus[0];
            foreach (double v in mus)
            {
                if (v < xMin)
                {
                    xMin = v;
                }
                if (v > xMax)
                {
                    xMax = v;
                }
            }

            double yMin = imgVs[0];
            double yMax = imgVs[0];
            foreach (double v in imgVs)
            {
                if (v < yMin)
                {
                    yMin = v;
                }
                if (v > yMax)
                {
                    yMax = v;
                }
            }

            int VertiNum = 10;
            int HoriNum = 10;

            double VertiLen = (int)(yMax - yMin + 1);
            double HoriLen = (int)(xMax - xMin + 1);
            double vTag = double.Parse(VertiLen.ToString()[0].ToString());
            long vScale = 1;
            for (int i = 1; i < VertiLen.ToString().Length; i++)
            {
                vScale *= 10;
            }
            vTag = (++vTag) * vScale;

            double hTag = double.Parse(HoriLen.ToString()[0].ToString());
            long hScale = 1;
            for (int i = 1; i < HoriLen.ToString().Length; i++)
            {
                hScale *= 10;
            }
            hTag = (++hTag) * hScale;

            yMin = (((long)yMin) / vScale) * vScale;
            xMin = (((long)xMin) / hScale) * hScale;

            if (hTag == 0)
            {
                hScale = 10;
                hTag = 100;
            }

            if (vTag == 0)
            {
                vScale = 100;
                vTag = 1000;
            }

            double xUnit = hTag / HoriNum;
            double yUnit = vTag / VertiNum;

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Green, 2.0F);
            Font font = new Font("Tahoma", 8.0F);
            Brush brush = new SolidBrush(Color.White);


            float stringwidth = g.MeasureString(vScale.ToString(), font).Width;
            float stringheight = g.MeasureString(vScale.ToString(), font).Height;

            float vBorder = stringheight + 35;
            float hBorder = stringwidth + 35;

            float xbmpUnit = (bmp.Width - 2 * hBorder) / HoriNum;
            float ybmpUnit = (bmp.Height - 2 * vBorder) / VertiNum;

            PointF yTop = new PointF(hBorder, 10);
            PointF originPt = new PointF(hBorder, bmp.Height - vBorder);
            PointF xRight = new PointF(bmp.Width - 10, bmp.Height - vBorder);

            g.DrawLine(pen, originPt, yTop);
            g.DrawLine(pen, originPt, xRight);

            g.DrawLine(pen, yTop, new PointF(yTop.X - 5, yTop.Y + 10));
            g.DrawLine(pen, yTop, new PointF(yTop.X + 5, yTop.Y + 10));

            g.DrawLine(pen, xRight, new PointF(xRight.X - 10, xRight.Y - 5));
            g.DrawLine(pen, xRight, new PointF(xRight.X - 10, xRight.Y + 5));

            float strleny = g.MeasureString("(average value)", font).Width;
            float strlenx = g.MeasureString("(mu)", font).Width;

            g.DrawString("(average value)", font, Brushes.Green, new PointF(hBorder - strleny - 5, 10));
            g.DrawString("(mu)", font, Brushes.Green, new PointF(bmp.Width - 10 - strlenx, bmp.Height - vBorder + 5));

            for (int i = 0; i <= VertiNum; i++)
            {
                if (i == 0)
                {
                    PointF p1 = new PointF(hBorder - 2, bmp.Height - vBorder);
                    //g.DrawLine(pen, originPt, p1);
                    float ylen = g.MeasureString(yMin.ToString(), font).Width + 1;
                    PointF strP = new PointF(hBorder - ylen, p1.Y - stringheight);
                    g.DrawString(yMin.ToString(), font, brush, strP);
                }
                else
                {
                    PointF p1 = new PointF(hBorder - 2, bmp.Height - vBorder - i * ybmpUnit);
                    PointF p2 = new PointF(hBorder, bmp.Height - vBorder - i * ybmpUnit);
                    g.DrawLine(pen, p1, p2);
                    float ylen = g.MeasureString((yMin + i * yUnit).ToString(), font).Width + 1;
                    PointF strP = new PointF(hBorder - ylen, p1.Y - stringheight);
                    g.DrawString((yMin + i * yUnit).ToString(), font, brush, strP);
                }
            }

            for (int i = 0; i <= HoriNum; i++)
            {
                if (i == 0)
                {
                    PointF p1 = new PointF(hBorder - 2, bmp.Height - vBorder);
                    //g.DrawLine(pen, originPt, p1);
                    PointF strP = new PointF(hBorder, p1.Y + 1);
                    g.DrawString(xMin.ToString(), font, brush, strP);
                }
                else
                {
                    PointF p1 = new PointF(hBorder + i * xbmpUnit, bmp.Height - vBorder - 2);
                    PointF p2 = new PointF(hBorder + i * xbmpUnit, bmp.Height - vBorder);
                    g.DrawLine(pen, p1, p2);
                    PointF strP = new PointF(p1.X, p1.Y + 1);
                    g.DrawString((xMin + i * xUnit).ToString(), font, brush, strP);
                }
            }

            List<PointF> list = new List<PointF>();

            for (int i = 0; i < mus.Length; i++)
            {
                double offmu = mus[i] - xMin;
                float offbmpmu = (float)((offmu / hTag) * (bmp.Width - 2 * hBorder));

                double offvalue = imgVs[i] - yMin;
                float offbmpvalue = (float)((offvalue / vTag) * (bmp.Height - 2 * vBorder));

                PointF p1 = new PointF(hBorder + offbmpmu, bmp.Height - vBorder - offbmpvalue);
                g.FillEllipse(Brushes.Green, new RectangleF(p1.X - 2, p1.Y - 2, 4, 4));
                list.Add(p1);

            }
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.DrawLines(pen, list.ToArray());
            g.Flush();
            g.Dispose();
            brush.Dispose();
            font.Dispose();
            pictureBox1.Image = bmp;
            pictureBox1.Refresh();
        }

    }
}
