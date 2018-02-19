using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Homework2
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
        int marioWidth = 44;
        int marioHeight = 72;
        
        // Collectible texture properties
        private Texture2D collectibleTexture;
        int collectibleWidth = 20;
        int collectibleHeight = 20;

        // Text drawing properties        
        private Vector2 totalScorePosition = new Vector2(0, 0);
        private Vector2 levelScorePosition = new Vector2(0, 35);
        private Vector2 levelTimerPosition = new Vector2(0, 70);

        private SpriteFont menuFont;
        private SpriteFont levelScoreFont;
        private SpriteFont totalScoreFont;
        private SpriteFont levelTimerFont;
        private SpriteFont gameOverFont;

        // Animation reqs
        public static int currentFrame;
        public static double fps;
        public static double secondsPerFrame;
        public static double timeCounter;

        // Game stats
        GameState gameState = GameState.Menu;

        int currentLevel;
        int nrCollectibles;
        double levelTimer;

        KeyboardState kbState;
        KeyboardState previousKbState;

        Random random = new Random();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 768;   // set this value to the desired height of your window
            graphics.ApplyChanges();

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

            currentLevel = 0;
            nrCollectibles = 1;
            //levelTimer = 100;

            player = new Player(0, 0, marioWidth, marioHeight);

            collectibles = new List<Collectible>();

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
            
            marioTexture = Content.Load<Texture2D>("MarioSpriteSheet");
            
            player = new Player(
                GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Width / 2,
                marioWidth,
                marioHeight
            );

            player.Texture2D = marioTexture;

            collectibleTexture = Content.Load<Texture2D>("CollectibleSprite");

            for (int i = 0; i < collectibles.Count; i++)
            {
                collectibles.Add(new Collectible(random.Next(50, GraphicsDevice.Viewport.Width),
                                                          random.Next(50, GraphicsDevice.Viewport.Height),
                                                          collectibleWidth,
                                                          collectibleHeight));

                collectibles[i].Texture2D = collectibleTexture;
            }

            menuFont = Content.Load<SpriteFont>("MenuFont");
            levelScoreFont = Content.Load<SpriteFont>("LevelScoreFont");
            totalScoreFont = Content.Load<SpriteFont>("TotalScoreFont");
            levelTimerFont = Content.Load<SpriteFont>("LevelTimerFont");
            gameOverFont = Content.Load<SpriteFont>("GameOverFont");

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
            //ProcessInput();

            kbState = Keyboard.GetState();

            switch (gameState)
            {
                case GameState.Menu:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        gameState = GameState.Game;
                        levelTimer = 10;
                    }
                    else
                    {
                        previousKbState = kbState;
                    }

                    break;
                case GameState.Game:

                    if (levelTimer > 0)
                    {
                        previousKbState = kbState;

                        levelTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                        ProcessInput();

                        for (int i = 0; i < collectibles.Count; i++)
                        {
                            if (collectibles[i].CheckCollision(player))
                            {
                                player.LevelScore++;
                                player.TotalScore++;
                            }
                        }

                        if (player.LevelScore == collectibles.Count())
                        {
                            NextLevel();
                        }

                        levelTimer -= 0.1;

                        if (levelTimer <= 0)
                        {
                            gameState = GameState.GameOver;
                        }
                    }
                    else
                    {
                        previousKbState = kbState;
                    }                    

                    break;
                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        previousKbState = kbState;

                        gameState = GameState.Menu;
                    }
                    else
                    {
                        previousKbState = kbState;
                    }

                    break;                
                default:
                    break;
            }

            UpdateAnimation(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the animation time
        /// </summary>
        /// <param name="gameTime">Game time information</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            // Add to the time counter (need TOTALSECONDS here)
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            // Has enough time gone by to actually flip frames?
            if (timeCounter >= secondsPerFrame)
            {
                // Update the frame and wrap
                currentFrame++;
                if (currentFrame >= 4)
                    currentFrame = 1;

                // Remove one "frame" worth of time
                timeCounter -= secondsPerFrame;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // TODO: Add your drawing code here
            switch (gameState)
            {
                case GameState.Menu:
                    spriteBatch.DrawString(
                        menuFont,
                        "Coin Collector\n" +
                        "\n" +
                        "Use the following keys to navigate" +
                        "W\nA S D",
                        new Vector2((GraphicsDevice.Viewport.Width / 3), 
                                    GraphicsDevice.Viewport.Height / 3),
                        Color.Red
                    );

                    break;
                case GameState.Game:
                    spriteBatch.DrawString(
                        totalScoreFont,
                        "Total Score: " + player.TotalScore.ToString(),
                        totalScorePosition,
                        Color.Blue
                    );

                    spriteBatch.DrawString(
                        levelScoreFont,
                        "Score this level: " + player.LevelScore.ToString(),
                        levelScorePosition,
                        Color.Blue
                    );

                    spriteBatch.DrawString(
                        levelTimerFont,
                        "Time left: " + String.Format("{0:0.00}", levelTimer),
                        levelTimerPosition,
                        Color.Blue
                    );

                    player.Draw(spriteBatch);

                    for (int i = 0; i < collectibles.Count; i++)
                    {
                        if (collectibles[i].ActiveCollectible)
                        {
                            collectibles[i].Draw(spriteBatch);
                        }
                    }

                    break;
                case GameState.GameOver:
                    spriteBatch.DrawString(
                        gameOverFont,
                        "GAME OVER",
                        new Vector2((GraphicsDevice.Viewport.Width / 2) - 4,
                                    GraphicsDevice.Viewport.Height / 2),
                        Color.Red
                    );

                    break;
                default:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void NextLevel()
        {
            currentLevel++;
            
            levelTimer = 10;
            player.LevelScore = 0;
            player.XPosition = (GraphicsDevice.Viewport.Width / 2);
            player.YPosition = (GraphicsDevice.Viewport.Height / 2);

            collectibles.Clear();

            nrCollectibles = (((currentLevel + 1) * 2) - 1);

            for (int i = 0; i < nrCollectibles; i++)
            {
                collectibles.Add(new Collectible(random.Next(50, (GraphicsDevice.Viewport.Width - collectibleWidth)),
                                                random.Next(50, GraphicsDevice.Viewport.Height - collectibleHeight),
                                                collectibleWidth,
                                                collectibleHeight));

                collectibles[i].Texture2D = collectibleTexture;
            }
        }

        private void ResetGame()
        {
            this.currentLevel = 0;

            player.TotalScore = 0;

            NextLevel();
        }

        private void ScreenWrap(GameObject gameObject)
        {
            if (gameObject.XPosition > GraphicsDevice.Viewport.Width)
            {
                gameObject.XPosition = 0;
            }
            else if (gameObject.XPosition < 0)
            {
                gameObject.XPosition = (GraphicsDevice.Viewport.Width - player.Rectangle.Width);
            }

            if (gameObject.YPosition > GraphicsDevice.Viewport.Height)
            {
                gameObject.YPosition = 0;
            }
            else if (gameObject.YPosition < 0)
            {
                gameObject.YPosition = (GraphicsDevice.Viewport.Height - player.Rectangle.Height);
            }
        }

        private bool SingleKeyPress(Keys keys)
        {
            bool returnValue = false;

            if ((kbState.IsKeyDown(keys)) && (previousKbState.IsKeyUp(keys)))
            {
                returnValue = true;
            }

            return returnValue;
        }

        private void ProcessInput()
        {
            if (kbState.IsKeyDown(Keys.A))                    // Move left
            {
                player.XPosition -= 5;

                if (player.SpritePreviousState == SpriteState.WalkRight)
                {
                    player.SpriteCurrentState = SpriteState.FaceRight;
                }
                else if (player.SpritePreviousState == SpriteState.FaceRight)
                {
                    player.SpriteCurrentState = SpriteState.Stand;
                }
                else if (player.SpritePreviousState == SpriteState.Stand)
                {
                    player.SpriteCurrentState = SpriteState.FaceLeft;
                }
                else if (player.SpritePreviousState == SpriteState.FaceLeft)
                {
                    player.SpriteCurrentState = SpriteState.WalkLeft;
                }
            }

            if (kbState.IsKeyDown(Keys.D))                    // Move right
            {
                player.XPosition += 5;

                if (player.SpritePreviousState == SpriteState.WalkLeft)
                {
                    player.SpriteCurrentState = SpriteState.FaceLeft;
                }
                else if (player.SpritePreviousState == SpriteState.FaceLeft)
                {
                    player.SpriteCurrentState = SpriteState.Stand;
                }
                else if (player.SpritePreviousState == SpriteState.Stand)
                {
                    player.SpriteCurrentState = SpriteState.FaceRight;
                }
                else if (player.SpritePreviousState == SpriteState.FaceRight)
                {
                    player.SpriteCurrentState = SpriteState.WalkRight;
                }
            }

            if (kbState.IsKeyDown(Keys.W))                    // Move up
            {
                player.YPosition -= 5;
            }

            if (kbState.IsKeyDown(Keys.S))                    // Move down
            {
                player.YPosition += 5;
            }

            if (kbState.IsKeyUp(Keys.W) &&                    // If all keys are up, Mario is standing
                kbState.IsKeyUp(Keys.A) &&
                kbState.IsKeyUp(Keys.S) &&
                kbState.IsKeyUp(Keys.D))
            {
                player.SpriteCurrentState = SpriteState.Stand;
            }

            ScreenWrap(player);
        }
    }
}
