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
		MonoTouch.UIKit.UILabel modeLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel difficultyLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel scoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton retryButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton menuButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel bestScoreLabel { get; set; }

		[Action ("retryButtonPressed:")]
		partial void retryButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("menuButtonPressed:")]
		partial void menuButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (modeLabel != null) {
				modeLabel.Dispose ();
				modeLabel = null;
			}

			if (difficultyLabel != null) {
				difficultyLabel.Dispose ();
				difficultyLabel = null;
			}

			if (scoreLabel != null) {
				scoreLabel.Dispose ();
				scoreLabel = null;
			}

			if (retryButton != null) {
				retryButton.Dispose ();
				retryButton = null;
			}

			if (menuButton != null) {
				menuButton.Dispose ();
				menuButton = null;
			}

			if (bestScoreLabel != null) {
				bestScoreLabel.Dispose ();
				bestScoreLabel = null;
			}
		}
	}
}
