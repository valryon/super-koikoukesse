// Copyright © 2013 Pixelnest Studio
// This file is subject to the terms and conditions defined in
// file 'LICENSE.md', which is part of this source code package.
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
		public GameMode Mode { get; set; }

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
		public ModeConfigurationItem GetModeConfiguration (GameMode mode, GameDifficulties difficulty)
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
			// { "version": 1, "last_edit": 2013-07-10T11:44:12+02:00, "config": "{ "modes": [ {"Mode": 0, "Difficulty": 0, "Time": 10, "Score": 100, "QuestionsCount": 20},}

			// Parse "config"
			if (json.ContainsKey ("config")) {
				JsonValue config = json ["config"];

				if (config.ContainsKey ("modes")) {
					JsonValue modesConfig = config ["modes"];

					if (modesConfig is JsonArray) {
						foreach (JsonValue c in modesConfig) {

							// Parsing each game mode configuration
							GameMode mode = GameMode.SCORE;
							GameDifficulties difficulty = GameDifficulties.NORMAL;
							int? time = null;
							int? score = null;
							int? questionCount = null;

							mode = (GameMode)Enum.Parse (typeof(GameMode), c ["Mode"].ToString ());
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
				if (json.ContainsKey ("properties")) {
					JsonValue properties = json ["properties"];

					if (properties is JsonArray) {
						foreach (JsonValue p in properties) {
							string key = p ["key"].ToString ();
							string value = p ["value"].ToString ();

							Properties.Add (new PropertyConfigurationItem (key, value));
						}
					}
				}
			}

		}
	}
}

