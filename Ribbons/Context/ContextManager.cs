using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Content;
using Ribbons.Graphics;
using Ribbons.Input;
using Ribbons.Storage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ribbons.Context
{
    public class ContextManager
    {
        ContextBase currentContext;
        float currentOverlayAlpha;
        float targetOverlayAlpha;
        bool exitGame;

        Canvas canvas;
        InputController input;
        AssetManager assets;
        StorageManager storage;

        public ContextManager(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            assets = new AssetManager();
            assets.LoadContent(content, graphicsDevice);
            canvas = new Canvas(assets, graphicsDevice, spriteBatch);
            input = new InputController(assets);
            storage = new StorageManager();
        }

        public void SetInitialContext(ContextBase context)
        {
            InitializeContextComponents(context);
            currentContext = context;
        }

        void InitializeContextComponents(ContextBase gameContext)
        {
            if (gameContext == null)
                return;
            gameContext.AssetManager = assets;
            gameContext.Canvas = canvas;
            gameContext.InputController = input;
            gameContext.StorageManager = storage;
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
            bool leftoverTransforms = canvas.PopAllTransforms();
#if DEBUG
            if (leftoverTransforms)
                Console.WriteLine("WARNING: Not all transforms were popped off the canvas stack.\n" +
                                  "    If you see this message, please check the Draw() function in the currently running context.");
#endif
        }
    }
}
