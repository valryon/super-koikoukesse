// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("GamePauseViewController")]
	partial class GamePauseViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton resumeButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton quitButton { get; set; }

		[Action ("resumeButtonPressed:")]
		partial void resumeButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("quitButtonPressed:")]
		partial void quitButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (resumeButton != null) {
				resumeButton.Dispose ();
				resumeButton = null;
			}

			if (quitButton != null) {
				quitButton.Dispose ();
				quitButton = null;
			}
		}
	}
}
