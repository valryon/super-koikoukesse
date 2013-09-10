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
	[Register ("ScoreViewController")]
	partial class ScoreViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel LabelDifficulty { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelMode { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewHistory { get; set; }

		[Action ("OnMenuTouched:")]
		partial void OnMenuTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnRetryTouched:")]
		partial void OnRetryTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (LabelMode != null) {
				LabelMode.Dispose ();
				LabelMode = null;
			}

			if (LabelDifficulty != null) {
				LabelDifficulty.Dispose ();
				LabelDifficulty = null;
			}

			if (ViewHistory != null) {
				ViewHistory.Dispose ();
				ViewHistory = null;
			}
		}
	}
}
