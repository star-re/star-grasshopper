using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star.starPoint
{
    public class sort_point_in_plane : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the sort_point_in_plane class.
        /// </summary>
        public sort_point_in_plane()
          : base("sort point in plane", "sort point in plane",
              "判断点在坐标平面上的位置，返回4则点在任意轴上",
              "star", "Point")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pts", "点集", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "坐标平面", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Index", "I", "点集索引，0123各代表不同的位置，4则是点在任意轴上",GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d p3 = new Point3d();
            Plane plane = new Plane();
            DA.GetData(0, ref p3);
            DA.GetData(1, ref plane);

            double n1;
            double n2;
            plane.ClosestParameter(p3, out n1, out n2);
            int index = 0;
            if (n1 > 0 && n2 > 0)
            {
                index = 0;
            }
            if (n1 < 0 && n2 > 0)
            {
                index = 1;
            }
            if (n1 < 0 && n2 < 0)
            {
                index = 2;
            }
            if (n1 > 0 && n2 < 0)
            {
                index = 3;
            }
            if (n1 == 0 && n2 == 0)
            {
                index = 4;
            }
            DA.SetData(0, index);
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
                return Properties.Resources.sort_point_in_plane;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("98e8bf07-510c-499b-a3fd-cfece6fdddfa"); }
        }
    }
}