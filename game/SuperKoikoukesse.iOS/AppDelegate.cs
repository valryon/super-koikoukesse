using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using Superkoikoukesse.Common.Utils;
using System.IO;

namespace SuperKoikoukesse.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{

		// class-level declarations
		UIWindow window;

		private GameViewController gameViewController;
		private MenuViewController menuViewController;
		private MenuDifficultyViewController menuDifficultyViewController;
		private LoadingViewController loadingViewController;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Logger.Log (LogLevel.Info, "Launching app...");

			// Load the font once
			CustomFont = UIFont.FromName ("SG10", 16);

			// Global parameters
			EncryptionHelper.SetKey (Constants.EncryptionKey);
			ImageService.Instance.Initialize (Constants.ImagesRootLocation);

			// Try to open the database
			DatabaseService.Instance.Load (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), Constants.DatabaseLocation));
		
			// Create first view
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			//viewController = new GameViewController ();
			menuViewController = new MenuViewController ();
			window.RootViewController = menuViewController;
			window.MakeKeyAndVisible ();

			// Load configuration
			UpdateConfiguration ();

			// Register on Game Center
			PlayerService = new GameCenterService (window.RootViewController);
			PlayerService.Authenticate ();

			return true;
		}

		/// <summary>
		/// Download webservice configuration
		/// </summary>
		public void UpdateConfiguration ()
		{
			SetLoading(true);

			// Get the distant configuration
			WebserviceConfiguration configWs = new WebserviceConfiguration ();
			configWs.Request ((config) => {
				this.Configuration = config;

				SetLoading(false);

				Logger.Log (LogLevel.Info, "Configuration loaded and updated.");
			},
			(e) => {
				Logger.Log (LogLevel.Warning, "Configuration was not loaded!. " + e);

				this.Configuration = configWs.LastValidConfig;

				if(this.Configuration == null) {

					Logger.Log (LogLevel.Warning, "Using default (and bad) values!. " + e);

					this.Configuration = new GameConfiguration();
				}

				SetLoading(false);
			});
		
		}

		#region Loading

		/// <summary>
		/// Add or hide a loading view
		/// </summary>
		/// <param name="value">If set to <c>true</c> value.</param>
		public void SetLoading(bool value) {

			if (IsLoading != value) {
				IsLoading = value;
			}

			if (loadingViewController == null) {
				loadingViewController = new LoadingViewController();
			}

			BeginInvokeOnMainThread (() => {

				if(IsLoading) {
					DisplayLoading();
				}
				else {
					HideLoading();
				}
			});
		}

		/// <summary>
		/// Display the loading screen
		/// </summary>
		private void DisplayLoading() {

			// Center
//			loadingViewController.View.Frame = new System.Drawing.RectangleF (
//
//				(window.RootViewController.View.Bounds.Width/2) - (loadingViewController.View.Frame.Width/2),
//				(window.RootViewController.View.Bounds.Height/2)- (loadingViewController.View.Frame.Height/2),
//				loadingViewController.View.Frame.Width,
//				loadingViewController.View.Frame.Height
//			);

			// Fill
			loadingViewController.View.Frame = (window.RootViewController.View.Bounds);

			window.RootViewController.View.AddSubview (loadingViewController.View);
		}

		/// <summary>
		/// Hide the loading screen
		/// </summary>
		private void HideLoading() {
			loadingViewController.View.RemoveFromSuperview ();
			//loadingViewController.
		}

		#endregion

		#region Views

		public void SwitchToMenuView ()
		{
			if (menuViewController == null) {
				menuViewController = new MenuViewController ();
			}

			window.RootViewController.RemoveFromParentViewController ();
			window.RootViewController = menuViewController;
			window.MakeKeyAndVisible ();

		}

		/// <summary>
		/// Change to ingame view
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		public void SwitchToDifficultiesView (GameModes mode)
		{
			if (menuDifficultyViewController == null) {
				menuDifficultyViewController = new MenuDifficultyViewController ();
			}
			
			menuDifficultyViewController.SetGameMode (mode);
			
			window.RootViewController.RemoveFromParentViewController ();
			window.RootViewController = menuDifficultyViewController;
			window.MakeKeyAndVisible ();
			
		}

		/// <summary>
		/// Change to ingame view
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		public void SwitchToGameView (GameModes mode, GameDifficulties difficulty)
		{
			if (gameViewController == null) {
				gameViewController = new GameViewController ();
			}
			
			gameViewController.InitializeQuizz (mode, difficulty);
			
			window.RootViewController.RemoveFromParentViewController ();
			window.RootViewController = gameViewController;
			window.MakeKeyAndVisible ();
			
		}



		#endregion

		/// <summary>
		/// Font to use everywhere
		/// </summary>
		/// <value>The custom font.</value>
		public UIFont CustomFont { get; set; }

		/// <summary>
		/// Global configuration
		/// </summary>
		/// <value>The configuration.</value>
		public GameConfiguration Configuration { get; set; }

		/// <summary>
		/// Service to get player info, leaderboards, etc
		/// </summary>
		/// <value>The player service.</value>
		public PlayerService PlayerService { get; set; }

		/// <summary>
		/// Is loading something
		/// </summary>
		public bool IsLoading { get; private set; }

		protected override void Dispose (bool disposing)
		{
			if (gameViewController != null) {
				gameViewController.Dispose ();
			}
			if (menuViewController != null) {
				menuViewController.Dispose ();
			}

			base.Dispose (disposing);
		}
	}
}

