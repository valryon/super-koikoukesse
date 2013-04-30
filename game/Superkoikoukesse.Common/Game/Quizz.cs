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
		/// Time left for this game (seconds) when quizz starts
		/// </summary>
		/// <value>The time left.</value>
		public float BaseTimeleft {private set; get;}

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
		public Dictionary<int, bool> Results { get; private set; }

		/// <summary>
		/// Date of the quizz start
		/// </summary>
		/// <value>The start time.</value>
		public DateTime StartTime{ get; private set; }

		/// <summary>
		/// Current question index
		/// </summary>
		/// <value>The question number.</value>
		public int QuestionNumber {get; private set;}

		/// <summary>
		/// Coins to add
		/// </summary>
		/// <value>The earned coins.</value>
		public int EarnedCoins {get; private set;}

		/// <summary>
		/// Rank found at the last score add
		/// </summary>
		/// <value>The rank for last score.</value>
		public int  RankForLastScore {get; private set;}

		/// <summary>
		/// Filter for game database
		/// </summary>
		/// <value>The filter.</value>
		public Filter Filter {get; private set;}

		// Game parameters
		private Queue<Question> m_questionsPool;
		private int m_initialQuestionCount;
		private bool m_infiniteQuestions;
		private int m_answerCount;

		private int m_baseScore;
		private int m_jokerMinPart;
		private int m_mistakesCount;
		private object m_timeLeftLock = new object ();
		private List<int> m_correctAnswerIds;
		private Random m_random;
		private List<ImageTransformations> m_availableTransformations;

		public Quizz ()
		{
			Results = new Dictionary<int, bool> ();
			m_random = new Random (DateTime.Now.Millisecond);
		}

		public void Initialize (GameModes mode, GameDifficulties difficulty, GameConfiguration config, Filter databaseFilter)
		{
			Mode = mode;
			Difficulty = difficulty;
			Filter = databaseFilter;

			Logger.Log (LogLevel.Info, "Initializing quizz " + Mode + " " + Difficulty + "...");

			// Initialize score, lives, combo
			TimeLeft = -1;
			Score = 0;
			Combo = 1;
			JokerPartCount = 0;
			Results.Clear ();
			StartTime = DateTime.Now;
			m_mistakesCount = 0;
			QuestionNumber = 0;

			initializeQuizzFromConfiguration (config);

			// Get questions
			m_correctAnswerIds = new List<int> ();
			m_questionsPool = new Queue<Question> ();

			for (int i=0; i< m_initialQuestionCount; i++) {

				var q = getRandomQuestion ();
				m_questionsPool.Enqueue (q);
			}

			Logger.Log (LogLevel.Info, "Quizz ready: " + m_questionsPool.Count + " questions in cache!");

			// Get the first
			NextQuestion ();

			IsPaused = false;
		}

		private void initializeQuizzFromConfiguration (GameConfiguration config)
		{
			// Get config
			var modeConfig = config.GetModeConfiguration (Mode, Difficulty);

			Lives = 3; // TODO ?

			// Get values
			if (modeConfig == null) {

				Logger.Log (LogLevel.Error, "No configuration for game mode " + Mode + "!");

				m_initialQuestionCount = 5;
				BaseTimeleft = 60;
				m_baseScore = 1;

			} else {
				if (modeConfig.QuestionCount.HasValue) {
					m_initialQuestionCount = modeConfig.QuestionCount.Value; 
				} else {
					m_initialQuestionCount = 1;
				}
				if (modeConfig.Score.HasValue) {
					m_baseScore = modeConfig.Score.Value;
				}
				if (modeConfig.Time.HasValue) {
					BaseTimeleft = modeConfig.Time.Value;
				}
			}

			if (Mode == GameModes.TimeAttack) {
				m_infiniteQuestions = true;
			} else if (Mode == GameModes.Survival) {
				m_infiniteQuestions = true;
			} else {
				m_infiniteQuestions = false;
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

			TimeLeft = BaseTimeleft;
		}

		/// <summary>
		/// Get a random question
		/// </summary>
		/// <returns>The random question.</returns>
		internal Question getRandomQuestion ()
		{
			int currentAnswersCount = 0;

			Question q = new Question ();

			while (currentAnswersCount < m_answerCount) {

				GameInfo game = Filter.GetGame();

				if (q.Answers.Contains (game) == false && m_correctAnswerIds.Contains (game.GameId) == false) {

					q.Answers.Add (game);
					currentAnswersCount++;

					// Decide that the first will be the correct answer
					if (q.CorrectAnswer == null) {
						q.CorrectAnswer = game;
						m_correctAnswerIds.Add (q.CorrectAnswer.GameId);
					}
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
			if (m_availableTransformations == null) {
				m_availableTransformations = new List<ImageTransformations> ();

				foreach (ImageTransformations it in Enum.GetValues (typeof(ImageTransformations))) {

					if(it != ImageTransformations.None) {
						m_availableTransformations.Add (it);
					}
				}
			}

			int randomIndex = m_random.Next (m_availableTransformations.Count);

			return m_availableTransformations [randomIndex];
		}

		/// <summary>
		/// Player has selected an answer.
		/// </summary>
		/// <param name="index">Answer index</param>
		public void SelectAnswer (int index, bool isJoker = false)
		{
			int score = m_baseScore; 
			int comboToApply = Combo;
			int malus = 0;
			int bonus = 0;

			bool result = false;

			if (isJoker) {
				Logger.Log (LogLevel.Info, "Joker answer");

				comboToApply = 0;

				result = true;

			} else {

				result = CurrentQuestion.IsValidAnswer (index);

				if (result) {
					Logger.Log (LogLevel.Info, "Good answer!");
					Combo++;

					Combo = Math.Min (Combo, Constants.ComboMax);

					JokerPartCount++;

					if (JokerPartCount >= Constants.JokerPartMax) {
						JokerPartCount = Constants.JokerPartMax; 
					}

					// Apply bonus/malus per mode
					if(Mode == GameModes.ScoreAttack) {
						malus = (int)TimeLeft;
					}

				} else {
					Logger.Log (LogLevel.Info, "Bad answer...");
					Combo = 1;
					comboToApply = 0;
					JokerPartCount = 0;

					m_mistakesCount++;

					// Apply bonus/malus per mode
					if (Mode == GameModes.ScoreAttack) {
						malus = 25;
					}
					else if (Mode == GameModes.TimeAttack) {

						malus = 25;

						// Losing time for each mistakes
						SubstractTime (m_mistakesCount); // 1 sec per accumulated mistakes

					} else if (Mode == GameModes.Survival) {

						// Losing live for each mistakes
						Lives--;

						Logger.Log (LogLevel.Debug, "Remaining lives: " + Lives);
					}
				}
			}

			// Apply score
			Score += (score + bonus - malus) * comboToApply; 

			if (Results.ContainsKey (CurrentQuestion.CorrectAnswer.GameId) == false) {
				Results.Add (CurrentQuestion.CorrectAnswer.GameId, result);
			} else {
				Logger.Log (LogLevel.Error, "Duplicated question registered! Are you testing something stupid?"); 
			}
		}

		/// <summary>
		/// Move to the next question. Mark as over if no more.
		/// </summary>
		public void NextQuestion ()
		{
			Logger.Log (LogLevel.Info, "Next question requested");

			QuestionNumber++;

			// Infinite list of question
			if (m_infiniteQuestions) {
				if (m_questionsPool.Count <= 2) {

					int questionCachedCount = 5;

					Logger.Log (LogLevel.Info, "Caching questions...");

					// Pool some
					for (int i=0; i<questionCachedCount; i++) {
						Logger.Log (LogLevel.Info, "Caching questions..." + i);
						m_questionsPool.Enqueue (getRandomQuestion ());
					}

					Logger.Log (LogLevel.Info, "Cached " + questionCachedCount + " new questions.");

				}
			}

			// Take next question
			if (m_questionsPool.Count > 0) {

				CurrentQuestion = m_questionsPool.Dequeue ();

				// Reset timer
				if (Mode == GameModes.TimeAttack) {
					if (TimeLeft < 0) {
						IsOver = true;
					}
				} else {
					TimeLeft = BaseTimeleft;
					IsOver = false;

					if (Mode == GameModes.Survival) {
						IsOver = (Lives <= 0);
					}
				}
			} else {

				if (Mode != GameModes.TimeAttack) {

					Logger.Log (LogLevel.Info, "Quizz is over!");
					IsOver = true;
				} 
			}

			// Change image transformation
			if (Difficulty != GameDifficulties.Normal) {
				ImageTransformation =  getImageTransformation();
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

				// Add a question
				m_questionsPool.Enqueue (getRandomQuestion ());

				SelectAnswer (-1, true);
			}
		}

		/// <summary>
		/// Operation on the time
		/// </summary>
		/// <param name="timeLost">Time lost.</param>
		public void SubstractTime (float timeLost)
		{
			lock (m_timeLeftLock) {
				TimeLeft -= timeLost;
			}
		}

		public void EndQuizz() {

			// Add score to local DB
			RankForLastScore = DatabaseService.Instance.AddLocalScore(new LocalScore() {
				Mode = Mode,
				Difficulty = Difficulty,
				Date = DateTime.Now,
				Score = Score
			});

			// Send score to Game Center
			ProfileService.Instance.AuthenticatedPlayer.AddScore (Mode, Difficulty, Score);

			// Multiplayer? End turn
			if(Mode == GameModes.Versus) {
				ProfileService.Instance.AuthenticatedPlayer.EndMatchTurn(Score,() => {

				});
			}

			// Send score to our server
			SendQuizzData();

			// New coins
			// SOLO = 1 coins per 1000 points
			EarnedCoins = (int)Math.Round(Score / 1000f);

			// Send those coins
			ProfileService.Instance.AddCoins(EarnedCoins);
		}

		#region Stats

		/// <summary>
		/// Send quizz data to the webservice
		/// </summary>
		public void SendQuizzData ()
		{
			WebserviceStats stats = new WebserviceStats ();

			stats.SendStats ("Valryon", Score, Mode, Difficulty, StartTime, Results, null);
		}

		#endregion
	}
}

