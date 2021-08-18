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
        public string  unitstring(string a)
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

        #region MergeSort
        public static void MergeSort(List<double> array)
        {
            MergeSort(array, 0, array.Count - 1);  //在调用里调用Mergesort并赋值（array，起始值，数组-1的长度）
        }

        private static void MergeSort(List<double> array, int p, int a) //被调用方法（array是赋值的数组，p代表起始值赋值，a代表数组-1的长度，对应上面）
        {
            if (p < a)  //如果，起始值小于数组-1的长度
            {
                int q = (p + a) / 2;    //找出数组中心
                MergeSort(array, p, q);  //左数组递归分割
                MergeSort(array, q + 1, a);  //右数组递归分割
                Merge(array, p, q, a);
            }
        }

        private static void Merge(List<double> array, int p, int q, int r)
        {
            double[] L = new double[q - p + 2];
            double[] R = new double[r - q + 1];
            L[q - p + 1] = int.MaxValue;
            R[r - q] = int.MaxValue;

            for (int i = 0; i < q - p + 1; i++)
            {
                L[i] = array[p + i];
            }

            for (int i = 0; i < r - q; i++)
            {
                R[i] = array[q + 1 + i];
            }

            int j = 0;
            int k = 0;
            for (int i = 0; i < r - p + 1; i++)
            {
                if (L[j] <= R[k])
                {
                    array[p + i] = L[j];
                    j++;
                }
                else
                {
                    array[p + i] = R[k];
                    k++;
                }
            }
        }
        #endregion
        //Administrator  l9815
    }

    class starcurvedy
    {
        #region 提取曲线的中心点
        public static Point3d curvecenter(Curve curve)
        {
            curve.Domain = new Interval(0, 1);
            Point3d pp=curve.PointAt(0.5);
            return pp;
        }

        public static List<Point3d> curvecenter(List<Curve> curves)
        {
            List<Point3d> listpoints = new List<Point3d>();
            for (int i = 0; i < curves.Count; i++)
            {
                curves[i].Domain = new Interval(0, 1);
                listpoints.Add(curves[i].PointAt(0.5));
            }
            return listpoints;
        }


        public static List<Point3d> curvecenter(Curve[] curves)
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


    }
}