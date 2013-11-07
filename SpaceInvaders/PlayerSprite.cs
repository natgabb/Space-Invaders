using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SpaceInvaders
{
    /// <summary>
    /// This is a game component that implements IDrawable.
    /// It encapsulates a player.
    /// </summary>
    public class PlayerSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Player player;
        private SpriteBatch spriteBatch;
        private Texture2D imagePlayer;
        private Game1 game;
        private LaserFactory laser;

        /// <summary>
        /// Property for the player's boundaries.
        /// Returns a rectangle containing the player.
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return player.Boundary;
            }
        }

        /// <summary>
        /// PlayerSprite's constructor.
        /// </summary>
        /// <param name="game">The game it is attached to.</param>
        /// <param name="laser">The player's laser.</param>
        public PlayerSprite(Game1 game, LaserFactory laser)
            : base(game)
        {
            this.game = game;
            this.laser = laser;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs
        /// to before starting to run.  This is where it can query for any
        /// required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Checks if the player is pressing the left or right arrow.
            checkInput(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the player on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(imagePlayer, player.Position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Creates the player, its image and the spriteBatch.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            imagePlayer = game.Content.Load<Texture2D>("player");
            player = new Player(imagePlayer.Height, imagePlayer.Width,
                                GraphicsDevice.Viewport.Width,
                                GraphicsDevice.Viewport.Height, 8F);
            base.LoadContent();
        }

        /// <summary>
        /// Checks if the player is pressing the left or right arrow and moves
        /// in the direction pressed.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void checkInput(GameTime gameTime)
        {
            // Moves the player left or right.
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(Keys.Right))
                player.MoveRight();
            else if (newState.IsKeyDown(Keys.Left))
                player.MoveLeft();

            // Makes the player shoot.
            if (newState.IsKeyDown(Keys.Space))
                laser.Launch(player.Boundary, gameTime);
        }
    }
}
