// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
  public partial class CardScoreViewController : AbstractCardViewController
  {
    #region Members

    private HighScoresControlViewController _innerController;

    #endregion

    #region Constructors

    public CardScoreViewController() : base("CardScoreView")
    {
    }

    public override void ViewWillAppear(bool animated)
    {
      base.ViewWillAppear(animated);

      if (_innerController == null)
      {
        _innerController = new HighScoresControlViewController();
        _innerController.SetScoreParameters(GameMode.SCORE, GameDifficulty.EASY);

        this.ViewScore.AddSubview(_innerController.View);
      }
    }

    #endregion

    #region Methods

    public void ForceUpdate()
    {
      _innerController.UpdateGameCenterLeaderboard();
    }

    /// <summary>
    /// Get mode from the selector
    /// </summary>
    /// <returns>The mode.</returns>
    private GameMode GetMode()
    {

      if (SelectorMode.SelectedSegment >= 0)
      {
        string mode = SelectorMode.TitleAt(SelectorMode.SelectedSegment);

        if (mode.ToLower().Contains("score"))
          return GameMode.SCORE;
        if (mode.ToLower().Contains("time"))
          return GameMode.TIME;
        if (mode.ToLower().Contains("survival"))
          return GameMode.SURVIVAL;
        if (mode.ToLower().Contains("versus"))
          return GameMode.VERSUS;
      }

      return GameMode.SCORE;
    }

    /// <summary>
    /// Get diff from the selector
    /// </summary>
    /// <returns>The difficulty.</returns>
    private GameDifficulty GetDifficulty()
    {
      if (SelectorDifficulty.SelectedSegment >= 0)
      {
        string diff = SelectorDifficulty.TitleAt(SelectorDifficulty.SelectedSegment);

        if (diff.ToLower().Contains("normal"))
          return GameDifficulty.EASY;
        if (diff.ToLower().Contains("hard"))
          return GameDifficulty.NORMAL;
        if (diff.ToLower().Contains("expert"))
          return GameDifficulty.HARD;
        if (diff.ToLower().Contains("nolife"))
          return GameDifficulty.INSANE;
      }

      return GameDifficulty.EASY;
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

