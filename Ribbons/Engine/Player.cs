using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

using Ribbons.Graphics;

namespace Ribbons.Engine
{

    #region UserData
    public struct PlayerUserData
    {
        public Player player;
    }
    #endregion

    public class Player
    {
        #region Fields
        // keep track for drawing
        List<Vector2> boundingBox;

        #endregion

        #region Constructor
        public Player(World world, Vector2 position)
        {
            PlayerUserData userData;
            userData.player = this;

            Body body = BodyFactory.CreateBody(world, position, 0, userData);
            Fixture fixture = body.CreateFixture(CreateShape(), userData);

            fixture.Friction = PlayerConstants.FRICTION;

            body.IsStatic = false;
            body.FixedRotation = true;
        }

        private Shape CreateShape()
        {
            List<Vector2> vertices = new List<Vector2>();

            float x = PlayerConstants.WIDTH / 2;
            float y = 0.5f;

            vertices.Add(new Vector2(-x, y));
            vertices.Add(new Vector2(-x, -y + PlayerConstants.SLANT));
            vertices.Add(new Vector2(0, -y));
            vertices.Add(new Vector2(x, -y + PlayerConstants.SLANT));
            vertices.Add(new Vector2(x, y));

            boundingBox = vertices;

            return new PolygonShape(new Vertices(vertices), PlayerConstants.DENSITY);
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
        public void Draw(Canvas canvas)
        {
            for(int i = 0; i+1 < boundingBox.Count; i++)
            {
                canvas.DrawLine(Color.OrangeRed, 5, boundingBox[i], boundingBox[i+1]);
            }
            canvas.DrawLine(Color.OrangeRed, 5, boundingBox[boundingBox.Count-1], boundingBox[0]);
        }
        #endregion
    }
}
