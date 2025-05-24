using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star.M1
{
    public class To_Meters : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the To_Meters class.
        /// </summary>
        public To_Meters()
          : base("To Meters", "To m",
              "单位转化到米",
              "star", "math")
        {
            string unit = Rhino.RhinoDoc.ActiveDoc.ModelUnitSystem.ToString();
            Message = string.Concat("文档单位：", unit);
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("number", "n", "需要转化的数据", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("result", "r", "结果", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double douBle = double.NaN;
            DA.GetData(0, ref douBle);
            
            string unit = Rhino.RhinoDoc.ActiveDoc.ModelUnitSystem.ToString();

            var result = new Object();
            if (unit == "Millimeters")
            {
                result = douBle / 1000;
            }
            else if (unit == "Centimeters")
            {
                result = douBle / 100;
            }
            else if (unit == "Meters")
            {
                result = douBle;
            }
            else
            {
                result = "单位是" + unit + "，别转了，转了也看不懂";
            }
            DA.SetData(0, result);
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
                return Properties.Resources.To_m;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("531e8888-6556-4529-bd3f-18dfc4c64348"); }
        }
    }
}