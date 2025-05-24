using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class ConcaveHull : GH_Component
    {
        public ConcaveHull()
          : base("ConcaveHull", "ConcaveHull",
              "二维凹包",
              "star", "Point")
        {
            Message = this.Name;
            this.Hidden = true;
        }
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("点集", "点集", "点集,最好去重", GH_ParamAccess.list);
            pManager.AddPlaneParameter("平面", "平面", "凹包线所在平面.", GH_ParamAccess.item, Plane.WorldXY);
            pManager.AddNumberParameter("距离", "距离", "凹包的距离因素", GH_ParamAccess.item,100);
        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("凹包线", "凹包线", "二维凹包线", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> pts = new List<Point3d>();
            Plane pl = default(Plane);
            double dis = 100;
            if (!DA.GetDataList(0, pts)) return;
            DA.GetData(1, ref pl);
            DA.GetData(2, ref dis);


            Mesh mesh= LinkMesh.Delaunay(pts, pl);
            LinkMesh.DeconstructMesh(mesh, out List<Point3d> pts1, out List<MeshFace> faces, out List<Color> colors, out List<Vector3d> normals);
            List<Point2d> pt2ds = new List<Point2d>();
            foreach (var v in pts)
            {
                pt2ds.Add(new Point2d(v.X, v.Y));
            }
            List<Triangle> triangles = new List<Triangle>();
            for (int i = 0; i < faces.Count; i++)
            {
                triangles.Add(new Triangle(faces[i].A, faces[i].B, faces[i].C));
            }
            DelaunayMesh2d delaunayMesh2d = new DelaunayMesh2d();
            delaunayMesh2d.Points = pt2ds;
            delaunayMesh2d.Faces = triangles;
            delaunayMesh2d.InitEdgesInfo();
            delaunayMesh2d.ExecuteEdgeDecimation(dis);
            List<EdgeInfo> edgeInfos = delaunayMesh2d.GetBoundaryEdges();
            List<Line> lines = new List<Line>();
            foreach (var edgeInfo in edgeInfos)
            {
                lines.Add(new Line(pts[edgeInfo.P0Index], pts[edgeInfo.P1Index]));
            }
            DA.SetDataList(0, lines);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Properties.Resources.aobao;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("D8C93CD7-CFDF-7EB4-06DA-94B58ED94D7D"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        //public struct dVertex
        //{
        //    public long x;
        //    public long y;
        //    public long z;
        //}
        //public struct dTriangle
        //{
        //    public long vv0;
        //    public long vv1;
        //    public long vv2;
        //}
        //public struct OrTriangle
        //{
        //    public Point2d p0;
        //    public Point2d p1;
        //    public Point2d p2;
        //}
        public struct Triangle
        {
            public int P0Index;
            public int P1Index;
            public int P2Index;
            public int Index;
            public Triangle(int p0index, int p1index, int p2index)
            {
                this.P0Index = p0index;
                this.P1Index = p1index;
                this.P2Index = p2index;
                this.Index = -1;
            }
            public Triangle(int p0index, int p1index, int p2index, int index)
            {
                this.P0Index = p0index;
                this.P1Index = p1index;
                this.P2Index = p2index;
                this.Index = index;
            }
        }
        public struct EdgeInfo
        {
            public int P0Index;
            public int P1Index;
            public List<int> AdjTriangle;
            public bool Flag;
            public double Length;
            public int GetEdgeType()
            {
                return AdjTriangle.Count;
            }
            public bool IsValid()
            {
                return P0Index != -1;
            }
            public EdgeInfo(int d)
            {
                P0Index = -1;
                P1Index = -1;
                Flag = false;
                AdjTriangle = new List<int>();
                Length = -1;
            }
        }
        public class DelaunayMesh2d
        {
            public List<Point2d> Points;
            public List<Triangle> Faces;
            public EdgeInfo[,] Edges;
            public DelaunayMesh2d()
            {
                Points = new List<Point2d>();
                Faces = new List<Triangle>();
            }
            public int AddVertex(Point2d p)
            {
                Points.Add(p);
                return Points.Count - 1;
            }
            public int AddFace(Triangle t)
            {
                Faces.Add(t);
                return Faces.Count - 1;
            }
            public void InitEdgesInfo()
            {
                Edges = new EdgeInfo[Points.Count, Points.Count];
                for (int i = 0; i < Points.Count; i++)
                {
                    for (int j = 0; j < Points.Count; j++)
                    {
                        Edges[i, j] = new EdgeInfo(0);
                    }
                }
                for (int i = 0; i < Faces.Count; i++)
                {
                    Triangle t = Faces[i];
                    SetEdge(t, i);
                }

            }
            private void SetEdge(Triangle t, int i)
            {
                if (t.P0Index < t.P1Index)
                {
                    Edges[t.P0Index, t.P1Index].P0Index = t.P0Index;
                    Edges[t.P0Index, t.P1Index].P1Index = t.P1Index;
                    Edges[t.P0Index, t.P1Index].AdjTriangle.Add(i);
                    Edges[t.P0Index, t.P1Index].Length = Points[t.P0Index].DistanceTo( Points[t.P1Index]);
                }
                else
                {
                    Edges[t.P1Index, t.P0Index].P0Index = t.P1Index;
                    Edges[t.P1Index, t.P0Index].P1Index = t.P0Index;
                    Edges[t.P1Index, t.P0Index].AdjTriangle.Add(i);
                    Edges[t.P1Index, t.P0Index].Length = Points[t.P0Index].DistanceTo(Points[t.P1Index]);
                }

                if (t.P1Index < t.P2Index)
                {
                    Edges[t.P1Index, t.P2Index].P0Index = t.P1Index;
                    Edges[t.P1Index, t.P2Index].P1Index = t.P2Index;
                    Edges[t.P1Index, t.P2Index].AdjTriangle.Add(i);
                    Edges[t.P1Index, t.P2Index].Length =Points[t.P1Index].DistanceTo(Points[t.P2Index]);
                }
                else
                {
                    Edges[t.P2Index, t.P1Index].P0Index = t.P2Index;
                    Edges[t.P2Index, t.P1Index].P1Index = t.P1Index;
                    Edges[t.P2Index, t.P1Index].AdjTriangle.Add(i);
                    Edges[t.P2Index, t.P1Index].Length =Points[t.P1Index].DistanceTo(Points[t.P2Index]);
                }

                if (t.P0Index < t.P2Index)
                {
                    Edges[t.P0Index, t.P2Index].P0Index = t.P0Index;
                    Edges[t.P0Index, t.P2Index].P1Index = t.P2Index;
                    Edges[t.P0Index, t.P2Index].AdjTriangle.Add(i);
                    Edges[t.P0Index, t.P2Index].Length =Points[t.P0Index].DistanceTo(Points[t.P2Index]);
                }
                else
                {
                    Edges[t.P2Index, t.P0Index].P0Index = t.P2Index;
                    Edges[t.P2Index, t.P0Index].P1Index = t.P0Index;
                    Edges[t.P2Index, t.P0Index].AdjTriangle.Add(i);
                    Edges[t.P2Index, t.P0Index].Length =Points[t.P0Index].DistanceTo(Points[t.P2Index]);
                }
            }
            public void ExecuteEdgeDecimation(double length)
            {
                Queue<EdgeInfo> queue = new Queue<EdgeInfo>();
                for (int i = 0; i < Points.Count; i++)
                {
                    for (int j = 0; j < Points.Count; j++)
                    {
                        if (i < j && Edges[i, j].IsValid())
                        {
                            if (Edges[i, j].GetEdgeType() == 0)
                            {
                                throw new Exception();
                            }
                            if (Edges[i, j].Length > length && Edges[i, j].GetEdgeType() == 1)
                            {
                                queue.Enqueue(Edges[i, j]);
                            }
                        }
                    }
                }
                EdgeInfo[] opp1Temp = new EdgeInfo[2];
                while (queue.Count != 0)
                {
                    EdgeInfo info = queue.Dequeue();
                    if (info.AdjTriangle.Count != 1)
                        throw new Exception();
                    int tindex = info.AdjTriangle[0];
                    Triangle t = Faces[tindex];
                    InitOppEdge(opp1Temp, t, info);
                    SetInvalid(info.P0Index, info.P1Index);
                    for (int i = 0; i < 2; i++)
                    {
                        EdgeInfo e = opp1Temp[i];
                        e.AdjTriangle.Remove(tindex);
                        if (e.GetEdgeType() == 0)
                        {
                            SetInvalid(e.P0Index, e.P1Index);
                        }
                        else if (e.GetEdgeType() == 1 && e.Length > length)
                        {
                            queue.Enqueue(e);
                        }
                    }
                }
            }
            public List<EdgeInfo> GetBoundaryEdges()
            {
                List<EdgeInfo> list = new List<EdgeInfo>();
                for (int i = 0; i < Points.Count; i++)
                {
                    for (int j = 0; j < Points.Count; j++)
                    {
                        if (i < j)
                        {
                            if (Edges[i, j].GetEdgeType() == 1)
                            {
                                list.Add(Edges[i, j]);
                            }
                        }
                    }
                }
                return list;
            }
            private void SetInvalid(int i, int j)
            {
                Edges[i, j].AdjTriangle.Clear();
                Edges[i, j].Flag = true;
                Edges[i, j].P0Index = -1;
                Edges[i, j].P1Index = -1;
            }
            private void InitOppEdge(EdgeInfo[] opp1Temp, Triangle t, EdgeInfo info)
            {
                int vindex = t.P0Index + t.P1Index + t.P2Index - info.P0Index - info.P1Index;
                if (vindex < info.P0Index)
                {
                    opp1Temp[0] = Edges[vindex, info.P0Index];
                }
                else
                {
                    opp1Temp[0] = Edges[info.P0Index, vindex];
                }

                if (vindex < info.P1Index)
                {
                    opp1Temp[1] = Edges[vindex, info.P1Index];
                }
                else
                {
                    opp1Temp[1] = Edges[info.P1Index, vindex];
                }
            }
        }
    }
}