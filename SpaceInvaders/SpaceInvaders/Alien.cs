using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    /// <summary>
    /// Describes the possible directions an Alien can move.
    /// </summary>
    public enum Direction { LEFT, RIGHT, DOWN };

    /// <summary>
    /// This class describes an Alien with a height, a width, a position
    /// and a speed within a screen that has a specified width and height.
    /// </summary>
    class Alien
    {
        public static float SPEED;
        private Vector2 position;
        private int alienWidth;
        private int alienHeight;
        private int screenWidth;
        private int screenHeight;

        /// <summary>
        /// Returns and sets the alien's life status.
        /// </summary>
        public bool Alive
        {
            get; set;
        }

        /// <summary>
        /// Property for the aliens's boundaries.
        /// Returns a rectangle containing the alien.
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return (new Rectangle((int)position.X, (int)position.Y,
                        alienWidth, alienHeight));
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
                return position; 
            }
            set 
            { 
                position = value; 
            }
        }

        /// <summary>
        /// Alien's constructor. It creates an Alien with the specified
        /// dimensions and position.
        /// </summary>
        /// <param name="speed">The speed at which the alien moves.</param>
        /// <param name="positionX">The X position of the alien
        ///                         on the screen.</param>
        /// <param name="positionY">The Y position of the alien
        ///                         on the screen.</param>
        /// <param name="alienWidth">The width of the alien.</param>
        /// <param name="alienHeight">The height of the alien.</param>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        public Alien(int positionX, int positionY, int alienWidth,
                     int alienHeight, int screenWidth, int screenHeight)
        {
            position = new Vector2(positionX, positionY);
            this.alienHeight = alienHeight;
            this.alienWidth = alienWidth;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            Alive = true;
        }

        /// <summary>
        /// Returns a boolean specifying if the alien can move further right,
        /// or further left.
        /// </summary>
        /// <param name="dir">The direction to check.</param>
        /// <returns>True if it can move, false if it cannot.</returns>
        public bool TryMove(Direction dir)
        {
            bool canMove = true;
            if (Alive)
            {
                switch (dir)
                {
                    case Direction.RIGHT:
                        if (position.X + alienWidth >= screenWidth)
                            canMove = false;
                        break;
                    case Direction.LEFT:
                        if (position.X <= 0)
                            canMove = false;
                        break;
                }
            }
            return canMove;
        }

        /// <summary>
        /// Moves the alien in the specified direction.
        /// </summary>
        /// <param name="dir">The direction to move to.</param>
        public void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.RIGHT:
                    position.X += SPEED;
                    break;
                case Direction.LEFT:
                    position.X -= SPEED;
                    break;
                case Direction.DOWN:
                    position.Y += SPEED;
                    break;
            }
        }

        /// <summary>
        /// Checks to see if the alien has reached the bottom of the screen.
        /// </summary>
        /// <returns>True if it has reached the bottom.</returns>
        public bool HasReachedBottom()
        {
            return (position.Y + alienHeight >= screenHeight - 60);
        }
    }
}