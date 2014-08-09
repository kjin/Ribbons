using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Graphics;
using Ribbons.Content;
using Ribbons.Utils;
using Ribbons.Input;
using Ribbons.Storage;
using Ribbons.Layout;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ribbons.Context
{
    public abstract class ContextElement : LayoutBase
    {
        protected Context UnderlyingContext { get; private set; }
        protected Canvas Canvas { get; private set; }
        protected InputController InputController { get; private set; }
        protected AssetManager AssetManager { get; private set; }
        protected StorageManager StorageManager { get; private set; }

        public void SetComponents(Context context)
        {
            UnderlyingContext = context;
            Canvas = context.Canvas;
            InputController = context.InputController;
            AssetManager = context.AssetManager;
            StorageManager = context.StorageManager;
        }

        public virtual void Initialize() { }

        public virtual void Dispose() { }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }
    }

    public class CoordinateTransformContextElement : ContextElement
    {
        CoordinateTransform transform;
        List<ContextElement> elements;

        public CoordinateTransformContextElement()
        {
            elements = new List<ContextElement>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (ContextElement e in elements)
                e.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (transform != null)
                Canvas.PushTransform(transform);
            foreach (ContextElement e in elements)
                e.Draw(gameTime);
            if (transform != null)
                Canvas.PopTransform();
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
                case "Elements":
                    ContextElement contextElement = ContextHelper.ContextFromName(childNode.Value);
                    if (contextElement != null)
                    {
                        contextElement.SetComponents(UnderlyingContext);
                        contextElement.Integrate(assets, childNode);
                        elements.Add(contextElement);
                        return true;
                    }
                    break;
            }
            return false;
        }
    }

    public class TextContextElement : ContextElement
    {
        TextSprite sprite;
        SpriteFont font = null;
        string text = null;

        public override void Draw(GameTime gametime)
        {
            Canvas.DrawTextSprite(sprite);
        }

        #region LayoutBase
        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode)
        {
            bool orderError = false;
            switch (childNode.Key)
            {
                case "Font":
                    font = assets.GetFont(childNode.Value);
                    if (text != null)
                        sprite = new TextSprite(font, text);
                    return true;
                case "Text":
                    text = childNode.Value;
                    if (font != null)
                        sprite = new TextSprite(font, text);
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
                Console.WriteLine("ContextElement WARNING: Tried to set {0}'s {1} field before the Text and Font were specified.", LayoutName, childNode.Key);
#endif
                return true;
            }
            return false;
        }
        #endregion
    }

    public class SpriteContextElement : ContextElement
    {
        Sprite sprite;

        public override void Update(GameTime gameTime)
        {
            sprite.Update();
        }

        public override void Draw(GameTime gametime)
        {
            Canvas.DrawSprite(sprite);
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
                case "Animation":
                    if (sprite != null)
                    {
                        AnimationCurveElement animationCurve = null;
                        switch (childNode.Value)
                        {
                            case "PeriodicCurve":
                                animationCurve = new PeriodicAnimationCurveElement();
                                break;
                        }
                        if (animationCurve != null)
                        {
                            animationCurve.Integrate(assets, childNode);
                            if (animationCurve.AnimationCurve != null)
                            {
                                sprite.PushAnimationCurve(animationCurve.AnimationCurve);
                                return true;
                            }
#if DEBUG
                            Console.WriteLine("LayoutEngine WARNING: An animation is missing an element.");
                            return true;
#endif
                        }
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
