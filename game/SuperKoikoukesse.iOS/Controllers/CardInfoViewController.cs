
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
	public partial class CardInfoViewController : UIViewController
	{
		public CardInfoViewController () 
			: base ("CardInfo"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
      return AppDelegate.HasSupportedInterfaceOrientations();
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

