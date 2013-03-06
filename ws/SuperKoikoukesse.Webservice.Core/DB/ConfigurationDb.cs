using Pixelnest.Common.Mongo;
using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Linq;

namespace SuperKoikoukesse.Webservice.Core.DB
{
    /// <summary>
    /// Manage configuration saved in database as a singleton
    /// </summary>
    public class ConfigurationDb: ModelDb<GameConfiguration>
    {
        public ConfigurationDb()
            : base("Config")
        {
        }

        /// <summary>
        /// Get the singleton and create it if necessary.
        /// </summary>
        /// <param name="target">Filter parameters for target (0 = all)</param>
        /// <returns></returns>
        public GameConfiguration GetConfiguration(int target)
        {
            GameConfiguration config = ReadAll().FirstOrDefault();

            if (config == null)
            {
                config = new GameConfiguration();
                config.InitializeDefaultValues();
                Add(config);
            }

            return config;
        }

        public override void Update(GameConfiguration element)
        {
            element.LastUpdate = DateTime.Now;
            base.Update(element);
        }
    }
}
