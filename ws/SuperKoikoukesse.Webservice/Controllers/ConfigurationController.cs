using SuperKoikoukesse.Webservice.Core.DB;
using SuperKoikoukesse.Webservice.Core.Model;
using SuperKoikoukesse.Webservice.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperKoikoukesse.Webservice.Controllers
{
    public class ConfigurationController : Controller
    {
        public ActionResult Index()
        {
            ConfigurationModel model = new ConfigurationModel();

            ConfigurationDb db = new ConfigurationDb();

            model.Config = db.GetConfiguration((int)ConfigurationTargetEnum.All);

            return View(model);
        }

    }
}
