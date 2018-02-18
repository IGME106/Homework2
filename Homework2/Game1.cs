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
        //private Rectangle marioSprite;
        int marioSpritesInSheetWidth;
        int marioSpritesInSheetHeight;
        int marioWidth;
        int marioHeight;
        
        // Collectible texture properties
        private Texture2D collectibleTexture;
        //private Rectangle collectibleSprite;
        int collectibleSpritesInSheetWidth;
        int collectibleSpritesInSheetHeight;
        int collectibleWidth;
        int collectibleHeight;

        // Text drawing properties
        private Vector2 totalScorePosition = new Vector2(0, 0);
        private Vector2 levelScorePosition = new Vector2(0, 35);
        private Vector2 timeCounterPosition = new Vector2(0, 70);

        private SpriteFont levelScoreFont;
        private SpriteFont totalScoreFont;
        private SpriteFont timeCounterFont;
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

        KeyboardState kbState;
        KeyboardState previousKbState;

        Random random = new Random();


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

            currentLevel = 0;
            nrCollectibles = 1;

            player = new Player(0, 0, marioWidth, marioHeight);

            //collectibles = new List<Collectible>();

            //for (int i = 0; i < nrCollectibles; i++)
            //{
            //    collectibles.Add(new Collectible(
            //        0,
            //        0,
            //        collectibleWidth,
            //        collectibleHeight));
            //}

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

            player.Texture2D = Content.Load<Texture2D>("MarioSpriteSheet");

            marioSpritesInSheetWidth = 5;
            marioSpritesInSheetHeight = 1;
            marioWidth = 44; // player.Texture2D.Width / marioSpritesInSheetWidth;
            marioHeight = 72; // player.Texture2D.Height / marioSpritesInSheetHeight;

            player.Rectangle = new Rectangle(0, 0, marioWidth, marioHeight);

            //collectibleTexture = Content.Load<Texture2D>("CollectibleSprite");

            //collectibleSpritesInSheetWidth = 1;
            //collectibleSpritesInSheetHeight = 1;
            //collectibleWidth = 20;
            //collectibleHeight = 20;

            levelScoreFont = Content.Load<SpriteFont>("LevelScoreFont");
            totalScoreFont = Content.Load<SpriteFont>("TotalScoreFont");
            timeCounterFont = Content.Load<SpriteFont>("TimeCounterFont");
            gameOverFont = Content.Load<SpriteFont>("GameOverFont");

            // Set up animation stuff
            currentFrame = 1;
            fps = 10.0;
            secondsPerFrame = 1.0f / fps;
            timeCounter = 10000;
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
                    }

                    break;
                case GameState.Game:

                    if (timeCounter != 0)
                    {
                        previousKbState = kbState;

                        timeCounter -= gameTime.ElapsedGameTime.TotalSeconds;

                        ProcessInput();

                        //for (int i = 0; i < collectibles.Count; i++)
                        //{
                        //    if (!collectibles[i].Active)
                        //    {
                        //        player.LevelScore++;
                        //        player.TotalScore++;
                        //    }
                        //}

                        //if (player.LevelScore == collectibles.Count())
                        //{
                        //    NextLevel();
                        //}

                        timeCounter -= 0.1;
                    }
                    else
                    {
                        gameState = GameState.GameOver;
                    }

                    break;
                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        gameState = GameState.Menu;
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
                        timeCounterFont,
                        "  W\nA S D",
                        new Vector2(100, 100),
                        Color.Red
                    );

                    break;
                case GameState.Game:
                    spriteBatch.DrawString(
                        totalScoreFont,
                        player.TotalScore.ToString(),
                        totalScorePosition,
                        Color.Blue
                    );

                    spriteBatch.DrawString(
                        levelScoreFont,
                        player.LevelScore.ToString(),
                        levelScorePosition,
                        Color.Blue
                    );

                    spriteBatch.DrawString(
                        timeCounterFont,
                        timeCounter.ToString(),
                        timeCounterPosition,
                        Color.Blue
                    );

                    player.Draw(spriteBatch);

                    //for (int i = 0; i < collectibles.Count; i++)
                    //{
                    //    if (!collectibles[i].Active)
                    //    {
                    //        player.LevelScore++;
                    //        player.TotalScore++;
                    //    }
                    //}

                    //NextLevel();

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

            timeCounter = 10;
            player.LevelScore = 0;
            player.XPosition = (GraphicsDevice.Viewport.Width / 2);
            player.YPosition = (GraphicsDevice.Viewport.Height / 2);

            collectibles.Clear();

            for (int i = 0; i < nrCollectibles; i++)
            {
                collectibles.Add(new Collectible(
                    random.Next(marioWidth + 10, GraphicsDevice.Viewport.Width),
                    random.Next(marioHeight + 10, GraphicsDevice.Viewport.Height),
                    collectibleWidth,
                    collectibleHeight));

                collectibles[i].Active = true;
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
