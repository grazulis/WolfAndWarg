using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using WolfAndWarg;
using WolfAndWarg.Game;

namespace GameStateManagement
{
    public class TileManager
    {

        public Map Map { get; set; }
        public Vector2 PlayerPosition { get; set; }
        /// <summary>
        /// The position of the outside 0,0 corner of the map, in pixels.
        /// </summary>
        private static Vector2 mapOriginPosition;


        /// <summary>
        /// Calculate the screen position of a given map location (in tiles).
        /// </summary>
        /// <param name="mapPosition">A map location, in tiles.</param>
        /// <returns>The current screen position of that location.</returns>
        public Vector2 GetScreenPosition(Point mapPosition)
        {
            return new Vector2(
                mapOriginPosition.X + mapPosition.X*Map.TileWidth,
                mapOriginPosition.Y + mapPosition.Y*Map.TileWidth);
        }

        /// <summary>
        /// The viewport that the tile engine is rendering within.
        /// </summary>
        private Viewport viewport;

        /// <summary>
        /// The viewport that the tile engine is rendering within.
        /// </summary>
        public Viewport Viewport
        {
            get { return viewport; }
            set 
            { 
                viewport = value;
                viewportCenter = new Vector2(
                    viewport.X + viewport.Width / 2f,
                    viewport.Y + viewport.Height / 2f);
            }
        }

        
        /// <summary>
        /// The center of the current viewport.
        /// </summary>
        private Vector2 viewportCenter;

        /// <summary>
        /// Update the tile engine.
        /// </summary>
        public void Update()
        {
            // adjust the Map origin so that the party is at the center of the viewport
            mapOriginPosition += viewportCenter - Map.Tiles[(int)PlayerPosition.X, (int)PlayerPosition.Y].GetScreenPosition(Map.TileWidth, mapOriginPosition);

            // check to see if map boundary is on screen, so only display edge

            mapOriginPosition.X = MathHelper.Min(mapOriginPosition.X, viewport.X + (Map.TileWidth));
            mapOriginPosition.Y = MathHelper.Min(mapOriginPosition.Y, viewport.Y + Map.TileWidth);
            mapOriginPosition.X += MathHelper.Max(
                (viewport.X + viewport.Width) -
                (mapOriginPosition.X + ((Map.MapWidth + 2) * Map.TileWidth)), 0f);
            mapOriginPosition.Y += MathHelper.Max(
                (viewport.Y + viewport.Height) -
                (mapOriginPosition.Y + ((Map.MapHeight + 2) * Map.TileWidth)), 0f);
        }

        /// <summary>
        /// Draw the visible tiles in the given map layers.
        /// </summary>
        public void DrawLayers(SpriteBatch spriteBatch)
        {
            // check the parameters
            if (spriteBatch == null)
            {
                throw new ArgumentNullException("spriteBatch");
            }

            var visibleTiles = new List<Tile>();

            foreach (var tile in Map.Tiles)
            {
                    // If the tile is inside the screen
                    if (CheckVisibility(tile))
                    {
                        visibleTiles.Add(tile);

                    }
            }

            foreach (var tile in visibleTiles)
            {
                spriteBatch.Draw(tile.Texture, tile.GetScreenPosition(Map.TileWidth, mapOriginPosition),
                                    Color.White);
            }
        }


        /// <summary>
        /// Returns true if the given rectangle is within the viewport.
        /// </summary>
        public bool CheckVisibility(Tile tile)
        {
            var screenPosition = tile.GetScreenPosition(Map.TileWidth, mapOriginPosition);
            return ((screenPosition.X > viewport.X - Map.TileWidth) &&
                (screenPosition.Y > viewport.Y - Map.TileWidth) &&
                (screenPosition.X < viewport.X + viewport.Width) &&
                (screenPosition.Y < viewport.Y + viewport.Height));
        }

        
    }
}