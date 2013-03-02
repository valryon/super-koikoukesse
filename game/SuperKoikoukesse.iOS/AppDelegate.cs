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
	public enum GameState
	{
		Menu,
		Game,
	}

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
			CustomFont = UIFont.FromName ("SG10", 14);

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

			return true;
		}

		/// <summary>
		/// Chance current view
		/// </summary>
		/// <param name="state">State.</param>
		public void SwitchView (GameState state)
		{

			UIViewController newViewController = null;

			switch (state) {
			case GameState.Game:
				if (gameViewController == null) {
					gameViewController = new GameViewController ();
				}
				gameViewController.InitializeQuizz();
				newViewController = gameViewController;
				break;

			case GameState.Menu:
				if (menuViewController == null) {
					menuViewController = new MenuViewController ();
				}
				
				newViewController = menuViewController;
				break;
			}

			if (menuViewController != null) {

				window.RootViewController.RemoveFromParentViewController ();
				window.RootViewController = newViewController;
				window.MakeKeyAndVisible ();
			}
		}

		/// <summary>
		/// Font to use everywhere
		/// </summary>
		/// <value>The custom font.</value>
		public UIFont CustomFont {get;set;}

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

