using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ribbons.Context
{
    static class ContextHelper
    {
        public static ContextElement ContextFromName(string name)
        {
            switch (name)
            {
                case "Sprite":
                    return new SpriteContextElement();
                case "Text":
                    return new TextContextElement();
                case "CoordinateTransform":
                    return new CoordinateTransformContextElement();
                case "Gameplay":
                    return new TestGameplayContext();
                default:
                    return null;
            }
        }
    }
}
