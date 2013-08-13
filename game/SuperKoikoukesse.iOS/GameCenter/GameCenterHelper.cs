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
		/// <summary>
		/// Gets (and download and cache if necessary) the player
		/// </summary>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="callback">Callback.</param>
		public static void GetPlayers (string[] playerIds, Action<GKPlayer[]> callback)
		{
			// Ask Game center
			GKPlayer.LoadPlayersForIdentifiers (playerIds, (players, error) => {
				if (error == null) {
					if (callback != null) {
						callback (players);
					}
				} else {
					Logger.E ("GameCenterHelper.GetPlayer", error);
				}
			});
		}

		/// <summary>
		/// Gets (and download and cache if necessary) the profile image for a player
		/// </summary>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="callback">Callback.</param>
		public static void GetProfileImage (GKPlayer player, Action<UIImage> callback)
		{
			Logger.I ("Loading player picture..." + player.PlayerID);

			// Ask Game center
			player.LoadPhoto (GKPhotoSize.Normal, (photo, error) => {
				if (error == null) {
					if (callback != null) {
						callback (photo);
					}
				} else {
					Logger.E ("GameCenterHelper.GetProfileImage", error);
				}
			});
		}

		public static VersusMatch ParseMatch (GKTurnBasedMatch match)
		{
			VersusMatch existingMatch = new VersusMatch ();

			if (match.MatchData == null) {
				return null;
			}

			// Match has data: it's not the first turn
			if (match.MatchData.Length > 0) {
				try {
					string jsonBase64 = NSString.FromData (match.MatchData, NSStringEncoding.UTF8);
					string json = System.Text.Encoding.UTF8.GetString (Convert.FromBase64String (jsonBase64));
					existingMatch.FromJson (json.ToString ());
				} catch (Exception e) {
					Logger.E ("GameCenterHelper.ParseMatch", e);
					return null;
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

		public static void KillMatch(GKTurnBasedMatch match) {

			Logger.W ("Removing match..." + match.MatchID + " " + match.Status);

			match.Participants [0].MatchOutcome = GKTurnBasedMatchOutcome.Won;
			match.Participants [1].MatchOutcome = GKTurnBasedMatchOutcome.Lost;

			NSData d;
			if (match.MatchData == null) {
				d = NSData.FromString ("Wabon");
			} else {
				d = match.MatchData;
			}

			match.EndMatchInTurn (d, (e1) => {

				if (e1 != null) {
					Logger.E ("GameCenterHelper.KillMatch - EndMatchInTurn", e1);
				}

				match.Remove (new GKNotificationHandler ((e2) => {
					if (e2 != null) {
						Logger.E ("GameCenterHelper.KillMatch - Remove", e2);
					}

				}));
			});
		}
	}
}

