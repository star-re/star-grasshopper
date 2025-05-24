using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace star.starMesh
{
    public class MeshShareFace : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshShareFace class.
        /// </summary>
        public MeshShareFace()
          : base("MeshShareFace", "Nickname",
              "查找网格存在共用边的网格面索引",
              "star", "Mesh")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "网格", GH_ParamAccess.tree);
            //pManager.AddGenericParameter("Mesh", "M", "网格", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Mesh Index", "I", "存在共用边的网格面索引", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_Mesh> dataTree = new GH_Structure<GH_Mesh>();
            //_Structure<IGH_Goo> dataTree = new GH_Structure<IGH_Goo>();
            DA.GetDataTree(0, out dataTree);
            for (int y = 0; y < dataTree.PathCount; y++)
            {
                List<GH_Mesh> listTree = new List<GH_Mesh>();
                listTree = dataTree[y];
                for (int z = 0; z < listTree.Count; z++)
                {
                    Mesh mesh = listTree[z].Value;
                    //GH_Convert.ToMesh(listTree[z], ref mesh, GH_Conversion.Both);
                    int[] result = new int[0];
                    int meshcount = mesh.Faces.Count;
                    int edgecount = mesh.TopologyEdges.Count;
                    DataTree<int> numTree = new DataTree<int>();
                    for (int i = 0; i < meshcount; i++)
                    {
                        GH_Path gp = new GH_Path(y, z, i);
                        numTree.Insert(i, gp, 0);
                    }
                    for (int i = 0; i < edgecount; i++)
                    {
                        result = mesh.TopologyEdges.GetConnectedFaces(i);
                        if (result.Length == 2)
                        {
                            for (int k = 0; k < result.Length; k++)
                            {
                                GH_Path gp = new GH_Path(y, z, result[k]);
                                if (result[0] != result[k])
                                {
                                    numTree.Add(result[0], gp);
                                }
                                if (result[1] != result[k])
                                {
                                    numTree.Add(result[1], gp);
                                }
                            }
                        }
                    }
                    DA.SetDataTree(0, numTree);
                }
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
                return Properties.Resources.MeshShareFace;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("420d84f7-3355-43ae-b331-760d69e368e1"); }
        }
    }
}