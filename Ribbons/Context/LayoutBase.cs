using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Content;

namespace Ribbons.Context
{
    public abstract class LayoutBase
    {
        public String LayoutName { get; set; }

        public void Integrate(AssetManager assets, LayoutTreeNode node)
        {
            IntegrationPreprocess(node);
            if (node.KeyExtension == null)
                LayoutName = node.Key;
            else
                LayoutName = node.KeyExtension;
#if DEBUG
            //Verify that we're working with the right class
            String typeName = GetType().Name;
            if (typeName != node.Value)
                Console.WriteLine("LayoutEngine WARNING: We were given {0} but expected {1}.", typeName, node.Value);
#endif
            foreach (LayoutTreeNode childNode in node.Children)
            {
                bool success = IntegrateChild(assets, childNode);                    
#if DEBUG
                if (!success)
                    Console.WriteLine("LayoutEngine WARNING: {0} hasn't been implemented yet.", childNode.Key);
#endif
            }
            IntegrationPostprocess(node);
        }

        protected virtual void IntegrationPreprocess(LayoutTreeNode node) { }

        protected virtual void IntegrationPostprocess(LayoutTreeNode node) { }

        protected abstract bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode);
    }
}
