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

        public int Defence { get; set; }

        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 OldPosition { get; set; }

        public void Move(Vector2 playerPosition, List<Mob> mobs, Map map)
        {
            //If dead then stay still
            if (Health <= 0) return;

           
            var movement = selectTarget(playerPosition, mobs);

            //round the movements to integers as the tiles positions are currently point-based
            movement.X = (float)Math.Round(movement.X);
            movement.Y = (float)Math.Round(movement.Y);

            if (Math.Abs(movement.X) > Math.Abs(movement.Y))
            {
                movement.Y = 0;
            }
            else
            {
                movement.X = 0;
            }

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

        private Vector2 selectTarget(Vector2 playerPosition, List<Mob> mobs)
        {
            //Set target to be self, ie. don't move if no target
            var target = Position;

            if (!IsFriendly)
            {
                target = Vector2.Lerp(Position, playerPosition, 0.99f);
            }

            foreach (var mob in mobs)
            {
                //TODO sort this list out and make so dead mobs are removed from list
                if (mob == this) continue ;
                if (mob.IsFriendly == IsFriendly) continue;
                if (mob.Health == 0 ) continue;

                var mobPosition = Vector2.Lerp(Position, mob.Position, 0.99f);
                var mobDistance = Vector2.Distance(Position, mobPosition);
                var currentTargetDistance = Vector2.Distance(Position, target);
                if(mobDistance < currentTargetDistance || currentTargetDistance == 0 ) target = mobPosition;
            }

            if (target == Position) return new Vector2(0, 0);
            var movement = target - Position;
            movement.Normalize();
            return movement;
        }
    }
}
