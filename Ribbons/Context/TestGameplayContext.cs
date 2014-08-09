﻿using System;
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
using Ribbons.Content;
using Ribbons.Layout;

namespace Ribbons.Context
{
    public class TestGameplayContext : ContextElement
    {
        World world;
        Player player;
        List<Ground> ground;
        GameplayTransform transform;
        ForceController forceController;

        public override void Initialize()
        {
            world = new World(new Vector2(0, WorldConstants.GRAVITY));
            player = new Player(world, new Vector2(5, 5));
            forceController = new ForceController(InputController, player);

            transform = new GameplayTransform(new Vector2(5, 5), 1f);

            ground = new List<Ground>();
            List<Vector2> rectangle = new List<Vector2>();
            rectangle.Add(new Vector2(0,0));
            rectangle.Add(new Vector2(10,0));
            rectangle.Add(new Vector2(10,2));
            rectangle.Add(new Vector2(0,2));
            PolygonF polygon;
            polygon.points = rectangle;
            ground.Add(new Ground(world, polygon, GroundType.Ground));
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
        }

        public override void Draw(GameTime gameTime)
        {
            Canvas.PushTransform(transform);
            player.Draw(Canvas);
            foreach (Ground g in ground)
            {
                g.Draw(Canvas);
            }
            Canvas.PopTransform();
        }

        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode) { return false; }
    }
}
