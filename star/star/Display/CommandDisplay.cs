using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Rhino.UI;

namespace star.Display
{
    public class CommandDisplay : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CommandDisplay class.
        /// </summary>
        public CommandDisplay()
          : base("CommandDisplay", "CommandDisplay",
              "在屏幕上显示命令行",
              "star", "Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddColourParameter("Color", "C", "颜色", GH_ParamAccess.item, Color.Black);
            pManager.AddIntegerParameter("行数", "行数", "行数", GH_ParamAccess.item, 5);
            pManager.AddIntegerParameter("Size", "Size", "大小", GH_ParamAccess.item, 13);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        int CCON = 0;
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            thisRecount++;
            Color cc = Color.FromArgb(0, 210, 255);
            int hang = 5;
            int height1 = 13;
            DA.GetData(0, ref cc);
            DA.GetData(1, ref hang);
            DA.GetData(2, ref height1);

            thiscolor = cc;
            height = height1;

            base.Hidden = false;

            string ss = Rhino.RhinoApp.CommandPrompt;

            Rhino.Commands.Command.EndCommand += Command_EndCommand;
            Rhino.Commands.Command.BeginCommand += Command_BeginCommand;
            if (Rhino.Commands.Command.InCommand())
            {
                Rhino.RhinoApp.KeyboardEvent += RhinoApp_KeyboardEvent;
            }
            var st = Rhino.RhinoApp.CommandHistoryWindowText.Split((Environment.NewLine.ToCharArray()));
            List<string> stArr = st.Where(s => !string.IsNullOrEmpty(s)).ToList();
            if (stArr[stArr.Count - 1] != ss)
            {
                stArr.Add(ss);
            }
            if (stArr.Count > hang)
            {
                stArr.RemoveRange(0, stArr.Count - hang);
            }

            if (ss == "指令" || ss == "Command")
            {
                stArr.Clear();
                CCON++;
                if (CCON <= 3)
                {
                    ss = Rhino.RhinoApp.CommandPrompt;
                    st = Rhino.RhinoApp.CommandHistoryWindowText.Split((Environment.NewLine.ToCharArray()));
                    stArr = st.Where(s => !string.IsNullOrEmpty(s)).ToList();
                    if (stArr[stArr.Count - 1] != ss)
                    {
                        stArr.Add(ss);
                    }
                    if (stArr.Count > hang)
                    {
                        stArr.RemoveRange(0, stArr.Count - hang);
                    }
                }
                else
                {
                    CCON = 0;
                    return;
                }
            }

            disStr = String.Join(Environment.NewLine, stArr);
        }


        private void RhinoApp_KeyboardEvent(int key)
        {
            Rhino.RhinoApp.KeyboardEvent -= RhinoApp_KeyboardEvent;
            ExpireSolution(true);
        }

        int thisRecount = 0;

        public string disStr = string.Empty;
        public Color thiscolor = new Color();
        public int height = 0;
        BoundingBox bb = new BoundingBox(Point3d.Origin, new Point3d(10, 10, 10));

        public override bool IsPreviewCapable
        {
            get
            {
                return true;
            }
        }
        private void Command_EndCommand(object sender, Rhino.Commands.CommandEventArgs e)
        {
            Rhino.Commands.Command.EndCommand -= Command_EndCommand;

            ExpireSolution(true);
        }

        private void Command_BeginCommand(object sender, Rhino.Commands.CommandEventArgs e)
        {
            Rhino.Commands.Command.BeginCommand -= Command_BeginCommand;

            ExpireSolution(true);
        }


        //Draw all meshes in this method.
        public override void DrawViewportMeshes(IGH_PreviewArgs args)
        {
            args.Display.Draw2dText(disStr, thiscolor, new Point2d(50, 50), false, height);
        }
        //Draw all wires and points in this method.


        //Return a BoundingBox that contains all the geometry you are about to draw.
        public override BoundingBox ClippingBox
        {
            get
            {
                return bb;
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
                return Properties.Resources.CommandDisplay;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2f60604b-4706-469f-bf86-abcd9636b233"); }
        }
    }

    class MouseMove : MouseCallback
    {
        public void Mou()
        {
            Enabled = true;
        }

        private static MouseEventHandler MouseEventHandler;

        public static event MouseEventHandler MousemoveU
        {
            add
            {
                MouseEventHandler += value;
            }
            remove
            {
                MouseEventHandler -= value;
            }
        }
    }
}