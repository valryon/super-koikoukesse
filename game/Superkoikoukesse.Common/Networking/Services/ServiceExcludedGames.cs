// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
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

