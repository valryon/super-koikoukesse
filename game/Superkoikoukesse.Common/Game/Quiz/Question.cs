// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Linq;
using System.Collections.Generic;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Question data
	/// </summary>
	public class Question
	{
		public Question ()
		{
			Answers = new List<GameEntry> ();
		}

		/// <summary>
		/// Get the game title for a specified position
		/// </summary>
		/// <returns>The game title.</returns>
		/// <param name="i">The index.</param>
		/// <param name="zone"></param>
		/// <param name="transformation"> </param>
		public string GetGameTitle (int answerIndex, GameZone zone, TextTransformations transformation)
		{
			GameEntry game = Answers [answerIndex];
			string title = "";

			if (zone == GameZone.PAL) {
				title = Answers [answerIndex].TitlePAL;
			} else if (zone == GameZone.NTSC) {
				title = Answers [answerIndex].TitleUS;
			}

			if (transformation != TextTransformations.NONE) {

				string[] words = title.Split (' ');
				string maskedTitle = "";

				foreach (string w in words) {

					for (int i=0; i<w.Length; i++) {
						if (i == 0 && transformation == TextTransformations.FIRST_LETTER_ONLY) {
							maskedTitle += w [i];
						} else {
							maskedTitle += "_";
						}
					}

					maskedTitle += " ";
				}

				title = maskedTitle.Substring(0, maskedTitle.Length-1);
			} 

			if (game == CorrectAnswer && Constants.DEBUG_MODE) {
				title += "*";
			}

			return title/*.ToUpper ()*/;
		}

		/// <summary>
		/// Test if the answer index is the good one
		/// </summary>
		public bool IsValidAnswer (int index)
		{
			// Index can be -1 if no answer selected
			if (index < 0 || index >= Answers.Count)
				return false;

			return (Answers [index] == CorrectAnswer);
		}

		/// <summary>
		/// Randomizer answers order
		/// </summary>
		public void ShuffleAnswers ()
		{
			Answers = Answers.OrderBy (q => Guid.NewGuid ()).ToList ();
		}

		public override string ToString ()
		{
			return CorrectAnswer.ToString ();
		}

		/// <summary>
		/// Possible answer for this question
		/// </summary>
		/// <value>The answers.</value>
		public List<GameEntry> Answers { get; set; }

		/// <summary>
		/// The correct question answer.
		/// </summary>
		/// <value>The correct answer.</value>
		public GameEntry CorrectAnswer { get; set; }

		/// <summary>
		/// Score won for this question
		/// </summary>
		/// <value>The score bonus.</value>
		public int ScoreBonus { get; set; }

		/// <summary>
		/// Time won for this question
		/// </summary>
		/// <value>The time bonus.</value>
		public int TimeBonus { get; set; }
	}
}

