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
		/// Subscription type (TODO)
		/// </summary>
		/// <value>The type of the subscription.</value>
		public int SubscriptionType { get; set; }

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

		public Player ()
		{
			Credits = Constants.BaseCredits;
			Coins = Constants.BaseCoins;
		}

		public Player (AuthenticatedPlayer aplayer)
			: this()
		{
			DisplayName = aplayer.DisplayName;

			// Clean ID from URL reserved chars
			Id = aplayer.PlayerId.Replace (":", "").Replace ("&", "").Replace ("/", "").Replace (" ", "");
		}

		public void BuildFromJsonObject (JsonValue json)
		{
			//json	{{"Id": "10550e72-da74-4b07-ac6d-a18e02712ec4", "GameCenterId": "G1728633519", "NickName": "G1728633519", "CreationDate": "2013-03-27T11:22:51.96Z", "Credits": 2500, "Coins": 3, "SubscriptionType": 0}}	System.Json.JsonObject
			string playerId = json ["GameCenterId"].ToString ();
			int credits = Convert.ToInt32 (json ["Credits"].ToString ());
			int coins = Convert.ToInt32 (json ["Coins"].ToString ());
			int subscriptionType = Convert.ToInt32 (json ["SubscriptionType"].ToString ());

			this.Id = playerId;
			this.Credits = credits;
			this.Coins = coins;
			this.SubscriptionType = subscriptionType;
		}

	}
}

