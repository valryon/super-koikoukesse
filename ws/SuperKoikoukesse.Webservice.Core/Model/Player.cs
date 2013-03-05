using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Model
{
    /// <summary>
    /// Information on the player
    /// </summary>
    public class Player
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        public string GameCenterId { get; set; }

        public string NickName { get; set; }

        public DateTime CreationDate { get; set; }

        public int Credits { get; set; }

        public long Coins { get; set; }

        public int SubscriptionType { get; set; }
    }
}
