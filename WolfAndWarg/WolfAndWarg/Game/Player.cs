﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WolfAndWarg.Game
{
    class Player : ISprite
    {
        
        public int Health { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

        public void Move(Vector2 movement, Map map)
        {
            if (!map.IsOverMapEdge(Position + movement))
            {
                var targetTilePosition = map.GetTile(Position + movement);

                if (targetTilePosition.Object != null)
                {
                    //Something there already - 
                }
                else
                {
                    //TODO Set tile object
                    Position += movement;
                }
            }
        }
    }
}
