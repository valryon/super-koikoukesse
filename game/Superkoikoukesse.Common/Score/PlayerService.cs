using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Generic player service for score, multiplayer, etc
	/// </summary>
	public abstract class PlayerService
	{
		public PlayerService ()
		{

		}

		/// <summary>
		/// Authenticate the player
		/// </summary>
		public abstract bool Authenticate();

		/// <summary>
		/// Get the player nickname
		/// </summary>
		/// <value>The player identifier.</value>
		public abstract string PlayerId {
			get;
			set;
		}
	}
}

