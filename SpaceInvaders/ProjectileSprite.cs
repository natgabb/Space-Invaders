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
    public class ProjectileSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game game;
        private Projectile projectile;
        private Texture2D imageProjectile;
        private int positionX;
        private int positionY;
        private float velocity;
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The projectile's position.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return projectile.Position;
            }
        }

        /// <summary>
        /// The projectile's current boundary.
        /// </summary>
        public Rectangle Boundary
        {
            get { return projectile.Boundary; }
        }

        /// <summary>
        /// The damage done by this projectile.
        /// </summary>
        public int Damage
        { get; set; }

        /// <summary>
        /// ProjectileSprite's constructor.
        /// </summary>
        /// <param name="game">The game it is attached to.</param>
        /// <param name="imageProjectile">The projectile's image.</param>
        /// <param name="positionX">The projectile's starting X position.</param>
        /// <param name="positionY">The projectile's starting Y position.</param>
        /// <param name="velocity">The projectile's velocity.</param>
        public ProjectileSprite(Game game, Texture2D imageProjectile, int positionX,
            int positionY, float velocity, int damage)
            : base(game)
        {
            this.game = game;
            this.imageProjectile = imageProjectile;
            this.positionX = positionX;
            this.positionY = positionY;
            this.velocity = velocity;
            this.Damage = damage;
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
            projectile.Move();

            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the projectile.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(imageProjectile, projectile.Position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Loads the projectile.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            projectile = new Projectile(imageProjectile.Height, imageProjectile.Width,
                positionX, positionY, velocity);
            base.LoadContent();
        }
    }
}
