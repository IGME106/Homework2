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
    class GameMenuHelper
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public void DrawString(string drawString, SpriteFont drawFont, string position, string color)
        {
            //string drawString = "Coin Collector\n" +
            //            "\n" +
            //            "Use the following keys to navigate" +
            //            "\n\n" +
            //            "  W\nA S D" +
            //            "\n\n" +
            //            "Press Enter to start/stop the game";

            spriteBatch.DrawString(
                drawFont,
                drawString,
                GetPosition(drawString, position),
                Color.Red
            );
        }

        private Vector2 GetPosition(string displayString, string position)
        {
            Vector2 returnValue = new Vector2(0, 0);

            GraphicsDevice.

            int viewportWidth = GraphicsDevice.Viewport.Width;
            int viewportHeight = GraphicsDevice.Viewport.Height;

            int textWidth = (int)font.MeasureString(displayString).X;
            int textHeight = (int)font.MeasureString(displayString).Y;

            switch (position)
            {
                case "TopLeft":
                    returnValue = new Vector2(
                        0,
                        (viewportHeight / 2) - (textHeight / 2)
                    );

                    break;
                case "TopCenter":
                    returnValue = new Vector2(
                        (viewportWidth / 2) - (textWidth / 2),
                        0
                    );

                    break;
                case "TopRight":
                    returnValue = new Vector2(
                        (viewportWidth - textWidth),
                        (viewportHeight / 2) - (textHeight / 2)
                    );

                    break;
                case "MiddleLeft":
                    returnValue = new Vector2(
                        (viewportWidth / 2) - (textWidth / 2),
                        (viewportHeight / 2) - (textHeight / 2)
                    );

                    break;
                case "MiddleCenter":
                    returnValue = new Vector2(
                        (viewportWidth / 2) - (textWidth / 2),
                        (viewportHeight / 2) - (textHeight / 2)
                    );

                    break;
                case "MiddleRight":
                    returnValue = new Vector2(
                        (viewportWidth / 2) - (textWidth / 2),
                        (viewportHeight / 2) - (textHeight / 2)
                    );

                    break;
                case "BottomLeft":
                    returnValue = new Vector2(
                        (viewportWidth / 2) - (textWidth / 2),
                        (viewportHeight / 2) - (textHeight / 2)
                    );

                    break;
                case "BottomCenter":
                    returnValue = new Vector2(
                        (viewportWidth / 2) - (textWidth / 2),
                        (viewportHeight / 2) - (textHeight / 2)
                    );

                    break;
                case "BottomRight":
                    returnValue = new Vector2(
                        (viewportWidth / 2) - (textWidth / 2),
                        (viewportHeight / 2) - (textHeight / 2)
                    );

                    break;
                default:
                    break;
            }

            return returnValue;
        }
    }
}
