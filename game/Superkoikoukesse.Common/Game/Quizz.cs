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
		/// Current question. 
		/// </summary>
		/// <value>The current question.</value>
		public Question CurrentQuestion{ get; private set; }

		/// <summary>
		/// Gets a value indicating whether this quizz is over.
		/// </summary>
		/// <value><c>true</c> if this instance is over; otherwise, <c>false</c>.</value>
		public bool IsOver { get; private set; }

		/// <summary>
		/// Time left for this game (seconds)
		/// </summary>
		/// <value>The time left.</value>
		public float TimeLeft { get; private set; }

		private int m_questionIndex;

		public Quizz ()
		{
		}

		public void Initialize ()
		{
			Logger.Log (LogLevel.Info, "Initializing quizz...");

			Questions = new List<Question> ();
			List<int> correctAnswerIds = new List<int> ();

			TimeLeft = 120f;

			int questionCount = 4;
			int answerCount = 4;

			// Fill questions
			for (int i=0; i<questionCount; i++) {
				int count = 0;

				Question q = new Question();

				while (count<answerCount) {
				
					GameInfo game = DatabaseService.Instance.RandomGame ();

					if(q.Answers.Contains(game) == false && correctAnswerIds.Contains(game.GameId) == false) {
						q.Answers.Add(game);
						count++;
					
						// Decide that the first will be the correct answer
						if(q.CorrectAnswer == null) {
							q.CorrectAnswer = game;
							correctAnswerIds.Add(game.GameId);
						}
					}
				}

				// Randomize answers
				q.ShuffleAnswers();

				Questions.Add (q);
			}

			// Get the first
			m_questionIndex = 0;
			CurrentQuestion = Questions[m_questionIndex];

			IsOver = false;

			Logger.Log (LogLevel.Info, "Quizz ready: "+Questions.Count+" questions!");
		}

		/// <summary>
		/// Player has selected its answer.
		/// </summary>
		/// <param name="index">Answer index</param>
		public void SelectQuestion (int index)
		{
			bool result = CurrentQuestion.IsValidAnswer (index);

			if (result) {
				Logger.Log (LogLevel.Info, "Good answer!");
			}
			else {
				Logger.Log (LogLevel.Info, "Bad answer...");
			}
		}

		/// <summary>
		/// Move to the next question. Mark as over if no more.
		/// </summary>
		public void NextQuestion ()
		{
			Logger.Log (LogLevel.Info, "Next question requested");
			
			m_questionIndex++;
			
			if (m_questionIndex < Questions.Count) {
				CurrentQuestion = Questions [m_questionIndex];
			} else {
				Logger.Log (LogLevel.Info, "Quizz is over!");
				IsOver = true;
			}
		}
	}
}

