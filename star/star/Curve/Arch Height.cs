using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

namespace star
{
    public class Arch_Height : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Arch_Height class.
        /// </summary>
        public Arch_Height()
          : base("Arch Height Curve", "Arch Height Curve",
              "根据拱高生成曲线点",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "曲线", GH_ParamAccess.item);
            pManager.AddNumberParameter("Arch Height", "Arch H", "拱高值", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("Tolerance", "T", "容差", GH_ParamAccess.item, 0.1);
            pManager.AddIntegerParameter("Iteration", "I", "迭代次数", GH_ParamAccess.item, 1000);
            pManager.AddBooleanParameter("Keep", "K", "最后结果过短时，是否保留（默认保留）", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pts", "生成点", GH_ParamAccess.list);
            pManager.AddCurveParameter("Curves", "Crs", "生成线", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve x = null;
            double y = double.NaN;
            double z = 0.1;
            int u = 1000;
            bool b = false;
            DA.GetData(0, ref x);
            DA.GetData(1, ref y);
            DA.GetData(2, ref z);
            DA.GetData(3, ref u);
            DA.GetData(4, ref b);
            double a = 0;
            Interval curvedim = new Interval(y - z / 2, y + z / 2);
            List<Point3d> p3list = new List<Point3d>();
            p3list.Add(x.PointAtStart);
            x.Domain = new Interval(0, 1);
            double ii = 1;
            List<double> curveT = new List<double>();
            for (double i = 0; i < 1; i = i + (ii / u))
            {
                Point3d pointb = x.PointAt(i);
                Point3d pointmiddle = Averagepoint(p3list[p3list.Count - 1], pointb);
                x.ClosestPoint(pointmiddle, out a);
                Point3d point3d = x.PointAt(a);
                if (curvedim.IncludesParameter(Math.Round(pointmiddle.DistanceTo(point3d), 2)))
                {
                    p3list.Add(pointb);
                    curveT.Add(i);
                    a = 0;
                }
            }
            Curve[] curves = splitTcurve(x, curveT, b);
            if (b)
            {
                if (curves.Length == p3list.Count - 1)
                {
                    p3list.RemoveAt(p3list.Count - 1);
                }
            }
            p3list.Add(x.PointAtEnd);
            DA.SetDataList(0, p3list);
            DA.SetDataList(1, curves);
        }

        public Curve[] splitTcurve(Curve x, List<double> curveT, bool b)
        {
            Curve[] cc = x.Split(curveT);
            List<double> curveslength = new List<double>();
            for (int i = 0; i < cc.Length; i++)
            {
                curveslength.Add(cc[i].GetLength());
            }
            if (b)
            {
                double zuihouyiduanchangdu = curveslength[curveslength.Count - 1];
                double averagelength = curveslength.Average();
                if (zuihouyiduanchangdu < averagelength)
                {
                    curveT.RemoveAt(curveslength.Count - 2);
                }
                Curve[] ccc = x.Split(curveT);
                return ccc;
            }

            return cc;
        }
        public Point3d Averagepoint(Point3d pa, Point3d pb)
        {
            List<double> xa = new List<double>();
            List<double> ya = new List<double>();
            List<double> za = new List<double>();
            xa.Add(pa.X);
            xa.Add(pb.X);
            ya.Add(pa.Y);
            ya.Add(pb.Y);
            za.Add(pa.Z);
            za.Add(pb.Z);
            return new Point3d(xa.Average(), ya.Average(), za.Average());
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                GH_DocumentObject gd = this;
                gd.SetIconOverride(Properties.Resources.arch_height);
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                //return Properties.Resources.arch_height;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("E950D0F7-9DE2-4A1A-B6BE-7098DEA3E22A"); }
        }
    }
}