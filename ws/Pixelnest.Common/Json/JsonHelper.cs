using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pixelnest.Common.Json
{
    /// <summary>
    /// Useful methods for Json serialization
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Serialize an objet into a Json string
        /// </summary>
        /// <param name="data"></param>
        /// <param name="indented"></param>
        /// <returns></returns>
        public static string Serialize(object data, bool indented = false)
        {
            string json = JsonConvert.SerializeObject(data, (indented ? Formatting.Indented : Formatting.None)
                , new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
            );

            return json;
        }

        /// <summary>
        /// Deserialize a Json string into an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            object obj = JsonConvert.DeserializeObject(json, typeof(T)
                , new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
            );

            if (obj is T)
            {
                return (T)obj;
            }

            return default(T);
        }
    }
}
