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
		/// <param name="zone"></param>
		/// <param name="transformation"> </param>
		public string GetGameTitle (int answerIndex, GameZones zone, TextTransformations transformation)
		{
			GameInfo game = Answers [answerIndex];
			string title = "";

			if (zone == GameZones.PAL) {
				title = Answers [answerIndex].TitlePAL;
			} else if (zone == GameZones.NTSC) {
				title = Answers [answerIndex].TitleUS;
			}

			if (transformation != TextTransformations.None) {

				string[] words = title.Split (' ');
				string maskedTitle = "";

				foreach (string w in words) {

					for (int i=0; i<w.Length; i++) {
						if (i == 0 && transformation == TextTransformations.FirstLetterOnly) {
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
	}
}

