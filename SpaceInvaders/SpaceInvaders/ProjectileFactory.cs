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
    public abstract class ProjectileFactory : 
        Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game1 game;
        private List<ProjectileSprite> projectiles = new List<ProjectileSprite>();

        /// <summary>
        /// The number of projectiles.
        /// </summary>
        protected int Length
        {
            get
            {
                return projectiles.Count;
            }
        }

        /// <summary>
        /// Indexer for the projectiles.
        /// </summary>
        /// <param name="x">The index</param>
        /// <returns>The projectile at the specified index.</returns>
        protected ProjectileSprite this[int x]
        {
            get
            {
                return projectiles[x];
            }
        }

        /// <summary>
        /// ProjectileFactory's constructor.
        /// </summary>
        /// <param name="game"></param>
        public ProjectileFactory(Game1 game)
            : base(game)
        {
            this.game = game;
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
            for (int i = 0; i < projectiles.Count; i++)
                if (projectiles[i].Position.Y < 0 ||
                    projectiles[i].Position.Y > GraphicsDevice.Viewport.Height)
                    projectiles.RemoveAt(i);
                else
                    projectiles[i].Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the all the projectiles.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var projectile in projectiles)
                projectile.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// This loads the content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// This is the abstract signature for launching a projectile.
        /// </summary>
        /// <param name="boundary">Where the projectile will be launched.</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public abstract void Launch(Rectangle boundary, GameTime gameTime);

        /// <summary>
        /// This adds a projectile.
        /// </summary>
        /// <param name="positionX">The projectile's X position.</param>
        /// <param name="positionY">The projectile's Y position.</param>
        /// <param name="velocity">The projectile's velocity.</param>
        /// <param name="imageProjectile">The projectile's texture.</param>
        protected void AddProjectile(int positionX, int positionY,
            float velocity, Texture2D imageProjectile, int damage)
        {
                ProjectileSprite projectile = new ProjectileSprite(game,
                    imageProjectile, positionX - imageProjectile.Width / 2,
                    positionY - imageProjectile.Height, velocity, damage);
                projectile.Initialize();
                projectiles.Add(projectile);
        }

        /// <summary>
        /// Removes projectiles that have left the screen.
        /// </summary>
        protected void RemoveOutsideProjectiles()
        {
            for (int i = 0; i < Length; i++)
                if (this[i].Boundary.Y < 0 || 
                    this[i].Boundary.Y > game.GraphicsDevice.Viewport.Height)
                {
                    projectiles[i].Dispose();
                    projectiles.RemoveAt(i);
                }
        }

        /// <summary>
        /// Abstract method for adding an opponent.
        /// </summary>
        /// <param name="component"></param>
        public abstract void AddOpponent(DrawableGameComponent component);

        /// <summary>
        /// This removes a projectile that has hit something.
        /// </summary>
        /// <param name="component">The component it has hit.</param>
        /// <param name="projectile">The projectile.</param>
        protected void projectileHit(ProjectileSprite projectile)
        {
            projectiles.Remove(projectile);
            projectile.Dispose();
        }

        /// <summary>
        /// Clears all projectiles.
        /// </summary>
        protected void resetProjectiles()
        {
            foreach (ProjectileSprite p in projectiles)
                p.Dispose();
            projectiles = new List<ProjectileSprite>();
        }

        /// <summary>
        /// Resets the projectiles for the new wave.
        /// </summary>
        /// <param name="wave"></param>
        private void onNewWave(int wave)
        {
            resetProjectiles();
        }

    }
}
