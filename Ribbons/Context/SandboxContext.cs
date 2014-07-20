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
            transform = new UITransform(new Vector2(1280, 720));
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
            sprite.Anchor = Anchor.TopLeft;
            Canvas.DrawSprite(sprite);
            Canvas.PopTransform();
            Canvas.EndDraw();
        }
    }
}
