using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star.starPoint
{
    public class Group_Points : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Group_Points class.
        /// </summary>
        public Group_Points()
          : base("Group Points", "Group Points",
              "通过目标点对当前点群排序",
              "star", "Point")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("TargetPoint", "t", "目标点", GH_ParamAccess.list);
            pManager.AddPointParameter("Points", "p", "排序点群", GH_ParamAccess.list);
            pManager.AddNumberParameter("Distance", "D", "距离", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "p", "输出结果", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Index", "I", "索引值", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> TargetPoint = new List<Point3d>();
            List<Point3d> Points = new List<Point3d>();
            double Distance = double.NaN;
            DA.GetDataList(0, TargetPoint);
            DA.GetDataList(1, Points);
            DA.GetData(2, ref Distance);
            /*------------------------------*/
            List<Point3d> ps = new List<Point3d>();
            List<int> Index = new List<int>(10);
         //   List<Point3d> result = new List<Point3d>();
            for (int i = 0; i < TargetPoint.Count; i++)
            {
                // ps = p3();
                for (int j = 0; j < Points.Count; j++)
                {
                    if (TargetPoint[i].DistanceTo(Points[j]) <= Distance)
                    {
                        ps.Add(Points[j]);
                        Index.Add(j);
                    }
                }
            //    result.AddRange(ps);
            }
            DA.SetDataList(0, ps);
            DA.SetDataList(1, Index);
        }

        public static List<Point3d> p3()
        {
            List<Point3d> pp = new List<Point3d>();
            return pp;
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
                return Properties.Resources.Group_Points;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("472b61cd-e6aa-4cc9-818c-1f4c0388f235"); }
        }
    }
}