using System;

/// <summary>
/// IGME-106 - Game Development and Algorithmic Problem Solving
/// Homework 2 - Coin Collector
/// Class Description   : Main class for instantiating the game
/// Author              : Benjamin Kleynhans
/// Modified By         : Benjamin Kleynhans
/// Date                : February 19, 2018
/// Filename            : Program.cs
/// </summary>
///

namespace Homework2
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }
#endif
}
