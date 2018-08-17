using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace ImageCapturing
{
    /// <summary>
    /// Window结构
    /// </summary>
    /// 2008/11/11 add
    public class ImageWindow
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name;
        /// <summary>
        /// Window Center
        /// </summary>
        public string Center;
        /// <summary>
        /// Window Width
        /// </summary>
        public string Width;
        /// <summary>
        /// 影像的类型:CT,MR等
        /// </summary>
        public string ImageType;
    }

    /// <summary>
    /// Color LUT
    /// </summary>
    /// 2010.05.25 add by jt
    public class ColorMode
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 颜色模板的LUT
        /// </summary>
        public System.Drawing.Color[] colorLUT;
    }


    public class xmlAcess_WL_Color_Model
    {

        public static string DataPath = ClassPath.GetAppDataSystemPath();
        private static string fileWLModel = DataPath + "WLModel.xml";
        private static string fileColorModel = DataPath + "\\ColorModel.xml";


        #region WLModel

        private static DataTable GenerateTable_WLModel()
        {
            DataTable tb = new DataTable("Window");
            tb.Columns.Add(new DataColumn("Window Name", typeof(System.String)));
            tb.Columns.Add(new DataColumn("Window Center", typeof(System.String)));
            tb.Columns.Add(new DataColumn("Window Width", typeof(System.String)));
            tb.Columns.Add(new DataColumn("Image Type", typeof(System.String)));

            return tb;
        }

        /// <summary>
        /// 根据名称从WLModel中删除行
        /// </summary>
        /// <param name="name"></param>
        public static void Delete_WLModelRow(string name, string type)
        {
            DataSet ds = GetDataTable_WLModel();

            DataTable table = ds.Tables[0];
            DataRow[] drs = table.Select("[Window Name] = '" + name + "' and [Image Type] = '" + type + "'");
            if (drs.Length < 1)
            {
                ds.Dispose();
                return;
            }

            for (int i = 0; i < drs.Length; ++i)
            {
                drs[i].Delete();
            }
            DataSet changeSet = ds.GetChanges();
            if (changeSet != null)
            {
                //changeSet = new DataSet();
                //changeSet.Tables.Add(GenerateTable_WLModel());
                changeSet.WriteXml(fileWLModel);
                changeSet.Dispose();
            }

            ds.Dispose();
        }

        /// <summary>
        /// 向WLModel表中添加或更新行
        /// </summary>
        /// <param name="w"></param>
        public static void Update_WLModelRow(ImageWindow w)
        {
            DataSet ds = GetDataTable_WLModel();
            DataTable table = ds.Tables[0];

            DataRow[] drs = table.Select("[Window Name] = '" + w.Name + "' and [Image Type] = '" + w.ImageType + "'");
            DataRow dr;
            if (drs != null && drs.Length > 0)
            {
                dr = drs[0];
                dr["Window Center"] = w.Center;
                dr["Window Width"] = w.Width;
                dr["Image Type"] = w.ImageType;
            }
            else
            {
                dr = table.NewRow();
                dr["Window Name"] = w.Name;
                dr["Window Center"] = w.Center;
                dr["Window Width"] = w.Width;
                dr["Image Type"] = w.ImageType;

                table.Rows.Add(dr);
            }

            CapturePub.WriteData(ds, fileWLModel);
            ds.Dispose();
        }

        /// <summary>
        /// 获得指定的Window信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ImageWindow GetWindow_WLModel(string name, string type)
        {
            ImageWindow w = new ImageWindow();
            DataSet ds = GetDataTable_WLModel();

            DataTable table = ds.Tables[0];
            DataRow[] drs = table.Select("[Window Name] = '" + name + "' and [Image Type] = '" + type + "'");
            if (drs.Length < 1)
            {
                ds.Dispose();
                return w;
            }

            w.Name = name;
            w.Center = drs[0][1].ToString();
            w.Width = drs[0][2].ToString();
            w.ImageType = type;

            return w;
        }

        /// <summary>
        /// 获得WLModel的表
        /// </summary>
        /// <returns></returns>
        public static DataSet GetDataTable_WLModel()
        {
            DataSet ds = CapturePub.GetData(fileWLModel);
            if (ds.Tables.Count < 1)
            {
                ds.Tables.Add(GenerateTable_WLModel());
            }

            return ds;
        }

        /// <summary>
        /// 获得指定类型的所有行数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DataRow[] GetDataRow_WLModel(string type)
        {
            DataSet ds = GetDataTable_WLModel();
            DataTable dt = ds.Tables[0];
            return dt.Select("[Image Type] = '" + type + "'");
        }

        /// <summary>
        /// 获得指定类型的所有行数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ImageWindow[] GetImageWindow_WLModel(string type)
        {
            List<ImageWindow> list = new List<ImageWindow>();
            DataSet ds = GetDataTable_WLModel();
            DataTable dt = ds.Tables[0];
            DataRow[] drs = dt.Select("[Image Type] = '" + type + "'");
            if (drs != null)
            {
                for (int i = 0; i < drs.Length; ++i)
                {
                    ImageWindow w = new ImageWindow();
                    w.Name = drs[i][0].ToString();
                    w.ImageType = type;
                    w.Center = drs[i][1].ToString();
                    w.Width = drs[i][2].ToString();
                    list.Add(w);
                }
            }
            return list.ToArray();
        }

        #endregion

        #region ColorMode

        private static DataTable GenerateTable_ColorModel()
        {
            DataTable tb = new DataTable("ColorModel");
            tb.Columns.Add(new DataColumn("Name", typeof(System.String)));
            tb.Columns.Add(new DataColumn("R", typeof(System.String)));
            tb.Columns.Add(new DataColumn("G", typeof(System.String)));
            tb.Columns.Add(new DataColumn("B", typeof(System.String)));

            return tb;
        }

        /// <summary>
        /// 获得指定的Window信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ColorMode GetLUT_ColorModel(string name)
        {
            ColorMode c = new ColorMode();
            DataSet ds = GetLUT_ColorModel();

            DataTable table = ds.Tables[0];
            DataRow[] drs = table.Select("[Name] = '" + name + "'");
            if (drs.Length < 1)
            {
                ds.Dispose();
                return null;
            }

            c.Name = name;
            c.colorLUT = new System.Drawing.Color[256];
            DataRow row = drs[0];
            if (row != null)
            {
                for (int i = 0; i <= 255; i++)
                {
                    int r = Convert.ToInt32(row["R"].ToString().Substring(3 * i, 3));
                    int g = Convert.ToInt32(row["G"].ToString().Substring(3 * i, 3));
                    int b = Convert.ToInt32(row["B"].ToString().Substring(3 * i, 3));

                    c.colorLUT[i] = System.Drawing.Color.FromArgb(r, g, b);
                }
            }
            else
            {
                for (int i = 0; i <= 255; i++)
                {
                    c.colorLUT[i] = System.Drawing.Color.FromArgb(i, i, i);
                }
            }

            return c;
        }

        /// <summary>
        /// 获得WLModel的表
        /// </summary>
        /// <returns></returns>
        public static DataSet GetLUT_ColorModel()
        {
            DataSet ds = CapturePub.GetData(fileColorModel);
            if (ds.Tables.Count < 1)
            {
                ds.Tables.Add(GenerateTable_ColorModel());
            }

            return ds;
        }

        /// <summary>
        /// 获得指定类型的所有行数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ColorMode[] ReadLUTColorModel()
        {
            List<ColorMode> list = new List<ColorMode>();
            DataSet ds = GetLUT_ColorModel();
            DataTable table = ds.Tables[0];
            if (table != null)
            {
                for (int j = 0; j < table.Rows.Count; ++j)
                {
                    ColorMode c = new ColorMode();
                    DataRow row = table.Rows[j];
                    c.Name = row[0].ToString();
                    c.colorLUT = new System.Drawing.Color[256];

                    if (row != null)
                    {
                        for (int i = 0; i <= 255; i++)
                        {
                            int r = Convert.ToInt32(row["R"].ToString().Substring(3 * i, 3));
                            int g = Convert.ToInt32(row["G"].ToString().Substring(3 * i, 3));
                            int b = Convert.ToInt32(row["B"].ToString().Substring(3 * i, 3));

                            c.colorLUT[i] = System.Drawing.Color.FromArgb(r, g, b);
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= 255; i++)
                        {
                            c.colorLUT[i] = System.Drawing.Color.FromArgb(i, i, i);
                        }
                    }
                    list.Add(c);
                }
            }
            return list.ToArray();
        }



        #endregion

    }
}
