using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class Curve_Fillet : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Curve_Fillet class.
        /// </summary>
        public Curve_Fillet()
          : base("Curve Fillet", "Curve Fillet",
              "曲线倒角",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "需倒角的曲线", GH_ParamAccess.item);
            pManager.AddNumberParameter("Radius", "R", "半径", GH_ParamAccess.item, 1.5);
            pManager.AddNumberParameter("Omit", "O", "省略掉过小的曲线，默认为半径*2", GH_ParamAccess.item);
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "曲线结果", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
             
            Curve curve = null;
            double Radius = double.NaN;
            double Omit = Radius * 2;
            DA.GetData(0, ref curve);
            DA.GetData(1, ref Radius);
            DA.GetData(2, ref Omit);
            double.IsNaN
            Curve result = CurveFillet2(curve, Radius, Omit);
            DA.SetData(0, result);
        }

        public static Curve CurveToDomain(Curve c)
        {
            Curve dp1 = c;
            dp1.Domain = new Interval(0, 1);
            return dp1;
        }

        public static Curve CurveFillet(Curve _Curve, double _Radius, double omit)
        {
            List<Curve> dups = _Curve.DuplicateSegments().ToList();
            if (dups.Count == 0 || dups.Count == 1)
            {
                return _Curve;
            }
            for (int i = 0; i < dups.Count; i++)
            {
                if (dups[i].GetLength() < _Radius * 2)
                {
                    dups.RemoveAt(i);
                }
            }
            Polyline point3Ds = new Polyline();
            Curve[] fillet = null;
            Line line = new Line();
            line.ClosestPoint
            List<Curve> curves = new List<Curve>();
            for (int i = 1; i < dups.Count + 1; i++)
            {
                if (i == 1)
                {
                    Point3d p0 = Point3d.Unset;
                    Point3d p1 = Point3d.Unset;
                    List<double> DisS = new List<double>();
                    Point3d p3aS = dups[i - 1].PointAtStart;
                    Point3d p3aE = dups[i - 1].PointAtEnd;
                    Point3d p3bS = dups[i].PointAtStart;
                    Point3d p3bE = dups[i].PointAtEnd;
                    DisS.Add(p3aS.DistanceTo(p3bS));
                    DisS.Add(p3aS.DistanceTo(p3bE));
                    DisS.Add(p3aE.DistanceTo(p3bS));
                    DisS.Add(p3aE.DistanceTo(p3bE));
                    Curve dp1 = CurveToDomain(dups[i - 1]);
                    Curve dp2 = CurveToDomain(dups[i]);
                    int index = DisS.IndexOf(DisS.Min());
                    switch (index)
                    {
                        case 0:
                            p0 = dp1.PointAt(0.05);
                            p1 = dp2.PointAt(0.05);
                            break;
                        case 1:
                            p0 = dp1.PointAt(0.05);
                            p1 = dp2.PointAt(0.95);
                            break;
                        case 2:
                            p0 = dp1.PointAt(0.95);
                            p1 = dp2.PointAt(0.05);
                            break;
                        case 3:
                            p0 = dp1.PointAt(0.95);
                            p1 = dp2.PointAt(0.95);
                            break;
                    }
                    fillet = Curve.CreateFilletCurves(dp1, p0, dp2, p1, _Radius, false, true, true, 0.01, 0.01);
                    Curve c1 = fillet[1];
                    Curve c2 = fillet[2];
                    fillet[1] = c2;
                    fillet[2] = c1;
                    curves.AddRange(fillet);
                }
                else if (i == dups.Count)
                {
                    Point3d p0 = Point3d.Unset;
                    Point3d p1 = Point3d.Unset;
                    List<double> DisS = new List<double>();
                    Point3d p3aS = curves[curves.Count - 1].PointAtStart;
                    Point3d p3aE = curves[curves.Count - 1].PointAtEnd;
                    Point3d p3bS = curves[0].PointAtStart;
                    Point3d p3bE = curves[0].PointAtEnd;
                    DisS.Add(p3aS.DistanceTo(p3bS));
                    DisS.Add(p3aS.DistanceTo(p3bE));
                    DisS.Add(p3aE.DistanceTo(p3bS));
                    DisS.Add(p3aE.DistanceTo(p3bE));
                    Curve dp1 = CurveToDomain(curves[curves.Count - 1]);
                    Curve dp2 = CurveToDomain(curves[0]);
                    int index = DisS.IndexOf(DisS.Min());
                    switch (index)
                    {
                        case 0:
                            p0 = dp1.PointAt(0.05);
                            p1 = dp2.PointAt(0.05);
                            break;
                        case 1:
                            p0 = dp1.PointAt(0.05);
                            p1 = dp2.PointAt(0.95);
                            break;
                        case 2:
                            p0 = dp1.PointAt(0.95);
                            p1 = dp2.PointAt(0.05);
                            break;
                        case 3:
                            p0 = dp1.PointAt(0.95);
                            p1 = dp2.PointAt(0.95);
                            break;
                    }
                    fillet = Curve.CreateFilletCurves(curves[curves.Count - 1], p0, curves[0], p1, _Radius, false, true, true, 0.01, 0.01);
                    Curve c1 = fillet[1];
                    Curve c2 = fillet[2];
                    fillet[1] = c2;
                    fillet[2] = c1;
                    curves.RemoveAt(curves.Count - 1);
                    curves.RemoveAt(0);
                    curves.AddRange(fillet);
                }
                else
                {
                    Point3d p0 = Point3d.Unset;
                    Point3d p1 = Point3d.Unset;
                    List<double> DisS = new List<double>();
                    Point3d p3aS = curves[curves.Count - 1].PointAtStart;
                    Point3d p3aE = curves[curves.Count - 1].PointAtEnd;
                    Point3d p3bS = dups[i].PointAtStart;
                    Point3d p3bE = dups[i].PointAtEnd;
                    DisS.Add(p3aS.DistanceTo(p3bS));
                    DisS.Add(p3aS.DistanceTo(p3bE));
                    DisS.Add(p3aE.DistanceTo(p3bS));
                    DisS.Add(p3aE.DistanceTo(p3bE));
                    Curve dp1 = CurveToDomain(curves[curves.Count - 1]);
                    Curve dp2 = CurveToDomain(dups[i]);
                    int index = DisS.IndexOf(DisS.Min());
                    switch (index)
                    {
                        case 0:
                            p0 = dp1.PointAt(0.05);
                            p1 = dp2.PointAt(0.05);
                            break;
                        case 1:
                            p0 = dp1.PointAt(0.05);
                            p1 = dp2.PointAt(0.95);
                            break;
                        case 2:
                            p0 = dp1.PointAt(0.95);
                            p1 = dp2.PointAt(0.05);
                            break;
                        case 3:
                            p0 = dp1.PointAt(0.95);
                            p1 = dp2.PointAt(0.95);
                            break;
                    }
                    fillet = Curve.CreateFilletCurves(curves[curves.Count - 1], p0, dp2, p1, _Radius, false, true, true, 0.01, 0.01);
                    Curve c1 = fillet[1];
                    Curve c2 = fillet[2];
                    fillet[1] = c2;
                    fillet[2] = c1;
                    curves.RemoveAt(curves.Count - 1);
                    curves.AddRange(fillet);
                }
            }
            Curve result = Curve.JoinCurves(curves)[0];
            return result;
        }

        public static Curve CurveFillet2(Curve _Curve, double _Radius, double omit)
        {
            if (omit.ToString() == double.NaN.ToString())
            {
                omit = _Radius * 2;
            }
            List<Curve> dups = _Curve.DuplicateSegments().ToList();
            if (dups.Count == 0 || dups.Count == 1)
            {
                return _Curve;
            }
            for (int i = 0; i < dups.Count; i++)
            {
                if (dups[i].GetLength() < omit)
                {
                    dups.RemoveAt(i);
                }
            }
            Curve[] filletArray = null;
            Curve filletcrv1 = null;
            Curve filletcrv2 = null;
            int flag = 0;
            int Dupindex = dups.Count;
            if (_Curve.IsClosed)
            {
                Dupindex = Dupindex + 1;
            }
            List<Curve> curves = new List<Curve>();
            for (int i = 1; i < Dupindex; i++)
            {
                if (i == 1)
                {
                    filletcrv1 = dups[i - 1];
                    filletcrv2 = dups[i];
                    flag = 0;
                }
                else if (i == dups.Count && _Curve.IsClosed)
                {
                    filletcrv1 = curves[curves.Count - 1];
                    filletcrv2 = curves[0];
                    flag = 1;
                }
                else
                {
                    filletcrv1 = curves[curves.Count - 1];
                    filletcrv2 = dups[i];
                    flag = 2;
                }
                Point3d p0 = Point3d.Unset;
                Point3d p1 = Point3d.Unset;
                List<double> DisS = new List<double>();
                Point3d p3aS = filletcrv1.PointAtStart;
                Point3d p3aE = filletcrv1.PointAtEnd;
                Point3d p3bS = filletcrv2.PointAtStart;
                Point3d p3bE = filletcrv2.PointAtEnd;
                DisS.Add(p3aS.DistanceTo(p3bS));
                DisS.Add(p3aS.DistanceTo(p3bE));
                DisS.Add(p3aE.DistanceTo(p3bS));
                DisS.Add(p3aE.DistanceTo(p3bE));
                Curve dp1 = CurveToDomain(filletcrv1);
                Curve dp2 = CurveToDomain(filletcrv2);
                int index = DisS.IndexOf(DisS.Min());
                switch (index)
                {
                    case 0:
                        p0 = dp1.PointAt(0.05);
                        p1 = dp2.PointAt(0.05);
                        break;
                    case 1:
                        p0 = dp1.PointAt(0.05);
                        p1 = dp2.PointAt(0.95);
                        break;
                    case 2:
                        p0 = dp1.PointAt(0.95);
                        p1 = dp2.PointAt(0.05);
                        break;
                    case 3:
                        p0 = dp1.PointAt(0.95);
                        p1 = dp2.PointAt(0.95);
                        break;
                }
                filletArray = Curve.CreateFilletCurves(dp1, p0, dp2, p1, _Radius, false, true, true, 0.01, 0.01);
                switch (flag)
                {
                    case 0:
                        Curve c1 = filletArray[1];
                        Curve c2 = filletArray[2];
                        filletArray[1] = c2;
                        filletArray[2] = c1;
                        curves.AddRange(filletArray);
                        break;
                    case 1:
                        c1 = filletArray[1];
                        c2 = filletArray[2];
                        filletArray[1] = c2;
                        filletArray[2] = c1;
                        curves.RemoveAt(curves.Count - 1);
                        curves.RemoveAt(0);
                        curves.AddRange(filletArray);
                        break;
                    case 2:
                        c1 = filletArray[1];
                        c2 = filletArray[2];
                        filletArray[1] = c2;
                        filletArray[2] = c1;
                        curves.RemoveAt(curves.Count - 1);
                        curves.AddRange(filletArray);
                        break;
                }

            }
            Curve result = Curve.JoinCurves(curves)[0];
            return result;
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.CurveFillet;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2e76c9de-2c3b-462b-8b68-c85594c5a11f"); }
        }
    }
}