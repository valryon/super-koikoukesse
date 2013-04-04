using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Generic player service for score, multiplayer, etc
	/// </summary>
	public abstract class AuthenticatedPlayer
	{
		private string playerId, displayName;
	
		public AuthenticatedPlayer ()
		{
			playerId = Guid.NewGuid ().ToString ();
			displayName = "Local";
		}

		/// <summary>
		/// Authenticate the player
		/// </summary>
		public abstract void Authenticate (Action authenticationFinished);

		/// <summary>
		/// Send a score to a leaderboard
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		/// <param name="score">Score.</param>
		public abstract void AddScore (GameModes mode, GameDifficulties difficulty, int score);

		/// <summary>
		/// Get the best score and its rank from leaderboard
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		/// <param name="gcRankCallback">Best rank and score.</param>
		public abstract void GetBestScoreAndRank (GameModes mode, GameDifficulties difficulty, Action<int,int> gcRankCallback);

		/// <summary>
		/// Get the leaderbord name
		/// </summary>
		/// <returns>The leaderboard identifier.</returns>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		public virtual string GetLeaderboardId (GameModes mode, GameDifficulties difficulty)
		{
			return (mode.ToString () + difficulty.ToString ()).ToLower ();
		}

		/// <summary>
		/// Get the player nickname
		/// </summary>
		/// <value>The player identifier.</value>
		public virtual string PlayerId {
			get {
				return playerId;
			}
		}

		/// <summary>
		/// Get the player nickname
		/// </summary>
		/// <value>The player identifier.</value>
		public virtual string DisplayName {
			get {
				return displayName;
			}
		}

		/// <summary>
		/// Is the player authenticated to the service
		/// </summary>
		/// <value><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</value>
		public virtual bool IsAuthenticated {
			get {
				return false;
			}
		}
	}
}

