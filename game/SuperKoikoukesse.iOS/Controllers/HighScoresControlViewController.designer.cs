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
	[Register ("HighScoresControlViewController")]
	partial class HighScoresControlViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIView gameCenterPanel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton leaderboardButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel localHighscoresLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel onlineRankLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel onlineRankValueLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank1Label { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank1ScoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank2Label { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank2ScoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank3Label { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank3ScoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank4Label { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank4ScoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank5Label { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rank5ScoreLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rankLastLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel rankLastScoreLabel { get; set; }

		[Action ("OnLeaderboardsTouched:")]
		partial void OnLeaderboardsTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (leaderboardButton != null) {
				leaderboardButton.Dispose ();
				leaderboardButton = null;
			}

			if (gameCenterPanel != null) {
				gameCenterPanel.Dispose ();
				gameCenterPanel = null;
			}

			if (onlineRankValueLabel != null) {
				onlineRankValueLabel.Dispose ();
				onlineRankValueLabel = null;
			}

			if (onlineRankLabel != null) {
				onlineRankLabel.Dispose ();
				onlineRankLabel = null;
			}

			if (localHighscoresLabel != null) {
				localHighscoresLabel.Dispose ();
				localHighscoresLabel = null;
			}

			if (rank1ScoreLabel != null) {
				rank1ScoreLabel.Dispose ();
				rank1ScoreLabel = null;
			}

			if (rank1Label != null) {
				rank1Label.Dispose ();
				rank1Label = null;
			}

			if (rank2Label != null) {
				rank2Label.Dispose ();
				rank2Label = null;
			}

			if (rank2ScoreLabel != null) {
				rank2ScoreLabel.Dispose ();
				rank2ScoreLabel = null;
			}

			if (rank3Label != null) {
				rank3Label.Dispose ();
				rank3Label = null;
			}

			if (rank3ScoreLabel != null) {
				rank3ScoreLabel.Dispose ();
				rank3ScoreLabel = null;
			}

			if (rank4Label != null) {
				rank4Label.Dispose ();
				rank4Label = null;
			}

			if (rank4ScoreLabel != null) {
				rank4ScoreLabel.Dispose ();
				rank4ScoreLabel = null;
			}

			if (rank5Label != null) {
				rank5Label.Dispose ();
				rank5Label = null;
			}

			if (rank5ScoreLabel != null) {
				rank5ScoreLabel.Dispose ();
				rank5ScoreLabel = null;
			}

			if (rankLastLabel != null) {
				rankLastLabel.Dispose ();
				rankLastLabel = null;
			}

			if (rankLastScoreLabel != null) {
				rankLastScoreLabel.Dispose ();
				rankLastScoreLabel = null;
			}
		}
	}
}
