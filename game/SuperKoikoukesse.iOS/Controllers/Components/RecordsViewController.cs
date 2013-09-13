// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
  public partial class RecordsViewController : UIViewController
  {
    #region Constants

    /// <summary>
    /// The number of lines to display.
    /// </summary>
    private const int NUMBER_OF_LINES = 5;

    #endregion

    #region Members

    /// <summary>
    /// Mode.
    /// </summary>
    private GameMode _mode;

    /// <summary>
    /// Difficulty.
    /// </summary>
    private GameDifficulty _difficulty;

    /// <summary>
    /// The current rank of the player (after a game).
    /// </summary>
    private int? _currentRank;

    /// <summary>
    /// The current value of the player (after a game).
    /// </summary>
    private int? _currentScore;

    #endregion

    #region Constructors

    public RecordsViewController() 
      : base("RecordsView" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
    }

    /// <summary>
    /// Refresh scores each time we display the view
    /// </summary>
    /// <param name="animated">If set to <c>true</c> animated.</param>
    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      SetValues();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets the records for a mode and difficulty.
    /// </summary>
    /// <param name="mode">Mode.</param>
    /// <param name="difficulty">Difficulty.</param>
    /// <param name="currentRank">Current rank.</param>
    /// <param name="currentScore">Current score.</param>
    public void SetRecords(GameMode mode, GameDifficulty difficulty, int? currentRank = null, int? currentScore = null)
    {
      this._mode = mode;
      this._difficulty = difficulty;
      this._currentRank = currentRank;
      this._currentScore = currentScore;

      if (IsViewLoaded)
      {
        SetValues();
      }
    }

    /// <summary>
    /// Set all the values.
    /// </summary>
    private void SetValues()
    {
      // Hide all the labels
      HideLocalRanks();

      // Get local highscores
      var localScores = GameDatabase.Instance.GetLocalScores(_mode, _difficulty, NUMBER_OF_LINES);

      // Set local scores
      UpdateLocals(localScores);

      // Set current score
      UpdateCurrent(LabelRankCurrent, LabelRankCurrentScore, _currentRank, _currentScore);

      // Set game center
      UpdateGameCenterLeaderboard();
    }

    /// <summary>
    /// Updates the row containing the current score of the player (at the end of a game).
    /// </summary>
    /// <param name="labelRank">Label rank.</param>
    /// <param name="labelScore">Label score.</param>
    /// <param name="rank">Rank.</param>
    /// <param name="score">Score.</param>
    private void UpdateCurrent(UILabel labelRank, UILabel labelScore, int? rank, int? score)
    {
      // New score
      if (rank.HasValue && score.HasValue)
      {
        if (rank.Value <= NUMBER_OF_LINES)
        {
          // Emphasize the row instead of displaying it beside.
        }
        else
        {
          UpdateLocalRank(labelRank, labelScore, score.Value, rank.Value);
        }
      }
    }

    /// <summary>
    /// Updates local scores.
    /// </summary>
    /// <param name="locals">List of local scores.</param>
    private void UpdateLocals(LocalScore[] locals)
    {
      // Reveal field depending on the available scores.
      if (locals.Length > 0)
        UpdateLocalRank(LabelRank1, LabelRank1Score, locals[0].Score);

      if (locals.Length > 1)
        UpdateLocalRank(LabelRank2, LabelRank2Score, locals[1].Score);

      if (locals.Length > 2)
        UpdateLocalRank(LabelRank3, LabelRank3Score, locals[2].Score);

      if (locals.Length > 3)
        UpdateLocalRank(LabelRank4, LabelRank4Score, locals[3].Score);

      if (locals.Length > 4)
        UpdateLocalRank(LabelRank5, LabelRank5Score, locals[4].Score);

    }

    /// <summary>
    /// Updates a row.
    /// </summary>
    /// <param name="labelRank">Label rank.</param>
    /// <param name="labelScore">Label score.</param>
    /// <param name="score">Score.</param>
    /// <param name="rank">Rank (optional)</param>
    private void UpdateLocalRank(UILabel labelRank, UILabel labelScore, int score, int? rank = null)
    {
      // Set the rank
      if (rank.HasValue)
        labelRank.Text = rank.Value + ".";

      // Set the score
      labelScore.Text = score.ToString();

      labelRank.Hidden = false;
      labelScore.Hidden = false;
    }

    /// <summary>
    /// Hides the local ranks (including the current score).
    /// </summary>
    private void HideLocalRanks()
    {
      // Player current score
      LabelRankCurrent.Hidden = true;
      LabelRankCurrentScore.Hidden = true;

      // 1
      LabelRank1.Hidden = true;
      LabelRank1Score.Hidden = true;

      // 2
      LabelRank2.Hidden = true;
      LabelRank2Score.Hidden = true;

      // 3 
      LabelRank3.Hidden = true;
      LabelRank3Score.Hidden = true;

      // 4
      LabelRank4.Hidden = true;
      LabelRank4Score.Hidden = true;

      // 5
      LabelRank5.Hidden = true;
      LabelRank5Score.Hidden = true;
    }

    /// <summary>
    /// Update game center part of the leaderboards. 
    /// We might not be logged in at first so we want to be able to do it after authentication.
    /// </summary>
    public void UpdateGameCenterLeaderboard()
    {
      ViewGameCenter.Hidden = !PlayerCache.Instance.AuthenticatedPlayer.IsAuthenticated;

      this.LabelOnlineRank.Hidden = true;
      this.LabelOnlineScore.Hidden = true;

      if (ViewGameCenter.Hidden == false)
      {

        PlayerCache.Instance.AuthenticatedPlayer.GetBestScoreAndRank(_mode, _difficulty, (rank, score) => {

          BeginInvokeOnMainThread(() => {
            this.LabelOnlineRank.Hidden = false;
            this.LabelOnlineRank.Text = rank.ToString();
            this.LabelOnlineScore.Hidden = false;
            this.LabelOnlineScore.Text = score.ToString("000000");
          });
        });
      }
    }

    #endregion

    #region Handlers

    /// <summary>
    /// Handler when the leaderboards button is touched.
    /// </summary>
    /// <param name="sender">Sender.</param>
    partial void OnLeaderboardsTouched(NSObject sender)
    {
      var app = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      app.ShowLeaderboards(PlayerCache.Instance.AuthenticatedPlayer.GetLeaderboardId(_mode, _difficulty), null);
    }

    #endregion

    #region Properties
    #endregion
  }
}

