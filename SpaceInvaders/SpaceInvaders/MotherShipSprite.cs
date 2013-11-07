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
    /// </summary>
    public class MotherShipSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private MotherShip ship;
        private SpriteBatch spriteBatch;
        private Texture2D imageShip;
        private Game1 game;
        private BombFactory bomb;

        private Direction currentDir = Direction.RIGHT;
        private TimeSpan lastAppearance = new TimeSpan(0, 0, 0, 0);
        private TimeSpan betweenAppearances;
        private Vector2[] startingPositions = new Vector2[2];

        private Random randomize = new Random();

        /// <summary>
        /// Property for the ship's boundaries.
        /// Returns a rectangle containing the ship.
        /// </summary>
        public Rectangle Boundary
        {
            get { return ship.Boundary; }
        }

        /// <summary>
        /// Returns the alien's life status.
        /// </summary>
        public bool Alive
        {
            get
            {
                return ship.Alive;
            }
            set
            {
                ship.Alive = value;
            }
        }

        /// <summary>
        /// Property for the position. Returns and sets the ship's upper
        /// left corner value.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return ship.Position;
            }
            set
            {
                ship.Position = value;
            }
        }

        /// <summary>
        /// This is the number of points for the score associated with the ship.
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
        public MotherShipSprite(Game1 game,BombFactory bomb, LaserFactory laser)
            : base(game)
        {
            this.game = game;
            this.bomb = bomb;
            this.Point = 100;
            laser.MotherShipHit += onShipHit;
            AlienSquad.NewWave += onNewWave;
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
            if (Alive)
            {
                // Will wait a certain time before bringing the ship back on screen.
                if (ship.MovedOut(currentDir))
                {
                    betweenAppearances = new TimeSpan(0, 0, 0, randomize.Next(2, 10));
                    if (gameTime.TotalGameTime - lastAppearance > betweenAppearances)
                    {
                        if (currentDir == Direction.RIGHT)
                            currentDir = Direction.LEFT;
                        else
                            currentDir = Direction.RIGHT;

                        ship.Move(currentDir);
                    }
                }

                else
                {
                    ship.Move(currentDir);
                    lastAppearance = gameTime.TotalGameTime;
                }

                bomb.Launch(Boundary, gameTime); 
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the ship on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if(Alive)
                spriteBatch.Draw(imageShip, ship.Position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Creates the spriteBatch and the ship.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            imageShip = game.Content.Load<Texture2D>("mothership");

            startingPositions[0] = new Vector2();
            startingPositions[1] = new Vector2();

            startingPositions[0].X = GraphicsDevice.Viewport.Width + 1;
            startingPositions[0].Y = 40;
            startingPositions[1].X = 0 - imageShip.Width;
            startingPositions[1].Y = 40;

            ship = new MotherShip((int)setDirection().X, (int)startingPositions[0].Y, 
                imageShip.Width, imageShip.Height, GraphicsDevice.Viewport.Width, 3);
            base.LoadContent();
        }

        /// <summary>
        /// This method is called when the ship gets hit by a player's projectile.
        /// </summary>
        /// <param name="ship">The ship that was hit.</param>
        /// <param name="projectile">The projectile that hit.</param>
        private void onShipHit(DrawableGameComponent ship, ProjectileSprite projectile)
        {
            (ship as MotherShipSprite).Alive = false;
        }

        /// <summary>
        /// This method is called when there is a new wave of aliens.
        /// Its Alive property is set to true, and its points are incremented by
        /// 100 * wave.
        /// </summary>
        private void onNewWave(int wave)
        {
            Point += 100 * wave;
            this.Alive = true;
            if(wave <= 14)
                ship.Speed += 0.1F * wave;

            ship.Position = setDirection();
        }

        /// <summary>
        /// Randomizes the ship's starting position.
        /// </summary>
        /// <returns>The ship's starting position</returns>
        private Vector2 setDirection()
        {
            int startingPosition = randomize.Next(0, 2);

            switch (startingPosition)
            {
                case 0:
                    currentDir = Direction.RIGHT;
                    break;
                case 1:
                    currentDir = Direction.LEFT;
                    break;
            }

            return startingPositions[startingPosition];
        }
    }
}
