using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public partial class VersusNewMatchViewController : UIViewController
	{
		private GameMode mSelectedMode;
		private GameDifficulties mSelectedDifficulty;
		private Filter mSelectedFilter;

		public VersusNewMatchViewController (IntPtr handle) : base (handle)
		{
		}

		partial void OnGoTouched (MonoTouch.Foundation.NSObject sender)
		{
			PerformSegue ("VersusToNewVersus", this);

			PlayerCache.Instance.AuthenticatedPlayer.NewMatch (
				// Match found
				(match) => {

				// Ensure it's a new match
				if (match.IsFirstTurn) {
					LaunchGame (GameMode.SCORE, GameDifficulties.NORMAL, new Filter ());
				} 
//					else {
//						var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
//
//						if (match.IsEnded) {
//							// See the final score
//							Dialogs.ShowMatchEnded ();
//						} else {
//							if (match.IsPlayerTurn (PlayerCache.Instance.AuthenticatedPlayer.PlayerId)) {
//								// Player turn
//								LaunchGame (mode, match.Difficulty, match.Filter);
//							} else {
//								// TODO Other player turn: display last score?
//								Dialogs.ShowNotYourTurn ();
//							}
//						}
//					}
			},
			// Cancel
				() => {
				// Nothing, controller is already dismissed
			},
			// Error
				() => {
				// Display an error dialog?
				UIAlertView alert = new UIAlertView (
					"Une erreur est survenue",
					"Nous n'avons pas pu démarrer une nouvelle partie car une erreur est survenue..",
					null,
					"Ok");

				alert.Show ();
			},
			// Player quit
				() => {
				// Kill the game? Inform the player?
			}
			);

		}

		public void LaunchGame (GameMode mode, GameDifficulties difficulty, Filter filter)
		{
			mSelectedMode = mode;
			mSelectedDifficulty = difficulty;
			mSelectedFilter = filter;

			if (mSelectedFilter == null) {
				mSelectedFilter = new Filter ("0", "Siphon filter", "defaultIcon");
			}

			InvokeInBackground (() => {
				int gamesCount = mSelectedFilter.Load ();

				BeginInvokeOnMainThread (() => {
					if (gamesCount < 30) {
						Dialogs.ShowDebugFilterTooRestrictive ();
					} else {
						PerformSegue ("MenuToGame", this);
					}
				});
			});
		}
	}
}
