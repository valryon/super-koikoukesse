using MongoDB.Driver;
using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.DB
{
    /// <summary>
    /// Manage players
    /// </summary>
    public class PlayersDb : ModelDb<Player>
    {
        public PlayersDb()
            : base("Players")
        {
        }
    }
}
