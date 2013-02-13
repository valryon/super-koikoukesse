#region Using Statements
using SuperKoikoukesse.Core.Main;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace SuperKoikoukesse
{
#if WINDOWS
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static SuperKoikoukesseGame game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            game = new SuperKoikoukesseGame();
            game.Run();
        }
    }
#endif
}
