using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ribbons.Graphics
{
    /// <summary>
    /// An interface for a coordinate transform.
    /// When applied to a texture, the Transform function
    /// will be applied to each image's position, rotation, and
    /// scale, mapping the respective coordinate system
    /// associated with the transform to the viewport coordinates.
    /// </summary>
    public interface CoordinateTransform
    {
        void Transform(ref Vector2 position,
                       ref float rotation,
                       ref Vector2 scale);
    }

    /// <summary>
    /// Maps textures expressed in the positional range
    /// [0,1]x[0,1] to viewport coordinates, with scaling based
    /// on y-coordinate.
    /// </summary>
    public class UITransform : CoordinateTransform
    {
        static Vector2 REFERENCE_DIMENSIONS = new Vector2(1280, 720);
        Vector2 dimensions; //uniform scale uses dimensions.Y
        float scale;

        public UITransform(Vector2 dimensions)
        {
            this.dimensions = dimensions;
            scale = dimensions.Y / REFERENCE_DIMENSIONS.Y;
        }

        public void Transform(ref Vector2 position,
                              ref float rotation,
                              ref Vector2 scale)
        {
            position *= dimensions;
            //we don't transform rotation.
            scale *= this.scale;
        }
    }
}
