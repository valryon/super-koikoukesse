using System;
using System.Linq;
using System.Collections.Generic;

namespace Superkoikoukesse.Common
{
	public class Quizz
	{
		/// <summary>
		/// List of questions
		/// </summary>
		/// <value>The questions.</value>
		public List<Question> Questions { get; private set; }

		/// <summary>
		/// Time left for this game (seconds)
		/// </summary>
		/// <value>The time left.</value>
		public float TimeLeft { get; private set; }

		public Quizz ()
		{
		}

		public void Initialize ()
		{
			Logger.Log (LogLevel.Info, "Initializing quizz...");

			Questions = new List<Question> ();
			TimeLeft = 120f;

			int questionCount = 4;
			int answerCount = 4;

			// Fill questions
			for (int i=0; i<questionCount; i++) {
				int count = 0;

				Question q = new Question();

				while (count<answerCount) {
				
					GameInfo game = DatabaseService.Instance.RandomGame ();

					if(q.Answers.Contains(game) == false) {
						q.Answers.Add(game);
						count++;
					}

					// Decide that the first will be the correct answer
					if(q.CorrectAnswer == null) {
						q.CorrectAnswer = game;
					}
				}

				Questions.Add (q);
			}

			// Randomize questions
			Questions = Questions.OrderBy (q => Guid.NewGuid()).ToList ();

			Logger.Log (LogLevel.Info, "Quizz ready: "+Questions.Count+" questions!");
		}
	}
}

