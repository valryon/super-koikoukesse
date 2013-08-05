// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("PagerMenuHighscoresView")]
	partial class CardScore
	{
		[Outlet]
		MonoTouch.UIKit.UIView highscoreView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISegmentedControl modeSelector { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISegmentedControl diffSelector { get; set; }

		[Action ("modeChanged:")]
		partial void modeChanged (MonoTouch.Foundation.NSObject sender);

		[Action ("difficultyChanged:")]
		partial void difficultyChanged (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (highscoreView != null) {
				highscoreView.Dispose ();
				highscoreView = null;
			}

			if (modeSelector != null) {
				modeSelector.Dispose ();
				modeSelector = null;
			}

			if (diffSelector != null) {
				diffSelector.Dispose ();
				diffSelector = null;
			}
		}
	}
}
