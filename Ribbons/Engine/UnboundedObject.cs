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

        #region Ribbon Methods

        /// <summary>
        /// It is any ribbon or ribbon element's responsibility to update an unbounded object's speed.
        /// </summary>
        public Vector2 RibbonSpeed
        {
            set { ribbonSpeed = value; }
        }

        #endregion

        #region Update

        public virtual void Update()
        {
            // get the physics body velocity and correct for ribbon movement
            Vector2 velocity = RelativeVelocity;

            // add the velocity change from the children
            velocity += velocityChange;

            // update values
            velocityChange = new Vector2(0, 0);
            prevRibbonSpeed = ribbonSpeed;
            ribbonSpeed = new Vector2(0,0);

            // set the changed physics body velocity
            RelativeVelocity = velocity;
        }

        #endregion

    }
}
