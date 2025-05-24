//using Grasshopper.Kernel;
//using Rhino.Geometry;
//using System;
//using Rhino;
//using System.Collections.Generic;
//using System.Linq;

//namespace star.M1
//{
//    public class BaseDynamicData : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the BaseDynamicData class.
//        /// </summary>
//        public BaseDynamicData()
//          : base("BaseDynamicData", "BaseDynamicData",
//              "Description",
//              "star", "Math")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddBrepParameter("基准曲面", "基准面", "制定排序规则的基准曲面", GH_ParamAccess.item);
//            pManager.AddCurveParameter("基准排序线", "基准线", "制定排序规则的基准曲线，以列表顺序为准", GH_ParamAccess.list);
//            pManager.AddBrepParameter("曲面", "曲面", "被排序的曲面，需与基准曲面的基本形状相同，且曲面方向一致", GH_ParamAccess.item);
//            pManager.AddCurveParameter("曲线", "线", "此接口只需选择基准线里类似形状的第一条曲线即可，且曲线方向一致", GH_ParamAccess.item);
//            pManager.AddCurveParameter("方向线(可空)", "方向线", "可根据此曲线方向定义曲线接口的方向，如不需要也可空", GH_ParamAccess.item);
//            pManager[4].Optional = true;
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddCurveParameter("结果线", "线", "此接口为匹配基准曲面与基准排序线生成的结果", GH_ParamAccess.list);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            Brep baseBrep = new Brep();
//            List<Curve> basecurves = new List<Curve>();
//            DA.GetData(0, ref baseBrep);
//            DA.GetDataList(1, basecurves);

//            /**************************************************************/

//            double[] baseCurveParam = PointInCurveTheParameterA(baseBrep, baseBrep.Edges.Select(i => i.EdgeCurve).ToList(), basecurves[0]);
//            Curve[] baseSurfaceCurves = baseBrep.Edges.Select(i => i.EdgeCurve).ToArray();
//            Array.Sort(baseCurveParam, baseSurfaceCurves);
//            List<int> indexs = new List<int>();
//            List<int> diagonalCau = new List<int>();
//            // List<Curve> democurves = Curve.JoinCurves(baseSurfaceCurves)[0].DuplicateSegments().ToList();
//            List<int> diagonalIndex = CreatediagonalIndex(ExplodeCurveA.ToList(), basecurves, ref diagonalCau);
//            //List<int> diagonalIndex = CreatediagonalIndex(Curve.JoinCurves(baseSurfaceCurves)[0].DuplicateSegments().ToList(), basecurves, ref diagonalCau);
//            for (int i = 0; i < basecurves.Count; i++)
//            {
//                int ind = 0;
//                double flag = double.MaxValue;
//                bool off = true;
//                for (int j = 0; j < baseSurfaceCurves.Length; j++)
//                {
//                    off = false;
//                    if (diagonalCau.Contains(i))
//                    {
//                        off = true;
//                        break;
//                    }
//                    double re = CurveMid(basecurves[i]).DistanceTo(CurveMid(baseSurfaceCurves[j]));
//                    if (flag > re)
//                    {
//                        flag = re;
//                        ind = j;
//                    }
//                }
//                if (off)
//                {
//                    continue;
//                }
//                else
//                {
//                    indexs.Add(ind);
//                }

//            }

//            Brep brep = new Brep();
//            Curve curve = null;
//            Curve flipCurve = null;
//            DA.GetData(2, ref brep);
//            DA.GetData(3, ref curve);
//            DA.GetData(4, ref flipCurve);
//            List<Curve> brepcurves = brep.DuplicateEdgeCurves().ToList();
//            List<Curve> Transitcurves = new List<Curve>();
//            if (flipCurve != null)
//            {
//                curve = FlipSortCurve(curve, flipCurve);
//            }
//            for (int i = 0; i < brepcurves.Count; i++)
//            {
//                starcurvedy.CurveSegments(Transitcurves, brepcurves[i], true);
//                // Explodecurves.AddRange(Transitcurves);
//            }
//            double[] CurveParam = PointInCurveTheParameterB(brep, Transitcurves, curve);
//            //double[] CurveParam = PointInCurveTheParameterB(brep, brep.Edges.Select(i => i.EdgeCurve).ToList(), curve);
//            Curve[] SurfaceCurves = null;
//            List<Curve> Explodecurves = new List<Curve>();
//            for (int i = 0; i < brep.DuplicateEdgeCurves().Length; i++)
//            {
//                starcurvedy.CurveSegments(Explodecurves, brep.DuplicateEdgeCurves()[i], true);
//            }
//            SurfaceCurves = Explodecurves.ToArray();
//            Array.Sort(CurveParam, SurfaceCurves);
//            List<Curve> ResultCurve = new List<Curve>();

//            List<Curve> InsertCurve = new List<Curve>();
//            for (int i = 0; i < indexs.Count; i++)
//            {
//                ResultCurve.Add(SurfaceCurves[indexs[i]]);
//            }
//            int ijk = 0;
//            for (int i = 1; i <= diagonalIndex.Count; i = i + 2)
//            {
//                ResultCurve.Insert(diagonalCau[ijk], new Line(ExplodeCurveB[diagonalIndex[i - 1]].PointAtStart, ExplodeCurveB[diagonalIndex[i]].PointAtStart).ToNurbsCurve());
//                ijk++;
//            }

//            DA.SetDataList(0, ResultCurve);
//        }

//        public static Curve FlipSortCurve(Curve curve, Curve flipCurve)
//        {
//            Point3d curvemid = CurveMid(curve);
//            double curveT = double.NaN;
//            flipCurve.ClosestPoint(curvemid, out curveT);
//            Vector3d vector3D = flipCurve.TangentAt(curveT);
//            Line line = new Line(curvemid, vector3D, 100);
//            if (starcurvedy.CurveFlip(curve, line.ToNurbsCurve()))
//            {
//                curve.Reverse();
//            }
//            return curve;
//        }
//        public static Plane GetPlane(Surface ss, Curve cc)
//        {
//            Plane plane = Plane.Unset;
//            ss.SetDomain(0, new Interval(0, 1));
//            ss.SetDomain(1, new Interval(0, 1));
//            Vector3d ssNor = ss.NormalAt(0.5, 0.5);
//            Vector3d ccVector = cc.PointAtEnd - cc.PointAtStart;
//            Vector3d vectorY = Vector3d.CrossProduct(ccVector, ssNor);
//            plane = new Plane(ss.PointAt(0.5, 0.5), ccVector, -vectorY);
//            return plane;
//        }

//        public static Curve[] ExplodeCurveA { get; set; }
//        public static Curve[] ExplodeCurveB { get; set; }

//        public static double[] PointInCurveTheParameterB(Brep bb, List<Curve> curves, Curve basecurve)
//        {
//            Surface basesurface = bb.Faces[0];
//            double[] Parameters = new double[curves.Count];
//            //Curve Joincurve = Curve.JoinCurves(bb.Edges.Select(i => i.EdgeCurve).ToList())[0];
//            Curve Joincurve = Curve.JoinCurves(bb.DuplicateEdgeCurves())[0];
//            // Joincurve.Domain = new Interval(0, 1);
//            Circle SortCircle = new Circle(GetPlane(basesurface, basecurve), 10);
//            if (starcurvedy.CurveFlip(Joincurve, SortCircle.ToNurbsCurve()))
//            {
//                Joincurve.Reverse();
//            }
//            double curve1Parameter = double.NaN;
//            Joincurve.ClosestPoint(basecurve.PointAtStart, out curve1Parameter);
//            Joincurve.ChangeClosedCurveSeam(curve1Parameter);
//            if (curve1Parameter >= 0)
//            {
//                Joincurve.Domain = new Interval(0, 1);
//            }
//            else
//            {
//                Joincurve.Domain = new Interval(-1, 0);
//            }
//            for (int i = 0; i < curves.Count; i++)
//            {
//                double tt = double.NaN;
//                Joincurve.ClosestPoint(CurveMid(curves[i]), out tt);
//                Parameters[i] = tt;
//            }

//            List<Curve> curves1 = new List<Curve>();
//            for (int i = 0; i < Joincurve.DuplicateSegments().Length; i++)
//            {
//                starcurvedy.CurveSegments(curves1, Joincurve.DuplicateSegments()[i], true);
//            }
//            ExplodeCurveB = curves1.ToArray();
//            return Parameters;
//        }
//        public static double[] PointInCurveTheParameterA(Brep bb, List<Curve> curves, Curve basecurve)
//        {
//            Surface basesurface = bb.Faces[0];
//            double[] Parameters = new double[curves.Count];
//            Curve Joincurve = Curve.JoinCurves(bb.Edges.Select(i => i.EdgeCurve).ToList())[0];
//            Joincurve.Domain = new Interval(0, 1);
//            Circle SortCircle = new Circle(GetPlane(basesurface, basecurve), 10);
//            if (starcurvedy.CurveFlip(Joincurve, SortCircle.ToNurbsCurve()))
//            {
//                Joincurve.Reverse();
//            }
//            double curve1Parameter = double.NaN;
//            Joincurve.ClosestPoint(basecurve.PointAtStart, out curve1Parameter);
//            Joincurve.ChangeClosedCurveSeam(curve1Parameter);
//            if (curve1Parameter >= 0)
//            {
//                Joincurve.Domain = new Interval(0, 1);
//            }
//            else
//            {
//                Joincurve.Domain = new Interval(-1, 0);
//            }
//            for (int i = 0; i < curves.Count; i++)
//            {
//                double tt = double.NaN;
//                Joincurve.ClosestPoint(CurveMid(curves[i]), out tt);
//                Parameters[i] = tt;
//            }
//            List<Curve> curves1 = new List<Curve>();
//            for (int i = 0; i < Joincurve.DuplicateSegments().Length; i++)
//            {
//                starcurvedy.CurveSegments(curves1, Joincurve.DuplicateSegments()[i], true);
//            }
//            ExplodeCurveA = curves1.ToArray();
//            return Parameters;
//        }

//        public static List<int> CreatediagonalIndex(List<Curve> basecurve, List<Curve> Sortcurve, ref List<int> diagonalCu)
//        {
//            List<int> Result = new List<int>();
//            List<int> diagonalIndex = diagonalCul(basecurve, Sortcurve);
//            diagonalCu = diagonalIndex;
//            for (int i = 0; i < diagonalIndex.Count; i++)
//            {
//                int index = int.MaxValue;
//                for (int k = 0; k <= 1; k++)
//                {
//                    double flag = double.MaxValue;
//                    if (k == 0)
//                    {
//                        for (int j = 0; j < basecurve.Count; j++)
//                        {
//                            double dis = Sortcurve[diagonalIndex[i]].PointAtStart.DistanceTo(basecurve[j].PointAtStart);
//                            if (dis < flag)
//                            {
//                                flag = dis;
//                                index = j;
//                            }
//                        }
//                    }
//                    else
//                    {
//                        for (int j = 0; j < basecurve.Count; j++)
//                        {
//                            double dis = Sortcurve[diagonalIndex[i]].PointAtEnd.DistanceTo(basecurve[j].PointAtStart);
//                            if (dis < flag)
//                            {
//                                flag = dis;
//                                index = j;
//                            }
//                        }
//                    }
//                    Result.Add(index);
//                }
//            }
//            return Result;
//        }

//        public static List<int> diagonalCul(List<Curve> basecurve, List<Curve> Sortcurve)
//        {
//            List<int> diagonalIndex = new List<int>();
//            List<int> CullIndex = new List<int>();
//            for (int i = 0; i < Sortcurve.Count; i++)
//            {
//                int ind = int.MinValue;
//                //double flag = double.MaxValue;
//                for (int j = 0; j < basecurve.Count; j++)
//                {
//                    double re = CurveMid(Sortcurve[i]).DistanceTo(CurveMid(basecurve[j]));
//                    if (re == 0 || re < Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance)
//                    {
//                        ind = i;
//                        CullIndex.Add(ind);
//                        continue;
//                    }
//                }
//                if (ind == int.MinValue)
//                {
//                    diagonalIndex.Add(i);
//                }
//            }
//            return diagonalIndex;
//        }

//        public static Point3d CurveMid(Curve cc)
//        {
//            Point3d point3D = cc.PointAt(cc.Domain.Mid);
//            //Point3d point3D = cc.PointAtLength(cc.GetLength() / 2);
//            return point3D;
//        }
//        /// <summary>
//        /// Provides an Icon for the component.
//        /// </summary>
//        /// 
//        private void InitializeComponent()
//        {
//            BofangGif();
//        }
//        protected override System.Drawing.Bitmap Icon
//        {
//            get
//            {
//                InitializeComponent();
//                //You can add image files to your project resources and access them like this:
//                // return Resources.IconForThisComponent;
//                return null;

//            }
//        }

//        public void BofangGif()
//        {
//            System.Drawing.Image image = Properties.Resources.QQ图片20231221232426;
//            System.Windows.Forms.PictureBox pictureBox = new System.Windows.Forms.PictureBox();
//            pictureBox.Location = System.Drawing.Point.Ceiling(this.Attributes.Pivot);
//            pictureBox.Width = 32;
//            pictureBox.Height = 32;
//            pictureBox.Image = image;
//            pictureBox.Show();
//        }
//        /// <summary>
//        /// Gets the unique ID for this component. Do not change this ID after release.
//        /// </summary>
//        public override Guid ComponentGuid
//        {
//            get { return new Guid("7763edd4-ad4a-4a8f-8ef1-da8c28c52c91"); }
//        }
//    }
//}