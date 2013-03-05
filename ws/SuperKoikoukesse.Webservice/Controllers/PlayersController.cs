using SuperKoikoukesse.Webservice.Core.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperKoikoukesse.Webservice.Controllers
{
    public class PlayersController : Controller
    {
        //
        // GET: /Players/

        public ActionResult Index()
        {
            PlayersDb db = new PlayersDb();

            

            return View();
        }

    }
}
