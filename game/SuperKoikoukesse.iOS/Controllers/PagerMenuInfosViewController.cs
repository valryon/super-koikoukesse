
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
	public partial class PagerMenuInfosViewController : UIViewController
	{
		public PagerMenuInfosViewController () 
			: base ("PagerMenuInfosView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return  UIInterfaceOrientationMask.LandscapeLeft | UIInterfaceOrientationMask.LandscapeRight;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		partial void creditsButtonPressed (MonoTouch.Foundation.NSObject sender) {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToCreditsView ();
		}

		partial void shopButtonPressed (MonoTouch.Foundation.NSObject sender) {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToShopView ();
		}
	}
}

