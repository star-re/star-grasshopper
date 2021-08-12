using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Runtime.CompilerServices;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.


namespace star
{
    public class starComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public starComponent()
          : base("number domain", "number domain",
              "以数字分离区间",
              "star", "Domain")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("number", "num", "请输入转化为domain的数字", GH_ParamAccess.item);
            pManager.AddNumberParameter("start", "S", "定义起始值（可空）", GH_ParamAccess.item,double.NaN);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntervalParameter("domain", "do", "输出domain", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double domainnumber = double.NaN;
            double start = double.NaN;
            DA.GetData(0, ref domainnumber);
            DA.GetData(1, ref start);

            Interval result = new Interval();
            double offsetnumber = domainnumber + start;
            if (double.IsNaN(start))
            {
                 result =new Interval((-domainnumber / 2), (domainnumber / 2));
            }
            else
            {
                result = new Interval(start, offsetnumber);
            }
           
            DA.SetData(0, result);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Properties.Resources.number_domain;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ae8adbe8-2e12-44f2-85d4-bddd19a8f97d"); }
        }
    }
}
