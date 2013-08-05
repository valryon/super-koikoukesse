// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace SuperKoikoukesse.iOS
{
	[Register ("PagerMenuModeViewController")]
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

		[Action ("OnPlayTouched:")]
		partial void OnPlayTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (LabelLifes != null) {
				LabelLifes.Dispose ();
				LabelLifes = null;
			}

			if (ButtonPlay != null) {
				ButtonPlay.Dispose ();
				ButtonPlay = null;
			}

			if (LabelDescription != null) {
				LabelDescription.Dispose ();
				LabelDescription = null;
			}

			if (LabelTitle != null) {
				LabelTitle.Dispose ();
				LabelTitle = null;
			}
		}
	}
}
