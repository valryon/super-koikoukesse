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

		private DateTime lastProfileCacheTime;
		private Player cachedLocalPlayer;

		public Player Player {
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
			bool createNewProfile = false;

			// try to get from database (look at the getter)
			if (Player == null) {

				// Nothing found: create something
				createNewProfile = true;
			} else {
				// Game center id is not the same as stored id?
				if (Player.Id != aplayer.PlayerId) {
					createNewProfile = true;
				}
			}

			// Locally create the player
			if (createNewProfile) {
				Player freshlyCreatedPlayer = new Player (aplayer);
				
				DatabaseService.Instance.SavePlayer (freshlyCreatedPlayer);
			}

			UpdatePlayer ();
		}

		/// <summary>
		/// Updates the player profile by calling the webservice.
		/// </summary>
		public void UpdatePlayer ()
		{
			// Get the player on the webservice and merge data
			WebserviceGetPlayer ws = new WebserviceGetPlayer (Player.Id);
			
			ws.Request ((player) => {
				if (player != null) {

					// TODO Sometimes we need to merge server data

					// Do we have some disconnected data that we want to upload now?
					updateDisconnectedData ();
				} 
			},
			(code, exception) => {

				if (code == 102) {
					// New player to create
					Logger.Log (LogLevel.Info, "Unknow player to server");
					
					// Create player
					WebserviceCreatePlayer wsCreate = new WebserviceCreatePlayer (Player);
					wsCreate.CreatePlayer ();
				} else {
					// Well, we crashed. See the log?
					Logger.Log (LogLevel.Error, "Calling profile service failed!");
				}
			});
		}

		/// <summary>
		/// Earns some credits.
		/// </summary>
		public void EarnSomeCredits ()
		{
			// Get local player
			Player localPlayer = Player;

			if (localPlayer != null) {
				// Time to get credits?
				bool addCredits = (localPlayer.LastCreditsUpdate.AddDays (1) <= DateTime.Now);

				if (addCredits) {
					int currentCredits = localPlayer.Credits;
					localPlayer.Credits = 5;
					localPlayer.LastCreditsUpdate = DateTime.Now;
					int newCreditsCount = localPlayer.Credits - currentCredits;

					ModifyCredits (newCreditsCount, null,
				               (code) => {
						// Server failed to update credits
						localPlayer.DisconnectedCreditsUsed = -newCreditsCount;
						DatabaseService.Instance.SavePlayer (localPlayer);
					});

					DatabaseService.Instance.SavePlayer (localPlayer);
				}
			}
		}

		/// <summary>
		/// Upload the disconnected data.
		/// </summary>
		private void updateDisconnectedData ()
		{
			Player localPlayer = Player;

			if (localPlayer.DisconnectedCoinsEarned != 0) {
				ModifyCoins (localPlayer.DisconnectedCoinsEarned,
				               () => {
					// Success
					localPlayer.DisconnectedCoinsEarned = 0;
					DatabaseService.Instance.SavePlayer (localPlayer);
				},
				null);
			}

			if (localPlayer.DisconnectedCreditsUsed != 0) {
				ModifyCredits (localPlayer.DisconnectedCreditsUsed,
				             () => {
					// Success
					localPlayer.DisconnectedCreditsUsed = 0;
					DatabaseService.Instance.SavePlayer (localPlayer);
				}, null);
			}
		}

		protected void ModifyCredits (int creditsUsed, Action callback, Action<int> callbackFailture)
		{
			if (creditsUsed != 0) {

				Logger.Log (LogLevel.Info, "Modifying credits: " + creditsUsed);

				// Update local
				Player p = Player;
				p.Credits += creditsUsed;
				
				DatabaseService.Instance.SavePlayer (p);

				// Tell the server
				WebservicePlayerCredits wsCredits = new WebservicePlayerCredits (Player);
				wsCredits.AddCredits (creditsUsed, 
				                      callback,

				                      (code) => {
					
					// Store coins in disconnected data
					Player localPlayer = Player;
					localPlayer.DisconnectedCreditsUsed += creditsUsed;
					
					DatabaseService.Instance.SavePlayer (localPlayer);
					
					if (callbackFailture != null) {
						callbackFailture (code);
					}
				});
			}
		}

		protected void ModifyCoins (int coinsUsed, Action callback, Action<int> callbackFailture)
		{
			if (coinsUsed != 0) {

				Logger.Log (LogLevel.Info, "Using coins: " + coinsUsed);

				// Update local
				Player p = Player;
				p.Coins += coinsUsed;
				
				DatabaseService.Instance.SavePlayer (p);
				
				// Tell the server
				WebservicePlayerCoins wsCoins = new WebservicePlayerCoins (Player);
				wsCoins.AddCoins (coinsUsed,
				                  callback,
				                  (code) => {

					// Store coins in disconnected data
					Player localPlayer = Player;
					localPlayer.DisconnectedCoinsEarned += coinsUsed;

					DatabaseService.Instance.SavePlayer (localPlayer);

					if (callbackFailture != null) {
						callbackFailture (code);
					}
				});
			}
		}

		/// <summary>
		/// Use one credit
		/// </summary>
		public void UseCredit ()
		{
			ModifyCredits (-1, null, null);
		}
	}
}

