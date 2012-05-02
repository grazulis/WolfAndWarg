using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WolfAndWarg.Game
{
    public class Mob : ISprite
    {
        private Random random = new Random(DateTime.Now.Millisecond);
        public string Name { get; set; }
        public bool IsFriendly { get; set; }
        public int Health { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 OldPosition { get; set; }

        public void Move(Vector2 playerPosition, Map map)
        {
            var enemyDirection = Vector2.Lerp(Position, playerPosition, 0.99f);
            var movement = Vector2.Zero;
            
            movement = enemyDirection - Position;
            
            movement.Normalize();

            //round the movements to integers as the tiles positions are currently point-based
            movement.X = (float)Math.Round(movement.X);
            movement.Y = (float)Math.Round(movement.Y);

            //TODO If player is next to mob then attack rather than move!
            if (!map.IsOverMapEdge(Position + movement))
            {
                var targetTilePosition = map.GetTile(Position + movement);
                
                if (targetTilePosition.Object != null)
                {
                    //Something there already - attack!
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
