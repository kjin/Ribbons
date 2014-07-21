using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Content;
using Ribbons.Graphics;
using Ribbons.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ribbons.Context
{
    public class ContextManager
    {
        GameContext currentContext;
        float currentOverlayAlpha;
        float targetOverlayAlpha;
        bool exitGame;

        Canvas canvas;
        InputController input;
        AssetManager assets;

        public ContextManager(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            assets = new AssetManager();
            assets.LoadContent(content, graphicsDevice);
            canvas = new Canvas(graphicsDevice, spriteBatch);
            input = new InputController(assets);
        }

        public void SetInitialContext(GameContext context)
        {
            InitializeContextComponents(context);
            currentContext = context;
        }

        void InitializeContextComponents(GameContext gameContext)
        {
            if (gameContext == null)
                return;
            gameContext.AssetManager = assets;
            gameContext.Canvas = canvas;
            gameContext.InputController = input;
            gameContext.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            input.Update();
            canvas.DisplayDebugInformation = input.Debug.JustPressed;
            if (currentContext.Exit || currentContext.NextContext != null)
            {
                targetOverlayAlpha = 1;
                if (currentOverlayAlpha == 1)
                {
                    //audioPlayer.StopSong();
                    if (currentContext.Exit)
                        exitGame = true;
                    else
                    {
                        InitializeContextComponents(currentContext.NextContext);
                        currentContext.Dispose();
                        currentContext = currentContext.NextContext;
                    }
                }
            }
            else
            {
                currentContext.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            currentContext.Draw(gameTime);
        }
    }
}
