using System;

namespace SuperKoikoukesse.Game
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SuperKoikoukesseGame game = new SuperKoikoukesseGame())
            {
                game.Run();
            }
        }
    }
}

