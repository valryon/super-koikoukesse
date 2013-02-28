using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Create and initialize database
	/// </summary>
	public class DBHelper
	{
		private string m_location;

		public DBHelper (string dbLocation)
		{
			m_location = dbLocation;

			loadDatabase ();
		}

		/// <summary>
		/// Initialize the database using the specifiec XML
		/// </summary>
		/// <param name="xml">Xml.</param>
		public void Initialize(string xml) {

			Logger.Log (LogLevel.Info, "Initializing database from xml...");


			Logger.Log (LogLevel.Info, "Initialization completed!");

		}

		/// <summary>
		/// Loads the database if possible
		/// </summary>
		private void loadDatabase() {

			Logger.Log(LogLevel.Info,"Loading database... "+m_location);
			Exists = false;

			if (Exists) {
				Logger.Log (LogLevel.Info, "Database loaded.");
			} else {
				Logger.Log (LogLevel.Warning, "Database not found.");
			}
		}

		/// <summary>
		/// Gets a value indicating whether the game database exists.
		/// </summary>
		/// <value><c>true</c> if database exists; otherwise, <c>false</c>.</value>
		public bool Exists { get; private set; }
	}
}

