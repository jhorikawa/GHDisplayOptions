using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace GHDisplayOptions
{
    public class GHDisplayOptionsInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "GHDisplayOptions";
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
                return new Guid("1152440F-7F75-43D7-BE4E-AA475EB0171A");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Junichiro Horikawa";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "jh@orangejellies.com";
            }
        }
    }
}
