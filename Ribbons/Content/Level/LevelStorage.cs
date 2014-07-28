using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Context;
using Ribbons.Utils;

namespace Ribbons.Content.Level
{
    public class LevelStorage : LayoutBase
    {
        List<RibbonStorage> ribbons;
        int worldNumber;
        int levelNumber;

        public LevelStorage(Text text)
        {
            LayoutTree layoutTree = LayoutEngine.BuildLayout(text);
            Integrate(null, layoutTree.Root);
        }

        protected override void IntegrationPreprocess(LayoutTreeNode node)
        {
            ribbons = new List<RibbonStorage>();
        }

        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode)
        {
            switch (childNode.Key)
            {
                case "Ribbons":
                    RibbonStorage ribbon = new RibbonStorage();
                    ribbon.Integrate(null, childNode);
                    ribbons.Add(ribbon);
                    return true;
                case "World":
                    worldNumber = Convert.ToInt32(childNode.Value);
                    return true;
                case "Level":
                    levelNumber = Convert.ToInt32(childNode.Value);
                    return true;
            }
            return false;
        }

#if DEBUG
        protected override void IntegrationPostprocess(LayoutTreeNode node)
        {
            if (worldNumber == 0)
                Console.WriteLine("LevelBuilder WARNING: A level was built in World 0.");
            if (levelNumber == 0)
                Console.WriteLine("LevelBuilder WARNING: A 0th level was built.");
        }
#endif
    }
}
