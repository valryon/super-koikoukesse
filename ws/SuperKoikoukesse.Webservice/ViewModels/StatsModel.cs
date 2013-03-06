using SuperKoikoukesse.Webservice.Core.DB;
using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperKoikoukesse.Webservice.ViewModels
{
    public class StatsModel
    {
        public List<GameStats> Stats { get; set; }

        public StatsModel()
        {
            Stats = new List<GameStats>();
        }
    }
}