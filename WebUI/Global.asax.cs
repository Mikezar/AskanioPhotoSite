﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AskanioPhotoSite.WebUI.Infrastructure;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System;
using System.Linq;
using AskanioPhotoSite.Core.Helpers;
using AskanioPhotoSite.WebUI.Controllers;

namespace AskanioPhotoSite.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly IList<string> _supportedCultures = new List<string>()
        {
            "en-US",
            "ru-RU"
        };

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }

        protected void Application_BeginRequest(object sender, EventArgs _args)
        {
            var httpApp = (HttpApplication)sender;
            Log.Trace($"Query register. Url - {httpApp.Request?.Url}, Agent - {httpApp.Request?.UserAgent}");
            HttpCookie cookie = HttpContext.Current.Request.Cookies["CurrentUICulture"];

            Log.Trace("Getting cookies...");

            if ((cookie != null) && (cookie?.Value != null))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cookie.Value);
            }

            else
            {
                Log.Trace("No cookies.");
                var languages = HttpContext.Current?.Request?.UserLanguages;

                if (languages == null) return;
                Log.Trace($"Supported languages: {languages.Select(x => x).Concat(languages)}");
                if (languages.Length > 0)
                {
                    for (int i = 0; i < languages.Length; i++)
                    {
                        if (_supportedCultures.Contains(languages[i]))
                        {
                            Thread.CurrentThread.CurrentCulture = new CultureInfo(languages[i]);
                            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languages[i]);
                            HttpContext.Current.Response.Cookies.Add(new HttpCookie("CurrentUICulture") { Value = languages[i] });
                            break;
                        }
                    }
                }
                else
                {
                    Log.Trace("Нет поддерживаемых языков. По умочланию устанавливается ru-RU");
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie("CurrentUICulture") { Value = "ru-RU"});
                }
            }
        }

        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();

            Log.RegisterError(exception);

            HttpException httpException = exception as HttpException;

            if (httpException == null)
                httpException = new HttpException(500, "Internal Server Error", exception);
            Response.Clear();

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
            {
	        case 404:
	            routeData.Values.Add("action", "NotFound");
	            break;
	        default:
	            routeData.Values.Add("action", "Index");
	            break;
	    }
	    Server.ClearError();

	    IController controller = new ErrorController();
	    controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}
