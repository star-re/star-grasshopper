using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class long_short_curve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the long_short_curve class.
        /// </summary>
        public long_short_curve()
          : base("long short curve", "long short curve",
              "提取最短线与最长线\r\n如有修改请联系作者:981574914",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "曲线", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Long Curve", "L", "曲线", GH_ParamAccess.item);
            pManager.AddCurveParameter("Short Curve", "S", "曲线", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve x = null;
            DA.GetData(0, ref x);

            Curve[] splitcurve = x.DuplicateSegments();
            List<double> length = new List<double>();
            for (int i = 0; i < splitcurve.Length; i++)
            {
                length.Add(splitcurve[i].GetLength());
            }
            double[] array = length.ToArray();
            Array.Sort(array, splitcurve);

            int listlength = splitcurve.Length;
            Curve outcurve = splitcurve[listlength - 1];
            Curve startcurve = splitcurve[0];
            DA.SetData(0, outcurve);
            DA.SetData(1, startcurve);
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
                return GH_DocumentProperties.;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8907aae3-a850-4fbe-82ed-d6837af9468c"); }
        }
    }
}