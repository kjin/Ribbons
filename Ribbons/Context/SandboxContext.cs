using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Ribbons.Graphics;

namespace Ribbons.Context
{
    public class SandboxContext : GameContext
    {
        Sprite sprite;
        UITransform transform;

        public override void Initialize()
        {
            sprite = new Sprite(AssetManager.GetAnimatedTexture("wetfloorsign"));
            transform = new UITransform(GraphicsConstants.VIEWPORT_DIMENSIONS);
        }

        public override void Dispose()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime)
        {
            Canvas.BeginDraw();
            Canvas.PushTransform(transform);
            sprite.Position = Vector2.Zero;
            sprite.Anchor = Anchor.TopLeft;
            Canvas.DrawSprite(sprite);
            sprite.Position = Vector2.UnitX;
            sprite.Anchor = Anchor.TopRight;
            Canvas.DrawSprite(sprite);
            sprite.Position = Vector2.One;
            sprite.Anchor = Anchor.BottomRight;
            Canvas.DrawSprite(sprite);
            sprite.Position = Vector2.UnitY;
            sprite.Anchor = Anchor.BottomLeft;
            Canvas.DrawSprite(sprite);
            Canvas.PopTransform();
            Canvas.EndDraw();
        }
    }
}
