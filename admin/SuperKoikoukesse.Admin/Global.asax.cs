using System;
using System.Web.Mvc;
using System.Web.Routing;
using SuperKoikoukesse.Common.Log;

namespace SuperKoikoukesse.Admin
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

            routes.MapRoute(
                "Default", // Nom d'itinéraire
                "{controller}/{action}/{id}", // URL avec des paramètres
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Paramètres par défaut
            );

        }

        protected void Application_Start()
        {
            // Initialize log
            Logger.Initialize("SuperKoikoukesse.Admin.Log");
            Logger.Log(LogLevel.Info, "Starting website SuperKoikoukesse.Admin...");

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}