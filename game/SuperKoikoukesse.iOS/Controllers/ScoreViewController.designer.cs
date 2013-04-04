// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("ScoreViewController")]
	partial class ScoreViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIView bodyView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel coinsLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton retryButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton menuButton { get; set; }

		[Action ("retryButtonPressed:")]
		partial void retryButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("menuButtonPressed:")]
		partial void menuButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bodyView != null) {
				bodyView.Dispose ();
				bodyView = null;
			}

			if (coinsLabel != null) {
				coinsLabel.Dispose ();
				coinsLabel = null;
			}

			if (retryButton != null) {
				retryButton.Dispose ();
				retryButton = null;
			}

			if (menuButton != null) {
				menuButton.Dispose ();
				menuButton = null;
			}
		}
	}
}
