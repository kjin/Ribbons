using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ribbons.Graphics
{
    /// <summary>
    /// Represents a sprite. This class does pretty much no
    /// invalid field checking, so don't do anything stupid.
    /// </summary>
    public class Sprite
    {
        #region Properties
        public AnimatedTexture Texture { get; private set; }
        /// <summary>
        /// How many ticks should pass before the sprite
        /// should advance to its next animation frame.
        /// </summary>
        public int TicksPerFrame { get; set; }
        /// <summary>
        /// Whether the sprite's animation should be looped,
        /// or stop after the first cycle.
        /// </summary>
        public bool Looped { get; set; }
        /// <summary>
        /// The current frame of animation. This value
        /// should be expected to change periodically.
        /// </summary>
        public int Frame { get; set; }
        /// <summary>
        /// The position of the sprite.
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// The rotation of the sprite.
        /// </summary>
        public float Rotation { get; set; }
        /// <summary>
        /// The scale of the sprite.
        /// </summary>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// The anchor point of the sprite, which is the point
        /// in the sprite drawn to the sprite's specified position,
        /// and the point about which the sprite is rotated and scaled.
        /// </summary>
        public Anchor Anchor { get; set; }
        /// <summary>
        /// The color of the sprite.
        /// </summary>
        public Color Color { get; set; }
        private int ticks;
        #endregion
        /// <summary>
        /// Creates a new sprite instance.
        /// </summary>
        /// <param name="texture">The texture associated with the sprite.</param>
        public Sprite(AnimatedTexture texture) : this(texture, Vector2.Zero, Vector2.One) { }
        /// <summary>
        /// Creates a new sprite instance.
        /// </summary>
        /// <param name="texture">The texture associated with the sprite.</param>
        /// <param name="position">The position at which the sprite should be drawn.</param>
        public Sprite(AnimatedTexture texture, Vector2 position) : this(texture, position, Vector2.One) { }
        /// <summary>
        /// Creates a new sprite instance.
        /// </summary>
        /// <param name="texture">The texture associated with the sprite.</param>
        /// <param name="position">The position at which the sprite should be drawn.</param>
        /// <param name="scale">The scale at which the sprite should be drawn.</param>
        public Sprite(AnimatedTexture texture, Vector2 position, Vector2 scale)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
            Anchor = Anchor.Center;
            TicksPerFrame = 1;
            Looped = true;
            Color = Color.White;
        }
        /// <summary>
        /// Updates the sprite.
        /// </summary>
        public void Update()
        {
            ticks++;
            if (ticks % TicksPerFrame == 0)
            {
                ticks = 0;
                Frame++;
                if (Frame == Texture.Frames)
                    Frame = 0;
            }
        }
        /// <summary>
        /// Resets the sprite's animation to the first frame.
        /// </summary>
        public void ResetAnimation()
        {
            Frame = 0;
            ticks = 0;
        }
    }
}
