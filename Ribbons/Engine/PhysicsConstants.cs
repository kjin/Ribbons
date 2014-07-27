using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ribbons.Engine
{
    /// <summary>
    /// Contains constants that effect the entire physics world.
    /// </summary>
    public class WorldConstants
    {
        public const float GRAVITY = -20.0f;
    }

    /// <summary>
    /// Contains constants that effect the physics of ground.
    /// </summary>
    public class GroundConstants
    {
        public const float DENSITY = 1.0f;
        public const float FRICTION = 1.0f;
    }

    /// <summary>
    /// Contains constants that effects the physics of the seamstress and how she is controlled.
    /// </summary>
    public class PlayerConstants
    {
        // seamstress parameters
        public const float SLANT = 0.06f; //how triangular seamstress is shaped (lesser values = more rectangular, 0.0f = rectangle)
        public const float WIDTH = 0.6f;
        public const float DENSITY = 0.01f;
        public const float FRICTION = 0.01f;

        // seamstress ground movement
        public const float GROUNDSPEED = 2.0f;
        public const float GROUNDDRAG = 0.3f;

        // seamstress jump movement
        public const float JUMPFORCE = 8.5f;
        public const float SMALLJUMPFORCE = 7.0f;
        public const int INITJUMP = 6;  //number of frames to jump
        public const int JUMPCOOLDOWN = 20; //number of frames to jump again

        // for desired air properties
        public const float AIRSPEED = 0.15f;
        public const float HORIZONTAL_AIRDRAG = 0.03f;
        public const float VERTICAL_AIRDRAG = 0.0f;
    }
}
