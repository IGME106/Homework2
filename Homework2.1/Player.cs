using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework21
{
    class Player : GameObject
    {
        private int levelScore;
        private int totalScore;

        public Player ()
        {
            LevelScore = 0;
            TotalScore = 0;
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
    }
}
