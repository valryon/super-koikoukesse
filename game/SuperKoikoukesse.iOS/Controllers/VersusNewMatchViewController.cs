using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using MonoTouch.Dialog;
using System.Collections.Generic;

namespace SuperKoikoukesse.iOS
{
  public partial class VersusNewMatchViewController : UIViewController
  {
    private static string VersusToGameSegueId = "VersusNewToGame";

    #region Fields

    private GameLauncher mGameLauncher;

    private int mCacheMinYear, mCacheMaxYear;
    private List<string> mCacheGenres, mCachePlatforms;

    private GameDifficulties mLastSelectedDifficulty;
    private int mSelectedMinYear;
    private int mSelectedMaxYear;
    private List<string> mSelectedGenres, mSelectedPlatforms;

    #endregion

    #region Constructor

    public VersusNewMatchViewController(IntPtr handle) : base (handle)
    {
      mCacheMinYear = GameDatabase.Instance.GetMinYear();
      mCacheMaxYear = GameDatabase.Instance.GetMaxYear();
      mCacheGenres = GameDatabase.Instance.GetGenres();
      mCachePlatforms = GameDatabase.Instance.GetPlatforms();

      mSelectedMinYear = mCacheMinYear;
      mSelectedMaxYear = mCacheMaxYear;
      mSelectedPlatforms = new List<string>();
      mSelectedGenres = new List<string>();
    }

    public override void ViewDidLoad()
    {
      mLastSelectedDifficulty = GameDifficulties.NORMAL;
      SliderDifficulty.Value = (float)GameDifficulties.NORMAL;

      ButtonYearMin.SetTitle(mCacheMinYear.ToString(), UIControlState.Normal);
      ButtonYearMax.SetTitle(mCacheMaxYear.ToString(), UIControlState.Normal);

      ButtonGenre.SetTitle("All", UIControlState.Normal);
      ButtonPlatform.SetTitle("All", UIControlState.Normal);

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
      displayGenrePicker();
    }

    partial void OnPlatformTouched (MonoTouch.Foundation.NSObject sender) {
      displayPlatformPicker();
    }
   
    partial void OnYearMinTouched (MonoTouch.Foundation.NSObject sender) {
      displayMinYearPicker();
    }

    partial void OnYearMaxTouched (MonoTouch.Foundation.NSObject sender) {
      displayMaxYearPicker();
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

    private void displayMinYearPicker()
    {
      Section section = new Section();

      List<int> years = new List<int>();
      for (int i=mCacheMinYear; i<mSelectedMaxYear; i++)
      {
        years.Add(i);
        section.Add(new RadioElement(i.ToString()));
      }

      var viewController = new DialogViewController (
        new RootElement ("Select minimum date", new  RadioGroup (0)){ 
        section
      }, true);

      viewController.OnSelection += (NSIndexPath obj) => {
        mSelectedMinYear = years[obj.Item];
        InvokeOnMainThread( () => {
          ButtonYearMin.SetTitle(mSelectedMinYear.ToString(), UIControlState.Normal);
          viewController.DismissViewController(true, null);
        });
      };
      NavigationController.PushViewController (viewController, true);
    }

    private void displayMaxYearPicker()
    {
      Section section = new Section();

      List<int> years = new List<int>();
      for (int i=mSelectedMinYear; i<=mCacheMaxYear; i++)
      {
        years.Add(i);
        section.Add(new RadioElement(i.ToString()));
      }
      var viewController = new DialogViewController (
        new RootElement ("Select maximum date", new  RadioGroup (0)){ 
        section
      }, true);

      viewController.OnSelection += (NSIndexPath obj) => {
        mSelectedMaxYear = years[obj.Item];
        InvokeOnMainThread( () => {
          ButtonYearMax.SetTitle(mSelectedMaxYear.ToString(), UIControlState.Normal);
          viewController.DismissViewController(true, null);
        });
      };
      NavigationController.PushViewController (viewController, true);
    }

    private void displayGenrePicker() {
      Section section = new Section();

      foreach(string g in mCacheGenres)
      {
        section.Add(new CheckboxElement(g));
      }
      var viewController = new DialogViewController (
        new RootElement ("Select genres", new  RadioGroup (0)){ 
        section
      }, true);

      viewController.OnSelection += (NSIndexPath obj) => {

      };
      NavigationController.PushViewController (viewController, true);
    }

    private void displayPlatformPicker() {
      Section section = new Section();

      foreach(string g in mCachePlatforms)
      {
        section.Add(new CheckboxElement(g));
      }
      var viewController = new DialogViewController (
        new RootElement ("Select platforms", new  RadioGroup (0)){ 
        section
      }, true);

      viewController.OnSelection += (NSIndexPath obj) => {

      };
      NavigationController.PushViewController (viewController, true);
    }

    #endregion
  }
}
