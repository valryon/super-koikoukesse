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
			Logger.Log (LogLevel.Info, "Loading database... " + m_location);
			m_db = new SQLiteConnection (location);

			// Try to figure if we're in a first launch
			int gamesCount = CountGames ();
			Exists = gamesCount > 0;

			Logger.Log (LogLevel.Debug, gamesCount + " games in database.");

			if (Exists) {
				Logger.Log (LogLevel.Info, "Database loaded.");
			} else {
				Logger.Log (LogLevel.Warning, "Database not found.");
			}
		}

		/// <summary>
		/// Initialize table structure
		/// </summary>
		public void CreateTables ()
		{
			lock (locker) {
				Logger.Log (LogLevel.Info, "Creating database schema");

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
			Logger.Log (LogLevel.Info, "Initializing database from xml...");
			
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

				GameInfo game = new GameInfo ();

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

			Logger.Log (LogLevel.Info, "Initialization completed, " + addCount + " games added!");
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
				Logger.Log (LogLevel.Info, "Deleting game id " + gameId);

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

