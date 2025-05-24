using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace star.M1
{
    public class Cull_List : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Cull_List class.
        /// </summary>
        public Cull_List()
          : base("Cull List", "Cull L",
              "剪掉树枝",
              "star", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "t", "树", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("Index", "i", "树枝索引", GH_ParamAccess.list);
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

            List<int> cullIndex = new List<int>();
            DA.GetDataList(1, cullIndex);


            bool warp = true;
            DA.GetData(2, ref warp);

            int pathCount = gH_Goos.PathCount;
            IList<GH_Path> pathlist = gH_Goos.Paths;
            GH_Structure<IGH_Goo> result = gH_Goos.Duplicate();

            int num = 0;
            if (warp)
            {

            }
            for (int i = 0; i < cullIndex.Count; i++)
            {
                if (warp)
                {
                    num = GH_MathUtil.WrapInteger(cullIndex[i], gH_Goos.PathCount);
                }
                result.RemovePath(gH_Goos.Paths[num]);
            }
            //result.Simplify(GH_SimplificationMode.CollapseAllOverlaps);
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
                return Properties.Resources.CullList;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("478123f6-9385-4a93-ac93-d093956cb6f1"); }
        }
    }
}