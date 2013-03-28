
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public partial class PagerMenuModeViewController : UIViewController
	{
		private GameModes mode;

		public PagerMenuModeViewController (GameModes m) 
			: base ("PagerMenuModeView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
			this.mode = m;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			string modeId = mode.ToString ().ToLower ();
			this.titleLabel.Text = NSBundle.MainBundle.LocalizedString (modeId + ".title", "");
			this.descriptionLabel.Text = NSBundle.MainBundle.LocalizedString (modeId + ".desc", "");
			this.image.Image = UIImage.FromFile (modeId + ".png");
		}

		partial void playButtonPressed (MonoTouch.Foundation.NSObject sender) {
			var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
			appDelegate.SwitchToDifficultiesView (mode);
		}
	}
}

