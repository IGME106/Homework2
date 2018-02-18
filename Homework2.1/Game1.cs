using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Homework21
{
    public enum GameState
    {
        Menu,
        Game,
        GameOver
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        List<Collectible> collectibles;

        // Player texture properties
        private Texture2D marioTexture;
        private Rectangle marioSprite;
        int marioSpritesInSheet;
        int marioWidth;
        int marioHeight;

        // Player Statemachine
        private enum MarioState
        {
            WalkLeft,
            FaceLeft,
            Stand,
            FaceRight,
            WalkRight
        }
        
        // Collectible texture properties
        private Texture2D collectibleTexture;
        private Rectangle collectibleSprite;
        int collectibleSpritesInSheet;
        int collectibleWidth;
        int collectibleHeight;

        // Text drawing properties
        private Vector2 levelScore = new Vector2(0, 0);
        private Vector2 totalScore = new Vector2(0, 35);

        private SpriteFont spriteFont;

        // Animation reqs
        int currentFrame;
        double fps;
        double secondsPerFrame;
        double timeCounter;

        // Game stats
        GameState gameState = GameState.Menu;

        int currentLevel = 0;
        double timer;

        KeyboardState kbState;
        KeyboardState previousKbState;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Window.Position = new Point(                    // Center the game view on the screen
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) -
                    (graphics.PreferredBackBufferWidth / 2),
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) -
                    (graphics.PreferredBackBufferHeight / 2)
            );
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


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

            // TODO: use this.Content to load your game content here
            marioTexture = Content.Load<Texture2D>("MarioSpriteSheet");
            marioSpritesInSheet = 5;
            marioWidth = marioTexture.Width / marioSpritesInSheet;

            collectibleTexture = Content.Load<Texture2D>("CoinSprite");
            collectibleSpritesInSheet = 1;
            collectibleWidth = collectibleTexture.Width / collectibleSpritesInSheet;

            // Set up animation stuff
            currentFrame = 1;
            fps = 10.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 0;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            ProcessInput();

            switch (gameState)
            {
                case GameState.Menu:
                    break;
                case GameState.Game:
                    break;
                case GameState.GameOver:
                    break;                
                default:
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            switch (gameState)
            {
                case GameState.Menu:
                    break;
                case GameState.Game:
                    break;
                case GameState.GameOver:
                    break;
                default:
                    break;
            }

            base.Draw(gameTime);
        }

        private void ProcessInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (!keyboardState.IsKeyDown(Keys.Enter))
            {

            }
            else
            {
                if (gameState == GameState.Game)
                {
                    gameState = GameState.GameOver;
                }
                else if (gameState == GameState.GameOver)
                {
                    gameState = GameState.Menu;
                }
                else if (gameState == GameState.Menu)
                {
                    gameState = GameState.Game;
                }
            }
        }

        private void NextLevel()
        {
            currentLevel++;

            timer = 10;
            player.LevelScore = 0;
            player.XPosition = (GraphicsDevice.Viewport.Width / 2);
            player.YPosition = (GraphicsDevice.Viewport.Height / 2);

            collectibles.Clear();

            int nrCollectibles = ((this.currentLevel * 3) - 2);

            for (int i = 0; i < nrCollectibles; i++)
            {
                collectibles.Add(new Collectible());
            }
            
        }
    }
}
