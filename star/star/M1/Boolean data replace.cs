using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;

namespace star.M1
{
    public class Boolean_data_replace : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Boolean_data_reverse class.
        /// </summary>
        public Boolean_data_replace()
          : base("Bool Replace Data", "BRData",
              "通过布尔值调换数据",
              "star", "Math")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Bool", "B", "布尔值，以调换。true指A，false指B", GH_ParamAccess.item);
            pManager.AddGenericParameter("DataA", "A", "需调换的数据", GH_ParamAccess.item);
            pManager.AddGenericParameter("DataB", "B", "调换的数据", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "D", "数据结果", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool boo = true;
            IGH_Goo dataA = null;
            IGH_Goo dataB = null ;
            DA.GetData(0, ref boo);
            DA.GetData(1, ref dataA);
            DA.GetData(2, ref dataB);
            //if (dataA == null)
            //{
            //    return;
            //}
            //if (dataB == null)
            //{
            //    return;
            //}

            //bool bo = true;
            //GH_Convert.ToBoolean(boo, out bo, GH_Conversion.Both);
            if (boo == true)
            {
                dataA = dataB;
            }
            DA.SetData(0, dataA);
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
                return Properties.Resources.bool_replace_data;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("3039c392-dc1f-4e74-bd24-6b262353f74e"); }
        }
    }
}