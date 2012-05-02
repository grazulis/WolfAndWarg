using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using WolfAndWarg.Game;

namespace WolfAndWarg
{
    public class Session
    {
        public Dictionary<int, Player> Players = new Dictionary<int, Player>();
        public Dictionary<int, Mob> Mobs = new Dictionary<int, Mob>();
        public Map Map;
        public string SessionState;

        public void Move(int playerId)
        {
            
        }

        public void Draw(List<SpriteBatch> spriteBatchs)
        {
            
        }



    }
}