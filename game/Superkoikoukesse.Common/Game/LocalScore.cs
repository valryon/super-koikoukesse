using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Local leaderboard stored in database
	/// </summary>
	[Serializable]
	public class LocalScore
	{
		public LocalScore ()
		{
		}

		public GameModes Mode {get;set;}

		public GameDifficulties Difficulty {get;set;}

		public int Score {get;set;}

		public DateTime Date {get;set;}
	}
}

