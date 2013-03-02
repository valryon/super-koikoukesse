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
		public float TimeLeft { get; set; }

		/// <summary>
		/// Game is paused?
		/// </summary>
		/// <value><c>true</c> if this instance is paused; otherwise, <c>false</c>.</value>
		public bool IsPaused { get; set; }

		/// <summary>
		/// Current score
		/// </summary>
		/// <value>The score.</value>
		public int Score { get; set; }

		/// <summary>
		/// Current score
		/// </summary>
		/// <value>The score.</value>
		public int Combo { get; set; }

		/// <summary>
		/// Lives count
		/// </summary>
		/// <value>The lives.</value>
		public int Lives { get; set; }

		/// <summary>
		/// Count the joker part filled
		/// </summary>
		/// <value>The joker part count.</value>
		public int JokerPartCount{ get; set; }

		/// <summary>
		/// Register answers results
		/// </summary>
		/// <value>The good answers count.</value>
		public Dictionary<Question, bool> Results { get; private set; }

		private int m_questionIndex;

		public Quizz ()
		{
			Results = new Dictionary<Question, bool> ();
		}

		public void Initialize ()
		{
			Logger.Log (LogLevel.Info, "Initializing quizz...");

			Questions = new List<Question> ();
			List<int> correctAnswerIds = new List<int> ();

			int questionCount = 4; // TODO Externaliser
			int answerCount = 4;// TODO Externaliser

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

			Logger.Log (LogLevel.Info, "Quizz ready: "+Questions.Count+" questions!");

			
			// Initialize score, lives, combo
			TimeLeft = -1;
			Lives = 3; //TODO externaliser
			Score = 0;
			Combo = 1;
			JokerPartCount = 0;
			Results.Clear ();

			// Get the first
			m_questionIndex = -1;
			NextQuestion ();

			IsPaused = false;
		}

		/// <summary>
		/// Player has selected its answer.
		/// </summary>
		/// <param name="index">Answer index</param>
		public void SelectQuestion (int index)
		{
			bool result = CurrentQuestion.IsValidAnswer (index);

			int score = 100;// TODO Externaliser
			int comboToApply = Combo;

			if (result) {
				Logger.Log (LogLevel.Info, "Good answer!");
				Combo++;
				JokerPartCount++;

				if(JokerPartCount > 3) JokerPartCount = 3; // TODO Externaliser
			}
			else {
				Logger.Log (LogLevel.Info, "Bad answer...");
				Combo = 1;
				comboToApply = 1;
				Lives--;
				JokerPartCount = 0;
			}

			// Apply score
			Score += score * comboToApply; 

			Question q = Questions [m_questionIndex];
			Results.Add (q, result);
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

				// Reset timer
				TimeLeft = 10f; // TODO Externaliser

				IsOver = false;
			} else {
				Logger.Log (LogLevel.Info, "Quizz is over!");
				IsOver = true;
			}
		}

		/// <summary>
		/// Make something when the time is over
		/// </summary>
		public void TimeIsOver ()
		{
			//TODO Fonction du mode

		}
	}
}

