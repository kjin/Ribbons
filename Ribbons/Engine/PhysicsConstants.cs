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
        public const float HORIZONTALAIRDRAG = 0.03f;
        public const float VERTICALAIRDRAG = 0.0f;
    }

    /// <summary>
    /// Contains constants determining how the ribbon moves.
    /// </summary>
    public class RibbonConstants
    {
        // ribbon speed
        public const float SPEED = 0.05f;
        public const float DYNAMICDRAG = 0.05f;

        // ribbon discrete movement
        public const float STATICDRAG = 1.0f;
        public const float DISCRETETHRESHOLD = 0.05f; //how close the ribbon has to be to be "discrete"
        public const float DISCRETEPULL = 0.005f; //how much the ribbon is pulled to discrete areas
        public const float DISCRETEOFFSET = 1.5f; //how far the ribbon must have been moved to be pulled back
        public const float STILLTHRESHOLD = 0.0001f; //how still the ribbon must be to go into "still" mode

        // ribbon fixture constants
        public const int FIXTUREOVERLAY = 4; //the number of overlapping fixtures used to describe the ribbon

    }
}
