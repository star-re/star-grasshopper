using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Grasshopper.Kernel.Types.Transforms;

namespace star
{
    public class ArrangeGeometry : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ArrangeGeometry class.
        /// </summary>
        public ArrangeGeometry()
          : base("star_ArrangeGeometry", "star_ArrangeGeometry",
              "按列表摆放，点线面体网格均可",
              "star", "Transform")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("物体", "物体", "点线面体网格均可", GH_ParamAccess.tree);
            pManager.AddPlaneParameter("定位平面", "平面", "从当前平面到定位点平面", GH_ParamAccess.tree);
            pManager.AddPointParameter("定位点", "定位点", "定位点", GH_ParamAccess.item, Point3d.Origin);
            pManager.AddNumberParameter("间隔", "间隔", "摆放框间隔", GH_ParamAccess.item, 10);
            pManager.AddNumberParameter("外框偏移大小", "外框偏移", "外框偏移大小", GH_ParamAccess.item, 100);
            pManager.AddPointParameter("框内点位", "点位", "MDSliderPoint", GH_ParamAccess.item, new Point3d(0.5, 0.5, 0));
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("摆放物件", "物件", "摆放后的物件", GH_ParamAccess.tree);
            pManager.AddRectangleParameter("定位框", "定位框", "定位框", GH_ParamAccess.tree);
            pManager.AddPointParameter("定位框中点", "中点", "定位框中点", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Structure<IGH_GeometricGoo> _EditGeometry = new GH_Structure<IGH_GeometricGoo>();
            if (!DA.GetDataTree(0, out _EditGeometry))
            {
                return;
            }

            GH_Structure<GH_Plane> PlaneGeometry = new GH_Structure<GH_Plane>();
            DA.GetDataTree(1, out PlaneGeometry);

            GH_Structure<IGH_GeometricGoo> _Geometry = _EditGeometry.Duplicate();
            //DataTree<GeometryBase> _Geometry = new DataTree<GeometryBase>();
            Point3d _Base = Point3d.Origin;
            double _Gap = 10;
            double _Extend = 100;
            Point3d point3D = new Point3d(0.5, 0.5, 0);
            DA.GetData(2, ref _Base);
            DA.GetData(3, ref _Gap);
            DA.GetData(4, ref _Extend);
            DA.GetData(5, ref point3D);

            int branCount = _Geometry.Branches.Count;
            List<int> ListbranCount = new List<int>();
            for (int i = 0; i < branCount; i++)
            {
                ListbranCount.Add(_Geometry.Branches[i].Count);
            }

            List<Rectangle3d> listRectangle = new List<Rectangle3d>();
            double Wid, Hei;

            RecWH(_Geometry, out Wid, out Hei);
            //List<Point3d> basePoint = Arrange(new Plane(_Base, Vector3d.ZAxis), Wid + _Extend + _Gap, Hei + _Extend + _Gap, _Gap, ListbranCount, out listRectangle);
            List<Point3d> basePoint = Arrange(new Plane(_Base, Vector3d.ZAxis), Wid + _Extend + _Gap, Hei + _Extend + _Gap, _Gap, point3D, ListbranCount, out listRectangle);


            DataTree<Point3d> treeBasePoint = new DataTree<Point3d>();
            DataTree<Rectangle3d> treeRectangle = new DataTree<Rectangle3d>();
            GH_Structure<IGH_GeometricGoo> dataTreeGeo = TransformGeo(_Geometry, PlaneGeometry, basePoint);
            basePoint = Arrange(new Plane(_Base, Vector3d.ZAxis), Wid + _Extend + _Gap, Hei + _Extend + _Gap, _Gap, ListbranCount, out listRectangle);
            for (int i = 0; i < dataTreeGeo.PathCount; i++)
            {
                int Count = dataTreeGeo.get_Branch(i).Count;
                List<Point3d> addP = basePoint.GetRange(0, Count);
                List<Rectangle3d> addR = listRectangle.GetRange(0, Count);
                treeBasePoint.AddRange(addP, dataTreeGeo.get_Path(i));
                treeRectangle.AddRange(addR, dataTreeGeo.get_Path(i));
                basePoint.RemoveRange(0, Count);
                listRectangle.RemoveRange(0, Count);
            }
            DA.SetDataTree(0, dataTreeGeo);
            DA.SetDataTree(2, treeBasePoint);
            //A = _Geometry.Branch(0);
            DA.SetDataTree(1, treeRectangle);
        }


        public void RecWH(GH_Structure<IGH_GeometricGoo> Geo, out double Width, out double Height)
        {
            double Wid = 0;
            double Hei = 0;

            for (int i = 0; i < Geo.Branches.Count; i++)
            {
                for (int j = 0; j < Geo.Branches[i].Count; j++)
                {
                    BoundingBox bb = Geo.Branches[i][j].Boundingbox;
                    Rectangle3d rc = new Rectangle3d(Plane.WorldXY, bb.Max, bb.Min);
                    if (rc.Width > Wid)
                    {
                        Wid = rc.Width;
                    }
                    if (rc.Height > Hei)
                    {
                        Hei = rc.Height;
                    }
                }
            }
            Width = Wid;
            Height = Hei;
        }

        public void RecWH(DataTree<GeometryBase> Geo, out double Width, out double Height)
        {
            double Wid = 0;
            double Hei = 0;
            for (int i = 0; i < Geo.Branches.Count; i++)
            {
                for (int j = 0; j < Geo.Branch(i).Count; j++)
                {
                    BoundingBox bb = Geo.Branch(i)[j].GetBoundingBox(Plane.WorldXY);
                    Rectangle3d rc = new Rectangle3d(Plane.WorldXY, bb.Max, bb.Min);
                    if (rc.Width > Wid)
                    {
                        Wid = rc.Width;
                    }
                    if (rc.Height > Hei)
                    {
                        Hei = rc.Height;
                    }
                }
            }
            Width = Wid;
            Height = Hei;
        }

        public List<Point3d> Arrange(Plane plane, double Width, double Height, double Gap, Point3d pt3, List<int> Count, out List<Rectangle3d> listRec)
        {
            double x1 = pt3.X;
            double y1 = pt3.Y;
            Rectangle3d rt = new Rectangle3d(plane, Width - Gap, Height - Gap);
            List<Rectangle3d> zeng = new List<Rectangle3d>();
            List<Point3d> Planetree = new List<Point3d>();
            for (int i = 0; i < Count.Count; i++)
            {
                for (int j = 0; j < Count[i]; j++)
                {
                    Transform tf = Transform.Translation(new Vector3d(Width * (double)j, -Height * (double)i, (double)0));
                    rt.Transform(tf);
                    zeng.Add(rt);
                    Planetree.Add(rt.PointAt(x1, y1));
                    rt = new Rectangle3d(plane, Width - Gap, Height - Gap);
                }
            }
            listRec = zeng;
            return Planetree;
        }

        public List<Point3d> Arrange(Plane plane, double Width, double Height, double Gap, List<int> Count, out List<Rectangle3d> listRec)
        {
            Rectangle3d rt = new Rectangle3d(plane, Width - Gap, Height - Gap);
            List<Rectangle3d> zeng = new List<Rectangle3d>();

            List<Point3d> Planetree = new List<Point3d>();
            for (int i = 0; i < Count.Count; i++)
            {
                for (int j = 0; j < Count[i]; j++)
                {
                    Transform tf = Transform.Translation(new Vector3d(Width * (double)j, -Height * (double)i, (double)0));
                    rt.Transform(tf);
                    zeng.Add(rt);
                    Planetree.Add(rt.Center);
                    rt = new Rectangle3d(plane, Width - Gap, Height - Gap);
                }
            }
            listRec = zeng;
            return Planetree;
        }

        public GH_Structure<IGH_GeometricGoo> TransformGeo(GH_Structure<IGH_GeometricGoo> Geo, GH_Structure<GH_Plane> pla, List<Point3d> ori)
        {
            if (pla.IsEmpty)
            {
                return TransformGeo(Geo, ori);
            }
            GH_Structure<IGH_GeometricGoo> result = new GH_Structure<IGH_GeometricGoo>();
            int index = 0;
            Plane plane = Plane.WorldXY;

            for (int i = 0; i < Geo.Branches.Count; i++)
            {
                for (int j = 0; j < Geo.Branches[i].Count; j++)
                {
                    plane.Origin = ori[index];
                    //ITransform transform = new Orientation(pla[i][j].Value, plane);
                    //Geo.Branches[i][j].Transform(transform.ToMatrix());
                    //result.Append(Geo.Branches[i][j].Transform(transform.ToMatrix()), Geo.get_Path(i));
                    result.Append(Geo.Branches[i][j].Transform(Transform.PlaneToPlane(pla[i][j].Value, plane)), Geo.get_Path(i));
                    //result.Add(Geo.Branches[i][j] as GeometryBase, Geo.get_Path(i));
                    index++;
                }
            }
            return result;
        }

        public GH_Structure<IGH_GeometricGoo> TransformGeo(GH_Structure<IGH_GeometricGoo> Geo, List<Point3d> ori)
        {
            GH_Structure<IGH_GeometricGoo> result = new GH_Structure<IGH_GeometricGoo>();
            int index = 0;
            for (int i = 0; i < Geo.Branches.Count; i++)
            {
                for (int j = 0; j < Geo.Branches[i].Count; j++)
                {
                    Geo.Branches[i][j].Transform(Transform.Translation(ori[index] - Geo.Branches[i][j].Boundingbox.Center));
                    result.Append(Geo.Branches[i][j], Geo.get_Path(i));
                    //result.Add(Geo.Branches[i][j] as GeometryBase, Geo.get_Path(i));
                    index++;
                }
            }
            return result;
        }

        public DataTree<GeometryBase> TransformGeo(DataTree<GeometryBase> Geo, List<Point3d> ori)
        {
            DataTree<GeometryBase> result = new DataTree<GeometryBase>();
            int index = 0;
            for (int i = 0; i < Geo.Branches.Count; i++)
            {
                for (int j = 0; j < Geo.Branch(i).Count; j++)
                {
                    Geo.Branch(i)[j].Transform(Transform.Translation(ori[index] - Geo.Branch(i)[j].GetBoundingBox(Plane.WorldXY).Center));
                    result.Add(Geo.Branch(i)[j], Geo.Path(i));
                    index++;
                }
            }
            return result;
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
                return Properties.Resources.ArrangeGeometry;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c643d961-2071-474b-8f4b-d476ba9ce74a"); }
        }
    }
}