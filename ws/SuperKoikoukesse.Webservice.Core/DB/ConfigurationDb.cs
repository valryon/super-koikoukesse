using Pixelnest.Common.Mongo;
using SuperKoikoukesse.Webservice.Core.Model;

namespace SuperKoikoukesse.Webservice.Core.DB
{
    public class ConfigurationDb: ModelDb<Configuration>
    {
        public ConfigurationDb()
            : base("Config")
        {
        }
    
    }
}
