using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;

namespace star
{
    public class Intersection_curve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Intersection_curve class.
        /// </summary>
        public Intersection_curve()
          : base("Intersection curve", "Intersection curve",
              "Description",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "输入线组", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Curves", "C", "输入线组", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Curves", "C", "输入线组", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> x = new List<Curve>();
            DA.GetDataList(0, x);
            /*----------------------*/
            CurveIntersections intersection;
            List<int> index = new List<int>();
            List<int> indext = new List<int>();
            double a = 2;
            double b = 0;
            for (int i = 0; i < x.Count; i++)
            {
                a = 2;
                b = 0;
                for (int j = i + 1; j < x.Count; j++)
                {
                    intersection = Intersection.CurveCurve(x[i], x[j], 0.1, 0.1);
                    if (intersection.Count != 0)
                    {
                        if (a == intersection.Count)
                        {
                            index.Add(i);
                            a = 0;
                        }
                        index.Add(j);
                    }
                    else
                    {
                        //if (b == intersection.Count)
                        //{
                        //    indext.Add(i);
                        //    b = 2;
                        //}
                        indext.Add(j);
                    }
                }
            }
            DA.SetDataList(0, index);
          DA.SetDataList(1, indext);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ea866500-4981-48e5-ad6d-a5254734bdf8"); }
        }
    }
}