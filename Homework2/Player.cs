using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    class Player : GameObject
    {
        private int levelScore;
        private int totalScore;

        private SpriteState spriteCurrentState = SpriteState.Stand;
        private SpriteState spritePreviousState = SpriteState.Stand;

        public Player (int x, int y, int width, int height) : base(x, y, width, height)
        {
            LevelScore = 0;
            TotalScore = 0;
        }

        public SpriteState SpriteCurrentState
        {
            get { return this.spriteCurrentState; }
            set { this.spriteCurrentState = value; }
        }

        public SpriteState SpritePreviousState
        {
            get { return this.spritePreviousState; }
            set { this.spritePreviousState = value; }
        }

        public int LevelScore
        {
            get { return this.levelScore; }
            set { this.levelScore = value; }
        }

        public int TotalScore
        {
            get { return this.totalScore; }
            set { this.totalScore = value; }
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            //Rectangle currentSprite = new Rectangle();
            SpriteEffects flips = SpriteEffects.None;

            if (SpriteCurrentState == SpriteState.FaceLeft ||             // Size definition if Mario is facing left
                SpriteCurrentState == SpriteState.WalkLeft)
            {
                flips = SpriteEffects.FlipHorizontally;                 // If mario is facing other way, flip sprite

                base.XPosition = base.Rectangle.Width * currentFrame;
                
                //currentSprite = new Rectangle(
                //                   base.Rectangle.Width * currentFrame,
                //                   0,
                //                   base.Rectangle.Width,
                //                   base.Rectangle.Height);
            }
            else if (SpriteCurrentState == SpriteState.FaceRight ||       // Size definition if Mario is facing right
                SpriteCurrentState == SpriteState.WalkRight)
            {
                base.XPosition = base.Rectangle.Width * currentFrame;
                //currentSprite = new Rectangle(
                //                   base.Rectangle.Width * currentFrame,
                //                   0,
                //                   base.Rectangle.Width,
                //                   base.Rectangle.Height);
            }
            else                                                        // If size definition if Mario is not moving
            {
                base.XPosition = 0;
                //currentSprite = new Rectangle(
                //                   0,
                //                   0,
                //                   base.Rectangle.Width,
                //                   base.Rectangle.Height);
            }

            spriteBatch.Draw(
                base.Texture2D,
                base.Rectangle,
                base.Rectangle,
                Color.White,
                0.0f,
                Vector2.Zero,
                flips,
                0.0f);





            //spriteBatch.Draw(                                           // Draw the sprite from the spriteBatch
            //    base.Texture2D,
            //    base.Rectangle,
            //    //currentSprite,
            //    Color.White,
            //    0.0f,
            //    Vector2.Zero,
            //    flips,
            //    0.0f);
            
        }
    }
}
