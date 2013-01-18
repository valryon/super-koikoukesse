using System;

namespace SuperKoiKouKesse.Game
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (SuperKoiKouKesseGame game = new SuperKoiKouKesseGame())
            {
                game.Run();
            }
        }
    }
}

