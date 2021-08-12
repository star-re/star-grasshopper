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
        //Administrator  l9815
        public static GH_Structure<GH_Number> Mergedata(GH_Structure<GH_Number> Numbers, double Tolarance, int dec, bool iflist = false)
        {
            GH_Structure<GH_Number> gh_Structure = new GH_Structure<GH_Number>();
            checked
            {
                GH_Structure<GH_Number> result;
                if (Numbers.Branches.Count == 0)
                {
                    result = Numbers;
                }
                else if (Numbers.Branches.Count == 1)
                {
                    if (Numbers.DataCount <= 1)
                    {
                        result = Numbers;
                    }
                    else
                    {
                        GH_Structure<GH_Number> gh_Structure2 = Numbers.Duplicate();
                        gh_Structure2.Graft(3);
                        result = Mergedata(gh_Structure2, Tolarance, dec, true);
                    }
                }
                else
                {
                    int count = Numbers.get_Branch(0).Count;
                    int num = Numbers.Branches.Count - 1;
                    for (int i = 0; i <= num; i++)
                    {
                        if (Numbers.get_Branch(i).Count != count)
                        {
                            Interaction.MsgBox("不同树干的果实数量不全相等，请核对数据。", MsgBoxStyle.OkOnly, "SEG");
                            return Numbers;
                        }
                    }
                    int num2 = count - 1;
                    for (int j = 0; j <= num2; j++)
                    {
                        List<double> list = new List<double>();
                        List<int> list2 = new List<int>();
                        List<Point3d> list3 = new List<Point3d>();
                        int num3 = Numbers.Branches.Count - 1;
                        for (int k = 0; k <= num3; k++)
                        {
                            GH_Number gh_Number = (GH_Number)Numbers.get_Branch(k)[j];
                            list.Add(gh_Number.Value);
                            list2.Add(k);
                        }
                        Array array = list.ToArray();
                        Array array2 = list2.ToArray();
                        Array.Sort(array, array2);
                        try
                        {
                            foreach (object value in array)
                            {
                                double num4 = Conversions.ToDouble(value);
                                Point3d item = new Point3d(num4, 0.0, 0.0);
                                list3.Add(item);
                            }
                        }
                        finally
                        {
                            IEnumerator enumerator;
                            if (enumerator is IDisposable)
                            {
                                (enumerator as IDisposable).Dispose();
                            }
                        }
                        Array array3 = SegPointUtility.GroupPointByGH(list3, Tolarance, SegPointUtility.CullMode.Average).ToArray();
                        Array.Sort(array2, array3);
                        int num5 = array3.Length - 1;
                        for (int l = 0; l <= num5; l++)
                        {
                            object value3;
                            object[] array4;
                            bool[] array5;
                            object value2 = NewLateBinding.LateGet(null, typeof(Math), "Round", array4 = new object[]
                            {
                                NewLateBinding.LateGet(value3 = array3.GetValue(l), null, "X", new object[0], null, null, null),
                                dec
                            }, null, null, array5 = new bool[]
                            {
                                true,
                                true
                            });
                            if (array5[0])
                            {
                                NewLateBinding.LateSetComplex(value3, null, "X", new object[]
                                {
                                    array4[0]
                                }, null, null, true, true);
                            }
                            if (array5[1])
                            {
                                dec = (int)Conversions.ChangeType(RuntimeHelpers.GetObjectValue(array4[1]), typeof(int));
                            }
                            double num6 = Conversions.ToDouble(value2);
                            gh_Structure.Append(new GH_Number(num6), new GH_Path(l));
                        }
                    }
                    if (iflist)
                    {
                        gh_Structure.Flatten(null);
                    }
                    result = gh_Structure;
                }
                return result;
            }
        }
    }
}