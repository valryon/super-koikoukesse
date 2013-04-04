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
	partial class PagerMenuHighscoresViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIView highscoreView { get; set; }

		[Action ("modeChanged:")]
		partial void modeChanged (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (highscoreView != null) {
				highscoreView.Dispose ();
				highscoreView = null;
			}
		}
	}
}
