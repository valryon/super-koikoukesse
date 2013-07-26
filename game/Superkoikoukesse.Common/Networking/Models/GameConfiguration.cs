using System;
using System.Json;
using System.Collections.Generic;
using System.Globalization;

namespace Superkoikoukesse.Common
{
	[Serializable]
	public class PropertyConfigurationItem
	{
		public PropertyConfigurationItem (string k, string v)
		{
			Key = k;
			Value = v;
		}

		public PropertyConfigurationItem ()
		{
		}

		public string Key { get; set; }

		public string Value { get; set; }
	}

	[Serializable]
	public class ModeConfigurationItem
	{
		public GameModes Mode { get; set; }

		public GameDifficulties Difficulty { get; set; }

		public int? Score{ get; set; }

		public int? Time { get; set; }

		public int? QuestionCount { get; set; }
	}

	[Serializable]
	public class GameConfiguration : IServiceOutput
	{
		public List<ModeConfigurationItem> ModesConfiguration { get; set; }

		public List<PropertyConfigurationItem> Properties { get; set; }

		public GameConfiguration ()
		{
			ModesConfiguration = new List<ModeConfigurationItem> ();
			Properties = new List<PropertyConfigurationItem> ();
		}

		/// <summary>
		/// Gets the mode configuration.
		/// </summary>
		/// <returns>The mode configuration.</returns>
		/// <param name="mode">Mode.</param>
		/// <param name="difficulty">Difficulty.</param>
		public ModeConfigurationItem GetModeConfiguration (GameModes mode, GameDifficulties difficulty)
		{
			foreach (var config in ModesConfiguration) {
				if (config.Mode == mode && config.Difficulty == difficulty) {
					return config;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <returns>The property value.</returns>
		/// <param name="key">Key.</param>
		public string GetPropertyValue (string key)
		{
			foreach (var prop in Properties) {
				if (prop.Key.ToLower () == key.ToLower ()) {
					return prop.Value;
				}
			}

			return null;
		}

		/// <summary>
		/// Parse Json config
		/// </summary>
		/// <param name="json">Json.</param>
		public void BuildFromJsonObject (JsonValue json)
		{
			// Example
			//{{"Id": "f355f650-d2d1-4981-810d-a17a02145187", "ModesConfiguration": [{"Mode": 0, "Difficulty": 0, "Time": 20, "Score": 100, "QuestionsCount": 4}, {"Mode": 0, "Difficulty": 1, "Time": 10, "Score": 200, "QuestionsCount": 5}, {"Mode": 0, "Difficulty": 2, "Time": 10, "Score": 200, "QuestionsCount": 8}, {"Mode": 0, "Difficulty": 3, "Time": 4, "Score": 4200, "QuestionsCount": 42}, {"Mode": 2, "Difficulty": 0, "Time": 60, "Score": 100}], "Properties": [{"Key": "Other prop", "Value": "10", "Help": "Juste un test 1", "Target": 0}, {"Key": "Other prop", "Value": "10", "Help": "Juste un test 2", "Target": 0}]}}

			// Parse "ModesConfiguration"
			if (json.ContainsKey ("ModesConfiguration")) {
				JsonValue modesConfig = json ["ModesConfiguration"];

				if (modesConfig is JsonArray) {
					foreach (JsonValue c in modesConfig) {

						// Parsing each game mode configuration
						GameModes mode = GameModes.SCORE_ATTACK;
						GameDifficulties difficulty = GameDifficulties.NORMAL;
						int? time = null;
						int? score = null;
						int? questionCount = null;

						mode = (GameModes)Enum.Parse (typeof(GameModes), c ["Mode"].ToString ());
						difficulty = (GameDifficulties)Enum.Parse (typeof(GameDifficulties), c ["Difficulty"].ToString ());

						if (c.ContainsKey ("Time")) {
							time = Convert.ToInt32 (c ["Time"].ToString ());
						}
						if (c.ContainsKey ("Score")) {
							score = Convert.ToInt32 (c ["Score"].ToString ());
						}
						if (c.ContainsKey ("QuestionsCount")) {
							questionCount = Convert.ToInt32 (c ["QuestionsCount"].ToString ());
						}

						ModeConfigurationItem modeConfig = new ModeConfigurationItem () {
						Mode = mode,
						Difficulty = difficulty,
						Time = time,
						Score = score,
						QuestionCount = questionCount
					};

						ModesConfiguration.Add (modeConfig);
					}
				}
			}

			// Parse other "Properties"
			if (json.ContainsKey ("Properties")) {
				JsonValue properties = json ["Properties"];

				if (properties is JsonArray) {
					foreach (JsonValue p in properties) {
						string key = p ["Key"].ToString ();
						string value = p ["Value"].ToString ();

						Properties.Add (new PropertyConfigurationItem (key, value));
					}
				}
			}
		}
	}
}

