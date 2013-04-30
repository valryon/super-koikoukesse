using System;
using System.Collections.Generic;
using System.Json;

namespace Superkoikoukesse.Common
{
	public class VersusMatchTurn
	{
		public int Score { get; set; }

		public string PlayerId { get; set; }
	}

	/// <summary>
	/// Match data for multiplayer mode
	/// </summary>
	public class VersusMatch
	{
		public string MatchId { get; set; }

		public Filter Filter { get; set; }

		public string Player1Id { get; set; }

		public string Player2Id { get; set; }

		public List<VersusMatchTurn>  Turns  { get; set; }

		public VersusMatch ()
		{
			Turns = new List<VersusMatchTurn> ();
		}

		/// <summary>
		/// Serialize the match data in json
		/// </summary>
		/// <returns>The json.</returns>
		public string ToJson ()
		{

			JsonObject json = new JsonObject ();

			json.Add (new KeyValuePair<string, JsonValue> ("MatchId", new JsonPrimitive (MatchId)));
			json.Add (new KeyValuePair<string, JsonValue> ("Player1Id", new JsonPrimitive (Player1Id)));
			json.Add (new KeyValuePair<string, JsonValue> ("Player2Id", new JsonPrimitive (Player2Id)));
			json.Add (new KeyValuePair<string, JsonValue> ("Filter", Filter.ToJson ()));

			JsonArray turnsJson = new JsonArray ();
			foreach (VersusMatchTurn turn in Turns) {
				JsonObject turnJson = new JsonObject ();

				turnJson.Add (new KeyValuePair<string, JsonValue> ("PlayerId", new JsonPrimitive (turn.PlayerId)));
				turnJson.Add (new KeyValuePair<string, JsonValue> ("Score", new JsonPrimitive (turn.Score)));

				turnsJson.Add (turnJson);
			}

			json.Add (new KeyValuePair<string, JsonValue> ("Turns", turnsJson));

			return json.ToString ();

		}

		/// <summary>
		/// Read match json and fill the current object
		/// </summary>
		public void FromJson (string jsonRaw)
		{
			JsonValue json = JsonObject.Parse(jsonRaw);

			MatchId = (json ["MatchId"].ToString ());
			Player1Id = (json ["Player1Id"].ToString ());
			Player2Id = (json ["Player2Id"].ToString ());

			Filter = new Filter ();
			Filter.FromJson (json ["Filter"].ToString ());

			Turns.Clear ();
			if (json ["Turns"] != null && json ["Turns"] is JsonArray) {
				foreach (JsonObject turnJson in ((JsonArray)json["Turns"])) {

					int score = Convert.ToInt32 (turnJson ["Score"].ToString ());
					string playerId = turnJson ["PlayerId"].ToString ();

					Turns.Add (new VersusMatchTurn () {
					PlayerId = playerId,
					Score = score
				});
				}
			}
		}
	}
}

