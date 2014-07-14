using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ribbons.Utils;

namespace Ribbons.Graphics
{
    public static class GraphicsHelper
    {
        public static void DrawGrid(Canvas canvas, RectangleF bounds, float thickness, float lineEvery)
        {
            canvas.DrawLine(Color.Red, thickness, new Vector2(bounds.Left, 0), new Vector2(bounds.Right, 0));
            canvas.DrawLine(Color.Green, thickness, new Vector2(0, bounds.Bottom), new Vector2(0, bounds.Top));
            for (float x = -lineEvery; x >= bounds.Left; x -= lineEvery)
                canvas.DrawLine(Color.Gray, thickness, new Vector2(x, bounds.Bottom), new Vector2(x, bounds.Top));
            for (float x = lineEvery; x <= bounds.Right; x += lineEvery)
                canvas.DrawLine(Color.Gray, thickness, new Vector2(x, bounds.Bottom), new Vector2(x, bounds.Top));
            for (float y = -lineEvery; y >= bounds.Top; y -= lineEvery)
                canvas.DrawLine(Color.Gray, thickness, new Vector2(bounds.Left, y), new Vector2(bounds.Right, y));
            for (float y = lineEvery; y <= bounds.Bottom; y += lineEvery)
                canvas.DrawLine(Color.Gray, thickness, new Vector2(bounds.Left, y), new Vector2(bounds.Right, y));
        }
    }
}
