using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.GameKit;
using Superkoikoukesse.Common;
using MonoTouch.Foundation;

namespace SuperKoikoukesse.iOS
{
	/// <summary>
	/// Tools for Game Center
	/// </summary>
	public static class GameCenterHelper
	{
		private static object playerCacheLock = new object ();
		private static Dictionary<string, GKPlayer> playersCache = new Dictionary<string, GKPlayer> ();
		private static object playerImageCacheLock = new object ();
		private static Dictionary<GKPlayer, UIImage> playerImagesCache = new Dictionary<GKPlayer, UIImage> ();

		/// <summary>
		/// Gets (and download and cache if necessary) the player
		/// </summary>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="callback">Callback.</param>
		public static void GetPlayer (string playerId, Action<GKPlayer> callback)
		{
			if (playersCache.ContainsKey (playerId) == false) {

				lock (playerCacheLock) {

					if (playersCache.ContainsKey (playerId) == false) {

						Logger.I ("Loading player..." + playerId);

						// Ask Game center
						GKPlayer.LoadPlayersForIdentifiers (new string[] { playerId }, (players, error) => {

							if (error == null) {

								// Cache
								foreach (var p in players) {
									
									lock (playerCacheLock) {
										// TODO Lock foireux
										if(playersCache.ContainsKey(p.PlayerID) == false) {
											playersCache.Add (p.PlayerID, p);
										}
									}
								}

								if (callback != null) {
									callback (playersCache [playerId]);
								}

							} else {
								Logger.E ("GameCenterHelper.GetPlayer", error);
							}
						});
					} else {
						if (callback != null) {
							callback (playersCache [playerId]);
						}
					}
				}
			} else {
				if (callback != null) {
					callback (playersCache [playerId]);
				}
			}
		}

		/// <summary>
		/// Gets (and download and cache if necessary) the profile image for a player
		/// </summary>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="callback">Callback.</param>
		public static void GetProfileImage (GKPlayer player, Action<UIImage> callback)
		{
			if (playerImagesCache.ContainsKey (player) == false) {

				lock (playerImageCacheLock) {

					if (playerImagesCache.ContainsKey (player) == false) {

						Logger.I ("Loading player picture..." + player.PlayerID);

						// Ask Game center
						player.LoadPhoto (GKPhotoSize.Normal, (photo, error) => {
							if (error == null) {

								lock (playerImageCacheLock) {
									// TODO Lock foireux
									if(playerImagesCache.ContainsKey(player) == false) {
										playerImagesCache.Add (player, photo);
									}
								}

								if (callback != null) {
									callback (playerImagesCache [player]);
								}

							} else {
								Logger.E ("GameCenterHelper.GetProfileImage", error);
							}
						});
					} else {
						if (callback != null) {
							callback (playerImagesCache [player]);
						}
					}
				}
			} else {
				if (callback != null) {
					callback (playerImagesCache [player]);
				}
			}
		}

		public static VersusMatch ParseMatch (GKTurnBasedMatch match)
		{
			VersusMatch existingMatch = new VersusMatch ();

			// Match has data: it's not the first turn
			if (match.MatchData.Length > 0) {
				try {
					string jsonBase64 = NSString.FromData (match.MatchData, NSStringEncoding.UTF8);
					string json = System.Text.Encoding.UTF8.GetString (Convert.FromBase64String (jsonBase64));
					existingMatch.FromJson (json.ToString ());
				} catch (Exception e) {
					Logger.E ("GameCenterPlayer.FoundMatch", e);
				}
			}
			// No data: new match, 
			else {
				existingMatch.MatchId = match.MatchID;
				existingMatch.Player1Id = match.Participants [0].PlayerID;
				existingMatch.Player2Id = match.Participants [1].PlayerID;

				// Set up outcomes
				match.Participants [0].MatchOutcome = GKTurnBasedMatchOutcome.First;
				match.Participants [1].MatchOutcome = GKTurnBasedMatchOutcome.Second;

				// TODO Select filter
				existingMatch.Filter = new Filter ("0", "MP Filter", "defaultIcon", // Test data
				                                   1990, 2050, null, new System.Collections.Generic.List<string> () {
					"combat"
				}, null);
			}

			return existingMatch;
		}
	}
}

