using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class NurbsToBezier : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ConverToBezier class.
        /// </summary>
        public NurbsToBezier()
          : base("NurbsToBezier", "NurbsToBezier",
              "把Nurbs曲线转化成贝塞尔曲线",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "需转换的曲线", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "转化的曲线结果", GH_ParamAccess.item);
            pManager.AddPointParameter("Node Point", "NP", "节点", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> cspan = new List<double>();
            List<Point3d> spanp = new List<Point3d>();
            Curve curve = null;
            if (DA.GetData(0, ref curve))//载入曲线
            {
                //---------------------------------------------
                if (curve != null)
                {
                    int span = curve.SpanCount;
                    cspan.Add(curve.SpanDomain(0).T0);
                    for (int i = 0; i < span; i++)
                    {
                        cspan.Add(curve.SpanDomain(i).T1);
                    }
                    for (int j = 0; j < cspan.Count; j++)
                    {
                        spanp.Add(curve.PointAt(cspan[j]));
                    }
                    //---------------------------------------------
                    Curve[] a = curve.Split(cspan);
                    DA.SetData(0, a);
                    DA.SetDataList(1, spanp);
                }
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
                return Properties.Resources.NurbsToBezier;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7e4e5f75-a350-4654-b23c-1d0275e2a9c7"); }
        }
    }
}