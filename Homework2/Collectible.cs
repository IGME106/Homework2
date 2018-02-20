using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// IGME-106 - Game Development and Algorithmic Problem Solving
/// Homework 2 - Coin Collector
/// Class Description   : Collectible class for creation of the collectible object
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : February 19, 2018
/// Filename            : Collectible.cs
/// </summary>
///

namespace Homework2
{
    /// <summary>
    /// Class file for the collectible object
    /// </summary>
    class Collectible : GameObject
    {
        private bool activeCollectible;

        /// <summary>
        /// Collecctible object constructor.  Takes input values and passes them to the base
        /// </summary>
        /// <param name="x">collectible x coordinate</param>
        /// <param name="y">collectible y coordinate</param>
        /// <param name="width">collectible sprite width</param>
        /// <param name="height">collectible sprite height</param>
        public Collectible(int x, int y, int width, int height) : base(x, y, width, height)
        {
            ActiveCollectible = true;
        }

        /// <summary>
        /// Property to determinen if collectible is active (not yet collected) or not
        /// </summary>
        public bool ActiveCollectible
        {
            get { return this.activeCollectible; }
            set { this.activeCollectible = value; }
        }

        /// <summary>
        /// Checks whether there was a collision on the rectangle elements between
        ///     the player and the collectible.  Returns true if there was a collision
        ///     and sets the collectible to inactive (active = false)
        /// </summary>
        /// <param name="gameObject">
        ///             player object that needs to be tested for 
        ///             intersection with this object
        /// </param>
        /// <returns>
        ///             true if the element was intersected by the player
        ///             and false if it was not intersected
        /// </returns>
        public bool CheckCollision(GameObject gameObject)
        {
            bool returnValue = false;

            if (this.ActiveCollectible)
            {
                if (this.Rectangle.Intersects(gameObject.Rectangle))            // Sets active to false if the object was
                {                                                                   // intersected by the player object
                    returnValue = true;
                    this.ActiveCollectible = false;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Draws the spritebatch
        /// </summary>
        /// <param name="spriteBatch"></param>
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
