using System;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;
using SQLite;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Database service.
	/// </summary>
	public class DatabaseService
	{
		#region Singleton 

		private static DatabaseService m_instance;

		private DatabaseService ()
		{
			locker = new object ();
		}

		/// <summary>
		/// Singleton
		/// </summary>
		/// <value>The instance.</value>
		public static DatabaseService Instance {
			get {
				if (m_instance == null) {
					m_instance = new DatabaseService ();
				}

				return m_instance;
			}
		}

		#endregion

		private string m_location;
		private SQLiteConnection m_db;
		private object locker;

		#region Initialization

		/// <summary>
		/// Load the database
		/// </summary>
		/// <param name="location">Location.</param>
		public void Load (string location)
		{
			m_location = location;

			// Initialize db connection
			Logger.I("Loading database... " + m_location);
			m_db = new SQLiteConnection (location);

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
			lock (locker) {
				Logger.I("Creating database schema");

				m_db.CreateTable<GameInfo> ();
				m_db.CreateTable<Player> ();
				m_db.CreateTable<LocalScore> ();
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
			//			<games>
			//				<game>
			//					<GameId>1</GameId>
			//					<ImagePath>00001.jpg</ImagePath>
			//					<TitlePAL>A-Train</TitlePAL>
			//					<TitleUS>A-Train</TitleUS>
			//					<Platform>amiga</Platform>
			//					<Genre>gestion</Genre>
			//					<Publisher>ocean</Publisher>
			//					<Year>1992</Year>
			//					<IsRemoved>true</IsRemoved>
			//				</game>
			//			</games>
			int addCount = 0;
			XElement element = XElement.Parse (xml);

			foreach (XElement gameXml in element.Elements("game")) {

				GameInfo game = new GameInfo ();

				game.GameId = Convert.ToInt32 (gameXml.Element ("GameId").Value);
				game.ImagePath = gameXml.Element ("ImagePath").Value;
				game.TitlePAL = gameXml.Element ("TitlePAL").Value;
				game.TitleUS = gameXml.Element ("TitleUS").Value;
				game.Platform = gameXml.Element ("Platform").Value;
				game.Genre = gameXml.Element ("Genre").Value;
				game.Publisher = gameXml.Element ("Publisher").Value;
				game.Year = Convert.ToInt32 (gameXml.Element ("Year").Value);
				bool isRemoved = Convert.ToBoolean (gameXml.Element ("IsRemoved").Value);

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
		public void AddGame (GameInfo gameInfo)
		{
			lock (locker) {
				m_db.Insert (gameInfo);
			}
		}

		/// <summary>
		/// Remove a game entry
		/// </summary>
		/// <param name="gameId">Game identifier.</param>
		public void RemoveGame (int gameId)
		{
			GameInfo game = m_db.Table<GameInfo> ().Where (g => g.GameId == gameId).FirstOrDefault ();

			if (game != null) {
				Logger.I("Deleting game id " + gameId);

				lock (locker) {
					m_db.Delete<GameInfo> (game.GameId);
				}
			}
		}

		/// <summary>
		/// Read games database and get matching ids
		/// </summary>
		public List<GameInfo> ReadGames (int minYear, int maxYear, List<string> publishers, List<string> genres, List<string> platforms)
		{
			lock (locker) {
				var query = m_db.Table<GameInfo> ()
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
		public GameInfo ReadGame (int gameId)
		{
			lock (locker) {
				return m_db.Table<GameInfo> ().Where (g => g.GameId == gameId).FirstOrDefault ();
			}
		}

		/// <summary>
		/// Get all publishers
		/// </summary>
		public List<string> GetPublishers ()
		{
			List<string> publishers = new List<string> ();
			lock (locker) {
				foreach (GameInfo game in m_db.Table<GameInfo> ()) {
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
			lock (locker) {
				var gameTable = m_db.Table<GameInfo> ();

				try {
					return gameTable.Count ();
				} catch (Exception) {
					return 0;
				}
			}
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
			lock (locker) {
				var playerTable = m_db.Table<Player> ();

				return playerTable.FirstOrDefault ();
			}
		}

		/// <summary>
		/// Saves the player information
		/// </summary>
		/// <param name="player">Player.</param>
		public void SavePlayer (Player player)
		{
			lock (locker) {
				var playerTable = m_db.Table<Player> ();

				// Clean to have only one element
				if (playerTable.Count () > 0) {
					m_db.DeleteAll<Player> ();
				}

				m_db.Insert (player);
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
			lock (locker) {
				m_db.Insert (score);
			}
			// Rank?
			List<LocalScore> modeScore = m_db.Table<LocalScore> ().Where (s => (s.Mode == score.Mode) && (s.Difficulty == score.Difficulty))
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
		public LocalScore[] GetLocalScores (GameModes mode, GameDifficulties difficulty, int count)
		{
			lock (locker) {
				return m_db.Table<LocalScore> ()
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

