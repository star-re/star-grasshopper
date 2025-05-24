using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace star
{
    class stardy
    {
        #region 单位文字群
        public string unitstring(string a)
        {
            string result = null;
            if (a == "Millimeters")
            {
                result = "毫米：";
            }
            else if (a == "Centimeters")
            {
                result = "厘米：";
            }
            else if (a == "Meters")
            {
                result = "米：";
            }
            else if (a == "Microns")
            {
                result = "微米：";
            }
            else if (a == "Kilometers")
            {
                result = "公里：";
            }
            else if (a == "Microinches")
            {
                result = "微英寸：";
            }
            else if (a == "Mils")
            {
                result = "密尔：";
            }
            else if (a == "Inches")
            {
                result = "英寸：";
            }
            else if (a == "Feet")
            {
                result = "英尺：";
            }
            else if (a == "Miles")
            {
                result = "英里：";
            }
            else if (a == "Angstroms")
            {
                result = "埃：";
            }
            else if (a == "Nanometers")
            {
                result = "纳米：";
            }
            else if (a == "Decimeters")
            {
                result = "分米：";
            }
            else if (a == "Dekameters")
            {
                result = "十米：";
            }
            else if (a == "Hectometers")
            {
                result = "百米：";
            }
            else if (a == "Megameters")
            {
                result = "兆米：";
            }
            else if (a == "Gigameters")
            {
                result = "吉米：";
            }
            else if (a == "Yards")
            {
                result = "码：";
            }
            else if (a == "PrinterPoints")
            {
                result = "印刷 点：";
            }
            else if (a == "PrinterPicas")
            {
                result = "印刷 派卡：";
            }
            else if (a == "NauticalMiles")
            {
                result = "海里：";
            }
            else if (a == "AstronomicalUnits")
            {
                result = "天文单位：";
            }
            else if (a == "LightYears")
            {
                result = "光年：";
            }
            else if (a == "Parsecs")
            {
                result = "秒差：";
            }
            else if (a == "CustomUnits")
            {
                result = "自定义单位：";
            }
            else if (a == "None")
            {
                result = "没有单位：";
            }
            return result;
        }
        #endregion

        #region GCD&LCM
        public static int GCD(List<int> listOri)
        {
            List<int> list = new List<int>(listOri);

            int c = 1;
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i - 1] < list[i]) //确定a>b
                {
                    list[i - 1] = list[i - 1] + list[i];
                    list[i] = list[i - 1] - list[i];
                    list[i - 1] = list[i - 1] - list[i];
                }
                for (c = list[i]; c >= 1; c--)
                {
                    if (list[i - 1] % c == 0 && list[i] % c == 0)
                        break;
                }
                list[i] = c;
            }
            return c;
        }
        public static int LCM(List<int> listOri)
        {
            List<int> list = new List<int>(listOri);

            int c = 1;
            for (int i = 1; i < list.Count; i++)
            {
                var lcm = list[i - 1] * list[i];
                var max = list[i - 1] > list[i] ? list[i - 1] : list[i];
                for (c = max; c <= lcm; c++)
                {
                    if (c % list[i - 1] == 0 && c % list[i] == 0)
                    {
                        list[i - 1] = c;
                        break;
                    }
                }
                list[i] = c;
            }
            return c;
        }
        #endregion


        //Administrator  l9815

        /// <summary>
        /// 求点群均点
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public Point3d Pointaverage(List<Point3d> points)
        {
            Console.WriteLine();
            List<double> xa = new List<double>();
            List<double> ya = new List<double>();
            List<double> za = new List<double>();
            for (int i = 0; i < points.Count; i++)
            {
                xa.Add(points[i].X);
                ya.Add(points[i].Y);
                za.Add(points[i].Z);
            }
            double xx = xa.Average();
            double yy = ya.Average();
            double zz = za.Average();
            return new Point3d(xx, yy, zz);
        }
    }

    class starcurvedy
    {
        #region 提取曲线的中心点
        public Point3d curvecenter(Curve curve)
        {
            curve.Domain = new Interval(0, 1);
            Point3d pp = curve.PointAt(0.5);
            return pp;
        }

        public List<Point3d> curvecenter(List<Curve> curves)
        {
            List<Point3d> listpoints = new List<Point3d>();
            for (int i = 0; i < curves.Count; i++)
            {
                curves[i].Domain = new Interval(0, 1);
                listpoints.Add(curves[i].PointAt(0.5));
            }
            return listpoints;
        }

        public List<Point3d> curvecenter(Curve[] curves)
        {
            List<Point3d> curveMid = new List<Point3d>();
            for (int i = 0; i < curves.Length; i++)
            {
                curves[i].Domain = new Interval(0, 1);
                curveMid.Add(curves[i].PointAt(0.5));
            }
            return curveMid;
        }
        #endregion

        #region 曲线Flip
        public static bool IsAligned(Curve curve, Curve BaseCurve)
        {
            int num = curve.SpanCount * Math.Min(curve.Degree, 3);
            int num2 = 0;
            for (int i = 0; i < num; i++)
            {
                double num3 = curve.Domain.ParameterAt((double)i / (double)num);
                Point3d point3d = curve.PointAt(num3);
                Vector3d vector3d = curve.TangentAt(num3);
                double num4 = double.NaN;
                if (BaseCurve.ClosestPoint(point3d, out num4))
                {
                    Vector3d vector3d2 = BaseCurve.TangentAt(num4);
                    if (vector3d.IsParallelTo(vector3d2, 1.5707963267948966) >= 0)
                    {
                        num2++;
                    }
                    else
                    {
                        num2--;
                    }
                }
            }
            return num2 >= 0;
        }

        public static bool CurveFlipFlag(Curve curve, Curve curve2)
        {
            bool result = false;
            bool flag = !curve2.IsClosed && curve2.IsLinear(curve2.GetLength() * 0.01);
            if (flag)
            {
                Vector3d vector3d = curve2.PointAtEnd - curve2.PointAtStart;
                if (curve.IsClosed)
                {
                    result = false;
                    return result;
                }
                Vector3d vector3d2 = curve.PointAtEnd - curve.PointAtStart;
                if (vector3d.IsParallelTo(vector3d2, 1.5707963267948966) == 1)
                {
                    result = false;
                    return result;
                }
                result = true;
            }
            else
            {
                Plane plane = CurvePlane(curve);
                Plane plane2 = CurvePlane(curve2);
                Polyline cp = curve.ToNurbsCurve().Points.ControlPolygon();
                Polyline cp2 = curve2.ToNurbsCurve().Points.ControlPolygon();
                int num = plane.ZAxis.IsParallelTo(plane2.ZAxis, 0.7853981633974483);
                if (num != -1)
                {
                    if (num != 1)
                    {
                        if (IsAligned(curve, curve2))
                        {
                            result = false;
                            return result;
                        }
                        result = true;
                    }
                    else
                    {
                        bool flag2 = IsClockwise(cp, plane);
                        bool flag3 = IsClockwise(cp2, plane2);
                        if (flag2 == flag3)
                        {
                            result = false;
                            return result;
                        }
                        result = true;
                    }
                }
                else
                {
                    bool flag4 = IsClockwise(cp, plane);
                    bool flag5 =IsClockwise(cp2, plane2);
                    if (flag4 != flag5)
                    {
                        result = false;
                        return result;
                    }
                    result = true;
                }
            }
            return result;
        }
        private static bool IsClockwise(Polyline cp, Plane plane)
        {
            cp.Transform(Transform.PlaneToPlane(plane, Plane.WorldXY));
            cp.Transform(Transform.PlanarProjection(Plane.WorldXY));
            if (!cp.IsClosed)
            {
                cp.Add(cp[0]);
            }
            double num = 0.0;
            for (int i = 0; i < cp.Count - 1; i++)
            {
                double num2 = cp[i + 1].X - cp[i].X;
                double num3 = cp[i + 1].Y + cp[i].Y;
                num += num2 * num3;
            }
            return num > 0.0;
        }
        private static Plane CurvePlane(Curve curve)
        {
            Plane result = Plane.Unset;
            if (curve.TryGetPlane(out result, curve.GetLength() * 0.01))
            {
                return result;
            }
            Polyline polyline = curve.ToNurbsCurve().Points.ControlPolygon();
           var aa =  Plane.FitPlaneToPoints(polyline, out result);
            //if (Plane.FitPlaneToPoints(polyline, out result) == null)
            //{
            //    return result;
            //}
            return result;
        }

        #endregion

        #region 曲线顺时针
        public bool IsAligned(Curve curve)
        {
            int num = curve.SpanCount * Math.Min(curve.Degree, 3);
            int num2 = 0;
            Point3d point3D = curve.GetBoundingBox(true).Center;
            Circle circle = new Circle(point3D, 0.5);
            Curve guide = circle.ToNurbsCurve();
            guide.Reverse();
            for (int i = 0; i < num; i++)
            {
                double t = curve.Domain.ParameterAt((double)i / (double)num);
                Point3d testPoint = curve.PointAt(t);
                Vector3d vector3d = curve.TangentAt(t);
                double t2;
                if (guide.ClosestPoint(testPoint, out t2))
                {
                    Vector3d other = guide.TangentAt(t2);
                    if (vector3d.IsParallelTo(other, 1.5707963267948966) >= 0)
                    {
                        num2++;
                    }
                    else
                    {
                        num2--;
                    }
                }
            }
            return num2 >= 0;
        }
        #endregion

        #region 判断曲线可炸开吗
        public bool IsExplode(Curve cc)
        {
            Curve[] cd = cc.DuplicateSegments();
            bool re = cd.Length != 0;
            return re;
        }

        public List<bool> IsExplode(List<Curve> cc)
        {
            Curve[] cd = null;
            List<bool> re = new List<bool>();
            for (int i = 0; i < cc.Count; i++)
            {
                cd = cc[i].DuplicateSegments();
                re.Add(cd.Length != 0);
            }
            return re;
        }

        public List<bool> IsExplode(Curve[] cc)
        {
            Curve[] cd = null;
            List<bool> re = new List<bool>();
            for (int i = 0; i < cc.Length; i++)
            {
                cd = cc[i].DuplicateSegments();
                re.Add(cd.Length != 0);
            }
            return re;
        }
        #endregion
    }

    class starMathdy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        #region 角度转弧度
        public static double Radians(double angle)
        {
            double a = angle * Math.PI / 180;
            return a;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        #region 弧度转角度
        public static double Dreeges(double radians)
        {
            double a = 180 / Math.PI * radians;
            return a;
        }
        #endregion

        /// <summary>
        /// 求C弦长度，angle输入角度，height输入高度
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="hei"></param>
        /// <returns></returns>
        #region 求C弦长度
        public double Clength(double angle, double height)
        {
            double a = (height / Math.Tan(Radians(angle)));
            double returndouble = Math.Pow(height, 2) + Math.Pow(a, 2);
            return Math.Sqrt(returndouble);
        }
        #endregion

        /// <summary>
        /// 切换布尔后刷新，第一次调用为true，若不切换则为false
        /// </summary>
        #region 切换布尔后刷新，第一次调用为true，若不切换则为false
        public static int index = 0;
        public static bool BoolFixed(bool bb)
        {
            bbb = bb;
            if (index != 0)
            {
                if (bb == cc)
                {
                    index = 0;
                    cc = !bb;
                    return true;
                }
                return false;
            }
            else
            {
                index++;
                return true;
            }
        }

        public static bool bbb;
        public static bool cc;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public float Q_rsqrt(float number)
        {
            long i = 0L;
            float x2, y;
            const float threehalfs = 1.5f;
            x2 = number * 0.5f;
            y = number;
            i = 0x5f3759df - (i >> 1);
            unsafe
            {
                y = *(float*)&i;
            }
            y = y * (threehalfs - (x2 * y * y));
            return y;
        }

        unsafe public List<bool> Jiou(List<int> i)
        {
            List<bool> bools = new List<bool>();
            for (int j = 0; j < i.Count; j++)
            {
                if ((i[j] >> 1 << 1) == i[j])
                {
                    bools.Add(true);
                }
                else
                {
                    bools.Add(false);
                }
            }
            return bools;
        }

        public List<bool> jIou(List<int> i)
        {
            List<bool> bools = new List<bool>();
            for (int j = 0; j < i.Count; j++)
            {
                if ((i[j] % 2) == 0)
                {
                    bools.Add(true);
                }
                else
                {
                    bools.Add(false);
                }
            }
            return bools;
        }

        unsafe public float SingleSqrt(float number)
        {
            int i;
            float x2, y;
            const float threehalfs = 1.5F;
            x2 = number * 0.5F;
            y = number;
            i = *(int*)&y;   // evil floating point bit level hacking
            i = 0x5f3759df - (i >> 1); // John Carmack's number
            y = *(float*)&i;
            y = y * (threehalfs - (x2 * y * y));
            return 1 / y;
        }
    }
}
