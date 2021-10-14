using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace star.Display
{
    public class Screen_Color : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Screen_Color class.
        /// </summary>
        public Screen_Color()
          : base("Screen Color", "Screen",
              "颜色混合",
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
            DA.GetData(0, ref colora);
            DA.GetData(1, ref colorb);
            /*-----------------------------*/
            star_color star_Color = new star_color();
            Color result = star_Color.screen(colora, colorb);
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
                return Properties.Resources.screen_color;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f9f37d69-85b2-4bee-94a2-a92910c3b2e0"); }
        }
    }
}