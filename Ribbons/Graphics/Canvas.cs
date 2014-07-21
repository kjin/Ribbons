using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ribbons.Graphics
{
    /// <summary>
    /// A class used for drawing sprites.
    /// </summary>
    public class Canvas
    {
        SpriteBatch spriteBatch;
        //1x1 texture used in drawing lines and boxes
        Texture2D square1x1;
        List<CoordinateTransform> transformationStack;
        bool displayDebugInformation;

        /// <summary>
        /// Constructs a new Canvas object.
        /// </summary>
        /// <param name="graphicsDevice">The GraphicsDevice object associated with the game.</param>
        /// <param name="spriteBatch">The SpriteBatch object used for drawing.</param>
        public Canvas(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;

            // initialize our tiny square. This square is used for drawing primitives.
            square1x1 = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Color[] data = new Color[1];
            data[0] = Color.White;
            square1x1.SetData<Color>(data);
            transformationStack = new List<CoordinateTransform>(4);
            displayDebugInformation = false;
        }

        /// <summary>
        /// Begins a SpriteBatch operation.
        /// </summary>
        public void BeginDraw()
        {
            spriteBatch.Begin();//0, null, null, null, null, null, camera.World);
        }

        /// <summary>
        /// Ends a SpriteBatch operation.
        /// </summary>
        public void EndDraw()
        {
            spriteBatch.End();
        }

        /// <summary>
        /// Pushes a coordinate transform onto the canvas's stack of transforms.
        /// </summary>
        /// <param name="transform">The coordinate transform to push.</param>
        public void PushTransform(CoordinateTransform transform)
        {
            transformationStack.Add(transform);
        }

        /// <summary>
        /// Pops the uppermost coordinate transform from the canvas's stack of transforms.
        /// </summary>
        public void PopTransform()
        {
            transformationStack.RemoveAt(transformationStack.Count - 1);
        }

        /// <summary>
        /// Draws a sprite to the screen, taking into account all of the coordinate
        /// transforms on the canvas's stack.
        /// </summary>
        /// <param name="sprite">The sprite to draw.</param>
        public void DrawSprite(Sprite sprite)
        {
            Vector2 position = sprite.Position;
            float rotation = sprite.Rotation;
            Vector2 scale = sprite.Scale * sprite.Texture.Scale;
            for (int i = 0; i < transformationStack.Count; i++)
                transformationStack[i].Transform(ref position, ref rotation, ref scale);
            Rectangle sourceRectangle = sprite.Texture.GetFrame(sprite.Frame);
            Vector2 origin = AnchorHelper.ComputeAnchorOrigin(sprite.Anchor, sprite.Texture.Dimensions * sprite.Texture.Scale);
            spriteBatch.Draw(sprite.Texture.Texture,
                             position,
                             sourceRectangle,
                             sprite.Color,
                             rotation,
                             origin,
                             scale,
                             SpriteEffects.None,
                             0);
#if DEBUG
            if (displayDebugInformation)
            {
                Console.WriteLine("spriteBatch.Draw called:\n" +
                                  "            Texture: {0} ({1}x{2})\n" +
                                  "           Position: {3}\n" +
                                  "    SourceRectangle: {4}\n" +
                                  "              Color: {5}\n" +
                                  "           Rotation: {6}\n" +
                                  "             Origin: {7} ({8})\n" +
                                  "              Scale: {9}",
                                  sprite.Texture.Name,
                                  sprite.Texture.Texture.Width,
                                  sprite.Texture.Texture.Height,
                                  position,
                                  sourceRectangle,
                                  sprite.Color,
                                  rotation,
                                  origin,
                                  sprite.Anchor,
                                  scale);
            }
#endif
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

        /// <summary>
        /// The SpriteBatch object used for drawing.
        /// </summary>
        public SpriteBatch SpriteBatch { get { return spriteBatch; } }

        public bool DisplayDebugInformation { get { return displayDebugInformation; } set { displayDebugInformation = value; } }
    }

    /*public class Camera
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
    }*/
}
