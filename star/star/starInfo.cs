using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace star
{
    public class starInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "star";
            }
        }

        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return Properties.Resources.star;
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
                return new Guid("b97f62d6-e007-43cb-acde-0cc1ab3cddd1");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "star";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "欢迎加入群聊568940144";
            }
        }
    }
}
