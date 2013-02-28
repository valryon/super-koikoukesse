using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Game entry in the database
	/// </summary>
	public class GameInfo
	{
		/// <summary>
		/// Id in the server database
		/// </summary>
		/// <value>The game identifier.</value>
		public int GameId { get; set; }

		/// <summary>
		/// European title
		/// </summary>
		public string TitlePAL { get; set; }

		/// <summary>
		/// American title
		/// </summary>
		public string TitleUS { get; set; }



		public GameInfo ()
		{
		}
	}
}

