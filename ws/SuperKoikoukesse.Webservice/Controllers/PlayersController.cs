using SuperKoikoukesse.Webservice.Core.DB;
using SuperKoikoukesse.Webservice.Models;
using SuperKoikoukesse.Webservice.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperKoikoukesse.Webservice.Controllers
{
    public class PlayersController : Controller
    {
        public ActionResult Index()
        {
            PlayersModel model = new PlayersModel();

            PlayersDb db = new PlayersDb();
            model.Players = db.ReadAll();

            return View(model);
        }
    }
}