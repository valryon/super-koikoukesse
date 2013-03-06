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

        public Guid PlayerId { get; set; }

        public int Score { get; set; }

        public DifficultiesEnum Difficulty { get; set; }

        public ModesEnum Mode { get; set; }

        public DateTime Date { get; set; }

        public Dictionary<int, bool> AnswersStats { get; set; }

        public GameStats()
        {
            AnswersStats = new Dictionary<int, bool>();
        }
    }
}
