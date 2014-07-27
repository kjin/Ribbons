using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Content;
using Ribbons.Graphics;
using Ribbons.Input;
using Ribbons.Storage;
using Microsoft.Xna.Framework;

namespace Ribbons.Context
{
    public abstract class ContextBase
    {
        public ContextBase NextContext { get; protected set; }
        public bool Exit { get; protected set; }
        public Canvas Canvas { get; set; }
        public InputController InputController { get; set; }
        public AssetManager AssetManager { get; set; }
        public StorageManager StorageManager { get; set; }

        protected ContextBase()
        {
            NextContext = null;
        }

        public abstract void Initialize();

        public abstract void Dispose();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);
    }
}
