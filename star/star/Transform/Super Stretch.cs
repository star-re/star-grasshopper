using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.Geometry.Morphs;
using Grasshopper.Kernel.Types;
using GH_IO.Serialization;

namespace star
{
    public class Super_Stretch : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Super_Stretch class.
        /// </summary>
        public Super_Stretch()
          : base("Super_Stretch", "Nickname",
              "超级拉伸",
              "star", "Transform")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "Geo", "需变化的几何体", GH_ParamAccess.item);
            pManager.AddLineParameter("Lines", "Ls", "基准变化线群", GH_ParamAccess.list);
            pManager.AddNumberParameter("Lengths", "Lts", "变化长度们", GH_ParamAccess.list);
            pManager.AddNumberParameter("Gap", "G", "间距", GH_ParamAccess.item, 100);
            pManager.AddBooleanParameter("Rigid", "R", "硬不硬", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "Geo", "几何体变化结果", GH_ParamAccess.item);
        }

        public static List<double> GapList1(List<double> GL)
        {
            GapList.AddRange(GL);
            return GapList;
        }
        public override void ClearData()
        {
            base.ClearData();
            if (GapList != null)
            {
                GapList.Clear();
                run = -1;
            }
        }

        public static List<double> GapList = new List<double>();
        public double GeoGap = 100;
        public int run = -1;
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            run++;
            IGH_GeometricGoo igh_GeometricGoo = null;
            List<Line> unset = new List<Line>();
            List<double> num = new List<double>();
            bool rigid = false;

            DA.GetData(0, ref igh_GeometricGoo);
            DA.GetDataList(1, unset);
            DA.GetDataList(2, num);
            DA.GetData(4, ref rigid);
            DA.GetData(3, ref GeoGap);

            //if (!DA.GetData<IGH_GeometricGoo>(0, ref igh_GeometricGoo))
            //{
            //    return;
            //}
            //if (!DA.GetDataList<Line>(1, unset))
            //{
            //    return;
            //}
            //if (!DA.GetDataList<double>(2, num))
            //{
            //    return;
            //}
            //if (!DA.GetData<bool>(3, ref rigid))
            //{
            //    return;
            //}
            if (GapList.Count == 0)
            {
                GapList.Add(0);
            }
            GeoGap = num[0] + GapList[GapList.Count - 1] + GeoGap;
            GapList.Add(GeoGap);

            int unsetCou = unset.Count;
            int numCou = num.Count;
            if (unsetCou > numCou)
            {
                for (int i = 0; i < unsetCou; i++)
                {
                    num.Add(num[num.Count - 1]);
                }
                for (int i = 0; i < unsetCou; i++)
                {
                    StretchSpaceMorph morph = new StretchSpaceMorph(unset[i].From, unset[i].To, num[i])
                    {
                        PreserveStructure = false,
                        QuickPreview = false
                    };
                    igh_GeometricGoo = SolveInstanceHelper(morph, igh_GeometricGoo, rigid);
                }
            }
            else
            {
                for (int i = 0; i < numCou; i++)
                {
                    unset.Add(unset[unset.Count - 1]);
                }
                for (int i = 0; i < numCou; i++)
                {
                    StretchSpaceMorph morph = new StretchSpaceMorph(unset[i].From, unset[i].To, num[i])
                    {
                        PreserveStructure = false,
                        QuickPreview = false
                    };
                    igh_GeometricGoo = SolveInstanceHelper(morph, igh_GeometricGoo, rigid);
                }
            }
            //  Transform transform = new Transform();
            Vector3d move = new Vector3d(Vector3d.XAxis);
            move.Unitize();
            move = move * GapList[run];

            igh_GeometricGoo.Transform(Transform.Translation(move));
            DA.SetData(0, igh_GeometricGoo);
        }


        public IGH_GeometricGoo SolveInstanceHelper(SpaceMorph morph, IGH_GeometricGoo geometry, bool rigid)
        {
            IGH_GeometricGoo result = null;
            if (morph != null && geometry != null)
            {
                if (rigid)
                {
                    BoundingBox boundingBox = geometry.GetBoundingBox(new Transform(1.0));
                    Plane worldXY = Plane.WorldXY;
                    worldXY.Origin = boundingBox.Center;
                    Plane plane = worldXY;
                    if (!morph.Morph(ref plane))
                    {
                        plane = worldXY;
                    }
                    Transform xform = Transform.PlaneToPlane(worldXY, plane);
                    result = geometry.DuplicateGeometry().Transform(xform);
                }
                else
                {
                    result = geometry.DuplicateGeometry().Morph(morph);
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
                return Properties.Resources.Super_Stretch;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("9a918676-2416-40c8-8ae2-e90f3672c8c9"); }
        }
    }
}