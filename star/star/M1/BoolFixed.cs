//using System;
//using System.Collections.Generic;

//using Grasshopper.Kernel;
//using Rhino.Geometry;

//namespace star.M1
//{
//    public class BoolFixed : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the BoolFixed class.
//        /// </summary>
//        public BoolFixed()
//          : base("BoolFixed", "BoolFixed",
//              "布尔切换",
//              "star", "Math")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddBooleanParameter("Bool", "B", "布尔", GH_ParamAccess.item, true);
//            Params.Input[0].Optional = true;
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddBooleanParameter("Bool", "B", "布尔", GH_ParamAccess.item);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            bool a = true;
//            DA.GetData(0, ref a);

//            bool b = BoolFixeda(a);
//            DA.SetData(0, b);
//        }

//        public static int index = 0;
//        public static bool BoolFixeda(bool bb)
//        {
//            bbb = bb;
//            if (index != 0)
//            {
//                if (bb == cc)
//                {
//                    index = 0;
//                    cc = !bb;
//                    return true;
//                }
//                return false;
//            }
//            else
//            {
//                index++;
//                return true;
//            }
//        }

//        public static bool bbb;
//        public static bool cc;
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
//            get { return new Guid("37869ee8-dbb5-4c22-8312-08404383a808"); }
//        }
//    }
//}