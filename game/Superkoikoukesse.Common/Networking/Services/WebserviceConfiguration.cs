using System;
using System.Xml.Serialization;
using System.IO;

namespace Superkoikoukesse.Common
{
	/// <summary>
	/// Webservice for configuration
	/// </summary>
	public class WebserviceConfiguration : GenericModelWeberviceCaller<GameConfiguration>
	{
		public WebserviceConfiguration ()
		{
		}

		public GameConfiguration LastValidConfig { get; set; }

		protected override GameConfiguration PostRequest (GameConfiguration newConfig, bool success)
		{
			// Compare downloaded config with local
			GameConfiguration localConfig = loadConfigurationFromDevice ();

			LastValidConfig = localConfig;

			if (success) {
				bool saveNewConfig = true;

				if (localConfig != null) {

					// Update local if necessary
					if (localConfig.LastUpdate >= newConfig.LastUpdate) {
						saveNewConfig = false;
					}
				} 

				if (saveNewConfig) {
					saveConfigurationOnDevice (newConfig);
				}

				// Return the config to use
				return newConfig;
			} else {
				// Any error: use local file
				return localConfig;
			}
		}

		private void saveConfigurationOnDevice (GameConfiguration config)
		{
			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

			var filePath = Path.Combine (path, Constants.ConfigFileLocation);
		
			Logger.Log (LogLevel.Info, "Saving configuration...");

			XmlSerializer serializer = new XmlSerializer (typeof(GameConfiguration));

			using (TextWriter writer = new StreamWriter (filePath)) {
			
				serializer.Serialize (writer, config); 
			}
		}

		private GameConfiguration loadConfigurationFromDevice ()
		{
			GameConfiguration config = null;

			var path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			
			var filePath = Path.Combine (path, Constants.ConfigFileLocation);

			if (File.Exists (filePath)) {

				Logger.Log (LogLevel.Info, "Loading configuration...");

				XmlSerializer serializer = new XmlSerializer (typeof(GameConfiguration));
				using (TextReader reader = new StreamReader (filePath)) {
					
					config = (GameConfiguration)serializer.Deserialize (reader); 
				}
			} else {
				Logger.Log (LogLevel.Info, "No configuration file found.");
			}

			return config;
		}

		public override Uri GetServiceUrl ()
		{
			// TODO Android

			return new Uri (Constants.WebserviceUrl + "ws/config/" + (int)GameTargets.iOS);
		}

	}
}

