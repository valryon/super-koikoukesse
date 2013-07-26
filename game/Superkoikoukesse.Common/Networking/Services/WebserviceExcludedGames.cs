using System;
using System.Collections.Generic;
using System.Json;

namespace Superkoikoukesse.Common
{
	public class ExcludedGamesList : IServiceOutput
	{
		public List<int> GamesId {get;set;}

		public ExcludedGamesList() {
			GamesId = new List<int>();
		}

		public void BuildFromJsonObject (System.Json.JsonValue json)
		{
			foreach (JsonValue c in json) {

				try {
					int id = Convert.ToInt32(c.ToString());

					GamesId.Add (id);
				}
				catch(Exception e) {
					Logger.LogException(LogLevel.Error, "ExcludedGamesList.BuildFromJsonObject",e);
				}

			}
		}

	}

	public class WebserviceExcludedGames : GenericModelWeberviceCaller<ExcludedGamesList>
	{
		public WebserviceExcludedGames ()
			: base()
		{
		}

		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WEBSERVICE_URL + "ws/games/exclusions");
		}
	}
}

