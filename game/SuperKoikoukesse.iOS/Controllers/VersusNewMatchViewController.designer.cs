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
	[Register ("VersusNewMatchViewController")]
	partial class VersusNewMatchViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton ButtonGenre { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonPlatform { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonYearMax { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton ButtonYearMin { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel LabelDifficulty { get; set; }

		[Outlet]
		MonoTouch.UIKit.UISlider SliderDifficulty { get; set; }

		[Action ("OnDifficultyValueChanged:")]
		partial void OnDifficultyValueChanged (MonoTouch.Foundation.NSObject sender);

		[Action ("OnGenreTouched:")]
		partial void OnGenreTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnGoTouched:")]
		partial void OnGoTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnPlatformTouched:")]
		partial void OnPlatformTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnYearMaxTouched:")]
		partial void OnYearMaxTouched (MonoTouch.Foundation.NSObject sender);

		[Action ("OnYearMinTouched:")]
		partial void OnYearMinTouched (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (LabelDifficulty != null) {
				LabelDifficulty.Dispose ();
				LabelDifficulty = null;
			}

			if (ButtonGenre != null) {
				ButtonGenre.Dispose ();
				ButtonGenre = null;
			}

			if (ButtonPlatform != null) {
				ButtonPlatform.Dispose ();
				ButtonPlatform = null;
			}

			if (ButtonYearMax != null) {
				ButtonYearMax.Dispose ();
				ButtonYearMax = null;
			}

			if (ButtonYearMin != null) {
				ButtonYearMin.Dispose ();
				ButtonYearMin = null;
			}

			if (SliderDifficulty != null) {
				SliderDifficulty.Dispose ();
				SliderDifficulty = null;
			}
		}
	}
}
