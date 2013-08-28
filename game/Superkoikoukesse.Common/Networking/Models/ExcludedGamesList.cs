// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
using System;
using System.Collections.Generic;
using System.Json;

namespace Superkoikoukesse.Common
{
	public class ExcludedGamesList : IServiceOutput
	{
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
					Logger.E("ExcludedGamesList.BuildFromJsonObject",e);
				}

			}
		}

		public List<int> GamesId {get;set;}

	}
}

