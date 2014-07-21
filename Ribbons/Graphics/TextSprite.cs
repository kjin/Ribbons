using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ribbons.Graphics
{
    public class TextSprite
    {
        /// <summary>
        /// The SpriteFont used to draw the text sprite.
        /// </summary>
        public SpriteFont SpriteFont { get; private set; }
        /// <summary>
        /// The text used in this sprite.
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// The position of the text sprite.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// The rotation of the text sprite.
        /// </summary>
        public float Rotation { get; set; }
        /// <summary>
        /// The scale of the text sprite.
        /// </summary>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// The anchor point of the text sprite, which is the point
        /// in the sprite drawn to the text sprite's specified position,
        /// and the point about which the text sprite is rotated and scaled.
        /// </summary>
        public Anchor Anchor { get; set; }
        /// <summary>
        /// The color of the text sprite.
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// The dimensions of the text sprite.
        /// </summary>
        public Vector2 Dimensions { get; private set; }
        /// <summary>
        /// Creates a new text sprite instance.
        /// </summary>
        /// <param name="spriteFont">The SpriteFont used when drawing the text.</param>
        /// <param name="text">The text associated with the text sprite.</param>
        public TextSprite(SpriteFont spriteFont, string text) : this(spriteFont, text, Vector2.Zero, Vector2.One) { }
        /// <summary>
        /// Creates a new text sprite instance.
        /// </summary>
        /// <param name="spriteFont">The SpriteFont used when drawing the text.</param>
        /// <param name="text">The text associated with the text sprite.</param>
        /// <param name="position">The position at which the text sprite should be drawn.</param>
        public TextSprite(SpriteFont spriteFont, string text, Vector2 position) : this(spriteFont, text, position, Vector2.One) { }
        /// <summary>
        /// Creates a new text sprite instance.
        /// </summary>
        /// <param name="spriteFont">The SpriteFont used when drawing the text.</param>
        /// <param name="text">The text associated with the text sprite.</param>
        /// <param name="position">The position at which the text sprite should be drawn.</param>
        /// <param name="scale">The scale at which the text sprite should be drawn.</param>
        public TextSprite(SpriteFont spriteFont, string text, Vector2 position, Vector2 scale)
        {
            SpriteFont = spriteFont;
            Text = text;
            Position = position;
            Scale = scale;
            Anchor = Anchor.Center;
            Color = Color.White;
            Dimensions = spriteFont.MeasureString(text);
        }
    }
}
