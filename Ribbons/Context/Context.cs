using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ribbons.Content;
using Ribbons.Graphics;
using Ribbons.Input;
using Ribbons.Storage;
using Ribbons.Utils;
using Ribbons.Layout;
using Microsoft.Xna.Framework;

namespace Ribbons.Context
{
    public class Context : LayoutBase
    {
        Color BackgroundColor;
        List<ContextElement> elements;
        public int NextContext { get; protected set; }
        public bool Exit { get; protected set; }
        public Canvas Canvas { get; set; }
        public InputController InputController { get; set; }
        public AssetManager AssetManager { get; set; }
        public StorageManager StorageManager { get; set; }

        public void Initialize()
        {
            NextContext = -1;
            for (int i = 0; i < elements.Count; i++)
                elements[i].Initialize();
        }

        public void Dispose()
        {
            for (int i = 0; i < elements.Count; i++)
                elements[i].Dispose();
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < elements.Count; i++)
                elements[i].Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Canvas.Clear(BackgroundColor);
            Canvas.BeginDraw();
            for (int i = 0; i < elements.Count; i++)
                elements[i].Draw(gameTime);
            Canvas.EndDraw();
        }

        #region LayoutBase
        protected override void IntegrationPreprocess(LayoutTreeNode node)
        {
            elements = new List<ContextElement>();
        }

        protected override bool IntegrateChild(AssetManager assets, LayoutTreeNode childNode)
        {
            switch (childNode.Key)
            {
                case "BGColor":
                    BackgroundColor = ExtendedConvert.ToColor(childNode.Value);
                    return true;
                case "Elements":
                    ContextElement contextElement = ContextHelper.ContextFromName(childNode.Value);
                    if (contextElement != null)
                    {
                        contextElement.SetComponents(this);
                        contextElement.Integrate(assets, childNode);
                        elements.Add(contextElement);
                        return true;
                    }
                    break;
            }
            return false;
        }
        #endregion
    }
}
