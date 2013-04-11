// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("MenuDifficultyViewController")]
	partial class MenuDifficultyViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel stunfestModeLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton stunfestModeButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton easyButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton hardButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton expertButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton nolifeButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton helpButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton backButton { get; set; }

		[Action ("easyButtonPressed:")]
		partial void easyButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("hardButtonPressed:")]
		partial void hardButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("expertButtonPressed:")]
		partial void expertButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("nolifeButtonPressed:")]
		partial void nolifeButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("backButtonPressed:")]
		partial void backButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("stunfestModeButtonClick:")]
		partial void stunfestModeButtonClick (MonoTouch.Foundation.NSObject sender);

		[Action ("helpButtonPressed:")]
		partial void helpButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (stunfestModeLabel != null) {
				stunfestModeLabel.Dispose ();
				stunfestModeLabel = null;
			}

			if (stunfestModeButton != null) {
				stunfestModeButton.Dispose ();
				stunfestModeButton = null;
			}

			if (easyButton != null) {
				easyButton.Dispose ();
				easyButton = null;
			}

			if (hardButton != null) {
				hardButton.Dispose ();
				hardButton = null;
			}

			if (expertButton != null) {
				expertButton.Dispose ();
				expertButton = null;
			}

			if (nolifeButton != null) {
				nolifeButton.Dispose ();
				nolifeButton = null;
			}

			if (helpButton != null) {
				helpButton.Dispose ();
				helpButton = null;
			}

			if (backButton != null) {
				backButton.Dispose ();
				backButton = null;
			}
		}
	}
}
