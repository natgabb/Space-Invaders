using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    /// <summary>
    /// This defines a Projectile which has a position, velocity, width and height.
    /// </summary>
    class Projectile
    {
        private Vector2 position;
        private float velocity;
        private int projectileWidth;
        private int projectileHeight;

        /// <summary>
        /// Returns the projectile's current boundary.
        /// </summary>
        public Rectangle Boundary
        {
            get 
            { 
                return (new Rectangle((int)position.X, 
                    (int)position.Y, projectileWidth, projectileHeight));
            }
        }

        /// <summary>
        /// Returns the projectile's position.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectileHeight">The projectile's height.</param>
        /// <param name="projectileWidth">The projectile's width.</param>
        /// <param name="positionX">The projectile's starting X position.</param>
        /// <param name="positionY">The projectile's starting Y position.</param>
        /// <param name="velocity">The projectile's velocity.</param>
        public Projectile(int projectileHeight, int projectileWidth, 
            int positionX, int positionY, float velocity)
        {
            this.projectileHeight = projectileHeight;
            this.projectileWidth = projectileWidth;
            position = new Vector2();
            position.X = positionX;
            position.Y = positionY;
            this.velocity = velocity;
        }

        /// <summary>
        /// This changes the projectile's position by the velocity.
        /// </summary>
        public void Move()
        {
            position.Y += velocity;
        }
    }
}
