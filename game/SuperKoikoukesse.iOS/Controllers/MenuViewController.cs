
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
		}


		partial void scoreAttackButtonPressed (MonoTouch.Foundation.NSObject sender) {

			// Launch game
			// TODO Choose difficulty

			var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchView(GameState.Game);
		}

		/// <summary>
		/// Force config reload (DEBUG)
		/// </summary>
		/// <param name="sender">Sender.</param>
		partial void configButtonPressed (MonoTouch.Foundation.NSObject sender) {
			var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
			appDelegate.UpdateConfiguration();
		}
	}
}

