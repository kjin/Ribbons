using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ribbons.Graphics
{
    public class Canvas
    {
        SpriteBatch 
    }

    public class Camera
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Matrix World { get; private set; }

        private void UpdateWorld()
        {
            World = Matrix.CreateScale(Scale);
        }
    }
}
