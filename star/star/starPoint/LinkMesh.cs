using Grasshopper.Kernel.Geometry;
using Grasshopper.Kernel.Geometry.Delaunay;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Plane = Rhino.Geometry.Plane;

namespace star
{
    public static class LinkMesh
    {
       /// <summary>
        /// 网格分解
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="pts"></param>
        /// <param name="faces"></param>
        /// <param name="colors"></param>
        /// <param name="normals"></param>
        public static void DeconstructMesh(Mesh mesh,out List<Point3d> pts,out List<MeshFace> faces,out List<Color> colors, out List<Vector3d> normals)
        {
            pts = new List<Point3d>(mesh.Vertices.Count);
            Point3d[] array = mesh.Vertices.ToPoint3dArray();
            for (int i = 0; i < array.Length; i++)
            {
                pts.Add(array[i]);
            }
            faces = new List<MeshFace>(mesh.Faces.Count);
            for (int j = 0; j < mesh.Faces.Count; j++)
            {
                faces.Add(mesh.Faces.GetFace(j));
            }
            colors = new List<Color>(mesh.VertexColors.Count);
            if (mesh.VertexColors.Count > 0)
            {
                for (int k = 0; k < mesh.VertexColors.Count; k++)
                {
                    colors.Add(mesh.VertexColors[k]);
                }
            }
            if (mesh.Normals.Count == 0)
            {
                mesh.Normals.ComputeNormals();
            }
            normals = new List<Vector3d>(mesh.Normals.Count);
            for (int l = 0; l < mesh.Normals.Count; l++)
            {
                normals.Add(mesh.Normals[l]);
            }
        }
        /// <summary>
        /// Delaunay 网格 通过点和平面,点必须要先去重复
        /// </summary>
        /// <param name="list"></param>
        /// <param name="unset"></param>
        /// <returns></returns>
        public static Mesh Delaunay(List<Point3d> list, Plane unset)
        {
            Mesh mesh = new Mesh();
            if (list.Count < 3)
            {
                return mesh;
            }
            Transform unset2 = Transform.Unset;
            Transform unset3 = Transform.Unset;
            Grasshopper.Kernel.Geometry.Node2List nodes = null;
            if (!TwoDUtil.MapPointsToPlane(list, ref unset, ref unset2, ref unset3, ref nodes))
            {
                return mesh;
            }
            //Type type = typeof(Grasshopper.Kernel.Geometry.Delaunay.Solver);
            //object reflectTest = Activator.CreateInstance(type);
            //Type[] tp = new Type[2];
            //tp[0] = typeof(Node2List);
            //tp[1] = typeof(double);
            //MethodInfo methodinfo = type.GetMethod("Solve_Faces", BindingFlags.Public | BindingFlags.Instance, null, tp, null);
            //object[] parameters = new object[2];
            //parameters[0] = nodes;
            //parameters[1] = 0.1 * LinkParams.tol;
            //methodinfo.Invoke(reflectTest, parameters);
            List<Face> list2 = Solver.Solve_Faces(nodes, 0.1 * 0.01);
            if (list2 == null || list2.Count == 0)
            {
                return mesh;
            }
            for (int i = 0; i < list.Count; i++)
            {
                mesh.Vertices.SetVertex(i, list[i]);
            }
            for (int j = 0; j < list2.Count; j++)
            {
                mesh.Faces.SetFace(j, list2[j].A, list2[j].B, list2[j].C);
            }
            mesh.Normals.ComputeNormals();
            return mesh;
        }
        public class TwoDUtil
        {
            public static string LastError
            {
                get
                {
                    return TwoDUtil.m_last_error;
                }
            }
            public static void ResetLastError()
            {
                TwoDUtil.m_last_error = null;
            }
            public static bool MapPointsToPlane(List<GH_Point> p_in, ref Rhino.Geometry.Plane plane, ref Transform w2p, ref Transform p2w, ref Grasshopper.Kernel.Geometry.Node2List p_out)
            {
                TwoDUtil.ResetLastError();
                if (!plane.IsValid)
                {
                    plane = GH_PointUtil.FitPlaneThroughPoints(p_in);
                    if (!plane.IsValid)
                    {
                        TwoDUtil.m_last_error = "Base plane could not be fitted automatically";
                        return false;
                    }
                }
                w2p = Transform.ChangeBasis(Rhino.Geometry.Plane.WorldXY, plane);
                p2w = Transform.ChangeBasis(plane, Rhino.Geometry.Plane.WorldXY);
                p_out = new Grasshopper.Kernel.Geometry.Node2List();
                checked
                {
                    int num = p_in.Count - 1;
                    for (int i = 0; i <= num; i++)
                    {
                        if (p_in[i] == null)
                        {
                            p_out.Append(null);
                        }
                        else
                        {
                            double nx;
                            double ny;
                            plane.ClosestParameter(p_in[i].Value, out nx, out ny);
                            p_out.Append(new Grasshopper.Kernel.Geometry.Node2(nx, ny));
                        }
                    }
                    p_out.RenumberNodes();
                    return true;
                }
            }
            public static bool MapPointsToPlane(List<Point3d> p_in, ref Rhino.Geometry.Plane plane, ref Transform w2p, ref Transform p2w, ref Grasshopper.Kernel.Geometry.Node2List p_out)
            {
                TwoDUtil.ResetLastError();
                if (!plane.IsValid)
                {
                    PlaneFitResult planeFitResult = Rhino.Geometry.Plane.FitPlaneToPoints(p_in, out plane);
                    if (planeFitResult == PlaneFitResult.Failure)
                    {
                        TwoDUtil.m_last_error = "Base plane could not be fitted automatically";
                        return false;
                    }
                }
                w2p = Transform.ChangeBasis(Rhino.Geometry.Plane.WorldXY, plane);
                p2w = Transform.ChangeBasis(plane, Rhino.Geometry.Plane.WorldXY);
                p_out = new Grasshopper.Kernel.Geometry.Node2List();
                checked
                {
                    int num = p_in.Count - 1;
                    for (int i = 0; i <= num; i++)
                    {
                        if (!p_in[i].IsValid)
                        {
                            p_out.Append(null);
                        }
                        else
                        {
                            double nx;
                            double ny;
                            plane.ClosestParameter(p_in[i], out nx, out ny);
                            p_out.Append(new Grasshopper.Kernel.Geometry.Node2(nx, ny));
                        }
                    }
                    p_out.RenumberNodes();
                    return true;
                }
            }
            public static bool MapPointsToPlane(List<GH_Point> p_in, ref Rhino.Geometry.Plane plane, ref Transform w2p, ref Transform p2w, ref Grasshopper.Kernel.Geometry.Node3List p_out)
            {
                TwoDUtil.ResetLastError();
                if (!plane.IsValid)
                {
                    plane = GH_PointUtil.FitPlaneThroughPoints(p_in);
                    if (!plane.IsValid)
                    {
                        TwoDUtil.m_last_error = "Base plane could not be fitted automatically";
                        return false;
                    }
                }
                w2p = Transform.ChangeBasis(Rhino.Geometry.Plane.WorldXY, plane);
                p2w = Transform.ChangeBasis(plane, Rhino.Geometry.Plane.WorldXY);
                p_out = new Grasshopper.Kernel.Geometry.Node3List();
                checked
                {
                    int num = p_in.Count - 1;
                    for (int i = 0; i <= num; i++)
                    {
                        if (p_in[i] == null)
                        {
                            p_out.Append(null);
                        }
                        else
                        {
                            double nX;
                            double nY;
                            plane.ClosestParameter(p_in[i].Value, out nX, out nY);
                            double nZ = plane.DistanceTo(p_in[i].Value);
                            p_out.Append(new Grasshopper.Kernel.Geometry.Node3(nX, nY, nZ, -1));
                        }
                    }
                    return true;
                }
            }
            private static string m_last_error;
        }
    }
}
