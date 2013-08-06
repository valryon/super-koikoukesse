// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("CardModeViewController")]
	partial class CardModeViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton ButtonPlay { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelDescription { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelLifes { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelTitle { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewCard { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewShadow { get; set; }

		[Action ("OnPlayTouched:")]
		partial void OnPlayTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonPlay != null) {
				ButtonPlay.Dispose ();
				ButtonPlay = null;
			}

			if (LabelDescription != null) {
				LabelDescription.Dispose ();
				LabelDescription = null;
			}

			if (LabelLifes != null) {
				LabelLifes.Dispose ();
				LabelLifes = null;
			}

			if (LabelTitle != null) {
				LabelTitle.Dispose ();
				LabelTitle = null;
			}

			if (ViewShadow != null) {
				ViewShadow.Dispose ();
				ViewShadow = null;
			}

			if (ViewCard != null) {
				ViewCard.Dispose ();
				ViewCard = null;
			}
		}
	}
}
