using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class Divide_Curve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Divide_Curve class.
        /// </summary>
        public Divide_Curve()
          : base("Divide Curve", "Divide Curve",
              "均分曲线并提取点上法线&切线",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "曲线", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Count", "N", "数量", GH_ParamAccess.item, 10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "点", GH_ParamAccess.list);
            pManager.AddVectorParameter("Tangents", "T", "切线", GH_ParamAccess.list);
            pManager.AddVectorParameter("Normal", "N", "法线", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            int cOunt = new int();
            DA.GetData(0, ref curve);
            DA.GetData(1, ref cOunt);
            /*----------------------------------------------------*/
            if (curve != null && cOunt != null)
            {
                List<Point3d> pointlist = new List<Point3d>();
                List<Vector3d> xAxis = new List<Vector3d>();
                List<Vector3d> yAxis = new List<Vector3d>();
                Vector3d zAxis = Vector3d.ZAxis;
                double[] pdouble = curve.DivideByCount(cOunt, true);
                for (int i = 0; i < pdouble.Length; i++)
                {
                    xAxis.Add(curve.TangentAt(pdouble[i]));
                    yAxis.Add(curve.CurvatureAt(pdouble[i]));
                    pointlist.Add(curve.PointAt(pdouble[i]));
                }

                DA.SetDataList(0, pointlist);
                DA.SetDataList(1, xAxis);
                DA.SetDataList(2, yAxis);
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
                return Properties.Resources.Divide_Curve;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a5454311-ce44-4830-bd26-5c9044ab0c1a"); }
        }
    }
}