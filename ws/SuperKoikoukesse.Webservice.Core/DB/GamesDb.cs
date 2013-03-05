using MongoDB.Driver;
using Pixelnest.Common.Log;
using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SuperKoikoukesse.Webservice.Core.DB
{
    /// <summary>
    /// Access to the game database
    /// </summary>
    public class GamesDb
    {
        public GamesDb()
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

        private MongoCollection<Game> m_gameDb;
        private MongoCollection<Game> getGameDb()
        {
            if (m_gameDb == null)
            {
                m_gameDb = MongoDbService.Instance.Get<Game>("GameInfo"); ;
            }
            return m_gameDb;
        }

        /// <summary>
        /// Get all available games
        /// </summary>
        /// <returns></returns>
        public List<Game> ReadAll()
        {
            var gamesDb = getGameDb();

            return gamesDb.FindAll().OrderBy(g => g.GameId).ToList();
        }

        /// <summary>
        /// Add a new game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public void Add(Game game)
        {
            var gamesDb = getGameDb();

            gamesDb.Insert(game);
        }

        /// <summary>
        /// Insert games collection
        /// </summary>
        /// <param name="games"></param>
        public void AddAll(List<Game> games)
        {
            var gamesDb = getGameDb();

            gamesDb.InsertBatch(games);
        }

        /// <summary>
        /// Delete all entries
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public void DeleteAll()
        {
            var gamesDb = getGameDb();

            gamesDb.RemoveAll();
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
                var gamesDb = getGameDb();

                gamesDb.RemoveAll();

                gamesDb.InsertBatch(m_backUp);
            }
        }
    }
}
