using SuperKoikoukesse.Webservice.Core.Model;
using System.Collections.Generic;

namespace SuperKoikoukesse.Webservice.ViewModels
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