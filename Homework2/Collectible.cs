using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework2
{
    class Collectible : GameObject
    {
        private bool active;

        public Collectible(int x, int y, int width, int height) : base(x, y, width, height)
        {
            Active = true;
        }

        public bool Active
        {
            get { return this.active; }
            set { this.active = value; }
        }

        public bool CheckCollision(GameObject gameObject)
        {
            bool returnValue = false;

            if (this.Active)
            {
                if (this.Rectangle.Intersects(gameObject.Rectangle))
                {
                    returnValue = true;
                }
            }

            return returnValue;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            if (this.Active)
            {
                spriteBatch.Draw(
                base.Texture2D,
                base.Rectangle,
                Color.White);
            }            
        }
    }
}
