using MongoDB.Driver;
using MongoDB.Driver.Builders;
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

        /// <summary>
        /// Create a new player
        /// </summary>
        /// <param name="gameCenterId"></param>
        /// <returns></returns>
        public Player CreatePlayer(string gameCenterId)
        {
            Player p = new Player();
            p.CreationDate = DateTime.Now;
            p.Coins = 2000; // TODO Valeurs
            p.Credits = 5; // TODO Valeurs
            p.GameCenterId = gameCenterId;
            p.NickName = gameCenterId;
            p.SubscriptionType = 0;

            Add(p);

            return p;
        }

        /// <summary>
        /// Get the player (or null if it doesn't exists)
        /// </summary>
        /// <param name="gameCenterId"></param>
        /// <returns></returns>
        public Player GetPlayer(string gameCenterId)
        {
            var db = GetDb();

            Player player = db.FindOneAs<Player>(Query<Player>.EQ(p => p.GameCenterId, gameCenterId));

            return player;
        }
    }
}
