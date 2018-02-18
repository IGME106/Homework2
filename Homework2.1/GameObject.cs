using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework21
{
    class GameObject : Game1
    {
        private Texture2D texture2d;
        private Rectangle rectangle;

        private int xPosition;
        private int yPosition;

        public GameObject(int x, int y, int width, int height)
        {
            Rectangle = new Rectangle(x, y, width, height);
        }

        public Texture2D Texture2D
        {
            get { return this.texture2d; }
            set { this.texture2d = value; }
        }

        public Rectangle Rectangle
        {
            get { return this.rectangle; }
            set { this.rectangle = value; }
        }

        public int XPosition
        {
            get { return this.xPosition; }
            set { this.xPosition = value; }
        }

        public int YPosition
        {
            get { return this.yPosition; }
            set { this.yPosition = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture2D,
                Rectangle,
                Color.White);
        }
    }    
}
