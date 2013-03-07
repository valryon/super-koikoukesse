using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Webservice de configuration
	/// </summary>
	public class WebserviceConfiguration : GenericWeberviceCaller<GameConfiguration>
	{
		public WebserviceConfiguration ()
		{
		}

		public override Uri GetServiceUrl ()
		{
			throw new NotImplementedException ();
		}

	}
}

