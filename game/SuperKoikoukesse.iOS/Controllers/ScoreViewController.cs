using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using MonoTouch.GameKit;
using System.Collections.Generic;

namespace SuperKoikoukesse.iOS
{
	public partial class ScoreViewController : UIViewController
	{
		private Quizz quizz;
		private static Dictionary<string, UIImage> playerImageCache;

		public ScoreViewController (Quizz q) 
			: base (null, null)
		{
			quizz = q;
			playerImageCache = new Dictionary<string, UIImage> ();
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
      return AppDelegate.HasSupportedInterfaceOrientations();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			createView ();

			SetViewDataFromQuizz ();
		}

		private void createView ()
		{
			View = ViewTools.CreateUIView ();

			// Common
			// ------------------------------------------

			// Game over label
			UILabel titleLabel = ViewTools.Createlabel (new RectangleF (10, 10, 150, 50), Localization.Get ("game.over"));
			View.AddSubview (titleLabel);

			// Mode and difficulty
			UILabel modeLabel = ViewTools.Createlabel (new RectangleF (10, 60, 150, 50), Localization.Get (quizz.Mode.ToString().ToLower() + ".title"));
			View.AddSubview (modeLabel);
			UILabel difficultyLabel = ViewTools.Createlabel (new RectangleF (10, 110, 150, 50), Localization.Get (quizz.Difficulty.ToString().ToLower() +  ".title"));
			View.AddSubview (difficultyLabel);

			// Quit button
			UIButton quitButton = ViewTools.CreateButton (
				new RectangleF (10, 500, 200, 80), 
				Localization.Get ("button.quit"),
				menuButtonPressed
			);

			View.AddSubview (quitButton);

			if (quizz.Mode == GameModes.Versus) {

				// Game center versus
				// ------------------------------------------
				VersusMatch match = ProfileService.Instance.AuthenticatedPlayer.CurrentMatch;

				// Game center match status
				if (match.IsEnded) {
					// -- Match is over
				} 
				else if (match.IsPlayerTurn(ProfileService.Instance.AuthenticatedPlayer.PlayerId) == false) {
					// -- Player is waiting for the opponent to play
				}
				else {
					// We should never be here but check...
					Logger.E( "Match not ended and player turn: wtf are you doing in score view?");
				}

				// Load players avatar
				if (playerImageCache.ContainsKey (match.Player1Id) == false) {
					GKPlayer.LoadPlayersForIdentifiers (
						new string[]{match.Player1Id},
						playersLoaded
					);
				}
				if (playerImageCache.ContainsKey (match.Player2Id) == false) {
					GKPlayer.LoadPlayersForIdentifiers (
						new string[]{match.Player2Id},
						playersLoaded
					);
				}

				// Game infos
				// There is only two turns but here it's a generic data structure
				foreach (var turn in match.Turns) 
				{
					string playerId = turn.PlayerId;
					int score = turn.Score;

					UIImage avatar = null;
					if (playerImageCache.TryGetValue (playerId, out avatar) == false) {
						// Default avatar
						avatar = new UIImage ();
					}
				}
			}
			else 
			{
				// Solo
				// ------------------------------------------ 

				// Score
				// -- Title
				UILabel localScoreTitle = ViewTools.Createlabel (new RectangleF (400, 50, 150, 50), Localization.Get ("scores.local"));
				View.AddSubview (localScoreTitle);

				// -- Current score
				UILabel currentScore = ViewTools.Createlabel (new RectangleF (400, 50, 150, 50), Localization.Get ("scores.local"));
				View.AddSubview (localScoreTitle);

				// Game center scores

				// Retry
				UIButton retryButton = ViewTools.CreateImportantButton (
					new RectangleF (10, 700, 200, 80), 
					Localization.Get ("button.retry"),
					retryButtonPressed
				);

				View.AddSubview (retryButton);
			}
		}

		
		private void playersLoaded(GKPlayer[] players, NSError error) {

			foreach(GKPlayer player in players) {
				player.LoadPhoto(GKPhotoSize.Normal, (image, loadImageError) => {

					playerImageCache.Add(player.PlayerID, image);

					BeginInvokeOnMainThread(() => {
						// TODO Refresh images
					});
				});
			}
		}

		private void SetViewDataFromQuizz ()
		{
			if (IsViewLoaded) {
//				this.coinsLabel.Text = quizz.EarnedCoins.ToString ();

				// Highscore control
//				highScoreControl.SetScoreParameters (quizz.Mode, quizz.Difficulty, quizz.RankForLastScore, quizz.Score);
			}

		}

		void retryButtonPressed ()
		{
			if (ProfileService.Instance.CachedPlayer.Credits > 0) {
				var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
				appDelegate.SwitchToGameView (quizz.Mode, quizz.Difficulty, quizz.Filter);
			} else {
				Dialogs.ShowNoMoreCreditsDialogs ();
			}
		}

		void menuButtonPressed ()
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToMenuView ();
		}
	}
}

