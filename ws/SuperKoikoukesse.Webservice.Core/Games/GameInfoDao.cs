using Pixelnest.Common.Log;
using SuperKoikoukesse.Webservice.Core.Games;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SuperKoikoukesse.Webservice.Core.Dao
{
    /// <summary>
    /// Access to the game database
    /// </summary>
    public class GameInfoDao
    {
        private string m_databaseLocation;
        private XDocument m_document;

        public GameInfoDao(string databaseLocation)
        {
            m_databaseLocation = databaseLocation;

            // Read the database
            if (File.Exists(databaseLocation))
            {
                IsNew = false;
                Logger.Log(LogLevel.Info, "Loading database " + databaseLocation);
                m_document = XDocument.Load(databaseLocation);
            }
            else
            {
                IsNew = true;
                Logger.Log(LogLevel.Info, "Creating database " + databaseLocation);

                Initialize();
            }
        }

        /// <summary>
        /// Called when the database is created the first time
        /// </summary>
        public void Initialize()
        {
            m_document = new XDocument();
            m_document.Add(new XElement("games"));
        }

        /// <summary>
        /// Get all available games
        /// </summary>
        /// <returns></returns>
        public List<GameInfo> ReadAll()
        {
            List<GameInfo> results = new List<GameInfo>();

            // Create objects from XML
            foreach (XElement gameElement in m_document.Root.Elements())
            {
                try
                {
                    GameInfo game = new GameInfo();
                    game.FromXml(gameElement);

                    results.Add(game);
                }
                catch (Exception e)
                {
                    Logger.LogException(LogLevel.Warning, "GameInfoDao.ReadAll", e);
                }
            }

            return results;
        }

        /// <summary>
        /// Add a new game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public void Add(GameInfo game)
        {
            m_document.Root.Add(game.ToXml());
        }

        /// <summary>
        /// Add a new game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public void DeleteAll()
        {
            m_document.Root.RemoveNodes();
        }


        /// <summary>
        /// Save the game database
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool Save()
        {
            m_document.Save(m_databaseLocation);

            IsNew = false;
            return true;
        }

        public bool IsNew { get; private set; }
    }
}
