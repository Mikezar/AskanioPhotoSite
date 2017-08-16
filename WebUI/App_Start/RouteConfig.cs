using System.Web.Mvc;
using System.Web.Routing;

namespace AskanioPhotoSite.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("Management/");

            routes.MapRoute(
            "Manage",
            "Secured/Management",
            new { controller = "Management", action = "Index" }
        );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
