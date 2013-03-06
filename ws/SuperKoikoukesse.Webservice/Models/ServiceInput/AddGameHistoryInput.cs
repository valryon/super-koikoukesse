using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SuperKoikoukesse.Webservice.Models.ServiceInput
{
    [DataContract]
    public class AddGameHistoryInput
    {
        [DataMember(Name="player", IsRequired=true)]
        public string PlayerId { get; set; }

        [DataMember(Name = "score", IsRequired = true)]
        public int Score { get; set; }

        [DataMember(Name = "difficulty", IsRequired = true)]
        public int Difficulty { get; set; }

        [DataMember(Name = "mode", IsRequired = true)]
        public int Mode { get; set; }

        [DataMember(Name = "date", IsRequired = true)]
        public DateTime Date { get; set; }

        [DataMember(Name = "answers", IsRequired = true)]
        public List<AnswerResult> AnswersStats { get; set; }

        /// <summary>
        /// Convert to the database object
        /// </summary>
        /// <returns></returns>
        public GameStats ToDbModel()
        {
            GameStats s = new GameStats()
            {
                PlayerId = new Guid(), // TODO Player id
                Date = this.Date,
                Difficulty = this.Difficulty,
                Mode = this.Mode,
                Score = this.Score,
                AnswersStats = new Dictionary<int,bool>() // TODO
            };

            return s;
        }
    }

    [DataContract]
    public class AnswerResult
    {
        [DataMember(Name = "id", IsRequired = true)]
        public int QuestionId { get; set; }

        [DataMember(Name = "result", IsRequired = true)]
        public bool Result { get; set; }
    }
}