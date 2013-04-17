
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace SuperKoikoukesse.iOS
{
	public partial class LoadingViewController : UIViewController
	{
		public LoadingViewController ()
			: base ( "LoadingView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
		}
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return  UIInterfaceOrientationMask.LandscapeLeft | UIInterfaceOrientationMask.LandscapeRight;
		}
	}
}

