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
    public class Map
    {
        
        public Map(ScreenManager screenManager, ContentManager content)
        {
            //TODO In future width will create viewport so map can be bigger than the width of the window
            MapWidth = (screenManager.GraphicsDevice.Viewport.Width/TileWidth) * 2;
            MapHeight = (screenManager.GraphicsDevice.Viewport.Height/TileWidth) *2;
            Tiles = new Tile[MapWidth + 1,MapHeight + 1];
            for (int y = 0; y <= MapHeight; y++)
            {
                for (int x = 0; x <= MapWidth; x++)
                {
                    Tiles[x, y] = new Tile {Texture = content.Load<Texture2D>("Tile"), MapPosition = new Point(x, y), TileSize = new Point(64, 64)};
                }
            }
        }

        public int TileWidth = 64;
        
        public Tile[,] Tiles { get; set; }

        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    foreach (var tile in Tiles)
        //    {
        //        spriteBatch.Draw(tile.Texture, tile.GetScreenPosition(tileWidth), Color.White);
        //    }
        //}

        public void SetTileObject(ISprite sprite)
        {
            GetTile(sprite.OldPosition).Object = null;
            GetTile(sprite.Position).Object = sprite;
        }

        public Vector2 GetSpritePosition(ISprite sprite)
        {
            Tile tile = GetTile(sprite.Position);
            var centre = tile.GetCentre();

            //Magic number 4 here because we are currently displaying sprite a half size.
            //TODO Find better way to manage this
            centre.Y -= sprite.Texture.Height / 4;
            centre.X -= sprite.Texture.Width / 4;
            return centre;
        }

        public Tile GetTile(Vector2 position)
        {
            return Tiles[(int) position.X, (int) position.Y];
        }

        public bool IsOverMapEdge(Vector2 position)
        {
            return position.X < 0 || position.X > MapWidth || position.Y < 0 || position.Y > MapHeight;
        }
    }
}