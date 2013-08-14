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
	[Register ("TimerViewController")]
	partial class TimerViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel LabelCurrentTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewCurrentTimer { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LabelCurrentTime != null) {
				LabelCurrentTime.Dispose ();
				LabelCurrentTime = null;
			}

			if (ViewCurrentTimer != null) {
				ViewCurrentTimer.Dispose ();
				ViewCurrentTimer = null;
			}
		}
	}
}
