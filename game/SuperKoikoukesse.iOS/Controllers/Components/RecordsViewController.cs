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
    #region Members

    private static int scoreLinesCount = 5;
    private GameMode mode;
    private GameDifficulty difficulty;
    private int? newScoreRank, newScoreValue;

    #endregion

    #region Constructors

    public RecordsViewController() : base("RecordsView" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
    {
    }

    /// <summary>
    /// Refresh scores each time we display the view
    /// </summary>
    /// <param name="animated">If set to <c>true</c> animated.</param>
    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      setViewValues();
    }

    #endregion

    #region Methods

    public void SetScoreParameters(GameMode m, GameDifficulty d, int? newScoreRank = null, int? newScoreValue = null)
    {
      this.mode = m;
      this.difficulty = d;
      this.newScoreRank = newScoreRank;
      this.newScoreValue = newScoreValue;

      if (IsViewLoaded)
      {
        setViewValues();
      }
    }

    private void setViewValues()
    {
      // No new score
      LabelRankCurrent.Hidden = true;
      LabelRankCurrentScore.Hidden = true;

      // New score
      if (newScoreRank.HasValue && newScoreValue.HasValue)
      {

        // TODO if the current score is in the top 5, emphasize it
        // otherwise, add it at the end


        // Not in top : display additionnal line
        //if (newScoreRank.Value >= scoreLinesCount) {
        LabelRankCurrent.Hidden = false;
        LabelRankCurrentScore.Hidden = false;

        // TODO remove this line:
        LabelRankCurrent.Text = "mon score";

        // TODO uncomment this line:
        //          rankLastLabel.Text = newScoreRank.Value.ToString()+".";


        LabelRankCurrentScore.Text = newScoreValue.Value.ToString("000000");
        //        } else {
        //          // In tops : TODO emphasize the corresponding one of the top 5 labels
        //          rankLastLabel.Hidden = true;
        //          rankLastScoreLabel.Hidden = true;
        //        }
      }

      // Get local highscores
      LocalScore[] localScores = GameDatabase.Instance.GetLocalScores(mode, difficulty, scoreLinesCount);

      // Fill labels...
      if (localScores.Length > 0)
      {
        LabelRank1Score.Text = localScores[0].Score.ToString("000000");
        LabelRank1.Hidden = false;
        LabelRank1Score.Hidden = false;
      }
      else
      {
        LabelRank1.Hidden = true;
        LabelRank1Score.Hidden = true;
      }
      if (localScores.Length > 1)
      {
        LabelRank2Score.Text = localScores[1].Score.ToString("000000");
        LabelRank2.Hidden = false;
        LabelRank2Score.Hidden = false;
      }
      else
      {
        LabelRank2.Hidden = true;
        LabelRank2Score.Hidden = true;
      }
      if (localScores.Length > 2)
      {
        LabelRank3Score.Text = localScores[2].Score.ToString("000000");
        LabelRank3.Hidden = false;
        LabelRank3Score.Hidden = false;
      }
      else
      {
        LabelRank3.Hidden = true;
        LabelRank3Score.Hidden = true;
      }
      if (localScores.Length > 3)
      {
        LabelRank4Score.Text = localScores[3].Score.ToString("000000");
        LabelRank4.Hidden = false;
        LabelRank4Score.Hidden = false;
      }
      else
      {
        LabelRank4.Hidden = true;
        LabelRank4Score.Hidden = true;
      }
      if (localScores.Length > 4)
      {
        LabelRank5Score.Text = localScores[4].Score.ToString("000000");
        LabelRank5.Hidden = false;
        LabelRank5Score.Hidden = false;
      }
      else
      {
        LabelRank5.Hidden = true;
        LabelRank5Score.Hidden = true;
      }

      // Get game center scores
      UpdateGameCenterLeaderboard();
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

        PlayerCache.Instance.AuthenticatedPlayer.GetBestScoreAndRank(mode, difficulty, (rank, score) => {

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

    partial void OnLeaderboardsTouched(NSObject sender)
    {
      var app = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      app.ShowLeaderboards(PlayerCache.Instance.AuthenticatedPlayer.GetLeaderboardId(mode, difficulty), null);
    }

    #endregion

    #region Properties
    #endregion
  }
}

