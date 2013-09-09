// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using SQLite;

namespace Superkoikoukesse.Common
{
  /// <summary>
  /// Hack to have a parameterless string class
  /// </summary>
  internal class SQLiteString
  {
    public string Value { get; set; }
  }

	/// <summary>
	/// Database service.
	/// </summary>
	public class GameDatabase
	{
		#region Singleton 
		private static GameDatabase mInstance;

		private GameDatabase ()
		{
			mLocker = new object ();
		}

		/// <summary>
		/// Singleton
		/// </summary>
		/// <value>The instance.</value>
		public static GameDatabase Instance {
			get {
				if (mInstance == null) {
					mInstance = new GameDatabase ();
				}

				return mInstance;
			}
		}
		#endregion
		private string mLocation;
		private SQLiteConnection mDb;
		private object mLocker;
		#region Initialization
		/// <summary>
		/// Load the database
		/// </summary>
		/// <param name="location">Location.</param>
		public void Load (string location)
		{
			mLocation = location;

			// Initialize db connection
			Logger.I("Loading database... " + mLocation);
			mDb = new SQLiteConnection (location);

			// Try to figure if we're in a first launch
			int gamesCount = CountGames ();
			Exists = gamesCount > 0;

			Logger.D( gamesCount + " games in database.");

			if (Exists) {
				Logger.I("Database loaded.");
			} else {
				Logger.W( "Database not found.");
			}
		}

		/// <summary>
		/// Initialize table structure
		/// </summary>
		public void CreateTables ()
		{
			lock (mLocker) {
				Logger.I("Creating database schema");

				mDb.CreateTable<GameEntry> ();
				mDb.CreateTable<Player> ();
				mDb.CreateTable<LocalScore> ();
			}
		}

		/// <summary>
		/// Initialize the database using the specifiec XML
		/// </summary>
		/// <param name="xml">Xml.</param>
		public void InitializeFromXml (string xml)
		{
			Logger.I("Initializing database from xml...");
			
			// Parse the xml
			// Format:
			//			<questions>
			//				<question>
			//					<id type="integer">1</id>
			//					<gameId type="integer">1</gameId>
			//					<image>00001.jpg</image>
			//					<titlePAL>A-Train</titlePAL>
			//					<titleUS>A-Train</titleUS>
			//					<titleJAP/>
			//					<platform>Amiga</platform>
			//					<genre>gestion</genre>
			//					<publisher>Ocean</publisher>
			//					<year type="integer">1992</year>
			//					<excluded type="trueclass">false</excluded>
			//				</question>
			//			</questions>
			int addCount = 0;
			XElement element = XElement.Parse (xml);

			foreach (XElement gameXml in element.Elements("question")) {

				GameEntry game = new GameEntry ();

				game.GameId = Convert.ToInt32 (gameXml.Element ("gameId").Value);
				game.ImagePath = gameXml.Element ("image").Value;
				game.TitlePAL = gameXml.Element ("titlePAL").Value;
				game.TitleUS = gameXml.Element ("titleUS").Value;
				game.Platform = gameXml.Element ("platform").Value;
				game.Genre = gameXml.Element ("genre").Value;
				game.Publisher = gameXml.Element ("publisher").Value;
				game.Year = Convert.ToInt32 (gameXml.Element ("year").Value);
				bool isRemoved = Convert.ToBoolean (gameXml.Element ("excluded").Value);

				if (isRemoved == false) {
					addCount++;

					AddGame (game);
				}
			}

			Logger.I("Initialization completed, " + addCount + " games added!");
		}
		#endregion
		#region GameInfo storage
		/// <summary>
		/// Add a new game entry
		/// </summary>
		/// <param name="gameInfo">Game info.</param>
		public void AddGame (GameEntry gameInfo)
		{
			lock (mLocker) {
				mDb.Insert (gameInfo);
			}
		}

		/// <summary>
		/// Remove a game entry
		/// </summary>
		/// <param name="gameId">Game identifier.</param>
		public void RemoveGame (int gameId)
		{
			GameEntry game = mDb.Table<GameEntry> ().Where (g => g.GameId == gameId).FirstOrDefault ();

			if (game != null) {
				Logger.I("Deleting game id " + gameId);

				lock (mLocker) {
					mDb.Delete<GameEntry> (game.GameId);
				}
			}
		}

		/// <summary>
		/// Read games database and get matching ids
		/// </summary>
		public List<GameEntry> ReadGames (int minYear, int maxYear, List<string> publishers, List<string> genres, List<string> platforms)
		{
			lock (mLocker) {
				var query = mDb.Table<GameEntry> ()
				.Where (g => (g.Year >= minYear) && (g.Year < maxYear));
			
				if (publishers != null && publishers.Count > 0) {
					query = query.Where (g => publishers.Contains (g.Publisher));
				}

				if (genres != null && genres.Count > 0) {
					query = query.Where (g => genres.Contains (g.Genre));
				}

				if (platforms != null && platforms.Count > 0) {
					query = query.Where (g => platforms.Contains (g.Platform));
				}

				return query.ToList ();
			}
		}

		/// <summary>
		/// Read games database and get matching ids
		/// </summary>
		public GameEntry ReadGame (int gameId)
		{
			lock (mLocker) {
				return mDb.Table<GameEntry> ().Where (g => g.GameId == gameId).FirstOrDefault ();
			}
		}

		/// <summary>
		/// Get all publishers
		/// </summary>
		public List<string> GetPublishers ()
		{
			List<string> publishers = new List<string> ();
			lock (mLocker) {
				foreach (GameEntry game in mDb.Table<GameEntry> ()) {
					publishers.Add (game.Publisher);
				}
			}

			return publishers;
		}

		/// <summary>
		/// Returns the current number of games in database
		/// </summary>
		/// <returns>The games.</returns>
		public int CountGames ()
		{
			lock (mLocker) {
				var gameTable = mDb.Table<GameEntry> ();

				try {
					return gameTable.Count ();
				} catch (Exception) {
					return 0;
				}
			}
		}

    /// <summary>
    /// Get the oldest game date (year)
    /// </summary>
    /// <returns>The minimum year.</returns>
    public int GetMinYear() {
      var gameTable = mDb.Table<GameEntry> ();

      GameEntry pouet = gameTable.OrderBy(g => g.Year).First();
      return pouet.Year;
    }

    /// <summary>
    /// Get the newest game date (year)
    /// </summary>
    /// <returns>The minimum year.</returns>
    public int GetMaxYear() {
      var gameTable = mDb.Table<GameEntry> ();

      GameEntry pouet = gameTable.OrderByDescending(g => g.Year).First();
      return pouet.Year;
    }

    /// <summary>
    /// Get all game genres
    /// </summary>
    /// <returns>The genres.</returns>
    public List<string> GetGenres() {

      List<string> result = new List<string>();

      List<SQLiteString> data = mDb.Query<SQLiteString>("SELECT Genre as 'Value' FROM GameEntry");
      foreach(var entry in data.GroupBy(g => g.Value).Select(grp => grp.First()))
      {
        result.Add(entry.Value);
      }

      return result;
    }

    /// <summary>
    /// Get all game platforms
    /// </summary>
    /// <returns>The genres.</returns>
    public List<string> GetPlatforms() {
      
      List<string> result = new List<string>();

      List<SQLiteString> data = mDb.Query<SQLiteString>("SELECT Platform as 'Value' FROM GameEntry");
      foreach(var entry in data.GroupBy(g => g.Value).Select(grp => grp.First()))
      {
        result.Add(entry.Value);
      }

      return result;
    }

		#endregion
		#region Player
		/// <summary>
		/// Get the stored player information
		/// </summary>
		/// <returns>The player.</returns>
		public Player ReadPlayer ()
		{
			// Find the first player stored instance
			lock (mLocker) {
				var playerTable = mDb.Table<Player> ();

				return playerTable.FirstOrDefault ();
			}
		}

		/// <summary>
		/// Saves the player information
		/// </summary>
		/// <param name="player">Player.</param>
		public void SavePlayer (Player player)
		{
			lock (mLocker) {
				var playerTable = mDb.Table<Player> ();

				// Clean to have only one element
				if (playerTable.Count () > 0) {
					mDb.DeleteAll<Player> ();
				}

				mDb.Insert (player);
			}
		}
		#endregion
		#region Scores
		/// <summary>
		/// Add a new score to the local DB
		/// </summary>
		/// <param name="score">Score.</param>
		public int AddLocalScore (LocalScore score)
		{
			lock (mLocker) {
				mDb.Insert (score);
			}
			// Rank?
			List<LocalScore> modeScore = mDb.Table<LocalScore> ().Where (s => (s.Mode == score.Mode) && (s.Difficulty == score.Difficulty))
				.OrderByDescending (s => s.Score).Take (999).ToList ();

			int rank = modeScore.IndexOf (score);
	
			if (rank >= 0) {
				return rank;
			}

			return 999;

		}

		/// <summary>
		/// Get scores from a leaderboard
		/// </summary>
		/// <returns>The local scores.</returns>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulties">Difficulties.</param>
		/// <param name="count">Count.</param>
		public LocalScore[] GetLocalScores (GameMode mode, GameDifficulty difficulty, int count)
		{
			lock (mLocker) {
				return mDb.Table<LocalScore> ()
				.Where (s => (s.Mode == mode) && (s.Difficulty == difficulty))
				.OrderByDescending (s => s.Score)
				.Take (count).ToArray ();
			}
		}
		#endregion
		/// <summary>
		/// Gets a value indicating whether the game database exists.
		/// </summary>
		/// <value><c>true</c> if database exists; otherwise, <c>false</c>.</value>
		public bool Exists { get; private set; }
	}
}

