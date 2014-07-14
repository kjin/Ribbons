using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ribbons.Graphics
{
    public static class GraphicsConstants
    {
        //these should largely be left untouched
        static float defaultGameplayPixelsPerUnit = 64.0f;
        //change these depending on what the startup res should be
        static int defaultWidth = 1280;
        static int defaultHeight = 720;
        static bool defaultFullScreen = false;

        static float gameplayPixelsPerUnit = defaultGameplayPixelsPerUnit;
        /// <summary>
        /// The size of a physical unit in pixels.
        /// </summary>
        public static float PIXELS_PER_UNIT { get { return gameplayPixelsPerUnit; } }

        static int viewportWidth = defaultWidth;
        static int viewportHeight = defaultHeight;
        public static Vector2 VIEWPORT_DIMENSIONS { get { return new Vector2(viewportWidth, viewportHeight); } }
        /// <summary>
        /// The default width of the camera/gameplay window.
        /// </summary>
        public static int VIEWPORT_WIDTH
        {
            get { return viewportWidth; }
            set
            {
                viewportWidth = value;
                float multiplier = Math.Max((float)viewportWidth / defaultWidth, (float)viewportHeight / defaultHeight);
                gameplayPixelsPerUnit = defaultGameplayPixelsPerUnit * multiplier;
            }
        }
        /// <summary>
        /// The default height of the camera/gameplay window.
        /// </summary>
        public static int VIEWPORT_HEIGHT
        {
            get { return viewportHeight; }
            set
            {
                viewportHeight = value;
                float multiplier = Math.Max((float)viewportWidth / defaultWidth, (float)viewportHeight / defaultHeight);
                gameplayPixelsPerUnit = defaultGameplayPixelsPerUnit * multiplier;
            }
        }

        static bool fullScreen = defaultFullScreen;
        /// <summary>
        /// Whether the game is launched in full screen or not.
        /// </summary>
        public static bool FULL_SCREEN { get { return fullScreen; } set { fullScreen = value; } }

        /// <summary>
        /// Epsilon constant used occassionally in the graphics engine.
        /// </summary>
        public const float EPSILON = 0.001f;
        /// <summary>
        /// The default smoothness of the camera's movement.
        /// </summary>
        public const float DEFAULT_CAMERA_SMOOTHNESS = 0.05f;
    }
}
