using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperKoikoukesse.Core.Data
{
    /// <summary>
    /// Constantes du jeu
    /// </summary>
    public static class Constants
    {
        #region Resolutions

        // Resolution
        // -- Device
        public static int GameResolutionWidth = 960, GameResolutionHeight = 640;
        // -- Virtual
        public static int DeviceResolutionWidth = 960, DeviceResolutionHeight = 640;

        #endregion

        #region GUI elements location and sizes

        public static Rectangle QuizzImageFrame = new Rectangle(30, 120, 500, 500);
        public static Vector2 QuizzJokerButtonLocation = new Vector2(540, 220);
        public static int QuizzButtonWidth = 300;
        public static int QuizzButtonHeight = 75;

        #endregion

        #region Assets

        public static string QuizzButtonAsset = "gfxs/gui/quizz_button";
        public static string QuizzBackgroundImageAsset = "gfxs/backgrounds/quizz_background";

        #endregion
    }
}
