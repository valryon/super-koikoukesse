using System;
using System.Web.Mvc;
using System.Web.Routing;
using Pixelnest.Common.Log;
using System.Configuration;
using Pixelnest.Common;

namespace SuperKoikoukesse.Webservice
{
    // Remarque : pour obtenir des instructions sur l'activation du mode classique IIS6 ou IIS7, 
    // visitez http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Webservices routes
            //------------------------------------------------------------------------------
            routes.MapRoute(
               "WSGameService",
               "ws/games",
               new { controller = "Service", action = "Games" }
           );

            routes.MapRoute(
               "WSExclusionService",
               "ws/exclusions",
               new { controller = "Service", action = "Exclusions" }
           );

            routes.MapRoute(
               "WSState",
               "ws/{action}",
               new { controller = "Service", action = "Index" }
            );

            routes.MapRoute(
               "WSDefault",
               "ws/{action}",
               new { controller = "Service" }
           );

            // Admin routes
            //------------------------------------------------------------------------------

            routes.MapRoute(
               "BOImportDB",
               "admin/db/import",
               new { controller = "GameDatabase", action = "ImportCSV" }
           );

            routes.MapRoute(
               "BOExportDB",
               "admin/db/export",
               new { controller = "GameDatabase", action = "ExportCSV" }
           );

            routes.MapRoute(
               "BOListDB",
               "admin/db/{page}",
               new { controller = "GameDatabase", action = "Index", page = 1 }
           );

            routes.MapRoute(
              "BODefautDB",
              "admin/db/{action}",
              new { controller = "GameDatabase" }
          );

            //------------------------------------------------------------------------------

            routes.MapRoute(
                "Default", // Nom d'itinéraire
                "{controller}/{action}/{id}", // URL avec des paramètres
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Paramètres par défaut
            );

        }

        protected void Application_Start()
        {
            // Initialize log
            Logger.Initialize("SuperKoikoukesse.Webservice.Log");
            Logger.Log(LogLevel.Info, "Starting website SuperKoikoukesse.Webservice...");

            // Initialize cryptography
            EncryptionHelper.Initialize(ConfigurationManager.AppSettings["ENCRYPTION_KEY"].ToString());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}