using MongoDB.Driver;
using Pixelnest.Common.Log;
using SuperKoikoukesse.Webservice.Core.Games;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;



namespace SuperKoikoukesse.Webservice.Core.DB
{
    /// <summary>
    /// Database wrapper
    /// </summary>
    public class ServiceDb
    {
        #region Singleton

        private static ServiceDb m_instance;

        public ServiceDb()
        {

        }

        public static ServiceDb Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ServiceDb();
                }

                return m_instance;
            }
        }

        #endregion

        private MongoClient m_mongoClient;
        private MongoServer m_mongoServer;
        private MongoDatabase m_mongoDb;

        /// <summary>
        /// Connect to the database
        /// </summary>
        public void Initialize(string connectionString)
        {
            // Initialize db connection
            Logger.Log(LogLevel.Info, "Connecting to database... ");
            Logger.Log(LogLevel.Debug, "ConnectionString=" + connectionString);
            try
            {
                var connectionStringMongo = new MongoConnectionStringBuilder(connectionString);
                m_mongoClient = new MongoClient(connectionString);
                m_mongoServer = m_mongoClient.GetServer();

                m_mongoDb = m_mongoServer.GetDatabase(connectionStringMongo.DatabaseName);
                Logger.Log(LogLevel.Info, "Using database " + connectionStringMongo.DatabaseName);

                // Dumb command to test connection
                m_mongoDb.GetStats();    
            }
            catch (MongoConnectionException e)
            {
                Logger.LogException(LogLevel.Error, "ServiceDb.Initialize", e);
                throw new ApplicationException("Database connection failed!");
            }
            //var collection = db.GetCollection<Post>("post");

        }

    }
}
