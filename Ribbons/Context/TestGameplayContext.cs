using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;

using Ribbons.Engine.Ground;
using Ribbons.Engine;
using Ribbons.Utils;
using Ribbons.Graphics;

namespace Ribbons.Context
{
    public class TestGameplayContext : GameContext
    {
        World world;
        Player player;
        List<Ground> ground;
        Camera camera;

        public override void Initialize()
        {
            world = new World(new Vector2(0, WorldConstants.GRAVITY));
            player = new Player(world, new Vector2(5, 5));

            ground = new List<Ground>();
            List<Vector2> rectangle = new List<Vector2>();
            rectangle.Add(new Vector2(0,0));
            rectangle.Add(new Vector2(10,0));
            rectangle.Add(new Vector2(10,2));
            rectangle.Add(new Vector2(0,2));
            PolygonF polygon;
            polygon.points = rectangle;
            ground.Add(new Ground(world, polygon, GroundType.Ground));
            camera = new Camera();
        }

        public override void Dispose()
        {
            world.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            world.Step((float) (gameTime.ElapsedGameTime).Milliseconds);
        }

        public override void Draw(GameTime gameTime)
        {
            Canvas.BeginDraw(camera);
            player.Draw(Canvas);
            foreach (Ground g in ground)
            {
                g.Draw(Canvas);
            }
            Canvas.EndDraw();
        }
    }
}
