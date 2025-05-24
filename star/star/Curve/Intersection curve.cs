//using System;
//using System.Collections.Generic;

//using Grasshopper;
//using Grasshopper.Kernel;
//using Rhino.Geometry;
//using Rhino.Geometry.Intersect;
//using Grasshopper.Kernel.Data;
//using System.Collections;

//namespace star
//{
//    public class Intersection_curve : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the Intersection_curve class.
//        /// </summary>
//        public Intersection_curve()
//          : base("Intersection curve", "Intersection curve",
//              "Description",
//              "star", "Curve")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//            pManager.AddCurveParameter("Curves", "C", "输入线组", GH_ParamAccess.list);
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddCurveParameter("Curves", "C", "输入线组", GH_ParamAccess.tree);
//            //pManager.AddIntegerParameter("Curves", "C", "输入线组", GH_ParamAccess.list);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            List<Curve> x = new List<Curve>();
//            DA.GetDataList(0, x);
//            /*----------------------*/

//            DA.SetDataTree(0, CrvIntersection(x));
//        }

//        public static DataTree<Curve> CrvIntersection(List<Curve> curves)
//        {
//            DataTree<Curve> dt = new DataTree<Curve>();
//            CurveIntersections intersection;
//            List<int> baseindex = new List<int>();
//            List<Curve> tragetindex = new List<Curve>();
//            List<Hashtable> ht = new List<Hashtable>();
//            int flag = 0;
//            bool flag2;
//            for (int i = 0; i < curves.Count; i++)
//            {
//                flag = 0;
//                tragetindex.Clear();
//                baseindex.Add(i);
//                Hashtable hashtable = new Hashtable();
//                for (int j = 0; j < curves.Count; j++)
//                {
//                    if (j == i || hashtable.ContainsKey(j))
//                    {
//                        continue;
//                    }
//                    if (ht.Count != 0 && flag == 0)
//                    {
//                        for (int k = 0; k < ht.Count; k++)
//                        {
//                            flag2 = ht[k].ContainsKey(i);
//                            if (flag2)
//                            {
//                                hashtable.Add(k, k);
//                                tragetindex.Add(curves[k]);
//                            }
//                        }
//                        flag = 1;
//                        goto jump;
//                    }
//                    intersection = Intersection.CurveCurve(curves[i], curves[j], 0.1, 0.1);
//                    if (intersection.Count != 0)
//                    {
//                        hashtable.Add(j, j);
//                        tragetindex.Add(curves[j]);
//                    }
//                jump:;
//                }
//                ht.Add(hashtable);
//                dt.AddRange(tragetindex, new GH_Path(i));
//            }
//            return dt;
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
//            get { return new Guid("ea866500-4981-48e5-ad6d-a5254734bdf8"); }
//        }
//    }
//}