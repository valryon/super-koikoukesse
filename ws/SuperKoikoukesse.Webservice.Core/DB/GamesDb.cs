using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Pixelnest.Common.Log;
using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MongoDB.Driver.Linq;
using Pixelnest.Common.Mongo;

namespace SuperKoikoukesse.Webservice.Core.DB
{
    /// <summary>
    /// Access to the game database
    /// </summary>
    public class GamesDb : ModelDb<Game>
    {
        public GamesDb()
            : base("Games")
        {
        }

        /// <summary>
        /// Export database as XML (ready to use in mobile solutions!)
        /// </summary>
        /// <returns></returns>
        public XDocument ExportXml()
        {
            XDocument doc = new XDocument();
            XElement root = new XElement("games");

            foreach (Game game in ReadAll())
            {
                root.Add(game.ToXml());
            }

            doc.Add(root);

            return doc;
        }

        /// <summary>
        /// Change state of an entry
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="isRemoved"></param>
        public void SetRemoved(int gameId, bool isRemoved)
        {
            var db = GetDb();

            Game game = (from e in db.AsQueryable<Game>()
            where e.GameId == gameId
            select e).FirstOrDefault();

            if (game != null)
            {
                game.IsRemoved = isRemoved;

                Update(game);
            }
        }

        /// <summary>
        /// Get all available games
        /// </summary>
        /// <returns></returns>
        public List<Game> ReadAll()
        {
            return base.ReadAll().OrderBy(g => g.GameId).ToList();
        }

        private List<Game> m_backUp;

        public void Backup()
        {
            m_backUp = ReadAll();
        }

        public void Restorebackup()
        {
            if (m_backUp != null && m_backUp.Count > 0)
            {
                var gamesDb = GetDb();

                gamesDb.RemoveAll();

                gamesDb.InsertBatch(m_backUp);
            }
        }
    }
}
