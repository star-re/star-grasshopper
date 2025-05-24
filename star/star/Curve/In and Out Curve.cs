using System;
using System.Collections.Generic;

using Rhino;
using Rhino.Geometry;
using Grasshopper.Kernel;

namespace star
{
    public class In_and_Out_Curve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the In_and_Out_Curve class.
        /// </summary>
        public In_and_Out_Curve()
          : base("In&Out Curves", "In&Out Cs",
              "在Brep上的里外线",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "几何体", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("In", "I", "里", GH_ParamAccess.list);
            pManager.AddCurveParameter("On", "O", "外", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = new Brep();
            DA.GetData(0, ref brep);

            Curve[] _in = new Curve[] { };
            Curve[] _out = new Curve[] { };
            _in = brep.DuplicateNakedEdgeCurves(false, true);
            _out = brep.DuplicateNakedEdgeCurves(true, false);
            DA.SetDataList(0, _in);
            DA.SetDataList(1, _out);
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
                return Properties.Resources.Brep_Edge_In_ON;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7541365e-2e03-4910-969a-75d7934ca638"); }
        }
    }
}