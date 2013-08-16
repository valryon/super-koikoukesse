using System;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public static class PXNConstants
	{
    #region Constants

    public static readonly UIColor MAIN_TEXT_COLOR = UIColor.FromHSB(0, 0, 0.35f);

    public static readonly UIColor QUARTER_ALPHA_BLACK = UIColor.FromHSBA(0, 0, 0, 0.25f);
    public static readonly UIColor HALF_ALPHA_BLACK = UIColor.FromHSBA(0, 0, 0, 0.5f);

    public static readonly UIColor BRAND_COLOR      = UIColor.FromRGB(red: 0.110f, green: 0.663f, blue: 0.878f);
    public static readonly UIColor BRAND_GREY       = UIColor.FromRGB(226.0f/255.0f, 226.0f/255.0f, 226.0f/255.0f);
    public static readonly UIColor BRAND_BACKGROUND = UIColor.FromRGB(243.0f/255.0f, 243.0f/255.0f, 243.0f/255.0f);
    public static readonly UIColor BRAND_BORDER     = UIColor.FromRGB(178.0f/255.0f, 178.0f/255.0f, 178.0f/255.0f);

    public static readonly UIColor COLOR_NAVIGATION = UIColor.FromWhiteAlpha(white:0.882f, alpha: 1f);

    //
    // Modes
    //

    public static readonly UIColor MODE_SCORE     = UIColor.FromRGB(red: 0.404f, green: 0.804f, blue: 0.863f);
    public static readonly UIColor MODE_TIME      = UIColor.FromRGB(red: 0.714f, green: 0.773f, blue: 0.424f);
    public static readonly UIColor MODE_SURVIVAL  = UIColor.FromRGB(red: 0.835f, green: 0.475f, blue: 0.859f);
    public static readonly UIColor MODE_VERSUS    = UIColor.FromRGB(red: 0.945f, green: 0.325f, blue: 0.427f);

    #endregion

    #region Methods

    /// <summary>
    /// Gets the color of the mode.
    /// </summary>
    /// <returns>The mode color</returns>
    /// <param name="mode">Mode</param>
    public static UIColor GetModeColor(GameMode mode)
    {
      switch (mode) 
      {
        case GameMode.SCORE:
          return MODE_SCORE;

        case GameMode.TIME:
          return MODE_TIME;

        case GameMode.SURVIVAL:
          return MODE_SURVIVAL;

        case GameMode.VERSUS:
          return MODE_VERSUS;
      }

      return MODE_SCORE;
    }

    #endregion
	}
}