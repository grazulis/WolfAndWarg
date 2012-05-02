using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WolfAndWarg.Game
{
    public class Player : ISprite
    {
        public int Health { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 OldPosition { get; set; }
        
        public void Draw(SpriteBatch spriteBatch, Vector2 mapLocation)
        {
            spriteBatch.Draw(Texture, mapLocation, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);  
        }

        public void Move(Vector2 movement, Map map)
        {
            if (!map.IsOverMapEdge(Position + movement))
            {
                var targetTilePosition = map.GetTile(Position + movement);

                if (targetTilePosition.Object != null)
                {
                    //Something there already - attack
                    CombatManager.Attack(this, targetTilePosition.Object);
                }
                else
                {
                    //Move and update tile object
                    OldPosition = Position;
                    Position += movement;
                    map.SetTileObject(this);
                }
            }
        }
    }
}
