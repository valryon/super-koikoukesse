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
    /// <summary>
    /// Configuration live edition
    /// </summary>
    public class ConfigurationController : Controller
    {
        public ActionResult Index()
        {
            ConfigurationModel model = new ConfigurationModel();

            ConfigurationDb db = new ConfigurationDb();

            var config = db.GetConfiguration((int)ConfigurationTargetEnum.All);

            model.ScoreAttack = config.ModesConfiguration.Where(m => m.Mode == ModesEnum.ScoreAttack).ToArray();
            model.TimeAttack = config.ModesConfiguration.Where(m => m.Mode == ModesEnum.TimeAttack).ToArray();
            model.Survival = config.ModesConfiguration.Where(m => m.Mode == ModesEnum.Survival).ToArray();
            model.Versus = config.ModesConfiguration.Where(m => m.Mode == ModesEnum.Versus).ToArray();

            return View(model);
        }

        public ActionResult Reinit()
        {
            ConfigurationDb db = new ConfigurationDb();

            var config = db.GetConfiguration((int)ConfigurationTargetEnum.All);

            config.InitializeDefaultValues();

            db.Update(config);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Save(ConfigurationModel config)
        {
            ConfigurationDb db = new ConfigurationDb();

            var localConfig = db.GetConfiguration((int)ConfigurationTargetEnum.All);

            localConfig.ModesConfiguration.Clear();
            localConfig.ModesConfiguration.AddRange(config.ScoreAttack.ToList());
            localConfig.ModesConfiguration.AddRange(config.TimeAttack.ToList());
            localConfig.ModesConfiguration.AddRange(config.Survival.ToList());
            localConfig.ModesConfiguration.AddRange(config.Versus.ToList());

            localConfig.Properties.Clear();

            db.Update(localConfig);

            return RedirectToAction("Index");
        }
    }
}
