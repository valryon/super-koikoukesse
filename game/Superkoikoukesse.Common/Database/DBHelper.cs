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
		}
	}
}

