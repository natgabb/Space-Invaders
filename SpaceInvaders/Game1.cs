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
    /// To keep track of the game's status.
    /// </summary>
    public enum GameStatus { ACTIVE, PAUSED, NEW_WAVE, OVER }

    public delegate void Hit(DrawableGameComponent sprite, ProjectileSprite projectile);
    public delegate void GameIsOver();

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private PlayerSprite player;
        private AlienSquad aliens;
        private LaserFactory playerLaser;
        private BombFactory alienBomb;
        private BombFactory shipBomb;
        private BombFactory bonusBomb;
        private ScoreSprite score;
        private MotherShipSprite ship;
        private Bonus bonus;
        private bool updatedBeforePause = false;

        public GameStatus Status
        {
            get;
            private set;
        }

        /// <summary>
        /// This is the game's constructor.
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 550;
            graphics.PreferredBackBufferWidth = 900;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs
        /// to before starting to run. This is where it can query for
        /// any required services and load any non-graphic related content.
        /// Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Texture2D motherShipBombsTexture = this.Content.Load<Texture2D>
                                                ("motherShipBombs");
            Texture2D alienBombsTexture = this.Content.Load<Texture2D>("laser2");
            Texture2D bonusImage = this.Content.Load<Texture2D>("bonus1");

            playerLaser = new LaserFactory(this);            
            alienBomb = new BombFactory(this, alienBombsTexture, 1, 3);
            shipBomb = new BombFactory(this, motherShipBombsTexture, 2, 3);
            bonusBomb = new BombFactory(this, bonusImage, -1, 2);
            player = new PlayerSprite(this, playerLaser);
            bonus = new Bonus(this, bonusBomb);
            aliens = new AlienSquad(this, alienBomb, playerLaser);
            score = new ScoreSprite(this, playerLaser, alienBomb, shipBomb, bonusBomb);
            ship = new MotherShipSprite(this, shipBomb, playerLaser);

            Components.Add(bonus);
            Components.Add(player);
            Components.Add(aliens);
            Components.Add(playerLaser);
            Components.Add(alienBomb);
            Components.Add(score);
            Components.Add(ship);
            Components.Add(shipBomb);
            Components.Add(bonusBomb);

            playerLaser.AddOpponent(aliens);
            playerLaser.AddOpponent(ship);
            alienBomb.AddOpponent(player);
            shipBomb.AddOpponent(player);
            bonusBomb.AddOpponent(player);

            ScoreSprite.GameOver += onGameOver;
            AlienSquad.GameOver += onGameOver;
            AlienSquad.NewWave += onNewWave;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Status == GameStatus.ACTIVE)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.P))
                    Status = GameStatus.PAUSED;
                base.Update(gameTime);
            }
            // Will update once and then wait until the user presses "c".
            else if (Status == GameStatus.PAUSED || Status == GameStatus.NEW_WAVE)
            {
                if (!updatedBeforePause)
                {
                    base.Update(gameTime);
                    updatedBeforePause = true;
                }
                onPause();
            }

            else //Status = GameStatus.OVER
            {
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    Status = GameStatus.ACTIVE;
                    score.Dispose();
                    Initialize();
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {          
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        /// <summary>
        /// This is called by the GameOver event in the ScoreSprite
        /// and AlienSquad if the game is over.
        /// </summary>
        private void onGameOver()
        {
            player.Dispose();
            aliens.Dispose();
            playerLaser.Dispose();
            alienBomb.Dispose();
            ship.Dispose();
            shipBomb.Dispose();
            bonus.Dispose();
            bonusBomb.Dispose();

            Status = GameStatus.OVER;
        }

        /// <summary>
        /// This is called when a new wave is needed.
        /// </summary>
        /// <param name="wave">The current wave number.</param>
        private void onNewWave(int wave)
        {
            Status = GameStatus.NEW_WAVE;
        }

        /// <summary>
        /// Checks for user input when game is paused.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        private void onPause()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                updatedBeforePause = false;
                Status = GameStatus.ACTIVE;
            }
        }
    }
}
