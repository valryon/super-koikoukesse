using System;
using System.Collections.Generic;
using System.Json;

namespace Superkoikoukesse.Common
{
	public class ServiceExcludedGames : BaseModelServiceCaller<ExcludedGamesList>
	{
		public ServiceExcludedGames ()
			: base()
		{
		}

		public override Uri GetServiceUrl ()
		{
			return new Uri (Constants.WEBSERVICE_URL + "questions/ex.json");
		}
	}
}

