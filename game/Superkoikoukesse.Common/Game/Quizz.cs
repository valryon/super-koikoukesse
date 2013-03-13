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
		public GameDifficulties Difficulty { get; set; }

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
		/// Transformation to apply to the current image
		/// </summary>
		/// <value>The image transformation.</value>
		public ImageTransformations ImageTransformation { get; private set; }


		/// <summary>
		/// Transformation to apply to the text
		/// </summary>
		/// <value>The image transformation.</value>
		public TextTransformations TextTransformation { get; private set; }

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
		private int m_mistakesCount;
		private object m_timeLeftLock = new object();
		private List<int> m_correctAnswerIds;

		public Quizz ()
		{
			Results = new Dictionary<Question, bool> ();
		}

		public void Initialize (GameModes mode, GameDifficulties difficulty, GameConfiguration config)
		{
			Mode = mode;
			Difficulty = difficulty;

			Logger.Log (LogLevel.Info, "Initializing quizz " + Mode + " " + Difficulty + "...");

			// Initialize score, lives, combo
			TimeLeft = -1;
			Score = 0;
			Combo = 1;
			JokerPartCount = 0;
			Results.Clear ();
			StartTime = DateTime.Now;
			m_mistakesCount = 0;

			initializeQuizzFromConfiguration (config);

			// Get questions
			m_correctAnswerIds = new List<int> ();
			Questions = new List<Question> ();

			// Randomly
			for (int i=0; i< m_questionCount; i++) {

				var q = getRandomQuestion ();

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

			if (modeConfig == null) {

				Logger.Log(LogLevel.Error, "No configuration for game mode "+Mode+"!");

				m_questionCount = 1;
				m_baseTimeleft = 60;
				m_baseScore = 1;

			} else {
				if (modeConfig.QuestionCount.HasValue) {
					m_questionCount = modeConfig.QuestionCount.Value; 
				}
				else {
					m_questionCount = 1;
				}
				if (modeConfig.Score.HasValue) {
					m_baseScore = modeConfig.Score.Value;
				}
				if (modeConfig.Time.HasValue) {
					m_baseTimeleft = modeConfig.Time.Value;
				}
			}

			// Transformations
			ImageTransformation = ImageTransformations.None;
			TextTransformation = TextTransformations.None;

			if (Difficulty == GameDifficulties.Hard) {

				ImageTransformation = getImageTransformation ();

			} else if (Difficulty == GameDifficulties.Expert) {

				ImageTransformation = getImageTransformation ();
				TextTransformation = TextTransformations.FirstLetterOnly;

			} else if (Difficulty == GameDifficulties.Nolife) {

				ImageTransformation = getImageTransformation ();
				TextTransformation = TextTransformations.UnderscoresOnly;
			}

			// Static vars
			m_answerCount = 4;
			m_jokerMinPart = 3;

			TimeLeft = m_baseTimeleft;
		}

		/// <summary>
		/// Get a rabdom question
		/// </summary>
		/// <returns>The random question.</returns>
		internal Question getRandomQuestion ()
		{
			int currentAnswersCount = 0;

			Question q = new Question ();

			while (currentAnswersCount < m_answerCount) {

				GameInfo game = DatabaseService.Instance.RandomGame ();

				if (q.Answers.Contains (game) == false && m_correctAnswerIds.Contains (game.GameId) == false) {

					q.Answers.Add (game);
					currentAnswersCount++;

					// Decide that the first will be the correct answer
					if (q.CorrectAnswer == null) {
						q.CorrectAnswer = game;
					}

					m_correctAnswerIds.Add (game.GameId);
				}
			}
			// Randomize answers
			q.ShuffleAnswers ();

			return q;
		}

		/// <summary>
		/// Get a random image transformation
		/// </summary>
		/// <returns>The image transformation.</returns>
		private ImageTransformations getImageTransformation ()
		{
			return ImageTransformations.Pixelization; // TODO Images transformation
		}

		/// <summary>
		/// Player has selected an answer.
		/// </summary>
		/// <param name="index">Answer index</param>
		public void SelectAnswer (int index, bool isJoker = false)
		{
			int score = m_baseScore; 
			int comboToApply = Combo;

			bool result = false;

			if (isJoker) {
				Logger.Log (LogLevel.Info, "Joker answer");

				comboToApply = 0;

				result = true;

			} else {

				result = CurrentQuestion.IsValidAnswer (index);

				if (result) {
					Logger.Log (LogLevel.Info, "Good answer! " + (isJoker ? "(JOKER)" : ""));
					Combo++;

					JokerPartCount++;

					if (JokerPartCount >= 3) {
						JokerPartCount = 3; 
					}
				} else {
					Logger.Log (LogLevel.Info, "Bad answer...");
					Combo = 1;
					comboToApply = 0;
					JokerPartCount = 0;

					m_mistakesCount++;

					if (Mode == GameModes.TimeAttack) {

						// Losing time for each mistakes
						SubstractTime(m_mistakesCount); // 1 sec per accumulated mistakes

					} else if (Mode == GameModes.Survival) {

						// Losing live for each mistakes
						Lives--;
					}
				}
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

			// Infinite list of question
			if (Mode == GameModes.TimeAttack) {
				if (m_questionIndex + 1 < Questions.Count) {
					Questions.Add (getRandomQuestion ());
				}
			}

			m_questionIndex++;
			
			if (m_questionIndex < Questions.Count) {
				CurrentQuestion = Questions [m_questionIndex];

				// Reset timer
				if (Mode != GameModes.TimeAttack) {
					TimeLeft = m_baseTimeleft;
					IsOver = false;
				}
			} else {

				if (Mode != GameModes.TimeAttack) {

					Logger.Log (LogLevel.Info, "Quizz is over!");
					IsOver = true;
				} 
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
		/// Operation on the time
		/// </summary>
		/// <param name="timeLost">Time lost.</param>
		public void SubstractTime(float timeLost) {
			lock (m_timeLeftLock) {
				TimeLeft -= timeLost;
			}
		}

		#region Stats

		/// <summary>
		/// Send quizz data to the webservice
		/// </summary>
		public void SendQuizzData (Action<Exception> failureCallback)
		{
			WebserviceStats stats = new WebserviceStats ();

			stats.SendStats ("Valryon", Score, Mode, Difficulty, StartTime, Results, failureCallback);
		}

		#endregion
	}
}

