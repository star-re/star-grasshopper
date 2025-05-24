using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;

namespace star.starSurface
{
    public class Plane_On_Brep : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Plane_On_Brep class.
        /// </summary>
        public Plane_On_Brep()
          : base("Plane On Brep", "POB",
              "求与Brep最近点的那个平面",
              "star", "Surface")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "求平面的Brep", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "P", "求最近点的平面", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "Pl", "平面", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = new Brep();
            List<Point3d> point3Ds = new List<Point3d>();
            DA.GetData(0, ref brep);
            DA.GetDataList(1, point3Ds);
            /*----------------------------------------*/

            List<Plane> result = BrepClosestPlane(brep, point3Ds);
            DA.SetDataList(0, result);
        }

        public static List<Plane> BrepClosestPlane(Brep x, List<Point3d> y)
        {
            Point3d closestPoint = Point3d.Origin;
            index = ComponentIndex.Unset;
            double s = 0;
            double t = 0;
            Vector3d normal = Vector3d.XAxis;
            Plane frame = Plane.Unset;
            List<Plane> planelist = new List<Plane>();

            for (int i = 0; i < y.Count; i++)
            {
                x.ClosestPoint(y[i], out closestPoint, out index, out s, out t, 0, out normal);
                x.Surfaces[index.Index].FrameAt(s, t, out frame);
               // frame.Origin = y[i];
                planelist.Add(frame);
            }
            return planelist;
        }

        public static ComponentIndex index;
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.Plane_On_Brep;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("C431233A-8D87-43D1-8D78-05C629964CD9"); }
        }
    }
}