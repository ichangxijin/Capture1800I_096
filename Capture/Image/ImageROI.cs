using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace ImageCapturing
{

    /// <summary>
    /// 提供图像增强和ROI处理
    /// </summary>
    public class ImageROI : ImageObject
    {

        public bool getClahe = true; //获得clahe后的图像，但是data不变
        public string strength = "";
        public string strengthPara = "";

        public List<PointF> ROIPoints = new List<PointF>();

        //for average capturing
        public int mu = 0;
        public int frameNum = 0;


        //统计区域以及统计信息的变量
        public List<Point> boundaryPoint; //Lst Add 2011.12.15 添加图像边界点变量
        public List<Point> noisePoint;//噪点？
        public double ROIMeanValue;//EPID平板的灰度值最大值为2^16，其实采用ulong就足够了
        public double ROIVar;//标准差
        public UInt32 ROIDiffNumber;//噪点个数？
        public float SNR = 0.0F;
        //public int ROIAverageValue;
        public string rectType = "toolStripMenuItemDefault";
        //初始检测到得边界
        public Rectangle BoundaryRect;

        //ROI边界
        public Rectangle ROIRectangle;



        public ushort[,] ImageData
        {
            get { return imageData; }
            set
            {
                imageData = value;
                if (imageData == null)
                {
                    BMP = new Bitmap(512, 512);
                }
                else
                {
                    ushort[,] imageDataTemp = (ushort[,])imageData.Clone();
                    imageWidth = imageData.GetLength(1);
                    imageHeight = imageData.GetLength(0);

                    if (getClahe)
                    {
                        imageDataTemp = ClaheProcess.CLAHE(strength + "\\" + strengthPara + "$", imageDataTemp);
                    }
                    if (showROI)
                    {
                        if (ROIPoints.Count <= 0)
                        {
                            List<PointF> points_pixel = ComputeDefaultROIPoints(imageDataTemp);
                            ROIPoints = GetROIPoints_Phy(points_pixel,pixelSize);
                        }
                        List<PointF> points_Pixel = GetROIPoints_Pixel(ROIPoints,pixelSize);
                        imageDataTemp = GetROIImageData(imageDataTemp, points_Pixel, Color.Red);
                    }
                    GenerateImageLUT(imageDataTemp);
                    BMP = TypeConvert.SliceImageToBitmap24(imageDataTemp, LUT, minValue);
                    smallBMP = TypeConvert.SliceImageToSmallBitmap(imageDataTemp, LUT, minValue);
                    GC.Collect();
                }
            }
        }

        public override ushort[,] GetImageData_Processed()
        {
            ushort[,] imageDataTemp = (ushort[,])imageData.Clone();
            if (getClahe)
            {
                imageDataTemp = ClaheProcess.CLAHE(strength + "\\" + strengthPara + "$", imageDataTemp);
            }
            if (showROI)
            {
                if (ROIPoints.Count <= 0)
                {
                    List<PointF> points_pixel = ComputeDefaultROIPoints(imageDataTemp);
                    ROIPoints = GetROIPoints_Phy(points_pixel, pixelSize);
                }
                List<PointF> points_Pixel = GetROIPoints_Pixel(ROIPoints, pixelSize);
                imageDataTemp = GetROIImageData(imageDataTemp, points_Pixel, Color.Red);
                GC.Collect();
            }
            return imageDataTemp;
        }

        private bool showROI = false;
        public bool ShowROI
        {
            get { return showROI; }
            set 
            {
                if (showROI != value)
                {
                    showROI = value;
                    ImageData = imageData;
                }
            }
        }

        public static ImageROI GenerateImageROI(ImageObject image)
        {
            ImageROI imgROI = new ImageROI();
            imgROI.imageGuid = Guid.NewGuid().ToString();
            imgROI.sourceID = image.sourceID;
            imgROI.centerX = image.centerX;
            imgROI.centerY = image.centerY;
            imgROI.imageWidth = image.imageWidth;
            imgROI.imageHeight = image.imageHeight;
            imgROI.minValue = image.minValue;
            imgROI.maxValue = image.maxValue;
            imgROI.averageValue = image.averageValue;
            imgROI.pixelSize = image.pixelSize;
            imgROI.level = image.level;
            imgROI.window = image.window;
            imgROI.converse = image.converse;
            imgROI.colorModeName = image.colorModeName;
            if (image.LUT != null)
            {
                imgROI.LUT = (Color[])image.LUT.Clone();
            }
            if (image.imageData != null)
            {
                imgROI.imageData = (ushort[,])image.imageData.Clone();
            }
            if (image.BMP != null)
            {
                imgROI.BMP = (Bitmap)image.BMP.Clone();
            }
            return imgROI;
        }

        public override ImageObject Clone()
        {
            ImageROI cBmp = GenerateImageROI(this);
            cBmp.getClahe = this.getClahe; //获得clahe后的图像，但是data不变
            cBmp.strength = this.strength;
            cBmp.strengthPara = this.strengthPara;
            cBmp.showROI = this.showROI;
            PointF[] ps = new PointF[this.ROIPoints.Count];
            this.ROIPoints.CopyTo(ps, 0);
            cBmp.ROIPoints = new List<PointF>(ps);
            return cBmp;
        }

        public void SetLUT(Color[] lut)
        {
            if (imageData == null)
            {
                return;
            }

            LUT = lut;
            if (LUT == null)
            {
                level = window = -1;
                return;
            }
            ushort[,] imgDataTemp = GetImageData_Processed();
            BMP = TypeConvert.SliceImageToBitmap24(imgDataTemp, LUT, minValue);
            smallBMP = TypeConvert.SliceImageToSmallBitmap(imgDataTemp,LUT,minValue);
        }

        public void SetStrength(string strengthName,string strengthParameter)
        {
            strength = strengthName;
            strengthPara = strengthParameter;
            if (strength != "" && strength != "No Process")
            {
                getClahe = true;
            }
            else 
            {
                getClahe = false;
            }
            ImageData = imageData;
        }

        public override void SaveasDicomFile(string path)
        {
           
        }


        /// 静态函数
        /// 
        private static List<PointF> ComputeDefaultROIPoints(ushort[,] imgData)
        {
            ushort[,] imagedataTemp = (ushort[,])imgData.Clone();
            int[] rdata = LunImage.FindMaxAndMin(imagedataTemp);
            int maxValue = rdata[0];
            int minValue = rdata[1];
            int lenValue = maxValue - minValue + 1;
            int[] histogramData = new int[lenValue];
            foreach (int da in imgData)
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
                return new List<PointF>();
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

            int level = thresholdValue / 2;
            int window = thresholdValue / 5;

            Histogram his = new Histogram(imagedataTemp);
            int HFMax = level + window / 2;
            int HFMin = level - window / 2;
            if (HFMax < 0 || HFMin < 0)
            {
                level = his.WinCenter;
                window = his.WinWidth;
            }
            else
            {
                if (HFMax > his.MaxValue || HFMin > his.MaxValue)
                {
                    level = his.WinCenter;
                    window = his.WinWidth;
                }
            }
            int maxvalue = his.LenValue < 256 ? 256 : his.LenValue;
            maxValue = maxvalue;
            minValue = his.MinValue;
            Color[] LUT = HisLUT.RefreshLUT(null, false, level, window, maxvalue, minValue);
            Bitmap BMP = TypeConvert.SliceImageToBitmap24(imagedataTemp, LUT, minValue);

            float x1 = 0;
            float y1 = 0;
            float x2 = BMP.Width - 1;
            float y2 = BMP.Height - 1;
            BoundaryTracker bt = new BoundaryTracker();
            bt.GetSerializedBoundary(BMP, LUT[0], new Rectangle((int)x1, (int)y1, (int)(x2 - x1 + 1), (int)(y2 - y1 + 1)), false);
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
                temp.Add(new PointF(0, BMP.Height - 1));
                temp.Add(new PointF(BMP.Width - 1, BMP.Height - 1));
                temp.Add(new PointF(BMP.Width - 1, 0));

            }
            return temp;
        }

        private static ushort[,] GetROIImageData(ushort[,] imgData, List<PointF> points_ROI, Color fillCL)
        {
            if (points_ROI == null || points_ROI.Count <= 0)
            {
                return imgData;
            }
            int[] pt = LunImage.FindMaxAndMin(imgData);
            int minV = pt[1];
            int H = imgData.GetLength(0);
            int W = imgData.GetLength(1);

            Bitmap bm = new Bitmap(W, H, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bm);
            Brush brush = new SolidBrush(fillCL);//Color.Red);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.FillPolygon(brush, points_ROI.ToArray(), System.Drawing.Drawing2D.FillMode.Alternate);
            g.Dispose();

            BitmapData bmpData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadWrite, bm.PixelFormat);
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
                                imgData[y, x] = (ushort)minV;
                            }
                            bmPtr += 3;
                        }
                    }
                }
                catch
                {
                    bm.UnlockBits(bmpData);
                }
            }
            bm.UnlockBits(bmpData);
            return imgData;
        }

        public static List<PointF> GetROIPoints_Pixel(List<PointF> ROIPoints, float pixelSize)
        {
            List<PointF> temp = new List<PointF>();
            for (int i = 0; i < ROIPoints.Count; i++)
            {
                float x = (float)(ROIPoints[i].X / pixelSize);
                float y = (float)(ROIPoints[i].Y / pixelSize);
                temp.Add(new PointF(x, y));
            }
            return temp;
        }

        public static List<PointF> GetROIPoints_Phy(List<PointF> ROIPoints, float pixelSize)
        {
            for (int i = 0; i < ROIPoints.Count; i++)
            {
                float x = ((float)ROIPoints[i].X * (float)pixelSize);
                float y = ((float)ROIPoints[i].Y * (float)pixelSize);
                ROIPoints[i] = (new PointF(x, y));
            }
            return ROIPoints;
        }
    }   
}
 
