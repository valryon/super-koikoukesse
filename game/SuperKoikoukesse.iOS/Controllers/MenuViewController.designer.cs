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
	[Register ("MenuViewController")]
	partial class MenuViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton ButtonDebug { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelCoins { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelCredits { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIPageControl PageControl { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView ScrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewLoading { get; set; }

		[Action ("OnCoinsTouched:")]
		partial void OnCoinsTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnConfigTouched:")]
		partial void OnConfigTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnCreditsTouched:")]
		partial void OnCreditsTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnDebugTouched:")]
		partial void OnDebugTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnSettingsTouched:")]
		partial void OnSettingsTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnShopTouched:")]
		partial void OnShopTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonDebug != null) {
				ButtonDebug.Dispose ();
				ButtonDebug = null;
			}

			if (LabelCoins != null) {
				LabelCoins.Dispose ();
				LabelCoins = null;
			}

			if (LabelCredits != null) {
				LabelCredits.Dispose ();
				LabelCredits = null;
			}

			if (PageControl != null) {
				PageControl.Dispose ();
				PageControl = null;
			}

			if (ScrollView != null) {
				ScrollView.Dispose ();
				ScrollView = null;
			}

			if (ViewLoading != null) {
				ViewLoading.Dispose ();
				ViewLoading = null;
			}
		}
	}
}
