
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using MonoTouch.CoreGraphics;

namespace SuperKoikoukesse.iOS
{
	public partial class MenuDifficultyViewController : UIViewController
	{
		public event Action<GameModes, GameDifficulties> DifficultySelected;
		public event Action BackSelected;

		private GameModes mode;

		public MenuDifficultyViewController ()
			: base ("MenuDifficultyView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
      return AppDelegate.HasSupportedInterfaceOrientations();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			setStunfestMode(true);
			showStunfestMode();
		}

		public void SetMode (GameModes m)
		{
			mode = m;

			if(IsViewLoaded) {
				showStunfestMode();
			}
		}

		private void showStunfestMode() {
			if(mode == GameModes.SCORE_ATTACK) {
				stunfestModeLabel.Hidden = false;
				stunfestModeButton.Hidden = false;
				StunfestMode = true;
			}
			else {
				stunfestModeLabel.Hidden = true;
				stunfestModeButton.Hidden = true;
				StunfestMode = false;
			}
		}

		partial void easyButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			selectDifficulty (GameDifficulties.NORMAL);
		}

		partial void hardButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			selectDifficulty (GameDifficulties.HARD);
		}

		partial void expertButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			selectDifficulty (GameDifficulties.EXPERT);
		}

		partial void nolifeButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			selectDifficulty (GameDifficulties.NOLIFE);
		}

		private void selectDifficulty (GameDifficulties diff)
		{
			hideMyself ();

			if (DifficultySelected != null) {
				DifficultySelected (mode, diff);
			}
		}

		partial void backButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			hideMyself ();

			if (BackSelected != null) {
				BackSelected ();
			}
		}

		private void hideMyself ()
		{
			View.RemoveFromSuperview();
		}

		partial void stunfestModeButtonClick (MonoTouch.Foundation.NSObject sender)
		{
			setStunfestMode(!StunfestMode);
		}

		private void setStunfestMode(bool mode) {

			StunfestMode = mode;

			stunfestModeLabel.Text = (mode? "ON" : "OFF");
		}

		public bool StunfestMode
		{
			get;
			set;
		}
	}
}

