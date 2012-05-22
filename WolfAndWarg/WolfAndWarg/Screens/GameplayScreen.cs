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
using WolfAndWarg;
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

        //TODO Remove content and spritefont
        ContentManager content;
        

        //Player player1 = new Player();
        //Mob enemy1 = new Mob();
        //Map map;
        private Session session;

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
            session = new Session(ScreenManager);

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
            session.Unload();
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
                //In future though this will be needed to manage animations, and other useful inter-turn stuff
            }
        }

        private static KeyboardState previousKeyboardState;
        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            
            // Look up inputs for the active player profile.
            if (ControllingPlayer != null)
            {
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

                    //TODO Create Input helper/manager to manage key presses better
                    if (keyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
                        movement.X--;
                    
                    if (keyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
                        movement.X++;

                    if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
                        movement.Y--;

                    if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
                        movement.Y++;

                    Vector2 thumbstick = gamePadState.ThumbSticks.Left;

                    movement.X += thumbstick.X;
                    movement.Y -= thumbstick.Y;

                    if (movement.Length() > 1)
                    {
                        movement.Normalize();
                    }

                    //If movement or spacebar pressed then update positions
                    //Spacebar means skip turn
                    if(movement.Length() > 0 || (keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space)))
                    {
                        session.Move(ControllingPlayer, movement);
                    
                    }

                }
                previousKeyboardState = keyboardState;
            }
        }



        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Set background colour to White
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                              Color.Black, 0, 0);
            session.Draw();

            // If the game is transitioning on or off, fade it out to black.);
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
