using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class Line_BothSides_Vector_ : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the curve_collision class.
        /// </summary>
        public Line_BothSides_Vector_()
          : base("Line BothSides(Vector)", "Line BothSides(Vector)",
              "通过中点与矢量，从中点画线",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Target", "T", "目标点", GH_ParamAccess.item);
            pManager.AddVectorParameter("Vector", "V", "方向", GH_ParamAccess.item, Vector3d.XAxis);
            pManager.AddNumberParameter("Length", "L", "长度", GH_ParamAccess.item, 10);
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
            Point3d targetpoint = new Point3d();
            Vector3d linev = new Vector3d();

            double length = double.NaN;
            DA.GetData(1, ref linev);
            DA.GetData(0, ref targetpoint);
            DA.GetData(2, ref length);
            Vector3d nega = Vector3d.Negate(linev);
            /*----------------------------------------------------*/
            Line linea = new Line(targetpoint, linev, length);
            Line lineb = new Line(targetpoint, nega, length);
            Point3d pa = linea.To;
            Point3d pb = lineb.To;
            Line line = new Line(pb, pa);
            double linelength = line.Length;
            DA.SetData(0, line);
            DA.SetData(1, linelength);
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
                return Properties.Resources.Line_BothSides_Vector_;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c7c5427c-a541-4829-ada1-570dbcc6a5b7"); }
        }
    }
}