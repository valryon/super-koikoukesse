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

		private List<GameInfo> matchingGames;

		public Filter (JsonValue json)
		{
			BuildFromJsonObject (json);
		}

		public Filter (string id, string name, string icon, int minYear = 0, int maxYear = 9999, List<string> publishers = null, List<string> genres = null, List<string> platforms = null)
		{
			Id = id;
			Name = name;
			Icon = icon;
			MinYear = minYear;
			MaxYear = maxYear;

			Publishers = publishers;
			Genres = genres;
			Platforms = platforms;
		}

		/// <summary>
		/// Caching database
		/// </summary>
		/// <param name="loadingComplete">Loading complete for n games.</param>
		public void Load (Action<int> loadingComplete)
		{
			BackgroundWorker worker = new BackgroundWorker ();

			worker.DoWork += (object sender, DoWorkEventArgs e) => {

				matchingGames = DatabaseService.Instance.ReadGames (MinYear, MaxYear, Publishers, Genres, Platforms);

				loadingComplete (matchingGames.Count);

			};

			worker.RunWorkerAsync ();
		}

		/// <summary>
		/// Get a game corresponding to this filter
		/// </summary>
		/// <returns>The games.</returns>
		/// <param name="count">Count.</param>
		public GameInfo GetGame ()
		{
			Logger.Log(LogLevel.Debug, "Petit enculé 2");
			// Random game
			var random = new Random (DateTime.Now.Millisecond);
			int randomIndex = random.Next (matchingGames.Count);

			Logger.Log(LogLevel.Debug, "Petit enculé 1");
			return matchingGames [randomIndex];
		}

		/// <summary>
		/// Load a predefined filter for Stunfest
		/// </summary>
		public void StunfestMode ()
		{
			if (Genres != null) {
				Genres.Clear ();
			} else {
				Genres = new List<string> ();
			}
			Genres.Add ("combat");
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
		}

	}
}

