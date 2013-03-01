
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

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
			
			// Perform any additional setup after loading the view, typically from a nib.
		}


		partial void scoreAttackButtonPressed (MonoTouch.Foundation.NSObject sender) {

			// Launch game
			// TODO Choose difficulty

			var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchView(GameState.Game);
		}
	}
}

