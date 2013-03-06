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
    public class StatsController : Controller
    {
        //
        // GET: /Stats/

        public ActionResult Index()
        {
            StatsModel model = new StatsModel();

            StatsDb db = new StatsDb();
            model.Stats = db.ReadAll();

            return View(model);
        }

    }
}
