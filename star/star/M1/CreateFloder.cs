using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.IO;
using Grasshopper.Kernel.Parameters;

namespace star.M1
{
    public class CreateFloder : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateFloder class.
        /// </summary>
        public CreateFloder()
          : base("CreateFloder", "CF",
              "创建文件夹",
              "star", "Data")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_FilePath(), "FolderPath", "FP", "文件夹路径", GH_ParamAccess.item);
            //pManager.AddFieldParameter("FolderPath", "FP", "文件夹路径", GH_ParamAccess.item);
            pManager.AddTextParameter("FolderName", "FN", "文件夹名称", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Create", "C", "点击创建", GH_ParamAccess.item,false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string folderpath = string.Empty;
            string foldername = string.Empty;
            bool flag = false;
            DA.GetData(0, ref folderpath);
            DA.GetData(1, ref foldername);
            DA.GetData(2, ref flag);

            string folder = folderpath + foldername; // 指定文件夹路径

            if (flag)
            {
                // 检查文件夹是否已存在，如果不存在则创建
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
            }

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
                return Properties.Resources.CreateFolder;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("82652a6e-49af-46cd-a064-80ccba8ef111"); }
        }
    }
}