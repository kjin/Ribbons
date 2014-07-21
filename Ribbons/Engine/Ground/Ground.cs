using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

using Ribbons.Utils;
using Ribbons.Graphics;


namespace Ribbons.Engine.Ground
{

    public enum GroundType { Ground, Miasma };

    public class Ground
    {
        #region Fields
        //for drawing
        private PolygonF shape;
        private GroundType type;

        #endregion

        #region Constructor

        public Ground(World world, PolygonF polygon, GroundType type)
        {
            this.shape = polygon;
            this.type = type;

            UserData userData = new UserData();
            userData.thing = this;

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
        #endregion

        #region Draw
        public void Draw(Canvas canvas)
        {
            List<Vector2> vectors = shape.points;
            for (int i = 0; i + 1 < vectors.Count; i++)
            {
                canvas.DrawLine(Color.DarkOliveGreen, 5, vectors[i], vectors[i + 1]);
            }
            canvas.DrawLine(Color.DarkOliveGreen, 5, vectors[vectors.Count - 1], vectors[0]);
        }
        #endregion
    }
}
