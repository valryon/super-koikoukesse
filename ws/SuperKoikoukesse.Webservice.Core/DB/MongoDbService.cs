using MongoDB.Driver;
using Pixelnest.Common.Log;
using SuperKoikoukesse.Webservice.Core.Model;
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
    public class MongoDbService
    {
        #region Singleton

        private static MongoDbService m_instance;

        public MongoDbService()
        {

        }

        public static MongoDbService Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new MongoDbService();
                }
                else
                {
                    // Try to reconnect to the database if it fails once.
                    if (string.IsNullOrEmpty(connectionString) == false)
                    {
                        if (m_instance.IsConnected == false)
                        {
                            m_instance.databaseConnection();
                        }
                    }
                }

                return m_instance;
            }
        }

        #endregion

        private static string connectionString;

        private MongoClient m_mongoClient;
        private MongoServer m_mongoServer;
        private MongoDatabase m_mongoDb;

        /// <summary>
        /// Connect to the database
        /// </summary>
        public void Initialize(string aConnectionString)
        {
            connectionString = aConnectionString;

            // Initialize db connection
            databaseConnection();

            //var collection = db.GetCollection<Post>("post");

        }

        private void databaseConnection()
        {
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

                Logger.Log(LogLevel.Info, "Connection to database successful!");

                IsConnected = true;
            }
            catch (MongoConnectionException e)
            {
                IsConnected = false;
                Logger.LogException(LogLevel.Error, "ServiceDb.Initialize", e);
                throw new ApplicationException("Database connection failed!");
            }
        }

        /// <summary>
        /// Get collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public MongoCollection<T> Get<T>(string name)
        {
            return m_mongoDb.GetCollection<T>(name);
        }

        /// <summary>
        /// Database connection state
        /// </summary>
        public bool IsConnected { get; private set; }

    }
}
