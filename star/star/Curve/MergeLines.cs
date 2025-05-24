using Grasshopper.Kernel;
using GKG = Grasshopper.Kernel.Geometry;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace star
{
    public class MergeLines : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MergeLines class.
        /// </summary>
        public MergeLines()
          : base("MergeLines", "Nickname",
              "Description",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Lines", "L", "线段集", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tol", "T", "公差", GH_ParamAccess.item, 0.01);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Lines", "L", "线段集", GH_ParamAccess.list);
        }

        //public void Dispose()
        //{
        //    Dispose
        //}
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> curves = new List<Curve>();
            DA.GetDataList(0, curves);
            DA.GetData(1, ref Tol);

            domain = new Interval(-Tol, Tol);
            
            List<List<int>> vectorMergeList = Vector3dMergeList(curves);
            List<List<Curve>> curves1 = new List<List<Curve>>();
            foreach (var item in vectorMergeList)
            {
                curves1.Add(item.Select(i => curves[i]).ToList());
            }
            List<List<int>> vectorEquals = new List<List<int>>();
            DataTree<int> tree = new DataTree<int>();
            List<Curve> result = new List<Curve>();
            foreach (var item in curves1)
            {
                result.AddRange(_MergeLines(item, IntersectionIndex(item)));
            }
            DA.SetDataList(0, result);
            //GC.WaitForPendingFinalizers();
            //GC.Collect();
        }

        public double Tol = 0.01;
        public Interval domain = new Interval();

        public List<List<int>> Vector3dMergeList(List<Curve> curves)
        {
            List<List<int>> EqIndex = new List<List<int>>();
            List<int> Eq = new List<int>() { 0 };
            List<int> EqIndexCon = new List<int>() { 0 };
            int thisi = 0;
            for (int i = 1; i < curves.Count; i++)
            {
                if (EqIndexCon.Count == curves.Count || thisi == curves.Count)
                {
                    break;
                }
                if (EqIndexCon.Contains(i))
                {
                    if (i >= thisi)
                        thisi++;
                    continue;
                }
                double aa = Rhino.RhinoMath.ToDegrees(Vector3d.VectorAngle(curves[thisi].TangentAtStart, curves[i].TangentAtStart));
                if (aa == 0 || aa == 180)
                {
                    Eq.Add(i);
                    EqIndexCon.Add(i);
                }
                if (EqIndexCon.Count != curves.Count && i + 1 == curves.Count)
                {
                    if (Eq.Count == 0)
                    {
                        thisi++;
                        i = 0;
                    }
                    else
                    {
                        EqIndex.Add(new List<int>(Eq));
                        Eq.Clear();
                        i = 0;
                    }
                }
                if (EqIndexCon.Count == curves.Count)
                {
                    EqIndex.Add(new List<int>(Eq));
                    Eq.Clear();
                    i = 0;
                    break;
                }
            }
            return EqIndex;
        }

        public List<List<int>> IntersectionIndex(List<Curve> curves)
        {

            Circle cir = Circle.Unset;
            Circle.TryFitCircleToPoints(curves.Select(i => i.PointAtStart), out cir);
            //List<List<int>> EqIndex = new List<List<int>>();
            List<int> Eq = new List<int>() { 0 };
            List<int> EqIndexCon = new List<int>() { 0 };
            List<double> Close = new List<double>();
            Vector3d v3 = Vector3d.CrossProduct(curves[0].TangentAtStart, Vector3d.ZAxis);
            v3.Unitize();
            v3 *= cir.Radius * 2;
            Point3d p31 = curves[0].PointAtStart;
            p31.Transform(Transform.Translation(v3));
            Point3d p32 = curves[0].PointAtStart;
            p32.Transform(Transform.Translation(-v3));
            Line line = new Line(p31, p32);
            for (int i = 0; i < curves.Count; i++)
            {
                Close.Add(line.ClosestParameter(curves[i].PointAt(curves[i].GetLength() / 2)));
            }
            var EqIndex = Close
                 .Select(i => Math.Round(i, 3))
                 .Select((num, index) => new { num, index })
                 .GroupBy(i => i.num)
                 .Select(j => j
                 .Select(k => k.index).ToList()).ToList();
            return EqIndex;
        }

        public List<Curve> _MergeLines(List<Curve> curves, List<List<int>> indexs)
        {
            List<Curve> result = new List<Curve>();
            for (int i = 0; i < indexs.Count; i++)
            {
                List<Curve> tempcurves = indexs[i].Select(ii => curves[ii]).ToList();
                result.AddRange(ReMergeLine(tempcurves));
            }
            return result;
        }

        public List<Curve> ReMergeLine(List<Curve> curves)
        {
            List<Curve> result = new List<Curve>();
            int CCount = curves.Count;
            List<int> Con = new List<int>() { 0 };
            int Ri = 0;
            for (int i = 0; i < curves.Count - 1; i++)
            {
                if (Ri > curves.Count)
                {
                    Ri = 0;
                }
                if (Ri == curves.Count || Ri == CCount - 1 || curves.Count == 1)
                {
                    break;
                }
                int ii = i + 1;
                if (Ri == ii)
                {
                    continue;
                }
                List<Curve> tempCurve = MergeLine(curves[Ri], curves[ii]);
                if (tempCurve.Count == 1)
                {
                    curves[Ri] = tempCurve[0];
                    i--;
                    curves.RemoveAt(ii);
                    tempCurve.Clear();
                }

                if (ii == curves.Count - 1)
                {
                    i = -2;
                    Ri++;
                }
            }
            return curves;
        }

        public List<Curve> MergeLine(Curve c1, Curve c2)
        {
            c1.Domain = new Interval(0, 1);
            c2.Domain = new Interval(0, 1);
            List<Curve> result = new List<Curve>();
            Curve RCurve = null;
            if (LineInLine(c1, c2, out RCurve))
            {
                result.Add(RCurve);
                return result;
            }

            Point3d p31 = MergeLineToPoint(c1, c2);
            if (!p31.IsValid)
            {
                result.Add(c1);
                result.Add(c2);
                return result;
            }
            Point3d p32 = MergeLineToPoint(c2, c1);
            result.Add(new Line(p31, p32).ToNurbsCurve());
            return result;
        }

        public bool LineInLine(Curve c1, Curve c2, out Curve c3)
        {
            if (c1.GetLength() > c2.GetLength())
            {
                c3 = c1;
            }
            else
            {
                c3 = c2;
            }
            c1.Domain = new Interval(0, 1);
            c2.Domain = new Interval(0, 1);

            double Tt1 = double.NaN;
            c1.ClosestPoint(c2.PointAtStart, out Tt1);
            double dis1 = c1.PointAt(Tt1).DistanceTo(c2.PointAtStart);

            double Tt2 = double.NaN;
            c1.ClosestPoint(c2.PointAtEnd, out Tt2);
            double dis2 = c1.PointAt(Tt2).DistanceTo(c2.PointAtEnd);
            if (domain.IncludesParameter(dis1) && domain.IncludesParameter(dis2))
            {
                return true;
            }
            double Tt3 = double.NaN;
            c2.ClosestPoint(c1.PointAtStart, out Tt3);
            double dis3 = c2.PointAt(Tt3).DistanceTo(c1.PointAtStart);

            double Tt4 = double.NaN;
            c2.ClosestPoint(c1.PointAtEnd, out Tt4);
            double dis4 = c2.PointAt(Tt1).DistanceTo(c1.PointAtEnd);
            if (domain.IncludesParameter(dis3) && domain.IncludesParameter(dis4))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Point3d MergeLineToPoint(Curve c1, Curve c2)
        {
            c1.Domain = new Interval(0, 1);
            c2.Domain = new Interval(0, 1);
            double Tt1 = double.NaN;
            c1.ClosestPoint(c2.PointAtStart, out Tt1);
            double dis1 = c1.PointAt(Tt1).DistanceTo(c2.PointAtStart);
            if (domain.IncludesParameter(dis1))
            {
                return c2.PointAtEnd;
            }
            double Tt2 = double.NaN;
            c1.ClosestPoint(c2.PointAtEnd, out Tt2);
            double dis2 = c1.PointAt(Tt2).DistanceTo(c2.PointAtEnd);
            if (domain.IncludesParameter(dis2))
            {
                return c2.PointAtStart;
            }
            else
            {
                return Point3d.Unset;
            }
        }
        //public List<List<int>> IntersectionIndex(List<Curve> curves)
        //{
        //    List<List<int>> EqIndex = new List<List<int>>();
        //    List<int> Eq = new List<int>() { 0 };
        //    List<int> EqIndexCon = new List<int>() { 0 };
        //    int thisi = 0;
        //    for (int i = 1; i < curves.Count; i++)
        //    {
        //        if (EqIndexCon.Count == curves.Count || thisi == curves.Count)
        //        {
        //            break;
        //        }
        //        if (EqIndexCon.Contains(i))
        //        {
        //            if (i >= thisi)
        //                thisi++;
        //            continue;
        //        }
        //        Curve c1 = curves[thisi];
        //        Curve c2 = curves[i];
        //        //double c1Len = c1.GetLength();
        //        //double c2Len = c2.GetLength();
        //        //if (c1.PointAt(c1Len / 2).DistanceTo(c2.PointAt(c2Len / 2)) < c1.GetLength())
        //        //{
        //        //    continue;
        //        //}

        //        //Point3d p31 = c1.PointAtStart;
        //        //Point3d p32 = c1.PointAtEnd;
        //        //Point3d p33 = c2.PointAtStart;
        //        //Plane pl = new Plane(p31, p32, p33);
        //        //return !pl.IsValid;
        //        //bool flag = 

        //        double aa = Rhino.RhinoMath.ToDegrees(Vector3d.VectorAngle(c1.TangentAtStart, (c1.PointAtStart - c2.PointAt(c2.GetLength() / 2))));
        //        if (aa == 0 || aa == 180)
        //        {
        //            Eq.Add(i);
        //            EqIndexCon.Add(i);
        //        }
        //        if (EqIndexCon.Count != curves.Count && i + 1 == curves.Count)
        //        {
        //            if (Eq.Count == 0)
        //            {
        //                thisi++;
        //                i = 0;
        //            }
        //            else
        //            {
        //                EqIndex.Add(new List<int>(Eq));
        //                Eq.Clear();
        //                i = 0;
        //            }
        //        }
        //        if (EqIndexCon.Count == curves.Count)
        //        {
        //            EqIndex.Add(new List<int>(Eq));
        //            Eq.Clear();
        //            i = 0;
        //            break;
        //        }
        //    }
        //    return EqIndex;
        //}
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.MergeLines;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b3f0df6e-824b-416d-875b-4e897b949a64"); }
        }
    }
}