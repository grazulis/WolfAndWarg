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
        public Point MapPosition { get; set;  }
        public Texture2D Texture { get; set; }
        public ISprite Object { get; set; }

        public Vector2 GetScreenPosition(float tileWidth)
        {
            return new Vector2(MapPosition.X * tileWidth, MapPosition.Y * tileWidth);
        }

        public Vector2 GetCentre(float tileWidth)
        {
            return new Vector2(tileWidth / 2 + (MapPosition.X * tileWidth), tileWidth / 2 + (MapPosition.Y * tileWidth));

        }


    }
}
