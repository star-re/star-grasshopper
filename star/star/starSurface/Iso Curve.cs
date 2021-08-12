//using System;
//using System.Collections.Generic;

//using Grasshopper.Kernel;
//using Rhino.Geometry;

//namespace star
//{
//    public class Iso_Curve : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the Iso_Curve class.
//        /// </summary>
//        public Iso_Curve()
//          : base("Iso Curve", "Iso Curve",
//              "获取曲面的Iso线",
//              "star", "Surface")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddSurfaceParameter("Surface", "S", "接入曲面", GH_ParamAccess.item);
            
//            pManager.AddPointParameter("UV Point", "UV", "面上的UV点位", GH_ParamAccess.item);
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddCurveParameter("U curve", "UC", "U方向的曲线", GH_ParamAccess.item);
//            pManager.AddCurveParameter("V curve", "VC", "V方向的曲线", GH_ParamAccess.item);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            Surface getuvSurfeace = null;
//            Point3d uvpoint = new Point3d();
//            DA.GetData(0, ref getuvSurfeace);
//            DA.GetData(1, ref uvpoint);

//            getuvSurfeace.SetDomain(0, new Interval(0, 1));
//            getuvSurfeace.SetDomain(1, new Interval(0, 1));
//            Curve ucurve = getuvSurfeace.IsoCurve(0, uvpoint.Y);
//            Curve vcurve = getuvSurfeace.IsoCurve(1, uvpoint.X);
//            DA.SetData(0, ucurve);
//            DA.SetData(1, vcurve);
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
//            get { return new Guid("2e75c712-e99e-4675-8542-8014957bdc24"); }
//        }
//    }
//}