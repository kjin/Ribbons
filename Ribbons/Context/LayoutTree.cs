using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ribbons.Context
{
    public class LayoutTree
    {
        public LayoutTreeNode Root;
        public String RepresentativeString { get { if (!stringMade) MakeString(); return representativeString; } }
        String representativeString;
        bool stringMade = false;

        private void MakeString()
        {
            StringBuilder sb = new StringBuilder();
            Root.ToString(sb, "");
            representativeString = sb.ToString();
            stringMade = true;
        }
    }
}
