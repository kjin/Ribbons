using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ribbons.Graphics
{
    /// <summary>
    /// An abstract class for a coordinate transform.
    /// When applied to a texture, the Transform function
    /// will be applied to each image's position, rotation, and
    /// scale, mapping the respective coordinate system
    /// associated with the transform to the viewport coordinates.
    /// </summary>
    public abstract class CoordinateTransform
    {
        public virtual void TransformPosition(ref Vector2 position) { }
        public virtual void TransformRotation(ref float rotation) { }
        public virtual void TransformScale(ref Vector2 scale) { }

        public void Transform(ref Vector2 position, ref float rotation, ref Vector2 scale)
        {
            TransformPosition(ref position);
            TransformRotation(ref rotation);
            TransformScale(ref scale);
        }
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

        /// <summary>
        /// Constructs a new UITransform object.
        /// </summary>
        public UITransform()
        {
            dimensions = GraphicsConstants.VIEWPORT_DIMENSIONS;
            scale = dimensions.Y / REFERENCE_DIMENSIONS.Y;
        }

        public override void TransformPosition(ref Vector2 position)
        {
            position *= dimensions;
        }

        public override void TransformScale(ref Vector2 scale)
        {
            scale *= this.scale;
        }
    }

    /// <summary>
    /// Maps textures expressed in gameplay coordinates
    /// to screen coordinates, based on the center position
    /// of the "camera".
    /// </summary>
    public class GameplayTransform : CoordinateTransform
    {
        Vector2 viewportDimensions;
        Vector2 center;
        Vector2 targetCenter;
        float zoom;
        float targetZoom;
        float convergenceRate;
        float pixelsPerUnit;

        /// <summary>
        /// Constructs a new GameplayTransform object, which simulates a virtual "camera".
        /// </summary>
        /// <param name="center">The center position of the camera.</param>
        /// <param name="zoom">How much the camera is zoomed.</param>
        public GameplayTransform(Vector2 center, float zoom = 1f, float convergenceRate = 1f)
        {
            this.viewportDimensions = GraphicsConstants.VIEWPORT_DIMENSIONS;
            this.center = targetCenter = center;
            this.zoom = targetZoom = zoom;
            this.convergenceRate = convergenceRate;
            this.pixelsPerUnit = GraphicsConstants.PIXELS_PER_UNIT;
        }

        public void Update()
        {
            center = Vector2.Lerp(center, targetCenter, convergenceRate);
            zoom = MathHelper.Lerp(zoom, targetZoom, convergenceRate);
        }

        public override void TransformPosition(ref Vector2 position)
        {
            position -= center;
            position *= zoom * pixelsPerUnit;
            position.Y *= -1;
            position += viewportDimensions / 2f;
        }

        public override void TransformScale(ref Vector2 scale)
        {
            scale *= zoom;
        }

        /// <summary>
        /// Gets or sets the center position of the camera.
        /// </summary>
        public Vector2 Center { get { return targetCenter; } set { targetCenter = value; } }
        /// <summary>
        /// Gets or sets the camera's zoom.
        /// </summary>
        public float Zoom { get { return targetZoom; } set { targetZoom = value; } }
    }
}
