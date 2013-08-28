// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
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

		public override bool Equals (object obj)
		{
			if(obj is LocalScore) {
				LocalScore s = obj as LocalScore;

				return s.Mode.Equals(Mode) && s.Difficulty.Equals(Difficulty) && s.Score.Equals(Score)
					&& s.Date.Year == Date.Year  // Careful: SqlLite is not precise on date
						&& s.Date.Month == Date.Month
						&& s.Date.Day == Date.Day
						&& s.Date.Hour == Date.Hour
						&& s.Date.Minute == Date.Minute
						&& s.Date.Second == Date.Second
						;
			}
			return base.Equals (obj);
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

		
		public GameMode Mode {get;set;}

		public GameDifficulties Difficulty {get;set;}

		public int Score {get;set;}

		public DateTime Date {get;set;}
	}
}

