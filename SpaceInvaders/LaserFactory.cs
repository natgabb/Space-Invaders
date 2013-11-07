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
    /// This is a game component that extends ProjectileFactory.
    /// </summary>
    public class LaserFactory : ProjectileFactory
    {
        private Game1 game;
        private Texture2D imageProjectile;
        private int velocity = -5;
        private int damage = 1;

        private TimeSpan lastShot = new TimeSpan(0,0,0);
        private readonly TimeSpan BETWEEN_SHOTS = new TimeSpan(0, 0, 0, 0, 350);

        private AlienSquad opponent;
        private MotherShipSprite motherShip;

        public event Hit AlienKilled;
        public event Hit MotherShipHit;

        /// <summary>
        /// LaserFactory's constructor.
        /// </summary>
        /// <param name="game">The game it is attached to.</param>
        public LaserFactory(Game1 game)
            : base(game)
        {
            this.game = game;
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
            checkHit();
            base.Update(gameTime);
        }

        /// <summary>
        /// This loads the projectile's image.
        /// </summary>
        protected override void LoadContent()
        {
            imageProjectile = game.Content.Load<Texture2D>("laser1");
            base.LoadContent();
        }

        /// <summary>
        /// This creates a new projectile at the player's position.
        /// </summary>
        /// <param name="boundary">The player's current boundary.</param>
        /// <param name="gameTime">The game's running time.</param>
        public override void Launch(Rectangle boundary, GameTime gameTime)
        {
            if (gameTime.TotalGameTime - lastShot > BETWEEN_SHOTS)
            {
                AddProjectile(boundary.X + (boundary.Width / 2),
                    boundary.Y, velocity, imageProjectile, damage);
                lastShot = gameTime.TotalGameTime;
            }
        }

        /// <summary>
        /// This adds the player's opponents.
        /// </summary>
        /// <param name="component">The opponent to be added.</param>
        public override void AddOpponent(DrawableGameComponent component)
        {

            if (component is AlienSquad)
                opponent = component as AlienSquad;
            else if (component is MotherShipSprite)
                motherShip = component as MotherShipSprite;
            else
                throw new ArgumentException
                    ("The opponent must be of AlienSquad or MotherShipSprite type.");
        }

        /// <summary>
        /// This checks to see if any of the player's projectiles has hit any of the
        /// active aliens or the mothership.
        /// </summary>
        private void checkHit()
        {
            for (int i = 0; i < base.Length; i++)
            {
                int alien = 0;

                if (base[i].Boundary.Intersects(motherShip.Boundary))
                {
                    if (motherShip.Alive)
                    {
                        if (MotherShipHit != null)
                            MotherShipHit(motherShip, base[i]);
                        projectileHit(base[i]);
                        alien = opponent.Length;
                    }
                }

                while (alien < opponent.Length)
                {
                    if (base[i].Boundary.Intersects(opponent[alien].Boundary))
                        if (opponent[alien].Alive)
                        {
                            if (AlienKilled != null)
                                AlienKilled(opponent[alien], base[i]);
                            projectileHit(base[i]);
                            alien = opponent.Length;
                        }
                    alien++;
                }
            }

            base.RemoveOutsideProjectiles();
        }
    }
}
