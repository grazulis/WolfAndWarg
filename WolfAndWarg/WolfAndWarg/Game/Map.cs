using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WolfAndWarg.Game
{
    class Map
    {
        public Map(ScreenManager screenManager, ContentManager content)
        {
            int x = 0;
            int y = 0;
            
            var numberOfTiles = screenManager.GraphicsDevice.Viewport.Width / tileWidth *
                                (screenManager.GraphicsDevice.Viewport.Height + 64) / tileWidth;
            Tiles = new Tile[numberOfTiles];
            for (int i = 0; i < numberOfTiles; i++)
            {
                Tiles[i] = new Tile {Texture = content.Load<Texture2D>("Tile"), MapPosition = new Point(x, y)};
                x++;
                if (Tiles[i].GetScreenPosition(tileWidth).X >= screenManager.GraphicsDevice.Viewport.Width)
                {
                    x = 0;
                    y++;
                }

            }
        }
        int tileWidth = 64;
        public int TileWidth { 
            get { return tileWidth; }
            set { tileWidth = value; }
        }

        public int TileHeight { get; set; }
        public Tile[] Tiles { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in Tiles)
            {
                spriteBatch.Draw(tile.Texture, tile.GetScreenPosition(tileWidth), Color.White);
            }
        }
    }

}
