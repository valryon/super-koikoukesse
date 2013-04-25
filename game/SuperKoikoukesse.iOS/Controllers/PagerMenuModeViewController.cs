
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using MonoTouch.CoreGraphics;

namespace SuperKoikoukesse.iOS
{
	public partial class PagerMenuModeViewController : UIViewController
	{
		private GameModes mode;

		public event Action<GameModes> GameModeSelected;

		public PagerMenuModeViewController (GameModes m) 
			: base ("PagerMenuModeView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
			this.mode = m;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return  UIInterfaceOrientationMask.LandscapeLeft | UIInterfaceOrientationMask.LandscapeRight;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			string modeId = mode.ToString ().ToLower ();
			this.titleLabel.Text = NSBundle.MainBundle.LocalizedString (modeId + ".title", "");
			this.descriptionLabel.Text = NSBundle.MainBundle.LocalizedString (modeId + ".desc", "");
			this.image.Image = UIImage.FromFile (modeId + ".png");

			this.descriptionLabel.SizeToFit();
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			// TODO if the game has just been installed, the default label will be displayed
			// find a workaround in this case
			Player profile = ProfileService.Instance.CachedPlayer;
			if (profile != null)
				livesLeftLabel.Text = "(sur " + profile.Credits.ToString() + ")";
		}

		partial void playButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			if (GameModeSelected != null) {
				GameModeSelected (mode);
			}
		}
	}
}

