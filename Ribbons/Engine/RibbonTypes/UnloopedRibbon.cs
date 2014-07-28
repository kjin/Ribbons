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

namespace Ribbons.Engine.RibbonTypes
{
    public class UnloopedRibbon : Ribbon
    {
        #region Fields

        //where the solid ribbon is
        List<Vector2> ribbonPoints = new List<Vector2>();

        #endregion

        #region Constructor

        public UnloopedRibbon(World world, List<Vector2> path, float start, float end)
            : base(world, path, start, end)
        {
            InitializeRibbon();
        }

        #endregion

        #region Overridden Methods

        //need to override GetDistanceAlongRibbonFromPosition and GetOrientationFromPositionAlongRibbon
        //and maybe UpdateSpeed

        #endregion

        #region Generate Shape

        protected override Shape GenerateShape()
        {
            if (end > length)
            {
                float diff = end - length;
                end -= diff;
                start -= diff;
            }

            if (start < 0)
            {
                end -= start;
                start -= start;
            }

            int i = 0;
            float startCursor = start;
            float endCursor = end;

            while (intervals[i] < startCursor)
            {
                startCursor -= intervals[i];
                endCursor -= intervals[i];
                i++;
            }

            Vector2 v = new Vector2();
            v = points[i] + startCursor * orientations[i];

            ribbonPoints.Add(v);

            while (intervals[i] < endCursor)
            {
                endCursor -= intervals[i];
                i++;
                while (i >= intervals.Count())
                {
                    i = i - intervals.Count();
                }
                ribbonPoints.Add(points[i]);
            }

            v = points[i] + endCursor * orientations[i];
            ribbonPoints.Add(v);

            Vertices clean_shape = new Vertices();
            //now, need to "clean" shape (avoid vertices that are too close together)
            for (int k = 0; k + 1 < points.Count; k++)
            {
                if ((ribbonPoints[k] - ribbonPoints[k + 1]).Length() > 0.01f)
                {
                    clean_shape.Add(points[k]);
                }
            }
            clean_shape.Add(ribbonPoints[ribbonPoints.Count - 1]);
            ribbonPoints = clean_shape;

            if (clean_shape.Count == 2)
            {
                return new EdgeShape(clean_shape[0], clean_shape[1]);
            }
            else
            {
                return new ChainShape(new Vertices(clean_shape));
            }
        }

        #endregion

        #region Draw

        public override void Draw(Canvas canvas)
        {
            for (int i = 0; i + 1 < ribbonPoints.Count; i++)
            {
                canvas.DrawLine(Color.DarkRed, 5, ribbonPoints[i] + body.Position, ribbonPoints[i + 1] + body.Position);
            }
            canvas.DrawLine(Color.DarkRed, 5, ribbonPoints[ribbonPoints.Count - 1] + body.Position, ribbonPoints[0] + body.Position);
        }

        #endregion
    }
}
