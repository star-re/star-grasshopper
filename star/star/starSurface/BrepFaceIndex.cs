using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;

namespace star.starSurface
{
    public class BrepFaceIndex : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the BrepFaceIndex class.
        /// </summary>
        public BrepFaceIndex()
          : base("Edge&FaceIndex", "Edge&FaceIndex",
              "面与边缘的索引",
              "star", "Surface")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "Brep", GH_ParamAccess.item);
            pManager.AddPointParameter("Point3d", "Pt", "需选取边缘与面的参照点", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Edge index", "E_i", "边缘索引", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Face index", "F_i", "面索引", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep brep = new Brep();
            Point3d p3 = Point3d.Unset;
            DA.GetData(0, ref brep);
            DA.GetData(1, ref p3);

            
            HashSet<int> hashSet = new HashSet<int>();
            int[] array;
            int[] other;
            int[] array2;
            brep.FindCoincidentBrepComponents(p3, 0.001, out array, out other, out array2);
            hashSet.UnionWith(other);
            DA.SetDataList(0, hashSet);
            DA.SetDataList(1, array);
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
                return Properties.Resources.EdgeFaceIndex;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a46191fd-fc97-4a79-99d8-2a3096102f34"); }
        }
    }
}