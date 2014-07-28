using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ribbons.Context
{
    public interface ILayout
    {
        void Integrate(LayoutTreeNode node);
    }
}
