using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
  public partial class CardScoreViewController : UIViewController
  {
    #region Members

    private HighScoresControlViewController highScoresController;

    #endregion

    #region Constructors

    public CardScoreViewController()
      : base ("CardScore" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
    }

    public override void ViewWillAppear(bool animated)
    {
      base.ViewWillAppear(animated);

      if (highScoresController == null)
      {
        highScoresController = new HighScoresControlViewController();
        highScoresController.SetScoreParameters(GameModes.SCORE_ATTACK, GameDifficulties.NORMAL);

        this.highscoreView.AddSubview(highScoresController.View);
      }
    }

    #endregion

    #region Methods

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

    public void ForceUpdate()
    {
      highScoresController.UpdateGameCenterLeaderboard();
    }

    /// <summary>
    /// Get mode from the selector
    /// </summary>
    /// <returns>The mode.</returns>
    private GameModes getMode()
    {

      if (modeSelector.SelectedSegment >= 0)
      {
        string mode = modeSelector.TitleAt(modeSelector.SelectedSegment);

        if (mode.ToLower().Contains("score"))
          return GameModes.SCORE_ATTACK;
        if (mode.ToLower().Contains("time"))
          return GameModes.TIME_ATTACK;
        if (mode.ToLower().Contains("survival"))
          return GameModes.SURVIVAL;
        if (mode.ToLower().Contains("versus"))
          return GameModes.VERSUS;
      }

      return GameModes.SCORE_ATTACK;
    }

    /// <summary>
    /// Get diff from the selector
    /// </summary>
    /// <returns>The difficulty.</returns>
    private GameDifficulties getDifficulty()
    {
      if (diffSelector.SelectedSegment >= 0)
      {
        string diff = diffSelector.TitleAt(diffSelector.SelectedSegment);

        if (diff.ToLower().Contains("normal"))
          return GameDifficulties.NORMAL;
        if (diff.ToLower().Contains("hard"))
          return GameDifficulties.HARD;
        if (diff.ToLower().Contains("expert"))
          return GameDifficulties.EXPERT;
        if (diff.ToLower().Contains("nolife"))
          return GameDifficulties.NOLIFE;
      }

      return GameDifficulties.NORMAL;
    }

    #endregion

    #region Handlers

    partial void modeChanged(MonoTouch.Foundation.NSObject sender)
    {
      highScoresController.SetScoreParameters(getMode(), getDifficulty());
    }

    partial void difficultyChanged(MonoTouch.Foundation.NSObject sender)
    {
      highScoresController.SetScoreParameters(getMode(), getDifficulty());
    }

    #endregion

    #region Properties
    #endregion
  }
}

