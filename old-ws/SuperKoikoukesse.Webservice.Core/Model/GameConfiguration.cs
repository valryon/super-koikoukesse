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
            ModesConfiguration.Clear();
            Properties.Clear();

            foreach (ModesEnum modes in Enum.GetValues(typeof(ModesEnum)))
            {
                foreach (DifficultiesEnum diff in Enum.GetValues(typeof(DifficultiesEnum)))
                {
                    ModeConfiguration c = new ModeConfiguration();

                    c.Mode = modes;
                    c.Difficulty = diff;

                    c.Score = 100 * (((int)diff) + 1);

                    if (modes == ModesEnum.Survival)
                    {
                        c.Lives = 3;
                        c.Time = 10;
                    }
                    else if (modes == ModesEnum.TimeAttack)
                    {
                        c.Time = 60;
                    }
                    else if (modes == ModesEnum.ScoreAttack)
                    {
                        c.QuestionsCount = 20;
                        c.Time = 10;
                    }
                    else if (modes == ModesEnum.Versus)
                    {
                        c.QuestionsCount = 5;
                        c.Time = 10;
                    }

                    ModesConfiguration.Add(c);
                }
            }

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
        public int? Lives { get; set; }
    }
}
