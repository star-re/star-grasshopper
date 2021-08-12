using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class Line_BothSides_Point_ : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Line_BothSides_Point_ class.
        /// </summary>
        public Line_BothSides_Point_()
          : base("Line BothSides(Point)", "Line BothSides(Point)",
              "通过两点，从中点画线",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("PointA", "Pa", "点A", GH_ParamAccess.item);
            pManager.AddPointParameter("PointB", "Pb", "点B", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "L", "从中点线", GH_ParamAccess.item);
            pManager.AddNumberParameter("Length", "L", "长度", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d point3da = new Point3d();
            Point3d point3db = new Point3d();
            DA.GetData(0, ref point3da);
            DA.GetData(1, ref point3db);
            /*----------------------------------------------------*/
            if (point3da != null && point3db != null)
            {
                Vector3d vector3D = point3db - point3da;
                Vector3d vector3Dne = Vector3d.Negate(vector3D);
                Line linea = new Line(point3da, point3db);
                Line lineb = new Line(point3da, vector3Dne);
                Point3d pointa = linea.To;
                Point3d pointb = lineb.To;
                Line result = new Line(pointa, pointb);
                double linelength = result.Length;
                DA.SetData(0, result);
                DA.SetData(1, linelength);
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
                return Properties.Resources.Line_BothSides_Point_;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c0f1a426-e788-4c24-a2bb-efbb6a93472c"); }
        }
    }
}