using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.DB
{
    /// <summary>
    /// Generic DAO
    /// </summary>
    public abstract class ModelDb<T>
    {
        private MongoCollection<T> m_db;
        protected string CollectionName;

        public ModelDb(string collectionName)
        {
            CollectionName = collectionName;
        }

        /// <summary>
        /// Get the associated collection
        /// </summary>
        /// <returns></returns>
        protected MongoCollection<T> GetDb()
        {
            if (m_db == null)
            {
                m_db = MongoDbService.Instance.Get<T>(CollectionName); ;
            }
            return m_db;
        }

        /// <summary>
        /// Get all entries
        /// </summary>
        /// <returns></returns>
        public virtual List<T> ReadAll(Func<T, bool> predicat = null)
        {
            var gamesDb = GetDb();

            if (predicat != null)
            {
                return gamesDb.FindAll().Where(T => predicat(T)).ToList();
            }
            else
            {
                return gamesDb.FindAll().ToList();
            }
        }


        /// <summary>
        /// Add a new T
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual void Add(T element)
        {
            var db = GetDb();

            db.Insert(element);
        }

        /// <summary>
        /// Insert collection
        /// </summary>
        /// <param name="elements"></param>
        public virtual void AddAll(List<T> elements)
        {
            var db = GetDb();
            
            db.InsertBatch(elements);
        }


        /// <summary>
        /// Update element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual void Update(T element)
        {
            var db = GetDb();
            db.Save(element);
        }

        /// <summary>
        /// Delete all entries
        /// </summary>
        /// <returns></returns>
        public virtual void DeleteAll()
        {
            var db = GetDb();

            db.RemoveAll();
        }
    }
}
