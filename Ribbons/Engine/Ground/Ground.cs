using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

using Ribbons.Utils;
using Ribbons.Engine;
    

namespace Ribbons.Engine.Ground
{

    public enum GroundType { Ground, Miasma };

    public struct GroundUserData
    {
        public GroundType groundType;
    }

    public class Ground
    {
        public Ground(World world, PolygonF shape, GroundType type)
        {
            GroundUserData userData;
            userData.groundType = type;

            Vertices vertices = new Vertices(shape.points);

            Body body = BodyFactory.CreateBody(world, userData);
            FixtureFactory.AttachPolygon(vertices, GroundConstants.GROUND_DENSITY, body, userData);
            body.IsStatic = true;

            if (type == GroundType.Miasma)
            {
                body.IsSensor = true;
            }
        }
    }
}
