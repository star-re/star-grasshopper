using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Grasshopper;
using Rhino.Geometry;

namespace star.M1
{
    public class Shift_Tree : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Shift_Tree class.
        /// </summary>
        public Shift_Tree()
          : base("Shift Tree", "Shift",
              "偏移树枝",
              "star", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "t", "树", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("Shift", "s", "偏移", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Warp", "w", "是否循环", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Branch", "B", "树枝", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<IGH_Goo> gH_Goos = new GH_Structure<IGH_Goo>();
            DA.GetDataTree(0, out gH_Goos);

            int shift = 0;
            DA.GetData(1, ref shift);


            bool warp = true;
            DA.GetData(2, ref warp);

            int pathCount = gH_Goos.PathCount;
            IList<GH_Path> pathlist = gH_Goos.Paths;
            GH_Structure<IGH_Goo> result = new GH_Structure<IGH_Goo>();
            if (pathCount <= shift)
            {
                int wh = pathCount;
                while (wh <= shift)
                {
                    shift = shift - pathCount;
                }
                if (!warp)
                {
                    shift = pathCount;
                }
            }
            if (-pathCount >= shift)
            {
                int wh = pathCount;
                while (-wh >= shift)
                {
                    shift = shift + pathCount;
                }
                if (!warp)
                {
                    shift = pathCount;
                }
            }

            /*----------------------------------------------------*/
            if (warp)
            {
                if (shift > 0)
                {
                    for (int i = pathCount - shift; i < pathCount; i++)
                    {
                        result.AppendRange(gH_Goos[i - (pathCount - shift)], pathlist[i]);
                    }
                }
                else
                {
                    for (int i = pathCount + shift;i < pathCount; i++)
                    {
                        result.AppendRange(gH_Goos[i], pathlist[i - (pathCount + shift)]);
                    }
                }
            }

            if (shift > 0)
            {
                for (int i = shift; i < gH_Goos.PathCount; i++)
                {
                    result.AppendRange(gH_Goos[i], pathlist[i - shift]);
                }
            }
            else
            {
                for (int i = -shift; i < gH_Goos.PathCount; i++)
                {
                    result.AppendRange(gH_Goos[i+shift], pathlist[i]);
                }
            }
            /*---------------------------------------------*/
            DA.SetDataTree(0, result);
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
                return Properties.Resources.shift_tree;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ff43a1fb-315e-4820-aa3f-b80e951cfcbb"); }
        }
    }
}