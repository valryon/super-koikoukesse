using Pixelnest.Common.Mongo;
using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.DB
{
    public class StatsDb : ModelDb<GameStats>
    {
        public StatsDb()
            : base("stats")
        {
        }
    }
}
