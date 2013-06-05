
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public partial class ScoreViewController : UIViewController
	{
		private Quizz quizz;
		private HighScoresControlViewController highScoreControl;

		public ScoreViewController (Quizz q) 
			: base (null, null)
		{
			quizz = q;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return  UIInterfaceOrientationMask.LandscapeLeft | UIInterfaceOrientationMask.LandscapeRight;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			createView ();

			SetViewDataFromQuizz ();
		}

		private void createView() 
		{
			View = ViewTools.CreateUIView ();

			// Common
			// ------------------------------------------

			// Game over label
			UILabel titleLabel = ViewTools.Createlabel(new RectangleF (10, 10, 150, 50), Localization.Get("game.over"));
			View.AddSubview (titleLabel);

			// Quit button
			UIButton quitButton = ViewTools.CreateButton (
				new RectangleF(10, 100, 200, 80), 
				Localization.Get("button.quit"),
				menuButtonPressed
				);

			View.AddSubview (quitButton);

			// Versus
			// ------------------------------------------
			if(quizz.Mode == GameModes.Versus) 
			{

				// Game center state

			}
			// Solo
			// ------------------------------------------
			else 
			{
				// Score

				// Game center

				// Retry
				UIButton retryButton = ViewTools.CreateImportantButton (
					new RectangleF(10, 200, 200, 80), 
					Localization.Get("button.retry"),
					retryButtonPressed
					);

				View.AddSubview (retryButton);
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

