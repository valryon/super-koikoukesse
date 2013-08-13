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
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		#region Fields

		private bool mDatabaseLoaded;
		private bool mConfigurationLoaded;

		#endregion

		#region Constructor & Initialization

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Logger.I ("Launching app...");

      		new PXNAppearance();

			// Global parameters
			EncryptionHelper.SetKey (Constants.ENCRYPTION_KEY);
			ImageDatabase.Instance.Initialize (Constants.IMAGE_ROOT_LOCATION);

//			mSplashScreenViewController = new SplashscreenViewController ();
//			SwitchToView (mSplashScreenViewController);

			// Load all the things!
			LoadDatabase ();
			LoadConfiguration ();

			return true;
		}

		/// <summary>
		/// Load the database in a thread
		/// </summary>
		private void LoadDatabase ()
		{
			mDatabaseLoaded = false;

			// Create or load database
			GameDatabase.Instance.Load (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments), Constants.DATABASE_LOCATION));

			// Create database structure as fast as possible so other threads can manipulate it.
			if (GameDatabase.Instance.Exists == false) {

				GameDatabase.Instance.CreateTables ();
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

				mDatabaseLoaded = true;

				// Maybe it was the last thing to load
				InvokeOnMainThread (() => {
					LoadingProgress ();
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
					Window.RootViewController.PresentViewController (gcController, true, null);
				});
			};

			// Player events
			PlayerCache.Instance.PlayerUpdated += (Player p) => {

				InvokeOnMainThread (() => {

//					if (mMenuViewController != null) {
//						mMenuViewController.UpdateViewWithPlayerInfos ();
//					}
				});
			};

			// Store a local profile from the game center info
			// Or create a temporary local player
			PlayerCache.Instance.Initialize (GameCenter);
		}

		/// <summary>
		/// Load configuration in a thread
		/// </summary>
		private void LoadConfiguration ()
		{
			mConfigurationLoaded = false;

			InvokeInBackground (() => {

				// Load the configuration from webservice or from local
				UpdateConfiguration (() => {

					mConfigurationLoaded = true;

					// Maybe it was the last thing to load
					InvokeOnMainThread (() => {
						LoadingProgress ();
					});
				});
			});
		}

		/// <summary>
		/// Update the loading view, dismiss if everything has been loaded
		/// </summary>
		private void LoadingProgress ()
		{
			Logger.I ("Loading... Database: " + mDatabaseLoaded + " Configuration: " + mConfigurationLoaded);

			IsInitialized = (mDatabaseLoaded && mConfigurationLoaded);

			if (IsInitialized) {
				SetLoading (false);
			}
		}
		#endregion

		#region Methods

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations (UIApplication application, UIWindow forWindow)
		{
			return AppDelegate.HasSupportedInterfaceOrientations ();
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

				Logger.I ("Configuration loaded and updated.");

				if (complete != null)
					complete ();
			},
			                  (code, e) => {
				Logger.W ("Configuration was not loaded!. ");

				// Try to use local
				this.Configuration = configWs.LastValidConfig;

				// No local? This is bad. Use default values.
				if (this.Configuration == null) {

					Logger.W ("Using default (local and bad) values!. ");

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
			if (isLoading) {
				if (OnLoading != null) {
					OnLoading ();
				}
			} else {
				if (OnLoadingComplete != null) {
					OnLoadingComplete ();
				}
			}
		}

		#endregion

		#region Game center views

		private GKLeaderboardViewController m_gkLeaderboardview;
		//		private GKAchievementViewController m_gkAchievementview;

		/// <summary>
		/// Display Game center leaderboards
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="callback">Callback.</param>
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

					Window.RootViewController.PresentViewController (m_gkLeaderboardview, true, null);
				}
			} 
		}
		#endregion

		#endregion

		#region Events

		/// <summary>
		/// Loading
		/// </summary>
		public event Action OnLoading;

		/// <summary>
		/// Game is ready to be played
		/// </summary>
		public event Action OnLoadingComplete;

		#endregion

		#region Static Properties

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
		public static UIInterfaceOrientationMask HasSupportedInterfaceOrientations ()
		{
			// If iPad, only landscape
			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
				return UIInterfaceOrientationMask.LandscapeLeft | UIInterfaceOrientationMask.LandscapeRight;

			// If iPhone, only portrait
			return UIInterfaceOrientationMask.Portrait | UIInterfaceOrientationMask.PortraitUpsideDown;
		}
		#endregion

		#region Properties

		/// <summary>
		/// For storyboard handle
		/// </summary>
		/// <value>The window.</value>
		public override UIWindow Window {
			get;
			set;
		}

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

		#endregion
	}
}

