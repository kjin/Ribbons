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
using Ribbons.Content.Level;

namespace Ribbons.Context
{
    public class TestGameplayContext : ContextBase
    {
        World world;
        Player player;
        List<Ground> ground;
        Ribbon ribbon;
        GameplayTransform transform;
        ForceController forceController;

        public override void Initialize()
        {
            world = new World(new Vector2(0, WorldConstants.GRAVITY));
            player = new Player(world, new Vector2(5, 5));
            forceController = new ForceController(InputController, player);
            //ribbon = RibbonFactory.Get(world, ribbonStorage);
 
            transform = new GameplayTransform(new Vector2(5, 5), 1f);

            ground = new List<Ground>();
            List<Vector2> rectangle = new List<Vector2>();
            rectangle.Add(new Vector2(-3,0));
            rectangle.Add(new Vector2(15,0));
            rectangle.Add(new Vector2(15,2));
            rectangle.Add(new Vector2(-3,2));
            PolygonF polygon;
            polygon.points = rectangle;
            ground.Add(new Ground(world, polygon, GroundType.Ground));

            RibbonStorage ribbonStorage = new RibbonStorage();
            List<Vector2> path = new List<Vector2>();
            path.Add(new Vector2(-5, 3));
            path.Add(new Vector2(17, 3));
            ribbonStorage.path = path;
            ribbonStorage.start = 5;
            ribbonStorage.end = 10;

            ribbon = RibbonFactory.Get(world, ribbonStorage);
        }

        public override void Dispose()
        {
            world.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            transform.Update();
            forceController.Update();
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            player.Update();
            ribbon.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            Canvas.PushTransform(transform);
            Canvas.BeginDraw();
            player.Draw(Canvas);
            foreach (Ground g in ground)
            {
                g.Draw(Canvas);
            }
            ribbon.Draw(Canvas);
            Canvas.EndDraw();
            Canvas.PopTransform();
        }
    }
}
