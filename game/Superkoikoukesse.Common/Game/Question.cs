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

		/// <summary>
		/// Get the game title for a specified position
		/// </summary>
		/// <returns>The game title.</returns>
		/// <param name="i">The index.</param>
		public string GetGameTitle (int i)
		{
			// TODO Handle PAL & US
			GameInfo game = Answers [i];

			if (game == CorrectAnswer) {
				return Answers[i].TitlePAL+"*";
			}

			return Answers [i].TitlePAL;
		}

		public override string ToString ()
		{
			return CorrectAnswer.ToString();
		}
	}
}

