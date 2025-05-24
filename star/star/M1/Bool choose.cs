using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Windows.Forms;

namespace star.M1
{
    public class Bool_choose : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Bool_choose class.
        /// </summary>
        public Bool_choose()
          : base("Bool choose", "Bool choose",
              "人生有很多岔路口，选择很重要\r\n对的事情要坚持下去\r\n而错的事情要勇于承认\r\n而对错参半时，就选择自己认为对的事吧！",
              "star", "Math")
        {
        }
        public List<bool> bb = new List<bool> { true, false ,true};
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Bools","B","布尔群",GH_ParamAccess.list,bb);
            Params.Input[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Bool","B","布尔值",GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (a1 == 1)
            {
                a = true;
            }
            else { a = false; }
            if (a1 == 2)
            {
                b = true;
            }
            else { b = false; }
            List<bool> x = new List<bool>();
            DA.GetDataList(0,x);

            bool result = true;
            int truecount = 0;
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i] == true)
                {
                    truecount++;
                }
            }
            int falsecount = x.Count - truecount;
            if (truecount == falsecount)
            {
                result = a;
            }
            else if (truecount > falsecount)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            DA.SetData(0,result);
        }


        public int a1 = 1;
        public bool a = false;
        public bool b = false;
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            // Place a call to the base class to ensure the default parameter menu
            // is still there and operational.
            base.AppendAdditionalMenuItems(menu);

            Menu_AppendItem(menu, "true", Menu_Circle, true, a);
            Menu_AppendItem(menu, "false", Menu_RoundControlPoint, true, b);
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
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.bool_choose;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("C1A569AA-5EE5-45A0-A384-2DB48EC2685C"); }
        }
    }
}