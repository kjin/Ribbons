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
using Ribbons.Engine.RibbonTypes;
using Ribbons.Content.Level;

namespace Ribbons.Engine
{
    /// <summary>
    /// Ribbons can be updated and drawn to the screen.
    /// </summary>
    public abstract class Ribbon : IUpdate, IDraw, IRibbonSpeed
    {
        #region Fields

        //fields for Farseer world
        protected Body body;
        protected Queue<Fixture> fixtures = new Queue<Fixture>();

        //list of points describing the ribbon path
        protected List<Vector2> points;

        //list of vectors describing the ribbon orientation between points
        protected List<Vector2> orientations = new List<Vector2>();

        //list of lengths between points
        protected List<float> intervals = new List<float>();
        protected float length = 0; //total length of the ribbon

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

        #region Constructor

        protected Ribbon(World world, List<Vector2> path, float start, float end)
        {
            this.points = path;
            this.start = start;
            this.end = end;

            for (int i = 0; i < points.Count; i++)
            {
                Vector2 v = points[(i + 1) % points.Count] - points[i];
                v.Normalize();
                orientations.Add(v);

                float dist = Vector2.Distance(points[i], points[(i + 1) % points.Count]);
                length += dist;
                intervals.Add(dist);
            }

            UserData userData = new UserData();
            userData.thing = this;

            body = new Body(world);
            body.UserData = userData;
            body.IsStatic = true;
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Creates the initial fixture for the ribbon.
        /// </summary>
        protected void InitializeRibbon()
        {
            UserData userData = new UserData();
            userData.thing = this;

            Shape shape = GenerateShape();
            fixtures.Enqueue(body.CreateFixture(shape, userData));
        }

        #endregion

        #region Protected Methods

        protected bool IsDiscrete
        {
            get { return Math.Abs(Math.Round(start) - start) < RibbonConstants.DISCRETETHRESHOLD; }
        }

        /// <summary>
        /// Converts a spacial position to distance along the ribbon.
        /// Assumes looped ribbon (can be overwritten)
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected virtual float GetDistanceAlongRibbonFromPosition(Vector2 position)
        {
            float totalPos = 0;
            float bestPos = 0;
            float minDist = float.MaxValue;

            //for each interval:
            for (int i = 0; i < points.Count(); i++)
            {
                //find what distance along the interval is the closest
                Vector2 A = position - points[i];
                Vector2 B = points[(i + 1) % points.Count] - points[i];

                float pos = Vector2.Dot(A, B) / B.Length();
                if (i > 0)
                {
                    pos = Math.Max(pos, 0);
                }
                if (i + 1 < points.Count)
                {
                    pos = Math.Min(pos, intervals[i]);
                }

                //get the position of that point
                Vector2 p = pos * orientations[i] + points[i];
                pos += totalPos;

                //see if it's the closest we've seen so far
                if (Vector2.DistanceSquared(p, position) < minDist)
                {
                    minDist = Vector2.DistanceSquared(p, position);
                    bestPos = pos;
                }

                //keep track of how far along the line we've moved
                totalPos += intervals[i];
            }

            return bestPos;
        }

        /// <summary>
        /// Converts distance along the ribbon to an orientation.
        /// Assumes looped ribbon (can be overwritten)
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
        protected virtual Vector2 GetOrientationFromDistanceAlongRibbon(float dist)
        {
            int i = 0;

            //correct for negative distances
            while (dist < 0)
            {
                dist += length;
            }

            //continue until we find the appropriate interval
            while (intervals[i] < dist)
            {
                dist -= intervals[i];
                i++;
                while (i >= orientations.Count())
                {
                    i = i - intervals.Count();
                }
            }

            return orientations[i];

        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Generates a shape based on points and start/end position
        /// </summary>
        /// <returns></returns>
        protected abstract Shape GenerateShape();

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

        #region IUpdate

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
        /// Updates speed (and start/end position) based on latest input and discrete pulling
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

            if (Math.Abs(speed) < RibbonConstants.STILLTHRESHOLD)
            {
                speed = 0;
            }

            start += speed;
            end += speed;
        }

        /// <summary>
        /// Updates physical fixtures.
        /// </summary>
        protected virtual void UpdateBody()
        {
            //if not moving
            if (Math.Abs(speed) < RibbonConstants.STILLTHRESHOLD)
            {
                //if the ribbon isn't moving, we don't need so many fixtures
                while (fixtures.Count > 1)
                {
                    (fixtures.Dequeue()).Dispose();
                }
            }
            else
            {
                UserData userData = new UserData();
                userData.thing = this;

                Shape shape = GenerateShape();
                fixtures.Enqueue(body.CreateFixture(shape, userData));

                while (fixtures.Count > RibbonConstants.FIXTUREOVERLAY)
                {
                    (fixtures.Dequeue()).Dispose();
                }
            }            
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

        #region IDraw

        public abstract void Draw(Canvas c);

        #endregion

        #region IRibbonSpeed

        public Vector2 RibbonSpeed(Vector2 position)
        {
            float distanceAlongRibbon = GetDistanceAlongRibbonFromPosition(position);
            return speed * GetOrientationFromDistanceAlongRibbon(distanceAlongRibbon);
        }

        public Ribbon GetRibbon()
        {
            return this;
        }

        #endregion
    }

    #region RibbonFactory

    public static class RibbonFactory
    {
        public static Ribbon Get(World world, RibbonStorage ribbonStorage)
        {
            return new UnloopedRibbon(world, ribbonStorage.path, ribbonStorage.start, ribbonStorage.end);
        }
    }

    #endregion
}
