using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class Reference_Offset_Curve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Reference_Offset_Curve class.
        /// </summary>
        public Reference_Offset_Curve()
          : base("Reference Offset Curve", "Reference Offset Curve",
              "根据参照平面偏移曲线",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "需延伸的曲线", GH_ParamAccess.item);
            pManager.AddNumberParameter("Distance", "D", "偏移距离", GH_ParamAccess.item, 10);
            pManager.AddPlaneParameter("Plane", "P", "参照平面", GH_ParamAccess.item, Plane.WorldXY);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "需延伸的曲线", GH_ParamAccess.list);
        }

        double d;
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve cc = null;
            double Distance = 10;
            Plane plane = Plane.WorldXY;
            DA.GetData(0, ref cc);
            DA.GetData(1, ref Distance);
            DA.GetData(2, ref plane);
            double bi = double.NaN;
            double CurveT = double.NaN;
            ClosestPoint(cc, plane.Origin, out CurveT, out bi);

            Vector3d CurveXV = cc.PointAt(CurveT) - plane.Origin;
            Vector3d CurveYV = cc.TangentAt(CurveT);
            Vector3d CurveZV = Vector3d.CrossProduct(CurveXV, CurveYV);
            Vector3d PlaneZ = plane.ZAxis;
            double VAngle = starMathdy.Dreeges(Vector3d.VectorAngle(CurveZV, PlaneZ));
            Curve[] curves = null;
            bool JJ = Distance < 0 ? true : false;
            if (VAngle <= 180)
            {
                double bj = double.NaN;
                curves = cc.Offset(plane, -Distance, 0.001, CurveOffsetCornerStyle.Sharp);
                ClosestPoint(curves[0], plane.Origin, out d, out bj);
                if (bj < bi)
                {
                    if (JJ)
                    {
                        DA.SetDataList(0, cc.Offset(plane, Distance, 0.001, CurveOffsetCornerStyle.Sharp));
                    }
                    else
                    {
                        DA.SetDataList(0, curves);
                    }
                }
                else
                {
                    if (JJ)
                    {
                        DA.SetDataList(0, curves);
                    }
                    else
                    {
                        DA.SetDataList(0, cc.Offset(plane, Distance, 0.001, CurveOffsetCornerStyle.Sharp));
                    }
                }

                //    if (bj > bi)
                //    {
                //        DA.SetDataList(0, cc.Offset(plane, Distance, 0.001, CurveOffsetCornerStyle.Sharp));
                //    }
                //    else
                //    {
                //        DA.SetDataList(0, curves);
                //    }
                //}
                //else
                //{
                //    double bj = double.NaN;
                //    curves = cc.Offset(plane, Distance, 0.001, CurveOffsetCornerStyle.Sharp);
                //    ClosestPoint(curves[0], plane.Origin, out d, out bj);
                //    if (Distance < 0)
                //    {
                //        JJ = false;
                //    }
                //    else
                //    {
                //        JJ = true;
                //    }
                //    if (JJ)
                //    {
                //        DA.SetDataList(0, curves);
                //    }
                //    else
                //    {
                //        DA.SetDataList(0, cc.Offset(plane, -Distance, 0.001, CurveOffsetCornerStyle.Sharp));
                //    }
            }

        }

        public void ClosestPoint(Curve curve, Point3d testpoint, out double t, out double Dis)
        {
            curve.ClosestPoint(testpoint, out t);
            Dis = testpoint.DistanceTo(curve.PointAt(t));
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
                return Properties.Resources.Reference_Offset_Curve;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("729f643d-9d2c-46c7-8814-077d9ef83dec"); }
        }
    }
}