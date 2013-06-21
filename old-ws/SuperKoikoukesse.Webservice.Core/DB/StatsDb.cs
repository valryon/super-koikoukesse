using Pixelnest.Common.Mongo;
using SuperKoikoukesse.Webservice.Core.Model;

namespace SuperKoikoukesse.Webservice.Core.DB
{
    public class StatsDb : ModelDb<GameStats>
    {
        public StatsDb()
            : base("Stats")
        {
        }
    }
}
