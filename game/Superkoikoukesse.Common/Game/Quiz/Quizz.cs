using System;
using System.Linq;
using System.Collections.Generic;

namespace Superkoikoukesse.Common
{
	public class Quizz
	{
		// Game parameters
		private Queue<Question> mQuestionsPool;
		private int mInitialQuestionCount;
		private bool mInfiniteQuestions;
		private int mAnswerCount;
		private int mBaseScore;
		private int mJokerMinPart;
		private int mMistakesCount;
		private object mTimeLeftLock = new object ();
		private List<int> mCorrectAnswerIds;
		private Random mRandom;
		private List<ImageTransformations> mAvailableTransformations;

		public Quizz ()
		{
			Results = new Dictionary<int, bool> ();
			mRandom = new Random (DateTime.Now.Millisecond);
		}

		/// <summary>
		/// Initialize the quiz with the given parameters
		/// </summary>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		/// <param name="config">Config.</param>
		/// <param name="databaseFilter">Database filter.</param>
		public void Initialize (GameModes mode, GameDifficulties difficulty, GameConfiguration config, Filter databaseFilter)
		{
			Mode = mode;
			Difficulty = difficulty;
			Filter = databaseFilter;

			Logger.I("Initializing quizz " + Mode + " " + Difficulty + "...");

			// Initialize score, lives, combo
			TimeLeft = -1;
			Score = 0;
			Combo = 1;
			JokerPartCount = 0;
			Results.Clear ();
			StartTime = DateTime.Now;
			mMistakesCount = 0;
			QuestionNumber = 0;

			InitializeQuizzFromConfiguration (config);

			// Get questions
			mCorrectAnswerIds = new List<int> ();
			mQuestionsPool = new Queue<Question> ();

			for (int i=0; i< mInitialQuestionCount; i++) {

				var q = GetRandomQuestion ();
				mQuestionsPool.Enqueue (q);
			}

			Logger.I("Quizz ready: " + mQuestionsPool.Count + " questions in cache!");

			// Get the first
			SetNextQuestion ();

			IsPaused = false;
		}

		/// <summary>
		/// Adjust game parameters from configuration
		/// </summary>
		/// <param name="config">Config.</param>
		private void InitializeQuizzFromConfiguration (GameConfiguration config)
		{
			// Get config
			var modeConfig = config.GetModeConfiguration (Mode, Difficulty);

			Lives = 3; // TODO ?

			// Get values
			if (modeConfig == null) {

				Logger.E( "No configuration for game mode " + Mode + "!");

				mInitialQuestionCount = 5;
				BaseTimeleft = 60;
				mBaseScore = 1;

			} else {
				if (modeConfig.QuestionCount.HasValue) {
					mInitialQuestionCount = modeConfig.QuestionCount.Value; 
				} else {
					mInitialQuestionCount = 1;
				}
				if (modeConfig.Score.HasValue) {
					mBaseScore = modeConfig.Score.Value;
				}
				if (modeConfig.Time.HasValue) {
					BaseTimeleft = modeConfig.Time.Value;
				}
			}

			if (Mode == GameModes.TIME_ATTACK) {
				mInfiniteQuestions = true;
			} else if (Mode == GameModes.SURVIVAL) {
				mInfiniteQuestions = true;
			} else {
				mInfiniteQuestions = false;
			}

			// Transformations
			ImageTransformation = ImageTransformations.NONE;
			TextTransformation = TextTransformations.NONE;

			if (Difficulty == GameDifficulties.HARD) {

				ImageTransformation = GetImageTransformation ();

			} else if (Difficulty == GameDifficulties.EXPERT) {

				ImageTransformation = GetImageTransformation ();
				TextTransformation = TextTransformations.FIRST_LETTER_ONLY;

			} else if (Difficulty == GameDifficulties.NOLIFE) {

				ImageTransformation = GetImageTransformation ();
				TextTransformation = TextTransformations.UNDERSCORES_ONLY;
			}

			// Static vars
			mAnswerCount = 4;
			mJokerMinPart = 3;

			TimeLeft = BaseTimeleft;
		}

		/// <summary>
		/// Get a random question
		/// </summary>
		/// <returns>The random question.</returns>
		internal Question GetRandomQuestion ()
		{
			int currentAnswersCount = 0;

			Question q = null;

			// Multiplayer and not the first turn?
			// We must play exactly the same game as the other players
			if (Mode == GameModes.VERSUS) {
				if (PlayerCache.Instance.AuthenticatedPlayer.CurrentMatch.IsFirstTurn == false) {
					q = Filter.GetMatchQuestion ();
				}
			}

			// Not multiplayer or no more registered quesiton?
			// Let's go random
			if (q == null) {

				q = new Question ();
				bool isFirstAndCorrectAnswer = true;

				while (currentAnswersCount < mAnswerCount) {

					// If we have a given game questions list (versus mode), we can randomize all answers except the correct one
					GameInfo game = Filter.GetRandomGame ();

					if (q.Answers.Contains (game) == false && mCorrectAnswerIds.Contains (game.GameId) == false) {

						q.Answers.Add (game);
						currentAnswersCount++;

						// Decide that the first will be the correct answer
						if (isFirstAndCorrectAnswer) {

							q.CorrectAnswer = game;
							mCorrectAnswerIds.Add (q.CorrectAnswer.GameId);

							isFirstAndCorrectAnswer = false;
						}
					}
				}
				// Randomize answers
				q.ShuffleAnswers ();
			}
			return q;
		}

		/// <summary>
		/// Get a random image transformation
		/// </summary>
		/// <returns>The image transformation.</returns>
		private ImageTransformations GetImageTransformation ()
		{
			// HACK DEBUG
//			return ImageTransformations.Test;

			if (mAvailableTransformations == null) {
				mAvailableTransformations = new List<ImageTransformations> ();

				foreach (ImageTransformations it in Enum.GetValues (typeof(ImageTransformations))) {

					if (it != ImageTransformations.NONE) {
						mAvailableTransformations.Add (it);
					}
				}
			}

			int randomIndex = mRandom.Next (mAvailableTransformations.Count);

			return mAvailableTransformations [randomIndex];
		}

		/// <summary>
		/// Player has selected an answer.
		/// </summary>
		/// <param name="index">Answer index</param>
		public void SelectAnswer (int index, bool isJoker = false)
		{
			int score = mBaseScore; 
			int comboToApply = Combo;
			int malus = 0;
			int bonus = 0;

			bool result = false;

			if (isJoker) {
				Logger.I("Joker answer");

				comboToApply = 0;

				result = true;

			} else {

				result = CurrentQuestion.IsValidAnswer (index);

				if (result) {
					Logger.I("Good answer!");
					Combo++;

					Combo = Math.Min (Combo, Constants.COMBO_MAXIMUM_COUNT);

					JokerPartCount++;

					if (JokerPartCount >= Constants.JOKER_PART_COUNT) {
						JokerPartCount = Constants.JOKER_PART_COUNT; 
					}

					// Apply bonus/malus per mode
					if (Mode == GameModes.SCORE_ATTACK) {
						malus = (int)TimeLeft;
					}

				} else {
					Logger.I("Bad answer...");
					Combo = 1;
					comboToApply = 0;
					JokerPartCount = 0;

					mMistakesCount++;

					// Apply bonus/malus per mode
					if (Mode == GameModes.SCORE_ATTACK) {
						malus = 25;
					} else if (Mode == GameModes.TIME_ATTACK) {

						malus = 25;

						// Losing time for each mistakes
						SubstractTime (mMistakesCount); // 1 sec per accumulated mistakes

					} else if (Mode == GameModes.SURVIVAL) {

						// Losing live for each mistakes
						Lives--;

						Logger.D( "Remaining lives: " + Lives);
					}
				}
			}

			// Apply score
			Score += (score + bonus - malus) * comboToApply; 

			if (Results.ContainsKey (CurrentQuestion.CorrectAnswer.GameId) == false) {
				Results.Add (CurrentQuestion.CorrectAnswer.GameId, result);
			} else {
				Logger.E( "Duplicated question registered! Are you testing something stupid?"); 
			}
		}

		/// <summary>
		/// Move to the next question. Mark as over if no more.
		/// </summary>
		public void SetNextQuestion ()
		{
			Logger.I("Next question requested");

			QuestionNumber++;

			// Infinite list of question
			if (mInfiniteQuestions) {
				if (mQuestionsPool.Count <= 2) {

					int questionCachedCount = 5;

					Logger.I("Caching questions...");

					// Pool some
					for (int i=0; i<questionCachedCount; i++) {
						Logger.I("Caching questions..." + i);
						mQuestionsPool.Enqueue (GetRandomQuestion ());
					}

					Logger.I("Cached " + questionCachedCount + " new questions.");

				}
			}

			// Take next question
			if (mQuestionsPool.Count > 0) {

				CurrentQuestion = mQuestionsPool.Dequeue ();

				// Reset timer
				if (Mode == GameModes.TIME_ATTACK) {
					if (TimeLeft < 0) {
						IsOver = true;
					}
				} else {
					TimeLeft = BaseTimeleft;
					IsOver = false;

					if (Mode == GameModes.SURVIVAL) {
						IsOver = (Lives <= 0);
					}
				}
			} else {

				if (Mode != GameModes.TIME_ATTACK) {

					Logger.I("Quizz is over!");
					IsOver = true;
				} 
			}

			// Multiplayer specifity: register all first player question @ answers
			if (IsOver == false && Mode == GameModes.VERSUS) {

				if (PlayerCache.Instance.AuthenticatedPlayer.CurrentMatch.IsFirstTurn) {
					
					Filter matchFilter = PlayerCache.Instance.AuthenticatedPlayer.CurrentMatch.Filter;

					if (matchFilter.RequiredGameIds == null) {
						matchFilter.RequiredGameIds = new Dictionary<int, int[]> ();
					}

					matchFilter.RequiredGameIds.Add (CurrentQuestion.CorrectAnswer.GameId, 
					                                 CurrentQuestion.Answers.Select (a => a.GameId).ToArray ()
					);
				}
			}

			// Change image transformation
			if (Difficulty != GameDifficulties.NORMAL) {
				ImageTransformation = GetImageTransformation ();
			}
		}

		/// <summary>
		/// Make something when the time is over
		/// </summary>
		public void SetTimeOver ()
		{
			if (Mode == GameModes.TIME_ATTACK) {
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
				mQuestionsPool.Enqueue (GetRandomQuestion ());

				SelectAnswer (-1, true);
			}
		}

		/// <summary>
		/// Operation on the time
		/// </summary>
		/// <param name="timeLost">Time lost.</param>
		public void SubstractTime (float timeLost)
		{
			lock (mTimeLeftLock) {
				TimeLeft -= timeLost;
			}
		}

		/// <summary>
		/// Terminate the quizz
		/// </summary>
		public void EndQuizz ()
		{
			// Add score to local DB
			RankForLastScore = GameDatabase.Instance.AddLocalScore (new LocalScore () {
				Mode = Mode,
				Difficulty = Difficulty,
				Date = DateTime.Now,
				Score = Score
			});

			// Send score to Game Center
			PlayerCache.Instance.AuthenticatedPlayer.AddScore (Mode, Difficulty, Score);

			// Multiplayer? End turn
			if (Mode == GameModes.VERSUS) {
				PlayerCache.Instance.AuthenticatedPlayer.EndMatchTurn (Score, () => {

				});
			}

			// Send score to our server
			SendQuizzData ();

			// New coins
			// SOLO = 1 coins per 1000 points
			EarnedCoins = (int)Math.Round (Score / 1000f);

			// Send those coins
			PlayerCache.Instance.AddCoins (EarnedCoins);
		}

		/// <summary>
		/// Send quizz data to the webservice
		/// </summary>
		public void SendQuizzData ()
		{
			WebserviceStats stats = new WebserviceStats ();

			stats.SendStats ("Valryon", Score, Mode, Difficulty, StartTime, Results, null);
		}

		#region Properties

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
		public float BaseTimeleft { private set; get; }

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
				return (JokerPartCount >= mJokerMinPart); 
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
		public int QuestionNumber { get; private set; }

		/// <summary>
		/// Coins to add
		/// </summary>
		/// <value>The earned coins.</value>
		public int EarnedCoins { get; private set; }

		/// <summary>
		/// Rank found at the last score add
		/// </summary>
		/// <value>The rank for last score.</value>
		public int  RankForLastScore { get; private set; }

		/// <summary>
		/// Filter for game database
		/// </summary>
		/// <value>The filter.</value>
		public Filter Filter { get; private set; }

		#endregion
	}
}

