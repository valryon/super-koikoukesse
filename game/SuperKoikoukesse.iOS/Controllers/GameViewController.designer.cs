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
	[Register ("GameViewController")]
	partial class GameViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton ButtonGame1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonGame2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonGame3 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonGame4 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonJoker { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonPause { get; set; }

		[Outlet]
		MonoTouch.UIKit.NSLayoutConstraint ConstraintCombo { get; set; }

		[Outlet]
		MonoTouch.UIKit.NSLayoutConstraint ConstraintJoker { get; set; }

		[Outlet]
		MonoTouch.UIKit.NSLayoutConstraint ConstraintTimer { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView ImageGame { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelCombo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelCount { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelMode { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelScore { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView ScrollViewImageGame { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewAnswers { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewCombo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewEmitter { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewImageShadow { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewInformations { get; set; }

		[Action ("OnButton1Touched:")]
		partial void OnButton1Touched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnButton2Touched:")]
		partial void OnButton2Touched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnButton3Touched:")]
		partial void OnButton3Touched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnButton4Touched:")]
		partial void OnButton4Touched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnJokerTouched:")]
		partial void OnJokerTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnPauseTouched:")]
		partial void OnPauseTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ViewImageShadow != null) {
				ViewImageShadow.Dispose ();
				ViewImageShadow = null;
			}

			if (ViewInformations != null) {
				ViewInformations.Dispose ();
				ViewInformations = null;
			}

			if (ViewEmitter != null) {
				ViewEmitter.Dispose ();
				ViewEmitter = null;
			}

			if (LabelMode != null) {
				LabelMode.Dispose ();
				LabelMode = null;
			}

			if (LabelCount != null) {
				LabelCount.Dispose ();
				LabelCount = null;
			}

			if (ButtonGame1 != null) {
				ButtonGame1.Dispose ();
				ButtonGame1 = null;
			}

			if (ButtonGame2 != null) {
				ButtonGame2.Dispose ();
				ButtonGame2 = null;
			}

			if (ButtonGame3 != null) {
				ButtonGame3.Dispose ();
				ButtonGame3 = null;
			}

			if (ButtonGame4 != null) {
				ButtonGame4.Dispose ();
				ButtonGame4 = null;
			}

			if (ButtonJoker != null) {
				ButtonJoker.Dispose ();
				ButtonJoker = null;
			}

			if (ButtonPause != null) {
				ButtonPause.Dispose ();
				ButtonPause = null;
			}

			if (ImageGame != null) {
				ImageGame.Dispose ();
				ImageGame = null;
			}

			if (LabelCombo != null) {
				LabelCombo.Dispose ();
				LabelCombo = null;
			}

			if (LabelScore != null) {
				LabelScore.Dispose ();
				LabelScore = null;
			}

			if (ViewCombo != null) {
				ViewCombo.Dispose ();
				ViewCombo = null;
			}

			if (LabelTime != null) {
				LabelTime.Dispose ();
				LabelTime = null;
			}

			if (ViewAnswers != null) {
				ViewAnswers.Dispose ();
				ViewAnswers = null;
			}

			if (ScrollViewImageGame != null) {
				ScrollViewImageGame.Dispose ();
				ScrollViewImageGame = null;
			}

			if (ConstraintTimer != null) {
				ConstraintTimer.Dispose ();
				ConstraintTimer = null;
			}

			if (ConstraintJoker != null) {
				ConstraintJoker.Dispose ();
				ConstraintJoker = null;
			}

			if (ConstraintCombo != null) {
				ConstraintCombo.Dispose ();
				ConstraintCombo = null;
			}
		}
	}
}
