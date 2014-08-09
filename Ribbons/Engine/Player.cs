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
    public class Player : UnboundedObject, IUpdate
    {
        #region Fields

        // keep track for debug drawing
        private List<Vector2> boundingBox = new List<Vector2>();

        // keep track of input
        private float moveLeft = 0;
        private float moveRight = 0;
        private bool startJump = false;
        private bool continueJump = false;

        // keep track of jump
        // (1 through INITJUMP is starting to jump, 
        // INITJUMP to JUMPCOOLDOWN is unable to jump)
        private int jumpCountdown = 0;
        private float jumpVelocity = 0;

        // The ribbon currently under control
        private Ribbon ribbon;

        // fields from UnboundedObject:
        //Body body;
        //
        //Fixture landingFixture;
        //List<Fixture> landedFixtures = new List<Fixture>();

        #endregion

        #region Properties

        public Ribbon Ribbon
        {
            get { return ribbon; }
        }

        #endregion

        #region Constructor

        public Player(World world, Vector2 position)
        {
            // create the user data
            UserData userData = new UserData();
            userData.thing = this;

            // create the body
            body = BodyFactory.CreateBody(world, position, 0, userData);
            body.IsStatic = false;
            body.FixedRotation = true;

            // create the body fixture
            Fixture bodyFixture = body.CreateFixture(CreateBodyShape(), userData);
            bodyFixture.Friction = PlayerConstants.FRICTION;

            // create the landing fixture
            landingFixture = body.CreateFixture(CreateLandingShape(), userData);
            landingFixture.IsSensor = true;
            landingFixture.OnCollision += OnLandingCollision;

            InitializeUnbounded();
        }

        /// <summary>
        /// Returns the shape of player's body
        /// </summary>
        private Shape CreateBodyShape()
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

        /// <summary>
        /// Returns the shape of landing detector
        /// </summary>
        private Shape CreateLandingShape()
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

        #region Private Methods

        private bool IsGrounded()
        {
            bool ret = false;
            foreach (Fixture f in landedFixtures)
            {
                if (!f.IsSensor)
                {
                    ret = true;
                }
            }

            return ret;
        }

        #endregion

        #region ForceController Methods

        public void MoveLeft(float speed)
        {
            moveLeft = speed;
        }

        public void MoveRight(float speed)
        {
            moveRight = speed;
        }

        public void Jump()
        {
            startJump = true;
        }

        public void ContinueJump()
        {
            continueJump = true;
        }

        public void RibbonFlip()
        {
            Ribbon.Flip(body.Position);
        }

        #endregion

        #region OnLandingCollision

        private bool OnLandingCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            UserData userData = (UserData)fixtureB.UserData;
            if (userData.thing is IRibbonSpeed)
            {
                ribbon = ((IRibbonSpeed)userData.thing).GetRibbon();
            }

            return true;
        }

        #endregion

        #region Update

        public override void Update(float dt)
        {
            // update jumpVelocity variable based on jump inputs
            UpdateJump();

            // update change in velocity
            UpdateVelocity();

            // update physics body velocity
            base.Update(dt);

            // reset values for next update
            UpdateEnd();
        }

        /// <summary>
        /// Checks jumpCountdown and input and edits jumpVelocity accordingly
        /// </summary>
        private void UpdateJump()
        {
            jumpVelocity = 0;

            // we can jump again
            if (jumpCountdown > PlayerConstants.JUMPCOOLDOWN)
            {
                jumpCountdown = 0;
            }

            // if we've initiated a jump
            if (jumpCountdown >= 1)
            {
                jumpCountdown++;
            }

            // a jump has been initiated
            if (startJump && IsGrounded() && (jumpCountdown == 0))
            {
                jumpCountdown = 1;
            }

            // jump startup is finished
            if (jumpCountdown == PlayerConstants.INITJUMP && IsGrounded())
            {
                if (continueJump)
                {
                    jumpVelocity = PlayerConstants.JUMPFORCE;
                }
                else
                {
                    jumpVelocity = PlayerConstants.SMALLJUMPFORCE;
                }
            }
        }

        /// <summary>
        /// Uses input to update physics body velocity.
        /// </summary>
        private void UpdateVelocity()
        {
            Vector2 velocityChange = new Vector2(0,0);

            velocityChange += new Vector2(0, jumpVelocity);

            if (IsGrounded())
            {
                velocityChange += new Vector2(-PlayerConstants.GROUNDSPEED * moveLeft, 0);
                velocityChange += new Vector2(PlayerConstants.GROUNDSPEED * moveRight, 0);
                velocityChange.X -= RelativeVelocity.X * PlayerConstants.GROUNDDRAG;
            }
            else
            {
                velocityChange += new Vector2(-PlayerConstants.AIRSPEED * moveLeft, 0);
                velocityChange += new Vector2(PlayerConstants.AIRSPEED * moveRight, 0);
                velocityChange.X -= RelativeVelocity.X * PlayerConstants.HORIZONTALAIRDRAG;
                velocityChange.Y -= RelativeVelocity.Y * PlayerConstants.VERTICALAIRDRAG;
            }

            ChangeSpeed(velocityChange);
        }

        /// <summary>
        /// Resets appropriate variables to be ready for the next time step.
        /// </summary>
        private void UpdateEnd()
        {
            moveLeft = 0;
            moveRight = 0;
            startJump = false;
            continueJump = false;

            jumpVelocity = 0;
        }

        #endregion

        #region Draw

        public void Draw(Canvas canvas)
        {
            for(int i = 0; i+1 < boundingBox.Count; i++)
            {
                canvas.DrawLine(Color.OrangeRed, 5, boundingBox[i] + body.Position, boundingBox[i+1] + body.Position);
            }
            canvas.DrawLine(Color.OrangeRed, 5, boundingBox[boundingBox.Count-1] + body.Position, boundingBox[0] + body.Position);
        }

        #endregion
    }
}
