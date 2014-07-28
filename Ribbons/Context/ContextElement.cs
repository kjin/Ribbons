using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Graphics;
using Ribbons.Content;
using Ribbons.Utils;
using Ribbons.Input;
using Microsoft.Xna.Framework;

namespace Ribbons.Context
{
    public abstract class ContextElement : LayoutBase
    {
        public virtual void Update(InputController inputController) { }

        public virtual void Draw(Canvas canvas) { }
    }

    public class CoordinateTransformContextElement : ContextElement
    {
        CoordinateTransform transform;

        public override void Draw(Canvas canvas)
        {
            if (transform != null)
                canvas.PushTransform(transform);
        }

        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode)
        {
            switch (childNode.Key)
            {
                case "Transform":
                    switch (childNode.Value)
                    {
                        case "UITransform":
                            transform = new UITransform();
                            return true;
                    }
                    break;
            }
            return false;
        }
    }

    public class SpriteContextElement : ContextElement
    {
        Sprite sprite;

        public override void Update(InputController inputController)
        {
            sprite.Update();
        }

        public override void Draw(Canvas canvas)
        {
            canvas.DrawSprite(sprite);
        }

        #region LayoutBase
        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode)
        {
            bool orderError = false;
            switch (childNode.Key)
            {
                case "Texture":
                    sprite = new Sprite(assets.GetAnimatedTexture(childNode.Value));
                    return true;
                case "Position":
                    if (sprite != null)
                    {
                        sprite.Position = ExtendedConvert.ToVector2(childNode.Value);
                        return true;
                    }
                    else
                        orderError = true;
                    break;
                case "Rotation":
                    if (sprite != null)
                    {
                        sprite.Rotation = Convert.ToSingle(childNode.Value);
                        return true;
                    }
                    else
                        orderError = true;
                    break;
                case "Scale":
                    if (sprite != null)
                    {
                        sprite.Scale = ExtendedConvert.ToVector2(childNode.Value);
                        return true;
                    }
                    else
                        orderError = true;
                    break;
                case "Color":
                    if (sprite != null)
                    {
                        sprite.Color = ExtendedConvert.ToColor(childNode.Value);
                        return true;
                    }
                    else
                        orderError = true;
                    break;
                case "Anchor":
                    if (sprite != null)
                    {
                        sprite.Anchor = ExtendedConvert.ToEnum<Anchor>(childNode.Value);
                        return true;
                    }
                    else
                        orderError = true;
                    break;
            }
            if (orderError)
            {
#if DEBUG
                Console.WriteLine("ContextElement WARNING: Tried to set {0}'s {1} field before the Texture was specified.", LayoutName, childNode.Key);
#endif
                return true;
            }
            return false;
        }
        #endregion
    }
}
