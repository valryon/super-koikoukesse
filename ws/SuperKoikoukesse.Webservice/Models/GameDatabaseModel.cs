using SuperKoikoukesse.Webservice.Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperKoikoukesse.Webservice.Models
{
    public class GameDatabaseModel
    {
        public List<GameInfo> Games { get; set; }

        public int Page { get; set; }

        public int MaxPage { get; set; }

        public GameDatabaseModel()
        {
            Games = new List<GameInfo>();
        }
    }
}