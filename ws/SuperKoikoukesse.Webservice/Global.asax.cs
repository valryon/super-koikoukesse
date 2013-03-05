using System;
using System.Web.Mvc;
using System.Web.Routing;
using Pixelnest.Common.Log;
using System.Configuration;
using Pixelnest.Common;
using SuperKoikoukesse.Webservice.Core.DB;

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
               "WSExclusionService",
               "ws/exclusions",
               new { controller = "Service", action = "Exclusions" }
           );

            routes.MapRoute(
               "WSPlayerService",
               "ws/player",
               new { controller = "Service", action = "GetPlayerInfo" }
           );

            routes.MapRoute(
               "WSGameHistoryService",
               "ws/games/add",
               new { controller = "Service", action = "AddGameHistory" }
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

            // Initialize database
            MongoDbService.Instance.Initialize(ConfigurationManager.ConnectionStrings["DB_KOIKOUKESSE"].ToString());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}