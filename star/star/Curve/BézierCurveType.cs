using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino.Geometry.Collections;


namespace star
{
    public class BézierCurveType : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BézierCurveType class.
        /// </summary>
        public BézierCurveType()
          : base("BézierCurveType", "BézierCurveType",
              "求出贝塞尔曲线各类属性",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "曲线", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Degree", "D", "阶数", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Count", "C", "点数", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "P", "控制点", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            DA.GetData(0, ref curve);
            if (curve != null)
            {
                List<int> degree = new List<int>();
                List<int> count = new List<int>();
                List<GH_Point> points = new List<GH_Point>();

                NurbsCurve nurbsCurve = curve.ToNurbsCurve();
                for (int i = 0; i < nurbsCurve.Points.Count; i++)
                {
                    ControlPoint controlPoint = nurbsCurve.Points[i];
                    points.Add(new GH_Point(controlPoint.Location));
                }
                degree.Add(curve.Degree);
                count.Add(points.Count);

                DA.SetDataList(0, degree);
                DA.SetDataList(1, count);
                DA.SetDataList(2, points);
            }
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
                return Properties.Resources.BézierCurveType;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c9fd2b0d-57ab-4ecf-ad3e-f51e8746cdfd"); }
        }
    }
}