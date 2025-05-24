//using System;
//using System.Collections.Generic;

//using Grasshopper.Kernel;
//using Rhino.Geometry;
//using Grasshopper.Kernel.Data;
//using Grasshopper.Kernel.Types;

//namespace star.M1
//{
//    public class Tree_Match : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the Tree_Match class.
//        /// </summary>
//        public Tree_Match()
//          : base("Tree_Match", "Nickname",
//              "Description",
//              "Category", "Subcategory")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Tree1", "T1", "树1", GH_ParamAccess.tree);
//            pManager.AddGenericParameter("Tree2", "T2", "树2", GH_ParamAccess.tree);
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Tree1", "T1", "树结果1", GH_ParamAccess.tree);
//            pManager.AddGenericParameter("Tree2", "T2", "树结果2", GH_ParamAccess.tree);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            GH_Structure<IGH_Goo> gH_DataTrees1 = (GH_Structure<IGH_Goo>)base.Params.Input[0].VolatileData;
//            GH_Structure<IGH_Goo> gH_DataTrees2 = (GH_Structure<IGH_Goo>)base.Params.Input[1].VolatileData;
//            GH_Structure<IGH_Goo> ReturnDataTrees1 = (GH_Structure<IGH_Goo>)base.Params.Output[0].VolatileData;
//            GH_Structure<IGH_Goo> ReturnDataTrees2 = (GH_Structure<IGH_Goo>)base.Params.Output[1].VolatileData;
//            gH_DataTrees1.Simplify(GH_SimplificationMode.CollapseAllOverlaps);
//            gH_DataTrees2.Simplify(GH_SimplificationMode.CollapseAllOverlaps);
//            int xCount = gH_DataTrees1.PathCount;
//            int yCount = gH_DataTrees2.PathCount;
//            for (int i = 0; i < xCount; i++)
//            {
//                for (int j = 0; j < yCount; j++)
//                {
//                    if (gH_DataTrees1.get_Path(i).Indices[0] == gH_DataTrees2.get_Path(j).Indices[0])
//                    {
//                        ReturnDataTrees1.AppendRange(gH_DataTrees1.Branches[i], new GH_Path(new int[] { gH_DataTrees1.get_Path(i).Indices[0], j }));
//                    }
//                }
//            }
//            for (int i = 0; i < xCount; i++)
//            {
//                for (int j = 0; j < yCount; j++)
//                {
//                    if (gH_DataTrees1.get_Path(i).Indices[0] == gH_DataTrees2.get_Path(j).Indices[0])
//                    {
//                        ReturnDataTrees2.AppendRange(gH_DataTrees1.Branches[i], new GH_Path(new int[] { gH_DataTrees1.get_Path(i).Indices[0], j }));
//                    }
//                }
//            }
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
//            get { return new Guid("7126f020-fe19-4865-9480-d0058df1a9ee"); }
//        }
//    }
//}