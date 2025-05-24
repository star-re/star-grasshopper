using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;
using System.Windows.Forms;

namespace star.Display
{
    public class blingbling : GH_Component
    {
        //protected blingbling(string name, string nickname, string description)
        //    : base()
        //{
        //}

        /// <summary>
        /// Initializes a new instance of the blingbling class.
        /// </summary>
        public blingbling()
          : base("blingbling", "blingbling",
              "blingbling",
              "star", "Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "P", "blingbling的点", GH_ParamAccess.list);
            pManager[0].Optional = true;
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
            List<Point3d> p = new List<Point3d>();
            DA.GetDataList(0, p);
            
            HiddenLineDrawingParameters hiddenLineDrawingParameters = new HiddenLineDrawingParameters
            {
                AbsoluteTolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance
            };
            px1(p);
        }

        private static List<Point3d> px = new List<Point3d>();

        public static List<Point3d> px1(List<Point3d> pp)
        {
            px.AddRange(pp);
            return px;
        }
        public int a1 = 2;
        public bool a = false;
        public bool b = false;
        public bool c = false;

        public override void ClearData()
        {
            base.ClearData();
            if (px != null)
            {
                px.Clear();
            }
        }

        //Draw all wires and points in this method.
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {

            var result = Rhino.Display.PointStyle.RoundControlPoint;

            if (a1 == 1)
            {
                
                result = Rhino.Display.PointStyle.Circle;
                a = true;
            }
            else { a = false; }
            if (a1 == 2)
            {
                result = Rhino.Display.PointStyle.RoundControlPoint;
                b = true;
            }
            else { b = false; }
            if (a1 == 3)
            {
                result = Rhino.Display.PointStyle.Pin;
                c = true;
            }
            else { c = false; }
            foreach (Point3d item in px)
            {
                args.Display.DrawPoint(item, result, 6, Color.FromArgb(0, 210, 255));
            }
        }



        // If you implement IGH_Param, then override this method. If you implement IGH_Component, 
        // then override AppendAdditionalComponentMenuItems instead.
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            // Place a call to the base class to ensure the default parameter menu
            // is still there and operational.
            base.AppendAdditionalMenuItems(menu);

            Bitmap b1 = new Bitmap(Properties.Resources.circle_circle);
            Bitmap b2 = new Bitmap(Properties.Resources.circle);
            Bitmap b3 = new Bitmap(Properties.Resources.pin);

            Menu_AppendItem(menu, "Circle", Menu_Circle,b1, true, a);
            Menu_AppendItem(menu, "RoundControlPoint", Menu_RoundControlPoint,b2, true, b);
            Menu_AppendItem(menu, "Pin", Menu_Pin,b3, true, c);
        }

        public void Menu_Circle(Object sender, EventArgs e)
        {
            a1 = 1;
            this.ExpireSolution(true);
        }
        public void Menu_RoundControlPoint(Object sender, EventArgs e)
        {
            a1 = 2;
            this.ExpireSolution(true);
        }
        public void Menu_Pin(Object sender, EventArgs e)
        {
            a1 = 3;
            this.ExpireSolution(true);
        }

        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
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
                return Properties.Resources.blingbling;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7b6baca8-f814-498e-b08c-95258da76ef3"); }
        }
    }
}