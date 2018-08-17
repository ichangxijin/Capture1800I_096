using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ImageCapturing
{
    public partial class ImgBase : ControlBase//, InterfaceRefresh
    {
        protected PictureBox mImgPictureBox = null;
        /// <summary>
        /// 绘图板
        /// </summary>
        public PictureBox ImgPictureBox
        {
            get
            {
                return mImgPictureBox;
            }
            set
            {
                mImgPictureBox = value;
            }
        }

        public bool MouseZoom = true;

        public ushort[,] imageData;
        //public List<GraphBase> graphs = new List<GraphBase>();
        public ImageObject imgObject = new ImageObject();

        /// 放缩后的坐标变换矩阵
        public Matrix Jacobi_Image = new Matrix();

        /// 最大的缩放比例
        private float ScaleMax = 100;

        /// 最小的缩放比例
        private float ScaleMin = 0.1F;

        private Point WLmovePoint = Point.Empty;

        public delegate void PositionAndValueDelegate(Point p, int v);
        public event PositionAndValueDelegate PositionAndValue;


        Bitmap IntegratedImage = null;
        Bitmap bakImage = null;


        public ImgBase()
        {
            BorderStyle = BorderStyle.Fixed3D;
            mImgPictureBox = new PictureBox();
            this.Controls.AddRange(new Control[]{
                mImgPictureBox });
            SizeChanged += new EventHandler(ImgBase_SizeChanged);

            mImgPictureBox.MouseDown += new MouseEventHandler(mImgPictureBox_MouseDown);
            mImgPictureBox.MouseUp += new MouseEventHandler(mImgPictureBox_MouseUp);
            mImgPictureBox.MouseMove += new MouseEventHandler(mImgPictureBox_MouseMove);
            //mImgPictureBox.Paint += new PaintEventHandler(mImgPictureBox_Paint);
            mImgPictureBox.MouseWheel += new MouseEventHandler(mImgPictureBox_MouseWheel);
            mImgPictureBox.MouseEnter += new EventHandler(mImgPictureBox_MouseEnter);
            mImgPictureBox.MouseLeave += new EventHandler(mImgPictureBox_MouseLeave);
        }

        private void ResetShowScale()
        {
            int ctHeight = 512;
            int ctWidth = 512;
            if (imageData != null)
            {
                ctHeight = imageData.GetLength(0);
                ctWidth = imageData.GetLength(1);
            }

            float viewW = (float)ImgPictureBox.Width;
            float viewH = (float)ImgPictureBox.Height;
            float scale = Math.Min(viewW / ctWidth, viewH / ctHeight);
            float scaleX = viewW / ctWidth;
            float scaleY = viewH / ctHeight;
            if (scale < 0.000001)
            {
                scale = 1;
            }
            float tempWidth = ctWidth * scale;
            float tempHeight = ctHeight * scale;
            if (tempWidth < 1.0f)
            {
                tempWidth = 1.0f;
            }
            if (tempHeight < 1.0f)
            {
                tempHeight = 1.0f;
            }
            float dx = (viewW - tempWidth) / 2.0f;
            float dy = (viewH - tempHeight) / 2.0f;
            Jacobi_Image = new Matrix(scale, 0, 0, scale, dx, dy);
            try
            {
                //imageBitmap = new Bitmap(srcImageBitmap, (int)(tempWidth + 0.5), (int)(tempHeight + 0.5));
            }
            catch
            {
                ;
            }

        }

        public void InitImageData(ImageObject image)
        {
            imgObject = image;
            imageData = imgObject.ImageData;

            if (Jacobi_Image.IsIdentity)
            {
                ResetShowScale();
            }
            RefreshView();
        }

        public virtual void ClearImageData()
        {
            imageData = null;
            imgObject = null;
            Jacobi_Image = new Matrix();
            RefreshView();
        }

        protected void ImgBase_SizeChanged(object sender, EventArgs e)
        {
            mImgPictureBox.Location = new Point(0, 0);
            mImgPictureBox.Size = new Size(this.Width, this.Height);
            ResetShowScale();
            RefreshView();
        }

        /// <summary>
        /// 刷新图像（将图层依次画到picturebox的图像上）
        /// </summary>
        public virtual void RefreshView()
        {
            if (imageData == null )
            {
                return;
            }
            int ImgWidth = mImgPictureBox.Width;
            int ImgHeight = mImgPictureBox.Height;
            if (ImgWidth < 1 || ImgHeight < 1)
            {
                return;
            }

            if (IntegratedImage == null || IntegratedImage.Size != mImgPictureBox.Size)
            {
                IntegratedImage = new Bitmap(mImgPictureBox.Width, mImgPictureBox.Height);
            }
            Graphics gp = Graphics.FromImage(IntegratedImage);
            GenerageBackImage();
            mImgPictureBox.Image = bakImage;
            mImgPictureBox.Refresh();
            gp.Dispose();
        }

        protected void GenerageBackImage()
        {
            if (mImgPictureBox.Width < 1 || this.mImgPictureBox.Height < 1)
            {
                return;
            }
            if (bakImage == null || bakImage.Size != mImgPictureBox.Size)
            {
                bakImage = new Bitmap(mImgPictureBox.Width, mImgPictureBox.Height);
            }
            Graphics g = Graphics.FromImage(bakImage);
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, bakImage.Width, bakImage.Height));
            RectangleF srcRec = new RectangleF(0, 0, (float)imageData.GetLength(1), (float)imageData.GetLength(0));
            RectangleF zoomSrcRec = PubMatrix.TransformRectangle(srcRec, Jacobi_Image);
            RectangleF viewRec = new RectangleF(0, 0, (float)mImgPictureBox.Width, (float)mImgPictureBox.Height);
            RectangleF viewZoomRec = RectangleF.Intersect(viewRec, zoomSrcRec);
            Matrix mxInvert = Jacobi_Image.Clone();
            mxInvert.Invert();
            RectangleF viewSrcRec = PubMatrix.TransformRectangle(viewZoomRec, mxInvert);
            if (PubMatrix.ReviseImageRect(imgObject.BMP.Size, ref viewSrcRec))
            {
                g.DrawImage(imgObject.BMP, viewZoomRec, viewSrcRec, GraphicsUnit.Pixel);
            }

            mxInvert.Dispose();
            g.Dispose();
        }

        #region events

        /// <summary>
        /// Draw graphic objects and group selection rectangle (optionally)
        /// </summary>
        private void mImgPictureBox_Paint(object sender, PaintEventArgs e)
        {
            //foreach (GraphBase graph in graphs)
            //{
            //    //
            //}
        }

        protected void mImgPictureBox_MouseEnter(object sender, EventArgs e)
        {
            mImgPictureBox.Focus();
            MouseZoom = true;//XmlAccess.readOpsValue("WheelZoom") != "F";
        }


        protected void mImgPictureBox_MouseLeave(object sender, EventArgs e)
        {
            MouseZoom = false;
            if (imageData != null && imageData.Length > 0)
            {
                if (PositionAndValue != null)
                {
                    PositionAndValue(Point.Empty, 0);
                }
            }
        }

        /// <summary>
        /// Mouse down.
        /// Left button down event is passed to active tool.
        /// Right button down event is handled in this class.
        /// </summary>
        protected virtual void mImgPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                    WLmovePoint = e.Location;
            }
        }

        /// <summary>
        /// Mouse move.
        /// Moving without button pressed or with left button pressed
        /// is passed to active tool.
        /// </summary>
        protected virtual void mImgPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (imageData != null && imageData.Length > 0)
            {
                Matrix m = Jacobi_Image.Clone();
                m.Invert();
                Point p = PubMatrix.TransformPoint(e.Location, m);
                int v = 0;
                if (p.X < 0 || p.Y < 0 || p.X >= imageData.GetLength(1) || p.Y >= imageData.GetLength(0))
                {
                    v = 0;
                }
                else
                {
                    v = imageData[p.Y, p.X];
                }

                if (PositionAndValue != null)
                {
                    PositionAndValue(p, v);
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                int dx = e.X - WLmovePoint.X;
                int dy = e.Y - WLmovePoint.Y;
                if (dx == 0 && dy == 0) return;
                WLmovePoint = e.Location;
                imgObject.imgWindow = imgObject.imgWindow + dy * 2;
                if (imgObject.imgWindow <= 0)
                {
                    imgObject.imgWindow = 1;
                }
                imgObject.imgLevel = imgObject.imgLevel + dx * 2;
                if (imgObject.imgLevel <= 0)
                {
                    imgObject.imgLevel = 1;
                }

                imgObject.GenerateImage();

                RefreshView();
            }

        }

        /// <summary>
        /// Mouse up event.
        /// Left button up event is passed to active tool.
        /// </summary>
        protected virtual void mImgPictureBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Cursor = Cursors.Default;
            }
        }


        void mImgPictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (MouseZoom)
            {
                if (e.Delta > 0)
                {
                    ZoomMouseUp(new PointF(e.X, e.Y), true);
                } 
                else if (e.Delta < 0)
                {
                    ZoomMouseUp(new PointF(e.X, e.Y), false);
                }
            }
        }
        /// <summary>
        /// 缩放时MouseUp时发生
        /// </summary>
        public void ZoomMouseUp(PointF curPoint, bool toBig)
        {
            if (imageData == null)
            {
                return;
            }
            Matrix invertJac = Jacobi_Image.Clone();
            invertJac.Invert();
            PointF[] ptsCur = new PointF []{curPoint};
            invertJac.TransformPoints(ptsCur);
            curPoint = ptsCur[0];
            int ctHeight = imageData.GetLength(0);
            int ctWidth = imageData.GetLength(1);
            
            float scale = Jacobi_Image.Elements[0];
            if ((toBig && scale >= ScaleMax) || (!toBig && scale <= ScaleMin))
            {
                return; //超过缩放限制后不再继续
            }
            float k = toBig ? 1.1f : 0.9f;
            float tempScale = scale * k;
            if (tempScale > ScaleMax)
            {
                tempScale = ScaleMax;
            }
            else if (tempScale < ScaleMin)
            {
                tempScale = ScaleMin;
            }
            //float defaultScale = Math.Min(imgPictureBox.Width / (float)ctWidth, imgPictureBox.Height / (float)ctHeight);
            PointF pCenter = PointF.Empty;
            bool dependMouse = true;
            if (dependMouse)
            {
                pCenter = curPoint;
                if (pCenter.IsEmpty) return;
                PointF[] pts = new PointF[1] { pCenter };
                Jacobi_Image.TransformPoints(pts);
                PointF pt1 = pts[0];
                Jacobi_Image.Scale(k, k);
                pts[0] = pCenter;
                Jacobi_Image.TransformPoints(pts);
                PointF pt2 = pts[0];
                float dx = pt1.X - pt2.X;
                float dy = pt1.Y - pt2.Y;
                Jacobi_Image.Translate(dx, dy, MatrixOrder.Append);
            }
            else
            {
                pCenter = new PointF(ctWidth / 2.0f, ctHeight / 2.0f);
                Jacobi_Image.Scale(k, k);
                Jacobi_Image = new Matrix(Jacobi_Image.Elements[0], 0.0f, 0.0f, Jacobi_Image.Elements[3], 0.0f, 0.0f);
                PointF[] pts = new PointF[] { pCenter };
                Jacobi_Image.TransformPoints(pts);
                float dx = mImgPictureBox.Width / 2.0f - pts[0].X;
                float dy = mImgPictureBox.Height / 2.0f - pts[0].Y;
                Jacobi_Image.Translate(dx, dy, MatrixOrder.Append);
            }
            //ResetShowScale();
            RefreshView();
        }

        #endregion
    }
}
