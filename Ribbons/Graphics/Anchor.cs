using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ribbons.Graphics
{
    /// <summary>
    /// An enumeration of anchor points for drawing sprites and text.
    /// </summary>
    public enum Anchor
    {
        TopLeft, TopCenter, TopRight, CenterLeft, Center, CenterRight, BottomLeft, BottomCenter, BottomRight
    }

    public static class AnchorHelper
    {
        /// <summary>
        /// Compute the origin of a box based on its dimensions, and an "anchor" point.
        /// </summary>
        /// <param name="anchor">The location of the anchor point.</param>
        /// <param name="dimensions">The dimensions of the box.</param>
        /// <returns>The computed origin.</returns>
        public static Vector2 ComputeAnchorOrigin(Anchor anchor, Vector2 dimensions)
        {
            return new Vector2((int)anchor % 3 * dimensions.X / 2, (int)anchor / 3 * dimensions.Y / 2);
        }
    }
}
