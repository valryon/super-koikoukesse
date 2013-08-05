// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("PagerMenuInfosViewController")]
	partial class CardInfo
	{
		[Outlet]
		MonoTouch.UIKit.UIButton creditsButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton shopButton { get; set; }

		[Action ("creditsButtonPressed:")]
		partial void creditsButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("shopButtonPressed:")]
		partial void shopButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (creditsButton != null) {
				creditsButton.Dispose ();
				creditsButton = null;
			}

			if (shopButton != null) {
				shopButton.Dispose ();
				shopButton = null;
			}
		}
	}
}
