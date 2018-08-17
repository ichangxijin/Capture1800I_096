using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Windows.Forms;
using AMRT;

namespace ImageCapturing
{
    public class AssistantCheckGraphTool
    {
        #region 变量

        public bool Dirty { get; set; }
        private int HandleNum = 0;
        public Matrix Transform = new Matrix();

        public List<ImageROI> imgBase;
        private ImageROI img;

        public delegate void StatisticsInformationHandller();
        public event StatisticsInformationHandller statisticsInformaiton;

        #endregion

        #region 构造函数
        public AssistantCheckGraphTool()
        {
        }

        #endregion

        #region 与图形单元相关的操作：绘制、检测、修改等

        public void DrawGraph(Graphics gp,Matrix transform)
        {
            if (img != null)
            {
                this.Transform = transform;
                Rectangle tempRect = new Rectangle();
                tempRect = img.ROIRectangle;
                Point[] temp = new Point[] { tempRect.Location };
                transform.TransformPoints(temp);
                tempRect.Location = temp[0];
                tempRect.Width = (int)(tempRect.Width * transform.Elements[0]);
                tempRect.Height = (int)(tempRect.Height * transform.Elements[0]);
                Pen rectPen = new Pen(Color.Red, 1);
                rectPen.DashStyle = DashStyle.Dash;
                gp.DrawRectangle(rectPen, tempRect);
                Pen errorPen = new Pen(Color.Blue, 2);
                if (img.noisePoint != null)
                {
                    if (img.noisePoint.Count != 0)
                    {
                        try
                        {
                            string flag = CapturePub.readCaptrueValue("ShowDiffPoint");
                            if (flag == "T")
                            {
                                for (int i = 0; i < img.noisePoint.Count; i++)
                                {
                                    Point[] tempDraw = new Point[] { img.noisePoint[i] };
                                    Transform.TransformPoints(tempDraw);
                                    gp.DrawEllipse(errorPen, tempDraw[0].X, tempDraw[0].Y, 2, 2);
                                }
                            }
                        }
                        catch
                        {
                            CapturePub.saveCaptrueValue("ShowDiffPoint", "F");
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  ZL 2006/05/31
        /// 光标感知直线
        /// </summary>
        /// <param name="desPoint1">感应的目标直线开始点</param>
        /// <param name="desPoint2">感应的目标直线结束点</param>
        /// <param name="x">鼠标当前的X坐标</param>
        /// <param name="y">鼠标当前的Y坐标</param>
        /// <returns>鼠标的样式</returns>
        /// <param name="extendFlag">是否判断线段延长的部分</param>
        /// <returns></returns>
        public Cursor DetectRectangle(float x, float y, bool extendFlag)
        {
            if (img == null)
            {
                return Cursors.Default;
            }
            InvTransform(ref x, ref y);
            int sensitiveCircle = 5;
            int x1 = img.ROIRectangle.X;
            int y1 = img.ROIRectangle.Y;
            int x2 = img.ROIRectangle.Right;
            int y2 = img.ROIRectangle.Bottom;
            List<Point> list = new List<Point>();

            

            list.Add(this.img.ROIRectangle.Location);
            list.Add(new Point(x2, y1));
            list.Add(new Point(x2, y2));
            list.Add(new Point(x1, y2));

            Point[] cood = list.ToArray();

            for (int i = 0; i < list.Count; i++)
            {
                Point p = list[i];
                if (Math.Abs(x - p.X) <= sensitiveCircle && Math.Abs(y - p.Y) <= sensitiveCircle)
                {
                    HandleNum = (2 + i * 4) / 2;
                    if (i % 2 == 0)
                    {
                        return Cursors.SizeNWSE;
                    }
                    else
                    {
                        return Cursors.SizeNESW;
                    }
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                Point desPoint1 = list[i % list.Count];
                Point desPoint2 = list[(i + 1) % list.Count];

                if (Math.Abs(desPoint1.X - desPoint2.X) <= 1 && Math.Abs(desPoint1.Y - desPoint2.Y) > 1)   //竖轴
                {
                    if (Math.Abs(desPoint1.X - x) <= sensitiveCircle &&
                        (extendFlag || y < Math.Max(desPoint1.Y, desPoint2.Y) && y > Math.Min(desPoint1.Y, desPoint2.Y)))
                    {
                        HandleNum = (i + 1) * 2;
                        return Cursors.SizeWE;
                    }
                }
                else if (Math.Abs(desPoint1.X - desPoint2.X) > 1 && Math.Abs(desPoint1.Y - desPoint2.Y) <= 1)   //横轴
                {
                    if (Math.Abs(desPoint1.Y - y) <= sensitiveCircle &&
                        (extendFlag || x < Math.Max(desPoint1.X, desPoint2.X) && x > Math.Min(desPoint1.X, desPoint2.X)))
                    {
                        HandleNum = (i + 1) * 2;
                        return Cursors.SizeNS;
                    }
                }
            }
            return Cursors.Default;
        }

        #endregion

        #region 所需的方法函数

        //记得每次更换选择图片时运行该方法
        public void CheckImageInList(int index)
        {
            if (imgBase != null)
            {
                if (imgBase.Count != 0)
                {
                    img = imgBase[index];
                }
            }
        }

        //edtionTag 为版本控制变量
        //版本1为边线框可以拖离边界区
        //版本2只能在边界区域内拖动
        public void MoveHandleTo(Point point,int edtionTag)
        {
            if (edtionTag == 0)
            {
                MoveHandleAnyWhere(point);
                GetStatisticsInfomation();
            }
            else if (edtionTag ==1)
            {
                MoveHandleFixedArea(point);
                GetStatisticsInfomation();
            }
            if (statisticsInformaiton != null)
            {
                statisticsInformaiton();
            }
        }

        private void MoveHandleFixedArea(Point point)
        {
           
        }

        private void TransMousePointToBitmap(ref Point point)
        {
            float x = (float)point.X;
            float y = (float)point.Y;
            InvTransform(ref x, ref y);
            point.X = (int)x;
            point.Y = (int)y;
        }

        /// <summary>
        /// Move handle to new point (resizing)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public void MoveHandleAnyWhere(Point point)
        {
            TransMousePointToBitmap(ref point);
            int left = img.ROIRectangle.Left;
            int right = img.ROIRectangle.Right;
            int top = img.ROIRectangle.Top;
            int bottom = img.ROIRectangle.Bottom;
            switch (HandleNum)
            {
                case 1://左上
                    top = ((img.ROIRectangle.Bottom - point.Y) <= 20) ? (img.ROIRectangle.Bottom - 20) : point.Y;
                    left = ((img.ROIRectangle.Right - point.X) <= 20) ? (img.ROIRectangle.Right - 20) : point.X;
                    break;
                case 2://上边
                    top = ((img.ROIRectangle.Bottom - point.Y) <= 20) ? (img.ROIRectangle.Bottom - 20) : point.Y;
                    break;
                case 3://右上
                    right = ((point.X - img.ROIRectangle.Left) <= 20) ? (img.ROIRectangle.Left + 20) : point.X;
                    top = ((img.ROIRectangle.Bottom - point.Y) <= 20) ? (img.ROIRectangle.Bottom - 20) : point.Y;
                    break;
                case 4://右边
                    right = ((point.X - img.ROIRectangle.Left) <= 20) ? (img.ROIRectangle.Left + 20) : point.X;
                    break;
                case 5://右下
                    right = ((point.X - img.ROIRectangle.Left) <= 20) ? (img.ROIRectangle.Left + 20) : point.X;
                    bottom = ((point.Y - img.ROIRectangle.Top) <= 20) ? (img.ROIRectangle.Top + 20) : point.Y;
                    break;
                case 6://底边
                    bottom = ((point.Y - img.ROIRectangle.Top) <= 20) ? (img.ROIRectangle.Top + 20) : point.Y;
                    break;
                case 7://左下
                    left = ((img.ROIRectangle.Right - point.X) <= 20) ? (img.ROIRectangle.Right - 20) : point.X;
                    bottom = ((point.Y - img.ROIRectangle.Top) <= 20) ? (img.ROIRectangle.Top + 20) : point.Y;
                    break;
                case 8://左边
                    left = ((img.ROIRectangle.Right - point.X) <= 20) ? (img.ROIRectangle.Right - 20) : point.X;
                    break;
            }
            left = (left < 0) ? 0 : ((left > 1024) ? 1024 : left);
            right = (right < 0) ? 0 : ((right > 1024) ? 1024 : right);
            top = (top < 0) ? 0 : ((top > 1024) ? 1024 : top);
            bottom = (bottom < 0) ? 0 : ((bottom > 1024) ? 1024 : bottom);
            Rectangle temp = new Rectangle(left, top, (right - left), (bottom - top));
            SetRectAngle(temp);

        }

        public void SetRectAngle(Rectangle tempRect)
        {
            img.ROIRectangle = tempRect;
            Dirty = true;
        }

        private void InvTransform(ref float x, ref float y)
        {
            PointF[] tempPt = new PointF[1] { new PointF(x, y) };
            Transform.Invert();
            Transform.TransformPoints(tempPt);
            Transform.Invert();

            x = tempPt[0].X;
            y = tempPt[0].Y;
        }

        /// <summary>
        /// 判断点是否在多边形内
        /// </summary>
        /// <param name="CoordInput">多边形数组</param>
        /// <param name="xx">点X坐标</param>
        /// <param name="yy">点Y坐标</param>
        /// <returns>是否在多边形内</returns>
        public static bool LYJ_pointIsInArea(Point[] CoordInput, float xx, float yy)
        {
            Point[] Coord = new Point[CoordInput.Length + 1];
            CoordInput.CopyTo(Coord, 0);
            Coord[Coord.Length - 1] = CoordInput[0];
            int npol = Coord.Length;
            int i, j;
            bool c = false;
            for (i = 0, j = npol - 1; i < npol; j = i++)
            {
                if ((((Coord[i].Y <= yy) && (yy < Coord[j].Y)) ||
                     ((Coord[j].Y <= yy) && (yy < Coord[i].Y))) &&
                    (xx < (Coord[j].X - Coord[i].X) * (yy - Coord[i].Y) /
                      (Coord[j].Y - Coord[i].Y) + Coord[i].X))
                {
                    c = !c;
                }
            }
            return c;
        }

        #endregion

        #region 提取给定图像ROI的信息，信号信息的平均值、方差等

        //对所给定的图像进行边缘检测，如果对比度较大，则计算出相应外轮廓
        //在ImageROI中添加相应的边界点数组，并添加相应的矩形区域选框
        //检测
        public void GetBoundaryOfData()
        {
            ushort[,] data = img.ImageData;
            int width = data.GetLength(0);
            int height = data.GetLength(1);
            int sum = 0;
            for (int i = width / 2 - 5; i < width / 2 + 5; i++)
            {
                for (int j = height / 2 - 5; j < height / 2 + 5; j++)
                {
                    sum += data[i, j];
                }
            }
            int average = sum / 100;
            double scale = 0.5;
            try
            {
                scale = double.Parse(CapturePub.readCaptrueValue("EvaluateScaleThreshold"));
            }
            catch (System.Exception ex)
            {
                scale = 0.5;
            }

            int threshold = (int)(average * scale);
            Bitmap bmp = (Bitmap)(img.BMP.Clone());
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (data[i, j] > threshold)
                    {
                        bmp.SetPixel(j, i, Color.Red);
                    }
                }
            }
            BoundaryTracker bt = new BoundaryTracker();
            if (bt.GetSerializedBoundary(bmp, Color.Red))
            {
                int indexPts = 0;
                for (int i = 0; i < bt.CL.Count; i++)
                {
                    if (bt.CL[i].Length > bt.CL[indexPts].Length)
                    {
                        indexPts = i;
                    }
                }
                if (bt.CL.Count != 0)
                {
                    PointF[] pts = bt.CL[indexPts];
                    int Xmin = int.MaxValue;
                    int Xmax = int.MinValue;
                    int Ymin = int.MaxValue;
                    int Ymax = int.MinValue;
                    for (int i = 0; i < pts.Length; i++)
                    {
                        if (Xmin > pts[i].X)
                        {
                            Xmin = (int)pts[i].X;
                        }
                        if (Ymin > pts[i].Y)
                        {
                            Ymin = (int)pts[i].Y;
                        }
                        if (Xmax < pts[i].X)
                        {
                            Xmax = (int)pts[i].X;
                        }
                        if (Ymax < pts[i].Y)
                        {
                            Ymax = (int)pts[i].Y;
                        }
                    }
                    Point pt1 = new Point(Xmin, Ymin);
                    Point pt2 = new Point(Xmax, Ymin);
                    Point pt3 = new Point(Xmax, Ymax);
                    Point pt4 = new Point(Xmin, Ymax);
                    Rectangle rect = new Rectangle(pt1, new Size(Xmax - Xmin, Ymax - Ymin));
                    img.boundaryPoint = new List<Point>() { pt1, pt2, pt3, pt4 };
                    img.BoundaryRect = rect;
                    img.ROIRectangle = rect;

                    return;
                }
            }
            img.boundaryPoint = new List<Point>() { new Point(0, 0), new Point(data.GetLength(0), 0), 
                new Point(data.GetLength(0), data.GetLength(1)), new Point(0, data.GetLength(1)) };
            img.BoundaryRect = new Rectangle(new Point(0, 0), new Size(data.GetLength(0), data.GetLength(1)));
            img.ROIRectangle = img.BoundaryRect;

            return;
        }

        //这里只是特殊处理矩形的情形，对于凸多边形或凹多边形等复杂情况暂不做考虑
        //只是在该矩形区域获取图像信号强度的平均值、方差等信息，满足一定阈值条件下的噪声点
        public void GetStatisticsInfomation()
        {
            int Xmin = img.ROIRectangle.Left;
            int Xmax = img.ROIRectangle.Right;
            int Ymin = img.ROIRectangle.Top;
            int Ymax = img.ROIRectangle.Bottom;
            

            int RecNum = 0;

            ushort[,] data = img.ImageData;
            if (Xmax > data.GetLength(1))
            {
                Xmax = data.GetLength(1);
            }
             if (Ymax > data.GetLength(0))
            {
                Ymax = data.GetLength(0);
            }
            double sum = 0;
            for (int i = Ymin; i < Ymax; i++)
            {
                for (int j = Xmin; j < Xmax; j++)
                {
                    sum += data[i, j];
                    RecNum++;
                }
            }
            img.ROIMeanValue = (double)(sum / ((Xmax - Xmin + 1) * (Ymax - Ymin + 1)));//图像灰度均值

            //计算标准差
            double sumVariance = 0;
            for (int i = Ymin; i < Ymax; i++)
            {
                for (int j = Xmin; j < Xmax; j++)
                {
                    sumVariance += Math.Pow((data[i, j] - img.ROIMeanValue), 2);//方差
                }
            }
            img.ROIVar = Math.Sqrt(sumVariance / ((Xmax - Xmin + 1) * (Ymax - Ymin + 1)));//标准差


            UInt32 diffNum = 0;
            Int32 diff = 100;
            try
            {
                diff = (Int32)(img.ROIMeanValue * double.Parse(CapturePub.readCaptrueValue("EvaluateDiffThreshold")));
            }
            catch (System.Exception ex)
            {
                diff = (int)(img.ROIMeanValue * 0.5);
            }
            List<Point> temp = new List<Point>();
            for (int i = Ymin; i < Ymax - 1; i++)
            {
                for (int j = Xmin; j < Xmax - 1; j++)
                {
                    Int32 diffColumn = data[i, j + 1] - data[i, j];//dy
                    Int32 diffRow = data[i + 1, j] - data[i, j];//dx
                    if (Math.Abs(diffColumn) > diff || Math.Abs(diffRow) > diff)
                    {
                        diffNum++;
                        temp.Add(new Point(j, i));
                    }
                }
            }
            img.ROIDiffNumber = diffNum;
            if (diffNum > (((Xmax - Xmin + 1) * (Ymax - Ymin + 1)) / 4))
            {
                try
                {
                    string tag = CapturePub.readCaptrueValue("ShowDiffPoint");
                    if (tag == "T")
                    {
                        cls_MessageBox.Show("There too many error point!");
                    }
                }
                catch (System.Exception ex)
                {
                    CapturePub.saveCaptrueValue("ShowDiffPoint", "F");
                }

            }
            else
            {
                img.noisePoint = temp;
            }

            //2011.12.21 syz calculate SNR;
            //long Ave = 0;
            //long Sum = 0;
            //double Variance = 0;
            //double SNR = 0;
            //ushort[,] data = imgROI.ImageData;
            //for (int i = 0; i < data.GetLength(0); i++)
            //{
            //    for (int j = 0; j < data.GetLength(1); j++)
            //    {


            //        Sum += data[i, j];

            //    }
            //}
            //Ave = Sum / (imgROI.ImageData.GetLength(0) * imgROI.ImageData.GetLength(1));
            //for (int i = 0; i < data.GetLength(0); i++)
            //{
            //    for (int j = 0; j < data.GetLength(1); j++)
            //    {
            //        Variance += (data[i, j] - Ave) * (data[i, j] - Ave);

            //    }
            //}
            if (RecNum > 0)
            {
                img.SNR = (float)(img.ROIMeanValue / (Math.Sqrt(sumVariance / RecNum))); //Ave / (Math.Sqrt(Variance / (imgROI.ImageData.GetLength(0) * imgROI.ImageData.GetLength(1))));
                //SNRlabel.Text = SNR.ToString("0.00");
            }
        }

        #endregion

        #region 特征点提取算法
        #endregion
    }
}
