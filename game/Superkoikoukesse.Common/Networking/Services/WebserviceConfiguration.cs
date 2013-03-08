using System;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Webservice de configuration
	/// </summary>
	public class WebserviceConfiguration : GenericModelWeberviceCaller<GameConfiguration>
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
			// TODO Android

			return new Uri(Constants.WebserviceUrl + "ws/config/"+ (int)GameTargets.iOS);
		}

	}
}

