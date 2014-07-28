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
    public class ContextManager : LayoutBase
    {
        List<ContextBase> contexts;
        int currentContext;
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
            LayoutTree layoutTree = LayoutEngine.BuildLayout(assets.GetText("mockup"));
            Integrate(assets, layoutTree.Root);
        }

        public void Update(GameTime gameTime)
        {
            input.Update();
            canvas.DisplayDebugInformation = input.Debug.JustPressed;
            if (contexts[currentContext].Exit || contexts[currentContext].NextContext != -1)
            {
                targetOverlayAlpha = 1;
                if (currentOverlayAlpha == 1)
                {
                    //audioPlayer.StopSong();
                    if (contexts[currentContext].Exit)
                        exitGame = true;
                    else
                    {
                        int oldContext = currentContext;
                        currentContext = contexts[currentContext].NextContext;
                        contexts[oldContext].Dispose();
                        contexts[currentContext].Initialize();
                    }
                }
            }
            else
            {
                contexts[currentContext].Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            contexts[currentContext].Draw(gameTime);
            bool leftoverTransforms = canvas.PopAllTransforms();
#if DEBUG
            if (leftoverTransforms)
                Console.WriteLine("ContextManager WARNING: Not all transforms were popped off the canvas stack.\n" +
                                  "    If you see this message, please check the Draw() function in the currently running context.");
#endif
        }

        #region LayoutBase
        protected override void IntegrationPreprocess(LayoutTreeNode node)
        {
            contexts = new List<ContextBase>(node.Children.Count);
        }
        
        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode)
        {
            switch (childNode.Key)
            {
                case "InitialContext":
                    for (int i = 0; i < contexts.Count; i++)
                    {
                        if (contexts[i].LayoutName == childNode.Value)
                        {
                            currentContext = i;
                            return true;
                        }
                    }
                    Console.WriteLine("LayoutEngine WARNING: Couldn't find a context named {0}. Make sure the context has been created first.", childNode.Value);
                    return true;
                case "Contexts":
                    ContextBase gameContext = new ContextBase();
                    gameContext.AssetManager = assets;
                    gameContext.Canvas = canvas;
                    gameContext.InputController = input;
                    gameContext.StorageManager = storage;
                    gameContext.Integrate(assets, childNode);
                    contexts.Add(gameContext);
                    return true;
            }
            return false;
        }

        protected override void IntegrationPostprocess(LayoutTreeNode node)
        {
            contexts[currentContext].Initialize();
        }
        #endregion
    }
}
