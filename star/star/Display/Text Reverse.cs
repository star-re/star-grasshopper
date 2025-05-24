using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star.Display
{
    public class Text_Reverse : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Text_Reverse class.
        /// </summary>
        public Text_Reverse()
          : base("Text Reverse", "Text Reverse",
              "文字反转，顾名思义，就是（转反字文）",
              "star", "Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "T", "文字反转：不爽，你打我", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Result", "R", "结果： 我打你，爽不", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string str = string.Empty;
            DA.GetData(0, ref str);
            string result = Reverse(str);
            DA.SetData(0, result);
        }

        public string Reverse(string input)
        {
            char[] array = input.ToCharArray();
            Array.Reverse(array);
            string result = new string(array);
            return result;
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
                return Properties.Resources.textreverse;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("4fa5938f-9afd-4eec-817e-84b8f81f3ec2"); }
        }
    }
}