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
using Ribbons.Content.Level;

namespace Ribbons.Engine
{
    /// <summary>
    /// Ribbons can be updated and drawn to the screen.
    /// </summary>
    public abstract class Ribbon
    {
        #region Fields

        //fields for Farseer world
        protected Body body;
        protected Queue<Fixture> fixtures = new Queue<Fixture>();

        //list of points describing the ribbon path
        protected List<Vector2> points = new List<Vector2>();

        //list of vectors describing the ribbon orientation between points
        protected List<Vector2> orientations = new List<Vector2>();

        //list of lengths between points
        protected List<float> lengths = new List<float>();

        //the speed the ribbon is currently moving at
        protected float speed = 0;

        //the position along the path the ribbon is currently at
        protected float start;
        protected float end;

        //how the ribbon has been controlled
        protected float moveLeft = 0;
        protected float moveRight = 0;
        protected bool moved = false; //whether the ribbon has been moved this frame
        protected float holdMovement = 0; //how far the ribbon has moved while holding the button

        #endregion

        #region Protected Methods

        public bool IsDiscrete
        {
            get { return Math.Abs(Math.Round(start) - start) < RibbonConstants.DISCRETETHRESHOLD; }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Generates a shape based on points and start/end position
        /// </summary>
        /// <returns></returns>
        protected abstract Shape GenerateShape();

        #endregion

        #region Constructor

        protected Ribbon(List<Vector2> path, float start, float end)
        {

        }

        #endregion

        #region ForceController Methods

        public void MoveLeft(float speed)
        {
            moveLeft = speed;
            if(speed != 0)
                moved = true;
        }

        public void MoveRight(float speed)
        {
            moveRight = speed;
            if(speed != 0)
                moved = true;
        }

        public void Flip(Vector2 position)
        {
            //TODO
        }

        #endregion

        #region Update

        public virtual void Update()
        {
            //updates speed local variable
            UpdateSpeed();

            //updates physical fixtures
            UpdateBody();

            //calls move on all children, resolving collisions if necessary
            UpdateChildren();

            //resets appropriate variables
            UpdateEnd();
        }

        /// <summary>
        /// Updates speed based on latest input and discrete pulling
        /// </summary>
        protected virtual void UpdateSpeed()
        {
            if (moved)
            {
                speed += moveRight * RibbonConstants.SPEED;
                speed -= moveLeft * RibbonConstants.SPEED;
                speed -= speed * RibbonConstants.DYNAMICDRAG;

                holdMovement += speed;
            }
            else
            {
                if (IsDiscrete)
                {
                    speed -= speed * RibbonConstants.STATICDRAG;
                }
                else
                {
                    float offset = start - (float)Math.Floor(start);

                    //if the ribbon was moved a short distance back:
                    if (holdMovement <= 0 && holdMovement > -RibbonConstants.DISCRETEOFFSET)
                    {
                        speed -= (1 - offset) * RibbonConstants.DISCRETEPULL;
                    }
                    //if the ribbon was moved a short distance forward:
                    else if (holdMovement >= 0 && holdMovement < RibbonConstants.DISCRETEOFFSET)
                    {
                        speed += (offset) * RibbonConstants.DISCRETEPULL;
                    }
                    //if the ribbon was moved a long distance:
                    else
                    {
                        speed -= (1 - offset) * RibbonConstants.DISCRETEPULL;
                        speed += (offset) * RibbonConstants.DISCRETEPULL;
                    }

                    speed -= speed * RibbonConstants.DYNAMICDRAG;
                }

                holdMovement = 0;
            }
        }

        /// <summary>
        /// Updates physical fixtures
        /// </summary>
        protected virtual void UpdateBody()
        {
            /*
            if (Math.Abs(speed) < RibbonConstants.STILLTHRESHOLD)
            {
                while(fixtures.Count > 1)
                {
                    fixtures.Po
            }
            else
            {


            }
             * */
        }

        /// <summary>
        /// Updates ribbon elements, ribbon features, etc.
        /// </summary>
        protected virtual void UpdateChildren()
        {
            //children not implemented yet! :)
        }

        /// <summary>
        /// Overwrites certain variables, etc.
        /// </summary>
        private void UpdateEnd()
        {
            moveLeft = 0;
            moveRight = 0;
            moved = false;
        }

        #endregion

        #region Draw
        public abstract void Draw(Canvas c);

        #endregion

        #region RibbonFactory

        public static class RibbonFactory
        {
            public static Ribbon Get(RibbonStorage ribbonStorage)
            {
                return null;
            }
        }

        #endregion
    }
}
