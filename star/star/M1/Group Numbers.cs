using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;

namespace star.M1
{
    public class Group_Numbers : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Group_Numbers class.
        /// </summary>
        public Group_Numbers()
          : base("Group Numbers", "Group Numbers",
              "数字分组",
              "star", "Math")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Numbers", "Num", "需分组的数字", GH_ParamAccess.list);
            pManager.AddNumberParameter("Distance", "D", "数组范围", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Array result", "A", "需分组的数字", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> numbers = new List<double>();
            double distance = double.NaN;
            DA.GetDataList(0, numbers);
            DA.GetData(1, ref distance);

            DataTree<double> result = new DataTree<double>();
            result = Re(numbers, distance);
            DA.SetDataTree(0, result);
        }

        public static DataTree<double> Re(List<double> intput, double diff)
        {
            List<double> rongqi = intput;
            List<double> rongqi2 = new List<double>();
            rongqi2.AddRange(rongqi);
            List<double> xx = null;
            DataTree<double> merge = new DataTree<double>();
            DataTree<int> index = new DataTree<int>();
            List<int> indexlist = new List<int>();
            int pathindex = 0;
            int i = 0;
            while (rongqi2.Count != 0)
            {
                GH_Path path = new GH_Path(pathindex,i);
                xx = returnlist();
                Interval start = new Interval(rongqi2[0]-diff, rongqi2[0] + diff);
                for (int j = 0; j < rongqi2.Count; j++)
                {
                    bool a111 = start.IncludesParameter(rongqi2[j]);
                    if (a111)
                    {
                        xx.Add(rongqi2[j]);
                        rongqi2.RemoveAt(j);
                        indexlist.Add(j);
                        j--;
                    }
                }
               // merge = returntree();
                merge.AddRange(xx, path);
                i++;
            }
            return merge;
        }

        public static DataTree<int> indextree;
        public static List<double> returnlist()
        {
            List<double> xx = new List<double>();
            return xx;
        }

        public static DataTree<double> returntree()
        {
            DataTree<double> xx1 = new DataTree<double>();
            return xx1;
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                GH_DocumentObject gdo = this;
                gdo.SetIconOverride(Properties.Resources.Group_Numbers);
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e3f79bb9-404e-4dd4-8834-99e4baff6483"); }
        }
    }
}