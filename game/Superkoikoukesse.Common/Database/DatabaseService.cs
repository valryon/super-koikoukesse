using System;

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
		}

		/// <summary>
		/// Singleton
		/// </summary>
		/// <value>The instance.</value>
		public static DatabaseService Instance {
			get {
				if(m_instance == null) {
					m_instance = new DatabaseService();
				}

				return m_instance;
			}
		}

		#endregion

		private string m_location;

		/// <summary>
		/// Load the database
		/// </summary>
		/// <param name="location">Location.</param>
		public void Load(string location) {
			m_location = location;

			Logger.Log(LogLevel.Info,"Loading database... "+m_location);
			Exists = false;
			
			if (Exists) {
				Logger.Log (LogLevel.Info, "Database loaded.");
			} else {
				Logger.Log (LogLevel.Warning, "Database not found.");
			}
		}

		/// <summary>
		/// Initialize the database using the specifiec XML
		/// </summary>
		/// <param name="xml">Xml.</param>
		public void InitializeFromXml(string xml) {
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
			
			Logger.Log (LogLevel.Info, "Initialization completed!");
		}

		/// <summary>
		/// Gets a value indicating whether the game database exists.
		/// </summary>
		/// <value><c>true</c> if database exists; otherwise, <c>false</c>.</value>
		public bool Exists { get; private set; }
	}
}

