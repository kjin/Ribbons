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
using Ribbons.Content;

namespace Ribbons.Context
{
    public class TestGameplayContext : ContextElement
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
            ribbonStorage.Path = path;
            ribbonStorage.Start = 5;
            ribbonStorage.End = 10;

            ribbon = RibbonFactory.Get(world, ribbonStorage);
        }

        public override void Dispose()
        {
            world.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            transform.Update();
            forceController.Update(dt);
            world.Step(dt);
            player.Update(dt);
            ribbon.Update(dt);
        }

        public override void Draw(GameTime gameTime)
        {
            Canvas.PushTransform(transform);
            player.Draw(Canvas);
            foreach (Ground g in ground)
            {
                g.Draw(Canvas);
            }
            ribbon.Draw(Canvas);
            Canvas.PopTransform();
        }

        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode) { return false; }
    }
}
