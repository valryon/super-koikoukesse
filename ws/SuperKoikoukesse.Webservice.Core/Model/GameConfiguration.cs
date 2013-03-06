using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Model
{
    /// <summary>
    /// Configuration properties for the game
    /// </summary>
    public class GameConfiguration
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public List<ModeConfiguration> ModesConfiguration { get; set; }
        public List<ConfigurationItem> Properties { get; set; }

        public GameConfiguration()
        {
            ModesConfiguration = new List<ModeConfiguration>();
            Properties = new List<ConfigurationItem>();
        }

        public void InitializeDefaultValues()
        {
            ModesConfiguration.Add(new ModeConfiguration()
            {
                Mode = ModesEnum.ScoreAttack,
                Difficulty = DifficultiesEnum.Easy,
                QuestionsCount = 4,
                Score = 100,
                Time = 20,
            });

            ModesConfiguration.Add(new ModeConfiguration()
            {
                Mode = ModesEnum.ScoreAttack,
                Difficulty = DifficultiesEnum.Hard,
                QuestionsCount = 5,
                Score = 200,
                Time = 10,
            });

            ModesConfiguration.Add(new ModeConfiguration()
            {
                Mode = ModesEnum.ScoreAttack,
                Difficulty = DifficultiesEnum.Expert,
                QuestionsCount = 8,
                Score = 200,
                Time = 10,
            });

            ModesConfiguration.Add(new ModeConfiguration()
            {
                Mode = ModesEnum.ScoreAttack,
                Difficulty = DifficultiesEnum.Nolife,
                QuestionsCount = 42,
                Score = 4200,
                Time = 4,
            });

            ModesConfiguration.Add(new ModeConfiguration()
            {
                Mode = ModesEnum.Survival,
                Difficulty = DifficultiesEnum.Easy,
                Score = 100,
                Time = 60,
            });

            Properties.Add(new ConfigurationItem()
            {
                Key = "Other prop",
                Value = "10",
                Target = ConfigurationTargetEnum.All,
                Help = "Juste un test 1"
            });

            Properties.Add(new ConfigurationItem()
            {
                Key = "Other prop",
                Value = "10",
                Target = ConfigurationTargetEnum.All,
                Help = "Juste un test 2"
            });
        }
    }

    /// <summary>
    /// Some properties may aim only specific version
    /// </summary>
    public enum ConfigurationTargetEnum : int
    {
        All = 0,
        iOS = 1,
        Android = 2,
        WindowsPhone = 3
    }

    /// <summary>
    /// A configuration property
    /// </summary>
    public class ConfigurationItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Help { get; set; }
        public ConfigurationTargetEnum Target { get; set; }
    }

    public class ModeConfiguration
    {
        public ModesEnum Mode { get; set; }
        public DifficultiesEnum Difficulty { get; set; }
        public int? Time { get; set; }
        public int? Score { get; set; }
        public int? QuestionsCount { get; set; }
    }
}
