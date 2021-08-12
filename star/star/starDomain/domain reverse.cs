using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Windows.Forms;

namespace star
{
    public class domain_reverse : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the domain_reverse class.
        /// </summary>
        public domain_reverse()
          : base("domain reverse", "domain reverse",
              "区间反转",
              "star", "Domain")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntervalParameter("Domain", "D", "需反转的区间", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Bool", "B", "条件1，条件2", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntervalParameter("Domain", "D", "反转后的区间", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Interval interval = new Interval();
            bool Bool = new bool();

            DA.GetData(0, ref interval);
            DA.GetData(1, ref Bool);
            if (interval != null)
            {
                if (Bool)
                {
                    interval.Swap();
                }
                else
                {
                    interval.Reverse();
                }

                DA.SetData(0, interval);
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
                return Properties.Resources.domain_reverse;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("be6a4126-b363-4050-9fa5-84bcef87b18c"); }
        }

    }
}