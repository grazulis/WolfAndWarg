using System;
using System.Collections.Generic;
using System.Linq;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WolfAndWarg.Game;

namespace WolfAndWarg
{
    public class Session
    {
        public Session(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            if (content == null)
                content = new ContentManager(screenManager.Game.Services, "Content");
            players.Add(0, new Player{Health = 10, Defence = 3, Position = new Vector2(5, 5), Texture = content.Load<Texture2D>("player")});
            mobs.Add(1, new Mob { Health = 10, Defence = 3, IsFriendly = false, Position = new Vector2(0, 0), Texture = content.Load<Texture2D>("enemymob") });
            mobs.Add(2, new Mob { Health = 10, Defence = 3, IsFriendly = false, Position = new Vector2(15, 10), Texture = content.Load<Texture2D>("enemymob") });
            mobs.Add(3, new Mob { Health = 10, Defence = 3, IsFriendly = true, Position = new Vector2(12, 10), Texture = content.Load<Texture2D>("friendlymob") });
            map = new Map(screenManager, content);
            tileManager = new TileManager { Map = map, Viewport = screenManager.GraphicsDevice.Viewport };

            gameFont = content.Load<SpriteFont>("gamefont");
        }

        SpriteFont gameFont;
        private ContentManager content;
        private ScreenManager screenManager;

        Dictionary<int, Player> players = new Dictionary<int, Player>();
        public Dictionary<int, Mob> mobs = new Dictionary<int, Mob>();
        public Map map;
        public string SessionState;
        public TileManager tileManager;

        public void Move(PlayerIndex? controllingPlayer, Vector2 movement)
        {
            var playerId = (int) controllingPlayer.Value;
            players[playerId].Move(movement, map);
            foreach (var mob in mobs)
            {
                mob.Value.Move(players[playerId].Position, mobs.Values.ToList(),  map);
                if (mob.Value.Health <= 0) mob.Value.Texture = content.Load<Texture2D>("enemymobdeceased");
            }
            checkIfGameOver(controllingPlayer);
        }

        public void Draw()
        {
            SpriteBatch spriteBatch = screenManager.SpriteBatch;
            spriteBatch.Begin();
            string hud = "";
            //map.Draw(spriteBatch);
            tileManager.PlayerPosition = players[0].Position;
            tileManager.Update();
            tileManager.DrawLayers(spriteBatch);

            foreach (var player in players)
            {
                spriteBatch.Draw(player.Value.Texture, map.GetSpritePosition(player.Value), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                hud += string.Format("Player {0} : {1},", player.Key, player.Value.Health);
            }

            foreach (var mob in mobs)
            {
                spriteBatch.Draw(mob.Value.Texture, map.GetSpritePosition(mob.Value), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                hud += string.Format("{2} {0} : {1}, ", mob.Key, mob.Value.Health, mob.Value.IsFriendly ? "Wolf": "Warg");
            }
            

            spriteBatch.DrawString(gameFont, hud, new Vector2(10, 10)
                                   , Color.DarkRed);

            spriteBatch.End();
        }

        private void checkIfGameOver(PlayerIndex? controllingPlayer)
        {
            int deadMobCount = mobs.Count(mob => mob.Value.Health <= 0 && mob.Value.IsFriendly == false);
            if (controllingPlayer != null)
            {
                if(players[(int)controllingPlayer.Value].Health <= 0)
                {

                    screenManager.AddScreen(
                        new GameOverScreen(
                            string.Format("Game Over! Player {0} Lost!", controllingPlayer.Value )), controllingPlayer
                        
                        );
                }

                if(deadMobCount >= mobs.Count(mob => mob.Value.IsFriendly == false))
                {
                    screenManager.AddScreen(
                        new GameOverScreen(
                            "Game Over! Wargs Lost!"), controllingPlayer

                        );
                }
            }
        }

        public void Unload()
        {
            content.Unload();
        }

    }
}