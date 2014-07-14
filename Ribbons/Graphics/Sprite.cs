using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ribbons.Content;

namespace Ribbons.Graphics
{
    /// <summary>
    /// A class that exposes relevant image dimensions for physical objects, and abstracts animation.
    /// </summary>
    public class Sprite
    {
        Texture2D texture;
        float scale;
        float width;
        float height;
        int frameWidth;
        int frameHeight;
        int rows;
        int columns;
        int frames;
        bool once;

        int currentFrame;
        int tickCounter;
        int ticksPerFrame;
        float animationProgress;
        /// <summary>
        /// Create a new animated SimpleSprite object, where frames are arranged on a grid in row-major order.
        /// For non-animated sprites, only the first argument should be specified.
        /// For animated sprites on a single filmstrip, the rows parameter may be omitted.
        /// </summary>
        /// <param name="texture">The texture on which this sprite is based.</param>
        /// <param name="columns">The number of columns of frames in the texture.</param>
        /// <param name="rows">The number of rows of frames in the texture.</param>
        Sprite(Texture2D texture, int columns = 1, int rows = 1, bool once = false, float scale = 1)
        {
            this.scale = scale;
            this.texture = texture;
            this.frameWidth = texture.Width / columns;
            this.frameHeight = texture.Height / rows;
            this.width = frameWidth / GraphicsConstants.PIXELS_PER_UNIT / scale;
            this.height = frameHeight / GraphicsConstants.PIXELS_PER_UNIT / scale;
            this.rows = rows;
            this.columns = columns;
            this.frames = rows * columns;
            this.once = once;

            Reset();
            ticksPerFrame = 1;
        }

        /// <summary>
        /// Creates an identical copy of this Sprite object.
        /// </summary>
        /// <returns>A copy of this Sprite object.</returns>
        public Sprite Clone()
        {
            Sprite s = new Sprite(texture, columns, rows, once, scale);
            s.currentFrame = currentFrame;
            s.tickCounter = tickCounter;
            s.ticksPerFrame = ticksPerFrame;
            s.animationProgress = animationProgress;
            return s;
        }

        /// <summary>
        /// Gets the width of the sprite in physical coordinates.
        /// </summary>
        public float Width { get { return width; } }
        /// <summary>
        /// Gets the height of the sprite in physical coordinates.
        /// </summary>
        public float Height { get { return height; } }
        /// <summary>
        /// Gets the number of frames in the sprite.
        /// </summary>
        public int Frames { get { return frames; } }
        /// <summary>
        /// Gets the number of rows in the sprite.
        /// </summary>
        public int Rows { get { return rows; } }
        /// <summary>
        /// Gets the number of columns in the sprite.
        /// </summary>
        public int Columns { get { return columns; } }

        /// <summary>
        /// Gets the number of ticks between frames.
        /// </summary>
        public int TicksPerFrame { get { return ticksPerFrame; } set { ticksPerFrame = value; } }
        /// <summary>
        /// Gets or sets the current frame.
        /// </summary>
        public int CurrentFrame { get { return currentFrame; } set { currentFrame = value; } }
        /// <summary>
        /// Gets whether the animation has finished.
        /// </summary>
        public bool AnimationDone { get { return animationProgress == 1f; } }
        /// <summary>
        /// Gets the animation progress.
        /// </summary>
        public float AnimationProgress { get { return animationProgress; } }

        public void Reset()
        {
            currentFrame = 0;
            tickCounter = 0;
            animationProgress = 0;
        }

        public void Tick()
        {
            tickCounter++;
            if (ticksPerFrame > 0 && tickCounter >= ticksPerFrame)
            {
                if (!once || currentFrame < Frames - 1)
                {
                    currentFrame = (currentFrame + 1) % Frames;
                    animationProgress = (float)currentFrame / Frames;
                }
                else
                    animationProgress = 1f;
                tickCounter = 0;
            }
        }

        /// <summary>
        /// Returns a bounding rectangle for the specified frame.
        /// </summary>
        /// <param name="num">The frame to target.</param>
        /// <returns>The bounding retangle for the specified frame.</returns>
        public Rectangle GetFrame(int num)
        {
            if (num >= Frames || num < 0) throw new ArgumentOutOfRangeException("Frame number is out of range.");
            return new Rectangle(frameWidth * (num % columns), frameHeight * (num / columns), frameWidth, frameHeight);
        }

        /// <summary>
        /// Gets the texture associated with this sprite.
        /// Note: the dimensions of this texture must not be used in any physics calculations.
        /// Use Sprite.Width and Sprite.Height instead.
        /// </summary>
        public Texture2D Texture { get { return texture; } }

        public static Sprite Build(AssetManager assets, string assetName, int startFrame = 0)
        {
            int columns = 1;
            int rows = 1;
            bool once = false;
            TextDictionary td = assets.GetDictionary("graphics.txt");
            if (td.CheckPropertyExists(assetName, "columns"))
                columns = td.LookupInt32(assetName, "columns");
            if (td.CheckPropertyExists(assetName, "rows"))
                rows = td.LookupInt32(assetName, "rows");
            if (td.CheckPropertyExists(assetName, "once"))
                once = td.LookupBoolean(assetName, "once");
            Sprite sprite = new Sprite(assets.GetTexture(assetName), columns, rows, once);
            sprite.currentFrame = startFrame;
            return sprite;
        }
    }
}
