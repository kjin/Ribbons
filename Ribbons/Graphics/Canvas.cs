using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ribbons.Graphics
{
    public class Canvas
    {
        SpriteBatch spriteBatch;
        //1x1 texture used in drawing lines and boxes
        Texture2D square1x1;

        public Canvas(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            // initialize our tiny square. This square is used for drawing primitives.
            square1x1 = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] data = new Color[1];
            data[0] = Color.White;
            square1x1.SetData<Color>(data);
        }

        public void BeginDraw(Camera camera)
        {
            spriteBatch.Begin(0, null, null, null, null, null, camera.World);
        }

        public void EndDraw()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// Draws a line on the screen.
        /// </summary>
        /// <param name="color">The color of the line.</param>
        /// <param name="thickness">The thickness of the line, in pixels.</param>
        /// <param name="p1">The starting point of the line.</param>
        /// <param name="p2">The ending point of the line.</param>
        public void DrawLine(Color color, float thickness, Vector2 p1, Vector2 p2)
        {
            Vector2 diff = p2 - p1;
            float angle = (float)Math.Atan2(diff.Y, diff.X);
            float length = diff.Length();
            spriteBatch.Draw(square1x1, p1, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0);
        }
    }

    public class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Camera()
        {
            Scale = Vector3.One;
        }

        /// <summary>
        /// The camera's world matrix. Watch out for stale values.
        /// </summary>
        public Matrix World { get; private set; }

        public void Update()
        {
            World = Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);
        }
    }
}
