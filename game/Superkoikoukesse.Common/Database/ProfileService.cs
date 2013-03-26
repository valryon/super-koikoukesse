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
		/// Initialize profile service
		/// </summary>
		/// <param name="aplayer">Aplayer.</param>
		public void Initialize (AuthenticatedPlayer aplayer)
		{
			AuthenticatedPlayer = aplayer;
			bool createNewProfile = false;

			// try to get from database
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
					// Merge inormations
					Player localPlayer = Player;
					
					localPlayer.Credits = player.Credits;
					localPlayer.Coins = player.Coins;
					localPlayer.DisplayName = player.DisplayName;
				} else {
					Logger.Log (LogLevel.Info, "Unknow player to server");
					
					// Create player
					WebserviceCreatePlayer wsCreate = new WebserviceCreatePlayer (Player);

					wsCreate.CreatePlayer();
				}
			},
			(exception) => {
				// Well, we crashed. See the log?
				Logger.Log (LogLevel.Error, "Calling service failed!");
			});

			// Do we have some disconnected data that we want to upload now?
			updateDisconnectedData ();

			// Is it time to earn some credits?
			earnSomeCredits ();
		}

		/// <summary>
		/// Earns some credits.
		/// </summary>
		private void earnSomeCredits ()
		{
			// Get local player
			Player localPlayer = Player;

			// Time to get credits?
			bool addCredits = (localPlayer.LastCreditsUpdate.AddDays (1) <= DateTime.Now);

			if (addCredits) {
				int currentCredits = localPlayer.Credits;
				localPlayer.Credits = 5;

				AddCredit (localPlayer.Credits - currentCredits);
			}

			DatabaseService.Instance.SavePlayer (localPlayer);
		}

		/// <summary>
		/// Upload the disconnected data.
		/// </summary>
		private void updateDisconnectedData ()
		{
			Player localPlayer = Player;

			if (localPlayer.DisconnectedCoinsEarned > 0) {
				AddCredit (localPlayer.DisconnectedCoinsEarned);
				localPlayer.DisconnectedCoinsEarned = 0;
			}

			if (localPlayer.DisconnectedCreditsUsed > 0) {
				AddCoins (localPlayer.DisconnectedCoinsEarned);
				localPlayer.DisconnectedCreditsUsed = 0;
			}
		}

		public void AddCredit (int creditsUsed)
		{
			if (creditsUsed != 0) {
				WebservicePlayerCredits wsCredits = new WebservicePlayerCredits (Player);
				wsCredits.AddCredits (creditsUsed);
			}
		}

		public void AddCoins (int coinsUsed)
		{
			if (coinsUsed != 0) {
				WebservicePlayerCoins wsCoins = new WebservicePlayerCoins (Player);
				wsCoins.AddCoins (coinsUsed);
			}
		}
	}
}

