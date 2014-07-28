using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Ribbons.Context;
using Ribbons.Utils;

namespace Ribbons.Content.Level
{
    public class RibbonStorage : LayoutBase
    {
        public List<Vector2> Path;
        public float Start;
        public float End;
        public bool Loop;
        public bool Complete;

        protected override void IntegrationPreprocess(LayoutTreeNode node)
        {
            Path = new List<Vector2>();
        }

        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode)
        {
            switch (childNode.Key)
            {
                case "Path":
                    Path.Add(ExtendedConvert.ToVector2(childNode.Value));
#if DEBUG
                    if (Convert.ToInt32(childNode.KeyExtension) != Path.Count)
                        Console.WriteLine("LevelBuilder WARNING: Ribbon path points are not in order.");
#endif
                    return true;
                case "Start":
                    Start = Convert.ToSingle(childNode.Value);
                    return true;
                case "End":
                    End = Convert.ToSingle(childNode.Value);
                    return true;
                case "Loop":
                    Loop = Convert.ToBoolean(childNode.Value);
                    return true;
                case "Complete":
                    Complete = Convert.ToBoolean(childNode.Value);
                    return true;
            }
            return false;
        }
    }
}
