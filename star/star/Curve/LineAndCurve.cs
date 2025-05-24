//using System;
//using System.Collections.Generic;

//using Grasshopper.Kernel;
//using Rhino.Geometry;
//using System.Linq;

//namespace star
//{
//    public class LineAndCurve : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the LineAndCurve class.
//        /// </summary>
//        public LineAndCurve()
//          : base("LineAndCurve", "Nickname",
//              "Description",
//              "star", "Curve")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddCurveParameter("Curve", "Curve", "Curve", GH_ParamAccess.list);
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddCurveParameter("Curve", "Curve", "Curve", GH_ParamAccess.list);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            List<Curve> cc = new List<Curve>();
//            DA.GetDataList(0, cc);



//            List<Curve> result = new List<Curve>();
//            ReLineAndCurve(cc, ref result);
//            DA.SetDataList(0, result);
//        }



//        public void ReLineAndCurve(List<Curve> cc, ref List<Curve> curves)
//        {
//            List<Curve> lines = new List<Curve>();
//            List<Curve> Curves = new List<Curve>();
//            Curve IsLineAndCurve = null;
//            List<Curve> tiao = new List<Curve>();
//            List<List<Curve>> tiaoList = new List<List<Curve>>();
//            int tiaoStart = 0;
//            int Count = 1;
//            int remove = 1;
//            bool flag = true;
//            for (int i = 0; i < cc.Count; i++)
//            {
//                tiao = cc.GetRange(tiaoStart, Count);
//                IsLineAndCurve = Curve.JoinCurves(tiao)[0];
//                lines.Clear();
//                if (IsLineAndCurve.IsLinear())
//                {
//                    if (Curves.Any() && tiao.Count == 2)
//                    {
//                        if (!Curve.JoinCurves(cc.GetRange(tiaoStart, 2))[0].IsLinear() || tiaoStart != 0)
//                        {
//                            Count++;
//                            continue;
//                        }
//                    }
//                    if (remove == 1 && Curves.Count != 0)
//                    {
//                        Curves.RemoveAt(Curves.Count - 1);
//                    }
//                    remove = 1;
//                    if (tiaoStart + Count < cc.Count)
//                    {
//                        Count++;
//                    }
//                    lines.AddRange(cc.GetRange(tiaoStart, Count));
//                    IsLineAndCurve = Curve.JoinCurves(lines)[0];
//                    Curves.Add(IsLineAndCurve);
//                }
//                else
//                {
//                    if (Curves.Any() && tiao.Count == 2)
//                    {
//                        if (!Curve.JoinCurves(cc.GetRange(tiaoStart, 2))[0].IsLinear() || tiaoStart != 0)
//                        {
//                            Count++;
//                            continue;
//                        }
//                    }
//                    lines.Clear();
//                    tiaoStart = i;
//                    Count = 2;
//                    remove = 0;
//                }
//            }
//            curves = Curves;
//        }
//        /// <summary>
//        /// Provides an Icon for the component.
//        /// </summary>
//        protected override System.Drawing.Bitmap Icon
//        {
//            get
//            {
//                //You can add image files to your project resources and access them like this:
//                // return Resources.IconForThisComponent;
//                return null;
//            }
//        }

//        /// <summary>
//        /// Gets the unique ID for this component. Do not change this ID after release.
//        /// </summary>
//        public override Guid ComponentGuid
//        {
//            get { return new Guid("21eeb1af-edb4-4e45-aea7-572bb61fb60e"); }
//        }
//    }
//}