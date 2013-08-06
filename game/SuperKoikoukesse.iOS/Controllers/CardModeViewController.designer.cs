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
		MonoTouch.UIKit.UIImageView ImageIcon { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelDescriptionMain { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelDescriptionSub { get; set; }

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

			if (ImageIcon != null) {
				ImageIcon.Dispose ();
				ImageIcon = null;
			}

			if (LabelTitle != null) {
				LabelTitle.Dispose ();
				LabelTitle = null;
			}

			if (ViewCard != null) {
				ViewCard.Dispose ();
				ViewCard = null;
			}

			if (LabelDescriptionMain != null) {
				LabelDescriptionMain.Dispose ();
				LabelDescriptionMain = null;
			}

			if (LabelDescriptionSub != null) {
				LabelDescriptionSub.Dispose ();
				LabelDescriptionSub = null;
			}

			if (ViewShadow != null) {
				ViewShadow.Dispose ();
				ViewShadow = null;
			}
		}
	}
}
