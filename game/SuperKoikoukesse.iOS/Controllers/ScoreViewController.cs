
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

		public ScoreViewController () 
			: base ("ScoreView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{

		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			SetViewDataFromQuizz ();
		}

		/// <summary>
		/// Sets the quizz data.
		/// </summary>
		/// <param name="q">Finished quizz</param>
		public void SetQuizzData (Quizz q)
		{
			this.quizz = q;

			SetViewDataFromQuizz ();
		}

		/// <summary>
		///  Display Game center leaderboards
		/// </summary>
		public void DisplayGameCenterLeaderboards ()
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.ShowLeaderboards (
				ProfileService.Instance.AuthenticatedPlayer.GetLeaderboardId (quizz.Mode, quizz.Difficulty),
				() => {
				
			}
			);
		}

		private void SetViewDataFromQuizz ()
		{
			if (IsViewLoaded) {
				this.scoreLabel.Text = quizz.Score.ToString ("00000000");
				this.modeLabel.Text = quizz.Mode.ToString ();
				this.difficultyLabel.Text = quizz.Difficulty.ToString ();
			}
		}
			                                        
		partial void retryButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToGameView (quizz.Mode, quizz.Difficulty);
		}
			
		partial void menuButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToMenuView ();
		}
	}
}
