#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WolfAndWarg.Game;

#endregion

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        //Vector2 playerPosition = new Vector2(100, 100);
        Vector2 enemyPosition = new Vector2(100, 100);

        Player player1 = new Player();
        Mob enemy1 = new Mob();
        Map map = new Map();

        Random random = new Random();

        float pauseAlpha;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            player1.Health = 10;
            player1.Texture = this.content.Load<Texture2D>("player");
            var playerStartPosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width/2 - 10,
                                                  ScreenManager.GraphicsDevice.Viewport.Height/2 - 10);
            player1.Position = playerStartPosition;

            enemy1.Health = 10;
            enemy1.IsFriendly = false;
            enemy1.Texture = this.content.Load<Texture2D>("enemymob");
            int x = 0;
            int y = 0;
            int tileWidth = 64;
            var numberOfTiles = ScreenManager.GraphicsDevice.Viewport.Width/tileWidth *
                                (ScreenManager.GraphicsDevice.Viewport.Height + 64 )/tileWidth;
            map.Tiles = new Tile[numberOfTiles];
            for (int i = 0; i < numberOfTiles; i++)
            {
                map.Tiles[i] = new Tile { ScreenPosition = new Vector2(x, y), Texture = this.content.Load<Texture2D>("Tile") };
                x += tileWidth;
                if(x >= ScreenManager.GraphicsDevice.Viewport.Width)
                {
                    x = 0;
                    y += tileWidth;
                }
            }

            gameFont = content.Load<SpriteFont>("gamefont");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(10);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                //As this game is turn based we will only currently do stuff when the player moves


                //// Apply some random jitter to make the enemy move around.
                //const float randomization = 10;

                //enemyPosition.X += (float)(random.NextDouble() - 0.5) * randomization;
                //enemyPosition.Y += (float)(random.NextDouble() - 0.5) * randomization;

                //// Apply a stabilizing force to stop the enemy moving off the screen.
                //Vector2 targetPosition = new Vector2(
                //    ScreenManager.GraphicsDevice.Viewport.Width / 2 , 
                //    200);

                //enemy1.Position = Vector2.Lerp(enemyPosition, targetPosition, 0.05f);

                //// TODO: this game isn't very fun! You could probably improve
                //// it by inserting something more interesting in this space :-)
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                Vector2 movement = Vector2.Zero;


                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    movement.X--;
                }

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                Vector2 thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                {
                    movement.Normalize();
                    
                }


                if(movement.Length() > 0)
                {
                    player1.Move(movement);
                    enemy1.Move(player1.Position);
                    
                }
                

                
            }
        }



        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Set background colour to White
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.White, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            foreach (var tile in map.Tiles)
            {
                spriteBatch.Draw(tile.Texture, tile.ScreenPosition, Color.White);
            }

            spriteBatch.Draw(player1.Texture, player1.Position, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0 );

            spriteBatch.Draw(enemy1.Texture, enemy1.Position, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

            //spriteBatch.DrawString(gameFont, "Insert Gameplay Here",
            //                       enemyPosition, Color.DarkRed);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
