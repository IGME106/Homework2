using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// IGME-106 - Game Development and Algorithmic Problem Solving
/// Homework 2 - Coin Collector
/// Class Description   : Main game glass for the project
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : February 19, 2018
/// Filename            : Game1.cs
/// </summary>
///

namespace Homework2
{
    /// <summary>
    /// Game state machine
    /// </summary>
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

        // Define player and collectible objects
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
        private Vector2 menuPosition = new Vector2(0, 0);
        private Vector2 totalScorePosition = new Vector2(0, 0);
        private Vector2 currentLevelPosition = new Vector2(0, 0);
        private Vector2 levelScorePosition = new Vector2(0, 0);
        private Vector2 levelTimerPosition = new Vector2(0, 0);
        private Vector2 gameOverPosition = new Vector2(0, 0);

        private SpriteFont menuFont;
        private SpriteFont currentLevelFont;
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

        // System stats
        KeyboardState kbState;
        KeyboardState previousKbState;

        // Random number generator
        Random random = new Random();
        
        /// <summary>
        /// Constructor for the class.
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1024;                           // Set desired width of window
            graphics.PreferredBackBufferHeight = 768;                           // Set desired height of window
            graphics.ApplyChanges();

            Window.Position = new Point(                                        // Center the game view on the screen
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
            currentLevel = 0;
            nrCollectibles = 1;
                        
            player = new Player(0, 0, marioWidth, marioHeight);                 // Create a new player object

            collectibles = new List<Collectible>();                             // Create a new collectibles list object

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);                      // Create a new SpriteBatch, which can be used to draw textures.

            marioTexture = Content.Load<Texture2D>("MarioSpriteSheet");         // Load the player spritesheet
            
            player = new Player(                                                // Create a new player with default values
                GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Width / 2,
                marioWidth,
                marioHeight
            );

            player.Texture2D = marioTexture;                                    // Add the player spritesheet to the player object

            collectibleTexture = Content.Load<Texture2D>("CollectibleSprite");  // Load the collectible spritesheet

            for (int i = 0; i < collectibles.Count; i++)                        // Create collectibles with default values
            {
                collectibles.Add(new Collectible(random.Next(50, GraphicsDevice.Viewport.Width),
                                                          random.Next(50, GraphicsDevice.Viewport.Height),
                                                          collectibleWidth,
                                                          collectibleHeight));

                collectibles[i].Texture2D = collectibleTexture;                 // Add the collectible spritesheet to the collectible object
            }

            // Load the different fonts
            menuFont = Content.Load<SpriteFont>("MenuFont");
            currentLevelFont = Content.Load<SpriteFont>("CurrentLevelFont");
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

            kbState = Keyboard.GetState();                                      // Get current state of keyboard

            switch (gameState)                                                  // Main game loop logic
            {
                // Display the game menu
                case GameState.Menu:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        gameState = GameState.Game;                             // Change from menu to game on enter
                        levelTimer = 100;
                    }
                    else
                    {
                        previousKbState = kbState;                              // If keypress was not enter, change previous state
                    }

                    break;
                // Start the game
                case GameState.Game:                                            // Change from game to gameover on enter

                    if (levelTimer > 0)
                    {
                        previousKbState = kbState;                              // Change previous state

                        levelTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                        ProcessInput();                                         // Process user input

                        for (int i = 0; i < collectibles.Count; i++)            // Test each collectible for collision
                        {
                            if (collectibles[i].CheckCollision(player))
                            {
                                player.LevelScore++;
                                player.TotalScore++;
                            }
                        }

                        if (player.LevelScore == collectibles.Count())          // If the player has collected all collectibles
                        {                                                           // advance to next level
                            NextLevel();
                        }

                        levelTimer -= 0.1;

                        if (levelTimer <= 0)                                    // If the timer ran out, go to game over
                        {
                            gameState = GameState.GameOver;
                        }
                    }
                    else
                    {
                        previousKbState = kbState;                              // Change previous state
                    }                    

                    break;
                // Display Game Over and score
                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        previousKbState = kbState;                              // Change previous state

                        gameState = GameState.Menu;                             // Change from game to gameover on enter
                    }
                    else
                    {
                        previousKbState = kbState;                              // Change previous state
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

            string drawString = null;

            spriteBatch.Begin();

            // Define which sprite fonts to display based on which state the game is in
            switch (gameState)
            {
                case GameState.Menu:

                    drawString = 
                        "               Coin Collector\n" +
                        "\n" +
                        "Use the following keys to navigate" +
                        "\n\n" +
                        "                         W\n" +
                        "                       A S D" +
                        "\n\n" +
                        "Press Enter to start/stop the game";

                    // Set menu to center of screen (kind of)
                    menuPosition = new Vector2(
                        (GraphicsDevice.Viewport.Width / 2) - (menuFont.MeasureString(drawString).X / 2),
                        (GraphicsDevice.Viewport.Height / 2) - (menuFont.MeasureString(drawString).Y / 2)
                    );

                    DrawSpriteBatch(menuFont, drawString, menuPosition, Color.Red);

                    break;
                case GameState.Game:

                    drawString = "Current Level:    " + this.currentLevel;

                    // Set current level position to top, second from top
                    currentLevelPosition = new Vector2(
                        (GraphicsDevice.Viewport.Width - currentLevelFont.MeasureString(drawString).X - 5),
                        35
                    );

                    DrawSpriteBatch(currentLevelFont, drawString, currentLevelPosition, Color.Red);

                    drawString = "Total Score:      " + player.TotalScore.ToString();

                    // Set total score position to top right
                    totalScorePosition = new Vector2(
                        (GraphicsDevice.Viewport.Width - totalScoreFont.MeasureString(drawString).X - 5),
                        5
                    );

                    DrawSpriteBatch(totalScoreFont, drawString, totalScorePosition, Color.Red);

                    drawString = "Level Score:      " + player.LevelScore.ToString();

                    // Set level score position to top left
                    levelScorePosition = new Vector2(5, 5);

                    DrawSpriteBatch(levelScoreFont, drawString, levelScorePosition, Color.Blue);

                    drawString = "Time left:        " + String.Format("{0:0.00}", levelTimer);

                    // Set timer position to top center
                    levelTimerPosition = new Vector2(
                        (GraphicsDevice.Viewport.Width / 2) - (levelTimerFont.MeasureString(drawString).X / 2),
                        5
                    );

                    if (levelTimer > 50)                                        // If the timer is larger than 50, use blue font
                    {
                        DrawSpriteBatch(levelTimerFont, drawString, levelTimerPosition, Color.Blue);
                    }
                    else if ((levelTimer <= 50) && (levelTimer >= 25))          // If the timer is less than 50 and larger than 25, 
                    {                                                               // use orange font
                        DrawSpriteBatch(levelTimerFont, drawString, levelTimerPosition, Color.Orange);
                    }
                    else                                                        // If the timer is less than 25, use red font
                    {
                        DrawSpriteBatch(levelTimerFont, drawString, levelTimerPosition, Color.Red);
                    }

                    // Draw the player sprite
                    player.Draw(spriteBatch);

                    // Draw the collectible sprites that are active
                    for (int i = 0; i < collectibles.Count; i++)
                    {
                        if (collectibles[i].ActiveCollectible)
                        {
                            collectibles[i].Draw(spriteBatch);
                        }
                    }

                    break;
                case GameState.GameOver:

                    drawString = 
                        "       GAME OVER" +
                        "\n\n" +
                        "   You reached level       " + this.currentLevel +
                        "\n\n" +
                        "   Your High Score is      " + player.TotalScore +
                        "\n\n" +
                        "To return to the menu, press Enter.";

                    // Set game over position to center scree (kind of)
                    gameOverPosition = new Vector2(
                                (GraphicsDevice.Viewport.Width / 2) - (gameOverFont.MeasureString(drawString).X / 2),
                                (GraphicsDevice.Viewport.Height / 2) - (gameOverFont.MeasureString(drawString).Y / 2)
                            );

                    DrawSpriteBatch(gameOverFont, drawString, gameOverPosition, Color.Red);

                    break;
                default:
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// This method is used to actually draw the textual graphics
        /// </summary>
        /// <param name="textFont">The name of the SpriteFont that was defined</param>
        /// <param name="textString">The string that needs to be printed</param>
        /// <param name="textVector">The location on the screen where the text should be printed</param>
        /// <param name="textColor">The color to use when printing the text</param>
        private void DrawSpriteBatch(SpriteFont textFont, string textString, Vector2 textVector, Color textColor)
        {
            spriteBatch.DrawString(
                textFont,
                textString,
                textVector,
                textColor
            );
        }

        /// <summary>
        /// Advances the player to the next level
        /// </summary>
        private void NextLevel()
        {
            currentLevel++;
            
            levelTimer = 100;
            player.LevelScore = 0;
            player.XPosition = (GraphicsDevice.Viewport.Width / 2);             // Reset player position to center screen
            player.YPosition = (GraphicsDevice.Viewport.Height / 2);

            collectibles.Clear();                                               // Clear the collectible objeccts

            nrCollectibles = (((currentLevel + 1) * 2) - 1);                    // Calculate a random number of collectibles

            for (int i = 0; i < nrCollectibles; i++)                            // Create the collectibles
            {
                collectibles.Add(new Collectible(random.Next(0, (GraphicsDevice.Viewport.Width - collectibleWidth)),
                                                random.Next(100, GraphicsDevice.Viewport.Height - collectibleHeight),
                                                collectibleWidth,
                                                collectibleHeight));

                collectibles[i].Texture2D = collectibleTexture;
            }
        }

        /// <summary>
        /// Resets the game variables and calls for the new level
        /// </summary>
        private void ResetGame()
        {
            this.currentLevel = 0;

            player.LevelScore = 0;
            player.TotalScore = 0;

            NextLevel();
        }

        /// <summary>
        /// Wraps the character around the screen if it moves off the visible display
        /// </summary>
        /// <param name="gameObject"></param>
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

        /// <summary>
        /// Tests whether key was pressed only once
        /// </summary>
        /// <param name="keys">The key that needs to be tested</param>
        /// <returns></returns>
        private bool SingleKeyPress(Keys keys)
        {
            bool returnValue = false;

            if ((kbState.IsKeyDown(keys)) && (previousKbState.IsKeyUp(keys)))
            {
                returnValue = true;
            }

            return returnValue;
        }

        /// <summary>
        /// Processes keyboard input and updates state machine to indicate which sprite
        ///   of Mario should be displayed
        /// </summary>
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

            if (kbState.IsKeyDown(Keys.W))                                      // Move up
            {
                player.YPosition -= 5;
            }

            if (kbState.IsKeyDown(Keys.S))                                      // Move down
            {
                player.YPosition += 5;
            }

            if (kbState.IsKeyUp(Keys.W) &&                                      // If all keys are up, Mario is standing
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
