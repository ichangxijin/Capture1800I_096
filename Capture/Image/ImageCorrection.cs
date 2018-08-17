using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace ImageCapturing
{
    public unsafe class ImageCorrection
    {
        private ushort[,] offset_img_;
        private float[,] gain_coeff_;
        private ushort[,] gain_img_=null;
        private bool[,] pixel_img_=null;
        public ushort[,] img_;
        private int width_;
        private int height_;

        public bool do_offset_ = false;
        public bool do_gain_ = false;
        public bool do_pixel_ = false;

        private int* pixel_corr_list_ = (int*)IntPtr.Zero;
        private ushort* gain_map_ = (ushort*)IntPtr.Zero;
        private ushort* gain_med_map_ = (ushort*)IntPtr.Zero;
        private ushort* offset_map_ = (ushort*)IntPtr.Zero;
        private ushort* img_buf_ = (ushort*)IntPtr.Zero;
        private int gain_seq_num_ = 0;


        public void SetGainFile(string fn)
        {
            if ((IntPtr)gain_map_!= IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)gain_map_);
                Kernel32Interface.CloseHandle((IntPtr)gain_map_);
                gain_map_= (ushort*)IntPtr.Zero;
            }

            if ((IntPtr)gain_med_map_!= IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)gain_med_map_);
                Kernel32Interface.CloseHandle((IntPtr)gain_med_map_);
                gain_med_map_= (ushort*)IntPtr.Zero;
            }
            if (File.Exists(fn))
            {
                List<ushort[,]> list = HisObject.OpenFile(fn).dataList;
                gain_seq_num_ = list.Count;
                int count = list[0].Length;
                width_ = list[0].GetLength(1);
                height_ = list[0].GetLength(0);
                gain_map_ = ArrayListToBuffer(list);
                gain_med_map_ = (ushort*)Marshal.AllocHGlobal((int)(gain_seq_num_* sizeof(short)));
                PKL_Interface.Acquisition_CreateGainMap(gain_map_,gain_med_map_, count,gain_seq_num_);

                offset_map_ = (ushort*)Marshal.AllocHGlobal((int)(width_*height_* sizeof(short)));
                for (int i = 0; i < width_ * height_;i++ )
                {
                    offset_map_[i] = 0;
                }
            }
        }
        public void SetOffsetFile(string fn)
        {
            HisObject his = new HisObject();
            his.LoadDataFromFile(fn);
            offset_img_ = his.dataList[0];
            width_ = his.hisHeader.BRX;
            height_ = his.hisHeader.BRY;
        }

        public void SetPixelMapFile(string fn)
        {
            if ((IntPtr)pixel_corr_list_ != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)pixel_corr_list_);
                Kernel32Interface.CloseHandle((IntPtr)pixel_corr_list_);
                pixel_corr_list_= (int*)IntPtr.Zero;
            }
            if (File.Exists(fn))
            {
                List<ushort[,]> list = HisObject.OpenFile(fn).dataList;
                int Frames = list.Count;
                width_ = list[0].GetLength(1);
                height_ = list[0].GetLength(0);
                int pCorrListSize = 0;
                ushort* pPixelSoure = ArrayListToBuffer(list);
                PKL_Interface.Acquisition_CreatePixelMap(pPixelSoure, height_, width_, null, ref pCorrListSize);
                pixel_corr_list_ = (int*)Marshal.AllocHGlobal((int)(pCorrListSize * sizeof(int)));
                PKL_Interface.Acquisition_CreatePixelMap(pPixelSoure, height_, width_,pixel_corr_list_, ref pCorrListSize);

                Marshal.FreeHGlobal((IntPtr)pPixelSoure);
                Kernel32Interface.CloseHandle((IntPtr)pPixelSoure);
                pPixelSoure = (ushort*)IntPtr.Zero;
            }
        }
        private unsafe static ushort* ArrayListToBuffer(List<ushort[,]> list)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            int Frames = list.Count;
            int Rows = list[0].GetLength(0);
            int Cols = list[0].GetLength(1);
            ushort* Buffer = (ushort*)System.Runtime.InteropServices.Marshal.AllocHGlobal((int)(Frames * Rows * Cols * sizeof(short)));
            ushort* p = Buffer;
            for (int n = 0; n < Frames; n++)
            {
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        *p = list[n][i, j];
                        p++;
                    }
                }
            }
            return Buffer;
        }
        private unsafe static ushort* ArrayToBuffer(ushort[,] arr)
        {
            if (arr== null )
            {
                return null;
            }
            int Rows = arr.GetLength(0);
            int Cols = arr.GetLength(1);
            ushort* Buffer = (ushort*)System.Runtime.InteropServices.Marshal.AllocHGlobal((int)( Rows * Cols * sizeof(short)));
            ushort* p = Buffer;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    *p = arr[i, j];
                    p++;
                }
            }
            return Buffer;
        }
        public void  Apply()
        {

            // offset
            //--------------------------------------------------------------------------------
            if (do_offset_)
            {
                for (int i=0;i<height_;i++)
                {
                    for (int j = 0; j < width_;j++ )
                    {
                        img_[i, j] -= offset_img_[i, j];
                    }
                }
            }


            //////////////////////////////////////////////////////////////////////////
            if ((IntPtr)img_buf_ != IntPtr.Zero)
            {
                Marshal.FreeHGlobal((IntPtr)img_buf_);
                Kernel32Interface.CloseHandle((IntPtr)img_buf_);
                img_buf_= (ushort*)IntPtr.Zero;
            }

            img_buf_ = ArrayToBuffer(img_);
            //////////////////////////////////////////////////////////////////////////

            // gain 
            if (do_gain_)
            {
                PKL_Interface.Acquisition_DoOffsetGainCorrection_Ex(img_buf_, img_buf_,offset_map_,gain_map_,gain_med_map_, width_*height_,gain_seq_num_);
            }

            //////////////////////////////////////////////////////////////////////////
            // pixel 
            if (do_pixel_)
            {
                PKL_Interface.Acquisition_DoPixelCorrection(img_buf_, pixel_corr_list_);
            }

            //////////////////////////////////////////////////////////////////////////
            ushort* p_idx = img_buf_;
            for (int i = 0; i < width_; i++)
            {
                for (int j = 0; j <height_; j++)
                {
                    img_[i, j] = *p_idx;
                    p_idx++;
                }
            }
            Marshal.FreeHGlobal((IntPtr)img_buf_);
            Kernel32Interface.CloseHandle((IntPtr)img_buf_);
            img_buf_= (ushort*)IntPtr.Zero;
        }

    }
}
