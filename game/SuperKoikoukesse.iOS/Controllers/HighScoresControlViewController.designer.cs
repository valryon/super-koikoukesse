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
		MonoTouch.UIKit.UILabel LabelOnlineRank { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelOnlineScore { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank1 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank1Score { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank2 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank2Score { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank3 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank3Score { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank4 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank4Score { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank5 { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRank5Score { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRankCurrent { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelRankCurrentScore { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewGameCenter { get; set; }

		[Action ("OnLeaderboardsTouched:")]
		partial void OnLeaderboardsTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ViewGameCenter != null) {
				ViewGameCenter.Dispose ();
				ViewGameCenter = null;
			}

			if (LabelOnlineRank != null) {
				LabelOnlineRank.Dispose ();
				LabelOnlineRank = null;
			}

			if (LabelOnlineScore != null) {
				LabelOnlineScore.Dispose ();
				LabelOnlineScore = null;
			}

			if (LabelRank1 != null) {
				LabelRank1.Dispose ();
				LabelRank1 = null;
			}

			if (LabelRank2 != null) {
				LabelRank2.Dispose ();
				LabelRank2 = null;
			}

			if (LabelRank3 != null) {
				LabelRank3.Dispose ();
				LabelRank3 = null;
			}

			if (LabelRank4 != null) {
				LabelRank4.Dispose ();
				LabelRank4 = null;
			}

			if (LabelRank5 != null) {
				LabelRank5.Dispose ();
				LabelRank5 = null;
			}

			if (LabelRank1Score != null) {
				LabelRank1Score.Dispose ();
				LabelRank1Score = null;
			}

			if (LabelRank2Score != null) {
				LabelRank2Score.Dispose ();
				LabelRank2Score = null;
			}

			if (LabelRank3Score != null) {
				LabelRank3Score.Dispose ();
				LabelRank3Score = null;
			}

			if (LabelRank4Score != null) {
				LabelRank4Score.Dispose ();
				LabelRank4Score = null;
			}

			if (LabelRank5Score != null) {
				LabelRank5Score.Dispose ();
				LabelRank5Score = null;
			}

			if (LabelRankCurrent != null) {
				LabelRankCurrent.Dispose ();
				LabelRankCurrent = null;
			}

			if (LabelRankCurrentScore != null) {
				LabelRankCurrentScore.Dispose ();
				LabelRankCurrentScore = null;
			}
		}
	}
}
