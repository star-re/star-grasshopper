using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;

namespace star.starSurface
{
    public class ConnectSrf : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ConnectSrf class.
        /// </summary>
        public ConnectSrf()
          : base("ConnectSrf", "ConnectSrf",
              "曲面连接（更新中）",
              "star", "Surface")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface A", "A", "曲面A", GH_ParamAccess.item);
            pManager.AddSurfaceParameter("Surface B", "B", "曲面B", GH_ParamAccess.item);
            pManager.AddPointParameter("ConnectPtA", "A", "连接对应点A", GH_ParamAccess.item);
            pManager.AddPointParameter("ConnectPtB", "B", "连接对应点B", GH_ParamAccess.item);
            pManager.AddNumberParameter("Length", "L", "延伸距离", GH_ParamAccess.item, 1000);
            ;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Surface A", "A", "曲面A", GH_ParamAccess.item);
            pManager.AddBrepParameter("Surface B", "B", "曲面B", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Surface surface = null;
            Surface surface1 = null;
            Point3d point = new Point3d();
            Point3d point1 = new Point3d();
            double length = 1000;
            DA.GetData(0, ref surface);
            DA.GetData(1, ref surface1);
            DA.GetData(2, ref point);
            DA.GetData(3, ref point1);
            DA.GetData(4,ref length);

            Brep exa = srfExtend(surface, point, length);
            Brep exb = srfExtend(surface1, point1, length);

            Brep[] a = exa.Trim(srfPlane(surface1), 0.1);
            Brep[] b = exb.Trim(srfPlane(surface), 0.1);
            Brep aa = rebuidbrep(a[0]);
            Brep bb = rebuidbrep(b[0]);
            DA.SetData(0, aa);
            DA.SetData(1,bb);
        }

        public static Brep srfExtend(Surface surface, Point3d point, double length)
        {
            double a = double.NaN;
            double b = double.NaN;
            surface.SetDomain(0,new Interval(0, 1.0));
            surface.SetDomain(1,new Interval(0, 1.0));
            surface.ClosestPoint(point, out a, out b);
            IsoStatus edge = surface.ClosestSide(a, b);
            Surface exa = surface.Extend(edge, length, true);
            return exa.ToBrep();
        }

        /// <summary>
        /// 找出平板上的平面
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public static Plane srfPlane(Surface surface)
        { 
        Plane plane = Plane.WorldXY;
            surface.TryGetPlane(out plane,0.01);
            return plane;
        }

        /// <summary>
        /// 三边或四边重建Brep
        /// </summary>
        /// <param name="brep"></param>
        /// <returns></returns>
        public static Brep rebuidbrep(Brep brep)
        {
            BrepCurveList bcl = brep.Curves3D;
            Brep aa = Brep.CreateEdgeSurface(bcl);
            return aa;
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
                return Properties.Resources.Connect_Srf;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("0E09E5B2-A61B-4FC4-BB4B-EA97C2867B13"); }
        }
    }
}