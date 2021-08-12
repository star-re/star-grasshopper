using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Drawing;
using System.Windows.Forms;
using Grasshopper.Kernel.Types;

namespace star.Display
{
    public class blingbling : GH_Component
    {
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
            pManager.HideParameter(0);
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
            List<GH_Point> p = new List<GH_Point>();
            DA.GetDataList<GH_Point>(0, p);
            

            px.AddRange(p);
        }

        List<GH_Point> px=new List<GH_Point>();
        int a1;
        bool a = false;
        bool b = false;
        bool c = false;

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
            Point3d p3 = new Point3d();
            foreach (GH_Point item in px)
            {
                GH_Convert.ToPoint3d(item, ref p3, GH_Conversion.Primary);
                args.Display.DrawPoint(p3, result, 6, Color.FromArgb(0, 210, 255));
            }

        }



        // If you implement IGH_Param, then override this method. If you implement IGH_Component, 
        // then override AppendAdditionalComponentMenuItems instead.
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            // Place a call to the base class to ensure the default parameter menu
            // is still there and operational.
            base.AppendAdditionalMenuItems(menu);

            Menu_AppendItem(menu, "Circle", Menu_Circle, true,a);
            Menu_AppendItem(menu, "RoundControlPoint", Menu_RoundControlPoint, true,b);
            Menu_AppendItem(menu, "Pin", Menu_Pin, true, c);
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