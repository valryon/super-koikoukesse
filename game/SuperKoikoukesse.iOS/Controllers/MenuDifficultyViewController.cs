
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
	public partial class MenuDifficultyViewController : UIViewController
	{
		private HelpGameDifficultiesViewController helpViewController;

		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public MenuDifficultyViewController ()
			: base (UserInterfaceIdiomIsPhone ? "MenuDifficultyView_iPhone" : "MenuDifficultyView_iPad", null)
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
			
			helpViewController = new HelpGameDifficultiesViewController ();
		}

		//partial void helpButtonPressed (MonoTouch.Foundation.NSObject sender) {
		//	showHelp();
		//}
		
		private void showHelp() {
			View.AddSubview (helpViewController.View);
		}
	}
}

