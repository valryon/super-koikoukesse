// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("PagerMenuModeViewController")]
	partial class PagerMenuModeViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel livesLeftLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton playButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel titleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel descriptionLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView image { get; set; }

		[Action ("playButtonPressed:")]
		partial void playButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (livesLeftLabel != null) {
				livesLeftLabel.Dispose ();
				livesLeftLabel = null;
			}

			if (playButton != null) {
				playButton.Dispose ();
				playButton = null;
			}

			if (titleLabel != null) {
				titleLabel.Dispose ();
				titleLabel = null;
			}

			if (descriptionLabel != null) {
				descriptionLabel.Dispose ();
				descriptionLabel = null;
			}

			if (image != null) {
				image.Dispose ();
				image = null;
			}
		}
	}
}
