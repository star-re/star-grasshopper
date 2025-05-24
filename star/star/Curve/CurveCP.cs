using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class CurveCP : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CurveCP class.
        /// </summary>
        public CurveCP()
          : base("CurveCP", "CurveCP",
              "CurveCP",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "点", GH_ParamAccess.list);
            pManager.AddCurveParameter("Curve", "C", "曲线", GH_ParamAccess.item);
            pManager[1].DataMapping = GH_DataMapping.Graft;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "P", "Point on the curve closest to the base point", GH_ParamAccess.list);
            pManager.AddNumberParameter("Parameter", "t", "Parameter on curve domain of closest point", GH_ParamAccess.list);
            pManager.AddNumberParameter("Distance", "D", "Distance between base point and curve", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> point3Ds = new List<Point3d>();
            Curve cc = null;
            DA.GetDataList(0, point3Ds);
            DA.GetData(1, ref cc);

            List<double> CtList = point3Ds.AsParallel().AsOrdered().Select(i => closestP(i, cc)).ToList();
            List<Point3d> PointList = CtList.AsParallel().AsOrdered().Select(i => cc.PointAt(i)).ToList();
            List<double> distances = new List<double>();
            for (int i = 0; i < point3Ds.Count; i++)
            {
                distances.Add(point3Ds[i].DistanceTo(PointList[i]));
            }
            DA.SetDataList(0, PointList);
            DA.SetDataList(1, CtList);
            DA.SetDataList(2, distances);
        }


        public double closestP(Point3d p3, Curve cc)
        {
            double Ct = double.NaN;
            cc.ClosestPoint(p3, out Ct);
            return Ct;
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
                return Properties.Resources.CurveCLosest;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e5b38e95-f2fb-47a4-8986-36b35b31c1f2"); }
        }
    }
}