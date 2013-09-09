// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
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

    private AbstractCardViewController _currentCard;

    private CardsController _cardsController;
    private CardChallengeViewController _challengeViewController;
    private CardScoreViewController _scoresCard;
    private GameLauncher _gameLauncher;

    #endregion

    #region Constructors

    public MenuViewController(IntPtr handle)
			: base (handle)
    {
      var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
//      appDelegate.OnLoading += () => ViewLoading.Hidden = false;
//      appDelegate.OnLoadingComplete += () => ViewLoading.Hidden = true;
    }

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();

      // Remove all occurrences of SplashscreenViewController in the navigation stack
      NavigationController.ViewControllers = NavigationController.ViewControllers
        .Where(val => !(val is SplashscreenViewController)).ToArray();

      // Show the navbar
      NavigationController.SetNavigationBarHidden(false, false);

      // Load the player profile
      var app = (AppDelegate) UIApplication.SharedApplication.Delegate; 
      if (app.GameCenterPlayer == null)
      {
        app.LoadPlayerProfile();
      }

      // Create the cards controller (the scrollview & pagecontroll need to be initialized)
      _cardsController = new CardsController(ScrollView, PageControl);
      _cardsController.CardChanged += OnCardChanged;
    }

    public override void ViewDidAppear(bool animated)
    {
      base.ViewDidAppear(animated);

      if (_cardsController.Count == 0)
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

    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
      base.TouchesBegan(touches, evt);

      if (_challengeViewController != null && _challengeViewController.IsPresented)
      {
        HideChallengeCard();
      }
    }

    public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
    {
      base.PrepareForSegue(segue, sender);

      if (_gameLauncher!= null && segue.Identifier == _gameLauncher.SegueId)
      {
        GameViewController gameViewController = (GameViewController) segue.DestinationViewController;

        // Prepare quizz
        _gameLauncher.Prepare(gameViewController);
      }
    }

    /// <summary>
    /// Update tickets and coins counters
    /// </summary>
    private void UpdateViewWithPlayerInfos()
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
      if (_scoresCard != null)
      {
        _scoresCard.ForceUpdate();
      }
    }

    #region Scroll view and pagination

    /// <summary>
    /// Create the cards for the menu
    /// </summary>
    private void CreateCards()
    {
      _scoresCard = null;
      _cardsController.Clear();

      // Credits
      var infoCard = new CardInfoViewController();
      infoCard.CreditsDisplayed += DisplayCredits;

      // Highscores
      _scoresCard = new CardScoreViewController();

      // Mode Score Card
      var scoreCard = new CardModeViewController(GameMode.SCORE);
      scoreCard.GameModeSelected += OnGameModeSelected;

      // Mode Time Card
      var timeCard = new CardModeViewController(GameMode.TIME);
      timeCard.GameModeSelected += OnGameModeSelected;

      // Mode Survival Card
      var survivalCard = new CardModeViewController(GameMode.SURVIVAL);
      survivalCard.GameModeSelected += OnGameModeSelected;

      // Mode Versus Card
      var versusCard = new CardModeViewController(GameMode.VERSUS);
      versusCard.GameModeSelected += OnGameModeSelected;

      // Add cards
      _cardsController.AddCard(infoCard);
      _cardsController.AddCard(_scoresCard);
      _cardsController.AddCard(scoreCard);
      _cardsController.AddCard(timeCard);
      _cardsController.AddCard(survivalCard);
      _cardsController.AddCard(versusCard);

      // Init
      _cardsController.Init();
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

    private void DisplayCredits()
    {
      PerformSegue("MenuToCredits", this);
    }

    /// <summary>
    /// Pop-up the difficulty chooser
    /// </summary>
    /// <param name="selectedMode">Selected mode.</param>
    private void DisplayDifficultyChooser(GameMode selectedMode)
    {
      // Display difficulty view
      if (_challengeViewController == null)
      {
        _challengeViewController = new CardChallengeViewController();
        _challengeViewController.Hidden += HideChallengeCard;
        _challengeViewController.DifficultySelected += (GameMode mode, GameDifficulty difficulty) => {
          var appDelegate = (AppDelegate) UIApplication.SharedApplication.Delegate; 
					
          Filter filter = null;

          // Remember to select Versus match parameters too
          if (mode == GameMode.VERSUS)
          {
            VersusMatch currentMatch = PlayerCache.Instance.AuthenticatedPlayer.CurrentMatch;

            currentMatch.Difficulty = difficulty;
            filter = currentMatch.Filter; // This is weird
          }

          _gameLauncher = new GameLauncher(this);
          _gameLauncher.Launch("MenuToGame", mode, difficulty, filter);
        }; 	
      }
			
      // Set mode
      _challengeViewController.SetMode(selectedMode);

      ShowChallengeCard();
    }

    /// <summary>
    /// Shows the challenge card.
    /// </summary>
    public void ShowChallengeCard()
    {
      // Add to the view
      _challengeViewController.AddToView(_currentCard.View);

      // Stop the scroll until the challenge view is visible
      ScrollView.ScrollEnabled = false;

      // Slowly fades out the PageControl
      UIView.Animate(
        0.7f, 
        () => PageControl.Alpha = 0f,
        () => PageControl.Hidden = true
      );
    }

    /// <summary>
    /// Hides the challenge card.
    /// </summary>
    public void HideChallengeCard()
    {
      // Remove
      _challengeViewController.RemoveFromView();

      // Restore the scroll
      ScrollView.ScrollEnabled = true;

      // Slowly fades in the PageControl
      UIView.Animate(
        0.7f, 
        () => {
          PageControl.Hidden = false;
          PageControl.Alpha = 1f;
        }
      );
    }

    #endregion

    #region Handlers

    private void OnCardChanged(AbstractCardViewController card)
    {
      _currentCard = card;
    }

    private void OnGameModeSelected(GameMode m)
    {
      // Stop if not enough credits
      if (PlayerCache.Instance.CachedPlayer.Credits <= 0)
      {
        Dialogs.ShowNoMoreCreditsDialogs();
        return;
      }

      if (m == GameMode.VERSUS)
      {
        if (PlayerCache.Instance.AuthenticatedPlayer.IsAuthenticated == false)
        {
          // Dialog
          Dialogs.ShowAuthenticationRequired(
            () => {
            PlayerCache.Instance.AuthenticatedPlayer.Authenticate(() => {

              if (PlayerCache.Instance.AuthenticatedPlayer.IsAuthenticated)
              {
                DisplayMatchMaker(m);
              }
            });
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

