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
    public class BombFactory : ProjectileFactory
    {
        private Game1 game;
        private Texture2D imageProjectile;
        private float velocity;
        private int damage;
        private PlayerSprite opponent;

        private TimeSpan lastShot = new TimeSpan(0, 0, 0, 1, 500);
        private readonly TimeSpan BETWEEN_SHOTS = new TimeSpan(0, 0, 0, 1, 500);

        public event Hit PlayerHit;

        /// <summary>
        /// The BombFactory's constructor.
        /// </summary>
        /// <param name="game">The game it is attached to.</param>
        public BombFactory(Game1 game, Texture2D imageProjectile, int damage,
                           float velocity) : base(game)
        {
            this.game = game;
            this.imageProjectile = imageProjectile;
            AlienSquad.NewWave += onNewWave;
            this.damage = damage;
            this.velocity = velocity;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to
        /// before starting to run.  This is where it can query for any required
        /// services and load content.
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
        /// Loads the image for the projectile.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// This method creates a new projectile coming from an alien.
        /// </summary>
        /// <param name="boundary">Where the projectile will be created.</param>
        /// <param name="gameTime">The game's running time.</param>
        public override void Launch(Rectangle boundary, GameTime gameTime)
        {
            if (gameTime.TotalGameTime - lastShot > BETWEEN_SHOTS)
            {
                AddProjectile(boundary.X + (boundary.Width / 2),
                    boundary.Y + boundary.Height, velocity, imageProjectile, damage);
                lastShot = gameTime.TotalGameTime;
            }
        }

        /// <summary>
        /// This adds the alien's opponent.
        /// </summary>
        /// <param name="component">The player</param>
        /// <exception cref="ArgumentException">Thrown if the component is not a
        /// PlayerSprite.</exception>
        public override void AddOpponent(DrawableGameComponent component)
        {
            if (component is PlayerSprite)
                opponent = component as PlayerSprite;
            else
                throw new ArgumentException
                    ("The opponent must be of PlayerSprite type.");       
        }

        /// <summary>
        /// Checks to see if any of the projectiles have hit the player.
        /// </summary>
        private void checkHit()
        {
            for (int i = 0; i < base.Length; i++)
            {
                if (base[i].Boundary.Intersects(opponent.Boundary))
                {
                    if (PlayerHit != null)
                        PlayerHit(opponent, base[i]);
                    projectileHit(base[i]);
                }
            }
            base.RemoveOutsideProjectiles();
        }

        /// <summary>
        /// Called with every new wave. It increases the aliens' bomb velocity and
        /// clears all current projectiles.
        /// </summary>
        /// <param name="wave"></param>
        private void onNewWave(int wave)
        {
            velocity += wave * 0.05F;
        }
    }
}
