// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	[Register ("MenuViewController")]
	partial class MenuViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel authorsLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton creditsButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView coinsImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView creditsImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel coinsLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel creditsLabel { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton configButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton paramsButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton shopButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIPageControl pageControl { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIScrollView scrollView { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIImageView bgImage { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton debugButton { get; set; }

		[Action ("configButtonPressed:")]
		partial void configButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("debugButtonPressed:")]
		partial void debugButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("paramsButtonPressed:")]
		partial void paramsButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("shopButtonPressed:")]
		partial void shopButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("creditsButtonPressed:")]
		partial void creditsButtonPressed (MonoTouch.Foundation.NSObject sender);

		[Action ("coinsButtonPressed:")]
		partial void coinsButtonPressed (MonoTouch.Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (authorsLabel != null) {
				authorsLabel.Dispose ();
				authorsLabel = null;
			}

			if (creditsButton != null) {
				creditsButton.Dispose ();
				creditsButton = null;
			}

			if (coinsImage != null) {
				coinsImage.Dispose ();
				coinsImage = null;
			}

			if (creditsImage != null) {
				creditsImage.Dispose ();
				creditsImage = null;
			}

			if (coinsLabel != null) {
				coinsLabel.Dispose ();
				coinsLabel = null;
			}

			if (creditsLabel != null) {
				creditsLabel.Dispose ();
				creditsLabel = null;
			}

			if (configButton != null) {
				configButton.Dispose ();
				configButton = null;
			}

			if (paramsButton != null) {
				paramsButton.Dispose ();
				paramsButton = null;
			}

			if (shopButton != null) {
				shopButton.Dispose ();
				shopButton = null;
			}

			if (pageControl != null) {
				pageControl.Dispose ();
				pageControl = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (bgImage != null) {
				bgImage.Dispose ();
				bgImage = null;
			}

			if (debugButton != null) {
				debugButton.Dispose ();
				debugButton = null;
			}
		}
	}
}
