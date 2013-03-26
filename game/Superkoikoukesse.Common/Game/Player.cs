using System;
using System.Json;

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
		}

		public void BuildFromJsonObject (JsonValue json)
		{

		}

	}
}

