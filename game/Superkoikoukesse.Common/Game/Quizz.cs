using System;
using System.Linq;
using System.Collections.Generic;

namespace Superkoikoukesse.Common
{
	public class Quizz
	{
		/// <summary>
		/// Game mode
		/// </summary>
		/// <value>The mode.</value>
		public GameModes Mode { get; set; }

		/// <summary>
		/// Game Difficulty
		/// </summary>
		/// <value>The difficulty.</value>
		public GameDifficulty Difficulty { get; set; }

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
		public int JokerPartCount { get; set; }

		/// <summary>
		/// Determines if the joker can be use
		/// </summary>
		public bool IsJokerAvailable {
			get {
				return (JokerPartCount >= m_jokerMinPart); 
			}
		}

		/// <summary>
		/// Register answers results
		/// </summary>
		/// <value>The good answers count.</value>
		public Dictionary<Question, bool> Results { get; private set; }

		/// <summary>
		/// Date of the quizz start
		/// </summary>
		/// <value>The start time.</value>
		public DateTime StartTime{ get; private set; }

		// Game parameters
		private int m_questionCount;
		private int m_answerCount;
		private float m_baseTimeleft;
		private int m_baseScore;
		private int m_jokerMinPart;

		// Current question index
		private int m_questionIndex;

		public Quizz ()
		{
			Results = new Dictionary<Question, bool> ();
		}

		public void Initialize (GameModes mode, GameDifficulty difficulty, GameConfiguration config)
		{
			Mode = mode;
			Difficulty = difficulty;

			Logger.Log (LogLevel.Info, "Initializing quizz...");

			// Initialize score, lives, combo
			TimeLeft = -1;
			Score = 0;
			Combo = 1;
			JokerPartCount = 0;
			Results.Clear ();
			StartTime = DateTime.Now;
			
			initializeQuizzFromConfiguration (config);

			// Get questions
			Questions = new List<Question> ();
			List<int> correctAnswerIds = new List<int> ();

			// Randomly
			for (int i=0; i< m_questionCount; i++) {
				int count = 0;

				Question q = new Question ();

				while (count < m_answerCount) {
				
					GameInfo game = DatabaseService.Instance.RandomGame ();

					if (q.Answers.Contains (game) == false && correctAnswerIds.Contains (game.GameId) == false) {
						q.Answers.Add (game);
						count++;
					
						// Decide that the first will be the correct answer
						if (q.CorrectAnswer == null) {
							q.CorrectAnswer = game;
							correctAnswerIds.Add (game.GameId);
						}
					}
				}

				// Randomize answers
				q.ShuffleAnswers ();

				Questions.Add (q);
			}

			Logger.Log (LogLevel.Info, "Quizz ready: " + Questions.Count + " questions!");

			// Get the first
			m_questionIndex = -1;
			NextQuestion ();

			IsPaused = false;
		}

		private void initializeQuizzFromConfiguration (GameConfiguration config)
		{
			// Get config
			var modeConfig = config.GetModeConfiguration (Mode, Difficulty);

			// Get values
			Lives = 3; // TODO ?

			if (modeConfig.QuestionCount.HasValue) {
				m_questionCount = modeConfig.QuestionCount.Value; 
			}
			if (modeConfig.Score.HasValue) {
				m_baseScore = modeConfig.Score.Value;
			}
			if (modeConfig.Time.HasValue) {
				m_baseTimeleft = modeConfig.Time.Value;
			}
			// Static vars
			m_answerCount = 4;
			m_jokerMinPart = 3;

			TimeLeft = m_baseTimeleft;
		}

		/// <summary>
		/// Player has selected an answer.
		/// </summary>
		/// <param name="index">Answer index</param>
		public void SelectAnswer (int index, bool isJoker = false)
		{
			bool result = CurrentQuestion.IsValidAnswer (index);

			if (isJoker) {
				result = true;
			}

			int score = m_baseScore; 
			int comboToApply = Combo;

			if (result) {
				Logger.Log (LogLevel.Info, "Good answer! " + (isJoker ? "(JOKER)" : ""));
				Combo++;

				if (isJoker == false) {
					JokerPartCount++;

					if (JokerPartCount >= 3) {
						JokerPartCount = 3; 
					}
				}
			} else {
				Logger.Log (LogLevel.Info, "Bad answer...");
				Combo = 1;
				comboToApply = 0;
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
				if (Mode != GameModes.TimeAttack) {
					TimeLeft = m_baseTimeleft;
				}
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
			if (Mode == GameModes.TimeAttack) {
				IsOver = true;
			} else {
				SelectAnswer (-1);
			}
		}

		/// <summary>
		/// Uses the joker.
		/// </summary>
		public void UseJoker ()
		{

			if (IsJokerAvailable) {
				JokerPartCount = 0;

				SelectAnswer (-1, true);
			}
		}

		/// <summary>
		/// Send quizz data to the webservice
		/// </summary>
		public void SendQuizzData (Action<Exception> failureCallback)
		{
			WebserviceStats stats = new WebserviceStats ();

			stats.SendStats ("Valryon", Score, Mode, Difficulty, StartTime, Results, failureCallback);
		}
	}
}

