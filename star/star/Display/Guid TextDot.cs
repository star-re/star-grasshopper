using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Rhino.DocObjects;

namespace star.Display
{
    public class Guid_TextDot : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Guid_TextDot class.
        /// </summary>
        public Guid_TextDot()
          : base("Guid TextDot", "Guid TextDot",
              "通过Guid获取Dot文字属性",
              "star", "Display")
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
            pManager.AddPointParameter("Plane", "P", "中心平面", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Length", "L", "文字长度", GH_ParamAccess.item);
            pManager.Register_DoubleParam("Height", "H", "文字高度", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object textguid = new object();
            //Guid g = default(Guid);//创建一个新guid
           // GH_Guid gG = new GH_Guid();


            TextDotObject to;
            TextDot textDot = null;

            DA.GetData(0, ref textguid);//输入数据给g
            string textdottype = "Rhino.Geometry.TextDot";
            string textdottype1 = textguid.ToString();
            if (textdottype1 == textdottype)
            {
              //  textDot = (TextDot)textguid;
                GH_ObjectWrapper gow = (GH_ObjectWrapper)textguid;
                textDot = (TextDot)gow.Value;
            }
            else
            {
                string a = textguid.ToString();
                Guid g = new Guid(a);
                to = Rhino.RhinoDoc.ActiveDoc.Objects.Find(g) as TextDotObject;//查找此guid并转换数据给TO
                textDot = to.Geometry as TextDot;
            }

            string text;

            text = textDot.Text;
            DA.SetData(0, text);//返回值给输出端

            Point3d dotpoints = textDot.Point;
            DA.SetData(1, dotpoints);


            int length = text.Length;//获取输入字的数量
            DA.SetData(2, length);

            int height = textDot.FontHeight;
            DA.SetData(3, height);
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
                return Properties.Resources.dotText;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ee2fa339-49e9-4386-9b0f-e372186ea6fe"); }
        }
    }
}