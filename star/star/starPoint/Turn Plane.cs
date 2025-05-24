using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star.starPoint
{
    public class Turn_Plane : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Turn_Plane class.
        /// </summary>
        public Turn_Plane()
          : base("Turn Plane", "Turn Plane",
              "旋转平面",
              "star", "Point")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Origin", "O", "定位点", GH_ParamAccess.item,Plane.WorldXY);
            pManager.AddNumberParameter("Angle A", "A", "X轴旋转角度", GH_ParamAccess.item,0);
            pManager.AddNumberParameter("Angle B", "B", "Y轴旋转角度", GH_ParamAccess.item,0);
            pManager.AddNumberParameter("Angle C", "C", "Z轴旋转角度", GH_ParamAccess.item,0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane ", "P", "平面结果", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            //Plane pp = Plane.WorldXY;
            Plane pp = new Plane();
            double A = 0;
            double B = 0;
            double C = 0;
            DA.GetData(0, ref pp);
            DA.GetData(1, ref A);
            DA.GetData(2, ref B);
            DA.GetData(3, ref C);
            /*---------------------------------------*/
            pp.Rotate(starMathdy.Radians(A), Vector3d.XAxis);
            pp.Rotate(starMathdy.Radians(B), Vector3d.YAxis);
            pp.Rotate(starMathdy.Radians(C), Vector3d.ZAxis);
           // pp.Origin = location;
            DA.SetData(0, pp);
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
                return Properties.Resources.turn_plane;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f0e6188a-ae1c-4f1a-adfc-8633fc5dde57"); }
        }
    }
}