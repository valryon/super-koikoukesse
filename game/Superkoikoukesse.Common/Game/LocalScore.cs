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

		public override bool Equals (object obj)
		{
			if(obj is LocalScore) {
				LocalScore s = obj as LocalScore;

				return s.Mode.Equals(Mode) && s.Difficulty.Equals(Difficulty) && s.Date.Equals(Date) && s.Score.Equals(Score);
			}
			return base.Equals (obj);
		}
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}

