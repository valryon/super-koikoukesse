
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.Collections.Generic;

namespace SuperKoikoukesse.iOS
{
	public partial class HighScoresControlViewController : UIViewController
	{
		private static int scoreLinesCount = 5;
		private GameModes mode;
		private GameDifficulties difficulty;
		private int? newScoreRank, newScoreValue;

		public HighScoresControlViewController () : base ("HighScoresControlView", null)
		{
		}

		public void SetScoreParameters (GameModes m, GameDifficulties d, int? newScoreRank = null, int? newScoreValue = null)
		{
			this.mode = m;
			this.difficulty = d;
			this.newScoreRank = newScoreRank;
			this.newScoreValue = newScoreValue;

			if (IsViewLoaded) {
				setViewValues ();
			}
		}

		/// <summary>
		/// Refresh scores each time we display the view
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			setViewValues ();
		}

		private void setViewValues ()
		{
			// No new score
			rankLastLabel.Hidden = true;
			rankLastScoreLabel.Hidden = true;

			// New score
			if (newScoreRank.HasValue && newScoreValue.HasValue) {

				// TODO if the current score is in the top 5, emphasize it
				// otherwise, add it at the end


				// Not in top : display additionnal line
				//if (newScoreRank.Value >= scoreLinesCount) {
					rankLastLabel.Hidden = false;
					rankLastScoreLabel.Hidden = false;

					// TODO remove this line:
					rankLastLabel.Text = "mon score";

					// TODO uncomment this line:
//					rankLastLabel.Text = newScoreRank.Value.ToString()+".";


					rankLastScoreLabel.Text = newScoreValue.Value.ToString("000000");
//				} else {
//					// In tops : TODO emphasize the corresponding one of the top 5 labels
//					rankLastLabel.Hidden = true;
//					rankLastScoreLabel.Hidden = true;
//				}
			}
			
			// Get local highscores
			LocalScore[] localScores = GameDatabase.Instance.GetLocalScores (mode, difficulty, scoreLinesCount);
			
			// Fill labels...
			if (localScores.Length > 0) {
				rank1ScoreLabel.Text = localScores [0].Score.ToString ("000000");
				rank1Label.Hidden = false;
				rank1ScoreLabel.Hidden = false;
			} else {
				rank1Label.Hidden = true;
				rank1ScoreLabel.Hidden = true;
			}
			if (localScores.Length > 1) {
				rank2ScoreLabel.Text = localScores [1].Score.ToString ("000000");
				rank2Label.Hidden = false;
				rank2ScoreLabel.Hidden = false;
			} else {
				rank2Label.Hidden = true;
				rank2ScoreLabel.Hidden = true;
			}
			if (localScores.Length > 2) {
				rank3ScoreLabel.Text = localScores [2].Score.ToString ("000000");
				rank3Label.Hidden = false;
				rank3ScoreLabel.Hidden = false;
			} else {
				rank3Label.Hidden = true;
				rank3ScoreLabel.Hidden = true;
			}
			if (localScores.Length > 3) {
				rank4ScoreLabel.Text = localScores [3].Score.ToString ("000000");
				rank4Label.Hidden = false;
				rank4ScoreLabel.Hidden = false;
			} else {
				rank4Label.Hidden = true;
				rank4ScoreLabel.Hidden = true;
			}
			if (localScores.Length > 4) {
				rank5ScoreLabel.Text = localScores [4].Score.ToString ("000000");
				rank5Label.Hidden = false;
				rank5ScoreLabel.Hidden = false;
			} else {
				rank5Label.Hidden = true;
				rank5ScoreLabel.Hidden = true;
			}

			// Get game center scores
			UpdateGameCenterLeaderboard ();
		}

		/// <summary>
		/// Update game center part of the leaderboards. 
		/// We might not be logged in at first so we want to be able to do it after authentication.
		/// </summary>
		public void UpdateGameCenterLeaderboard ()
		{

			gameCenterPanel.Hidden = !PlayerCache.Instance.AuthenticatedPlayer.IsAuthenticated;

			this.onlineRankValueLabel.Hidden = true;
			this.onlineRankValueLabel.Hidden = true;
			
			if (gameCenterPanel.Hidden == false) {
				
				PlayerCache.Instance.AuthenticatedPlayer.GetBestScoreAndRank (mode, difficulty, (rank, score) => {
					
					BeginInvokeOnMainThread (() => {
						this.onlineRankLabel.Hidden = false;
						this.onlineRankLabel.Text = rank.ToString ();
						this.onlineRankValueLabel.Hidden = false;
						this.onlineRankValueLabel.Text = score.ToString ("000000");
					});
				});
			}
		}

		partial void leaderboardsButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.ShowLeaderboards (PlayerCache.Instance.AuthenticatedPlayer.GetLeaderboardId (mode, difficulty), null);
		}
	}
}

