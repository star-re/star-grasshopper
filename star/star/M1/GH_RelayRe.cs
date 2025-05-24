//using Grasshopper.Kernel;
//using Rhino.Geometry;
//using System;
//using System.Collections.Generic;
//using Grasshopper;
//using Grasshopper.Kernel.Data;
//using Grasshopper.Kernel.Types;
//using Grasshopper.GUI.Canvas;
//using Grasshopper.Kernel.Special;
//using System.Windows.Forms;

//namespace star.M1
//{
//    public class ceshi : GH_Component
//    {
//        /// <summary>
//        /// Initializes a new instance of the ceshi class.
//        /// </summary>
//        public ceshi()
//          : base("ceshi", "Nickname",
//              "Description",
//              "Category", "Subcategory")
//        {
//        }

//        /// <summary>
//        /// Registers all the input parameters for this component.
//        /// </summary>
//        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
//        {
//        }

//        /// <summary>
//        /// Registers all the output parameters for this component.
//        /// </summary>
//        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
//        {
//            pManager.AddIntegerParameter("a", "a", "a", GH_ParamAccess.item);
//        }

//        /// <summary>
//        /// This is the method that actually does the work.
//        /// </summary>
//        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>

//        protected override void SolveInstance(IGH_DataAccess DA)
//        {
//            GH_Document gH_Document = OnPingDocument();
//            Grasshopper.Instances.ActiveCanvas.CanvasPrePaintObjects -= ActiveCanvas_CanvasPostPaintObjects;
//            Grasshopper.Instances.ActiveCanvas.CanvasPostPaintObjects += ActiveCanvas_CanvasPostPaintObjects;
//            if (Counts % 2 == 0)
//            {
//                gH_Relay.ExpireSolution(true);
//            }
//            DA.SetData(0, Counts);
//        }


//        private void ActiveCanvas_CanvasPostPaintObjects(GH_Canvas sender)
//        {
//            //gg_document.ObjectsAdded += Gg_document_ObjectsAdded;
//            //gg_document.ObjectsAdded -= Gg_document_ObjectsAdded;
//            //Counts++;
//            // toolStripMenuItem = (ToolStripMenuItem)sender;
//            relayReName();
//        }

//        private void Gg_document_ObjectsAdded(object sender, GH_DocObjectEventArgs e)
//        {
//            Counts++;
//            // ExpireSolution(true);
//        }

//        public int Counts = 1;
//        public GH_Document gg_document;
//        public GH_Relay gH_Relay = new GH_Relay();
//        public Guid RalayGuid = new Guid();
//        //public ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
//        public void relayReName()
//        {
//            // IGH_Param target = (IGH_Param)toolStripMenuItem.Tag;
//            gg_document = this.OnPingDocument();
//            List<IGH_ActiveObject> gao = gg_document.ActiveObjects();

//            for (int i = 0; i < gao.Count; i++)
//            {
//                if (gao[i].Name == "Relay" || gao[i].Name == "Goo")
//                {
//                    IGH_ActiveObject gg = gao[i];
//                    gH_Relay = (GH_Relay)gao[i];
//                    gH_Relay.DisplayName = gH_Relay.TypeName;
//                    gH_Relay.Name = gH_Relay.TypeName;
//                    gH_Relay.NickName = gH_Relay.TypeName;
//                    gH_Relay.Attributes.ExpireLayout();
//                    gH_Relay.Attributes.PerformLayout();
//                    gH_Relay.ResolveDisplayName();
//                }
//            }
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
//            get
//            {
//                return new Guid("e19f4cf6-d854-49f9-a589-0035980c9673");
//            }
//        }
//    }
//}