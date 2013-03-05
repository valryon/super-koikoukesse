using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperKoikoukesse.Webservice.Models
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