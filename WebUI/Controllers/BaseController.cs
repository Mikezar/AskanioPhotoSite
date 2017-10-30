using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Helpers;

namespace AskanioPhotoSite.WebUI.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            Log.RegisterError(filterContext.Exception);
            filterContext.ExceptionHandled = true;

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error"
            };
        }
    }
}