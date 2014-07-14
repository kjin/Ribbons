using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ribbons.Engine
{
    public class WorldConstants
    {
        public const float GRAVITY = -1.0f;
    }

    public class GroundConstants
    {
        public const float DENSITY = 1.0f;
        public const float FRICTION = 1.0f;
    }

    public class PlayerConstants
    {
        // seamstress parameters
        public const float SLANT = 0.06f; //how triangular seamstress is shaped (lesser values = more rectangular, 0.0f = rectangle)
        public const float WIDTH = 0.6f;
        public const float DENSITY = 0.1f;
        public const float FRICTION = 0.1f;

        // seamstress movement
        public const float GROUNDSPEED = 1.0f;
        public const float AIRSPEED = 0.1f;
        public const float JUMPFORCE = 10.0f;

        // for desired air properties
        public const float HORIZONTAL_AIRDRAG = 0.02f;
        public const float VERTICAL_AIRDRAG = 0.015f;
    }
}
