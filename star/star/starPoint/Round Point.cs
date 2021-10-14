using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Windows.Forms;

namespace star
{
    public class Round_Point : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Round_Point class.
        /// </summary>
        public Round_Point()
          : base("Round Point", "Round Point",
              "点取整",
              "star", "Point")
        {
        }


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pts", "取整点", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Round", "R", "点位数", GH_ParamAccess.item,0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Pts", "取整后点", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> oripoint = new List<Point3d>();
            int round = 0;
            DA.GetData(1, ref round);
            DA.GetDataList(0, oripoint);
            List<Point3d> roundpoints = roundpoint(oripoint,round);

            //List<double> pointx = new List<double>();
            //List<double> pointy = new List<double>();
            //List<double> pointz = new List<double>();
            //List<Point3d> roundpoints = new List<Point3d>();
            //int inta = 1;
            //bool aa = false;
            //bool ba = false;
            //bool ca = false;
            //a = aa;
            //b = ba;
            //c = ca;
            //a1 = inta;
            //if (inta == 1)
            //{
            //    for (int i = 0; i < oripoint.Count; i++)
            //    {
            //        pointx.Add(Math.Floor(oripoint[i].X));
            //        pointy.Add(Math.Floor(oripoint[i].Y));
            //        pointz.Add(Math.Floor(oripoint[i].Z));
            //    }
            //    a = true;
            //}
            //else { a = false; }
            //if (inta == 2)
            //{
            //    for (int i = 0; i < oripoint.Count; i++)
            //    {
            //        pointx.Add(Math.Ceiling(oripoint[i].X));
            //        pointy.Add(Math.Ceiling(oripoint[i].Y));
            //        pointz.Add(Math.Ceiling(oripoint[i].Z));
            //    }
            //    a = true;
            //}
            //else { a = false; }
            //if (inta == 3)
            //{
            //    for (int i = 0; i < oripoint.Count; i++)
            //    {
            //        pointx.Add(Math.Round(oripoint[i].X));
            //        pointy.Add(Math.Round(oripoint[i].Y));
            //        pointz.Add(Math.Round(oripoint[i].Z));
            //    }
            //    a = true;
            //}
            //else { a = false; }

            //for (int i = 0; i < oripoint.Count; i++)
            //{
            //    //for (int j = 0; j < oripoint.Count; j++)
            //    //{

            //    //}
            //    roundpoints.Add(new Point3d(pointx[i], pointy[i], pointz[i]));
            //}
            DA.SetDataList(0, roundpoints);

            message(a1);
        }

        public static int a1 = 1;
        public static bool a = false;
        public static bool b = false;
        public static bool c = false;

        public static List<Point3d> roundpoint(List<Point3d> oripoints,int round)
        {
            List<double> pointx = new List<double>();
            List<double> pointy = new List<double>();
            List<double> pointz = new List<double>();
            List<Point3d> roundpoint = new List<Point3d>();
            if (a1 == 1)
            {
                for (int i = 0; i < oripoints.Count; i++)
                {
                    pointx.Add(Math.Floor(oripoints[i].X));
                    pointy.Add(Math.Floor(oripoints[i].Y));
                    pointz.Add(Math.Floor(oripoints[i].Z));
                }
                
                a = true;
            }
            else { a = false; }
            if (a1 == 2)
            {
                for (int i = 0; i < oripoints.Count; i++)
                {
                    pointx.Add(Math.Ceiling(oripoints[i].X));
                    pointy.Add(Math.Ceiling(oripoints[i].Y));
                    pointz.Add(Math.Ceiling(oripoints[i].Z));
                }
                b = true;
            }
            else { b = false; }
            if (a1 == 3)
            {
                for (int i = 0; i < oripoints.Count; i++)
                {
                    pointx.Add(Math.Round(oripoints[i].X,round));
                    pointy.Add(Math.Round(oripoints[i].Y,round));
                    pointz.Add(Math.Round(oripoints[i].Z,round));
                }
                c = true;
            }
            else { c = false; }

            for (int i = 0; i < oripoints.Count; i++)
            {
                roundpoint.Add(new Point3d(pointx[i], pointy[i], pointz[i]));
            }
            return roundpoint;
        }




        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            // Place a call to the base class to ensure the default parameter menu
            // is still there and operational.
            base.AppendAdditionalMenuItems(menu);

            Menu_AppendItem(menu, "Floor", Menu_Circle, true, a);
            Menu_AppendItem(menu, "Ceiling", Menu_RoundControlPoint, true, b);
            Menu_AppendItem(menu, "Round", Menu_Pin, true, c);
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

        private void message(int mess)
        {
            if (mess ==1)
            {
                this.Message = "Floor";
            }
            else if (mess == 2)
            {
                this.Message = "Ceiling";
            }
            else if (mess == 3)
            {
                this.Message = "Round";
            }
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {

            get
            {
                GH_DocumentObject gd = this;
                gd.SetIconOverride(Properties.Resources.Round_Point);
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
            get { return new Guid("0f57bfc5-9143-4ccf-9b75-36bf89099d35"); }
        }
    }
}