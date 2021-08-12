using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class Curve_Degree : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Curve_Degree class.
        /// </summary>
        public Curve_Degree()
          : base("Curve Degree", "Curve Degree",
              "更改曲线阶数",
              "star", "curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "输入需更改阶数的曲线", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Degree", "D", "阶数", GH_ParamAccess.item, 3);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Curve", "C", "曲线结果", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            int degree = new int();
            DA.GetData(0, ref curve);
            DA.GetData(1, ref degree);
            /*----------------------------------------------------*/
            if (curve != null && degree != null)
            {
                if (degree > 32)
                {
                    DA.SetData(0, "阶数无法超过32");
                }
                else
                {
                    NurbsCurve nc = (NurbsCurve)curve;
                    nc.IncreaseDegree(degree);
                    DA.SetData(0, nc);
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
                return Properties.Resources.curve_degree;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8895ce25-9c0a-4ac4-90c6-0880d5fd8ca0"); }
        }
    }
}