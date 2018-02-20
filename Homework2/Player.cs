using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// IGME-106 - Game Development and Algorithmic Problem Solving
/// Homework 2 - Coin Collector
/// Class Description   : Player class for creation of the player object
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : February 19, 2018
/// Filename            : Player.cs
/// </summary>
///

namespace Homework2
{
    // Sprite Statemachine
    public enum SpriteState
    {
        WalkLeft,
        FaceLeft,
        Stand,
        FaceRight,
        WalkRight
    }

    /// <summary>
    /// Class file for the player object
    /// </summary>
    class Player : GameObject
    {
        private int levelScore;
        private int totalScore;

        private SpriteState spriteCurrentState = SpriteState.Stand;
        private SpriteState spritePreviousState = SpriteState.Stand;

        /// <summary>
        /// Player object constructor.  Takes input values and passes them to the base
        /// </summary>
        /// <param name="x">player x coordinate</param>
        /// <param name="y">player y coordinate</param>
        /// <param name="width">player sprite width</param>
        /// <param name="height">player sprite height</param>
        public Player (int x, int y, int width, int height) : base(x, y, width, height)
        {
            LevelScore = 0;
            TotalScore = 0;
        }

        /// <summary>
        /// Property of current statemachine state of the sprite
        /// </summary>
        public SpriteState SpriteCurrentState
        {
            get { return this.spriteCurrentState; }
            set { this.spriteCurrentState = value; }
        }

        /// <summary>
        /// Property of previuos statemachine state of the sprite
        /// </summary>
        public SpriteState SpritePreviousState
        {
            get { return this.spritePreviousState; }
            set { this.spritePreviousState = value; }
        }

        /// <summary>
        /// Property of current level score
        /// </summary>
        public int LevelScore
        {
            get { return this.levelScore; }
            set { this.levelScore = value; }
        }

        /// <summary>
        /// Property of total score
        /// </summary>
        public int TotalScore
        {
            get { return this.totalScore; }
            set { this.totalScore = value; }
        }

        /// <summary>
        /// Determines which way Mario is facing and applies the appropriate
        ///   SpriteEffects.  Also defines the section from the SpriteSheet
        ///   that is to be drawn.
        /// </summary>
        public new void Draw(SpriteBatch spriteBatch)
        {
            Rectangle spriteCurrentState = new Rectangle();
            SpriteEffects flips = SpriteEffects.None;

            if (SpriteCurrentState == SpriteState.FaceLeft ||                   // Size definition if Mario is facing left
                SpriteCurrentState == SpriteState.WalkLeft)
            {
                flips = SpriteEffects.FlipHorizontally;                         // If mario is facing other way, flip sprite
                
                spriteCurrentState = new Rectangle(
                                   base.Rectangle.Width * currentFrame,
                                   0,
                                   base.Rectangle.Width,
                                   base.Rectangle.Height);
            }
            else if (SpriteCurrentState == SpriteState.FaceRight ||             // Size definition if Mario is facing right
                SpriteCurrentState == SpriteState.WalkRight)
            {
                spriteCurrentState = new Rectangle(
                                   base.Rectangle.Width * currentFrame,
                                   0,
                                   base.Rectangle.Width,
                                   base.Rectangle.Height);
            }
            else                                                                // If size definition if Mario is not moving
            {
                spriteCurrentState = new Rectangle(
                                   0,
                                   0,
                                   base.Rectangle.Width,
                                   base.Rectangle.Height);
            }

            spriteBatch.Draw(                                                   // Draw the sprite from the spriteBatch
                base.Texture2D,
                base.Rectangle,
                spriteCurrentState,
                Color.White,
                0.0f,
                Vector2.Zero,
                flips,
                0.0f
            );            
        }
    }
}
