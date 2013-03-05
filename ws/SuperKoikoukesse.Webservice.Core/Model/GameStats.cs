using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperKoikoukesse.Webservice.Core.Model
{
    public class GameStats
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        public string Difficulty { get; set; }

        public string Mode { get; set; }

        public DateTime Date { get; set; }

        public Dictionary<int, bool> AnswersStats { get; set; }

        public GameStats()
        {
            AnswersStats = new Dictionary<int, bool>();
        }
    }
}
