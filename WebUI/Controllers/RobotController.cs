using AskanioPhotoSite.WebUI.Helpers;
using System.Web.Mvc;

namespace AskanioPhotoSite.WebUI.Controllers
{
    public class RobotController : BaseController
    {
        public ContentResult Index()
        {
            string path = HttpContext.Server.MapPath("~/robots.txt");

            string content = ContentLoader.Get(path);

            return Content(content, "text/plain");
        }
    }
}