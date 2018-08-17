using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Collections;
using System.Drawing;
using System.IO;

namespace ImageCapturing
{
    public class TrackerPointState
    {
        public int road = 0;
        public int RoadNum = 0;
        public int positionIndex;
        public TrackerPointState(int Index)
        {
            positionIndex = Index;
        }
    }
    public class BoundaryTracker
    {
        private int[,] m_data = null; // image data.
        private int m_size;             // the length of data.
        private int m_rows;             // rows of image data
        private int m_cols;             // cols of image data

        private int BreakNum;
        private int OffsetX = 0;
        private int OffsetY = 0;

        private const int INNER = 1;
        private const int BACKP = 0;
        private const int BOUND = 1;

        private int MaxPointNum = 0;
        public int MaxPointIdx = -1; 
        public List<PointF[]> CL = null;
        private int[,] step = new int[8, 2] { { -1, 0 }, { 0, 1 },{ -1, 1 },  { 1, 0 },{ 1, 1 },  { 1, -1 }, { 0, -1 }, { -1, -1 } };

        public BoundaryTracker()
        {
            CL = new List<PointF[]>();
        }

        public void Dispose()
        {
            m_data = null;
            if (CL != null)
            {
                for (int i = 0; i < CL.Count; i++)
                {
                    CL[i] = null;
                }
                CL.Clear();
                CL = null;
            }
            step = null;
        }

        // Description: stat the sum of value of the specialized pixel. If sum is bigger than s special value, return.
        // return: return the sum of value of the specialized pixel.
        //   Algorithm:
        //            ***
        //    		  *X*
        //			  ***
        // The pMatrix points to pixel 'X'. Sum up the value of pixel from the left-up one to right-bottom one.
        // In this process, if sum is bigger than 'InnerValue', stop and return 'sum', or ,go on to the last.
        private int Sum(int r, int c)
        {
            int sum = 0;
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {                    
                    sum += m_data[r+i, c+j];
                }
                if (sum > INNER)
                {
                    return sum;
                }
            }
            return sum;
        }

        // Description: check the line whether is closed line.
        // return: If closed ,return true, or false.
        //
        // Algorithm: map:  6
        //             ****************
        //             ******XX********
        //             *****X**X******* 3
        //			   ******X**X******
        //			   *******XXX******
        // The pMatrix points to pixel(6,3). In this map , the pixel is the final pixel of current line. And ,it's connected to the
        // first pixel of this line , so ,return true.
        // If the start point of the same line is in the domain of pMatrix, the line is closed.
        private bool IsClosed(int sr, int sc, int r, int c)
        {
            int dr = sr - r;
            int dc = sc - c;
            return (dr >= -1 && dr <= 1 && dc >= -1 && dc <= 1);          
        }

        private void beginTreat()
        {
            OffsetX -= 1;
            OffsetY -= 1;
            //多加一行一列，后面还要外扩一行一列
            m_data = new int[m_rows + 2 + 1, m_cols + 2 + 1];

            // first ,set the extended element to be 'BACKGROUNDPOINT'
            int L = m_cols + 2;
            for (int i = 0; i < m_rows + 3; i++)
            {
                m_data[i, 0] = BACKP;
                m_data[i, L - 1] = BACKP;
                m_data[i, L] = BACKP;
            }
            L = m_rows + 2;
            for (int i = 1; i < m_cols + 2; i++)
            {
                m_data[0, i] = BACKP;
                m_data[L - 1, i] = BACKP;
                m_data[L, i] = BACKP;
            }
        }

        private void endTreat()
        {
            // 给左侧和下侧外扩一个像素
            int[,] tmpVMatrix = (int[,])m_data.Clone();
            for (int i = 0; i < m_rows; i++)
            {
                for (int j = 0; j < m_cols; j++)
                {
                    if (tmpVMatrix[i + 1, j + 1] != BOUND)
                    {
                        continue;
                    }
                    if (tmpVMatrix[i + 1, j + 2] != BOUND)
                    {
                        m_data[i + 1, j + 2] = BOUND;
                    }

                    if (tmpVMatrix[i + 1, j] == BOUND && tmpVMatrix[i + 2, j + 1] != BOUND)
                    {
                        m_data[i + 2, j + 1] = BOUND;
                    }
                }
            }
            m_cols++;
            m_rows++;
            tmpVMatrix = null;
            //if a element of the valueMatrix is under the following condition:
            //                 1
            //              1  1 1     (the middle '1' is the element.)
            //                 1
            //this element is not the boundary of a area, so , set it to be 'BACKGROUNDPOINT';
            tmpVMatrix = (int[,])m_data.Clone();
            for (int i = 1; i < m_rows - 1; i++)
            {
                for (int j = 1; j < m_cols - 1; j++)
                {
                    if (m_data[i + 1, j + 1] != BOUND)
                    {
                        continue;
                    }

                    if (tmpVMatrix[i, j + 1] == BOUND
                        && tmpVMatrix[i + 2, j + 1] == BOUND
                        && tmpVMatrix[i + 1, j] == BOUND
                        && tmpVMatrix[i + 1, j + 2] == BOUND)
                    {
                        m_data[i + 1, j + 1] = BACKP;
                    }
                }
            }
            tmpVMatrix = null;
            //GC.Collect();
        }

        // prepare the value matrix. if the data if in the value +/- delta, set
        // these corresponding bolMatrix elements to be 0, or set to be 1.
        // if the number of  elements which has value of 0 in the valueMatrix
        // is less than 5, it's consider as no contour line.
        // NOTE: for the consideration of speed , the value Matrix and the temp
        // value Matrix are extended by the one on the rim:
        //    data  matrix:        extended value Matrix
        //                         000000000000000000000
        //  xxxxxxxxxxxxxxxxx      0yyyyyyyyyyyyyyyyyyy0
        //  xxxxxxxxxxxxxxxxx      0yyyyyyyyyyyyyyyyyyy0
        //  xxxxxxxxxxxxxxxxx      0yyyyyyyyyyyyyyyyyyy0
        //                         000000000000000000000
        // the extended elements of the value matrix are set to be 'BACKGROUNDPOINT' constantly.
        private void ValueMatrix(float[,] src, float value, float delta)
        {
            beginTreat();

            // if the pData is between value+/- delta, set pMatrix to be ' BOUNDARYPOINT', or BACKGROUNDPOINT
            float dif = 0;
            for(int i = 0 ; i < m_rows; i++)
            {
                for(int j = 0; j < m_cols; j++)
                {
                    dif = src[i, j] - value;
                    m_data[i+1, j+1] = (dif <= delta && dif >= -delta) ? BOUND : BACKP;                    
                }
            }

            endTreat();
        }

        private void ValueMatrix(ref bool[,] src)
        {
            beginTreat();

            for (int i = 0; i < m_rows; i++)
            {
                for (int j = 0; j < m_cols; j++)
                {
                    m_data[i + 1, j + 1] = (src[i, j]) ? BOUND : BACKP;
                }
            }
            src = null;
            endTreat();
        }

        private void AddOutline(List<PointF> tmp)
        {
            PointF[] pts = tmp.ToArray();
            for (int i = 0; i < pts.Length; i++)
            {
                pts[i].X += OffsetX;
                pts[i].Y += OffsetY;
            }
            CL.Add(pts);

            if(pts.Length > MaxPointNum)
            {
                MaxPointIdx = CL.Count - 1;
                MaxPointNum = pts.Length;
            }
        }

        private void Excuse()
        {
            MaxPointNum = 0;
            MaxPointIdx = -1;
            bool isCircle = true;
            Stack<TrackerPointState> stateStack = new Stack<TrackerPointState>();
            CL.Clear();
            List<PointF> tmpCL = new List<PointF>();

            // now, the element in the valueMatrix has value 'BOUNDARYPOINT' is the boundary element of line.
            // search for contour lines:
            int startRow = 1;
            int startCol = 1;
            bool bNotExist = false;
            int colLen = m_data.GetLength(0);//每列长度
            int rowLen = m_data.GetLength(1);//每行长度
            while (!bNotExist)
            {
                #region Find Start Point
                // search for the start point of each contour line
                // if there is no point that value +/- delta exists in the data,
                // there is no more contour line exists:
                int row = startRow;
                int col = 1;
                bool bFound = false;
                while (!bFound && row <= m_rows) // the contour line will be omitted if all points of it are on the last col,
                {                               // so ,the break condition is m_rows -1.
                    for (col = 1; col <= m_cols; col++)
                    {
                        if (m_data[row, col] == BOUND)
                        {// find the start point of a new contour line.
                            bFound = true;
                            break;
                        }
                    }
                    row++;
                }

                if (!bFound)
                {// no more contour line:
                    break;
                }

                if (Sum(row, col) == 0)
                {// it's alone:
                    m_data[row, col] = BACKP;
                    continue;
                }

                // deal with the start point:
                startCol = col;
                startRow = row;
                #endregion

                #region Search Outline
                // from first point search all the point of current contour line:
                int ptNum = row * rowLen + col; //0
                tmpCL.Clear();
                tmpCL.Add(new PointF(col, row));
                m_data[row, col] = BACKP;
                int summ = 0;
                while (!bNotExist)
                {
                    // case 1: is the last point of closed line.
                    // case 2: is an end of line.
                    // case 3: not the end, not the last point of closed line. 
                    summ = Sum(row, col);  
                    if (IsClosed(startRow, startCol, row, col) && tmpCL.Count > 6) //if points is less than 6, it's a not really a closed string.
                    {// case 1: is the last point of closed line:                         
                        stateStack.Clear();
                        isCircle = true;
                        tmpCL.Add(new PointF(startCol, startRow));
                        AddOutline(tmpCL);
                        tmpCL.Clear();
                        break; // finish the search of this line.  
                    }
                    else if ( summ == 0)
                    {// case 2: is an end of line.
                        if (isCircle && stateStack.Count > 0)
                        {
                            bool findRoad = false;
                            while (!findRoad)
                            {
                                TrackerPointState s = stateStack.Peek();
                                PointF p = tmpCL[s.positionIndex];
                                row = (int)p.Y;
                                col = (int)p.X;
                                while(tmpCL.Count - 1 > s.positionIndex)
                                {
                                    PointF pi = tmpCL[tmpCL.Count - 1];
                                    m_data[(int)pi.Y, (int)pi.X] = BOUND;
                                    tmpCL.RemoveAt(tmpCL.Count - 1);
                                }
                                if (s.road == s.RoadNum)
                                {
                                    s = stateStack.Pop();
                                    if (stateStack.Count == 0)
                                    {
                                        isCircle = false;
                                        findRoad = true;
                                        break;
                                    }
                                    continue;
                                }
                                else
                                {
                                    s.road ++;
                                    findRoad = true;
                                }                
                            }
                        }
                        else
                        {
                            //BmpToCurve.showPiont(m_data);
                            AddOutline(tmpCL);
                            tmpCL.Clear();
                            isCircle = true;
                            break;
                        }  
                    }
                    else
                    {// case 3: not the end, not the last point of closed line.
                        if (isCircle)
                        {
                            int road = 0;
                            if (summ != 1)
                            {
                                bool addState = true;
                                TrackerPointState s = null;
                                if (stateStack.Count > 0)
                                {
                                    s = stateStack.Peek();
                                    PointF p = tmpCL[s.positionIndex];
                                    if (col == (int)p.X && row == (float)p.Y && summ == s.RoadNum)
                                    {
                                        addState = false;           
                                        road = s.road;
                                    }
                                }
                                if(addState)
                                {
                                    TrackerPointState state = new TrackerPointState(tmpCL.Count - 1);
                                    state.RoadNum = summ;
                                    state.road = 1;
                                    road = state.road;
                                    stateStack.Push(state); 
                                }
                            }
                            
                            int curRoad = 0;
                            for (int s = 0; s < 8; s++)
                            {
                                int nr = row + step[s, 0];
                                int nc = col + step[s, 1];
                                if (nr < 1 || nr > m_rows || nc < 1 || nc > m_cols)
                                {
                                    continue;
                                }
                                if (m_data[nr, nc] == BOUND)
                                {
                                    if (summ != 1)
                                    {
                                        curRoad++;
                                        if (road != curRoad)
                                        {
                                            continue;
                                        }
                                    }
                                    row = nr;
                                    col = nc;
                                    tmpCL.Add(new PointF(col, row));
                                    m_data[row, col] = BACKP;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int s = 0; s < 8; s++)
                            {
                                int nr = row + step[s, 0];
                                int nc = col + step[s, 1];
                                if (nr < 1 || nr > m_rows || nc < 1 || nc > m_cols)
                                {
                                    continue;
                                }
                                if (m_data[nr, nc] == BOUND)
                                {
                                    row = nr;
                                    col = nc;
                                    tmpCL.Add(new PointF(col, row));
                                    m_data[row, col] = BACKP;
                                    break;
                                }
                            }
                        }
                        if (ptNum > BreakNum) // in the case of infinite loop.
                        {
                            bNotExist = true;
                        }
                    }
                    ptNum++;//2009/04/08 暂时添加保护
                }
                #endregion

                tmpCL.Clear();
            }
        }

        // Description: Get serialized boundary of object specialized by value, delta from image.
        //   value: the value of object to track in image.
        //   delta: the error of value.
        //   skip:  if the distance of two line is less than 'skip', joint them.
        //   Dependence: IsClosed ,Sum, and  ValueMatrix
        //   return: the serialized boundary point
        public void GetSerializedBoundary(float[,] data, float value, float delta, int skip)
        {
	        if(data == null)return;

            OffsetX = 0;
            OffsetY = 0;
            
            m_rows = data.GetLength(0);
	        m_cols = data.GetLength(1);
	        m_size = data.Length;
	        BreakNum = m_size;

	        // parameter check:
            if(skip < 0) skip = 0;
	        if(delta <0) delta = -delta;

            // get the value matrix:
            ValueMatrix(data, value, delta);

            Excuse();
        }

        public void GetSerializedBoundary(bool[,] data)
        {
            if (data == null) return;

            OffsetX = 0;
            OffsetY = 0;

            m_rows = data.GetLength(0);
            m_cols = data.GetLength(1);
            m_size = data.Length;
            BreakNum = m_size;
                        
            // get the value matrix:
            ValueMatrix(ref data);

            Excuse();
        }

        public List<PointF> taxis(List<Point> list)
        {
            if (list.Count == 0) return null;
            int _Size = 0;
            foreach (Point p in list)
            {
                if(p.X > _Size) _Size = p.X;
                if(p.Y > _Size) _Size = p.Y;
            }
            _Size++;
            m_data = new int[513, 513];
            foreach (Point p in list)
            {
                int j = p.Y;
                int i = p.X;
                m_data[j, i] = BOUND;
            }

            m_rows = m_data.GetLength(0) - 1;
            m_cols = m_data.GetLength(1) - 1;
            BreakNum = m_data.Length;

            Excuse();

            List<PointF> pt = new List<PointF>();
            if (CL != null && CL.Count >0)
            {
                pt.AddRange(CL[0]);
            }
            return pt;
        }

        public bool GetSerializedBoundary(Bitmap source, Color DrawColor)
        {
            return GetSerializedBoundary(source, DrawColor, new Rectangle(0, 0, source.Width, source.Height));
        }

        public bool GetSerializedBoundary(Bitmap source, Color DrawColor, bool v)
        {
            return GetSerializedBoundary(source, DrawColor, new Rectangle(0, 0, source.Width, source.Height),false);
        }

        /// <summary>
        ///  Load image data from bitmap.
        /// </summary>
        /// <param name="source">the source bitmap</param>
        /// <returns>if load successfully, return true, or flase.</returns>
        unsafe public bool GetSerializedBoundary(Bitmap source, Color DrawColor, Rectangle Range)
        {
            if (source == null || source.Width < 2 || source.Height < 2)
            {// there is no enough data to track.
                return false;
            }

            OffsetX = Range.X;
            OffsetY = Range.Y;

            int bytes, offset;
            byte* pR, pG, pB;

            System.Drawing.Imaging.BitmapData bmpData = new System.Drawing.Imaging.BitmapData();

            // load bitmap information:
            int _height = source.Height;
            int _width = source.Width;

            try
            {//try is  for the case of gif format image and so on.
                Rectangle rect = new Rectangle(0, 0, _width, _height);
                bmpData = source.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, source.PixelFormat);
            }
            catch
            {// if fail to copy data:                    
                source.UnlockBits(bmpData);
                throw new Exception("Load image failed! The image can not be resolved!");
            }

            // set pointer to RGB channel 
            bytes = (int)bmpData.Stride / _width;                                    // get the bit type of bitmap.
            if (bytes == 4 || bytes == 3)                                                 // only deal with 8bits 24bits and 32 bytes.
            {
                pB = (byte*)bmpData.Scan0;
                pG = pB + 1;
                pR = pG + 1;
            }
            else if (bytes == 1)
            {
                pG = pB = pR = (byte*)bmpData.Scan0;
            }
            else
            {// not deal with these case yet.
                source.UnlockBits(bmpData);
                return false;
            }

            // copy image data to data array:
            offset = bmpData.Stride - bytes * _width;
            bool[,] _data = null;
            try
            {
                _data = new bool[Range.Height, Range.Width];
            }
            catch (System.Exception e)
            {
                source.UnlockBits(bmpData);
                return false;	
            }

            int numRow = Range.Y * (bytes * _width + offset);
            int numColA = bytes * Range.X;
            int numColB = bytes * (_width - Range.Width - Range.X) + offset;

            pR += numRow; //偏移到第M行
            pG += numRow;
            pB += numRow;
            for (int i = 0; i < Range.Height; i++)
            {
                pR += numColA; //偏移到第N列
                pG += numColA;
                pB += numColA;
                for (int j = 0; j < Range.Width; j++)
                {
                    _data[i, j] = (*pR == DrawColor.R) && (*pG == DrawColor.G) && (*pB == DrawColor.B);
                    pR += bytes;
                    pG += bytes;
                    pB += bytes;                        
                }
                pR += numColB;//偏移到下一行 
                pG += numColB;
                pB += numColB;
            }
            
            // free any reference to any object:
            source.UnlockBits(bmpData);
            pR = pG = pB = null;
            bmpData = null;

            // set data:
            m_rows = _data.GetLength(0);
            m_cols = _data.GetLength(1);
            m_size = _data.Length;
            BreakNum = m_size;

            // get the value matrix:
            ValueMatrix(ref _data);
            Excuse();
            return true;
        }

        /// <summary>
        ///  Load image data from bitmap.
        /// </summary>
        /// <param name="source">the source bitmap</param>
        /// <returns>if load successfully, return true, or flase.</returns>
        unsafe public bool GetSerializedBoundary(Bitmap source, Color DrawColor, Rectangle Range,bool v)
        {
            if (source == null || source.Width < 2 || source.Height < 2)
            {// there is no enough data to track.
                return false;
            }

            OffsetX = Range.X;
            OffsetY = Range.Y;

            int bytes, offset;
            byte* pR, pG, pB;

            System.Drawing.Imaging.BitmapData bmpData = new System.Drawing.Imaging.BitmapData();

            // load bitmap information:
            int _height = source.Height;
            int _width = source.Width;

            try
            {//try is  for the case of gif format image and so on.
                Rectangle rect = new Rectangle(0, 0, _width, _height);
                bmpData = source.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, source.PixelFormat);
            }
            catch
            {// if fail to copy data:                    
                source.UnlockBits(bmpData);
                throw new Exception("Load image failed! The image can not be resolved!");
            }

            // set pointer to RGB channel 
            bytes = (int)bmpData.Stride / _width;                                    // get the bit type of bitmap.
            if (bytes == 4 || bytes == 3)                                                 // only deal with 8bits 24bits and 32 bytes.
            {
                pB = (byte*)bmpData.Scan0;
                pG = pB + 1;
                pR = pG + 1;
            }
            else if (bytes == 1)
            {
                pG = pB = pR = (byte*)bmpData.Scan0;
            }
            else
            {// not deal with these case yet.
                source.UnlockBits(bmpData);
                return false;
            }

            // copy image data to data array:
            offset = bmpData.Stride - bytes * _width;
            bool[,] _data = null;
            try
            {
                _data = new bool[Range.Height, Range.Width];
            }
            catch (System.Exception e)
            {
                source.UnlockBits(bmpData);
                return false;
            }

            int numRow = Range.Y * (bytes * _width + offset);
            int numColA = bytes * Range.X;
            int numColB = bytes * (_width - Range.Width - Range.X) + offset;

            pR += numRow; //偏移到第M行
            pG += numRow;
            pB += numRow;
            for (int i = 0; i < Range.Height; i++)
            {
                pR += numColA; //偏移到第N列
                pG += numColA;
                pB += numColA;
                for (int j = 0; j < Range.Width; j++)
                {
                    _data[i, j] = !((*pR == DrawColor.R) && (*pG == DrawColor.G) && (*pB == DrawColor.B));
                    pR += bytes;
                    pG += bytes;
                    pB += bytes;
                }
                pR += numColB;//偏移到下一行 
                pG += numColB;
                pB += numColB;
            }

            // free any reference to any object:
            source.UnlockBits(bmpData);
            pR = pG = pB = null;
            bmpData = null;

            // set data:
            m_rows = _data.GetLength(0);
            m_cols = _data.GetLength(1);
            m_size = _data.Length;
            BreakNum = m_size;

            // get the value matrix:
            ValueMatrix(ref _data);
            Excuse();
            return true;
        }
    }
}
