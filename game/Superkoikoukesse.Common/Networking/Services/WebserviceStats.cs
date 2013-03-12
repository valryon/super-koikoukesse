using System;
using Superkoikoukesse.Common.Networking;
using System.Collections.Generic;
using System.Json;

namespace Superkoikoukesse.Common
{
	public class WebserviceStats : BaseWebserviceCaller
	{
		public WebserviceStats ()
		{
		}

		public void SendStats (string playerId, int score, GameModes mode, GameDifficulty difficulty, DateTime date, List<KeyValuePair<int, bool>> answers, Action<Exception> callbackFailure)
		{
			var jsonBody = buildJson (playerId, score, mode, difficulty, date, answers);
		
			RequestPostJsonAsync (jsonBody, null, callbackFailure);
		}

		private string buildJson (string playerId, int score, GameModes mode, GameDifficulty difficulty, DateTime date, List<KeyValuePair<int, bool>> answers)
		{
//			{
//			“player”:”Varyon”,
//			“score”:10000,
//			“difficulty”:0,
//			“mode”:0,
//			“date”:”2012-03-07 23:15:00”,
//			“answers”: [
//				           {“id” : 002, “result” : false},
//				           {“id” : 003, “result” : true},
//				            ]
//			}

			JsonObject json = new JsonObject ();

			json.Add ("player", new JsonPrimitive (playerId));
			json.Add ("score", new JsonPrimitive (score));
			json.Add ("mode", new JsonPrimitive ((int)mode));
			json.Add ("difficulty", new JsonPrimitive ((int)difficulty));
			json.Add ("date", new JsonPrimitive (date.ToString ("yyyy-MM-dd hh:mm:ss")));

			List<JsonValue> answersItemsJson = new List<JsonValue> ();
			foreach (var answer in answers) {

				JsonObject o = new JsonObject();
				o.Add("id", new JsonPrimitive(answer.Key));
				o.Add("result", new JsonPrimitive(answer.Value));

				answersItemsJson.Add (o);
			}

			JsonArray answersJson = new JsonArray (answersItemsJson);
			json.Add ("answers", answersJson);

			return json.ToString ();
		}

		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WebserviceUrl + "ws/stats/add");
		}


	}
}

