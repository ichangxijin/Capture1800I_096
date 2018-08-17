using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.Imaging;

namespace ImageCapturing
{
    /// <summary>
    /// 多网格小图控件
    /// </summary>
    public partial class UserControlMutiImages : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserControlMutiImages()
        {
            InitializeComponent();
        }

        #region Varibles
        /// <summary>
        /// 通用委托,委托用作应用程序接收到消息时封装事件的方式
        /// </summary>
        /// <param name="Sender">对象</param>
        /// <param name="e">事件</param>
        public delegate void EventChanged(object Sender, EventArgs e);

        /// <summary>
        /// 改变网格单元格索引时发生的事件
        /// </summary>
        public event EventChanged ImageNumChanged;

        public PictureBox mPicturebox
        {
            get { return pictureBoxImages; }
            set { pictureBoxImages = value; }
        }

        /// <summary>
        /// 片子的数量
        /// </summary>
        public int Num = 0;

        protected float SmallImageW = 81;
        protected float SmallImageH = 81;
        protected int AllRow = 2;	      //总行数	
        protected int mPreImageIndex = int.MinValue;  // 保存前面选中的图片索引
        protected Color cl = Color.FromArgb(192, 192, 192);//初始网格的颜色
        protected int IniLX = 0;//-1;//设置初始PanelImages的Location位置
        protected int IniLY = 0;//-1;//-2;
        protected Form fmParent; //控件的父窗体  
        protected List<Bitmap> Bits;
        ///// <summary>
        ///// the imgs shown row number
        ///// </summary>
        //protected int CountFlag = 0;
        /// <summary>
        /// the row index of current image. due to the grid
        /// the first value is 0
        /// </summary>
        protected int ShowRowIndex = 0;
        /// <summary>
        /// the column index of current image. due to the grid
        /// the first value is 0
        /// </summary>
        protected int ShowColumnIndex = 0;

        protected int OffH = 2;
        protected int OffW = 2;

        /// <summary>
        /// 字符串显示格式
        /// </summary>
        StringFormat drawFormat;
        /// <summary>
        /// 字体
        /// </summary>
        Font drawFont;
        /// <summary>
        /// 网格颜色
        /// </summary>
        SolidBrush drawBrush;
        /// <summary>
        /// Image的填充颜色
        /// </summary>
        Color mImageBackColor = Color.Black;

        #endregion

        #region Events
        //设置初始网格，没有图片
        private void UserControlMutiImages_Load(object sender, EventArgs e)
        {
            fmParent = ControlPanel.FindForm();//检索控件所在的窗体
            if (fmParent != null)
            {
                fmParent.KeyPreview = true;
                fmParent.KeyUp += new KeyEventHandler(fmParent_KeyUp);
                fmParent.KeyPress += new KeyPressEventHandler(fmParent_KeyPress);
                if (mMouseWheelEnable)
                {
                    fmParent.MouseWheel += new MouseEventHandler(fmParent_MouseWheel);
                }
            }
            this.pictureBoxImages.MouseEnter += new EventHandler(pictureBoxImages_MouseEnter);

            SetIniSize(Num);
        }

        #region Key Events

        private void fmParent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.pictureBoxImages.Focused) // 只有当前有焦点输入时，才可以使用此功能；
            {
                ControlPanel_KeyPress(sender, e);
            }
        }

        protected virtual void fmParent_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.pictureBoxImages.Focused) // 只有当前有焦点输入时，才可以使用此功能；
            {
                ControlPanel_KeyUp(sender, e);
            }
            if (UseDirectionKeyPress)
            {
                this.pictureBoxImages.Focus();
            }
        }

        private void ControlPanel_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (!mUseDirectionKeyPress)
            {
                SetImageByKey(e.KeyData);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (mUseDirectionKeyPress)
            {
                SetImageByKey(keyData);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 按下按键时触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 2008/05/16 add
        private void ControlPanel_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (mUseDirectionKeyPress) return;

            Char Temp = e.KeyChar;
            if (Temp == 's')  //S Down
            {
                GetKeyLocation("DOWN");
            }
            else if (Temp == 'a')  //A Left
            {
                GetKeyLocation("LEFT");
            }
            else if (Temp == 'd')  //D Right
            {
                GetKeyLocation("RIGHT");
            }
            else if (Temp == 'w')  //W Up
            {
                GetKeyLocation("UP");
            }
        }

        protected void fmParent_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.pictureBoxImages.Focused)
            {
                ControlPanel_MouseWheel(sender, e);
            }
        }

        /// <summary>
        /// 滚轮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ControlPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int MouseData = e.Delta;

            if (MouseData > 0)
            {
                GetKeyLocation("LEFT");
            }
            else if (MouseData < 0)
            {
                GetKeyLocation("RIGHT");
            }
        }

        #endregion

        protected virtual void pictureBoxImages_MouseDown(object sender, MouseEventArgs e)
        {
            int w = btnPreviousLine1.Width;
            int h = btnNextLine1.Location.Y - btnPreviousLine1.Location.Y + btnNextLine1.Height;
            Rectangle rectUL = new Rectangle(btnPreviousLine1.Location, new Size(w, h));
            Rectangle rectDR = new Rectangle(btnPreviousLine2.Location, new Size(w, h));

            if (e.Button == MouseButtons.Left && !rectUL.Contains(e.Location)
                && !rectDR.Contains(e.Location))
            {
                if (CheckImageIndexChanged(e.X, e.Y, mSingleSelect))
                {
                    SetImageCursor(true, false, true);
                    //SetImageCursor(e.X, e.Y);
                }
            }
            pictureBoxImages.Focus();
        }

        protected bool CursorInGrid(int x, int y, int GridSN)
        {
            return false;
        }

        void pictureBoxImages_MouseEnter(object sender, EventArgs e)
        {
            this.pictureBoxImages.Focus();
        }

        /// <summary>
        /// 按下鼠标后移动，动态选择图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void pictureBoxImages_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (CheckImageIndexChanged(e.X, e.Y, mSingleSelect))
                {
                    //SetImageCursor(e.X, e.Y);
                    SetImageCursor(true, false, true);
                }
            }
            //if (e.Button == MouseButtons.None) // 鼠标没有按下，移动时，判断是否显示左上角的PreviousLine和右下角的NextLine两个按钮
            //{
            //    int ey = e.Y + pictureBoxImages.Location.Y;
            //    int ex = e.X + pictureBoxImages.Location.X;
            //    if (mColumn * mRow < Num
            //    && ex >= btnPreviousLine1.Location.X && ey >= btnPreviousLine1.Location.Y
            //    && ex <= btnNextLine1.Location.X + btnNextLine1.Size.Width + 40
            //    && ey <= btnNextLine1.Location.Y + btnNextLine1.Size.Height + 15)
            //    {
            //        btnPreviousLine1.Visible = true;
            //        btnNextLine1.Visible = true;
            //        //alphaTop = 255;
            //    }
            //    else if (mColumn * mRow < Num
            //    && ex >= btnPreviousLine2.Location.X - 40 && ey >= btnPreviousLine2.Location.Y - 15
            //    && ex <= btnNextLine2.Location.X + btnNextLine2.Size.Width
            //    && ey <= btnNextLine2.Location.Y + btnNextLine2.Size.Height)
            //    {
            //        btnPreviousLine2.Visible = true;
            //        btnNextLine2.Visible = true;
            //    }
            //}
        }

        private void UserControlMutiImages_SizeChanged(object sender, EventArgs e)
        {
            if (Bits == null)
            {
                return;
            }
            this.DrawImagesInPane(Bits, false);
            SetBtnLocation();
        }

        // Move to Previous line 
        private void btnPreviousLine_Click(object sender, EventArgs e)
        {
            GetKeyLocation("PageUp");
        }

        // Move to Next line 
        private void btnNextLine_Click(object sender, EventArgs e)
        {
            GetKeyLocation("PageDown");
        }

        private void btnDirection_Enter(object sender, EventArgs e)
        {
            this.pictureBoxImages.Focus();
        }

        #endregion

        #region Functions

        #region Public Functions

        public void Dispose()
        {
            if (drawFont != null) drawFont.Dispose();
            if (drawBrush != null) drawBrush.Dispose();
            if (drawFormat != null) drawFormat.Dispose();
        }

        /// <summary>
        /// 向方格中画入图片 
        /// </summary>
        /// <param name="bitmaps">要画的图片列表</param>
        /// <param name="selectIndex">初始要选中的网格索引</param>
        public void DrawImagesInPane(List<Bitmap> bitmaps, int selectIndex)
        {
            if (bitmaps == null || bitmaps.Count == 0)
            {
                mSelectedIndex = -1;
            }
            if (selectIndex >= bitmaps.Count)
            {
                mSelectedIndex = bitmaps.Count - 1;
            }
            else
            {
                mSelectedIndex = selectIndex;
            }
            //SetShow_RowColumn(selectIndex);

            DrawImagesInPane(bitmaps, true);
        }

        /// <summary>
        /// 向方格中画入图片
        /// </summary>
        /// <param name="bitmaps">要画的图片列表</param>
        /// <param name="selectEvent">是否执行选中事件</param>
        public void DrawImagesInPane(List<Bitmap> bitmaps, bool selectEvent)
        {
            if (mSelectedIndex < 0)
            {
                mSelectedIndex = bitmaps.Count - 1;
            }

            Bits = bitmaps;
            if (Bits != null)
            {
                Num = Bits.Count;// 获得长度 
            }
            else
            {
                Num = 0;
            }
            SetShow_RowColumn(mPreImageIndex);//0);
            SetIniSize(Num);

            drawFont = new Font("Arial", 8);
            drawBrush = new SolidBrush(cl);
            drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            //if (mSelectedIndex != mPreImageIndex || mSelectedIndex < 1)//
            //{
            //    ShowRowIndex = 0;
            //    ShowColumnIndex = 0;
            //}
            //SetShow_RowColumn(mSelectedIndex);//0);

            DrawImages(Bits, selectEvent);
        }

        /// <summary>
        /// 向方格中画入图片 
        /// 小方格中Image大小为 SmallImageW - 5。 
        /// </summary>
        /// <param name="bitmaps">要画的图片列表</param>
        public void DrawImagesInPane(List<Bitmap> bitmaps)
        {
            DrawImagesInPane(bitmaps, true);
        }

        /// <summary>
        /// 用来通过代码控制mImageNum的改变
        /// </summary>
        /// <param name="NumForward">True为正向移动， False为逆向移动</param> 
        public void ChangeImageNum(string direction)
        {
            GetKeyLocation(direction);
        }

        /// <summary>
        /// 清空图片
        /// </summary>
        public void Images_Clear()
        {
            pictureBoxImages.Image = null;
            Num = 0;
            ShowColumnIndex = 0;
            ShowRowIndex = 0;
            mSelectedIndex = -1;
            mPreImageIndex = -1;
            SetIniSize(Num);
            pictureBoxImages.Refresh();
        }
        #endregion

        #region Private Function

        protected void BrushGrid(int ImageIndex)
        {
            if (ImageIndex < 0 || ImageIndex > Num - 1) return;
            Graphics graphics = Graphics.FromImage(pictureBoxImages.Image);
            int NumCol = GetShowColumn(ImageIndex);
            int NumRow = GetShowRow(ImageIndex);

            int OffSet = 2; // 亮框离网格边线的距离
            int X1 = (int)(NumCol * SmallImageW + OffW / 2) + OffSet / 2;
            int Y1 = (int)(NumRow * SmallImageH + OffH / 2) + OffSet / 2;
            int X2 = (int)((NumCol + 1) * SmallImageW + OffW / 2) - OffSet / 2;
            int Y2 = (int)((NumRow + 1) * SmallImageH) + OffH / 2 - OffSet / 2;
            //int X1 = (int)(NumCol * SmallImageW + OffW / 2);
            //int Y1 = (int)(NumRow * SmallImageH + OffH / 2);
            //int X2 = (int)((NumCol + 1) * SmallImageW + OffW / 2);
            //int Y2 = (int)((NumRow + 1) * SmallImageH + OffH / 2);
            Rectangle Rect = new Rectangle(new Point(X1, Y1), new Size(X2 - X1, Y2 - Y1));
            Brush b = new SolidBrush(mImageBackColor);
            graphics.FillRectangle(b, Rect);
            graphics.Flush();
            b.Dispose();
            graphics.Dispose();
        }

        /// <summary>
        /// 给指定索引的小网格画亮框
        /// </summary>
        /// <param name="ImageNum">小网格索引</param>
        protected void DrawGridCursor(int ImageIndex)
        {
            if (ImageIndex < 0 || ImageIndex > Num - 1) return;

            Graphics graphics = Graphics.FromImage(pictureBoxImages.Image);
            int NumCol = GetShowColumn(ImageIndex);
            int NumRow = GetShowRow(ImageIndex);
            Pen sPen = new Pen(mSelectGridColor);
            sPen.Width = mSelectPenWidth;// 2.0f;
            int OffSet = mSelectPenWidth;// 2; // 亮框离网格边线的距离
            int x1 = (int)(NumCol * SmallImageW + OffW / 2) + OffSet;
            int y1 = (int)(NumRow * SmallImageH + OffH / 2) + OffSet;
            int x2 = (int)((NumCol + 1) * SmallImageW + OffW / 2) - OffSet;
            int y2 = (int)((NumRow + 1) * SmallImageH) + OffH / 2 - OffSet;

            Rectangle LightRec = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            graphics.DrawRectangle(sPen, LightRec);
            graphics.Dispose();
            sPen.Dispose();

            //if (refresh) this.pictureBoxImages.Refresh();
        }

        /// <summary>
        /// 用指定颜色在小网格中画出小网格的索引
        /// </summary>
        /// <param name="ImageNum">小网格索引</param>
        /// <param name="fontColor">字的颜色</param>
        protected void DrawGridIndex(int ImageIndex, Color fontColor)
        {
            if (ImageIndex < 0 || ImageIndex > Num - 1) return;
            if (!mShowImageIndex) return;
            Graphics graphics = Graphics.FromImage(pictureBoxImages.Image);
            int NumCol = GetShowColumn(ImageIndex);
            int NumRow = GetShowRow(ImageIndex);
            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(fontColor);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            int OffSmallImageBoder = 4;        // 图片轮廓每边移入小网格的距离 
            int X1 = (int)(NumCol * SmallImageW + OffW / 2) + OffSmallImageBoder;
            int Y1 = (int)(NumRow * SmallImageH + OffH / 2) + OffSmallImageBoder;
            int X2 = (int)SmallImageW - 2 * OffSmallImageBoder;
            int Y2 = (int)SmallImageH - 2 * OffSmallImageBoder;
            Rectangle destRect = new Rectangle(X1, Y1, X2, Y2);
            graphics.DrawString((ImageIndex + 1).ToString(), drawFont, drawBrush, destRect, drawFormat);

            if (!mSingleSelect)
            {
                int CheckRecSize = 18;
                if (SmallImageW < CheckRecSize * 3 || SmallImageH < CheckRecSize * 3)
                {
                    float smallWidth = SmallImageW < SmallImageH ? SmallImageW : SmallImageH;
                    CheckRecSize = (int)(smallWidth / 4.0f);
                }
                OffSmallImageBoder = 5;        // 图片轮廓每边移入小网格的距离 
                X1 = (int)(NumCol * SmallImageW + OffW / 2) + OffSmallImageBoder;
                Y1 = (int)(NumRow * SmallImageH + OffH / 2) + OffSmallImageBoder;
                destRect = new Rectangle(X1 + 18, Y1, CheckRecSize, CheckRecSize);
                graphics.DrawRectangle(new Pen(mSelectGridColor, 2.0f), destRect);
            }

            graphics.Dispose();
            drawFont.Dispose();
            drawBrush.Dispose();
            drawFormat.Dispose();
        }

        protected void DrawImageToGrid(int ImageIndex)
        {
            if (ImageIndex < 0 || ImageIndex > Num - 1) return;

            Graphics graphics = Graphics.FromImage(pictureBoxImages.Image);
            int imageW = Bits[ImageIndex].Width;
            int imageH = Bits[ImageIndex].Height;
            float OffSmallImageBoder = 2.0f;        // 图片较长一边移入小网格的距离
            float bigImageLen = imageW > imageH ? imageW : imageH;
            float smallGridLen = SmallImageW < SmallImageH ? SmallImageW : SmallImageH;
            float scale = (smallGridLen - 2.0f * OffSmallImageBoder) / bigImageLen;
            int NumCol = GetShowColumn(ImageIndex);
            int NumRow = GetShowRow(ImageIndex);
            int xMid = (int)(NumCol * SmallImageW + OffW / 2) + (int)(SmallImageW / 2);
            int yMid = (int)(NumRow * SmallImageH + OffH / 2) + (int)(SmallImageH / 2);
            float x2 = (float)(imageW * scale);
            float y2 = (float)(imageH * scale);
            int x1 = xMid - (int)(x2 / 2);
            int y1 = yMid - (int)(y2 / 2);

            Rectangle destSquare = new Rectangle(x1, y1, (int)x2, (int)y2);
            graphics.DrawImage(Bits[ImageIndex], destSquare);
            graphics.Dispose();
        }

        protected void DrawImages(List<Bitmap> bitmaps, bool selectEvent)
        {
            if (bitmaps == null) return;

            pictureBoxImages.Image = null;
            if (bitmaps.Count > 0 && bitmaps[0] != null)
            {
                this.mImageBackColor = Color.Black;// bitmaps[0].GetPixel(5, 5);
            }
            //this.mPreImageNum = 0;
            DrawMutiGrids();

            if (pictureBoxImages.Image == null) return;//防止sizeChanged时为null 2008/09/12

            int startIndex = GetStartImageIndex(mSelectedIndex);
            //Check startIndex is valid
            int otherRow = mRow - (this.Num - startIndex - 1) / mColumn - 1;
            if (otherRow > 0)
            {//Need reset ShowColumnIndex, ShowRowIndex
                ShowRowIndex += otherRow;
                startIndex -= otherRow * mColumn;
            }

            for (int y = 0; y < mRow; y++)
            {
                for (int x = 0; x < mColumn; x++)
                {
                    //int k = (y + CountFlag) * mColumn + x ;
                    int k = (y * mColumn + x) + startIndex;
                    if (k > Num - 1) return;

                    DrawImageToGrid(k);
                    DrawGridIndex(k, this.cl);
                    if (k == Num - 1)
                    {
                        SetImageCursor(selectEvent, true, true);
                        return;
                    }
                }
            }
            SetImageCursor(selectEvent, true, true);
        }

        public void DrawTagToGrid(int ImageIndex, string Tag, Color fontColor)
        {
            if (ImageIndex < 0 || ImageIndex > Num - 1) return;
            if (!mShowImageIndex) return;
            string[] str = Tag.Split(',');
            Graphics graphics = Graphics.FromImage(pictureBoxImages.Image);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            int NumCol = GetShowColumn(ImageIndex);
            int NumRow = GetShowRow(ImageIndex);
            Font drawFont = new Font("Tahoma", 25F, FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(fontColor);
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DisplayFormatControl;

            int OffSmallImageBoder = 2;        // 图片轮廓每边移入小网格的距离 
            int X1 = (int)(NumCol * SmallImageW + OffW / 2) + OffSmallImageBoder;
            int Y1 = (int)(NumRow * SmallImageH + OffH / 2) + OffSmallImageBoder;
            int X2 = (int)SmallImageW - 2 * OffSmallImageBoder;
            int Y2 = (int)SmallImageH - 2 * OffSmallImageBoder;
            Rectangle destRect = new Rectangle(X1, Y1 + 20, X2, Y2);
            //graphics.DrawEllipse(new Pen(fontColor,2), X1, Y1, 40, 40);
            graphics.DrawString(str[0], drawFont, drawBrush, destRect, drawFormat);
            if (str.Length == 2)
            {
                //graphics.DrawEllipse(new Pen(fontColor, 2), X1+40, Y1, 40, 40);
                destRect = new Rectangle(X1 + 45, Y1 + 20, X2, Y2);
                graphics.DrawString(str[1], drawFont, drawBrush, destRect, drawFormat);
            }
            graphics.Dispose();
            drawFont.Dispose();
            drawBrush.Dispose();
            drawFormat.Dispose();
            pictureBoxImages.Refresh();
        }

        /// <summary>
        /// 设置图像的初始大小和相应参数,利用SetScrollBar()设置滚动框的初始参数，调用DrawMutiGrids()画网格
        /// </summary>
        private void SetIniSize(int INum)
        {
            if (mRow < 1 || mColumn < 1) return;
            //把Image分开为N个小方格
            Num = INum;
            AllRow = 1;
            if (Num % mColumn != 0)
            {
                AllRow = (int)Num / mColumn + 1;
            }
            else
            {
                AllRow = Num / mColumn;//以上if,else为得到所有行数
            }

            if (AllRow <= mRow)
            {
                AllRow = mRow;
            }

            panelImages.Parent = ControlPanel;
            panelImages.Location = new Point(IniLX, IniLY);  //初始显示的位置
            int panelWidth = 0;
            int panelHeight = 0;

            panelWidth = ControlPanel.Width;
            panelWidth -= IniLX;
            //panelHeight -= IniLY;
            panelHeight = ControlPanel.Height;
            panelImages.Size = new Size(panelWidth, panelHeight);

            //pictureBoxImages.BackColor = Color.Black;
            pictureBoxImages.Parent = panelImages;
            //pictureBoxImages.Location = new Point(0, PicLocationY);
            pictureBoxImages.Width = panelImages.Width;
            pictureBoxImages.Height = panelImages.Height;

            SmallImageH = (pictureBoxImages.Height - OffH) / (float)mRow;     // OffH ：网格要画出上下边线每边需移入OffH/2
            SmallImageW = (pictureBoxImages.Width - OffW) / (float)mColumn;   // OffW ：网格要画出左右边线每边需移入OffW/2

            // 设置两个按钮的位置 -- Jerin.2008.2.26    ZXD 2008.03.10 Update
            SetBtnLocation();

            int count = (Bits != null) ? Bits.Count : 0;
            int startIndex = GetStartImageIndex(mSelectedIndex);
            bool lineVisible = (startIndex > 0) || (startIndex + Column - 1 < count - 1);//(Num > mRow * mColumn);
            btnPreviousLine1.Visible = lineVisible;
            btnNextLine1.Visible = lineVisible;
            btnPreviousLine2.Visible = lineVisible;
            btnNextLine2.Visible = lineVisible;
            SetBtnEnable();
        }

        private void SetBtnLocation()
        {
            int sx = 2;
            int sy = 2;
            int sy2 = 21;
            btnPreviousLine1.Location = new Point(sx, sy);//8,8
            btnNextLine1.Location = new Point(sx, sy2);//8,27
            btnPreviousLine2.Location = new Point(this.ControlPanel.Width - btnPreviousLine2.Width - sx - 1, this.ControlPanel.Height - btnPreviousLine2.Height - sy2);
            btnNextLine2.Location = new Point(this.ControlPanel.Width - btnNextLine2.Width - sx - 1, this.ControlPanel.Height - btnNextLine2.Height - sy);
        }

        /// <summary>
        /// 画方格
        /// </summary>
        private void DrawMutiGrids()
        {
            Pen sPen = new Pen(cl);
            Brush blackBrush = new SolidBrush(mImageBackColor);
            sPen.Width = 2;
            if (pictureBoxImages.Image != null)
            {
                Graphics graphics = Graphics.FromImage(pictureBoxImages.Image);
                graphics.FillRectangle(blackBrush, 0, 0, pictureBoxImages.Width, pictureBoxImages.Height);
                //全部填黑避免在vista中的灰色边框xcj07.03.23
                DrawOneGrid(graphics, sPen);
                graphics.Dispose();
            }
            else
            {
                if (pictureBoxImages.Width > 0 && this.pictureBoxImages.Height > 0)
                {
                    Bitmap S_Image = new Bitmap(pictureBoxImages.Width, pictureBoxImages.Height);
                    Graphics graphics = Graphics.FromImage(S_Image);
                    graphics.FillRectangle(blackBrush, 0, 0, S_Image.Width, S_Image.Height);
                    //全部填黑避免在vista中的灰色边框xcj07.03.23
                    DrawOneGrid(graphics, sPen);
                    pictureBoxImages.Image = S_Image;
                    graphics.Dispose();
                }
            }
            sPen.Dispose();
            blackBrush.Dispose();
        }

        /// <summary>
        /// 画小网格，被DrawMutiGrids()调用
        /// </summary>
        private void DrawOneGrid(Graphics graphics, Pen sPen)
        {
            for (int x = 0; x < (mRow + 1); x++)  //画横线
            {
                int ptY = (int)(SmallImageH * x + OffH / 2);
                int ptX = (int)(SmallImageW * mColumn + OffW);
                Point pts = new Point(0, ptY);
                Point pte = new Point(ptX, ptY);

                graphics.DrawLine(sPen, pts, pte);
            }
            for (int y = 0; y < (mColumn + 1); y++)//画纵线
            {
                int ptX = (int)(y * SmallImageW + OffW / 2);
                int ptY = (int)(mRow * SmallImageH + OffH);

                graphics.DrawLine(sPen, new Point(ptX, 1), new Point(ptX, ptY));
            }
        }

        /// <summary>
        /// 选定图像, 为选定区域设置显示效果、光标方框 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        private bool SetImageCursor(int X, int Y)
        {
            this.mSelectedIndex = GetSelectIndexByMouse(X, Y);

            return SetImageCursor(true, true, true);
        }

        protected virtual bool SetImageCursor(bool externalEvent, bool setBtnEnable, bool refresh) //参数指示是否触发外部事件
        {
            if (mSelectedIndex > Num - 1 || mSelectedIndex < 0) return false;
            //if (mImageNum == mPreImageNum) return false;
            if (pictureBoxImages.Image == null || SmallImageW < 1 || SmallImageH < 1) return false;

            BrushGrid(mPreImageIndex);
            DrawImageToGrid(mPreImageIndex); //恢复之前的选中小网格
            DrawGridIndex(mPreImageIndex, this.cl);
            mPreImageIndex = mSelectedIndex;

            DrawGridCursor(mSelectedIndex);
            DrawGridIndex(mSelectedIndex, mSelectGridColor);

            int count = (Bits != null) ? Bits.Count : 0;
            int startIndex = GetStartImageIndex(mSelectedIndex);
            bool lineVisible = (startIndex > 0) || (startIndex + Column - 1 < count - 1);//(Num > mRow * mColumn);
            btnPreviousLine1.Visible = lineVisible;
            btnNextLine1.Visible = lineVisible;
            btnPreviousLine2.Visible = lineVisible;
            btnNextLine2.Visible = lineVisible;
            if (setBtnEnable) SetBtnEnable();
            if(refresh) pictureBoxImages.Refresh();

            ////在画选中网格的时候触发选中事件
            if (externalEvent && ImageNumChanged != null)
            {
                ImageNumChanged(this, new EventArgs());
            }
            return true;
        }

        /// <summary>
        /// 返回第几张片子
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        protected bool CheckImageIndexChanged(int X, int Y, bool isSingleSelect)
        {
            //int PicMarginH = pictureBoxImages.Margin.Left;
            //int PicMarginV = pictureBoxImages.Margin.Top;
            //if (X <= PicMarginH || Y <= PicMarginV
            //    || X >= pictureBoxImages.Width - 2 * PicMarginH
            //    || Y >= pictureBoxImages.Height - 2 * PicMarginV)
            //{
            //    return false;
            //}

            //int NumCol = (int)((X - OffW / 2) / SmallImageW);
            //int NumRow = (int)((Y - OffH / 2) / SmallImageH);
            //int SelectNum = NumRow * mColumn + NumCol + CountFlag * mColumn; //获取Image在数组中的位置 + 1
            int SelectNum = GetSelectIndexByMouse(X, Y);
            if (SelectNum < 0) return false;

            if (SelectNum == mSelectedIndex && isSingleSelect)//this.mSingleSelect)
            {
                return false;
            }
            if (SelectNum < Num)
            {
                mSelectedIndex = SelectNum;
                SetShow_RowColumn(mPreImageIndex);
                return true;
            }
            return false;
        }

        private void SetImageByKey(Keys keyData)
        {
            string ch = "";
            switch (keyData)
            {
                case Keys.A:
                case Keys.Left:
                    ch = "LEFT";
                    break;
                case Keys.D:
                case Keys.Right:
                    ch = "RIGHT";
                    break;
                case Keys.W:
                case Keys.Up:
                    ch = "UP";
                    break;
                case Keys.S:
                case Keys.Down:
                    ch = "DOWN";
                    break;
                case Keys.PageDown:
                    ch = "PageDown";
                    break;
                case Keys.PageUp:
                    ch = "PageUp";
                    break;
                case Keys.Home:
                    ch = "Home";
                    break;
                case Keys.End:
                    ch = "End";
                    break;
            }
            GetKeyLocation(ch);
        }

        /// <summary>
        /// 用来获取移动到的位置
        /// </summary>
        /// <param name="MD"></param>
        protected virtual void GetKeyLocation(string MD)
        {
            if (mSelectedIndex > Num - 1 || mSelectedIndex < 0)
            {
                return;
            }

            int oldImageIndex = mSelectedIndex;
            if ((MD == "DOWN"))
            {
                mSelectedIndex += mColumn;

                mSelectedIndex = CheckSelectIndex(mSelectedIndex);
            }
            else if ((MD == "UP"))
            {
                mSelectedIndex -= mColumn;
                if (mSelectedIndex < 0)
                {
                    mSelectedIndex = 0;
                    //mSelectedIndex += mColumn;
                    //return;
                }
            }
            else if (MD == "LEFT")
            {
                if (mSelectedIndex < 1)
                {
                    return;
                }
                mSelectedIndex -= 1;
            }
            else if (MD == "RIGHT")
            {
                if (mSelectedIndex > Num - 2)
                {
                    return;
                }
                mSelectedIndex += 1;
            }
            else if (MD == "PageDown")
            {
                mSelectedIndex += mRow * mColumn;
                mSelectedIndex = CheckSelectIndex(mSelectedIndex);
                //CountFlag += mRow;
            }
            else if (MD == "PageUp")
            {
                mSelectedIndex -= mRow * mColumn;
                if (mSelectedIndex < 0)
                {
                    mSelectedIndex = 0;
                    //int returnRowNum = (mSelectedIndex+1) / mColumn;
                    //returnRowNum = returnRowNum < mRow ? returnRowNum : mRow;
                    //mSelectedIndex -= (returnRowNum - 1) * mColumn;
                    //if (mSelectedIndex < 0) mSelectedIndex = 0;
                }
                //CountFlag -= mRow;
            }
            else if (MD == "Home")
            {
                mSelectedIndex = 0;
            }
            else if (MD == "End")
            {
                mSelectedIndex = Num - 1;
            }
            else
            {
                return;
            }

            bool isChangedView = SetShow_RowColumn(oldImageIndex);
            if (isChangedView)
            {
                DrawImages(Bits, true);
            }
            else
            {
                if (oldImageIndex == mSelectedIndex) return;
                SetImageCursor(true, true, false);
            }
            //this.pictureBoxImages.Focus(); //获取焦点 
            this.pictureBoxImages.Refresh();
        }

        /// <summary>
        /// 来转到指定的位置，并获得其Index
        /// </summary>
        /// <param name="ChangeNum"></param>
        private void ChangeImageNumTo()
        {
            //if (ChangeNum > Num || ChangeNum < 1)
            //{
            //    return;
            //}
            //mSelectedIndex = ChangeNum;
            DrawImages(Bits, true);
        }

        private void SetBtnEnable()
        {
            if (btnPreviousLine1.Visible)
            {
                int startIndex = GetStartImageIndex(mSelectedIndex);
                int lastIndex = startIndex + mRow * mColumn;
                bool preEnable = startIndex > 0;
                bool nextEnable = lastIndex - Num < 0;

                Image enableUp = null;
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlMutiImages));
                if (preEnable)
                {
                    enableUp = ((System.Drawing.Image)(resources.GetObject("btnPreviousLine1.BackgroundImage")));
                }
                else
                {
                    enableUp = ((System.Drawing.Image)(resources.GetObject("btnPreviousLine2.BackgroundImage")));
                }
                Image enableDown = null;
                if (nextEnable)
                {
                    enableDown = ((System.Drawing.Image)(resources.GetObject("btnNextLine1.BackgroundImage")));
                }
                else
                {
                    enableDown = ((System.Drawing.Image)(resources.GetObject("btnNextLine2.BackgroundImage")));
                }
                btnPreviousLine1.Enabled = preEnable;
                btnPreviousLine2.Enabled = preEnable;
                btnNextLine1.Enabled = nextEnable;
                btnNextLine2.Enabled = nextEnable;
                btnPreviousLine1.BackgroundImage = enableUp;
                btnPreviousLine2.BackgroundImage = enableUp;
                btnNextLine1.BackgroundImage = enableDown;
                btnNextLine2.BackgroundImage = enableDown;
            }
        }

        protected int GetShowRow(int imgIndex)
        {
            int startIndex = GetStartImageIndex(mSelectedIndex);
            return (imgIndex - startIndex) / mColumn;
        }

        protected int GetShowColumn(int imgIndex)
        {
            int startIndex = GetStartImageIndex(mSelectedIndex);
            return (imgIndex - startIndex) % mColumn;
        }

        private void CheckRowIndex()
        {
            if (ShowRowIndex > mRow - 1) ShowRowIndex = Row - 1;
            else if (ShowRowIndex < 0) ShowRowIndex = 0;
        }

        private void CheckColumnIndex()
        {
            if (ShowColumnIndex > mColumn) ShowColumnIndex = mColumn - 1;
            else if (ShowColumnIndex < 0) ShowColumnIndex = 0;
        }

        private bool SetShow_RowColumn(int oldImageIndex)
        {
            mSelectedIndex = CheckSelectIndex(mSelectedIndex);
            oldImageIndex = CheckSelectIndex(oldImageIndex);

            int oldStartIndex = GetStartImageIndex(oldImageIndex);
            //If is the last row, and in the same view as front ShowColumnIndex, reset ShowColumnIndex
            int spaceNum = mSelectedIndex - oldImageIndex;
            //the next column and row based on the old.
            if (spaceNum > 0) //Move after old one
            {
                int oldRowIndex = ShowRowIndex;
                ShowColumnIndex += spaceNum % mColumn;
                ShowRowIndex += spaceNum / mColumn;
                if (ShowColumnIndex >= mColumn)
                {
                    ShowColumnIndex = mColumn - 1;
                    ShowRowIndex += 1;
                }
                if (ShowRowIndex > mRow - 1)
                {
                    ShowRowIndex = oldRowIndex;
                }
            }
            else if (spaceNum < 0) //Move before old one
            {
                int oldRowIndex = ShowRowIndex;
                ShowRowIndex += spaceNum / mColumn;

                if (mSelectedIndex < 1)
                {
                    ShowColumnIndex = 0;
                    ShowRowIndex = 0;
                }
                else if (ShowRowIndex < 0)
                {
                    ShowRowIndex = oldRowIndex;
                    //ShowColumnIndex += spaceNum % mColumn;
                }
                else
                {
                    ShowColumnIndex += spaceNum % mColumn;
                }
            }

            CheckRowIndex();
            CheckColumnIndex();

            bool isChangedView = false;//Whether the view is changed
            int startIndex = GetStartImageIndex(mSelectedIndex);
            if (oldStartIndex != startIndex) isChangedView = true;

            return isChangedView;
        }

        /// <summary>
        /// get the first image index
        /// </summary>
        /// <returns></returns>
        protected int GetStartImageIndex(int imageIndex)
        {
            int startIndex = imageIndex - (ShowRowIndex * mColumn + ShowColumnIndex);
            if (startIndex < 0) startIndex = 0;

            return startIndex;
        }

        protected int GetSelectIndexByMouse(float x, float y)
        {
            int curSelectIndex = -1;
            //if (x < OffW / 2) x = OffW / 2;
            //if (y < OffH / 2) y = OffH / 2;
            //if (x > pictureBoxImages.Width) x = pictureBoxImages.Width - OffW / 2;
            //if (y > pictureBoxImages.Height) y = pictureBoxImages.Height - OffH / 2;
            if (x <= OffW / 2 || y <= OffH / 2
                || x >= pictureBoxImages.Width - OffW / 2
                || y >= pictureBoxImages.Height - OffH / 2)
            {
                return curSelectIndex;
            }

            int NumCol = (int)((x - OffW / 2) / SmallImageW);
            int NumRow = (int)((y - OffH / 2) / SmallImageH);
            if (NumRow >= mRow) NumRow -= 1;
            if (NumCol >= mColumn) NumCol -= 1;

            int startIndex = GetStartImageIndex(mSelectedIndex);
            curSelectIndex = startIndex + NumRow * mColumn + NumCol;

            return curSelectIndex;
        }

        private int CheckSelectIndex(int index)
        {
            if (index > Num - 1) index = Num - 1;
            else if (index < 0) index = 0;

            return index;
        }

        #endregion
        #endregion

        #region   属性定义
        /// <summary>
        /// 选中的网格索引
        /// </summary>
        protected int mSelectedIndex = -1; //当前选中的片子索引,从１开始
        /// <summary>
        /// 选中的网格索引
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                // add by jt 2010.07.05
                mSelectedIndex = CheckSelectIndex(mSelectedIndex);
                return mSelectedIndex;
            }
            set
            {
                if (mSelectedIndex != value)
                {
                    mSelectedIndex = value;
                    SetShow_RowColumn(mPreImageIndex);
                    ChangeImageNumTo();
                }
            }
        }

        /// <summary>
        /// 设置要显示的行数
        /// </summary>
        protected int mRow = 1;  //行
        public int Row
        {
            get { return mRow; }
            set
            {
                mRow = value;
                if (mRow < 1) mRow = 1;
            }
        }

        /// <summary>
        /// 设置要显示的列数
        /// </summary>
        protected int mColumn = 5;  //列
        public int Column
        {
            get { return mColumn; }
            set
            {
                mColumn = value;
                if (mColumn < 1) mColumn = 1;
            }
        }

        /// <summary>
        /// 设置选中单元格的网格颜色
        /// </summary>
        protected Color mSelectGridColor = Color.Green;//LightGreen;  //选中网格的颜色
        public Color SelectGridColor
        {
            get { return mSelectGridColor; }
            set { mSelectGridColor = value; }
        }

        private int mSelectPenWidth = 2;
        /// <summary>
        /// 设置选中网格时选中框的显示宽度
        /// </summary>
        public int SelectPenWidth
        {
            get { return mSelectPenWidth; }
            set { mSelectPenWidth = value; }
        }

        /// <summary>
        /// 是否在小图上显示片子的位置
        /// </summary>
        protected bool mShowImageIndex = true;//是否显示小网格所在的位置 
        public bool ShowImageIndex
        {
            get { return mShowImageIndex; }
            set { mShowImageIndex = value; }
        }

        /// <summary>
        /// 设置MouseWheel是否可用
        /// </summary>
        protected bool mMouseWheelEnable = true;
        public bool MouseWheelEnable
        {
            get { return mMouseWheelEnable; }
            set { mMouseWheelEnable = value; }
        }

        /// <summary>
        /// 小网格是否是单选择的
        /// (为了不改变基类的程序结构，该字段在派生类中用到的多)
        /// </summary>
        protected bool mSingleSelect = true;
        public bool SingleSelect
        {
            get { return mSingleSelect; }
            set
            {
                mSingleSelect = value;
            }
        }

        private bool mUseDirectionKeyPress = true;
        [DefaultValue(true)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        /// <summary>
        /// Whether use the press of Direction key"->, <-..."
        /// </summary>
        public bool UseDirectionKeyPress
        {
            get { return mUseDirectionKeyPress; }
            set
            {
                mUseDirectionKeyPress = value;
            }
        }

        /// <summary>
        /// 获得或设置主区域显示的背景色
        /// </summary>
        public Color ShowBackColor
        {
            get { return this.pictureBoxImages.BackColor; }
            set
            {
                this.pictureBoxImages.BackColor = value;
                this.panelImages.BackColor = value;
                this.ControlPanel.BackColor = value;
                this.ControlPanel.Refresh();
            }
        }

        /// <summary>
        /// 获得或设置Image的填充颜色
        /// </summary>
        public Color ImageBackColor
        {
            get { return mImageBackColor; }
            set { mImageBackColor = value; }
        }

        #endregion

    }
}
