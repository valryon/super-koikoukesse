using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using System.IO;
using MonoTouch.GameKit;
using System.Linq;

namespace SuperKoikoukesse.iOS
{
  public partial class MenuViewController : UIViewController
  {
    #region Members

    private List<UIViewController> mCards;
    private MenuDifficultyViewController mDifficultyViewController;
    private CardScoreViewController mHighScorePanel;
    private GameLauncher mGameLauncher;

    #endregion

    #region Constructors

    public MenuViewController(IntPtr handle)
			: base (handle)
    {
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      appDelegate.OnLoading += () => ViewLoading.Hidden = false;
      appDelegate.OnLoadingComplete += () => ViewLoading.Hidden = true;

      mCards = new List<UIViewController>();
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      // Remove all occurrences of SplashscreenViewController
      NavigationController.ViewControllers = NavigationController.ViewControllers
        .Where(val => !(val is SplashscreenViewController)).ToArray();

      NavigationController.SetNavigationBarHidden(false, false);

      // Hide credits and coins until player profile is loaded
      LabelCoins.Hidden = true;
      LabelCredits.Hidden = true;
		
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      appDelegate.LoadPlayerProfile();
    }

    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      if (mCards.Count == 0)
      {
        // We need auto layout to be set up, so we can create panels only here
        CreateCards();
      }

      ButtonDebug.SetTitle(Constants.DEBUG_MODE + "", UIControlState.Normal);

      UpdateViewWithPlayerInfos();
    }

    #endregion

    #region Methods

    public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
    {
      return AppDelegate.HasSupportedInterfaceOrientations();
    }

    /// <summary>
    /// Update tickets and coins counters
    /// </summary>
    public void UpdateViewWithPlayerInfos()
    {
      // Show infos is they were hidden
      if (LabelCoins.Hidden)
      {
        LabelCoins.Hidden = false;
        LabelCredits.Hidden = false;

        // Fade in
        LabelCoins.Alpha = 0f;
        LabelCredits.Alpha = 0f;

        UIView.Animate(2f, () => {
          LabelCoins.Alpha = 1f;
          LabelCredits.Alpha = 1f;
        });
      }

      // Load the player from db
      Player profile = PlayerCache.Instance.CachedPlayer;

      // Display credits and coins
      if (profile != null)
      {
        LabelCredits.Text = profile.Credits.ToString();
        LabelCoins.Text = profile.Coins.ToString();
      }

      // Update leaderboards ?
      if (mHighScorePanel != null)
      {
        mHighScorePanel.ForceUpdate();
      }

    }

    #region Scroll view and pagination

    /// <summary>
    /// Create the cards for the menu
    /// </summary>
    private void CreateCards()
    {
      PageControl.ValueChanged += (object sender, EventArgs e) => {
        SetScrollViewToPage(PageControl.CurrentPage);
      };
      ScrollView.DecelerationEnded += (object sender, EventArgs e) => {
        double page = Math.Floor((ScrollView.ContentOffset.X - ScrollView.Frame.Width / 2) / ScrollView.Frame.Width) + 1;
				
        PageControl.CurrentPage = (int) page;
      };

      mHighScorePanel = null;
      mCards.Clear();

      // Credits
      CardInfoViewController infos = new CardInfoViewController();
      mCards.Add(infos);

      // Highscores
      mHighScorePanel = new CardScoreViewController();
      mCards.Add(mHighScorePanel);

      //
      // Build modes
      //

      // -- Score attack
      CardModeViewController scoreAttackMode = new CardModeViewController(GameMode.SCORE);
      scoreAttackMode.GameModeSelected += HandleGameModeSelected;
      mCards.Add(scoreAttackMode);

      // -- Time attack
      CardModeViewController timeAttackMode = new CardModeViewController(GameMode.TIME);
      timeAttackMode.GameModeSelected += HandleGameModeSelected;
      mCards.Add(timeAttackMode);

      // -- Survival
      CardModeViewController survivalMode = new CardModeViewController(GameMode.SURVIVAL);
      survivalMode.GameModeSelected += HandleGameModeSelected;
      mCards.Add(survivalMode);

      // -- Versus
      CardModeViewController versusMode = new CardModeViewController(GameMode.VERSUS);
      versusMode.GameModeSelected += HandleGameModeSelected;
      mCards.Add(versusMode);

      int count = mCards.Count;
      RectangleF scrollFrame = ScrollView.Frame;

      scrollFrame.Width = scrollFrame.Width * count;
      ScrollView.ContentSize = scrollFrame.Size;

      for (int i = 0; i < count; i++)
      {

        // Compute location and size
        RectangleF frame = ScrollView.Frame;

        PointF location = new PointF();
        location.X = frame.Width * i;
        frame.Location = location;

        mCards[i].View.Frame = frame;

        // Add to scroll and paging
        ScrollView.AddSubview(mCards[i].View);
      }

      PageControl.Pages = count;

      // Set 3rd page as the first displayed
      int firstDisplayedPageNumber = 2;
      PageControl.CurrentPage = firstDisplayedPageNumber;

      SetScrollViewToPage(firstDisplayedPageNumber);
    }

    private void SetScrollViewToPage(int page)
    {
      ScrollView.SetContentOffset(new PointF(page * ScrollView.Frame.Width, 0), true);
    }

    #endregion

    /// <summary>
    /// Pop-up the Game Center matchmaker
    /// </summary>
    /// <param name="mode">Mode.</param>
    private void DisplayMatchMaker(GameMode mode)
    {
      PerformSegue("MenuToVersus", this);
    }

    /// <summary>
    /// Pop-up the difficulty chooser
    /// </summary>
    /// <param name="selectedMode">Selected mode.</param>
    private void DisplayDifficultyChooser(GameMode selectedMode)
    {
      // Display difficulty view
      if (mDifficultyViewController == null)
      {
        mDifficultyViewController = new MenuDifficultyViewController();
        mDifficultyViewController.DifficultySelected += (GameMode mode, GameDifficulties difficulty) => {
          var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
					
          Filter filter = null;

          // Remember to select Versus match parameters too
          if (mode == GameMode.VERSUS)
          {
            VersusMatch currentMatch = PlayerCache.Instance.AuthenticatedPlayer.CurrentMatch;

            currentMatch.Difficulty = difficulty;
            filter = currentMatch.Filter; // This is weird
          }

          mGameLauncher = new GameLauncher(this);
          mGameLauncher.Launch("MenuToGame", mode, difficulty, filter);
        }; 	
      }
			
      mDifficultyViewController.SetMode(selectedMode);
      mDifficultyViewController.View.Frame = View.Frame;
			
      View.AddSubview(mDifficultyViewController.View);
    }

    void HandleGameModeSelected(GameMode m)
    {
      // Enough credits?
      if (PlayerCache.Instance.CachedPlayer.Credits > 0)
      {
        if (m == GameMode.VERSUS)
        {
          if (PlayerCache.Instance.AuthenticatedPlayer.IsAuthenticated == false)
          {
            PlayerCache.Instance.AuthenticatedPlayer.Authenticate(() => {

              if (PlayerCache.Instance.AuthenticatedPlayer.IsAuthenticated)
              {
                DisplayMatchMaker(m);
              }
              else
              {
                // Dialog
                Dialogs.ShowAuthenticationRequired();
              }
            });
          }
          else
          {
            DisplayMatchMaker(m);
          }
        }
        else
        {
          DisplayDifficultyChooser(m);
        }
      }
      else
      {
        Dialogs.ShowNoMoreCreditsDialogs();
      }
    }

    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
    {
      base.PrepareForSegue(segue, sender);

      if (mGameLauncher!= null && segue.Identifier == mGameLauncher.SegueId)
      {
        GameViewController gameVc = (GameViewController) segue.DestinationViewController;

        // Prepare quizz
        mGameLauncher.Prepare(gameVc);

        // Hide loading
        ViewLoading.Hidden = true;
      }
    }

    #endregion

    #region Handlers

    partial void OnSettingsTouched(MonoTouch.Foundation.NSObject sender)
    {
      PerformSegue("MenuToSettings", this);
    }

    partial void OnShopTouched(MonoTouch.Foundation.NSObject sender)
    {
      PerformSegue("MenuToShop", this);
    }

    /// <summary>
    /// Force config reload (DEBUG)
    /// </summary>
    /// <param name="sender">Sender.</param>
    partial void OnConfigTouched(MonoTouch.Foundation.NSObject sender)
    {
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      appDelegate.UpdateConfiguration(null);
    }

    partial void OnDebugTouched(MonoTouch.Foundation.NSObject sender)
    {
      Constants.DEBUG_MODE = !Constants.DEBUG_MODE;
      ButtonDebug.SetTitle(Constants.DEBUG_MODE + "", UIControlState.Normal);
      Logger.I("Debug mode? " + Constants.DEBUG_MODE);
    }

    partial void OnCreditsTouched(MonoTouch.Foundation.NSObject sender)
    {
      PlayerCache.Instance.AddCreditsDebug(Constants.BASE_CREDITS);
    }

    partial void OnCoinsTouched(MonoTouch.Foundation.NSObject sender)
    {
      PlayerCache.Instance.AddCoins(Constants.BASE_COINS);
    }

    #endregion
  }
}

