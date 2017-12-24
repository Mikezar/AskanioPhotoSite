using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AskanioPhotoSite.WebUI.Controllers
{
    public class ErrorController : BaseController
    {

        public ActionResult Index()
        {
            Response.ContentType = "text/html";
            return View("Error");
        }

        public ActionResult NotFound()
        {
            Response.ContentType = "text/html";
            return View();
        }
    }
}