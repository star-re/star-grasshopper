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
    public class DataInAndOutAttributes
    {
        public void InComponent(GH_Component gH_Component)
        {
            this.InputComponent = gH_Component;
        }

        public void OutComponent(GH_Component gH_Component)
        {
            this.OutputComponent = gH_Component;
        }


        //private void Params_ParameterSourcesChanged1()
        //{
        //    pointFs.Clear();
        //    IList<IGH_DocumentObject> ido = null;
        //    if (InGroup)
        //    {
        //        ido = basegroup.Objects();
        //    }
        //    else
        //    {
        //        ido = GrasshopperDocuments.Objects;
        //    }
        //    //GrasshopperDocument.FindComponent(new Guid("23CE84A4-24E6-4174-B575-20F83B689D2F"));
        //    for (int i = 0; i < ido.Count; i++)
        //    {
        //        IGH_Component ic = GrasshopperDocuments.FindComponent(ido[i].InstanceGuid);
        //        if (ic != null)
        //        {
        //            if (ic.Name == outputname)
        //            {
        //                pointFs.Add(ic.Attributes.Pivot);
        //                ic.ExpireSolution(true);
        //            }
        //        }
        //    }
        //}

        public GH_Document GrasshopperDocuments = new GH_Document();
        public GH_Component InputComponent { get; set; }
        public GH_Component OutputComponent { get; set; }
        string componentname;
        public List<PointF> pointFs = new List<PointF>();
        public bool InGroup = false;
        public GH_Group basegroup;
    }
}
