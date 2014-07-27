using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Ribbons.Graphics;

namespace Ribbons.Context
{
    public class SandboxContext : ContextBase
    {
        Sprite sprite;
        TextSprite text;
        GameplayTransform transform;
        UITransform ui;

        public override void Initialize()
        {
            sprite = new Sprite(AssetManager.GetAnimatedTexture("PlayerSprites/playeridle"));
            sprite.PushAnimationCurve(new SineAnimationCurve(SpriteComponents.PositionX, Sinusoid.BuildSine(1, 60)));
            text = new TextSprite(AssetManager.GetFont("default"), "Wet Floor Confidential - Do Not Distribute");
            text.Color = Color.Black;
            transform = new GameplayTransform(Vector2.Zero, 1, 0.5f);
            ui = new UITransform();
            LayoutEngine le = new LayoutEngine(AssetManager.GetText("mockup"));
        }

        public override void Dispose()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            sprite.Update();
            Vector2 cameraDelta = Vector2.Zero;
            if (InputController.CameraLeft.JustPressed)
                cameraDelta.X--;
            if (InputController.CameraRight.JustPressed)
                cameraDelta.X++;
            if (InputController.CameraDown.JustPressed)
                cameraDelta.Y--;
            if (InputController.CameraUp.JustPressed)
                cameraDelta.Y++;
            if (InputController.Zoom.Pressed)
                transform.Zoom = 2;
            else
                transform.Zoom = 1;
            transform.Center += cameraDelta;
            transform.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            Canvas.BeginDraw();
            Canvas.PushTransform(transform);
            Canvas.DrawSprite(sprite);
            Canvas.PopTransform();
            Canvas.PushTransform(ui);
            text.Position = Vector2.Zero;
            text.Anchor = Anchor.TopLeft;
            Canvas.DrawTextSprite(text);
            text.Position = Vector2.UnitX;
            text.Anchor = Anchor.TopRight;
            Canvas.DrawTextSprite(text);
            text.Position = Vector2.One;
            text.Anchor = Anchor.BottomRight;
            Canvas.DrawTextSprite(text);
            text.Position = Vector2.UnitY;
            text.Anchor = Anchor.BottomLeft;
            Canvas.DrawTextSprite(text);
            Canvas.PopTransform();
            Canvas.EndDraw();
        }
    }
}
