using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class OffsetOnSrf : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the OffsetOnSrf class.
        /// </summary>
        public OffsetOnSrf()
          : base("Offset On Srf", "Offset On Srf",
              "偏移在曲面上的曲线",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "曲线", GH_ParamAccess.list);
            pManager.AddNumberParameter("Double", "D", "偏移距离", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface", "S", "曲面", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "曲线输出", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> curves = new List<Curve>();
            double num = double.NaN;
            List<Surface> surfaces = new List<Surface>();
            DA.GetDataList(0, curves);
            DA.GetData(1, ref num);
            DA.GetDataList(2, surfaces);

            List<Curve> offsets = new List<Curve>();
            Curve[] offsetcurve = new Curve[0];
            for (int i = 0; i < curves.Count; i++)
            {
                offsetcurve = curves[i].OffsetOnSurface(surfaces[i], num, 0.01);
                offsets.Add(offsetcurve[0]);
            }
            DA.SetDataList(0, offsets);
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
                return Properties.Resources.offset_On_Surface;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("47fda736-4fa9-43d0-8431-d6951b3c659a"); }
        }
    }
}