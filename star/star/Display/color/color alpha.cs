using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;

namespace star.Display.color
{
    public class color_alpha : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the color_alpha class.
        /// </summary>
        public color_alpha()
          : base("color alpha", "color alpha",
              "颜色的透明度",
              "star", "Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Colors", "C", "色s", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Alpha", "A", "透明度：0~255", GH_ParamAccess.item,255);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("Colors", "C", "色s", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Color color = new Color();
            int alpha = 255;
            DA.GetData(0, ref color);
            DA.GetData(1, ref alpha);
            /*-----------------------------------*/
            color = Color.FromArgb(alpha, color);
            DA.SetData(0, color);
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
                return Properties.Resources.color_alpha;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b864ef0c-94a5-4fc5-92b6-76e284766492"); }
        }
    }
}