using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class size_swap : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Rendom_data_size_swap class.
        /// </summary>
        public size_swap()
          : base("size swap", "size swap",
              "把数据大小调换",
              "star", "Math")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("number", "n", "需要调换的数据", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("result", "r", "调换后", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> x = new List<double>();
            DA.GetDataList(0, x);

            List<double> reverse = new List<double>(x);
            List<int> index = new List<int>();
          //  List<double> result = new List<double>();
            reverse.Sort();
            for (int i = 0; i < reverse.Count; i++)
            {
                index.Add(x.IndexOf(reverse[i]));
            }
            reverse.Reverse();
            Array sor = reverse.ToArray();
            Array.Sort(index.ToArray(), sor);

            DA.SetDataList(0,sor);
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
                return Properties.Resources.size_swap;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e98cb204-78a5-4bef-8e2f-29230225fc0e"); }
        }
    }
}