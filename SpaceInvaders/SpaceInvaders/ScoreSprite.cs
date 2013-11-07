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
    public class ScoreSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game1 game;
        private SpriteFont font;
        private Texture2D healthImage;
        private SpriteBatch spriteBatch;

        private int health = 5;
        private ulong score = 0;
        private int currentWave = 1;

        public static event GameIsOver GameOver;

        /// <summary>
        /// ScoreSprite's constructor.
        /// </summary>
        /// <param name="game">The game</param>
        public ScoreSprite(Game1 game, LaserFactory laser, BombFactory bomb,
                           BombFactory motherShipBomb, BombFactory bonusBomb)
            : base(game)
        {
            this.game = game;
            bomb.PlayerHit += onPlayerHit;
            motherShipBomb.PlayerHit += onPlayerHit;
            bonusBomb.PlayerHit += onPlayerHit;
            laser.AlienKilled += onAlienHit;
            laser.MotherShipHit += onAlienHit;
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
            base.Update(gameTime);
        }

        /// <summary>
        /// Loads the images needed for the scoring.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = game.Content.Load<SpriteFont>("scoreFont");
            healthImage = game.Content.Load<Texture2D>("health");
            base.LoadContent();
        }

        /// <summary>
        /// Draws the score, health bar, current wave, game paused
        /// screen and level complete screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            int height = GraphicsDevice.Viewport.Height - 30;

            string display = "Wave " + currentWave;
            spriteBatch.DrawString(font, display, 
                new Vector2(GraphicsDevice.Viewport.Width - display.Length * 12,
                height), Color.White);

            // Displays a different string if the game is over.
            if (game.Status == GameStatus.OVER)
                gameOverDisplay();
            else
                displayScoreAndHealth();

            // Displays a special string if the game is paused.
            String message = "Press \"C\" to continue.";

            if (game.Status == GameStatus.NEW_WAVE)
            {
                spriteBatch.DrawString(font, "Level Complete! " + message,
                    new Vector2(5, height), Color.Yellow);
            }
            else if (game.Status == GameStatus.PAUSED)
                spriteBatch.DrawString(font, "Game paused! " + message,
                    new Vector2(5, height), Color.Yellow);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// This is called when the player is hit by a projectile.
        /// (The player parameter could be used if in the future a multiplayer
        /// level is added.)
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="projectile">The projectile</param>
        private void onPlayerHit(DrawableGameComponent player, 
            ProjectileSprite projectile)
        {
            health = health - projectile.Damage;
            if (health <= 0)
                if (GameOver != null)
                {
                    GameOver();
                }
        }

        /// <summary>
        /// Adds to the score when an opponent dies.
        /// </summary>
        /// <param name="alien"></param>
        /// <param name="projectile"></param>
        private void onAlienHit(DrawableGameComponent component,
                                ProjectileSprite projectile)
        {
            if (component is AlienSprite)
                score += (ulong)(component as AlienSprite).Point;
            else if (component is MotherShipSprite)
                score += (ulong) (component as MotherShipSprite).Point;
        }

        /// <summary>
        /// Adds a bonus score and an extra health for each new wave.
        /// </summary>
        /// <param name="wave">Current wave number.</param>
        private void onNewWave(int wave)
        {
            currentWave = wave;
            score += (ulong)wave * 100;
            health ++;
        }

        /// <summary>
        /// This is what displays once the game is over.
        /// </summary>
        private void gameOverDisplay()
        {
            int height = GraphicsDevice.Viewport.Height / 2;
            int width = GraphicsDevice.Viewport.Width / 2;

            string gameOverStr = "Game Over!!!";
            spriteBatch.DrawString(font, gameOverStr,
                new Vector2(width - gameOverStr.Length * 6, height
                    - 60), Color.Red);

            string scoreStr = "Your score is: " + score;
            spriteBatch.DrawString(font, scoreStr,
                new Vector2(width - scoreStr.Length * 6, height
                    - 35), Color.Yellow);

            string highScore = "High Score: " + HighScore.RetrieveHighScore();
            spriteBatch.DrawString(font, highScore,
                new Vector2(width - highScore.Length * 6, height
                    - 10), Color.Yellow);
            HighScore.WriteHighScore(score);

            string toReplay = "Press \"R\" to start a new game.";
            spriteBatch.DrawString(font, toReplay,
                new Vector2(width - toReplay.Length * 6, height
                    + 15), Color.Yellow);
        }

        /// <summary>
        /// This is the general display.
        /// </summary>
        private void displayScoreAndHealth()
        {
            spriteBatch.DrawString(font, "Score: " + score, new Vector2(5, 0),
                Color.White);

            // Displays the player's image representing its health at the top right.
            int rightX = GraphicsDevice.Viewport.Width - healthImage.Width - 5;
            int startingX = rightX;
            int startingY = 5;
            for (int i = 1; i <= health; i++)
            {
                spriteBatch.Draw(healthImage, new Vector2(startingX, startingY),
                    Color.White);
                startingX -= healthImage.Width + 5;
                // If the person has more than 10 lives, starts a new line.
                if (i % 10 == 0)
                {
                    startingX = rightX;
                    startingY += healthImage.Height + 5;
                }
            }
        }
    }
}
