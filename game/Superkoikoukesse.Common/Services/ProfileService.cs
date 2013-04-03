using System;

namespace Superkoikoukesse.Common
{
	public class ProfileService
	{
		#region Singleton 
		
		private static ProfileService m_instance;
		
		private ProfileService ()
		{
		}
		
		/// <summary>
		/// Singleton
		/// </summary>
		/// <value>The instance.</value>
		public static ProfileService Instance {
			get {
				if (m_instance == null) {
					m_instance = new ProfileService ();
				}
				
				return m_instance;
			}
		}
		
		#endregion

		public AuthenticatedPlayer AuthenticatedPlayer { get; private set; }

		public event Action<Player> PlayerUpdated;

		private DateTime lastProfileCacheTime;
		private Player cachedLocalPlayer;

		public Player CachedPlayer {
			get {
				if (lastProfileCacheTime.AddMinutes (Constants.ProfileCacheDuration) <= DateTime.Now) {
					lastProfileCacheTime = DateTime.Now;

					cachedLocalPlayer = DatabaseService.Instance.ReadPlayer ();
				}

				return cachedLocalPlayer;
			}
		}

		/// <summary>
		/// Initialize a local profile from a player service like Game center
		/// </summary>
		/// <param name="aplayer">Aplayer.</param>
		public void Initialize (AuthenticatedPlayer aplayer)
		{
			AuthenticatedPlayer = aplayer;

			// Call game service auth method (Game center)
			// ------------------------------------------------------------------------
			aplayer.Authenticate (() => {

				bool createNewProfile = false;
				
				// try to get from database (look at the getter)
				if (CachedPlayer == null) {
					
					// Nothing found: create something
					createNewProfile = true;
					
				} else {
					// Game center id is not the same as stored id?
					if (aplayer.IsAuthenticated) {
						if (CachedPlayer.Id != Player.CleanId (aplayer.PlayerId)) {
							createNewProfile = true;
						}
					}
				}
				
				// Locally create the player
				// ------------------------------------------------------------------------
				if (createNewProfile) {

					// Use game service infos if available
					Player freshlyCreatedPlayer = new Player (aplayer);

					savePlayer (freshlyCreatedPlayer);
				}

				// Check the player on the server side
				WebserviceGetPlayer ws = new WebserviceGetPlayer (CachedPlayer.Id);

				// Make a GET and see if the player exists
				// ------------------------------------------------------------------------
				ws.Request ((serverPlayer) => {
					if (serverPlayer != null) {
						
						// Merge server data
						Player localPlayer = CachedPlayer;

						// Server credits should be right
						localPlayer.Credits = Math.Max (serverPlayer.Credits - localPlayer.DisconnectedCreditsUsed, 0);
						localPlayer.Coins = Math.Max (serverPlayer.Coins - localPlayer.DisconnectedCoinsEarned, 0); // We don't want negative values

						savePlayer (localPlayer);

						// Simply update
						UpdatePlayer ();
					} 
				},
				(code, exception) => {
					
					if (code == 102) {

						// New player to create
						Logger.Log (LogLevel.Info, "Unknow player to server");
						
						// Create player on server
						// ------------------------------------------------------------------------
						WebserviceCreatePlayer wsCreate = new WebserviceCreatePlayer (CachedPlayer);
						wsCreate.CreatePlayer ();

					} else {
						// Well, we crashed. See the log?
						Logger.Log (LogLevel.Error, "Calling profile service failed!");
					}

					// Update anyway
					UpdatePlayer ();
				});
			});
		}

		/// <summary>
		/// Updates the player profile (credits and coins)
		/// </summary>
		public void UpdatePlayer ()
		{
			// Do we have some disconnected data that we want to upload now?
			updateDisconnectedData ();

			// Add credits if its time to do so
			earnSomeCredits ();

			if (PlayerUpdated != null) {
				PlayerUpdated (CachedPlayer);
			}
		}

		/// <summary>
		/// Earns some credits.
		/// </summary>
		private void earnSomeCredits ()
		{
			// Get local player
			Player localPlayer = CachedPlayer;

			if (localPlayer != null) {
				// Time to get credits?
				bool addCredits = (localPlayer.LastCreditsUpdate.AddDays (1) <= DateTime.Now);
				addCredits = true;
				if (addCredits) {

					Logger.Log (LogLevel.Info, "Reseting credits!");

					// Modify the stored profile
					int currentCredits = localPlayer.Credits;

					// Reset credits
					localPlayer.Credits = 5;

					// Set date to today
					localPlayer.LastCreditsUpdate = DateTime.Now;
					int localAndServerCreditsDiff = localPlayer.Credits - currentCredits;

					// Save
					savePlayer (localPlayer);

					if(localAndServerCreditsDiff != 0) {
						// Tell server what we've done
						ModifyCredits (localAndServerCreditsDiff, false, null,
					               (code) => {
							// Server failed to update credits, store them for later
							Player p = CachedPlayer;
							p.DisconnectedCreditsUsed = -localAndServerCreditsDiff;

							savePlayer (localPlayer);
						});
					}
				}
			}
		}

		/// <summary>
		/// Upload the disconnected data.
		/// </summary>
		private void updateDisconnectedData ()
		{
			Player localPlayer = CachedPlayer;

			if (localPlayer.DisconnectedCoinsEarned != 0) {
				ModifyCoins (localPlayer.DisconnectedCoinsEarned,
				               () => {
					// Success
					localPlayer.DisconnectedCoinsEarned = 0;

					savePlayer (localPlayer);
				},
				null);
			}

			if (localPlayer.DisconnectedCreditsUsed != 0) {
				ModifyCredits (localPlayer.DisconnectedCreditsUsed, true,
				             () => {
					// Success
					localPlayer.DisconnectedCreditsUsed = 0;

					savePlayer (localPlayer);

				}, null);
			}
		}

		protected void ModifyCredits (int creditsUsed, bool updateLocalPlayer, Action callback, Action<int> callbackFailture)
		{
			if (creditsUsed != 0) {

				Logger.Log (LogLevel.Info, "Modifying credits: " + creditsUsed);

				// Update local
				if (updateLocalPlayer) {
					Player p = CachedPlayer;
					p.Credits += creditsUsed;
				
					DatabaseService.Instance.SavePlayer (p);
				}

				// Tell the server
				WebservicePlayerCredits wsCredits = new WebservicePlayerCredits (CachedPlayer);
				wsCredits.AddCredits (creditsUsed, 
				                      callback,

				                      (code) => {
					
					// Store coins in disconnected data
					Player localPlayer = CachedPlayer;
					localPlayer.DisconnectedCreditsUsed += creditsUsed;
					
					savePlayer (localPlayer);
					
					if (callbackFailture != null) {
						callbackFailture (code);
					}
				});
			}
		}

		protected void ModifyCoins (int coinsUsed, Action callback, Action<int> callbackFailture)
		{
			if (coinsUsed != 0) {

				Logger.Log (LogLevel.Info, "Modifying coins: " + coinsUsed);

				// Update local
				Player p = CachedPlayer;
				p.Coins += coinsUsed;
				
				DatabaseService.Instance.SavePlayer (p);
				
				// Tell the server
				WebservicePlayerCoins wsCoins = new WebservicePlayerCoins (CachedPlayer);
				wsCoins.AddCoins (coinsUsed,
				                  callback,
				                  (code) => {

					// Store coins in disconnected data
					Player localPlayer = CachedPlayer;
					localPlayer.DisconnectedCoinsEarned += coinsUsed;

					savePlayer (localPlayer);

					if (callbackFailture != null) {
						callbackFailture (code);
					}
				});
			}
		}
		
		/// <summary>
		/// Add credits (DEBUG ONLY)
		/// </summary>
		public void AddCreditsDebug (int credits)
		{
			ModifyCredits (credits, true, null, null);
			
			if (PlayerUpdated != null) {
				PlayerUpdated (CachedPlayer);
			}
		}

		
		/// <summary>
		/// Add credits 
		/// </summary>
		public void AddCoins (int coins)
		{
			ModifyCoins (coins, null, null);
			
			if (PlayerUpdated != null) {
				PlayerUpdated (CachedPlayer);
			}
		}


		/// <summary>
		/// Use one credit
		/// </summary>
		public void UseCredit ()
		{
			ModifyCredits (-1, true, null, null);
		}

		// Save player routine

		private object saveLock;

		private void savePlayer (Player p)
		{

			if (saveLock == null)
				saveLock = new object ();

			lock (saveLock) {
				lastProfileCacheTime = DateTime.MinValue;
				DatabaseService.Instance.SavePlayer (p);
			}
		}
	}
}

