using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Intersect;


namespace star.starSurface
{
    public class Brep_Split : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Brep_Split class.
        /// </summary>
        public Brep_Split()
          : base("Brep Split", "Brep Split",
              "用线切割几何体",
              "star", "Surface")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "需切割的Brep", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curves", "Crs", "切割线段，需线在Brep上", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tol", "T", "公差", GH_ParamAccess.item, 0.01);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "切割后的Brep", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = new Brep();
            List<Curve> curves = new List<Curve>();
            double Tol = 0.01;
            DA.GetData(0, ref brep);
            DA.GetDataList(1, curves);
            DA.GetData(2, ref Tol);

            Curve[] curvesArray = curves.ToArray();
            int cou = brep.Faces.Count;
            Brep bb = null;

            Curve[] ncr = null;
            Point3d[] np3 = null;
            List<List<Curve>> splitcurve = new List<List<Curve>>();
            List<List<int>> splitindex = new List<List<int>>();
            for (int i = 0; i < cou; i++)
            {
                List<int> index = new List<int>();
                List<Curve> curvesplist = new List<Curve>();
                for (int j = 0; j < curves.Count; j++)
                {
                    if (Intersection.CurveBrep(curves[j], brep.Faces[i].ToBrep(), Tol, out ncr, out np3))
                    {
                        index.Add(j);
                        curvesplist.Add(curves[j]);
                    }
                }
                splitindex.Add(index);
                splitcurve.Add(curvesplist);
            }

            for (int i = 0; i < cou; i++)
            {
                if (splitindex[i].Count != 0)
                {
                    if (bb != null) 
                    {
                        brep = bb;
                    }
                    bb = brep.Faces[i].Split(splitcurve[i], Tol);
                }
            }
            DA.SetData(0, bb);
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
                return Properties.Resources.Brep_Split;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("491ede42-02f0-4462-bfb8-a11cf6a3decd"); }
        }
    }
}