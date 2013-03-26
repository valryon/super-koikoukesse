
using System;
using System.Linq;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.Text;

namespace SuperKoikoukesse.iOS
{
	public partial class CreditsViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
//			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
			get { return true; }
		}

		public CreditsViewController ()
			: base (UserInterfaceIdiomIsPhone ? "CreditsView_iPhone" : "CreditsView_iPad", null)
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
			
			// Load all publishers name and display them
			StringBuilder credits = new StringBuilder();

			var games =DatabaseService.Instance.ReadGames ();
			var publishers = games.Select (g => g.Publisher).Distinct ().ToList ();

			foreach (var publisher in publishers) {
				credits.Append(publisher+ " ");
			}

			creditsLabel.Text = credits.ToString();
		}

		partial void backButtonPressed (MonoTouch.Foundation.NSObject sender) {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToMenuView ();
		}
	}
}

