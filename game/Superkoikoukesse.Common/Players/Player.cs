using System;
using System.Json;
using System.Web;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Player information (id, credits, coins)
	/// </summary>
	public class Player : IServiceOutput
	{
		/// <summary>
		/// Cleans the identifier.
		/// </summary>
		/// <returns>The identifier.</returns>
		/// <param name="playerId">Player identifier.</param>
		public static string CleanId(string playerId) {
			return playerId.Replace (":", "").Replace ("&", "").Replace ("/", "").Replace (" ", "");
		}

		public Player ()
		{
			Credits = Constants.BASE_CREDITS;
			Coins = Constants.BASE_COINS;
		}

		public Player (AuthenticatedPlayer aplayer)
			: this()
		{
			DisplayName = aplayer.DisplayName;

			// Clean ID from URL reserved chars
			Id = CleanId(aplayer.PlayerId);
		}

		public void BuildFromJsonObject (JsonValue json)
		{
			//json  {"id":1,"playerId":"G1725278793","creation_date":"2013-07-11T11:16:59+02:00","credits":13,"coins":7500,"platform":"ios"}
			string playerId = json ["playerId"].ToString ();
			int credits = Convert.ToInt32 (json ["credits"].ToString ());
			int coins = Convert.ToInt32 (json ["coins"].ToString ());

			this.Id = playerId;
			this.Credits = credits;
			this.Coins = coins;
		}

		/// <summary>
		/// Unique identifier for the player (game center id)
		/// </summary>
		/// <value>The identifier.</value>
		public string Id { get; set; }

		/// <summary>
		/// Player name to use
		/// </summary>
		/// <value>The display name.</value>
		public string DisplayName { get; set; }

		/// <summary>
		/// Credits left
		/// </summary>
		/// <value>The credits.</value>
		public int Credits { get; set; }

		/// <summary>
		/// Earned coins
		/// </summary>
		/// <value>The coins.</value>
		public long Coins { get; set; }

		/// <summary>
		/// Last time player credits has been upgraded
		/// </summary>
		/// <value>The last credits update.</value>
		public DateTime LastCreditsUpdate { get; set; }

		/// <summary>
		/// Credits used while disconnected
		/// </summary>
		/// <value>The disconnected credits used.</value>
		public int DisconnectedCreditsUsed  { get; set; }

		/// <summary>
		/// Coins earned while disconnected
		/// </summary>
		/// <value>The disconnected coins earned.</value>
		public int DisconnectedCoinsEarned  { get; set; }
	}
}

