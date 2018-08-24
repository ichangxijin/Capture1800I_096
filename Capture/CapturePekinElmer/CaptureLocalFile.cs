using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace ImageCapturing
{
    public class CaptureLocalFile : CaptureBase
    {
        public CaptureLocalFile()
            : base()
        {
            InitParam();
        }

        /// <summary>
        /// Panel抓取图像的行数
        /// </summary>
        private uint imageRows;
        /// <summary>
        /// Panel抓取图像的列数
        /// </summary>
        private uint imageColumns;
        private ImageObject img = new ImageObject();
        public string[] Files;

        protected override void InitParam()
        {
            ReadSetupConfig();
            SetupAngle = 0;
        }

        public override void Start()
        {
            base.Start();
        }


        private void ReadFile(string file)
        {            
            //TiffReader tiff = new TiffReader();
            //IOD_RI RI = new IOD_RI();
            HisObject His = new HisObject();
            //IOD_RD RD = new IOD_RD();
            ImageObject img = new ImageObject();

            //if (tiff.ReadFile(file))
            //{
            //    imageRows = (uint)tiff.ImageHegiht;
            //    imageColumns = (uint)tiff.ImageWidth;
            //    RefreshScale();

            //    img.pixelSize = pixelSize;
            //    img.centerX = imageCenterX;
            //    img.centerY = imageCenterY;
            //    ushort[,] image = new ushort[tiff.ImageHegiht, tiff.ImageWidth];
            //    int sn = 0;
            //    for (int y = 0; y < tiff.ImageHegiht; y++)
            //    {
            //        for (int x = 0; x < tiff.ImageWidth; x++)
            //        {
            //            image[y, x] = tiff.Value[sn++];
            //        }
            //    }
            //    img.imageData = image;
            //    img.createTime = DateTime.Now;
            //    imgList.Add(img);
            //}
            //if (RI.OpenFromFile(file))
            //{
            //    img.pixelSize = RI.RIImage.ImagePlanePixelSpacing[0];
            //    img.centerX = -RI.RIImage.RTImagePosition[0] / RI.RIImage.ImagePlanePixelSpacing[0];
            //    img.centerY = RI.RIImage.RTImagePosition[1] / RI.RIImage.ImagePlanePixelSpacing[1];
            //    img.level = (int)RI.RIImage.WindowCenter;
            //    img.window = (int)RI.RIImage.WindowWidth;
            //    img.imageHeight = RI.ImagePixel.PixelData.GetLength(0);
            //    img.imageWidth = RI.ImagePixel.PixelData.GetLength(1);
            //    ushort[,] image = new ushort[img.imageHeight, img.imageWidth];
            //    short[,] dv = RI.ImagePixel.PixelData;
            //    for (int y = 0; y < img.imageHeight; y++)
            //    {
            //        for (int x = 0; x < img.imageWidth; x++)
            //        {
            //            image[y,x] = (ushort)dv[y,x];
            //        }
            //    }
            //    img.imageData = image;
            //    imgList.Add(img);
            //    GC.Collect();
            //}
            if (His.LoadDataFromFile(file))
            {
                if (His.dataList != null)
                {
                    for (int i = 0; i < His.dataList.Count; i++)
                    {
                        ImageObject imageObj = new ImageObject();
                   
                        imageObj.ImageData = His.dataList[i];
                        imageRows = (uint)imageObj.ImageData.GetLength(0);
                        imageColumns = (uint)imageObj.ImageData.GetLength(1);
                        RefreshScale();
                        imageObj.pixelSize = pixelSize;
                        imageObj.centerX = imageCenterX;
                        imageObj.centerY = imageCenterY;
                        imgList.Enqueue(imageObj);

                    }
                    His.dataList = null;
                    GC.Collect();
                }
            }
            //else if (RD.OpenFromFile(file))//增加RD图像，2011.12.15，ml
            //{
                
            //    //img.level = 4590;
            //    //img.window = 10000;
            //    if (RD.RD.PixelData.Length > 0)
            //    {
            //        img.pixelSize = RD.RD.PixelSpacing;
            //        img.imageWidth = RD.RD.PixelData[0].GetLength(1);
            //        img.imageHeight = RD.RD.PixelData[0].GetLength(0);
            //        img.centerX = img.imageWidth / 2; //-RDF.fra.ImagePosition[0] / PixelSize;
            //        img.centerY = img.imageHeight / 2;//RDF.fra.ImagePosition[1] / PixelSize;
            //        img.SliceDepth = RD.RD.ImagePosition[1];

            //        ushort[,] image = new ushort[img.imageHeight, img.imageWidth];

            //        UInt32[,] rd = RD.RD.PixelData[0];
            //        for (int i = 0; i < img.imageHeight; i++)
            //        {
            //            for (int j = 0; j < img.imageWidth; j++)
            //            {
            //                image[i, j] = Convert.ToUInt16(rd[i, j] * RD.RD.DoseGridScaling * 1000F);
            //            }
            //        }
            //        img.imageData = image;
            //        imgList.Add(img);
            //        GC.Collect();
            //    }
            //}
            //else
            //{
            //    try
            //    {
            //        //Bitmap bm = new Bitmap(file);
            //        //LWList.Add(new Point(-1, -1));   
            //        //imgList.Add(BitmapToUShort(bm));
            //        //pixelSize = phySize / imgList[0].GetLength(0);
            //        //imageCenterX = phyCenterX / pixelSize;
            //        //imageCenterY = phyCenterY / pixelSize;
            //    }
            //    catch (System.Exception ex)
            //    {
            //        ;
            //    }
            //}
        }

        public override void CaptureImageData()
        {
            base.CaptureImageData();

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.*|*.*";

            string lastFilePath = CapturePub.readCaptrueValue(XmlField.OpenFilePath);
            if (Directory.Exists(lastFilePath))
            {
                dlg.InitialDirectory = lastFilePath;
            }
            dlg.Multiselect = true;

            string[] files = null;
            if (dlg.ShowDialog() == DialogResult.OK)
            {                
                files = dlg.FileNames;
                SeqFrameCount = files.Length;
            }
            if (files != null && files.Length > 0)
            {
                FileInfo fi = new FileInfo(files[0]);
                CapturePub.saveCaptrueValue(XmlField.OpenFilePath, fi.DirectoryName);
                int msgID = GenerateWinMessage("Loading files...");
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS,msgID , SeqFrameCount);
                for (int i = 0; i < SeqFrameCount; i++)
                {
                    ReadFile(files[i]);
                    Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -1, 1);
                }
                Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_SHOW_PROGRESS, -2, 0);
                //Kernel32Interface.SendMessage(HostHandle, WIN_MSG.WM_CAPTURE_DATA, (int)CapturePKI.PanelCaptureMode.Sequence,0);
            }
            else 
            {
                files = null;
            }
            Files = files;
        }
        //public AverageImageObjectBase CaptureAverageImageData()//增加his融合图像，2011.12.15，ml
        //{
        //    base.CaptureAverageImageData();
        //    OpenFileDialog dlg = new OpenFileDialog();
        //    dlg.Filter = "*.*|*.*";

        //    string lastFilePath = CapturePub.readCaptrueValue("OpenfilePath");
        //    if (Directory.Exists(lastFilePath))
        //    {
        //        dlg.InitialDirectory = lastFilePath;
        //    }
        //    dlg.Multiselect = true;
        //    string[] files = null;
        //    if (dlg.ShowDialog() == DialogResult.OK)
        //    {
        //        files = dlg.FileNames;
        //        FrameCount = files.Length;
        //    }
        //    AverageImageObjectBase aveImage = new AverageImageObjectBase();
        //    if (files != null && files.Length > 0)
        //    {
        //        FileInfo fi = new FileInfo(files[0]);
        //        CapturePub.saveCaptrueValue("OpenfilePath", fi.DirectoryName);

        //        for (int i = 0; i < FrameCount; i++)
        //        {

        //            ReadFile(files[i]);

        //            //if (i == 0)
        //            //{
        //            aveImage.pixelSize = img.pixelSize;
        //            aveImage.centerX = img.centerX; 
        //            aveImage.centerY = img.centerY;
        //            //}

        //            aveImage.AppendImageData(imgList[i].imageData, 0);
        //            //  数据加aveIMage     dataList[0]
        //        }
        //        return aveImage;
        //    }
        //    else 
        //    {
        //        files = null;
        //    }
        //    Files = files;
        //    return null;
        //}

        //public ImageObject SumImageObectBase(ref float[,] data1)//增加his融合图像，2011.12.15，ml
        //{
        //    OpenFileDialog dlg = new OpenFileDialog();
        //    dlg.Filter = "*.*|*.*";

        //    string lastFilePath = CapturePub.readCaptrueValue("OpenfilePath");
        //    if (Directory.Exists(lastFilePath))
        //    {
        //        dlg.InitialDirectory = lastFilePath;
        //    }
        //    dlg.Multiselect = true;
        //    string[] files = null;
        //    if (dlg.ShowDialog() == DialogResult.OK)
        //    {
        //        files = dlg.FileNames;
        //        FrameCount = files.Length;
        //    }
        //    if (files != null && files.Length > 0)
        //    {
        //        ImageObject SumImage = new ImageObject();
        //        FileInfo fi = new FileInfo(files[0]);
        //        CapturePub.saveCaptrueValue("OpenfilePath", fi.DirectoryName);
        //        long[,] TotalImageValue = null;
                
        //        for (int k = 0; k < FrameCount; k++)
        //        {

        //            ReadFile(files[k]);
        //            if (imgList.Count <= 0)
        //            {
        //                continue;
        //            }
        //            img = imgList[0];
        //            imgList.Clear();
        //            if (TotalImageValue == null)
        //            {
        //                SumImage.pixelSize = img.pixelSize;
        //                SumImage.centerX = img.centerX;
        //                SumImage.centerY = img.centerY;
        //                TotalImageValue = new long[img.imageData.GetLength(0), img.imageData.GetLength(1)];
        //            }
        //            long Sum = 0;
        //            int count = 0;
        //            long MedianValue;
        //            //如下取图像中心区域20x20像素平均值，作为判断该图是否该算入总剂量
        //            for (int i = img.imageData.GetLength(0) / 2 - 10; i < img.imageData.GetLength(0) / 2 + 10; i++)
        //            {
        //                for (int j = img.imageData.GetLength(1) / 2 - 10; j < img.imageData.GetLength(1) / 2 + 10; j++)
        //                {
        //                    Sum += (long)img.imageData[i, j];
        //                    count++;
        //                }
        //            }
        //            MedianValue = Sum / count;

        //            if (MedianValue > 10000)//筛选条件，像素值大于10000
        //            {
        //                for (int i = 0; i < img.imageData.GetLength(0); i++)
        //                {
        //                    for (int j = 0; j < img.imageData.GetLength(1); j++)
        //                    {
        //                        TotalImageValue[i, j] += (long)(img.imageData[i, j]);
        //                    }
        //                }
        //            }
        //            //  数据加aveIMage     dataList[0]
        //        }

        //        if (TotalImageValue == null)
        //        {
        //            return null;
        //        }
        //        long MaxValue = -1;
             
        //        ushort[,] dosedata = new ushort[TotalImageValue.GetLength(0), TotalImageValue.GetLength(1)];
        //        data1 = new float[TotalImageValue.GetLength(0), TotalImageValue.GetLength(1)];
        //        for (int i = 0; i < data1.GetLength(0); i++)
        //        {
        //            for (int j = 0; j < data1.GetLength(1); j++)
        //            {
        //                data1[i, j] = (float)(0.14574f + 5.1f * Math.Pow(10, -5) * TotalImageValue[i, j] - 2.65f *
        //                    Math.Pow(10, -14) * TotalImageValue[i, j] * TotalImageValue[i, j]) / 100f;//单位Gy

        //                dosedata[i, j] = (ushort)(data1[i, j] * 1000F);
        //            }
        //        }

        //        SumImage.imageData = dosedata;
        //        return SumImage;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //    return null;
        //}

        public override void Cancel()
        {
            base.Cancel();
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
