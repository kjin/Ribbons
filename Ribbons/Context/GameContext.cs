﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Content;
using Ribbons.Graphics;
using Ribbons.Input;
using Microsoft.Xna.Framework;

namespace Ribbons.Context
{
    public abstract class GameContext
    {
        public GameContext NextContext { get; protected set; }
        public bool Exit { get; protected set; }
        public Canvas Canvas { get; set; }
        public InputController InputController { get; set; }
        public AssetManager AssetManager { get; set; }

        protected GameContext()
        {
            NextContext = null;
        }

        public abstract void Initialize();

        public abstract void Dispose();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}