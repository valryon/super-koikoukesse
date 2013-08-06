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

    private HighScoresControlViewController _innerController;

    #endregion

    #region Constructors

    public CardScoreViewController()
      : base ("CardScoreView" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
    }

    public override void ViewWillAppear(bool animated)
    {
      base.ViewWillAppear(animated);

      if (_innerController == null)
      {
        _innerController = new HighScoresControlViewController();
        _innerController.SetScoreParameters(GameModes.SCORE_ATTACK, GameDifficulties.NORMAL);

        this.ViewScore.AddSubview(_innerController.View);
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
      _innerController.UpdateGameCenterLeaderboard();
    }

    /// <summary>
    /// Get mode from the selector
    /// </summary>
    /// <returns>The mode.</returns>
    private GameModes GetMode()
    {

      if (SelectorMode.SelectedSegment >= 0)
      {
        string mode = SelectorMode.TitleAt(SelectorMode.SelectedSegment);

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
    private GameDifficulties GetDifficulty()
    {
      if (SelectorDifficulty.SelectedSegment >= 0)
      {
        string diff = SelectorDifficulty.TitleAt(SelectorDifficulty.SelectedSegment);

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

    partial void OnModeChanged(MonoTouch.Foundation.NSObject sender)
    {
      _innerController.SetScoreParameters(GetMode(), GetDifficulty());
    }

    partial void OnDifficultyChanged(MonoTouch.Foundation.NSObject sender)
    {
      _innerController.SetScoreParameters(GetMode(), GetDifficulty());
    }

    #endregion

    #region Properties
    #endregion
  }
}

