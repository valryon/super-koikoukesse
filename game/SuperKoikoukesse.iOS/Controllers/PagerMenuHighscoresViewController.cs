
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

			if (highScoresController == null) {
				highScoresController = new HighScoresControlViewController ();
				highScoresController.SetScoreParameters (GameModes.ScoreAttack, GameDifficulties.Normal);

				this.highscoreView.AddSubview (highScoresController.View);
			}
		}

		public void ForceUpdate ()
		{
			highScoresController.UpdateGameCenterLeaderboard ();
		}

		/// <summary>
		/// Get mode from the selector
		/// </summary>
		/// <returns>The mode.</returns>
		private GameModes getMode ()
		{

			if (modeSelector.SelectedSegment >= 0) {
				string mode = modeSelector.TitleAt (modeSelector.SelectedSegment);

				if (mode.ToLower ().Contains ("score"))
					return GameModes.ScoreAttack;
				if (mode.ToLower ().Contains ("time"))
					return GameModes.TimeAttack;
				if (mode.ToLower ().Contains ("survival"))
					return GameModes.Survival;
				if (mode.ToLower ().Contains ("versus"))
					return GameModes.Versus;
			}

			return GameModes.ScoreAttack;
		}

		/// <summary>
		/// Get diff from the selector
		/// </summary>
		/// <returns>The difficulty.</returns>
		private GameDifficulties getDifficulty ()
		{
			if (diffSelector.SelectedSegment >= 0) {
				string diff = diffSelector.TitleAt (diffSelector.SelectedSegment);
				
				if (diff.ToLower ().Contains ("normal"))
					return GameDifficulties.Normal;
				if (diff.ToLower ().Contains ("hard"))
					return GameDifficulties.Hard;
				if (diff.ToLower ().Contains ("expert"))
					return GameDifficulties.Expert;
				if (diff.ToLower ().Contains ("nolife"))
					return GameDifficulties.Nolife;
			}

			return GameDifficulties.Normal;
		}

		partial void modeChanged (MonoTouch.Foundation.NSObject sender)
		{
			highScoresController.SetScoreParameters (getMode (), getDifficulty ());
		}

		partial void difficultyChanged (MonoTouch.Foundation.NSObject sender)
		{
			highScoresController.SetScoreParameters (getMode (), getDifficulty ());
		}
	}
}

