using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace star.Display.color
{
    public class Fuse_Color : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Fuse_Color class.
        /// </summary>
        public Fuse_Color()
          : base("Fuse Color", "FuseC",
              "融合颜色，以因子取色",
              "star", "Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Color a", "a", "颜色A", GH_ParamAccess.item);
            pManager.AddColourParameter("Color b", "b", "颜色B", GH_ParamAccess.item);
            pManager.AddNumberParameter("factor", "f", "选定因子（0~1）", GH_ParamAccess.item, 0.5);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("color", "c", "颜色", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color colora = new Color();
            Color colorb = new Color();
            double f = 0.5;
            DA.GetData(0, ref colora);
            DA.GetData(1, ref colorb);
            DA.GetData(2, ref f);
            /*-----------------------------*/
            star_color star_Color = new star_color();
            Color result = star_Color.fuse(colora, colorb, f);
            DA.SetData(0, result);
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
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
                return Properties.Resources.fuse_color;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("075beb86-c4ce-4925-b9e2-5483cfb9f9ce"); }
        }
    }
}