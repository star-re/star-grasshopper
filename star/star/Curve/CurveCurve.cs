using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Rhino.Geometry.Intersect;

namespace star
{
    public class CurveCurve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CurveCurve class.
        /// </summary>
        public CurveCurve()
          : base("Curve|Curve", "CCX",
              "Description",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "线", GH_ParamAccess.item);
            pManager.AddNumberParameter("Length", "Len", "筛选长度", GH_ParamAccess.item, 10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "线", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            double Len = 10;    
            DA.GetData(0, ref curve);
            DA.GetData(1, ref Len);

            DA.SetData(0, JoinCurve(curve, Len)[0]);
        }

        public List<Curve> DispatchCurve(Curve cc, double len)
        {
            Curve[] curves = cc.DuplicateSegments();
            List<Curve> resultCurve = new List<Curve>();
            for (int i = 0; i < curves.Length; i++)
            {
                if (curves[i].GetLength() > len)
                {
                    resultCurve.Add(curves[i]);
                }
            }
            return resultCurve;
        }

        public Curve SplitCurve(Curve cc, Point3d p3, CurveEnd ce)
        {
            double doubleT = 0;
            cc.ClosestPoint(p3, out doubleT);
            Curve[] curves = cc.Split(doubleT);
            Curve result = null;
            switch (ce)
            {
                case CurveEnd.Start:
                    result = curves[0];
                    break;
                case CurveEnd.End:
                    result = curves[1];
                    break;
            }
            return result;
        }

        List<Curve> ShowListCurve = new List<Curve>();
        List<Point3d> ShowListPoint = new List<Point3d>();

        public Point3d CurveIntersectionCurve(Curve c1, Curve c2, int flag)
        {
            CurveIntersections curveinter = Intersection.CurveCurve(c1, c2, 0.01, 0.01);
            Point3d SplitPoint1 = Point3d.Origin;
            if (flag == 2 && curveinter.Count == 2)
            {
                SplitPoint1 = curveinter[1].PointA;
            }
            else
            {
                SplitPoint1 = curveinter[0].PointA;

            }
            return SplitPoint1;
        }

        public Curve[] JoinCurve(Curve cc, double len)
        {
            ShowListCurve.Clear();
            List<Curve> curves = DispatchCurve(cc, len);
            int flag1 = curves.Count;
            if (!cc.IsClosed)
            {
                flag1--;
            }

            int index1 = 0;
            int index2 = 0;
            for (int i = 0; i < flag1; i++)
            {
                if (i == curves.Count - 1)
                {
                    index1 = i;
                    index2 = 0;
                }
                else
                {
                    index1 = i;
                    index2 = i + 1;
                }
                Curve casualCrv1 = curves[index1].Extend(CurveEnd.End, len * 1.1, CurveExtensionStyle.Smooth);
                Curve casualCrv2 = curves[index2].Extend(CurveEnd.Start, len * 1.1, CurveExtensionStyle.Smooth);
                Point3d point1 = CurveIntersectionCurve(casualCrv1, casualCrv2, flag1);
                casualCrv1 = SplitCurve(casualCrv1, point1, CurveEnd.Start);
                casualCrv2 = SplitCurve(casualCrv2, point1, CurveEnd.End);
                curves[index1] = casualCrv1;
                curves[index2] = casualCrv2;
                //ShowListPoint.Add(point1);
            }
            //ShowListCurve.AddRange(curves);
            return Curve.JoinCurves(curves);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("60b1e0b3-0a7a-4e8c-bbd3-0be0c0abeaa7"); }
        }
    }
}