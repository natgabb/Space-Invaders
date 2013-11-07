using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    /// <summary>
    /// Defines a bonus launching bombs.
    /// </summary>
    class Bonus : Microsoft.Xna.Framework.GameComponent
    {
        private Game1 game;
        private TimeSpan lastBonus = new TimeSpan(0, 0, 0);
        private readonly TimeSpan BETWEEN_BONUSES = new TimeSpan(0, 0, 30);
        private Random randomize = new Random();
        private BombFactory bomb;

        /// <summary>
        /// Creates a bonus with a specified bomb.
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="bomb">Its bomb</param>
        public Bonus(Game1 game, BombFactory bomb) : base(game)
        {
            this.game = game;
            this.bomb = bomb;
        }

        /// <summary>
        /// Launches the bonus bomb.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - lastBonus > BETWEEN_BONUSES)
                if (randomize.Next(1, 1000) == 1)
                {
                    bomb.Launch(new Rectangle(randomize.Next
                        (0, game.GraphicsDevice.Viewport.Width), 10, 1, 1), gameTime);
                    lastBonus = gameTime.TotalGameTime;
                }
            base.Update(gameTime);
        }

    }
}
