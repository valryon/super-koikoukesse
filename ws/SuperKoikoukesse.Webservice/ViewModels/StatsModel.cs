using SuperKoikoukesse.Webservice.Core.Model;
using System.Collections.Generic;

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