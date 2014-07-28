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
    /// <summary>
    /// Abstract class for any object with a physical body
    /// that moves with the ribbon when on it, but is not attached to anything.
    /// </summary>
    public abstract class UnboundedObject
    {
        #region Fields

        // physics body
        protected Body body;

        // fixture that keeps track of all IRibbonSpeed objects collided with
        protected Fixture landingFixture;
        protected List<Fixture> landedFixtures = new List<Fixture>();

        // for changing speed
        private Vector2 velocityChange;

        // for moving with the ribbon
        private Vector2 ribbonSpeed = new Vector2(0,0);
        private Vector2 prevRibbonSpeed = new Vector2(0,0);

        #endregion

        #region Constructor

        protected UnboundedObject()
        {

        }

        #endregion

        #region Initialize

        /// <summary>
        /// Called by child; initializes properties of the unbounded object
        /// landedFixture must have been initialized first
        /// </summary>
        protected void InitializeUnbounded()
        {
            if (landingFixture == null)
            {
#if DEBUG
                throw new Exception("unbounded object initialized incorrectly");
#endif
                return;
            }

            landingFixture.IsSensor = true;
            landingFixture.OnCollision += OnLandingCollision;
            landingFixture.OnSeparation += OnLandingSeparation;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Get the physics object velocity without the ribbon speed.
        /// </summary>
        protected Vector2 RelativeVelocity
        {
            get { return body.LinearVelocity - prevRibbonSpeed; }
            private set { body.LinearVelocity = value + prevRibbonSpeed; }
        }

        protected void ChangeSpeed(Vector2 change)
        {
            velocityChange += change;
        }

        #endregion

        #region OnLandingCollision

        private bool OnLandingCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (!landedFixtures.Contains(fixtureB))
            {
                landedFixtures.Add(fixtureB);
            }

            return true;
        }

        private void OnLandingSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (landedFixtures.Contains(fixtureB))
            {
                landedFixtures.Remove(fixtureB);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the physical body's velocity.
        /// Make sure child calls ChangeVelocity before calling Update to change velocity
        /// </summary>
        public virtual void Update()
        {
            // get the physics body velocity and correct for ribbon movement
            Vector2 velocity = RelativeVelocity;

            // add the velocity change from the children
            velocity += velocityChange;

            // update values
            velocityChange = new Vector2(0, 0);
            prevRibbonSpeed = ribbonSpeed;
            UpdateRibbonSpeed();

            // set the changed physics body velocity
            RelativeVelocity = velocity;
        }

        /// <summary>
        /// Looks at all ribbon features that are touched,
        /// and updates the ribbon speed accordingly (and start and end position)
        /// </summary>
        private void UpdateRibbonSpeed()
        {
            foreach (Fixture f in landedFixtures)
            {
                UserData userData = (UserData)f.UserData;
                Object o = userData.thing;

                if (o is IRibbonSpeed)
                {
                    IRibbonSpeed ribbon = (IRibbonSpeed)o;
                    ribbonSpeed = ribbon.RibbonSpeed(body.Position);
                    return;
                }
            }

            ribbonSpeed = new Vector2(0, 0);
        }

        #endregion

    }
}
