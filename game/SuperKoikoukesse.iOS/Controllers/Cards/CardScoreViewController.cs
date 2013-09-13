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

    private RecordsViewController _innerController;

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
        _innerController = new RecordsViewController();
        _innerController.SetRecords(GameMode.SCORE, GameDifficulty.EASY);

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
        return GameModeHelper.Convert(SelectorMode.SelectedSegment);
      }

      return GameMode.SCORE;
    }

    /// <summary>
    /// Get diff from the selector
    /// </summary>
    /// <returns>The difficulty.</returns>
    private GameDifficulty GetDifficulty()
    {
      if (SelectorChallenge.SelectedSegment >= 0)
      {
        return GameDifficultyHelper.Convert(SelectorChallenge.SelectedSegment);
      }

      return GameDifficulty.EASY;
    }

    #endregion

    #region Handlers

    partial void OnModeChanged(MonoTouch.Foundation.NSObject sender)
    {
      _innerController.SetRecords(GetMode(), GetDifficulty());
    }

    partial void OnDifficultyChanged(MonoTouch.Foundation.NSObject sender)
    {
      _innerController.SetRecords(GetMode(), GetDifficulty());
    }

    #endregion

    #region Properties
    #endregion
  }
}

