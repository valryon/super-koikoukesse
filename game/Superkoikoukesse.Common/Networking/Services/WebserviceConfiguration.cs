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

		protected override GameConfiguration PostRequest (GameConfiguration newConfig)
		{
			// Compare downloaded config with local
			GameConfiguration localConfig = loadConfigurationFromDevice ();

			bool saveNewConfig = true;

			if (localConfig != null) {

				// Update local if necessary
				if (localConfig.LastUpdate >= newConfig.LastUpdate) {
					saveNewConfig = false;
				}
			} 

			if(saveNewConfig){
				saveConfigurationOnDevice(newConfig);
			}

			// Return the config to use
			return newConfig;
		}

		private void saveConfigurationOnDevice(GameConfiguration config) {
		}

		private GameConfiguration loadConfigurationFromDevice() {
			return null;
		}

		public override Uri GetServiceUrl ()
		{
			// TODO Android

			return new Uri(Constants.WebserviceUrl + "ws/config/"+ (int)GameTargets.iOS);
		}

	}
}

