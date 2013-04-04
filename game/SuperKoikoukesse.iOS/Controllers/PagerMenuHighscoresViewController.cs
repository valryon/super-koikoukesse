
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
	public partial class PagerMenuHighscoresViewController : UIViewController
	{
		private HighScoresControlViewController highScoresController;

		public PagerMenuHighscoresViewController ()
			: base ("PagerMenuHighscoresView" + (AppDelegate.UserInterfaceIdiomIsPhone ? "_iPhone" : "_iPad"), null)
		{
		}
		
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if(highScoresController == null) {
				highScoresController = new HighScoresControlViewController();
				highScoresController.SetScoreParameters(GameModes.ScoreAttack, GameDifficulties.Normal);

				this.highscoreView.AddSubview(highScoresController.View);
			}
		}

		public void ForceUpdate() {
			highScoresController.UpdateGameCenterLeaderboard();
		}

		partial void modeChanged (MonoTouch.Foundation.NSObject sender) {
			highScoresController.SetScoreParameters(GameModes.TimeAttack, GameDifficulties.Hard);
		}
	}
}

