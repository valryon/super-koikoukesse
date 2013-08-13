using System;
using System.Xml.Serialization;
using System.IO;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Webservice for configuration
	/// </summary>
	public class ServiceConfiguration : BaseModelServiceCaller<GameConfiguration>
	{
		public ServiceConfiguration ()
		{
			// Compare downloaded config with local
			GameConfiguration localConfig = loadConfigurationFromDevice ();
			
			LastValidConfig = localConfig;
		}

		protected override GameConfiguration PostRequest (GameConfiguration newConfig, bool success)
		{
			if (success) {

				// TODO diff between the last and the new to see if we have to save
				// the new config
				// Maybe it's simpler to just save it all the time...
				//if (LastValidConfig != null) {
				saveConfigurationOnDevice (newConfig);
				//}

				// Return the config to use
				return newConfig;
			} else {
				// Any error: use local file
				return LastValidConfig;
			}
		}

		private void saveConfigurationOnDevice (GameConfiguration config)
		{
			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

			var filePath = Path.Combine (path, Constants.CONFIG_FILE_LOCATION);

			Logger.I ("Saving configuration...");

			XmlSerializer serializer = new XmlSerializer (typeof(GameConfiguration));

			using (TextWriter writer = new StreamWriter (filePath)) {

				serializer.Serialize (writer, config); 
			}
		}

		private GameConfiguration loadConfigurationFromDevice ()
		{
			GameConfiguration config = null;

			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

			var filePath = Path.Combine (path, Constants.CONFIG_FILE_LOCATION);

			if (File.Exists (filePath)) {

				Logger.I( "Loading configuration...");

				try {
					XmlSerializer serializer = new XmlSerializer (typeof(GameConfiguration));
					using (TextReader reader = new StreamReader (filePath)) {
						config = (GameConfiguration)serializer.Deserialize (reader); 
					}
				} catch (Exception) {
					Logger.E("Error during config file load. Removing it from disk so next time it should be cleaned up.");
					File.Delete (filePath);
				}
			} else {
				Logger.I("No configuration file found.");
			}

			return config;
		}

		public override Uri GetServiceUrl ()
		{
			// TODO Android

			return new Uri (Constants.WEBSERVICE_URL + "config.json/" + Platforms.IOS.ToString ().ToLower () + "/1");
		}
		
		public GameConfiguration LastValidConfig { get; set; }

	}
}

