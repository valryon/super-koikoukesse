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
	[Register ("CardChallengeViewController")]
	partial class CardChallengeViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton ButtonHide { get; set; }

		[Outlet]
		SuperKoikoukesse.iOS.DifficultyView ViewEasy { get; set; }

		[Outlet]
		SuperKoikoukesse.iOS.DifficultyView ViewHard { get; set; }

		[Outlet]
		SuperKoikoukesse.iOS.DifficultyView ViewInsane { get; set; }

		[Outlet]
		SuperKoikoukesse.iOS.DifficultyView ViewNormal { get; set; }

		[Action ("OnHideTouched:")]
		partial void OnHideTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ButtonHide != null) {
				ButtonHide.Dispose ();
				ButtonHide = null;
			}

			if (ViewEasy != null) {
				ViewEasy.Dispose ();
				ViewEasy = null;
			}

			if (ViewNormal != null) {
				ViewNormal.Dispose ();
				ViewNormal = null;
			}

			if (ViewHard != null) {
				ViewHard.Dispose ();
				ViewHard = null;
			}

			if (ViewInsane != null) {
				ViewInsane.Dispose ();
				ViewInsane = null;
			}
		}
	}
}
