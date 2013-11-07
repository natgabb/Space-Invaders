using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    /// <summary>
    /// This class describes a MotherShip with a height, a width, a position
    /// and a speed within a screen that has a specified width and height.
    /// </summary>
    class MotherShip
    {
        private Vector2 position;
        private int shipWidth;
        private int shipHeight;
        private int screenWidth;

        /// <summary>
        /// Returns and sets the ships's life status.
        /// </summary>
        public bool Alive
        {
            get;
            set;
        }

        /// <summary>
        /// Property for the ship's boundaries.
        /// Returns a rectangle containing the alien.
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return (new Rectangle((int)position.X, (int)position.Y,
                        shipWidth, shipHeight));
            }
        }

        /// <summary>
        /// Property for the position. Returns and sets the ships's upper
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
        /// Property for the speed of the ship.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Alien's constructor. It creates a MotherShip with the specified
        /// dimensions and position.
        /// </summary>
        /// <param name="speed">The speed at which the ship moves.</param>
        /// <param name="positionX">The X position of the alien
        ///                         on the screen.</param>
        /// <param name="positionY">The Y position of the ship
        ///                         on the screen.</param>
        /// <param name="alienWidth">The width of the ship.</param>
        /// <param name="shipHeight">The height of the ship.</param>
        /// <param name="screenWidth">The width of the screen.</param>
        public MotherShip(int positionX, int positionY, int shipWidth,
                     int shipHeight, int screenWidth, float speed)
        {
            position = new Vector2(positionX, positionY);
            this.shipHeight = shipHeight;
            this.shipWidth = shipWidth;
            this.screenWidth = screenWidth;
            this.Speed = speed;
            Alive = true;
        }

        /// <summary>
        /// Returns a boolean specifying if the ship moved out of the screen.
        /// </summary>
        /// <param name="dir">The direction to check.</param>
        /// <returns>True if it moved out, false if it did not.</returns>
        public bool MovedOut(Direction dir)
        {
            bool outside = false;
            if (Alive)
            {
                switch (dir)
                {
                    case Direction.RIGHT:
                        if (position.X >= screenWidth)
                            outside = true;
                        break;
                    case Direction.LEFT:
                        if (position.X + shipWidth <= 0)
                            outside = true;
                        break;
                }
            }
            return outside;
        }

        /// <summary>
        /// Moves the ship in the specified direction.
        /// </summary>
        /// <param name="dir">The direction to move to.</param>
        public void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.RIGHT:
                    position.X += Speed;
                    break;
                case Direction.LEFT:
                    position.X -= Speed;
                    break;
                case Direction.DOWN:
                    throw new ArgumentException("The MotherShip cannot go down.");
            }
        }
    }
}
