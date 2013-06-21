using SuperKoikoukesse.Webservice.Core.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SuperKoikoukesse.Webservice.Models
{
    [DataContract]
    public class AddGameHistoryInput
    {
        [DataMember(Name = "player", IsRequired = true)]
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
            Dictionary<int, bool> answers = new Dictionary<int, bool>();

            foreach (var stat in this.AnswersStats)
            {
                answers.Add(stat.QuestionId, stat.Result);
            }

            GameStats s = new GameStats()
            {
                PlayerId = new Guid(), // TODO Player id
                Date = this.Date,
                Difficulty = DifficultiesEnumEx.FromInt(this.Difficulty),
                Mode = ModesEnumEx.FromInt(this.Mode),
                Score = this.Score,
                AnswersStats = answers
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