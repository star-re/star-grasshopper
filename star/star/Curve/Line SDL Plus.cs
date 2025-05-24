using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class Line_SDL_Plus : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Line_SDL_Plus class.
        /// </summary>
        public Line_SDL_Plus()
          : base("Line SDL Plus", "Line Plus",
              "创建由起点、单个或多个的（矢量和长度）定义的线段。",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Plane", "Pl", "平面", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "D", "单个或多个矢量", GH_ParamAccess.list);
            pManager.AddNumberParameter("Length", "L", "单个或多个长度", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Polyline", "PLine", "线段", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d x = Plane.WorldXY.Origin;
            Plane basePlane = new Plane();
            List<Vector3d> listDir = new List<Vector3d>();
            List<double> listLength = new List<double>();
            DA.GetData(0, ref basePlane);
            DA.GetDataList(1, listDir);
            DA.GetDataList(2, listLength);

            Transform TT = Transform.PlaneToPlane(Plane.WorldXY, basePlane);
            if (listDir.Count == 0 || listLength.Count == 0)
            {
                return;
            }
            List<Point3d> pts = new List<Point3d>();
            Vector3d v3 = listDir[0];
            double d = listLength[0];
            pts.Add(x);
            int num = -1;


            if (listDir.Count == listLength.Count)
            {
                for (int i = 0; i < listDir.Count; i++)
                {
                    v3 = listDir[i];
                    v3.Unitize();
                    v3 *= listLength[i];
                    //        y[i].Unitize();
                    //        y[i] *= z[i];
                    x.Transform(Transform.Translation(v3));
                    pts.Add(x);
                }
            }
            else if (listDir.Count > listLength.Count)
            {
                for (int i = 0; i < listDir.Count; i++)
                {
                    num++;
                    if (num == listLength.Count)
                    {
                        num = 0;
                    }
                    v3 = listDir[i];
                    v3.Unitize();
                    v3 *= listLength[num];
                    x.Transform(Transform.Translation(v3));
                    pts.Add(x);
                }
            }
            else if (listLength.Count > listDir.Count)
            {
                for (int i = 0; i < listLength.Count; i++)
                {
                    num++;
                    if (num == listDir.Count)
                    {
                        num = 0;
                    }
                    v3 = listDir[num];
                    v3.Unitize();
                    v3 *= listLength[i];
                    x.Transform(Transform.Translation(v3));
                    pts.Add(x);
                }
            }

            Curve cc = Curve.CreateControlPointCurve(pts, 1);
            cc.Transform(TT);
            DA.SetData(0, cc);
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
                return Properties.Resources.Line_SDL_Plus;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("227fbeda-9984-4010-9996-273dd401c7be"); }
        }
    }
}