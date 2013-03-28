
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
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}
}

