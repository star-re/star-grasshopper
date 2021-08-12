using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star.M1
{
    public class Bool_Index : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Bool_Index class.
        /// </summary>
        public Bool_Index()
          : base("Bool Index", "Bool Index",
              "找出布尔的Index",
              "star", "Math")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Booleans", "Bools", "布尔群", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("True Index", "T", "值为true的Index", GH_ParamAccess.list);
            pManager.AddIntegerParameter("False Index", "F", "值为False的Index", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<bool> bools = new List<bool>();
            DA.GetDataList(0, bools);

            List<int> index = new List<int>();
            List<int> index2 = new List<int>();
            for (int i = 0; i < bools.Count; i++)
            {
                if (bools[i] == true)
                {
                    index.Add(i);
                }
                else
                {
                    index2.Add(i);
                }
            }

            DA.SetDataList(0, index);
            DA.SetDataList(1, index2);
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
                return Properties.Resources.bool_index;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("134d44f3-3465-454d-9f0d-9b6354ce4782"); }
        }
    }
}