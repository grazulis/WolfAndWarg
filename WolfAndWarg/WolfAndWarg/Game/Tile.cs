using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WolfAndWarg.Game
{
    public class Tile
    {
        public Point MapPosition { get; set;  }
        public Point TileSize { get; set; }
        public Vector2 ScreenPosition { get; set; }
        public Texture2D Texture { get; set; }
        public ISprite Object { get; set; }

        public Vector2 GetScreenPosition(float tileWidth, Vector2 screenCentre)
        {
            ScreenPosition = new Vector2(MapPosition.X * tileWidth, MapPosition.Y * tileWidth) + screenCentre;
            return ScreenPosition;
        }


        public Vector2 GetCentre()
        {
            return new Vector2(ScreenPosition.X + (TileSize.X/2), ScreenPosition.Y + (TileSize.Y/2));
        }


    }
}
