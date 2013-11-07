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
    /// It encapsulates an alien.
    /// </summary>
    public class AlienSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Alien alien;
        private SpriteBatch spriteBatch;
        private Texture2D imageAlien;
        private Game1 game;

        private int positionX;
        private int positionY;

        /// <summary>
        /// Property for the alien's boundaries.
        /// Returns a rectangle containing the alien.
        /// </summary>
        public Rectangle Boundary
        {
            get { return alien.Boundary; }
        }

        /// <summary>
        /// Returns the alien's life status.
        /// </summary>
        public bool Alive
        {
            get
            {
                return alien.Alive;
            }
            set
            {
                alien.Alive = value;
            }
        }

        /// <summary>
        /// Property for the position. Returns and sets the alien's upper
        /// left corner value.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return alien.Position;
            }
            set
            {
                alien.Position = value;
            }
        }

        /// <summary>
        /// This is the number of points for the score associated with the alien.
        /// </summary>
        public int Point { get; set; }

        /// <summary>
        /// AlienSprite's constructor. Creates an alien with an image,
        /// starting at the specified position.
        /// </summary>
        /// <param name="game">The game it is attached to.</param>
        /// <param name="imageAlien">The image representing the alien.</param>
        /// <param name="positionX">The X position of the alien.</param>
        /// <param name="positionY">The Y position of the alien.</param>
        public AlienSprite(Game1 game, Texture2D imageAlien, int positionX,
                           int positionY, int point)
            : base(game)
        {
            this.game = game;
            this.imageAlien = imageAlien;
            this.positionX = positionX;
            this.positionY = positionY;
            this.Point = point;
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
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the alien on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(imageAlien, alien.Position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Creates the spriteBatch and the alien.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            alien = new Alien(positionX, positionY, imageAlien.Width,
                              imageAlien.Height, GraphicsDevice.Viewport.Width,
                              GraphicsDevice.Viewport.Height);
            base.LoadContent();
        }

        /// <summary>
        /// Verifies if the alien can move in the specified direction.
        /// (Direction.LEFT or Direction.RIGHT)
        /// </summary>
        /// <param name="dir">The direction to check.</param>
        /// <returns>True if the alien can move, false otherwise.</returns>
        public bool TryMove(Direction dir)
        {
            return alien.TryMove(dir);
        }

        /// <summary>
        /// Moves the alien in the specified direction.
        /// </summary>
        /// <param name="dir">The direction to move the alien.</param>
        public void Move(Direction dir)
        {
            alien.Move(dir);
        }

        /// <summary>
        /// Checks if the alien has reached the bottom.
        /// </summary>
        /// <returns>Whether the alien has reached the bottom.</returns>
        public bool HasReachedBottom()
        {
            return alien.HasReachedBottom();
        }
    }
}
