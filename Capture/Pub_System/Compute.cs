using System;
using System.Collections.Generic;
using System.Text;
using AMRT;

namespace ImageCapturing
{
    public class Compute
    {
        public static bool ComputeShift(float[] beamA, float[] beamB, ref float[] result)
        {
            if ((beamA == null || beamA.Length < 3) && (beamB == null || beamB.Length < 3))
            {
                return false;
            }
            int gantryA = (int)beamA[0] % 360;
            //int gantryB = (int)beamB[0] % 360;
            //int angle = Math.Abs(gantryA - gantryB);
            //if (gantryA != 0 && gantryB != 0 && angle != 90 && angle != 270)
            //{
            //    string msg = "Only support gantry angle pairs 0/90 or 0/270";
            //    cls_MessageBox.Show(msg);
            //    return false;
            //}

            //const float maxError = 0.2f;
            //if (Math.Abs(beamB[2] - beamA[2]) > maxError)
            //{
            //    string msg = "The value of Offset Y should be same.";
            //    cls_MessageBox.Show(msg);
            //    return false;
            //}

            //result[1] = - (beamA[2] + beamB[2]) * 0.5f; //LNG, Foot(-) -> Head(+)
            //if(gantryA == 0)
            //{
            //    result[0] = -beamA[1];                   //LAT, Right(-) -> Left(+)
            //    result[2] = -beamB[1];                   //VRT, Prior(-) -> Back(+)
            //}
            //else
            //{
            //    result[0] = -beamB[1];
            //    result[2] = -beamA[1];
            //}

            if (beamA != null && beamA.Length == 3 && (beamB == null || beamB.Length < 3))
            {
                result[1] = beamA[2]; //LNG, Foot(-) -> Head(+)  //20110124 CXX 头脚方向的偏移和实际方向一致，不应取反
                if (Math.Abs((float)Math.Cos(gantryA * Math.PI / 180) - 0) > 0.00001)
                {
                    result[0] = -beamA[1] * (float)Math.Cos(gantryA * Math.PI / 180);
                }
                if (Math.Abs((float)Math.Sin(gantryA * Math.PI / 180) - 0) > 0.00001)
                {
                    result[2] = -beamA[1] * (float)Math.Sin(gantryA * Math.PI / 180);
                }
                if (gantryA > 180)
                {
                    result[2] *= -1;
                }
            }
            else if (beamA != null && beamA.Length == 3 && beamB != null && beamB.Length == 3)
            {
                int gantryB = (int)beamB[0] % 360;
                const float maxError = 0.2f;
                if (Math.Abs(beamB[2] - beamA[2]) > maxError)
                {
                    string msg = "The value of Offset Y should be same.";
                    cls_MessageBox.Show(msg);
                    return false;
                }
                result[1] = (beamA[2] + beamB[2]) * 0.5f; //LNG, Foot(-) -> Head(+)  //20110124 CXX 头脚方向的偏移和实际方向一致，不应取反
                result[0] = (-beamA[1] * (float)Math.Cos(gantryA * Math.PI / 180) - beamB[1] * (float)Math.Cos(gantryB * Math.PI / 180)) ;
                result[2] = (-beamA[1] * (float)Math.Sin(gantryA * Math.PI / 180) - beamB[1] * (float)Math.Sin(gantryB * Math.PI / 180)) ;
                if (Math.Abs(Math.Cos(gantryA * Math.PI / 180))> 0.000001 && Math.Abs(Math.Cos(gantryB * Math.PI / 180)) > 0.000001)
                {
                    result[0] /= 2.0f;
                }
                if (Math.Abs(Math.Sin(gantryA * Math.PI / 180)) > 0.000001 && Math.Abs(Math.Sin(gantryB * Math.PI / 180)) > 0.000001)
                {
                    result[2] /= 2.0f;
                }
                if (gantryA + gantryB > 180)
                {
                    result[2] *= -1;
                }
            }
            return true;
        }

        public static bool ComputeShiftNew(float SAD ,float[] beamA, float[] beamB, ref float[] result)
        {
            if ((beamA == null || beamA.Length < 3) && (beamB == null || beamB.Length < 3))
            {
                return false;
            }
            int gantryA = (int)beamA[0] % 360;
            int gantryB = (int)beamB[0] % 360;
            double theta1 = gantryA * Math.PI / 180;
            double theta2 = gantryB * Math.PI / 180;

            double x1 = -beamA[1];
            double y1 = beamA[2];
            double x2 = -beamB[1];
            double y2 = beamB[2];
            x1 = Math.Abs(x1) < 0.01 ? 0 : x1;
            x2 =  Math.Abs(x2) < 0.01 ? 0 : x2;
            y1 =  Math.Abs(y1) < 0.01 ? 0 : y1;
            y2 = Math.Abs(y2) < 0.01 ? 0 : y2;
            try
            {
                double t1, t2, u1, u2;//临时变量
                t1 = SAD * Math.Cos(theta1) + x1 * Math.Sin(theta1);
                t2 = SAD * Math.Sin(theta1) - x1 * Math.Cos(theta1);
                u1 = SAD * Math.Cos(theta2) + x2 * Math.Sin(theta2);
                u2 = SAD * Math.Sin(theta2) - x2 * Math.Cos(theta2);

                double X, Y, Z;
                ///以下对照射灯与投影点之间的连线进行分析，要对直线单位向量上分量为0的进行单独处理

                if (t1 != 0 && t2 != 0 && u1 != 0 && u2 != 0 && y1 != 0 && y2 != 0)
                {
                    X = SAD * (t2 * u2 * (Math.Cos(theta1) - Math.Cos(theta2)) - (t1 * u2 * Math.Sin(theta1) - t2 * u1 * Math.Sin(theta2))) / (t2 * u1 - t1 * u2);
                    Z = SAD * (t1 * u1 * (Math.Sin(theta2) - Math.Sin(theta1)) - (t1 * u2 * Math.Cos(theta2) - t2 * u1 * Math.Cos(theta1))) / (t2 * u1 - t1 * u2);
                    Y = -y2 * (X - SAD * Math.Sin(theta1)) / t2;
                }

                else if (t1 == 0)
                {
                    Z = SAD * Math.Cos(theta1);
                    if (t2 == 0)
                    {
                        X = SAD * Math.Sin(theta1);
                    }
                    else
                    {
                        if (u2 == 0)
                            X = SAD * Math.Sin(theta2);
                        else
                            X = SAD * (Math.Sin(theta1) - u2 / u1 * (Math.Cos(theta2) - Math.Cos(theta1)));
                    }
                    if (y1 != 0 && y2 != 0)
                        Y = y2 * SAD * (Math.Cos(theta2) - Math.Cos(theta1)) / u1;
                    else
                        Y = 0;
                }
                else
                {
                    if (t2 == 0)
                    {
                        X = SAD * Math.Sin(theta1);
                        if (u1 == 0)
                            Z = SAD * Math.Cos(theta2);
                        else
                            Z = SAD * (Math.Cos(theta2) - u1 / u2 * (Math.Sin(theta2) - Math.Sin(theta1)));
                        if (y1 != 0 && y2 != 0)
                            Y = y2 * SAD * (Math.Sin(theta2) - Math.Sin(theta1)) / u2;
                        else
                            Y = 0;
                    }
                    else
                    {
                        if (u1 == 0)
                        {
                            Z = SAD * Math.Cos(theta2);
                            if (u2 == 0)
                                X = SAD * Math.Sin(theta2);
                            else
                                X = SAD * (Math.Sin(theta1) - t2 / t1 * (Math.Cos(theta1) - Math.Cos(theta2)));
                            if (y1 != 0 && y2 != 0)
                                Y = y1 * SAD * (Math.Cos(theta1) - Math.Cos(theta2)) / t1;
                            else
                                Y = 0;
                        }
                        else
                        {
                            if (u2 == 0)
                            {
                                X = SAD * Math.Sin(theta2);
                                Z = SAD * (Math.Cos(theta1) - t1 / t2 * (Math.Sin(theta1) - Math.Sin(theta2)));
                            }
                            else
                            {
                                X = SAD * (t2 * u2 * (Math.Cos(theta1) - Math.Cos(theta2)) - (t1 * u2 * Math.Sin(theta1) - t2 * u1 * Math.Sin(theta2))) / (t2 * u1 - t1 * u2);
                                Z = SAD * (t1 * u1 * (Math.Sin(theta2) - Math.Sin(theta1)) - (t1 * u2 * Math.Cos(theta2) - t2 * u1 * Math.Cos(theta1))) / (t2 * u1 - t1 * u2);
                            }
                            if (y1 != 0 && y2 != 0)
                                Y = y1 * SAD * (Math.Sin(theta1) - Math.Sin(theta2)) / t2;
                            else
                                Y = 0;
                        }
                    }

                }
                result[0] = (float)X;
                result[1] = (float)Y;
                result[2] = (float)Z;
            }
            catch (System.Exception ex)
            {
                cls_MessageBox.Show("Calculate Error!");
            }
            return true;
        }

        public static void Synchronization(float gantryA, double rscScaleXA, float resultA, float gantryB, double rscScaleXB, ref float resultB)
        {
            try
            {
                if (Math.Abs(Math.Cos(gantryA * Math.PI / 180)) > 0.000001 && Math.Abs(Math.Cos(gantryB * Math.PI / 180)) > 0.000001)
                {
                    resultB = (resultA * (float)rscScaleXA / (float)rscScaleXB) *
                        (float)Math.Cos(gantryA * Math.PI / 180) / (float)Math.Cos(gantryB * Math.PI / 180);
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        public static void Synchronization(string Dir, float gantryA, float offA, float gantryB, ref float offB) //同步移动
        {
            if (gantryA == float.MinValue || gantryB == float.MinValue)
            {
                return;
            }
            if (Dir == "Up")
            {
                offB += offA;
            }
            else if (Dir == "Down")
            {
                offB -= offA;
            }
            else if (Dir == "Left")
            {
                try
                {
                    if (Math.Abs((float)Math.Cos(gantryB * Math.PI / 180)) > 0.00001)
                    {
                        offB -= (offA * (float)Math.Cos(gantryA * Math.PI / 180) / (float)Math.Cos(gantryB * Math.PI / 180));
                    }
                }
                catch (System.Exception ex)
                {

                }
            }
            else if (Dir == "Right")
            {
                try
                {
                    if (Math.Abs((float)Math.Cos(gantryB * Math.PI / 180)) > 0.00001)
                    {
                        offB += (offA * (float)Math.Cos(gantryA * Math.PI / 180) / (float)Math.Cos(gantryB * Math.PI / 180));
                    }
                }
                catch (System.Exception ex)
                {

                }
            }
        }
#region  hide
        public static float[] ComputeOff(float gantry1, float offx1, float offy1, float gantry2, float offx2, float offy2)
        {
            float[] result = new float[3];
            if (ComputeShift(new float[] { gantry1, offx1, offx2 }, new float[] { gantry2, offx2, offy2 }, ref result))
            {
                return result;
            }
            else
            {
                return null;
            }


            float s = 100.0f;
            try
            {
                s = float.Parse(CapturePub.readCaptrueValue("SAD")) * 0.1f;
            }
            catch (System.Exception ex)
            {
                s = 100.0f;
            }

            float[] diff = new float[3];
            float r1 = gantry1;
            float r2 = gantry2;
            float dx = offx1;
            float dy = offy1;
            float ds = offx2;
            float dt = offy2;
            dx = float.Parse(dx.ToString("0.00"));
            dy = float.Parse(dy.ToString("0.00"));
            ds = float.Parse(ds.ToString("0.00"));
            dt = float.Parse(dt.ToString("0.00"));
            float[][] tempB1 = new float[2][];
            tempB1[0] = new float[3];
            tempB1[1] = new float[3];
            float[][] tempB2 = new float[2][];
            tempB2[0] = new float[3];
            tempB2[1] = new float[3];

            //float[] result = new float[3];
            float[] sourse = { 0, 0, s };
            float[] A2 = { dx, dy, 0 };
            float[] B2 = { ds, dt, 0 };

            float a = (float)(Math.Cos(r1 * Math.PI / 180) * Math.Cos(r2 * Math.PI / 180));
            float b = dx * ds;
            float c = dy * dt;
            if ((a * b < -0.0001) || ((Math.Abs(r1 - r2) == 180) && ((b > -0.0001) || Math.Abs(dx + ds) > 0.01)) || c < -0.0001)
            {
                return null;
            }
            tempB1[0] = TransCoor(0, r1, A2);
            tempB1[1] = TransCoor(0, r1, sourse);
            tempB2[0] = TransCoor(0, r2, B2);
            tempB2[1] = TransCoor(0, r2, sourse);

            float[,] coorB = {{tempB1[0][0],tempB1[1][0],tempB2[0][0],tempB2[1][0]},
                              {tempB1[0][1],tempB1[1][1],tempB2[0][1],tempB2[1][1]},
                              {tempB1[0][2],tempB1[1][2],tempB2[0][2],tempB2[1][2]}};
            result = interPoint(coorB);
            float[] trans1 = new float[3];
            float[] trans2 = new float[3];
            //坐标变换，将求得交点的坐标由0度时的坐标系分别变换到两个角度坐标系下的坐标。
            trans1 = TransCoor(0, r1, result);
            trans2 = TransCoor(0, r2, result);
            //r1角度下x方向偏移量不为零时，理论上 Math.Abs(Math.Abs(trans1[0] / dx) + Math.Abs(trans1[2] / s)=1,r2角度下亦然
            if ((dx != 0 && Math.Abs(Math.Abs(trans1[0] / dx) + Math.Abs(trans1[2] / s) - 1) > 0.1) || (ds != 0 && Math.Abs(Math.Abs(trans2[0] / ds) + Math.Abs(trans2[2] / s) - 1) > 0.1))
            {
                return null;                    
            }

            if (c == 0)//y偏移量中有0值，result[1]=0
            {
                if (result[1] != 0)
                    return null;
            }
            else
            {
                //若两个角度下Y方向上的偏移都不为零
                //往x正方向旋转为正，旋转角度在180度以内，理论上Math.Abs(Math.Abs(trans1[1] / dy) + Math.Abs(trans1[2] / s)=1 
                if (((180 - r1) > 0 || (180 + r1) < 0) && Math.Abs(Math.Abs(trans1[1] / dy) + Math.Abs(trans1[2] / s) - 1) > 0.1)
                {
                    return null;
                }
                else//180度以外，Math.Abs(Math.Abs(dy / trans1[1]) + Math.Abs(s / (s + trans1[2]))) - 1 =0
                {
                    if (((180 - r1) < 0 || (180 + r1) > 0) && Math.Abs(Math.Abs(dy / trans1[1]) + Math.Abs(s / (s + trans1[2]))) - 1 > 0.1)
                        return null;
                }
                if (((180 - r2) > 0 || (180 + r2) < 0) && Math.Abs(Math.Abs(trans2[1] / dt) + Math.Abs(trans2[2] / s) - 1) > 0.1)
                {
                    return null;
                }
                else
                {
                    if (((180 - r2) < 0 || (180 + r2) > 0) && Math.Abs(Math.Abs(dt / trans2[1]) + Math.Abs(Math.Abs(trans2[2]) / (s + Math.Abs(trans2[2])))) - 1 > 0.1)
                        return null;
                }
            }
            result[0] = float.Parse(result[0].ToString("0.00"));
            result[1] = float.Parse(result[1].ToString("0.00"));
            result[2] = -float.Parse(result[2].ToString("0.00"));

            return result;
        }

        public static float[] TransCoor(float dc, float angle, float[] coor)
        {

            float[] A = new float[3];
            float PI_180 = (float)(Math.PI / 180);
            float rota = angle * PI_180;
            float sin_r = (float)Math.Sin(-rota);
            float cos_r = (float)Math.Cos(-rota);
            A[0] = coor[0] * cos_r - coor[2] * sin_r + dc * sin_r;
            A[1] = coor[1];
            A[2] = coor[0] * sin_r + coor[2] * cos_r + dc * (1 - cos_r);
            return A;
        }

        public static float[] interPoint(float[,] points)
        {
            float S0 = 0;
            float[] aPoint = new float[3];
            float X12 = points[0, 0] - points[0, 1];
            float X43 = points[0, 3] - points[0, 2];
            float X42 = points[0, 3] - points[0, 1];

            float Y12 = points[1, 0] - points[1, 1];
            float Y43 = points[1, 3] - points[1, 2];
            float Y42 = points[1, 3] - points[1, 1];

            float Z12 = points[2, 0] - points[2, 1];
            float Z43 = points[2, 3] - points[2, 2];
            float Z42 = points[2, 3] - points[2, 1];

            if (Math.Abs(Z43 * X12 - X43 * Z12) > 0)
                S0 = (Z42 * X12 - X42 * Z12) / (Z43 * X12 - X43 * Z12);

            aPoint[0] = points[0, 3] - X43 * S0;
            aPoint[1] = points[1, 3] - Y43 * S0;
            aPoint[2] = points[2, 3] - Z43 * S0;
            return aPoint;
        }
#endregion

    }
}
