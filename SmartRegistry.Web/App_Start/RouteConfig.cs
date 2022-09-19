using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartRegistry.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            /*
             * най-горе дефинираният route се използва първо,
             * ако не втрши работа тогава минава на втория route
             */

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
          //  routes.MapRoute(
          //      name: "Authentication",
          //      url: "{controller}/{action}/{id}",
          //      defaults: new { controller = "Authentication", action = "Index", id = UrlParameter.Optional }
          //  );

          //  routes.MapRoute(
          //    name: "AdministrativeBodies",
          //    url: "{controller}/{action}/{id}",
          //    defaults: new { controller = "AdministrativeBodies", action = "Index", id = UrlParameter.Optional }
          //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
