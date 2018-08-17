using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace ImageCapturing
{

    /// <summary>
    /// 最基本的图像信息类对象，存储了图像通用的信息
    /// </summary>
    public class ImageObject : IDisposable
    {   
        /// <summary>
        /// 对象创建出来以后自动生成的一个GUID，一直都不会变，用于唯一标识一个对象
        /// </summary>
        public string imageGuid = null;
        public DateTime createTime = DateTime.Now;

        public ushort[,] ImageData = null;

        public float centerX; //单位为像素
        public float centerY; //单位为像素
        public int imageWidth
        {
            get
            {
                if(ImageData != null)
                {
                    return ImageData.GetLength(1);
                }
                else
                {
                    return 0;
                }
            }
        }
        public int imageHeight
        {
            get
            {
                if (ImageData != null)
                {
                    return ImageData.GetLength(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        //图像的物理信息
        public float pixelSize;
        public float AverageValue;
        public Bitmap BMP;

        public int imgLevel = 0;
        public int imgWindow = 0;

        public ImageObject()
        {
            imageGuid = Guid.NewGuid().ToString();
        }

        public void Dispose()
        {
            if (BMP != null)
            {
                BMP.Dispose();
            }
        }

        public unsafe void GenerateImage()
        {
            if (imgLevel == 0 && imgWindow == 0)
            {
                Histogram_Data his = new Histogram_Data();
                his.ComputeHistogram(ImageData);
                imgLevel = his.windowCenter;
                imgWindow = his.windowWidth;
            }

            Color[] LUT = null;
            HisLUT.RefreshLUT(ref LUT, null, false, imgLevel, imgWindow, 65536);

            BMP = SliceImageToBitmap24(ImageData, LUT);
        }

        public static Bitmap SliceImageToBitmap24(ushort[,] value, Color[] LUT)
        {
            if (value == null || LUT == null)
            {
                return null;
            }
            int row = value.GetLength(0);
            int col = value.GetLength(1);
            Bitmap bm = new Bitmap(col, row, PixelFormat.Format24bppRgb);
            BitmapData bmData = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height),
                ImageLockMode.WriteOnly, bm.PixelFormat);
            IntPtr bmDataScan = bmData.Scan0;
            int bmStride = bmData.Stride;
            int Offset = bmStride - bm.Width * 3;
            unsafe
            {
                byte* bmPtr = (byte*)(void*)bmDataScan;
                try
                {
                    for (int y = 0; y < row; y++)
                    {
                        for (int x = 0; x < col; x++)
                        {
                            uint v = (uint)(value[y, x] );
                            Color cl = LUT[v];
                            bmPtr[0] = cl.B;
                            bmPtr[1] = cl.G;
                            bmPtr[2] = cl.R;
                            bmPtr += 3;
                        }
                        bmPtr += Offset;
                    }
                }
                catch (System.Exception ex)
                {
                    //string Mes = ex.ToString();
                    //bm.UnlockBits(bmData);
                    ////Bitmap bm = new Bitmap(col, row, PixelFormat.Format24bppRgb);
                    //Graphics gb = Graphics.FromImage(bm);
                    //gb.DrawString("Read error", SystemFonts.DefaultFont, Brushes.Red, row / 2.0f, col / 2.0f);
                    //gb.Dispose();
                    //return bm;
                }
            }//end unsafe.

            bm.UnlockBits(bmData);
                       //bm.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\22.bmp");
            return bm;
        }

        public unsafe void SaveRawFile(string path, int headbuffersize, ushort* headbuffer)
        {
            if (ImageData == null)
            {
                return;
            }
            FileInfo finfo = new FileInfo(path);
            if (!finfo.Directory.Exists)
            {
                finfo.Directory.Create();
            }
            ushort* acq = null;
            int headsize = 0;

            if((IntPtr)headbuffer != IntPtr.Zero)
            {
                acq = (ushort*)headbuffer;
                headsize = headbuffersize;

            }
            ushort[,] imgdata = ImageData;

            fixed(ushort* p = &imgdata[0,0])
            {
                FileStream fileStream = new FileStream(path,FileMode.Create,FileAccess.Write);

                byte[] numArray = new byte[imgdata.Length * 2 + headsize];
                if(headsize > 0)
                {
                    Marshal.Copy((IntPtr)acq,numArray,0,headsize);
                }
                Marshal.Copy((IntPtr)p,numArray,headsize,numArray.Length - headsize);
                fileStream.Write(numArray,0,numArray.Length);
                fileStream.Flush();
                fileStream.Close();
            }
        }
    }   
}
 
