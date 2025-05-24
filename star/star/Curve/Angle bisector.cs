using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace star
{
    public class Angle_bisector : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Angle_bisector class.
        /// </summary>
        public Angle_bisector()
          : base("Angle bisector", "Angle bisector",
              "两线之角平分线",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter("Line a", "A", "线1", GH_ParamAccess.item);
            pManager.AddLineParameter("Line b", "B", "线2", GH_ParamAccess.item);
            pManager.AddNumberParameter("Length", "L", "长度", GH_ParamAccess.item, 10);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "两线交点", GH_ParamAccess.item);
            pManager.AddLineParameter("Line", "L", "角平分线", GH_ParamAccess.item);
            pManager.AddNumberParameter("Angle", "A", "角度", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Line linea = new Line();
            Line lineb = new Line();
            double d1 = 0;
            double d2 = 0;
            Point3d center = new Point3d();
            Vector3d v1 = new Vector3d();
            Vector3d v2 = new Vector3d();
            double length = double.NaN;

            DA.GetData(0, ref linea);
            DA.GetData(1, ref lineb);
            DA.GetData(2, ref length);
            /*----------------------------------------------------*/
            if (Intersection.LineLine(linea, lineb, out d1, out d2))
            {
                center = linea.PointAt(d1);
            }
            v1 = linea.Direction;
            v2 = lineb.Direction;
            double pointdistance1 = linea.From.DistanceTo(center);
            double pointdistance2 = linea.To.DistanceTo(center);
            double pointdistance3 = lineb.From.DistanceTo(center);
            double pointdistance4 = lineb.To.DistanceTo(center);
            if (pointdistance1 > pointdistance2)
            {
                v1 = Vector3d.Negate(v1);
            }

            if (pointdistance3 > pointdistance4)
            {
                v2 = Vector3d.Negate(v2);
            }
            double towline = Vector3d.VectorAngle(v1, v2);
            Vector3d zAxis = Vector3d.CrossProduct(v1, v2);
            v1.Rotate(towline / 2, zAxis);
            Line ab = new Line(center, v1, length);

            DA.SetData(0, center);
            DA.SetData(1, ab);
            DA.SetData(2, towline);
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
                return Properties.Resources.Angle_bisector;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a06615a6-66e3-4e6f-9de9-970902011304"); }
        }
    }
}