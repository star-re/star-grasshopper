//using Grasshopper.Kernel;
//using Rhino.Geometry;
//using System;
//using System.Collections.Generic;

//namespace star.starPoint
//{
//    public class Mid_Point : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the Mid_Point class.
//        /// </summary>
//        public Mid_Point()
//          : base("Mid Point", "Mid Point",
//              "Description",
//              "Category", "Subcategory")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddPointParameter("Points", "Pts", "点群", GH_ParamAccess.list);
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddPointParameter("Point", "Pt", "中心点", GH_ParamAccess.item);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            List<Point3d> p3s = new List<Point3d>();
//            DA.GetDataList(0, p3s);

//            for (int i = 0; i < p3s.Count; i++)
//            {

//            }
//        }


//        public double PDistance(Point3d p31, Point3d p32)
//        {
//            double x = Math.Pow(p31.X - p32.X, 2);
//            double y = Math.Pow(p31.Y - p32.Y, 2);
//            double z = Math.Pow(p31.Z - p32.Z, 2);
//            double result = Math.Sqrt(x + y + z);
//            return result;
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
//            get { return new Guid("e36148a8-63d0-407a-a796-f55869cc96f3"); }
//        }
//    }
//}