using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System.Linq;

namespace star.starSurface
{
    public class Extract_Surface_From_Length : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Extract_Surface_From_Length class.
        /// </summary>
        public Extract_Surface_From_Length()
          : base("Extract Surface From Length", "Extract Srf F Len",
              "根据长度提取符合条件的曲面",
              "star", "Surface")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "Brep", "几何体", GH_ParamAccess.item);
            pManager.AddNumberParameter("Lenghts", "Lens", "长度", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tol", "T", "公差", GH_ParamAccess.item,0.01);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Result", "result", "提取结果", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Index", "i", "在Brep里的序号", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep _Brep = new Brep();
            List<double> _Lengths = new List<double>();
            double _Tol = 0.01;
            DA.GetData(0, ref _Brep);
            DA.GetDataList(1, _Lengths);
            DA.GetData(2, ref _Tol);

            List<Brep> result = new List<Brep>();
            List<int> indexs = new List<int>();
            int flag = 0;
            int Index = 0;
            BrepFaceList bfl = _Brep.Faces;
            List<double> D_Lengths = _Lengths.Select(i => i).ToList();
            for (int qi = 0; qi < bfl.Count; qi++)
            {
                BrepEdgeList bel = bfl[qi].DuplicateFace(true).Edges;
                for (int beli = 0; beli < bel.Count; beli++)
                {
                    for (int i = 0; i < D_Lengths.Count; i++)
                    {
                        Interval domaini = new Interval(D_Lengths[i] - _Tol, D_Lengths[i] + _Tol);
                        if (domaini.IncludesParameter(bel[beli].GetLength()))
                        {
                            flag++;
                            D_Lengths.Remove(D_Lengths[i]);
                            if (flag == _Lengths.Count)
                            {
                                result.Add(bfl[qi].DuplicateFace(true));
                                indexs.Add(qi);
                            }
                        }
                    }
                }
                D_Lengths = _Lengths.Select(i => i).ToList();
                flag = 0;
            }
            DA.SetDataList(0, result);
            DA.SetDataList(1, indexs);
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
                return Properties.Resources.Extract_Surface_from_length;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c3ef84c9-3c66-4cdd-b2a7-53d759a47e63"); }
        }
    }
}