// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("MenuViewController")]
	partial class MenuViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView bgImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton scoreAttackButton { get; set; }

		[Action ("scoreAttackButtonPressed:")]
		partial void scoreAttackButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (bgImage != null) {
				bgImage.Dispose ();
				bgImage = null;
			}

			if (scoreAttackButton != null) {
				scoreAttackButton.Dispose ();
				scoreAttackButton = null;
			}
		}
	}
}
