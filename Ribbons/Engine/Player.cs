using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

namespace Ribbons.Engine
{
    public class Player
    {
        #region Fields

        #endregion

        #region UserData
        public struct PlayerUserData
        {
            public Player player;
        }
        #endregion

        #region Constructor
        public Player(World world)
        {
            PlayerUserData userData;
            userData.player = this;

            Body body = BodyFactory.CreateBody(world, userData);
            Fixture fixture = body.CreateFixture(CreateShape(), userData);

            fixture.Friction = PlayerConstants.FRICTION;

            body.IsStatic = true;
        }

        private Shape CreateShape()
        {
            Vertices vertices = new Vertices();

            float x = PlayerConstants.WIDTH / 2;
            float y = 0.5f;

            vertices.Add(new Vector2(-x, y));
            vertices.Add(new Vector2(-x, -y + PlayerConstants.SLANT));
            vertices.Add(new Vector2(0, -y));
            vertices.Add(new Vector2(x, -y + PlayerConstants.SLANT));
            vertices.Add(new Vector2(x, y));

            return new PolygonShape(vertices, PlayerConstants.DENSITY);
        }
        #endregion

        #region ForceController Methods
        public void MoveLeft(float speed)
        {

        }

        public void MoveRight(float speed)
        {

        }

        public void Jump()
        {

        }
        #endregion

        #region Update
        public void Update()
        {

        }
        #endregion

        #region Draw
        public void Draw()
        {

        }
        #endregion
    }
}
