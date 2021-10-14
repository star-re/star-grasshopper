using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class ExtendOnSurface : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExtendOnSurface class.
        /// </summary>
        public ExtendOnSurface()
          : base("ExtendOnSurface", "ExtendOnSurface",
              "在曲面上延伸曲线",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "输入需延伸的曲线", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface", "Srf", "输入基准曲面", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve result", "C", "延伸后的结果", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            Surface ss = null;
            DA.GetData(0, ref curve);
            DA.GetData(1, ref ss);

            curve = curve.ExtendOnSurface(CurveEnd.Both, ss);
            DA.SetData(0, curve);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                GH_DocumentObject GD = this;
                GD.SetIconOverride(Properties.Resources.ExtendOnSurface);
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
            get { return new Guid("8de2b5f0-cbc4-4b56-badd-8e43ef8e3616"); }
        }
    }
}