﻿using System.Web.Mvc;
using System.Web.Routing;
using AskanioPhotoSite.WebUI.Infrastructure.Files;

namespace AskanioPhotoSite.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("Management/{action}");

            routes.IgnoreRoute("Management/{action}/{id}");

            //Обработка изображений
            routes.Add(new Route(
                "image", 
                null, 
                new RouteValueDictionary(new { MvcContraint = new ImageRouteHandlerConstaint() }), 
                new ImageRouteHandler()));
            
            routes.MapRoute(
            "Manage",
            "Secured/Management/{action}/{id}",
            new {controller = "Management", action = "Index", id = UrlParameter.Optional }
        );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
