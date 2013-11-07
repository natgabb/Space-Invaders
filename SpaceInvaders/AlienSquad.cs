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
    public delegate void ChangeWave(int wave);

    /// <summary>
    /// This is a game component that implements IDrawable.
    /// It encapsulates a squad of Aliens.
    /// </summary>
    public class AlienSquad : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game1 game;
        private Direction currentDir = Direction.LEFT;
        private AlienSprite[,] aliens = new AlienSprite[6, 10];
        private BombFactory bomb;
        private Random alienBombing = new Random();

        private int numberOfHits = 0;
        private int waves = 1;
        private static readonly Vector2 STARTING_POSITION = new Vector2(90,90);
        private const float STARTING_SPEED = 2;

        public static event GameIsOver GameOver;
        public static event ChangeWave NewWave;

        /// <summary>
        /// The number of aliens in the squad.
        /// </summary>
        public int Length
        {
            get
            {
                return aliens.Length;
            }
        }

        /// <summary>
        /// AlienSquad's constructor.
        /// </summary>
        /// <param name="game">The game it is attached to.</param>
        /// <param name="bomb">The bombs dropped by the aliens.</param>
        public AlienSquad(Game1 game, BombFactory bomb, LaserFactory laser)
            : base(game)
        {
            this.game = game;
            this.bomb = bomb;
            laser.AlienKilled += onAlienHit;
        }

        /// <summary>
        /// Indexer for the aliens in the squad.
        /// </summary>
        /// <param name="x">The index of the alien.</param>
        /// <returns>The alien at the index.</returns>
        public AlienSprite this[int x]
        {
            get
            {
                int length = aliens.GetLength(1);
                return aliens[x / length, x % length];
            }
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
        /// Moves the AlienSquad on the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            bool canMove = false;
            Direction dir = currentDir;

            // Verifies if it can move in the current direction.
            switch (currentDir)
            {
                case Direction.RIGHT:
                    for (int a = 0; a < aliens.GetLength(0); a++)
                        for (int b = aliens.GetLength(1) - 1; b >= 0; b--)
                        {
                            canMove = aliens[a, b].TryMove(currentDir);
                            if (!canMove)
                            {
                                b = 0;
                                a = aliens.GetLength(0);
                            }
                        }
                    break;

                case Direction.LEFT:
                    for (int a = 0; a < aliens.GetLength(0); a++)
                        for (int b = 0; b < aliens.GetLength(1); b++)
                        {
                            canMove = aliens[a, b].TryMove(currentDir);
                            if (!canMove)
                            {
                                b = aliens.GetLength(1);
                                a = aliens.GetLength(0);
                            }
                        }
                    break;
            }

            // If it can move further in the current direction, it will go down
            // once and then go in the opposite direction.
            if (!canMove)
            {
                dir = Direction.DOWN;
                if (currentDir == Direction.LEFT)
                    currentDir = Direction.RIGHT;
                else
                    currentDir = Direction.LEFT;
            }

            // Moves the AlienSquad in the specified direction.
            for (int a = 0; a < aliens.GetLength(0); a++)
                for (int b = 0; b < aliens.GetLength(1); b++)
                {
                    aliens[a, b].Move(dir);
                    if (dir == Direction.DOWN)
                        if (aliens[a,b].HasReachedBottom() && aliens[a,b].Alive)
                            if (GameOver != null)
                                GameOver();
                }

            generateProjectile(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the Aliens on the screen, in their new position.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
                int notAlive = 0;

                for (int a = 0; a < aliens.GetLength(0); a++)
                    for (int b = 0; b < aliens.GetLength(1); b++)
                        if (aliens[a, b].Alive)
                            aliens[a, b].Draw(gameTime);
                        else
                            notAlive++;

                // Checks if all the aliens are dead.
                if (notAlive == aliens.Length)
                    newWave();
 
            base.Draw(gameTime);
        }

        /// <summary>
        /// Loads the images and creates the Aliens for the AlienSquad.
        /// </summary>
        protected override void LoadContent()
        {
            int basePoint = 60;

            Alien.SPEED = STARTING_SPEED;

            // Holds the images for the aliens.
            Texture2D[] images = new Texture2D[3];
            images[0] = game.Content.Load<Texture2D>("flyingsaucer1");
            images[1] = game.Content.Load<Texture2D>("bug1");
            images[2] = game.Content.Load<Texture2D>("satellite1");

            // Holds the distance between two aliens of the same type.
            int height = images[0].Height + images[0].Height / 4;

            // Creates the rows of Aliens.
            int twoAliens = images[0].Width * 2;
            int startingPosition = (int)STARTING_POSITION.X;
            int positionX;
            int positionY = (int)STARTING_POSITION.Y;

            for (int a = 0; a < aliens.GetLength(0); a ++)
            {
                int i = a % images.Length;
                positionX = startingPosition;
                for (int b = 0; b < aliens.GetLength(1); b++)
                {
                    aliens[a, b] = new AlienSprite(game, images[i],
                                                   positionX, positionY, basePoint);
                    aliens[a, b].Initialize();
                    positionX += twoAliens;
                }
                positionY += height;
                basePoint -= 10;
            }

            base.LoadContent();
        }

        /// <summary>
        /// This selects at random an alien and then lauches a projectile from its
        /// position if it is alive.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        private void generateProjectile(GameTime gameTime)
        {
            int alien = alienBombing.Next(0, aliens.Length - 1);
            if(this[alien].Alive)
                bomb.Launch(this[alien].Boundary, gameTime);
        }

        /// <summary>
        /// This resets the alien's positions and status for a new wave.
        /// </summary>
        private void newWave()
        {
            Vector2 changePosition = aliens[0,0].Position - STARTING_POSITION;

            // Makes the aliens start progressively lower, for a maximum of 110px.
            if(waves <= 11)
                changePosition.Y -= waves * 10;
            else
                changePosition.Y -= 110;

            for (int a = 0; a < aliens.GetLength(0); a++)
                for (int b = 0; b < aliens.GetLength(1); b++)
                {
                    aliens[a, b].Alive = true;
                    aliens[a, b].Position -= changePosition;
                    aliens[a, b].Point += 2;
                }

            Alien.SPEED = STARTING_SPEED + waves * 0.1F;

            waves += 1;

            if(NewWave != null)
                NewWave(waves);
        }

        /// <summary>
        /// This method kills an alien that was hit.
        /// </summary>
        /// <param name="alien">The alien that was hit.</param>
        /// <param name="projectile">The projectile that hit the alien.</param>
        private void onAlienHit(DrawableGameComponent alien,
            ProjectileSprite projectile)
        {
            (alien as AlienSprite).Alive = false;
            incrementAlienSpeed();
        }

        /// <summary>
        /// This increments the aliens' speed everytime 3 aliens die.
        /// </summary>
        /// <param name="alien">The alien that was hit.</param>
        /// <param name="projectile">The projectile that hit.</param>
        private void incrementAlienSpeed()
        {
            numberOfHits += 1;

            if (numberOfHits == 3)
            {
                Alien.SPEED += 0.10F;
                numberOfHits = 0;
            }
        }
    }
}
