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
	[Register ("VersusMatchsViewController")]
	partial class VersusMatchsViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton ButtonNewMatch { get; set; }

		[Action ("OnNewMatchTouched:")]
		partial void OnNewMatchTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonNewMatch != null) {
				ButtonNewMatch.Dispose ();
				ButtonNewMatch = null;
			}
		}
	}
}
