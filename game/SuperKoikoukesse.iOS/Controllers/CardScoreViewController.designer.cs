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
  [Register ("CardScoreViewController")]
	partial class CardScoreViewController
	{
		[Outlet]
		MonoTouch.UIKit.UISegmentedControl SelectorDifficulty { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISegmentedControl SelectorMode { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIView ViewScore { get; set; }

		[Action ("OnDifficultyChanged:")]
		partial void OnDifficultyChanged (MonoTouch.Foundation.NSObject sender);

		[Action ("OnModeChanged:")]
		partial void OnModeChanged (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ViewScore != null) {
				ViewScore.Dispose ();
				ViewScore = null;
			}

			if (SelectorMode != null) {
				SelectorMode.Dispose ();
				SelectorMode = null;
			}

			if (SelectorDifficulty != null) {
				SelectorDifficulty.Dispose ();
				SelectorDifficulty = null;
			}
		}
	}
}
