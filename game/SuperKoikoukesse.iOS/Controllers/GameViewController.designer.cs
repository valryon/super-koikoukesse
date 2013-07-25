// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("GameViewController")]
	partial class GameViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIImageView bgImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonPause { get; set; }

		[Outlet]
		MonoTouch.UIKit.NSLayoutConstraint ConstraintCombo { get; set; }

		[Outlet]
		MonoTouch.UIKit.NSLayoutConstraint ConstraintLeadingTimerLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton game1Button { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton game2Button { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton game3Button { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton game4Button { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView gameImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView gameImageScroll { get; set; }

		[Outlet]
		MonoTouch.UIKit.NSLayoutConstraint jokerBottomConstraints { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton jokerButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelCombo { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelCurrentTime { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView livesImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel modeLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel questionCountLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel scoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel scoreTitleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.NSLayoutConstraint timeFullHeightConstraint { get; set; }

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

		[Outlet]
		MonoTouch.UIKit.UIView ViewTimer { get; set; }

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
			if (bgImage != null) {
				bgImage.Dispose ();
				bgImage = null;
			}

			if (ButtonPause != null) {
				ButtonPause.Dispose ();
				ButtonPause = null;
			}

			if (ConstraintLeadingTimerLabel != null) {
				ConstraintLeadingTimerLabel.Dispose ();
				ConstraintLeadingTimerLabel = null;
			}

			if (game1Button != null) {
				game1Button.Dispose ();
				game1Button = null;
			}

			if (game2Button != null) {
				game2Button.Dispose ();
				game2Button = null;
			}

			if (game3Button != null) {
				game3Button.Dispose ();
				game3Button = null;
			}

			if (game4Button != null) {
				game4Button.Dispose ();
				game4Button = null;
			}

			if (gameImage != null) {
				gameImage.Dispose ();
				gameImage = null;
			}

			if (gameImageScroll != null) {
				gameImageScroll.Dispose ();
				gameImageScroll = null;
			}

			if (jokerBottomConstraints != null) {
				jokerBottomConstraints.Dispose ();
				jokerBottomConstraints = null;
			}

			if (jokerButton != null) {
				jokerButton.Dispose ();
				jokerButton = null;
			}

			if (LabelCurrentTime != null) {
				LabelCurrentTime.Dispose ();
				LabelCurrentTime = null;
			}

			if (livesImage != null) {
				livesImage.Dispose ();
				livesImage = null;
			}

			if (modeLabel != null) {
				modeLabel.Dispose ();
				modeLabel = null;
			}

			if (questionCountLabel != null) {
				questionCountLabel.Dispose ();
				questionCountLabel = null;
			}

			if (scoreLabel != null) {
				scoreLabel.Dispose ();
				scoreLabel = null;
			}

			if (scoreTitleLabel != null) {
				scoreTitleLabel.Dispose ();
				scoreTitleLabel = null;
			}

			if (timeFullHeightConstraint != null) {
				timeFullHeightConstraint.Dispose ();
				timeFullHeightConstraint = null;
			}

			if (ViewAnswers != null) {
				ViewAnswers.Dispose ();
				ViewAnswers = null;
			}

			if (ViewEmitter != null) {
				ViewEmitter.Dispose ();
				ViewEmitter = null;
			}

			if (ViewImageShadow != null) {
				ViewImageShadow.Dispose ();
				ViewImageShadow = null;
			}

			if (ViewInformations != null) {
				ViewInformations.Dispose ();
				ViewInformations = null;
			}

			if (ViewCombo != null) {
				ViewCombo.Dispose ();
				ViewCombo = null;
			}

			if (ViewTimer != null) {
				ViewTimer.Dispose ();
				ViewTimer = null;
			}

			if (LabelCombo != null) {
				LabelCombo.Dispose ();
				LabelCombo = null;
			}

			if (ConstraintCombo != null) {
				ConstraintCombo.Dispose ();
				ConstraintCombo = null;
			}
		}
	}
}
