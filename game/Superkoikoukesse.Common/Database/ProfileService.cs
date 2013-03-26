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

			// Get or register the player on the webservice
			WebserviceGetPlayer ws = new WebserviceGetPlayer (AuthenticatedPlayer.PlayerId);
			
			ws.Request ((player) => {
				if (player != null) {
					// Just save the new profile
					DatabaseService.Instance.SavePlayer (player);
				}
			},
			(exception) => {
				// Well, we crashed. See the log?
			});

			// Update data
			UpdatePlayer ();
		}

		/// <summary>
		/// Updates the player profile by calling the webservice.
		/// </summary>
		public void UpdatePlayer ()
		{
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
				localPlayer.Credits = 5;
			}

			DatabaseService.Instance.SavePlayer (localPlayer);

			// Notify server
			if (addCredits) {
			}
		}

		/// <summary>
		/// Upload the disconnected data.
		/// </summary>
		private void updateDisconnectedData ()
		{
			Player localPlayer = Player;

			if (localPlayer.DisconnectedCoinsEarned > 0) {
				UseCredit (localPlayer.DisconnectedCoinsEarned);
				localPlayer.DisconnectedCoinsEarned = 0;
			}

			if (localPlayer.DisconnectedCreditsUsed > 0) {
				UseCoins (localPlayer.DisconnectedCoinsEarned);
				localPlayer.DisconnectedCreditsUsed = 0;
			}
		}

		public void UseCredit (int creditsUsed)
		{
			// TODO
		}

		public void UseCoins (int coinsUsed)
		{
			// TODO
		}
	}
}

