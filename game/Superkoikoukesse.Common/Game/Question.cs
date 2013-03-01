using System;
using System.Collections.Generic;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Question data
	/// </summary>
	public class Question
	{
		public List<GameInfo> Answers { get; set; }

		public GameInfo CorrectAnswer { get; set; }

		public int ScoreBonus { get; set; }

		public int TimeBonus { get; set; }

		public Question ()
		{
			Answers = new List<GameInfo> ();
		}
	}
}

