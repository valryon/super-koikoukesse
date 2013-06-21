using SuperKoikoukesse.Webservice.Core.Model;
using System.Collections.Generic;

namespace SuperKoikoukesse.Webservice.ViewModels
{
    public class PlayersModel
    {
        public List<Player> Players { get; set; }

        public PlayersModel()
        {
            Players = new List<Player>();
        }
    }
}