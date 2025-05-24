using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Grasshopper;
using Rhino.Geometry.Intersect;

namespace star.starMesh
{
    public class MeshFaceAngle : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshFaceAngle class.
        /// </summary>
        public MeshFaceAngle()
          : base("MeshFaceAngle", "MeshFaceAngle",
              "计算每片MeshFace的内角",
              "star", "Mesh")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "接入网格面", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Angle", "A", "网格面角度", GH_ParamAccess.tree);
            pManager.AddPointParameter("Points", "P", "网格角度对应点", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<GH_Mesh> dataTree = new GH_Structure<GH_Mesh>();
            DA.GetDataTree(0, out dataTree);
            for (int y = 0; y < dataTree.PathCount; y++)
            {
                List<GH_Mesh> listTree = new List<GH_Mesh>();
                listTree = dataTree[y];
                for (int z = 0; z < listTree.Count; z++)
                {
                    Mesh mesh = listTree[z].Value;
                    //GH_Convert.ToMesh(listTree[z], ref mesh, GH_Conversion.Both);
                    Polyline[] polyline = mesh.GetNakedEdges();
                    Curve[] explode = polyline[0].ToNurbsCurve().DuplicateSegments();
                    List<double> angle = new List<double>();
                    DataTree<double> numTree = new DataTree<double>();
                    GH_Path gp = new GH_Path();
                    /*-----------------------------------------------*/
                    starMathdy starMathdy = new starMathdy();
                    for (int x = 0; x < explode.Length; x++)
                    {
                        gp = new GH_Path(y, z);
                        Line linea = new Line();
                        Line lineb = new Line();
                        if (x == explode.Length - 1)
                        {
                            GH_Convert.ToLine(explode[x], ref linea, GH_Conversion.Both);
                            GH_Convert.ToLine(explode[0], ref lineb, GH_Conversion.Both);
                        }
                        else
                        {
                            GH_Convert.ToLine(explode[x], ref linea, GH_Conversion.Both);
                            GH_Convert.ToLine(explode[x + 1], ref lineb, GH_Conversion.Both);
                        }
                        DataTree<Point3d> point3ds = new DataTree<Point3d>();
                        double d1 = 0;
                        double d2 = 0;
                        Point3d center = new Point3d();
                        if (Intersection.LineLine(linea, lineb, out d1, out d2))
                        {
                            center = linea.PointAt(d1);
                        }
                        angle.Add(Vector3d.VectorAngle(linea.Direction, lineb.Direction));
                        DataTree<double> data = new DataTree<double>();

                        data.Add(starMathdy.Dreeges(Math.PI - angle[x]), gp);
                        point3ds.Add(center, gp);
                        DA.SetDataTree(0, data);
                        DA.SetDataTree(1, point3ds);
                    }
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
                return Properties.Resources.MeshFaceAngle;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("f4c0ead0-3032-4324-b926-077fe7caab59"); }
        }
    }
}