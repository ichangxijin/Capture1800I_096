using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using ImageCapturing;


namespace ImageCapturing
{
    public class AverageImageObjectBase : IDisposable, IComparable
    {
        public string imageGuid = null;  //对象创建出来以后自动生成的一个GUID，一直都不会变，用于唯一标识一个对象

        public long ID = long.MinValue; //如果保存在数据库中，则为图像对象在数据库中的ID，否则ID为long的无穷小
        public float centerX;
        public float centerY;
        public float pixelSize;
        public bool converse;
        public int level = -1;
        public int window = -1;
        public string colorModeName;
        public bool getSmallBMP = false;
        public ushort[,] imageData;//2010/08/18 保存原始图，避免失真.数据库中除了Data外，其他都是Drr层面的值
        public ushort[,] ImageData
        {
            get { return imageData; }
            set
            {
                imageData = value;
                if (imageData != null)
                {
                    if (LUT == null)
                    {
                        LUT = GetImageDataLut(imageData);
                    }
                    if (!getSmallBMP)
                    {
                        BMP = TypeConvert.SliceImageToBitmap24(imageData, LUT, minValue);
                    }
                    else
                    {
                        BMP = TypeConvert.SliceImageToSmallBitmap(imageData, LUT, minValue);
                    }
                    nobmp = false;
                }
                else
                {
                    BMP = new Bitmap(512, 512);
                    nobmp = true;
                }
            }
        }

        public int nFrame;
        public int imageWidth;
        public int imageHeight;
        public Bitmap BMP;

        public Color[] LUT;

        public ushort[,] imageDataROI = null;
        private bool showROI = false;
        public bool ShowROI
        {
            get { return showROI; }
            set
            {
                showROI = value;
                if (showROI)
                {
                    if (ROIPoints.Count <= 0)
                    {
                        ComputeDefaultROIPoints();
                    }
                    AverageImageObjectBase cbTemp = Clone();
                    RefreshROI(ref cbTemp, Color.Red);
                    imageDataROI = cbTemp.imageData;
                    BMP = cbTemp.BMP;
                }
                else
                {
                    ImageData = imageData;
                }
            }
        }

        public ushort[,] DisplayData
        {
            get
            {
                return showROI ? imageDataROI : imageData;
            }
        }

        private void RefreshROI(ref AverageImageObjectBase cImage, Color fillCL)
        {
            if (cImage == null || cImage.ROIPoints == null || cImage.ROIPoints.Count <= 0)
            {
                return;
            }
            int[] pt = LunImage.FindMaxAndMin(cImage.imageData);
            int minV = pt[1];
            List<PointF> temp = GetROIPoint(cImage);

            Bitmap bm = (Bitmap)(cImage.BMP.Clone());
            Graphics g = Graphics.FromImage(bm);
            Brush brush = new SolidBrush(fillCL);//Color.Red);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.FillPolygon(brush, temp.ToArray(), System.Drawing.Drawing2D.FillMode.Alternate);
            g.Dispose();

            Bitmap btpb = (Bitmap)(cImage.BMP);
            int H = cImage.ImageData.GetLength(0);
            int W = cImage.ImageData.GetLength(1);
            System.Drawing.Imaging.BitmapData bmpData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height),
            System.Drawing.Imaging.ImageLockMode.ReadWrite, bm.PixelFormat);
            System.IntPtr bmDataScan = bmpData.Scan0;
            int bmStride = bmpData.Stride;
            int Offset = bmStride - bm.Width * 3;
            unsafe
            {
                try
                {
                    int clR = fillCL.R;
                    int clG = fillCL.G;
                    int clB = fillCL.B;
                    byte* bmPtr = (byte*)(void*)bmDataScan;
                    for (int y = 0; y < H; y++)
                    {
                        for (int x = 0; x < W; x++)
                        {
                            if (!(bmPtr[0] == clB && bmPtr[1] == clG && bmPtr[2] == clR))
                            {
                                cImage.imageData[y, x] = (ushort)minV;
                            }
                            bmPtr += 3;
                        }
                    }
                }
                catch
                {
                    bm.UnlockBits(bmpData);
                    return;
                }
            }
            bm.UnlockBits(bmpData);

            cImage.BMP = TypeConvert.SliceImageToBitmap24(cImage.imageData, cImage.LUT, cImage.minValue);
            //histogramControl1.RefreshHistogram(cImage.imageData);
        }

        public int minValue = 0;

        public int maxValue = 0;

        public int medianValue = 0;

        /// <summary>
        /// BMP是否为空图
        /// </summary>
        public bool nobmp = true;

        public bool isOverlay = false; //此图是否为Overlay

        public float overlayValue = 0;

        public string imageAGuid;

        public string imageBGuid;

        public List<PointF> ROIPoints = new List<PointF>();

        public DateTime createTime = DateTime.Now;
        public AverageImageObjectBase()
        {
            imageGuid = Guid.NewGuid().ToString();
        }

        public void AppendImageData(ushort[,] data, int Value)
        {
            long Sum = 0;
            long MedianValue = 0;
            /*foreach (ushort v in data)
            {
                Sum += v;
            }
            MedianValue = Sum / data.Length;*///原本是取所有像素值的平均值，现在要改为取中间20*20的均值
            for (int i = data.GetLength(0) / 2 - 10; i < data.GetLength(0) / 2 + 10;i++ )
            {
                for (int j = data.GetLength(1) / 2 - 10; j < data.GetLength(1) / 2 + 10;j++ )
                {
                    Sum += data[i, j];
                }
            }
            MedianValue = Sum / 400;

            if (MedianValue > 10000)
            {
                if (imageData == null)
                {
                    nFrame++;
                    imageData = (ushort[,])data.Clone();
                }
                else
                {
                    nFrame++;
                    for (int row = 0; row < data.GetLength(0); row++)
                    {
                        for (int col = 0; col < data.GetLength(1); col++)
                        {

                            int sum = imageData[row, col] + data[row, col];
                            imageData[row, col] = (ushort)(sum / 2);
                        }
                    }
                }
                //Sum = 0;
                //foreach (ushort v in imageData)
                //{
                //    Sum += v;
                //}
                //medianValue = (int)(Sum / imageData.Length);

            }

        }



        private void ComputeDefaultROIPoints()
        {
            AverageImageObjectBase cbTemp = Clone();
            ushort[,] imagedataTemp = cbTemp.imageData;
            int[] rdata = LunImage.FindMaxAndMin(imagedataTemp);
            int maxValue = rdata[0];
            int minValue = rdata[1];
            int lenValue = maxValue - minValue + 1;
            int[] histogramData = new int[lenValue];
            foreach (int da in cbTemp.ImageData)
            {
                histogramData[da - minValue]++;
            }

            double sum = 0;
            double csum = 0.0;
            int n = 0;
            int thresholdValue = 0;
            for (int k = 0; k < lenValue; k++)
            {
                sum += (double)k * (double)histogramData[k];    /* x*f(x) 质量矩*/
                n += histogramData[k];                                         /*  f(x)    质量    */
            }

            if (n <= 0)
            {
                // if n has no value, there is problems...
                return;
            }
            // do the otsu global thresholding method
            double fmax = -1.0;
            int n1 = 0;
            int n2 = 0;
            double m1, m2 = 0;
            for (int k = 0; k < lenValue; k++)
            {
                n1 += histogramData[k];
                if (n1 <= 0) { continue; }
                n2 = n - n1;
                if (n2 == 0) { break; }
                csum += (double)k * histogramData[k];
                m1 = csum / n1;
                m2 = (sum - csum) / n2;
                double sb = (double)n1 * (double)n2 * (m1 - m2) * (m1 - m2);
                /* bbg: note: can be optimized. */
                if (sb > fmax)
                {
                    fmax = sb;
                    thresholdValue = k;
                }
            }
            for (int i = 0; i < imagedataTemp.GetLength(0); i++)
            {
                for (int j = 0; j < imagedataTemp.GetLength(1); j++)
                {
                    if (imagedataTemp[i, j] < thresholdValue)
                    {
                        imagedataTemp[i, j] = 0;
                    }
                    else
                        imagedataTemp[i, j] = (ushort)thresholdValue;
                }
            }
            cbTemp.SetLUT(null);
            cbTemp.level = thresholdValue / 2;
            cbTemp.window = thresholdValue / 5;
            cbTemp.ImageData = imagedataTemp;

            float x1 = 0;
            float y1 = 0;
            float x2 = BMP.Width - 1;
            float y2 = BMP.Height - 1;
            BoundaryTracker bt = new BoundaryTracker();
            bt.GetSerializedBoundary(cbTemp.BMP, LUT[0], new Rectangle((int)x1, (int)y1, (int)(x2 - x1 + 1), (int)(y2 - y1 + 1)), false);
            List<PointF> temp = new List<PointF>();
            if (bt.MaxPointIdx != -1)
            {
                PointF[] tmp = bt.CL[bt.MaxPointIdx];
                if (tmp.Length > 3)
                {
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        temp.Add(tmp[i]);
                    }
                }
            }
            else
            {
                temp.Add(new PointF(0, 0));
                temp.Add(new PointF(0, cbTemp.BMP.Height - 1));
                temp.Add(new PointF(cbTemp.BMP.Width - 1, cbTemp.BMP.Height - 1));
                temp.Add(new PointF(cbTemp.BMP.Width - 1, 0));

            }
            for (int i = 0; i < temp.Count; i++)
            {
                float x = ((float)temp[i].X * (float)pixelSize);
                float y = ((float)temp[i].Y * (float)pixelSize);
                temp[i] = (new PointF(x, y));
            }
            ROIPoints = temp;
            //ImageObjectBase tempcb = cb.Clone();
            //RefreshROI(ref tempcb, Color.Red);
            //cb.showROI = true;
            //cb.BMP = tempcb.BMP;
            //loadImagebyJacbi(pictureBox1, cb.BMP, 1);
        }

        public AverageImageObjectBase Clone()
        {
            AverageImageObjectBase cBmp = new AverageImageObjectBase();
            if (BMP != null) cBmp.BMP = (Bitmap)(BMP.Clone());
            cBmp.ID = ID;
            cBmp.centerX = centerX;
            cBmp.centerY = centerY;
            cBmp.level = level;
            cBmp.window = window;
            cBmp.nobmp = nobmp;
            cBmp.minValue = minValue;
            cBmp.LUT = LUT;
            cBmp.converse = converse;
            cBmp.colorModeName = colorModeName;
            cBmp.ROIPoints = ROIPoints;
            cBmp.pixelSize = pixelSize;
            if (imageData != null) cBmp.imageData = (ushort[,])imageData.Clone();
            return cBmp;
        }

        public void SetLUT(Color[] lut)
        {
            if (nobmp)
            {
                return;
            }

            LUT = lut;
            if (LUT == null || BMP == null)
            {
                level = window = -1;
                return;
            }
            BMP = TypeConvert.SliceImageToBitmap24(DisplayData, LUT, minValue);
        }

        /// <summary>
        /// 是否选中,即存到数据库中没有, capture 模块使用的字段
        /// </summary>
        public bool IsSelect = false;
        public void Dispose()
        {
            imageData = null;
        }

        /// <summary>
        /// 获得初始的Lut
        /// </summary>
        private Color[] GetImageDataLut(ushort[,] data)
        {
            //int maxValueLimit = 65536;

            AverageHistogram his = new AverageHistogram(data);
            int HFMax = level + window / 2;
            int HFMin = level - window / 2;
            if (HFMax < 0 || HFMin < 0)
            {
                //level = his.winWidth / 2 + his.MinValue;
                //window = his.LenValue;
                level = his.WinCenter;
                window = his.WinWidth;
            }
            else
            {
                if (HFMax > his.MaxValue || HFMin > his.MaxValue)
                {
                    //level = his.LenValue / 2 + his.MinValue;
                    //window = his.LenValue;
                    level = his.WinCenter;
                    window = his.WinWidth;
                }
            }
            int maxvalue = his.LenValue < 256 ? 256 : his.LenValue;
            maxValue = maxvalue;
            minValue = his.MinValue;
            Color[] colorLUT = null;
            ImageCapturing.ColorMode[] colorModels = HistogramControl.ReadLUTColorModel();
            for (int i = 0; i < colorModels.Length; i++)
            {
                if (colorModels[i].Name == colorModeName)
                {
                    colorLUT = colorModels[i].colorLUT;
                    break;
                }
            }
            return HisLUT.RefreshLUT(colorLUT, converse, level, window, maxvalue, minValue);
        }


        public static List<PointF> GetROIPoint(AverageImageObjectBase cImage)
        {
            List<PointF> temp = new List<PointF>();
            for (int i = 0; i < cImage.ROIPoints.Count; i++)
            {
                float x = (float)mmToPixel(cImage.ROIPoints[i].X, 1.0 / (double)cImage.pixelSize);
                float y = (float)mmToPixel(cImage.ROIPoints[i].Y, 1.0 / (double)cImage.pixelSize);
                temp.Add(new PointF(x, y));
            }
            return temp;
        }


        public static double mmToPixel(double mm, double scl)
        {
            return scl * mm;
        }

        public int CompareTo(object obj)
        {
            if (obj is AverageImageObjectBase)
            {
                AverageImageObjectBase other = (AverageImageObjectBase)obj;
                return this.medianValue.CompareTo(other.medianValue);
            }
            else
            {
                throw new ArgumentException("object is not a ImageObjectBase");
            }
        }
    }
}

