using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Json;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Filter for game database
	/// </summary>
	public class Filter: IServiceOutput
	{
		private int mCurrentRequiredGameIdsIndex;
		private int mCurrentRequiredGameIdsAnswerIndex;
		private List<GameEntry> mMatchingGames;

		public Filter (JsonValue json)
		{
			BuildFromJsonObject (json);
		}

		public Filter (string id = "", string name = "default", string icon = "", int minYear = 0, int maxYear = 9999, List<string> publishers = null, List<string> genres = null, List<string> platforms = null)
		{
			Id = id;
			Name = name;
			Icon = icon;
			MinYear = minYear;
			MaxYear = maxYear;

			Publishers = publishers;
			Genres = genres;
			Platforms = platforms;

			RequiredGameIds = null;
			mCurrentRequiredGameIdsIndex = 0;
		}

		/// <summary>
		/// Caching database
		/// </summary>
		public int Load ()
		{
			if (RequiredGameIds != null && RequiredGameIds.Count > 0) {
				mCurrentRequiredGameIdsIndex = 0;
			}

			mMatchingGames = GameDatabase.Instance.ReadGames (MinYear, MaxYear, Publishers, Genres, Platforms);

			return mMatchingGames.Count;
		}

		/// <summary>
		/// Get a new question for a multiplayer match, not the first turn
		/// </summary>
		/// <returns>The question.</returns>
		public Question GetMatchQuestion ()
		{
			if (RequiredGameIds != null && RequiredGameIds.Count > 0) {
				
				if (mCurrentRequiredGameIdsIndex < RequiredGameIds.Count) {

					Question q = new Question ();

					KeyValuePair<int, int[]> questionInfo = RequiredGameIds.ElementAt (mCurrentRequiredGameIdsIndex);

					for (int i=0; i < questionInfo.Value.Length; i++) {

						int answerId = questionInfo.Value[i];
						GameEntry game = mMatchingGames.Where (g => g.GameId == answerId).FirstOrDefault ();

						if (game == null) {
							Logger.E( "The game with id " + answerId + " wasn't loaded by the filter!");
							// HACK : The game is missing, let's not crash.
							return null;
						} else {
							q.Answers.Add (game);

							if (answerId == questionInfo.Key) {
								q.CorrectAnswer = game;
							}
						}
					}
					
					mCurrentRequiredGameIdsIndex++;

					return q;
				}
			}

			return null;
		}

		/// <summary>
		/// Get a game corresponding to this filter
		/// </summary>
		/// <returns>The games.</returns>
		/// <param name="count">Count.</param>
		public GameEntry GetRandomGame ()
		{
			GameEntry game = null;

			// Random game
			var random = new Random (DateTime.Now.Millisecond);
			int randomIndex = random.Next (mMatchingGames.Count);

			game = mMatchingGames [randomIndex];

			return game;
    }

		/// <summary>
		/// Serialize into Json
		/// </summary>
		/// <returns>The json.</returns>
		public JsonValue ToJson ()
		{
			JsonObject json = new JsonObject ();

			json.Add (new KeyValuePair<string, JsonValue> ("Id", new JsonPrimitive (Id)));
			json.Add (new KeyValuePair<string, JsonValue> ("Name", new JsonPrimitive (Name)));
			json.Add (new KeyValuePair<string, JsonValue> ("Icon", new JsonPrimitive (Icon)));
			json.Add (new KeyValuePair<string, JsonValue> ("MinYear", new JsonPrimitive (MinYear)));
			json.Add (new KeyValuePair<string, JsonValue> ("MaxYear", new JsonPrimitive (MaxYear)));

			if (Publishers != null && Publishers.Count > 0) {
				JsonArray publishersJson = new JsonArray ();

				foreach (var publish in Publishers) {
					publishersJson.Add (publish);
				}

				json.Add ("Publishers", publishersJson);
			}
			if (Genres != null && Genres.Count > 0) {
				JsonArray genresJson = new JsonArray ();

				foreach (var genre in Genres) {
					genresJson.Add (genre);
				}

				json.Add ("Genres", genresJson);
			}
			if (Platforms != null && Platforms.Count > 0) {
				JsonArray platformsJson = new JsonArray ();
				
				foreach (var platform in Platforms) {
					platformsJson.Add (platform);
				}
				
				json.Add ("Platforms", platformsJson);
			}
			if (RequiredGameIds != null && RequiredGameIds.Count > 0) {
				JsonArray requiredGameIdJson = new JsonArray ();
				
				foreach (var question in RequiredGameIds) {
					JsonObject questionJson = new JsonObject ();

					questionJson.Add (new KeyValuePair<string, JsonValue> ("GameId", new JsonPrimitive (question.Key)));

					JsonArray answersJson = new JsonArray ();

					for (int i=0; i<question.Value.Length; i++) {
						answersJson.Add (question.Value [i]);
					}

					questionJson.Add ("Answers", answersJson);

					requiredGameIdJson.Add (questionJson);
				}
				
				json.Add ("RequiredGameIds", requiredGameIdJson);
			}

			return json;
		}

		public void BuildFromJsonObject (JsonValue json)
		{
			Id = json ["Id"];
			Name = json ["Name"];
			Icon = json ["Icon"];

			MinYear = Convert.ToInt32 (json ["MinYear"].ToString ());
			MaxYear = Convert.ToInt32 (json ["MaxYear"].ToString ());

			if (json.ContainsKey ("Publishers")) {
				
				if (json ["Publishers"] is JsonArray) {
					Publishers = new List<string> ();
					
					foreach (var publisherJson in ((JsonArray)json["Publishers"])) {
						Publishers.Add (publisherJson);
					}
				}
			}

			if (json.ContainsKey ("Genres")) {
				
				if (json ["Genres"] is JsonArray) {
					Genres = new List<string> ();
					
					foreach (var genreJson in ((JsonArray)json["Genres"])) {
						Genres.Add (genreJson);
					}
				}
			}

			if (json.ContainsKey ("Platforms")) {
				
				if (json ["Platforms"] is JsonArray) {
					Platforms = new List<string> ();
					
					foreach (var platformJson in ((JsonArray)json["Platforms"])) {
						Platforms.Add (platformJson);
					}
				}
			}

			if (json.ContainsKey ("RequiredGameIds")) {
				
				if (json ["RequiredGameIds"] is JsonArray) {
					RequiredGameIds = new Dictionary<int, int[]> ();
					
					foreach (var jsonQuestion in ((JsonArray)json["RequiredGameIds"])) {

						int gameId = Convert.ToInt32 (jsonQuestion ["GameId"].ToString ());
						List<int> answers = new List<int> ();

						foreach (var jsonAnswer in ((JsonArray)jsonQuestion["Answers"])) {
							int answerGameId = Convert.ToInt32 (jsonAnswer.ToString ());
							answers.Add (answerGameId);
						}

						RequiredGameIds.Add (gameId, answers.ToArray());
					}
				}
			}


		}

		#region Properties

		/// <summary>
		/// Unique filter ID
		/// </summary>
		/// <value>The identifier.</value>
		public string Id  { get; private set; }

		/// <summary>
		/// Filter name
		/// </summary>
		/// <value>The name.</value>
		public string Name  { get; private set; }

		/// <summary>
		/// Filter icon to use
		/// </summary>
		/// <value>The icon.</value>
		public string Icon  { get; private set; }

		/// <summary>
		/// Minimal year for a game
		/// </summary>
		/// <value>The minimum year.</value>
		public int MinYear { get; private set; }

		/// <summary>
		/// Maximal year for a game
		/// </summary>
		/// <value>The max year.</value>
		public int MaxYear { get; private set; }

		/// <summary>
		/// Specify pbulishers
		/// </summary>
		/// <value>The publisher.</value>
		public List<string> Publishers { get; private set; }

		/// <summary>
		/// Specify a genre
		/// </summary>
		/// <value>The genre.</value>
		public List<string> Genres { get; private set; }

		/// <summary>
		/// Specify a platform
		/// </summary>
		/// <value>The platform.</value>
		public List<string> Platforms { get; private set; }

		/// <summary>
		/// Define specific game ids and their answers
		/// </summary>
		/// <value>The game identifiers.</value>
		public Dictionary<int, int[]> RequiredGameIds { get; set; }

		#endregion
	}
}

