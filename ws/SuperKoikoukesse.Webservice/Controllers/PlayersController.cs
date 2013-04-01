using SuperKoikoukesse.Webservice.Core.DB;
using SuperKoikoukesse.Webservice.Core.Model;
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

        public ActionResult AddCredits(string id, int credits)
        {
            Guid guid = new Guid(id);

            PlayersDb db = new PlayersDb();

            Player p = db.GetPlayer(guid);
            p.Credits += credits;

            db.Update(p);

            return RedirectToAction("Index");
        }

        public ActionResult AddCoins(string id, int coins)
        {
            Guid guid = new Guid(id);

            PlayersDb db = new PlayersDb();

            Player p = db.GetPlayer(guid);
            p.Coins += coins;

            db.Update(p);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            Guid guid = new Guid(id);

            PlayersDb db = new PlayersDb();

            db.DeleteFromGuid(guid);

            return RedirectToAction("Index");
        }
    }
}