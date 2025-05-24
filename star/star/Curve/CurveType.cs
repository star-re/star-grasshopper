using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star
{
    public class CurveType : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CurveType class.
        /// </summary>
        public CurveType()
          : base("CurveType", "CurveType",
              "曲线分类",
              "star", "Curve")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "C", "想分类的曲线", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddLineParameter("Line", "L", "直线", GH_ParamAccess.item);
            pManager.AddCurveParameter("PolyLine", "PL", "多重直线", GH_ParamAccess.item);
            pManager.AddCircleParameter("Circle", "C", "圆圈", GH_ParamAccess.item);
            pManager.AddArcParameter("Arc", "A", "圆弧", GH_ParamAccess.item);
            pManager.AddCurveParameter("Ellipase", "E", "椭圆", GH_ParamAccess.item);
            pManager.AddCurveParameter("BézierCurve", "BC", "贝塞尔曲线", GH_ParamAccess.item);
            pManager.AddCurveParameter("NurbsCurve", "NC", "Nurbs曲线", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curveType = null;

            DA.GetData(0, ref curveType);
            if (curveType != null)
            {
                Curve Line = null;
                Curve PolyLine = null;
                Curve Circle = null;
                Curve Arc = null;
                Curve Ellipse = null;
                Curve BezierCurve = null;
                Curve NurbsCurve = null;
                int Span = curveType.SpanCount;
                if (curveType.IsLinear())
                {
                    Line = curveType;
                }
                else
                {
                    if (curveType.IsPolyline())
                    {
                        PolyLine = curveType;
                    }
                    else
                    {
                        if (curveType.IsCircle())
                        {
                            Circle = curveType;
                        }
                        else
                        {
                            if (curveType.IsArc())
                            {
                                Arc = curveType;
                            }
                            else
                            {
                                if (curveType.IsEllipse())
                                {
                                    Ellipse = curveType;
                                }
                                else
                                {
                                    if (Span == 1)
                                    {
                                        BezierCurve = curveType;
                                    }
                                    else
                                    {
                                        NurbsCurve = curveType;
                                    }
                                }
                            }
                        }
                    }
                }
                DA.SetData(0, Line);
                DA.SetData(1, PolyLine);
                DA.SetData(2, Circle);
                DA.SetData(3, Arc);
                DA.SetData(4, Ellipse);
                DA.SetData(5, BezierCurve);
                DA.SetData(6, NurbsCurve);
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
                return Properties.Resources.CurveType;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8e04558f-376d-4b4f-94fb-64501631b315"); }
        }
    }
}