using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System.Drawing;
using Grasshopper.Kernel.Special;
using System.Linq;

namespace star.M1
{
    public class DataInput : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public DataInput()
          : base("DataInput", "DataInput",
              "数据接入，变量名一定要给",
              "star", "Data")
        {
        }


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "Data", "数据", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Name", "Name", "变量名", GH_ParamAccess.item);
            Params.Input[0].Optional = true;
            Params.Input[1].Optional = true;

        }

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            IList<IGH_DocumentObject> ido = GrasshopperDocuments.Objects;
            //GrasshopperDocument.FindComponent(new Guid("23CE84A4-24E6-4174-B575-20F83B689D2F"));
            for (int i = 0; i < ido.Count; i++)
            {
                IGH_Component ic = GrasshopperDocuments.FindComponent(ido[i].InstanceGuid);
                if (ic != null)
                {
                    if (ic.Name == outputname)
                    {
                        ic.ExpireSolution(true);
                    }
                }
            }
        }

        private void GrasshopperDocuments_ObjectsAdded(object sender, GH_DocObjectEventArgs e)
        {
            throw new NotImplementedException();
        }

        public GH_Document GrasshopperDocuments = new GH_Document();
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "Data", "数据", GH_ParamAccess.tree);
        }



        public void GroupFlag()
        {
            List<GH_Group> gg = GrasshopperDocuments.Objects.Where(i => i.GetType() == typeof(GH_Group)).Select(i => i as GH_Group).ToList();
            for (int i = 0; i < gg.Count; i++)
            {
                if (gg[i].ObjectIDs.Contains(this.InstanceGuid))
                {
                    InGroup = true;
                    basegroup = gg[i];
                    return;
                }
                else
                {
                    InGroup = false;
                    return;
                }
            }
            //A = gg.ObjectIDs.Contains(this.Component.InstanceGuid);
        }


        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GrasshopperDocuments = this.OnPingDocument();
            componentname = string.Empty;
            //   GH_Structure<IGH_Goo> gH_Structure = new GH_Structure<IGH_Goo>();
            //   DA.GetDataTree(0, out gH_Structure);
            DA.GetData(1, ref componentname);
            this.Name = componentname;
            this.NickName = componentname;

            outputname = string.Format("DataOutput:{0}", componentname);
            if (componentname == string.Empty)
            {
                Message = "变量名为:???";
            }
            else
            {
                Message = String.Format("变量名为:{0}", componentname);
            }

            DA.SetDataTree(0, this.Params.Input[0].VolatileData);
            GroupFlag();
            if (pointFs.Count != 0)
            {
                ComponentPivChange(DA);
            }
            Params_ParameterSourcesChanged1();

            //Params.ParameterSourcesChanged += Params_ParameterSourcesChanged2;
            //base.Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
            //  DA.SetDataTree(0, gH_Structure);
            //IGH_GeometricGoo gH_Goo = null;
            // DA.GetData(0, ref  gH_Goo);
        }

        string componentname;
        public List<PointF> pointFs = new List<PointF>();
        public bool InGroup = false;
        public GH_Group basegroup;
        private void Params_ParameterSourcesChanged1()
        {
            pointFs.Clear();
            IList<IGH_DocumentObject> ido = null;
            if (InGroup)
            {
                ido = basegroup.Objects();
            }
            else
            {
                ido = GrasshopperDocuments.Objects;
            }
            //GrasshopperDocument.FindComponent(new Guid("23CE84A4-24E6-4174-B575-20F83B689D2F"));
            for (int i = 0; i < ido.Count; i++)
            {
                IGH_Component ic = GrasshopperDocuments.FindComponent(ido[i].InstanceGuid);
                if (ic != null)
                {
                    if (ic.Name == outputname)
                    {
                        pointFs.Add(ic.Attributes.Pivot);
                        ic.ExpireSolution(true);
                    }
                }
            }
        }

        private void ComponentPivChange(IGH_DataAccess DA)
        {
            for (int i = 0; i < pointFs.Count; i++)
            {
                IGH_Component ic = GrasshopperDocuments.FindComponent(new System.Drawing.Point((int)pointFs[i].X, (int)pointFs[i].Y));
                if (ic != null && ic.Name == componentname)
                {
                    IGH_Structure ighs = this.Params.Output[0].VolatileData;
                    DA.SetDataTree(0, ighs);
                }
                else
                {
                    Params_ParameterSourcesChanged1();
                }
            }
            return;
        }

        public string outputname = string.Empty;
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.datainput;
            }
        }


        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2afeac4b-5348-49d1-929a-f30f16860d1e"); }
            // get { return getGUID(); }
        }

        public static Guid getGUID()
        {
            System.Guid guid = new Guid();
            guid = Guid.NewGuid();
            return guid;
        }
    }
}