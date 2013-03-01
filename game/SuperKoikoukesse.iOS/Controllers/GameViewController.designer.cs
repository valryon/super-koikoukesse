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
		
		void ReleaseDesignerOutlets ()
		{
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
		}
	}
}
