// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Collections.Generic;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Generic player service for score, multiplayer, etc
	/// </summary>
	public abstract class AuthenticatedPlayer
	{
		private string mPlayerId, mDisplayName;
	
		public AuthenticatedPlayer ()
		{
			mPlayerId = Guid.NewGuid ().ToString ();
			mDisplayName = "Local";
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
		public abstract void AddScore (GameMode mode, GameDifficulties difficulty, int score);

		/// <summary>
		/// Get the best score and its rank from leaderboard
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		/// <param name="gcRankCallback">Best rank and score.</param>
		public abstract void GetBestScoreAndRank (GameMode mode, GameDifficulties difficulty, Action<int,int> gcRankCallback);

		/// <summary>
		/// Lists the versus matchs.
		/// </summary>
		/// <param name="matchsCallback">Matchs callback.</param>
		/// <param name="errorCallback">Error callback.</param>
		public abstract void ListMatchs (Action<List<VersusMatch>> matchsCallback, Action errorCallback);

		/// <summary>
		/// Create a new turn based match
		/// </summary>
		/// <param name="callback">Callback.</param>
		public abstract void NewMatch(Action<VersusMatch> matchFoundCallback, Action cancelCallback, Action errorCallback, Action playerQuitCallback);

    /// <summary>
    /// Register match data
    /// </summary>
    /// <param name="match">Match.</param>
    public abstract void SetMatch(VersusMatch match);

		/// <summary>
		/// Ends the current match's turn.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public abstract void EndMatchTurn(int score, Action callback);

		/// <summary>
		/// Resigns the current match.
		/// </summary>
		/// <param name="callback">Callback.</param>
		public abstract void QuitMatch(Action callback);

		/// <summary>
		/// Get the leaderbord name
		/// </summary>
		/// <returns>The leaderboard identifier.</returns>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		public virtual string GetLeaderboardId (GameMode mode, GameDifficulties difficulty)
		{
			return (mode.ToString () + difficulty.ToString ()).ToLower ();
		}

		/// <summary>
		/// Get the player nickname
		/// </summary>
		/// <value>The player identifier.</value>
		public virtual string PlayerId {
			get {
				return mPlayerId;
			}
		}

		/// <summary>
		/// Get the player nickname
		/// </summary>
		/// <value>The player identifier.</value>
		public virtual string DisplayName {
			get {
				return mDisplayName;
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

		/// <summary>
		/// Current versus match
		/// </summary>
		/// <value>The current match.</value>
		public VersusMatch CurrentMatch 
		{
			get; 
      protected set;
		}
	}
}

