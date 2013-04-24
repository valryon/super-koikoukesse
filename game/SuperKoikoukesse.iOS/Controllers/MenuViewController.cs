
using System;
using System.Collections.Generic;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.IO;

namespace SuperKoikoukesse.iOS
{
	public partial class MenuViewController : UIViewController
	{
		private List<UIViewController> panels;
		private MenuDifficultyViewController difficultyViewController;
		private PagerMenuHighscoresViewController highScorePanel;

		public MenuViewController ()
			: base ("MenuView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
			panels = new List<UIViewController> ();
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return  UIInterfaceOrientationMask.LandscapeLeft | UIInterfaceOrientationMask.LandscapeRight;
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

			// Hide credits and coins until player profile is loaded
			coinsLabel.Hidden = true;
			coinsImage.Hidden = true;
			creditsLabel.Hidden = true;
			creditsImage.Hidden = true;

			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.LoadPlayerProfile();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			if (panels.Count == 0) {
				// We need auto layout to be set up, so we can create panels only here
				createPanels ();
			}

			debugButton.SetTitle (Constants.DebugMode + "", UIControlState.Normal);

			UpdateViewWithPlayerInfos ();
		}

		/// <summary>
		/// Update counters
		/// </summary>
		public void UpdateViewWithPlayerInfos ()
		{
			// Show infos is they were hidden
			if (coinsLabel.Hidden) {
				coinsLabel.Hidden = false;
				coinsImage.Hidden = false;
				creditsLabel.Hidden = false;
				creditsImage.Hidden = false;

				// Fade in
				coinsLabel.Alpha = 0f;
				coinsImage.Alpha = 0f;
				creditsLabel.Alpha = 0f;
				creditsImage.Alpha = 0f;

				UIView.Animate (2f, () => {
					coinsLabel.Alpha = 1f;
					coinsImage.Alpha = 1f;
					creditsLabel.Alpha = 1f;
					creditsImage.Alpha = 1f;
				});
			}

			// Load the player from db
			Player profile = ProfileService.Instance.CachedPlayer;

			// Display credits and coins
			if (profile != null) {
				creditsLabel.Text = profile.Credits.ToString ();
				coinsLabel.Text = profile.Coins.ToString ();
			}

			// Update leaderboards ?
			if(highScorePanel != null) {
				highScorePanel.ForceUpdate();
			}

		}

		#region Scroll view and pagination

		private void createPanels ()
		{
			pageControl.ValueChanged += (object sender, EventArgs e) => {
				setScrollViewToPage (pageControl.CurrentPage);
			};
			scrollView.DecelerationEnded += (object sender, EventArgs e) => {
				double page = Math.Floor ((scrollView.ContentOffset.X - scrollView.Frame.Width / 2) / scrollView.Frame.Width) + 1;
				
				pageControl.CurrentPage = (int)page;
			};

			highScorePanel = null;
			panels.Clear ();

			// Credits
			PagerMenuInfosViewController infos = new PagerMenuInfosViewController ();
			panels.Add (infos);

			// Highscores
			highScorePanel = new PagerMenuHighscoresViewController ();
			panels.Add (highScorePanel);

			// Build for each modes
			// -- Score attack
			PagerMenuModeViewController scoreAttackMode = new PagerMenuModeViewController (GameModes.ScoreAttack);
			scoreAttackMode.GameModeSelected += HandleGameModeSelected;
			panels.Add (scoreAttackMode);

			// -- Time attack
			PagerMenuModeViewController timeAttackMode = new PagerMenuModeViewController (GameModes.TimeAttack);
			timeAttackMode.GameModeSelected += HandleGameModeSelected;
			panels.Add (timeAttackMode);

			// -- Survival
			PagerMenuModeViewController survivalMode = new PagerMenuModeViewController (GameModes.Survival);
			survivalMode.GameModeSelected += HandleGameModeSelected;
			panels.Add (survivalMode);

			// -- Versus
//			PagerMenuModeViewController versusMode = new PagerMenuModeViewController (GameModes.Versus);
//			versusMode.GameModeSelected += HandleGameModeSelected;
//			panels.Add (versusMode);

			int count = panels.Count;
			RectangleF scrollFrame = scrollView.Frame;

			scrollFrame.Width = scrollFrame.Width * count;
			scrollView.ContentSize = scrollFrame.Size;

			for (int i = 0; i < count; i++) {

				// Compute location and size
				RectangleF frame = scrollView.Frame;
				PointF location = new PointF ();
				location.X = frame.Width * i;
				frame.Location = location;

				panels [i].View.Frame = frame;

				// Add to scroll and paging
				scrollView.AddSubview (panels [i].View);
			}

			pageControl.Pages = count;

			// Set 3rd page as the first displayed
			int firstDisplayedPageNumber = 2;
			pageControl.CurrentPage = firstDisplayedPageNumber;

			setScrollViewToPage (firstDisplayedPageNumber);
		}

		private void setScrollViewToPage (int page)
		{
			scrollView.SetContentOffset (new PointF (page * scrollView.Frame.Width, 0), true);
		}

		#endregion

		void HandleGameModeSelected (GameModes m)
		{
			// Enough credits?
			if (ProfileService.Instance.CachedPlayer.Credits > 0) {

				// Display difficulty view
				if (difficultyViewController == null) {
					difficultyViewController = new MenuDifficultyViewController ();
					difficultyViewController.DifficultySelected += (GameModes mode, GameDifficulties difficulty) => {
						var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 

						Filter filter = null;
						if(difficultyViewController.StunfestMode) {
							filter = new Filter();
							filter.StunfestMode();
						}

						appDelegate.SwitchToGameView (mode, difficulty, filter);
					}; 	
				}

				difficultyViewController.SetMode (m);
				difficultyViewController.View.Frame = View.Frame;

				View.AddSubview (difficultyViewController.View);
			} else {
				Dialogs.ShowNoMoreCreditsDialogs ();
			}
		}

		/// <summary>
		/// Force config reload (DEBUG)
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void configButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.UpdateConfiguration (null);
		}

		partial void debugButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			Constants.DebugMode = !Constants.DebugMode;
			debugButton.SetTitle (Constants.DebugMode + "", UIControlState.Normal);
			Logger.Log (LogLevel.Info, "Debug mode? " + Constants.DebugMode);
		}

		partial void paramsButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.ShowOptions ();
		}

		partial void shopButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToShopView ();
		}

		partial void creditsButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			ProfileService.Instance.AddCreditsDebug (Constants.BaseCredits);
		}

		partial void coinsButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			ProfileService.Instance.AddCoins (Constants.BaseCoins);
		}
	}
}

