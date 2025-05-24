//using System;
//using System.Collections.Generic;

//using Grasshopper.Kernel;
//using Grasshopper.Kernel.Types;
//using Grasshopper.Kernel.Data;
//using Grasshopper;
//using Rhino.Geometry;

//namespace star.M1
//{
//    public class Tree_Reverse : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the Tree_Reverse class.
//        /// </summary>
//        public Tree_Reverse()
//          : base("Tree Reverse", "Reverse",
//              "反转树",
//              "star", "Data")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Tree", "t", "树", GH_ParamAccess.tree);
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Tree", "t", "树", GH_ParamAccess.tree);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            GH_Structure<IGH_Goo> gH_Goos = new GH_Structure<IGH_Goo>();
//            DA.GetDataTree(0, out gH_Goos);

//            int pathCount = gH_Goos.PathCount;
//            IList<GH_Path> pathlist = gH_Goos.Paths;
//            GH_Structure<IGH_Goo> result = new GH_Structure<IGH_Goo>();
//            gH_Goos.Reverse();
//            //for (int i = 0; i < pathCount; i++)
//            //{
//            //    List<IGH_Goo> LGG = gH_Goos[i];
//            //    gH_Goos.Reverse();
//            //    LGG.Reverse();
//            //    result.AppendRange(LGG, pathlist[i]);
//            //}

//            DA.SetDataTree(0, gH_Goos);
//        }


//public override GH_Exposure Exposure
//{
//    get
//    {
//        return GH_Exposure.secondary;
//    }
//}
//        /// <summary>
//        /// Provides an Icon for the component.
//        /// </summary>
//        protected override System.Drawing.Bitmap Icon
//        {
//            get
//            {
//                //You can add image files to your project resources and access them like this:
//                // return Resources.IconForThisComponent;
//                return null;
//            }
//        }

//        /// <summary>
//        /// Gets the unique ID for this component. Do not change this ID after release.
//        /// </summary>
//        public override Guid ComponentGuid
//        {
//            get { return new Guid("38998944-c1cd-4b6a-b698-3844f9805c43"); }
//        }
//    }
//}