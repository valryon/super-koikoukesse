using System;
using System.Json;
using System.Collections.Generic;

namespace Superkoikoukesse.Common
{
	public class ModeConfigurationItem
	{
		public GameModes Mode { get; set; }

		public GameDifficulty Difficulty { get; set; }

		public int Score{ get; set; }

		public float Time { get; set; }
	}

	public class GameConfiguration : IServiceOutput
	{
		public List<ModeConfigurationItem> ModesConfiguration { get; set; }
		public List<KeyValuePair<string,string>> Properties { get; set; }

		public GameConfiguration ()
		{
			ModesConfiguration = new List<ModeConfigurationItem> ();
			Properties = new List<KeyValuePair<string, string>> ();
		}

		public void BuildFromJsonObject (JsonValue json)
		{
			// Example
			//{{"Id": "f355f650-d2d1-4981-810d-a17a02145187", "ModesConfiguration": [{"Mode": 0, "Difficulty": 0, "Time": 20, "Score": 100, "QuestionsCount": 4}, {"Mode": 0, "Difficulty": 1, "Time": 10, "Score": 200, "QuestionsCount": 5}, {"Mode": 0, "Difficulty": 2, "Time": 10, "Score": 200, "QuestionsCount": 8}, {"Mode": 0, "Difficulty": 3, "Time": 4, "Score": 4200, "QuestionsCount": 42}, {"Mode": 2, "Difficulty": 0, "Time": 60, "Score": 100}], "Properties": [{"Key": "Other prop", "Value": "10", "Help": "Juste un test 1", "Target": 0}, {"Key": "Other prop", "Value": "10", "Help": "Juste un test 2", "Target": 0}]}}

			// Get last update time

			// Parse "ModesConfiguration"

			// Parse other "Properties"
		}
	}
}

