using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace star.M1
{
    public class TextSplit : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the TextSplit class.
        /// </summary>
        public TextSplit()
          : base("TextSplit", "TextSplit",
              "文字分隔（左右）",
              "star", "Display")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Text", "T", "需分隔的文字", GH_ParamAccess.item, "star");
            pManager.AddIntegerParameter("Count", "C", "分隔数量", GH_ParamAccess.item, 2);
            pManager.AddBooleanParameter("Switch(左)", "S(左)", "从左（右）开始对字符串分隔", GH_ParamAccess.item, true);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Left Text", "L T", "文字（左）", GH_ParamAccess.item);
            pManager.AddTextParameter("Right Text", "R T", "文字（右）", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string s = string.Empty;
            int TextCount = 0;
            bool Sw = true;

            if (DA.GetData(0, ref s))
            {
                if (DA.GetData(1, ref TextCount))
                {
                    if (DA.GetData(2, ref Sw))
                    {
                        if (Sw)
                        {
                            this.Params.Input[2].Name = "Switch(左)";
                            this.Params.Input[2].NickName = "S(左)";
                            this.Message = "从左开始分隔";
                        }
                        else
                        {
                            this.Params.Input[2].Name = "Switch(右)";
                            this.Params.Input[2].NickName = "S(右)";
                            this.Message = "从右开始分隔";
                        }
                    }
                }
            }

            TextSplits(s, Math.Abs(TextCount), Sw);
            DA.SetData(0, Result1);
            DA.SetData(1, Result2);
        }

        public static string Result1 { get; set; }
        public static string Result2 { get; set; }

        public static void TextSplits(string text, int Count, bool flag)
        {
            string result = text;

            if (flag)
            {
                if (text.Length < Count)
                {
                    Result1 = text;
                    Result2 = string.Empty;
                }
                else
                {
                    Result1 = result.Remove(Count, text.Length - Count);
                    Result2 = result.Remove(0, Count);
                }
            }
            else
            {
                if (text.Length < Count)
                {
                    Result2 = string.Empty;
                    Result1 = text;
                }
                else
                {
                    Result2 = result.Remove(0, text.Length - Count);
                    Result1 = result.Remove(text.Length - Count, Count);
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
                return Properties.Resources.TextSplit;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6933faa5-e0d7-4016-bf9c-a469ebd78a80"); }
        }
    }
}