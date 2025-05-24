using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
//using Rhino.Geometry;
using Grasshopper;
using System.Drawing;
using System.Linq;
using Grasshopper.Kernel.Special;

namespace star.M1
{
    public class DataOutput : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DataOutput class.
        /// </summary>
        public DataOutput()
          : base("DataOutput", "DataOutput",
              "数据输出，需获取电池的变量名",
              "star", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Name", "Name", "电池名字", GH_ParamAccess.item);
            Params.Input[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "Data", "数据", GH_ParamAccess.tree);
        }

        public GH_Document GrasshopperDocuments = new GH_Document();

        public void GroupFlag(Guid guidInstance)
        {
            List<GH_Group> gg = GrasshopperDocuments.Objects.Where(i => i.GetType() == typeof(GH_Group)).Select(i => i as GH_Group).ToList();
            for (int i = 0; i < gg.Count; i++)
            {
                if (gg[i].ObjectIDs.Contains(guidInstance))
                {
                    InGroup = true;
                    basegroup = gg[i];
                    return;
                }
                else
                {

                }

            }
            InGroup = false;
            return;
            //A = gg.ObjectIDs.Contains(this.Component.InstanceGuid);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string componentname = string.Empty;
            DA.GetData(0, ref componentname);
            GrasshopperDocuments = this.OnPingDocument();

            this.Name = string.Format("DataOutput:{0}", componentname);
            this.NickName = string.Format("DataOutput:{0}", componentname); ;
            if (componentname == string.Empty)
            {
                Message = "???的数据";
            }
            else
            {
                Message = String.Format("{0}的数据", componentname);
            }
            /*---------------------------------------*/
            if (componentname == string.Empty)
            {
                return;
            }

            if (!componentpoint.IsEmpty)
            {
                IGH_Component ic = GrasshopperDocuments.FindComponent(componentpoint);
                if (ic != null && ic.Name == componentname)
                {
                    DataInput input = new DataInput();
                    GroupFlag(ic.InstanceGuid);
                    if (InGroup)
                    {
                        if (basegroup.ObjectIDs.Contains(this.InstanceGuid))
                        {
                            this.Params.Output[0].ClearData();
                            IGH_Structure ighs = ic.Params.Output[0].VolatileData;
                            DA.SetDataTree(0, ighs);
                            return;
                        }
                        else
                        {
                            string ss = string.Format("此变量电池在{0}群组里，外部环境无法获取", basegroup.NickName);
                            DataTree<string> dataTree = new DataTree<string>();
                            dataTree.Add(ss);
                            DA.SetDataTree(0, dataTree);
                            return;
                        }
                    }
                    else
                    {
                        goto tiaozhuan;
                    }
                }
                else
                {
                    goto tiaozhuan;
                }
            }


        tiaozhuan: IList<IGH_DocumentObject> ido = GrasshopperDocuments.Objects;
            List<string> comname = ido.Where(i => i.Name == componentname).Select(i => i.Name).ToList();
            IList<IGH_DocumentObject> ido1 = ido.Where(i => i.Name == componentname).ToList();

            for (int i = 0; i < ido1.Count; i++)
            {
                IGH_Component ic = GrasshopperDocuments.FindComponent(ido1[i].InstanceGuid);
                if (ic != null)
                {
                    if (ic.Name == componentname)
                    {
                        GroupFlag(ic.InstanceGuid);
                        if (InGroup)
                        {
                            if (basegroup.ObjectIDs.Contains(this.InstanceGuid))
                            {
                                this.Params.Output[0].ClearData();
                                componentpoint = new Point((int)ic.Attributes.Pivot.X, (int)ic.Attributes.Pivot.Y);
                                IGH_Structure ighs = ic.Params.Output[0].VolatileData;
                                //    this.Params.Output[i].AddVolatileData() = ighs;
                                //  IGH_GeometricGoo ggg = new GH_GeometricGoo();
                                DA.SetDataTree(0, ighs);//ighs.AllData(false));
                                return;
                            }
                            else
                            {
                                if (comname.Count == 1)
                                {
                                    string ss = string.Format("此变量电池在{0}群组里，外部环境无法获取", basegroup.NickName);
                                    DataTree<string> dataTree = new DataTree<string>();
                                    dataTree.Add(ss);
                                    DA.SetDataTree(0, dataTree);
                                    return;
                                }
                                else
                                {
                                    if (!basegroup.ObjectIDs.Contains(this.InstanceGuid))
                                    {
                                        string ss = string.Format("此变量电池在{0}群组里，外部环境无法获取", basegroup.NickName);
                                        DataTree<string> dataTree = new DataTree<string>();
                                        dataTree.Add(ss);
                                        DA.SetDataTree(0, dataTree);
                                    }
                                }
                            }
                        }
                        else
                        {
                            //string ss = string.Format("此变量电池在{0}群组里，外部环境无法获取", basegroup.NickName);
                            //DataTree<string> dataTree = new DataTree<string>();
                            //dataTree.Add(ss);
                            //DA.SetDataTree(0, dataTree);
                            //return;
                            componentpoint = new Point((int)ic.Attributes.Pivot.X, (int)ic.Attributes.Pivot.Y);
                            IGH_Structure ighs = ic.Params.Output[0].VolatileData;
                            //    this.Params.Output[i].AddVolatileData() = ighs;
                            //  IGH_GeometricGoo ggg = new GH_GeometricGoo();
                            DA.SetDataTree(0, ighs);//ighs.AllData(false));
                        }
                    }
                }
            }
            //string ss1 = string.Format("此变量电池在{0}群组里，外部环境无法获取", basegroup.NickName);
            //DataTree<string> dataTree1 = new DataTree<string>();
            //dataTree1.Add(ss1);
            //DA.SetDataTree(0, dataTree1);
        }
        public bool InGroup = false;
        public GH_Group basegroup;
        public Point componentpoint;
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.dataoutput;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("23CE84A4-24E6-4174-B575-20F83B689D2F"); }
        }
    }
}