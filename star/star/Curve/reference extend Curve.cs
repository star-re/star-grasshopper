using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;
using System.Windows.Forms;

namespace star
{
    public class reference_extend_Curve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the reference_extend class.
        /// </summary>
        public reference_extend_Curve()
          : base("Reference Extend Curve", "Ref extend Crv",
              "根据给定参照点而延伸曲线",
              "star", "Curve")

        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "需延伸的曲线", GH_ParamAccess.item);
            pManager.AddPointParameter("Point", "P", "参照点", GH_ParamAccess.item);
            pManager.AddNumberParameter("Length", "L", "延伸距离", GH_ParamAccess.item,10);
            pManager.AddBooleanParameter("switch", "s", "正反延伸切换，true即延伸至参照点位置，false则反", GH_ParamAccess.item,true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "曲线结果", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve x = null;
            Point3d y = new Point3d();
            double z = double.NaN;
            bool u = true;
            if (DA.GetData(0, ref x))
            {
                if (DA.GetData(1, ref y))
                {
                    if (DA.GetData(2, ref z))
                    {
                        if (DA.GetData(3, ref u))
                        {

                        }
                        else { return; }
                    }
                    else { return; }
                }
                else { return; }
            }
            else { return; }
            
            
           
            /*-------------------------------------*/
            CurveReverse(ref x, y);

            Point3d curvePointA = x.PointAtStart;
            Point3d curvePointB = x.PointAtEnd;
            double valuea = y.DistanceTo(curvePointA);
            double valueb = y.DistanceTo(curvePointB);
            bool b = valuea > valueb;
            Curve x1 = null;
            if (!u)
            {
                b = !b;
            }
            if (b)
            {
                if (z > 0)
                {
                    x1= x.Extend(CurveEnd.End, z, CurveExtensionStyle.Smooth);
                }
                else
                {
                    x1= x.Trim(CurveEnd.End, -z);
                }
            }
            else
            {
                if (z > 0)
                {
                    x1= x.Extend(CurveEnd.Start, z, CurveExtensionStyle.Smooth);
                }
                else
                {
                    x1= x.Trim(CurveEnd.Start, -z);
                }
            }
            DA.SetData(0, x1);
        }

        public void CurveReverse(ref Curve crvRev,Point3d baseP)
        {
            Curve baseC = crvRev;
            double CaDistance = baseC.PointAtStart.DistanceTo(baseP);
            if (a1 == 1)
            {
                return;
            }
            if (a1 ==2 )
            {
                baseC = baseC.Extend(CurveEnd.Start, 0.001, CurveExtensionStyle.Smooth);
                if (baseC.PointAtStart.DistanceTo(baseP) < CaDistance)
                {
                    crvRev.Reverse();
                    return;
                }
            }
        }

        public int a1 = 2;
        public bool a = false;
        public bool b = false;

        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            // Place a call to the base class to ensure the default parameter menu
            // is still there and operational.
            base.AppendAdditionalMenuItems(menu);

            Bitmap b1 = new Bitmap(Properties.Resources.circle_circle);
            Bitmap b2 = new Bitmap(Properties.Resources.circle);

            Menu_AppendItem(menu, "Not", Menu_Not, b1, true, a);
            Menu_AppendItem(menu, "follow", Menu_follow, b2, true, b);

        }

        public void Menu_Not(Object sender, EventArgs e)
        {
            a1 = 1;
            this.ExpireSolution(true);
        }
        public void Menu_follow(Object sender, EventArgs e)
        {
            a1 = 2;
            this.ExpireSolution(true);
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
                return Properties.Resources.reference_extend;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("dd545b45-bf73-4cfd-bd0f-35e6c46c84fb"); }
        }
    }
}