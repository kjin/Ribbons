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
        GameplayTransform transform;

        public override void Initialize()
        {
            sprite = new Sprite(AssetManager.GetAnimatedTexture("PlayerSprites/playeridle"));
            transform = new GameplayTransform(Vector2.Zero, 1, 0.5f);
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
            Canvas.EndDraw();
        }
    }
}
