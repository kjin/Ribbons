using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using Ribbons.Graphics;

namespace Ribbons.Engine
{
    /// <summary>
    /// A class with Update implemented.
    /// </summary>
    public interface IUpdate
    {
        void Update(float dt);
    }

    /// <summary>
    /// A class with Draw implemented.
    /// </summary>
    public interface IDraw
    {
        void Draw(Canvas c);
    }

    /// <summary>
    /// A class describing things that move with the ribbon and can return
    /// a value describing speed and direction.
    /// </summary>
    public interface IRibbonSpeed
    {
        Vector2 RibbonSpeed(Vector2 position);

        Ribbon GetRibbon();
    }
}
