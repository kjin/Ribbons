using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Content;

namespace Ribbons.Layout
{
    /// <summary>
    /// The base class for anything that can be built from a layout tree.
    /// </summary>
    public abstract class LayoutBase
    {
        /// <summary>
        /// The name of the original layout element associated with this object.
        /// </summary>
        public String LayoutName { get; set; }

        /// <summary>
        /// Applies a node of a layout tree to this object.
        /// </summary>
        /// <param name="assets">The AssetManager associated with the game; use null if unused by the tree.</param>
        /// <param name="node">The node to apply.</param>
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
                Console.WriteLine("LayoutEngine WARNING: We were given {0} but expected {1}.", node.Value, typeName);
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

        /// <summary>
        /// An overridable method that is run immediately before a node's children are applied.
        /// </summary>
        /// <param name="node">The node to be applied to this object.</param>
        protected virtual void IntegrationPreprocess(LayoutTreeNode node) { }

        /// <summary>
        /// An overridable method that is run immediately after a node's children are applied.
        /// </summary>
        /// <param name="node">The node to be applied to this object.</param>
        protected virtual void IntegrationPostprocess(LayoutTreeNode node) { }

        /// <summary>
        /// A method that is called once for every child of the node applied to this object.
        /// </summary>
        /// <param name="node">A single child of the node applied to this object.</param>
        protected abstract bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode);
    }
}
