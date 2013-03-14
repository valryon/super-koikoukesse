
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.IO;

namespace SuperKoikoukesse.iOS
{
	public partial class MenuViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MenuViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MenuViewController_iPhone" : "MenuViewController_iPad", null)
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

			// Initialize game database id first launch
			if (DatabaseService.Instance.Exists == false) {
				
				// Load gamedb.xml
				String xmlDatabase = File.ReadAllText (@"database/gamedb.xml");
				
				DatabaseService.Instance.InitializeFromXml (xmlDatabase);
			}

			// Set fonts manually because Interface Builder is a dick.
			var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
			scoreAttackButton.TitleLabel.Font = appDelegate.CustomFont;
			timeAttackButton.TitleLabel.Font = appDelegate.CustomFont;
			survivalButon.TitleLabel.Font = appDelegate.CustomFont;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			debugButton.SetTitle ("DEBUG: " + Constants.DebugMode, UIControlState.Normal);
		}


		partial void scoreAttackButtonPressed (MonoTouch.Foundation.NSObject sender) {
			
			// Launch game
			// TODO Choose difficulty
			
			var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToGameView(GameModes.ScoreAttack, GameDifficulties.Easy);
		}

		partial void timeAttackButtonPressed (MonoTouch.Foundation.NSObject sender) {
			
			// Launch game
			// TODO Choose difficulty
			
			var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToGameView(GameModes.TimeAttack, GameDifficulties.Easy);
		}

		partial void survivalButtonPressed (MonoTouch.Foundation.NSObject sender) {
			
			// Launch game
			// TODO Choose difficulty
			
			var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToGameView(GameModes.Survival, GameDifficulties.Hard);
		}

		/// <summary>
		/// Force config reload (DEBUG)
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void configButtonPressed (MonoTouch.Foundation.NSObject sender) {
			var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
			appDelegate.UpdateConfiguration();
		}

		partial void debugButtonPressed (MonoTouch.Foundation.NSObject sender) {
			Constants.DebugMode = !Constants.DebugMode;
			debugButton.SetTitle ("DEBUG: " + Constants.DebugMode, UIControlState.Normal);
			Logger.Log(LogLevel.Info,"Debug mode? " + Constants.DebugMode);
		}
	}
}

