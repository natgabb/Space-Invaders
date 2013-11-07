using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    /// <summary>
    /// This class describes a Player with a height, a width, a speed,
    /// and a position within a screen that has a specified width.
    /// </summary>
    class Player
    {
        private readonly float SPEED;
        private int screenWidth;

        private int playerHeight;
        private int playerWidth;
        private Vector2 position;

        /// <summary>
        /// Property for the player's boundaries.
        /// Returns a rectangle containing the player.
        /// </summary>
        public Rectangle Boundary
        {
            get
            {
                return (new Rectangle((int)position.X, (int)position.Y,
                        playerWidth, playerHeight));
            }
        }

        /// <summary>
        /// Property for the position. Returns the player's upper left corner value.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }

        /// <summary>
        /// Player's constructor. It creates a player with the specified dimensions.
        /// The player will be placed at the bottom, in the middle of the screen.
        /// </summary>
        /// <param name="playerHeight">The height of the player.</param>
        /// <param name="playerWidth">The width of the player.</param>
        /// <param name="screenWidth">The width of the screen.</param>
        /// <param name="screenHeight">The height of the screen.</param>
        /// <param name="speed">The speed at which the player moves.</param>
        public Player(int playerHeight, int playerWidth, int screenWidth,
                      int screenHeight, float speed)
        {
            this.playerHeight = playerHeight;
            this.playerWidth = playerWidth;
            this.screenWidth = screenWidth;
            this.SPEED = speed;

            position = new Vector2();
            position.X = screenWidth / 2 - playerWidth / 2;
            position.Y = screenHeight - playerHeight - 30;
        }

        /// <summary>
        /// Moves the player to the right, if there is still space.
        /// </summary>
        public void MoveRight()
        {
            position.X = MathHelper.Min(position.X + SPEED,
                            screenWidth - playerWidth);
            
        }

        /// <summary>
        /// Moves the player to the left, if there is still space.
        /// </summary>
        public void MoveLeft()
        {
            position.X = MathHelper.Max(position.X - SPEED, 0);
        }
    }
}
