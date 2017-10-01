using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AskanioPhotoSite.WebUI.Infrastructure;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;
using System;

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
            HttpCookie cookie = HttpContext.Current.Request.Cookies["CurrentUICulture"];

            if ((cookie != null) && (cookie.Value != null))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(cookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(cookie.Value);
            }

            else
            {
               var languages = HttpContext.Current.Request.UserLanguages;

                if (languages.Length > 0)
                {
                    for (int i = 0; i < languages.Length; i++)
                    {
                        if (_supportedCultures.Contains(languages[i]))
                        {
                            Thread.CurrentThread.CurrentCulture = new CultureInfo(languages[i]);
                            break;
                        }
                    }
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
                }
            }
        }

    }
}
