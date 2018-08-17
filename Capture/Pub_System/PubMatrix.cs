using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ImageCapturing
{
    public class PubMatrix
    {
        /// <summary>
        /// 判断要Clone的矩形对象是否有效,并在有限的范围内进行校正.无效返回false
        /// </summary>
        /// <returns>是否有效</returns>
        public static bool ReviseImageRect(Size bmSize, ref RectangleF cloneRectF)
        {
            if (cloneRectF.X >= bmSize.Width)
            {
                return false;
            }
            if (cloneRectF.Y >= bmSize.Height)
            {
                return false;
            }
            if (cloneRectF.Width < 1)
            {
                return false;
            }
            if (cloneRectF.Height < 1)
            {
                return false;
            }
            if (cloneRectF.X < 0)
            {
                cloneRectF.X = 0;
            }
            if (cloneRectF.Y < 0)
            {
                cloneRectF.Y = 0;
            }
            int lx = (int)(Math.Ceiling(cloneRectF.X));
            if (lx + cloneRectF.Width > bmSize.Width)
            {
                cloneRectF.Width = bmSize.Width - lx;
            }
            int ly = (int)(Math.Ceiling(cloneRectF.Y));
            if (ly + cloneRectF.Height > bmSize.Height)
            {
                cloneRectF.Height = bmSize.Height - ly;
            }
            return true;
        }

        public static RectangleF TransformRectangle(RectangleF srcRec, Matrix matrix)
        {
            PointF[] pts = new PointF[] { new PointF(srcRec.X, srcRec.Y), 
                new PointF(srcRec.X + srcRec.Width, srcRec.Y + srcRec.Height) };
            matrix.TransformPoints(pts);
            RectangleF rec = RectangleF.Empty;
            rec.Location = pts[0];
            rec.Size = new SizeF((pts[1].X - pts[0].X), (pts[1].Y - pts[0].Y));
            return rec;
        }

        public static Point TransformPoint(Point p, Matrix matrix)
        {
            Point[] pts = new Point[] { new Point(p.X, p.Y) };
            matrix.TransformPoints(pts);
            return pts[0];
        }
    }
}
