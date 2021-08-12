using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Types.Transforms;
using Rhino.Geometry;

namespace star
{
    public class Vector_Align : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Vector_Align class.
        /// </summary>
        public Vector_Align()
          : base("Vector Align", "Vector Align",
              "通过矢量对齐物件",
              "star", "Transform")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "进行矢量对齐的物件", GH_ParamAccess.item);
            pManager.AddVectorParameter("Vector", "V", "物件矢量", GH_ParamAccess.item);
            pManager.AddVectorParameter("Align Vector", "V", "绝对矢量", GH_ParamAccess.item, Vector3d.XAxis);
            pManager.AddPlaneParameter("Plane", "P", "对齐基准平面", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry Result", "R", "对齐结果", GH_ParamAccess.item);
            pManager.AddTransformParameter("Transform", "T", "变换", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //IGH_GeometricGoo geometry = null;  //用这个好
            GeometryBase geometry = null;   //尽量避免使用geometryBase，几何体被编辑后有可能强制变回最基层的形态*/
            Vector3d vectorA = new Vector3d();
            Vector3d vectorB = new Vector3d();
            Plane plane = new Plane();
            DA.GetData(0, ref geometry);
            DA.GetData(1, ref vectorA);
            DA.GetData(2, ref vectorB);
            DA.GetData(3, ref plane);

            double angle = Vector3d.VectorAngle(vectorA, vectorB);
            double caseangle = Vector3d.VectorAngle(vectorB, Vector3d.XAxis);
            double pi = Math.PI * 0.5;


            if (angle > pi)
            {
                angle = Math.PI - angle;
            }
            if (caseangle > ((Math.PI * 0.5) * 0.5))
            {
                if (vectorA.X > 0 && vectorA.Y > 0)
                {
                    angle = Math.Abs(angle);
                    //DA.SetData(0, angle);
                }
                else if (vectorA.X < 0 && vectorA.Y > 0)
                {
                    angle = -angle;
                    //DA.SetData(0, angle);
                }
                else if (vectorA.X < 0 && vectorA.Y < 0)
                {
                    angle = Math.Abs(angle);
                    //DA.SetData(0, angle);
                }
                else if (vectorA.X > 0 && vectorA.Y < 0)
                {
                    angle = -angle;
                    //DA.SetData(0, angle);
                }
            }
            else
            {
                if (vectorA.X > 0 && vectorA.Y > 0)
                {
                    angle = -angle;
                }
                else
                {
                    if (vectorA.X < 0 && vectorA.Y < 0)
                    {
                        angle = -angle;
                    }
                }
            }
            Vector3d cross = Vector3d.ZAxis;

            Point3d center = geometry.GetBoundingBox(true).Center;
            if (!plane.IsValid)
            {
                Transform transform = Transform.Rotation(angle, cross, plane.Origin);
                //IGH_GeometricGoo result = geometry.Transform(transform);
                if (geometry.Transform(transform))
                {
                    DA.SetData(0, geometry);
                    DA.SetData(1, transform);
                }
            }
            else
            {
                Transform transform = Transform.Rotation(angle, cross, center);
                //IGH_GeometricGoo result = geometry.Transform(transform);
                if (geometry.Transform(transform))
                {
                    DA.SetData(0, geometry);
                    DA.SetData(1, transform);
                }
            }
            //  DA.SetData(1);
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
                return Properties.Resources.Vector_ailgn;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("60600024-a1cb-4aac-88b5-73601e5c8409"); }
        }
    }
}