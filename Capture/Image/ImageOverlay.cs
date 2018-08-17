using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;


namespace ImageCapturing
{
    public class ImageOverlay : ImageROI
    {
        public float overlayValue = 0;

        public string imageGuid_A;
        public string imageGuid_B;
        public bool ShowOverlay = true;

        public static ImageOverlay GenerateImageOverlay(ImageObject image)
        {
            ImageOverlay imgOverlay = new ImageOverlay();
            imgOverlay.imageGuid = Guid.NewGuid().ToString();
            //imgOverlay.sourceID = image.sourceID;
            imgOverlay.centerX = image.centerX;
            imgOverlay.centerY = image.centerY;
            //imgOverlay.imageWidth = image.imageWidth;
            //imgOverlay.imageHeight = image.imageHeight;
            //imgOverlay.minValue = image.minValue;
            //imgOverlay.maxValue = image.maxValue;
            //imgOverlay.averageValue = image.averageValue;
            imgOverlay.pixelSize = image.pixelSize;
            //imgOverlay.level = image.level;
            //imgOverlay.window = image.window;
            //imgOverlay.converse = image.converse;
            //imgOverlay.colorModeName = image.colorModeName;
            //imgOverlay.LUT = image.LUT;
            //if (image.imageData != null)
            //{
            //    imgOverlay.imageData_A = (ushort[,])image.imageData.Clone();
            //}
            imgOverlay.imageGuid_A = image.imageGuid;
            return imgOverlay;
        }
    }   
}
 
