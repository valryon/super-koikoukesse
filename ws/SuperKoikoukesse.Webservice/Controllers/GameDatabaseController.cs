using SuperKoikoukesse.Webservice.Core.Dao;
using SuperKoikoukesse.Webservice.Core.Games;
using SuperKoikoukesse.Webservice.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperKoikoukesse.Webservice.Controllers
{
    [Authorize]
    public class GameDatabaseController : Controller
    {
        private GameInfoDao m_dao;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (m_dao == null)
            {
                string dbPath = Server.MapPath(ConfigurationManager.AppSettings["GAME_DB_PATH"].ToString());

                m_dao = new GameInfoDao(dbPath);
                if (m_dao.IsNew)
                {
                    m_dao.Save();
                }
            }
        }

        private static int pageSize = 100;

        public ActionResult Index(int page = 1)
        {
            if (page < 1) page = 1;
            List<GameInfo> gameDb = m_dao.ReadAll();

            GameDatabaseModel model = new GameDatabaseModel();
            model.Page = page;
            model.MaxPage = gameDb.Count / pageSize;

            if (page > model.MaxPage) page = model.MaxPage;

            // Apply pagination
            model.Games = gameDb.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(model);
        }

        /// <summary>
        /// Import CSV database
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportCSV(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0 && Path.GetExtension(file.FileName).ToLower().EndsWith("csv"))
            {
                //TODO  Lire CSV
            }

            return RedirectToAction("Index");
        }
    }
}
