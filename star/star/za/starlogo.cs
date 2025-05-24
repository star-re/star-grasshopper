using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace star
{
    public class mylogo : Grasshopper.Kernel.GH_AssemblyPriority
    {
        public override Grasshopper.Kernel.GH_LoadingInstruction PriorityLoad()
        {
            Grasshopper.Instances.ComponentServer.AddCategoryIcon("star", Properties.Resources.star);
            Grasshopper.Instances.ComponentServer.AddCategoryShortName("star","**");
            Grasshopper.Instances.ComponentServer.AddCategorySymbolName("star",'*');
            Grasshopper.Instances.Settings.ConstainsEntry("star");
            return Grasshopper.Kernel.GH_LoadingInstruction.Proceed;
        }
    }
}
