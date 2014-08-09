using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Utils;
using Ribbons.Graphics;
using Ribbons.Content;
using Ribbons.Layout;

namespace Ribbons.Context
{
    public class AnimationCurveElement : LayoutBase
    {
        public AnimationCurve AnimationCurve;
        protected SpriteComponents spriteComponent;
        protected bool spriteComponentSet = false;

        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode)
        {
            if (childNode.Key == "Component")
            {
                spriteComponent = ExtendedConvert.ToEnum<SpriteComponents>(childNode.Value);
                spriteComponentSet = true;
                return true;
            }
            return false;
        }
    }

    public class PeriodicAnimationCurveElement : AnimationCurveElement
    {
        float amplitude = 1;
        float period = 1;
        float phase;
        float dcOffset;
        string shape = null;

        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode)
        {
            switch (childNode.Key)
            {
                case "Shape":
                    shape = childNode.Value;
                    return true;
                case "Amplitude":
                    amplitude = Convert.ToSingle(childNode.Value);
                    return true;
                case "Period":
                    period = Convert.ToSingle(childNode.Value);
                    return true;
                case "Phase":
                    phase = Convert.ToSingle(childNode.Value);
                    return true;
                case "DCOffset":
                    dcOffset = Convert.ToSingle(childNode.Value);
                    return true;
            }
            return base.IntegrateChild(assets, childNode);
        }

        protected override void IntegrationPostprocess(LayoutTreeNode node)
        {
            if (spriteComponentSet && shape != null)
            {
                switch (shape)
                {
                    case "Sinusoid":
                        AnimationCurve = new SineAnimationCurve(spriteComponent, new Sinusoid(amplitude, period, phase, dcOffset));
                        return;
                }
            }
        }
    }
}
