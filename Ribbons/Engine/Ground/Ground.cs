using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
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
        public Ground(World world, PolygonF polygon, GroundType type)
        {
            GroundUserData userData;
            userData.groundType = type;

            Vertices vertices = new Vertices(polygon.points);

            Body body = BodyFactory.CreateBody(world, userData);
            Fixture fixture = body.CreateFixture(new PolygonShape(vertices, GroundConstants.DENSITY), userData);

            fixture.Friction = GroundConstants.FRICTION;
            body.IsStatic = true;

            if (type == GroundType.Miasma)
            {
                body.IsSensor = true;
            }
        }
    }
}
