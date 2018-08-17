using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImageCapturing
{
    public partial class MultipleSelectedSmallImages : UserControlMutiImages
    {

        public MultipleSelectedSmallImages()
        {
            InitializeComponent();
        }

        #region Variables & Properties

        public bool MayCancelSelect = false;

        private int OldStartIndex = 0;

        /// <summary>
        /// 是否按下Shift键
        /// </summary>
        private bool mShift = false;

        /// <summary>
        /// 选中的网格索引的列表
        /// </summary>
        public List<int> SelectList = new List<int>(); // 被选中网格的索引
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectList"></param>
        /// <param name="deleteIndex">只有在Delete时候deleteIndex才大于-1，其他时候为-1</param>
        public delegate void DelegateVoidP1(List<int> selectList, int deleteIndex);
        public event DelegateVoidP1 EventSelectListChanged;

        public event EventHandler DoubleClick;

        private Image mSignImg = null;
        /// <summary>
        /// the image of sign flag. If is null, it will not drawn on 
        /// </summary>
        [DefaultValue(null)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Image SignImg
        {
            set
            {
                mSignImg = value;
            }
            get
            {
                return mSignImg;
            }
        }
        #endregion

        #region Override Events

        protected override void pictureBoxImages_MouseDown(object sender, MouseEventArgs e)
        {
            if (!mSingleSelect)//  base.pictureBoxImages_MouseDown(sender, e);            
            
            {
                if (Control.ModifierKeys == Keys.Shift && e.Button == MouseButtons.Left)
                {
                    int preIndex = mPreImageIndex;
                    if (CheckImageIndexChanged(e.X, e.Y, mSingleSelect))
                    {
                        int min = preIndex > mSelectedIndex ? mSelectedIndex : preIndex;
                        int max = preIndex > mSelectedIndex ? preIndex : mSelectedIndex;
                        min = min < 0 ? 0 : min;
                        bool isSelectChanged = false;
                        for (int i = min; i <= max; i++)
                        {
                            if (!SelectList.Contains(i))
                            {
                                SelectList.Add(i);
                                isSelectChanged = true;
                            }
                        }
                        if (!isSelectChanged) //为反向 2010/08/20
                        {
                            for (int i = min; i <= max; i++)
                            {
                                if (SelectList.Contains(i))
                                {
                                    SelectList.Remove(i);
                                    isSelectChanged = true;
                                }
                            }
                            if (isSelectChanged) DrawImages(Bits, false);
                        }
                        if (isSelectChanged)
                        {
                            SetEventSelectListChanged(-1);
                        }
                        mShift = true;
                        SetMultiImageCursor(e.X, e.Y, false);
                    }
                }
                else if (e.Button == MouseButtons.Left 
                    || (e.Button == MouseButtons.Right))
                {
                    if (CheckImageIndexChanged(e.X, e.Y, true))
                    {
                        SetImageCursor(true, false, true);
                        //SetMultiImageCursor(e.X, e.Y, false);
                    }

                    if (e.Button == MouseButtons.Left)
                    {
                        int mSelectedIndex = GetSelectIndexByMouse(e.X, e.Y);
                        int NumCol = GetShowColumn(mSelectedIndex);
                        int NumRow = GetShowRow(mSelectedIndex);
                        int CheckRecSize = 18;
                        if (SmallImageW < CheckRecSize * 3 || SmallImageH < CheckRecSize * 3)
                        {
                            float smallWidth = SmallImageW < SmallImageH ? SmallImageW : SmallImageH;
                            CheckRecSize = (int)(smallWidth / 4.0f);
                        }
                        int OffSmallImageBoder = 5;        // 图片轮廓每边移入小网格的距离 
                        int X1 = (int)(NumCol * SmallImageW + OffW / 2) + OffSmallImageBoder;
                        int Y1 = (int)(NumRow * SmallImageH + OffH / 2) + OffSmallImageBoder;
                        Rectangle destRect = new Rectangle(X1 + 18, Y1, CheckRecSize, CheckRecSize);

                        if (mSelectedIndex < Num && destRect.Contains(e.X, e.Y))
                        {
                            if (!SelectList.Contains(mSelectedIndex))
                            {
                                SelectList.Add(mSelectedIndex);
                                DrawGridIndex(mSelectedIndex, mSelectGridColor);
                                DrawGridCursor(mSelectedIndex);
                                DrawSelectFlag();
                                this.pictureBoxImages.Refresh();
                                if (DoubleClick != null)
                                {
                                    DoubleClick(null, null);
                                }
                            }
                            else if (MayCancelSelect)
                            {
                                SelectList.Remove(mSelectedIndex);
                                DrawImageToGrid(mSelectedIndex);
                                DrawGridIndex(mSelectedIndex, mSelectGridColor);
                                DrawGridCursor(mSelectedIndex);
                                DrawSelectFlag();
                                this.pictureBoxImages.Refresh();
                                if (DoubleClick != null)
                                {
                                    DoubleClick(null, null);
                                }
                            }
                            mShift = false;
                            pictureBoxImages.Focus();
                        }
                    }
                    //ImageNumChangedFun();
                }
                mShift = false;
                pictureBoxImages.Focus();
            }
            else
            {
                base.pictureBoxImages_MouseDown(sender, e);
            }
            //if(selectindexChanged != null) selectindexChanged(mSelectedIndex);
        }

        protected override void pictureBoxImages_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mSingleSelect)
            {
                int SelectNum = GetSelectIndexByMouse(e.X, e.Y);
                if (SelectNum > Num - 1)
                {
                    return;
                }
                int NumCol = GetShowColumn(SelectNum);
                int NumRow = GetShowRow(SelectNum);
                int CheckRecSize = 18;
                if (SmallImageW < CheckRecSize * 3 || SmallImageH < CheckRecSize * 3)
                {
                    float smallWidth = SmallImageW < SmallImageH ? SmallImageW : SmallImageH;
                    CheckRecSize = (int)(smallWidth / 4.0f);
                }
                int OffSmallImageBoder = 5;        // 图片轮廓每边移入小网格的距离 
                int X1 = (int)(NumCol * SmallImageW + OffW / 2) + OffSmallImageBoder;
                int Y1 = (int)(NumRow * SmallImageH + OffH / 2) + OffSmallImageBoder;
                //int signX2 = mSignImg.Width;
                //int signY2 = mSignImg.Height;
                //int signX1 = xMid - (int)((float)signX2 / 2);
                //int signY1 = yMid - (int)((float)signY2 / 2);
                Rectangle destRect = new Rectangle(X1 + 18, Y1, CheckRecSize, CheckRecSize);
                
                if (destRect.Contains(e.X, e.Y))
                {
                    Cursor = Cursors.Hand;
                }
                else
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        protected override void ControlPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            base.ControlPanel_MouseWheel(sender, e);
        }
       
        #endregion

        #region Private Function

        /// <summary>
        /// 给小网格画选择符号
        /// </summary>
        /// <param name="ImageNum">小网格索引</param>
        private void DrawGridSelectSign(int imageIndex)
        {
            if (mSignImg == null) 
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultipleSelectedSmallImages));
                mSignImg = ((System.Drawing.Image)(resources.GetObject("Change4dDisplay")));
                if (mSignImg == null) return;
            }

            Graphics graphics = Graphics.FromImage(pictureBoxImages.Image);

            int NumCol = GetShowColumn(imageIndex);
            int NumRow = GetShowRow(imageIndex);

            int CheckRecSize = 18;
            if (SmallImageW < CheckRecSize * 3 || SmallImageH < CheckRecSize * 3)
            {
                float smallWidth = SmallImageW < SmallImageH ? SmallImageW : SmallImageH;
                CheckRecSize = (int)(smallWidth / 4.0f);
            }
            int OffSmallImageBoder = 5;        // 图片轮廓每边移入小网格的距离 
            int X1 = (int)(NumCol * SmallImageW + OffW / 2) + OffSmallImageBoder;
            int Y1 = (int)(NumRow * SmallImageH + OffH / 2) + OffSmallImageBoder;
            //int signX2 = mSignImg.Width;
            //int signY2 = mSignImg.Height;
            //int signX1 = xMid - (int)((float)signX2 / 2);
            //int signY1 = yMid - (int)((float)signY2 / 2);
            Rectangle destRect = new Rectangle(X1 + 18, Y1, CheckRecSize, CheckRecSize);
            //graphics.FillRectangle(Brushes.AntiqueWhite, destRect);
            int offsetSize = (int)(CheckRecSize / 10.0f);
            int smallRecSize = (int)(CheckRecSize * 4.0f / 5.0f);
            graphics.DrawRectangle(new Pen(mSelectGridColor, 2.0f), destRect);
            graphics.DrawImage(mSignImg, new Rectangle(destRect.X + offsetSize, destRect.Y + offsetSize, smallRecSize, smallRecSize));
            graphics.Dispose();
        }

        /// <summary>
        /// 多选图像, 按坐标方式为选定区域设置显示效果、光标方框 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        private bool SetMultiImageCursor(int X, int Y, bool selectFlag)
        {
            this.mSelectedIndex = GetSelectIndexByMouse(X, Y);

            if (SetMultiImageCursor(this.mSelectedIndex, selectFlag)) return true;
            else return false;
        }

        /// <summary>
        /// 多选图像, 按索引方式为选定区域设置显示效果、光标方框 
        /// </summary>
        /// <param name="ImageNum">小网格索引</param>
        /// <returns></returns>
        public bool SetMultiImageCursor(int ImageIndex, bool selectFlag)
        {
        s:
            int startIndex = GetStartImageIndex(mSelectedIndex);
            if (OldStartIndex == startIndex && !mShift)
            {   //设置前一个状态                
                BrushGrid(mPreImageIndex);
                DrawImageToGrid(mPreImageIndex);
                DrawGridIndex(mPreImageIndex, this.cl);
                //if (SelectList.Contains(mPreImageIndex))
                //{
                //    DrawGridSelectSign(mPreImageIndex);
                //    DrawGridIndex(mPreImageIndex, mSelectGridColor);
                //}
                mPreImageIndex = ImageIndex;

                BrushGrid(ImageIndex);
                DrawImageToGrid(ImageIndex);
                if (SelectList.Contains(ImageIndex))
                {
                    if(selectFlag)
                    {
                        SelectList.Remove(ImageIndex); //2010/08/11 delete   
                        SetEventSelectListChanged(ImageIndex);
                    }
                }
                else
                {
                    if (selectFlag)
                    {
                        SelectList.Add(ImageIndex);
                        SetEventSelectListChanged(-1);
                    }
                    //DrawSelectFlag(); 
                    //DrawGridSelectSign(ImageIndex);
                    //DrawGridIndex(ImageIndex, mSelectGridColor);   
                }
                DrawGridIndex(ImageIndex, mSelectGridColor);               
                DrawGridCursor(ImageIndex);
                DrawSelectFlag(); 
            }
            else
            {
                if (mShift && selectFlag)
                {
                    SelectList.Remove(ImageIndex);
                    SetEventSelectListChanged(ImageIndex);
                }
                mShift = false;
                DrawGridCursor(mSelectedIndex);
                DrawSelectFlag();

                OldStartIndex = startIndex;
                goto s;
            }
            this.pictureBoxImages.Refresh();
            return true;
           
        }

        ///// <summary>
        ///// 画要选定的网格
        ///// </summary>
        ///// <param name="selectEvent">是否需要执行选中的事件</param>
        protected override bool SetImageCursor(bool externalEvent, bool setBtnEnable, bool refresh)
        {
            base.SetImageCursor(externalEvent, setBtnEnable, this.mSingleSelect);
            if(!this.mSingleSelect)
            {
                //这段代码表示在多选网格换页以后，选择事件和单选不同
                //应设置好 mDelFromDict 和 oldBiemap
               // SetMultiImageCursor(int ImageNum)
                //DrawGridCursor(mSelectedIndex);
                DrawSelectFlag();
                this.pictureBoxImages.Refresh();
            }
            return true;
        }

        /// <summary>
        /// 设置右键菜单
        /// </summary>
        /// <param name="cMenu"></param>
        public void SetContextMenuStrip(ContextMenuStrip contextMenuStrip)
        {
            pictureBoxImages.ContextMenuStrip = contextMenuStrip;
        }

        #endregion

        /// <summary>
        /// 设置是否选中Image
        /// </summary>
        public void SetSelectImage(bool select)
        {
            if (SelectedIndex >= Num) return;

            if ((select && SelectList.Contains(SelectedIndex)) 
                || (!select && !SelectList.Contains(SelectedIndex)))
            {
                return;
            }
            SetMultiImageCursor(SelectedIndex, true);           
        }

        public void SelectAll(bool all)
        {
            if (Bits == null || Bits.Count <= 0) return;
            SelectList.Clear();
            if(all)
            {
                for (int i = 0; i < Bits.Count; i++)
                {
                    SelectList.Add(i);
                }                
            }
            SetEventSelectListChanged(-1);
            DrawImages(Bits, false);
            DrawSelectFlag();
            this.pictureBoxImages.Refresh();
        }

        private void DrawSelectFlag()
        {
            if (SelectList.Count > 0)
            {
                foreach (int index in SelectList)
                {
                    //DrawGridIndex(index, mSelectGridColor);
                    DrawGridSelectSign(index);
                }
            }
        }

        private void SetEventSelectListChanged(int deleteIndex)
        {
            if (EventSelectListChanged != null)
            {
                EventSelectListChanged(this.SelectList, deleteIndex);
            }
        }
    
    }
}
