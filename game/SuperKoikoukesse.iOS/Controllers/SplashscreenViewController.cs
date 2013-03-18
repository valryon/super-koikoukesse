
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
		public SplashscreenViewController () : base ("SplashscreenView", null)
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

			View.Alpha = 0f;

			// Fade in & out animation
			UIView.Animate (
				Constants.SplashScreenFadeDuration,
				() => {
					View.Alpha = 1f;
				}, () => {
					UIView.Animate (
						Constants.SplashScreenFadeDuration,
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
			appDelegate.FirstInitialization();

		}
	}
}

