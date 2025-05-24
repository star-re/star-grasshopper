using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star.Domain
{
    public class origin_domain : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the origin_domain class.
        /// </summary>
        public origin_domain()
          : base("origin domain", "origin domain",
              "拍回起始区间",
              "star", "Domain")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntervalParameter("Domain", "D", "区间", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Bool", "B", "从零开始，或正反值", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntervalParameter("Domain", "D", "区间", GH_ParamAccess.item);
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

            double Domainend = Math.Abs(interval.Length);
            Interval result;
            if (Bool)
            {

                result = new Interval(0, Domainend);
            }
            else
            {
                result = new Interval(-Domainend / 2, Domainend / 2);
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
                return Properties.Resources.origin_domain;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("00000000-ad42-4a3b-ba1d-8de53896ed9d"); }
        }
    }
}