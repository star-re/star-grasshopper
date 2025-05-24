using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using Rhino.DocObjects;
using Grasshopper.Kernel.Types;
using System.Linq;

namespace star.Display
{
    public class Object_Status : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Object_Status class.
        /// </summary>
        public Object_Status()
          : base("Object Status", "O Status",
              "通过Guid改变物件状态",
              "star", "Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Guid", "G", "请获取想获取的ID", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Select", "Select", "选取", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Show", "Show", "显示", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Hide", "Hide", "隐藏", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Lock", "Lock", "锁定", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Unlock", "Unlock", "解锁", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Delect", "Delect", "删除", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Group", "Group", "群组", GH_ParamAccess.item);
            Params.Input[1].Optional = true;
            Params.Input[2].Optional = true;
            Params.Input[3].Optional = true;
            Params.Input[4].Optional = true;
            Params.Input[5].Optional = true;
            Params.Input[6].Optional = true;
            Params.Input[7].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool sele = false;
            bool show = false;
            bool hide = false;
            bool locK = false;
            bool unlock = false;
            bool dele = false;
            bool grou = false;

            DA.GetData(1, ref sele);
            DA.GetData(2, ref show);
            DA.GetData(3, ref hide);
            DA.GetData(4, ref locK);
            DA.GetData(5, ref unlock);
            DA.GetData(6, ref dele);
            DA.GetData(7, ref grou);

            List<Guid> g = new List<Guid>();//创建一个新guid
            List<GH_Guid> gG = new List<GH_Guid>();

            DA.GetDataList(0, gG);//输入数据给g
            g = gG.Select(i => i.Value).ToList();


            if (sele)
            {
                foreach (var item in g)
                {
                    Rhino.RhinoDoc.ActiveDoc.Objects.Select(item);
                }

            }
            if (show)
            {
                foreach (var item in g)
                {
                    Rhino.RhinoDoc.ActiveDoc.Objects.Show(item, true);
                }
            }
            if (hide)
            {
                foreach (var item in g)
                {
                    Rhino.RhinoDoc.ActiveDoc.Objects.Hide(item, true);
                }
            }
            if (locK)
            {
                foreach (var item in g)
                {
                    Rhino.RhinoDoc.ActiveDoc.Objects.Lock(item, true);
                }
            }
            if (unlock)
            {
                foreach (var item in g)
                {
                    Rhino.RhinoDoc.ActiveDoc.Objects.Unlock(item, true);
                }
            }
            if (dele)
            {
                foreach (var item in g)
                {
                    Rhino.RhinoDoc.ActiveDoc.Objects.Delete(item, true);
                }
            }
            if (grou)
            {
                Rhino.RhinoDoc.ActiveDoc.Groups.Add(g);
            }
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
                return Properties.Resources.object_status;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b5479192-2baa-468a-b16b-b4d0121aca54"); }
        }
    }
}