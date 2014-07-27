using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ribbons.Graphics
{
    public enum SpriteComponents
    {
        None, PositionX, PositionY, Rotation, ScaleX, ScaleY, ColorRed, ColorGreen, ColorBlue, ColorAlpha
    }

    public abstract class AnimationCurve
    {
        protected int ticks;
        float value;
        SpriteComponents spriteComponent;

        public AnimationCurve(SpriteComponents spriteComponent)
        {
            this.spriteComponent = spriteComponent;
            ticks = 0;
        }

        public void Update()
        {
            ticks++;
            value = ApplyCurve();
        }

        protected abstract float ApplyCurve();

        public void Reset() { ticks = 0; }

        public SpriteComponents SpriteComponent { get { return spriteComponent; } }
        public float Value { get { return value; } }
    }

    public class SineAnimationCurve : AnimationCurve
    {
        Sinusoid curve;

        public SineAnimationCurve(SpriteComponents spriteComponent, Sinusoid curve) : base(spriteComponent) { this.curve = curve; }

        protected override float ApplyCurve()
        {
            return curve.Evaluate(ticks);
        }
    }

    public struct Sinusoid
    {
        public float Amplitude;
        public float Period;
        public float Phase;
        public float DCOffset;

        public Sinusoid(float amplitude, float period, float phase, float dcOffset)
        {
            Amplitude = amplitude;
            Period = period;
            Phase = phase;
            DCOffset = dcOffset;
        }

        public static Sinusoid BuildSine(float amplitude, float period)
        {
            return new Sinusoid(amplitude, period, 0, 0);
        }

        public static Sinusoid BuildCosine(float amplitude, float period)
        {
            return new Sinusoid(amplitude, period, MathHelper.PiOver2, 0);
        }

        public float Evaluate(float t)
        {
            return Amplitude * (float)Math.Sin(MathHelper.TwoPi * t / Period + Phase) + DCOffset;
        }
    }
}
