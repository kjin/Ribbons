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
    public class AnimatedTexture
    {
        Texture2D texture;
        float scale;
        Vector2 dimensions;
        int frameWidth;
        int frameHeight;
        int rows;
        int columns;
        int frames;

        /// <summary>
        /// Create a new animated Sprite object, where frames are arranged on a grid in row-major order.
        /// For non-animated sprites, only the first argument should be specified.
        /// For animated sprites on a single filmstrip, the rows parameter may be omitted.
        /// </summary>
        /// <param name="texture">The texture on which this sprite is based.</param>
        /// <param name="columns">The number of columns of frames in the texture.</param>
        /// <param name="rows">The number of rows of frames in the texture.</param>
        AnimatedTexture(Texture2D texture, int columns = 1, int rows = 1, float scale = 1)
        {
            this.scale = scale;
            this.texture = texture;
            this.frameWidth = texture.Width / columns;
            this.frameHeight = texture.Height / rows;
            this.dimensions.X = frameWidth / scale;
            this.dimensions.Y = frameHeight / scale;
            this.rows = rows;
            this.columns = columns;
            this.frames = rows * columns;
        }

        /// <summary>
        /// Gets the dimensions of the sprite.
        /// </summary>
        public Vector2 Dimensions { get { return dimensions; } }
        /// <summary>
        /// Gets the width of the sprite.
        /// </summary>
        public float Width { get { return dimensions.X; } }
        /// <summary>
        /// Gets the height of the sprite.
        /// </summary>
        public float Height { get { return dimensions.Y; } }
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
        /// Gets the scale of the sprite.
        /// </summary>
        public float Scale { get { return scale; } }

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

        public static AnimatedTexture Build(AssetManager assets, string assetName)
        {
            int columns = 1;
            int rows = 1;
            float scale = 1;
            TextDictionary td = assets.GetDictionary("graphics");
            if (td.CheckPropertyExists(assetName, "columns"))
                columns = td.LookupInt32(assetName, "columns");
            if (td.CheckPropertyExists(assetName, "rows"))
                rows = td.LookupInt32(assetName, "rows");
            if (td.CheckPropertyExists(assetName, "scale"))
                scale = td.LookupSingle(assetName, "scale");
            AnimatedTexture sprite = new AnimatedTexture(assets.GetTexture(assetName), columns, rows, scale);
            return sprite;
        }
    }
}
