#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Ribbons.Graphics;
using Ribbons.Utils;
using Ribbons.Input;
using Ribbons.Content;
#endregion

namespace Ribbons
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class RibbonsGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Canvas canvas;
        Camera camera;
        InputController input;

        public RibbonsGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager assets = new AssetManager();
            assets.LoadContent(Content, GraphicsDevice);
            canvas = new Canvas(GraphicsDevice, spriteBatch);
            camera = new Camera();

            input = new InputController(assets);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here
            camera.Update();
            input.Update();
            Vector3 v = new Vector3();
            if (input.MenuLeft.Pressed)
                v.X -= 2;
            if (input.MenuRight.Pressed)
                v.X += 2;
            if (input.MenuUp.Pressed)
                v.Y -= 2;
            if (input.MenuDown.Pressed)
                v.Y += 2;
            if (input.MenuForward.Pressed)
                camera.Scale *= 1.1f;
            if (input.MenuBackward.Pressed)
                camera.Scale /= 1.1f;
            camera.Position += v;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            canvas.BeginDraw(camera);
            GraphicsHelper.DrawGrid(canvas, new RectangleF(-800, 800, -800, 800), 2, 50);
            canvas.EndDraw();

            base.Draw(gameTime);
        }
    }
}
