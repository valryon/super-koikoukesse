using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;

namespace SuperKoikoukesse.iOS
{
  public partial class VersusNewMatchViewController : UIViewController
  {
    private static string VersusToGameSegueId = "VersusNewToGame";

    #region Fields

    private GameLauncher mGameLauncher;
    private GameDifficulties mLastSelectedDifficulty;

    #endregion

    #region Constructor

    public VersusNewMatchViewController(IntPtr handle) : base (handle)
    {
    }

    public override void ViewDidLoad()
    {
      SliderDifficulty.Value = 0f;
      base.ViewDidLoad();
    }

    #endregion

    #region Events

    partial void OnDifficultyValueChanged (MonoTouch.Foundation.NSObject sender) {
      SliderDifficulty.SetValue ((float)Math.Round (SliderDifficulty.Value), false);

      foreach(GameDifficulties d in Enum.GetValues(typeof(GameDifficulties))) {
        if(((int)d) == (int)SliderDifficulty.Value) {
          mLastSelectedDifficulty = d;
          break;
        }
      }

      LabelDifficulty.Text = Localization.Get(mLastSelectedDifficulty.ToString().ToLower()+".title");
    }

    partial void OnGenreTouched (MonoTouch.Foundation.NSObject sender) {

    }

    partial void OnPlatformTouched (MonoTouch.Foundation.NSObject sender) {

    }

    partial void OnYearMaxTouched (MonoTouch.Foundation.NSObject sender) {

    }

    partial void OnYearMinTouched (MonoTouch.Foundation.NSObject sender) {

    }

    partial void OnGoTouched(MonoTouch.Foundation.NSObject sender)
    {
      PlayerCache.Instance.AuthenticatedPlayer.NewMatch(
				// Match found
        (match) => {

        // Ensure it's a new match
        if (match.IsFirstTurn)
        {
          PlayerCache.Instance.AuthenticatedPlayer.SetMatch(match);
          mGameLauncher = new GameLauncher(this);

//          float diffi

          mGameLauncher.Launch(VersusToGameSegueId, GameMode.SCORE, GameDifficulties.NORMAL, new Filter());
        } 
//					else {
//						var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate; 
//
//						if (match.IsEnded) {
//							// See the final score
//							Dialogs.ShowMatchEnded ();
//						} else {
//							if (match.IsPlayerTurn (PlayerCache.Instance.AuthenticatedPlayer.PlayerId)) {
//								// Player turn
//								LaunchGame (mode, match.Difficulty, match.Filter);
//							} else {
//								// TODO Other player turn: display last score?
//								Dialogs.ShowNotYourTurn ();
//							}
//						}
//					}
      },
			// Cancel
        () => {
        // Nothing, controller is already dismissed
      },
			// Error
        () => {
        // Display an error dialog?
        UIAlertView alert = new UIAlertView(
          "Une erreur est survenue",
          "Nous n'avons pas pu démarrer une nouvelle partie car une erreur est survenue..",
          null,
          "Ok");

        alert.Show();
      },
			// Player quit
        () => {
        // Kill the game? Inform the player?
      }
      );

    }

    #endregion

    #region Methods

    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
    {
      base.PrepareForSegue(segue, sender);

      if (mGameLauncher != null && segue.Identifier == mGameLauncher.SegueId)
      {
        GameViewController gameVc = (GameViewController) segue.DestinationViewController;

        // Prepare quizz
        mGameLauncher.Prepare(gameVc);
      }
    }

    #endregion
  }
}
