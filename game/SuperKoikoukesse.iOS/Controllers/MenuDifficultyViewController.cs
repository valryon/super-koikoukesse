
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public partial class MenuDifficultyViewController : UIViewController
	{
		public event Action<GameDifficulties> DifficultySelected;
		public event Action BackSelected;

		public MenuDifficultyViewController ()
			: base ("MenuDifficultyView"+ (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
		}

		partial void easyButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			selectDifficulty (GameDifficulties.Easy);
		}

		partial void hardButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			selectDifficulty (GameDifficulties.Hard);
		}

		partial void expertButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			selectDifficulty (GameDifficulties.Expert);
		}

		partial void nolifeButtonPressed (MonoTouch.Foundation.NSObject sender)
		{
			selectDifficulty (GameDifficulties.Nolife);
		}

		private void selectDifficulty (GameDifficulties diff)
		{
			hideMyself ();

			if (DifficultySelected != null) {
				DifficultySelected (diff);
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

