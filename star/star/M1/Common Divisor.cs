using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Runtime.CompilerServices;

[assembly: SuppressIldasm()]
namespace star
{
    public class Common_Divisor : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Common_Divisor class.
        /// </summary>
        public Common_Divisor()
          : base("Common_Divisor", "Common_Divisor",
              "求一组数的最大公约数和最小公倍数\r\n此电池图标设计思路源自hare电池作者兔子，请支持原作者",
              "star", "Math")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("number", "n", "请输入一组整数", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Common Divisor", "GCD", "最大公约数", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Common Multiple", "LCM", "最小公倍数", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<int> num = new List<int>();
            DA.GetDataList(0, num);

            int gcdResult = stardy.GCD(num);
            DA.SetData(0, gcdResult);
            int lcmResult = stardy.LCM(num);
            DA.SetData(1, lcmResult);
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
                return Properties.Resources.Common_Divisor;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ab153b7a-ecc9-4d5e-a41d-f72e71e41e33"); }
        }
    }
}