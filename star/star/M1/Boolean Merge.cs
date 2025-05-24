using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Linq;

namespace star.M1
{

    interface boo
    {
    }
    public class Boolean_Merge : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Boolean_Merge class.
        /// </summary>
        public Boolean_Merge()
          : base("Boolean Merge", "Boolean Merge",
              "把列表里的布尔值归并",
              "star", "Math")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Bools", "Bools", "Bools", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Bools", "Bools", "Bools", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
             GH_Structure<GH_Boolean> Datax = new GH_Structure<GH_Boolean>();
            DA.GetDataTree(0, out Datax);
            

            List<GH_Boolean> x1 = new List<GH_Boolean>();
            x1.AddRange(Datax.Branches[0].ToList());
            for (int i = 1; i < Datax.Branches.Count; i++)
            {
                for (int j = 0; j < Datax.Branches[0].Count; j++)
                {
                    if (Datax.Branches[i][j].Value != false)
                    {
                        x1.RemoveAt(j);
                        x1.Insert(j, Datax.Branches[i][j]);
                    }
                }
            }
            DA.SetDataList(0, x1);

            //List<bool> x1 = new List<bool>();
            //x1.AddRange(x.Branch(0));
            //for (int i = 1; i < x.BranchCount; i++)
            //{
            //    for (int j = 0; j < x.Branch(i).Count; j++)
            //    {
            //        if (x.Branch(i)[j] != false)
            //        {
            //            x1.RemoveAt(j);
            //            x1.Insert(j, x.Branch(i)[j]);
            //        }
            //    }
            //}
            //DA.SetDataList(0, x1);
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
                return Properties.Resources.Boolean_Merge;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("55ec88cc-c6b6-4098-bf44-7ab6ad5e74fb"); }
        }
    }
}