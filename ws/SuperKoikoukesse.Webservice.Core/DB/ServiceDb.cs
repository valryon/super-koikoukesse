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

        private string m_databaseLocation;
        //private DBreezeEngine m_db;

        /// <summary>
        /// Initialize and load the database
        /// </summary>
        /// <param name="location"></param>
        public void Initialize(string location)
        {
            m_databaseLocation = location;

             // Initialize db connection
            Logger.Log(LogLevel.Info, "Loading database... " + location);

            bool exists = true; // TODO Tester existence ?
            if (exists)
            {
                Logger.Log(LogLevel.Info, "Database loaded.");
            }
            else
            {
                Logger.Log(LogLevel.Warning, "Database not found.");

                // First time: initialize

            }
        }

    }
}
