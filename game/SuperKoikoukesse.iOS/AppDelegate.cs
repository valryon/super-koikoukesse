using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Superkoikoukesse.Common;
using Superkoikoukesse.Common.Utils;
using System.IO;
using MonoTouch.GameKit;

namespace SuperKoikoukesse.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		/// <summary>
		/// iPhone or iPad ?
		/// </summary>
		/// <value><c>true</c> if user interface idiom is phone; otherwise, <c>false</c>.</value>
		public static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

    /// <summary>
    /// Get the supported orientations for the device:
    /// - portrait for iPhone
    /// - landscape for iPad
    /// </summary>
    /// <returns>Landscape if iPad, portrait otherwise</returns>
    public static UIInterfaceOrientationMask HasSupportedInterfaceOrientations()
    {
      // If iPad, only landscape
      if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
        return UIInterfaceOrientationMask.LandscapeLeft | UIInterfaceOrientationMask.LandscapeRight;

      // If iPhone, only portrait
      return UIInterfaceOrientationMask.Portrait | UIInterfaceOrientationMask.PortraitUpsideDown;
    }

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations (UIApplication application, UIWindow forWindow)
		{
      return AppDelegate.HasSupportedInterfaceOrientations();
		}

		// class-level declarations
		UIWindow window;
		private SplashscreenViewController splashScreenViewController;
		private GameViewController gameViewController;
		private MenuViewController menuViewController;
		private ScoreViewController scoreViewController;
		private LoadingViewController loadingViewController;
		private CreditsViewController creditsViewController;
		private bool databaseLoaded;
		private bool configurationLoaded;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Logger.I("Launching app...");

			// Global parameters
			EncryptionHelper.SetKey (Constants.ENCRYPTION_KEY);
			ImageDatabase.Instance.Initialize (Constants.IMAGE_ROOT_LOCATION);

			// Create first view
			window = new UIWindow (UIScreen.MainScreen.Bounds);

			//viewController = new GameViewController ();
			splashScreenViewController = new SplashscreenViewController ();
			window.RootViewController = splashScreenViewController;
			window.MakeKeyAndVisible ();

			// Load all the things!
			loadDatabase ();
			loadConfiguration ();

			return true;
		}

		/// <summary>
		/// Load the database in a thread
		/// </summary>
		private void loadDatabase ()
		{
			databaseLoaded = false;

			// Create or load database
			GameDatabase.Instance.Load (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), Constants.DATABASE_LOCATION));

			// Create database structure as fast as possible so other threads can manipulate it.
			if (GameDatabase.Instance.Exists == false) {

				GameDatabase.Instance.CreateTables();
			}

			InvokeInBackground (() => {
 		
				// Create database structure as fast as possible so other threads can manipulate it.
				if (GameDatabase.Instance.Exists == false) {

					// Load gamedb.xml
					String xmlDatabase = File.ReadAllText (@"database/gamedb.xml");
					
					GameDatabase.Instance.InitializeFromXml (xmlDatabase);
				}

				// Check excluded games
				ServiceExcludedGames exGames = new ServiceExcludedGames ();
				exGames.Request ((list) => {
					
					foreach (int id in list.GamesId) {
						GameDatabase.Instance.RemoveGame (id);
					}
					
				}, null);

				databaseLoaded = true;

				// Maybe it was the last thing to load
				InvokeOnMainThread (() => {
					loadingProgress ();
				});
			});


		}

		/// <summary>
		/// Load configuration in a thread
		/// </summary>
		private void loadConfiguration ()
		{
			configurationLoaded = false;
			
			InvokeInBackground (() => {

				// Load the configuration from webservice or from local
				UpdateConfiguration (() => {

					configurationLoaded = true;

					// Maybe it was the last thing to load
					InvokeOnMainThread (() => {
						loadingProgress ();
					});
				});
			});
		}

		/// <summary>
		/// Load Game Center and player profile
		/// It's not the same mechanism, due to Game Center which should be displayed on the menu.
		/// </summary>
		public void LoadPlayerProfile ()
		{
			// On main thread we load Game Center
			GameCenter = new GameCenterPlayer ();
			GameCenter.ShowGameCenter += (UIViewController gcController) => {
				InvokeOnMainThread (() => {
					window.RootViewController.PresentViewController (gcController, true, null);
				});
			};

			// Player events
			PlayerCache.Instance.PlayerUpdated += (Player p) => {
				
				InvokeOnMainThread (() => {

					if (menuViewController != null) {
						menuViewController.UpdateViewWithPlayerInfos ();
					}
				});
			};
			
			// Store a local profile from the game center info
			// Or create a temporary local player
			PlayerCache.Instance.Initialize (GameCenter);
		}

		private void loadingProgress ()
		{
			Logger.I("Loading... Database: " + databaseLoaded + " Configuration: " + configurationLoaded);

			IsInitialized =  (databaseLoaded && configurationLoaded);

			if(IsInitialized) {
				SetLoading(false);
				if(InitializationComplete != null) {
					InitializationComplete();
				}
			}
		}

		/// <summary>
		/// Download webservice configuration
		/// </summary>
		public void UpdateConfiguration (Action complete)
		{
			// Get the distant configuration
			ServiceConfiguration configWs = new ServiceConfiguration ();
			configWs.Request ((config) => {
				this.Configuration = config;

				Logger.I("Configuration loaded and updated.");

				if (complete != null)
					complete ();
			},
			(code, e) => {
				Logger.W( "Configuration was not loaded!. ");

				// Try to use local
				this.Configuration = configWs.LastValidConfig;

				// No local? This is bad. Use default values.
				if (this.Configuration == null) {

					Logger.W( "Using default (local and bad) values!. ");

					this.Configuration = new GameConfiguration ();
				}

				if (complete != null)
					complete ();
			});
		
		}

		#region Loading

		/// <summary>
		/// Add or hide a loading view
		/// </summary>
		public void SetLoading (bool isLoading)
		{
			if (loadingViewController == null) {
				loadingViewController = new LoadingViewController ();
			}

			BeginInvokeOnMainThread (() => {

				if (isLoading) {
					DisplayLoading ();
				} else {
					HideLoading ();
				}
			});
		}

		/// <summary>
		/// Display the loading screen
		/// </summary>
		private void DisplayLoading ()
		{

			// Center
//			loadingViewController.View.Frame = new System.Drawing.RectangleF (
//
//				(window.RootViewController.View.Bounds.Width/2) - (loadingViewController.View.Frame.Width/2),
//				(window.RootViewController.View.Bounds.Height/2)- (loadingViewController.View.Frame.Height/2),
//				loadingViewController.View.Frame.Width,
//				loadingViewController.View.Frame.Height
//			);

			// Fill
			loadingViewController.View.Frame = (window.RootViewController.View.Bounds);

			window.RootViewController.View.AddSubview (loadingViewController.View);
		}

		/// <summary>
		/// Hide the loading screen
		/// </summary>
		private void HideLoading ()
		{
			loadingViewController.View.RemoveFromSuperview ();
			//loadingViewController.
		}

		#endregion

		#region Views

		public void SwitchToMenuView ()
		{
			if (menuViewController == null) {
				menuViewController = new MenuViewController ();
			}

			window.RootViewController.RemoveFromParentViewController ();
			window.RootViewController = menuViewController;
			window.MakeKeyAndVisible ();

			// Careful: we may not have fully loaded the game
			SetLoading(!IsInitialized);
		}

		/// <summary>
		/// Change to ingame view
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		public void SwitchToGameView (GameModes mode, GameDifficulties difficulty, Filter filter = null)
		{
			if (gameViewController == null) {
				gameViewController = new GameViewController ();
			}

			// Create a new filter
			Filter f = null;

			if(filter != null) 
			{
				f = filter;
			}
			else {
				f = new Filter ("0", "Siphon filter", "defaultIcon");
			}

			// Caching filter results
			SetLoading (true);

			f.Load ((gamesCount) => {

				if (gamesCount < 30) {
					BeginInvokeOnMainThread (() => {
						Dialogs.ShowDebugFilterTooRestrictive ();
					});
				}

				// Prepare quizz
				gameViewController.InitializeQuizz (mode, difficulty, f);

				// Display it
				BeginInvokeOnMainThread (() => {

					SetLoading (false);

					window.RootViewController.RemoveFromParentViewController ();
					window.RootViewController = gameViewController;
					window.MakeKeyAndVisible ();

					gameViewController.DisplayQuizz ();
				});

			});
			
		}

		public void SwitchToScoreView (Quizz quizz)
		{
			if (scoreViewController != null) {
				scoreViewController.Dispose ();
				scoreViewController = null;
			}
			scoreViewController = new ScoreViewController (quizz);
			
			window.RootViewController.RemoveFromParentViewController ();
			window.RootViewController = scoreViewController;
			window.MakeKeyAndVisible ();
		}

		/// <summary>
		/// Change to credits view
		/// </summary>
		public void SwitchToCreditsView ()
		{
			if (creditsViewController == null) {
				creditsViewController = new CreditsViewController ();
			}
			
			window.RootViewController.RemoveFromParentViewController ();
			window.RootViewController = creditsViewController;
			window.MakeKeyAndVisible ();
			
		}
	
		/// <summary>
		/// Diplsy shop
		/// </summary>
		public void SwitchToShopView ()
		{

		}

		/// <summary>
		/// Display options as pop up
		/// </summary>
		public void ShowOptions ()
		{

		}

		#endregion

		#region Game center

		private GKLeaderboardViewController m_gkLeaderboardview;
//		private GKAchievementViewController m_gkAchievementview;

		public void ShowLeaderboards (string id, Action callback)
		{
			if (PlayerCache.Instance.AuthenticatedPlayer.IsAuthenticated) {
				if (m_gkLeaderboardview == null) {
					m_gkLeaderboardview = new GKLeaderboardViewController ();
				}

				m_gkLeaderboardview.Category = id; 

				if (m_gkLeaderboardview != null) {
					m_gkLeaderboardview.DidFinish += delegate(object sender, EventArgs e) {
						m_gkLeaderboardview.DismissViewController (true, null);

						m_gkLeaderboardview = null;

						if (callback != null) {
							callback ();
						}
					};

					window.RootViewController.PresentViewController (m_gkLeaderboardview, true, null);
				}
			} 
		}

		#endregion

		/// <summary>
		/// Game is ready to be played
		/// </summary>
		public event Action InitializationComplete;

		/// <summary>
		/// Global configuration
		/// </summary>
		/// <value>The configuration.</value>
		public GameConfiguration Configuration { get; set; }

		/// <summary>
		/// Is loading something in background
		/// </summary>
		public bool IsInitialized { get; private set; }

		/// <summary>
		/// Game center data
		/// </summary>
		/// <value>The game center.</value>
		public GameCenterPlayer GameCenter  { get; private set; }

		protected override void Dispose (bool disposing)
		{
			if (gameViewController != null) {
				gameViewController.Dispose ();
			}
			if (menuViewController != null) {
				menuViewController.Dispose ();
			}

			base.Dispose (disposing);
		}
	}
}

