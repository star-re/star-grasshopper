//using System;
//using System.Collections.Generic;

//using Grasshopper.Kernel;
//using Rhino.Geometry;

//namespace star.M1
//{
//    public class Merge_Sort : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the Merge_Sort class.
//        /// </summary>
//        public Merge_Sort()
//          : base("Merge_Sort", "Merge_Sort",
//              "归并排序（尝试）",
//              "star", "Math")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddNumberParameter("Numbers", "N", "要排序的数字（从小到大）", GH_ParamAccess.list);
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddNumberParameter("Numbers", "N", "排好的（从小到大）", GH_ParamAccess.list);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            List<double> numbers = new List<double>();
//            DA.GetDataList(0, numbers);

//            stardy.MergeSort(numbers);
//            DA.SetDataList(0, numbers);
//        }

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
//            get { return new Guid("43588851-e367-4f7f-9516-9ede117664e2"); }
//        }
//    }
//}