using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class 作者想讲的一些话 : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the 作者想讲的一些话 class.
        /// </summary>
        public 作者想讲的一些话()
          : base("作者想说的一些话", "作者想说的一些话",
              "直接拖",
              "star", "about & star")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "Text", "Text", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string result = "其实一开始没想着要写插件来着\r\n但随着使用次数越多\r\n想着也能做个插件把功能都集成起来吧\r\n" +
                "所以star上发布的功能大部分都是gh层面稍微花点功夫就能做出来的\r\n我称它为方便型插件\r\n希望的就是在日常使用gh的时候能减少一些电池量以及阅读障碍\r\n" +
                "star插件支持永久更新，将会不断维护，更新电池\r\n谢谢用户们的支持\r\n如对电池有修改，或bug上的提出，请加入群聊568940144";
            DA.SetData(0, result);
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
                return Properties.Resources.hua;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f496e374-5362-4a33-b75f-b65376bd012f"); }
        }
    }
}