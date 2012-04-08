using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WolfAndWarg.Game
{
    class Tile
    {
        public float Width { get; set; }
        public float Centre { get; set; }
        public Point MapPosition { get; set;  }
        public Vector2 ScreenPosition { get; set; }
        public Texture2D Texture { get; set; }
    }
}
