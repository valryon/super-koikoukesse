
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public partial class SplashscreenViewController : UIViewController
	{
		public SplashscreenViewController () 
			: base ("SplashscreenView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
			
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return  UIInterfaceOrientationMask.LandscapeLeft | UIInterfaceOrientationMask.LandscapeRight;
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

			View.UserInteractionEnabled = true;
			View.Alpha = 0f;

			// Fade in & out animation
			UIView.Animate (
				Constants.SplashScreenOpenFadeDuration,
				0,
				UIViewAnimationOptions.CurveEaseIn,
				() => {
					View.Alpha = 1f;
				}, () => {
					Thread.Sleep(1000);
					UIView.Animate (
						Constants.SplashScreenCloseFadeDuration,
							() => {
						View.Alpha = 0f;

					}, () => {
						
						BeginInvokeOnMainThread(() => {
							gotoMenu();
						});
					}
				);
			}
			);
		}

		private void gotoMenu ()
		{
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToMenuView ();

		}
	}
}

