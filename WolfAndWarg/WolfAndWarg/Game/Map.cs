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
                //See if the width of this tile goes over the width of the screen, if so then start next row
                //This will probably mean that tile will go over edge of screen in some cases
                //but am not worried abou that for now.
                if (Tiles[i].GetScreenPosition(tileWidth).X + tileWidth >= screenManager.GraphicsDevice.Viewport.Width)
                {
                    if (MapWidth == 0) MapWidth = x-1;
                    x = 0;
                    y++;
                }

            }
            MapHeight = y-1;
        }
        int tileWidth = 64;
        public int TileWidth { 
            get { return tileWidth; }
            set { tileWidth = value; }
        }

        public int TileHeight { get; set; }
        public Tile[] Tiles { get; set; }

        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var tile in Tiles)
            {
                spriteBatch.Draw(tile.Texture, tile.GetScreenPosition(tileWidth), Color.White);
            }
        }

        public void SetTileObject(ISprite sprite)
        {
            GetTile(sprite.OldPosition).Object = null;
            GetTile(sprite.Position).Object = sprite;
        }

        public Vector2 GetSpritePosition(ISprite sprite)
        {
            Tile tile = GetTile(sprite.Position);
            var centre =  tile.GetCentre(tileWidth);

            //Magic number 4 here because we are currently displaying sprite a half size.
            //TODO Find better way to manage this
            centre.Y -= sprite.Texture.Height/4;
            centre.X -= sprite.Texture.Width/4;
            return centre;
        }

        public Tile GetTile(Vector2 position)
        {
            return Tiles.First(x => x.MapPosition.X == (int) position.X && x.MapPosition.Y == (int) position.Y);
        }

        public bool IsOverMapEdge(Vector2 position)
        {
            return position.X < 0 || position.X > MapWidth || position.Y < 0 || position.Y > MapHeight;
        }
    }

}
