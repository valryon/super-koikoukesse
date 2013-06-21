using SuperKoikoukesse.Webservice.Core.Model;
using System.Collections.Generic;

namespace SuperKoikoukesse.Webservice.ViewModels
{
    public class StatsModel
    {
        public List<GameStats> Stats { get; set; }

        public List<AnswersStatModel> AnswersStat { get; set; }

        public StatsModel()
        {
            Stats = new List<GameStats>();
            AnswersStat = new List<AnswersStatModel>();
        }
    }

    public class AnswersStatModel
    {
        public Game Game { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }

        public int Total
        {
            get
            {
                return SuccessCount + FailureCount;
            }
        }
    }
}