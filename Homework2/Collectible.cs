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
        private bool activeCollectible;

        public Collectible(int x, int y, int width, int height) : base(x, y, width, height)
        {
            ActiveCollectible = true;
        }

        public bool ActiveCollectible
        {
            get { return this.activeCollectible; }
            set { this.activeCollectible = value; }
        }

        public bool CheckCollision(GameObject gameObject)
        {
            bool returnValue = false;

            if (this.ActiveCollectible)
            {
                if (this.Rectangle.Intersects(gameObject.Rectangle))
                {
                    returnValue = true;
                    this.ActiveCollectible = false;
                }
            }

            return returnValue;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            if (this.ActiveCollectible)
            {
                spriteBatch.Draw(
                base.Texture2D,
                base.Rectangle,
                Color.White);
            }            
        }
    }
}
