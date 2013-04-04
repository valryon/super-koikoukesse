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
			m_random = new Random (DateTime.Now.Millisecond);
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
		private Random m_random;

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
		private void createTables ()
		{

			Logger.Log (LogLevel.Info, "Creating database schema");

			m_db.CreateTable<GameInfo> ();
			m_db.CreateTable<Player> ();
			m_db.CreateTable<LocalScore>();
		}

		/// <summary>
		/// Initialize the database using the specifiec XML
		/// </summary>
		/// <param name="xml">Xml.</param>
		public void InitializeFromXml (string xml)
		{

			createTables ();

			Logger.Log (LogLevel.Info, "Initializing database from xml...");
			
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
			m_db.Insert (gameInfo);
		}

		/// <summary>
		/// Remove a game entry
		/// </summary>
		/// <param name="gameId">Game identifier.</param>
		public void RemoveGame (GameInfo gameInfo)
		{
			m_db.Delete (gameInfo);
		}

		/// <summary>
		/// Read games database
		/// </summary>
		public List<GameInfo> ReadGames ()
		{

			return m_db.Table<GameInfo> ().ToList ();
		}

		/// <summary>
		/// Get a random game
		/// </summary>
		/// <returns>The game.</returns>
		public GameInfo RandomGame ()
		{

			int randomIndex = m_random.Next (0, CountGames ());

			return m_db.Table<GameInfo> ().ElementAt (randomIndex);
		}

		/// <summary>
		/// Returns the current number of games in database
		/// </summary>
		/// <returns>The games.</returns>
		public int CountGames ()
		{
			var gameTable = m_db.Table<GameInfo> ();

			try {
				return gameTable.Count ();
			} catch (Exception) {
				return 0;
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
			var playerTable = m_db.Table<Player> ();

			return playerTable.FirstOrDefault ();
		}

		/// <summary>
		/// Saves the player information
		/// </summary>
		/// <param name="player">Player.</param>
		public void SavePlayer (Player player)
		{
			var playerTable = m_db.Table<Player> ();

			// Clean to have only one element
			if (playerTable.Count() > 0) {
				m_db.DeleteAll<Player> ();
			}

			m_db.Insert (player);
		}

		#endregion

		#region Scores

		/// <summary>
		/// Add a new score to the local DB
		/// </summary>
		/// <param name="score">Score.</param>
		public void AddLocalScore(LocalScore score) {
			m_db.Insert(score);
		}

		/// <summary>
		/// Get scores from a leaderboard
		/// </summary>
		/// <returns>The local scores.</returns>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulties">Difficulties.</param>
		/// <param name="count">Count.</param>
		public LocalScore[] GetLocalScores(GameModes mode, GameDifficulties difficulty, int count) {

			return m_db.Table<LocalScore>()
				.Where(s => (s.Mode == mode) && (s.Difficulty == difficulty))
				.OrderByDescending(s => s.Score)
				.Take (count).ToArray();
		}

		#endregion

		/// <summary>
		/// Gets a value indicating whether the game database exists.
		/// </summary>
		/// <value><c>true</c> if database exists; otherwise, <c>false</c>.</value>
		public bool Exists { get; private set; }
	}
}

