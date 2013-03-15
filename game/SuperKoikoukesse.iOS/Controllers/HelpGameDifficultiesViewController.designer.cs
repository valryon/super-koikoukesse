// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("HelpGameDifficulties")]
	partial class HelpGameDifficultiesViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton backButton { get; set; }

		[Action ("backButtonPressed:")]
		partial void backButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (backButton != null) {
				backButton.Dispose ();
				backButton = null;
			}
		}
	}
}
