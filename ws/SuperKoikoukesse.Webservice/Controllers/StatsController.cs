using SuperKoikoukesse.Webservice.Core.DB;
using SuperKoikoukesse.Webservice.Core.Model;
using SuperKoikoukesse.Webservice.Models;
using SuperKoikoukesse.Webservice.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SuperKoikoukesse.Webservice.Controllers
{
    public class StatsController : Controller
    {
        /// <summary>
        /// List of played games
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            StatsModel model = new StatsModel();

            StatsDb db = new StatsDb();
            model.Stats = db.ReadAll().OrderByDescending(s => s.Date).ToList();

            return View(model);
        }

        /// <summary>
        /// Export lsit of games
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportCSV()
        {
            StatsModel model = new StatsModel();

            StatsDb db = new StatsDb();
            model.Stats = db.ReadAll().OrderByDescending(s => s.Date).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var stat in model.Stats)
            {
                sb.AppendLine(stat.Id + ";" + stat.Date + ";" + stat.Mode + ";" + stat.Difficulty + ";" + stat.PlayerId + ";" + stat.Score + ";");
            }

            return new FileContentResult(
                 Encoding.UTF8.GetBytes(sb.ToString()),
                 "stats.csv"
            );
        }

        /// <summary>
        /// List of questions
        /// </summary>
        /// <returns></returns>
        public ActionResult QuestionStats()
        {
            StatsModel model = new StatsModel();

            StatsDb db = new StatsDb();

            GamesDb gamesDb = new GamesDb();
            List<Game> allGames = gamesDb.ReadAll();

            var rawStats = db.ReadAll().OrderByDescending(s => s.Date).ToList();

            var knownGame = new Dictionary<int,AnswersStatModel>();

            foreach (var rawStat in rawStats)
            {
                // Get the answers
                foreach (var rawAnswer in rawStat.AnswersStats)
                {
                    AnswersStatModel answer;

                    if (knownGame.TryGetValue(rawAnswer.Key, out answer) == false)
                    {
                        answer = new AnswersStatModel();
                        answer.Game = allGames.Where(g => g.GameId == rawAnswer.Key).FirstOrDefault();
                    }

                    if (rawAnswer.Value)
                    {
                        answer.SuccessCount += 1;
                    }
                    else
                    {
                        answer.FailureCount += 1;
                    }

                    knownGame[rawAnswer.Key] = answer;
                }
            }

            model.AnswersStat = knownGame.Values.ToList();

            return View(model);
        }
    }
}
