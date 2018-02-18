using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework21
{
    class Collectible : GameObject
    {
        private bool active;

        public Collectible()
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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.Active)
            {
                spriteBatch.Draw(
                Texture2D,
                Rectangle,
                Color.White);
            }            
        }
    }
}
