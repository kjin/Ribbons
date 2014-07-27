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
        public int TicksPerFrame { get { return ticksPerFrame; } set { ticksPerFrame = value; } }
        int ticksPerFrame;
        /// <summary>
        /// Whether the sprite's animation should be looped,
        /// or stop after the first cycle.
        /// </summary>
        public bool Looped { get { return looped; } set { looped = value; } }
        bool looped;
        /// <summary>
        /// The current frame of animation. This value
        /// should be expected to change periodically.
        /// </summary>
        public int Frame { get { return frame; } set { frame = value; } }
        int frame;
        /// <summary>
        /// The position of the sprite.
        /// </summary>
        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 AnimatedPosition { get { return aPosition; } }
        Vector2 position;
        Vector2 aPosition;
        /// <summary>
        /// The rotation of the sprite.
        /// </summary>
        public float Rotation { get { return rotation; } set { rotation = value; } }
        public float AnimatedRotation { get { return aRotation; } }
        float rotation;
        float aRotation;
        /// <summary>
        /// The scale of the sprite.
        /// </summary>
        public Vector2 Scale { get { return scale; } set { scale = value; } }
        public Vector2 AnimatedScale { get { return aScale; } }
        Vector2 scale;
        Vector2 aScale;
        /// <summary>
        /// The anchor point of the sprite, which is the point
        /// in the sprite drawn to the sprite's specified position,
        /// and the point about which the sprite is rotated and scaled.
        /// </summary>
        public Anchor Anchor { get { return anchor; } set { anchor = value; } }
        Anchor anchor;
        /// <summary>
        /// The color of the sprite.
        /// </summary>
        public Color Color { get { return color; } set { color = value; } }
        public Color AnimatedColor { get { return aColor; } }
        Color color;
        Color aColor;
        // Number of frames elapsed in the sprite.
        private int ticks;
        // Animation information.
        List<AnimationCurve> animationCurveStack;
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
            this.position = position;
            this.scale = scale;
            anchor = Anchor.Center;
            ticksPerFrame = 1;
            looped = true;
            color = Color.White;
            animationCurveStack = new List<AnimationCurve>(10);
        }
        /// <summary>
        /// Updates the sprite.
        /// </summary>
        public void Update()
        {
            ticks++;
            if (ticks % ticksPerFrame == 0)
            {
                ticks = 0;
                frame++;
                if (frame == Texture.Frames)
                    frame = 0;
            }
            aPosition = position;
            aRotation = rotation;
            aScale = scale;
            aColor = color;
            for (int i = animationCurveStack.Count - 1; i >= 0; i--)
            {
                animationCurveStack[i].Update();
                ProcessAnimationCurves(i);
            }
        }
        /// <summary>
        /// Resets the sprite's animation to the first frame.
        /// </summary>
        public void ResetAnimation()
        {
            frame = 0;
            ticks = 0;
            for (int i = 0; i < animationCurveStack.Count; i++)
                animationCurveStack[i].Reset();
        }

        private void ProcessAnimationCurves(int curveNumber)
        {
            AnimationCurve ac = animationCurveStack[curveNumber];
            switch (ac.SpriteComponent)
            {
                case SpriteComponents.PositionX:
                    aPosition.X += ac.Value;
                    break;
                case SpriteComponents.PositionY:
                    aPosition.Y += ac.Value;
                    break;
                case SpriteComponents.Rotation:
                    aRotation += ac.Value;
                    break;
                case SpriteComponents.ScaleX:
                    aScale.X *= ac.Value;
                    break;
                case SpriteComponents.ScaleY:
                    aScale.Y *= ac.Value;
                    break;
                case SpriteComponents.ColorRed:
                    aColor.R = (byte)(255 * MathHelper.Clamp(aColor.R / 255f + ac.Value, 0f, 1f));
                    break;
                case SpriteComponents.ColorGreen:
                    aColor.G = (byte)(255 * MathHelper.Clamp(aColor.G / 255f + ac.Value, 0f, 1f));
                    break;
                case SpriteComponents.ColorBlue:
                    aColor.B = (byte)(255 * MathHelper.Clamp(aColor.B / 255f + ac.Value, 0f, 1f));
                    break;
                case SpriteComponents.ColorAlpha:
                    aColor.A = (byte)(255 * MathHelper.Clamp(aColor.A / 255f + ac.Value, 0f, 1f));
                    break;
            }
        }

        public void PushAnimationCurve(AnimationCurve animationCurve)
        {
            animationCurveStack.Add(animationCurve);
        }

        public void PopAnimationCurve()
        {
            animationCurveStack.RemoveAt(animationCurveStack.Count - 1);
        }
    }
}
