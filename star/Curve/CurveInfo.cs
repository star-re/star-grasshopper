﻿using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Curve
{
    public class CurveInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Curve";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("8ef644c4-26c0-4394-833f-750362a36232");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
