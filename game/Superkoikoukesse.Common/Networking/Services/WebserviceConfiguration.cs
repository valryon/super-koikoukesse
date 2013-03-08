using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Webservice de configuration
	/// </summary>
	public class WebserviceConfiguration : GenericModelWeberviceCaller
	{
		public WebserviceConfiguration ()
		{
		}

		protected override GameConfiguration PostRequest (GameConfiguration parsedObject)
		{
			// Compare downloaded config with local

			// Update local if necessary

			// Return the config
			return parsedObject;
		}

		public override Uri GetServiceUrl ()
		{
			return new Uri(Constants.WebserviceUrl + "ws/config");
		}

	}
}

