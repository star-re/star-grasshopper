using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star.Display
{
    public class unti : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the file class.
        /// </summary>
        public unti()
          : base("Unti", "unti",
              "获取当前文件的单位等",
              "star", "display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Unti", "U", "单位", GH_ParamAccess.item);
            pManager.AddTextParameter("Tolerance", "T", "绝对公差", GH_ParamAccess.item);
            pManager.AddTextParameter("Angle Tolerance", "A", "角度公差", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string unit = Rhino.RhinoDoc.ActiveDoc.ModelUnitSystem.ToString();
            string toler = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance.ToString();
            string Atoler = Rhino.RhinoDoc.ActiveDoc.ModelAngleToleranceDegrees.ToString();

            stardy stardycs = new stardy();
            unit = stardycs.unitstring(unit) + unit;
            DA.SetData(0, unit);
            DA.SetData(1, toler);
            DA.SetData(2, Atoler);
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
                return Properties.Resources.unti;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a53f4ee1-750f-40f6-8f05-2d2e6d1847a8"); }
        }
    }
}