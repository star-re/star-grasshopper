using System;
using System.Collections.Generic;
using System.Drawing;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.DocObjects;
using Grasshopper.Kernel.Types;
using Rhino.Display;

namespace star
{
    public class Guid_Text : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Guid_Text class.
        /// </summary>
        public Guid_Text()
          : base("Guid_Text", "Guid_Text",
              "通过Guid获取文字属性",
              "star", "display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Guid", "G", "请获取想获取的ID", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "T", "文字", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Length", "L", "文字长度", GH_ParamAccess.item);
            pManager.Register_DoubleParam("Height", "H", "文字高度", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "P", "中心平面", GH_ParamAccess.item);
            pManager.AddNumberParameter("Angle", "A", "文字角度", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Guid g = default(Guid);//创建一个新guid
            GH_Guid gG = new GH_Guid();

            DA.GetData(0, ref gG);//输入数据给g
            g = gG.Value;


            TextObject to;
            string text;
            to = Rhino.RhinoDoc.ActiveDoc.Objects.Find(g) as TextObject;//查找此guid并转换数据给TO
            text = to.DisplayText;
            DA.SetData(0, text);//返回值给输出端


            int length = text.Length;//获取输入字的数量
            DA.SetData(1, length);

            double height =to.TextGeometry.TextHeight;
            DA.SetData(2, height);
           

            Point3d centerP = to.TextGeometry.GetBoundingBox(true).Center;
            Plane p3 = to.TextGeometry.Plane;
            p3.Origin = centerP;   //直接把centerP的位置给p3即可把平面移动过去
            DA.SetData(3, p3);


            Vector3d vx = to.TextGeometry.Plane.XAxis;
            Vector3d vxx = Vector3d.XAxis;
            double vtradian = Vector3d.VectorAngle(vx, vxx);
            double vtangle = 180 / Math.PI * vtradian;
            DA.SetData(4, vtangle);
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
                return Properties.Resources.guid_text;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b5221e9b-1576-4cdc-9565-d38c0a110f84"); }
        }
    }
}