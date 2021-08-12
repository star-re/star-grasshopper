using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace star
{
    public class UnTrim_or_Trim : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the UnTrim_or_Trim class.
        /// </summary>
        public UnTrim_or_Trim()
          : base("UnTrim or Trim", "UnTrim or Trim",
              "区分修剪与非修剪",
              "star", "surface")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "B", "请输入想要结果的Brep", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "B", "结果", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Trim", "T", "输出布尔值\r\nTrue是修剪\r\n反之则未修剪", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep x=new Brep();
            if (DA.GetData(0, ref x))
            {
                return;
            }
            
            GH_Surface gH_surface = new GH_Surface(x);

            string trimstr= "Trimmed Surface";
            string toghs = gH_surface.ToString();
            bool triM = string.Equals(trimstr, toghs);

            DA.SetData(0, gH_surface);
            DA.SetData(1, triM);
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
                return Properties.Resources.UnTrim_or_Trim;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f13f24e3-caf5-45d5-a241-51768d7e305d"); }
        }
    }
}