using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperKoikoukesse.Webservice.Models
{
    public class GameDatabaseModel
    {
        public List<Game> Games { get; set; }

        public int Page { get; set; }

        public int MaxPage { get; set; }

        public GameDatabaseModel()
        {
            Games = new List<Game>();
        }
    }
}