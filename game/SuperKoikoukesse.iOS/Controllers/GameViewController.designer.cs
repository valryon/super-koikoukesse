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
		MonoTouch.UIKit.NSLayoutConstraint timeFullHeightConstraint { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView timeEmpty { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView timeFull { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView timeTop { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel livesCountLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel livesLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel questionCountLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel modeLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView gameImageScroll { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel comboLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel scoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel scoreTitleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel timeTitleLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel timeLeftLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView bgImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView gameImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton jokerButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton game1Button { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton game2Button { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton game3Button { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton game4Button { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton pauseButton { get; set; }

		[Action ("game1ButtonPressed:")]
		partial void game1ButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("game2ButtonPressed:")]
		partial void game2ButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("game3ButtonPressed:")]
		partial void game3ButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("game4ButtonPressed:")]
		partial void game4ButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("pauseButtonPressed:")]
		partial void pauseButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("jokerButtonPressed:")]
		partial void jokerButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (timeFullHeightConstraint != null) {
				timeFullHeightConstraint.Dispose ();
				timeFullHeightConstraint = null;
			}

			if (timeEmpty != null) {
				timeEmpty.Dispose ();
				timeEmpty = null;
			}

			if (timeFull != null) {
				timeFull.Dispose ();
				timeFull = null;
			}

			if (timeTop != null) {
				timeTop.Dispose ();
				timeTop = null;
			}

			if (livesCountLabel != null) {
				livesCountLabel.Dispose ();
				livesCountLabel = null;
			}

			if (livesLabel != null) {
				livesLabel.Dispose ();
				livesLabel = null;
			}

			if (questionCountLabel != null) {
				questionCountLabel.Dispose ();
				questionCountLabel = null;
			}

			if (modeLabel != null) {
				modeLabel.Dispose ();
				modeLabel = null;
			}

			if (gameImageScroll != null) {
				gameImageScroll.Dispose ();
				gameImageScroll = null;
			}

			if (comboLabel != null) {
				comboLabel.Dispose ();
				comboLabel = null;
			}

			if (scoreLabel != null) {
				scoreLabel.Dispose ();
				scoreLabel = null;
			}

			if (scoreTitleLabel != null) {
				scoreTitleLabel.Dispose ();
				scoreTitleLabel = null;
			}

			if (timeTitleLabel != null) {
				timeTitleLabel.Dispose ();
				timeTitleLabel = null;
			}

			if (timeLeftLabel != null) {
				timeLeftLabel.Dispose ();
				timeLeftLabel = null;
			}

			if (bgImage != null) {
				bgImage.Dispose ();
				bgImage = null;
			}

			if (gameImage != null) {
				gameImage.Dispose ();
				gameImage = null;
			}

			if (jokerButton != null) {
				jokerButton.Dispose ();
				jokerButton = null;
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

			if (pauseButton != null) {
				pauseButton.Dispose ();
				pauseButton = null;
			}
		}
	}
}
