
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
		public event Action<GameMode, GameDifficulties> DifficultySelected;
		public event Action BackSelected;

		private GameMode mode;

		public MenuDifficultyViewController ()
			: base ("MenuDifficultyView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

		public void SetMode (GameMode m)
		{
			mode = m;
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
			selectDifficulty (GameDifficulties.INSANE);
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
	}
}

