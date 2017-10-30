using System.Web;
using System.Threading;
using System.Globalization;
using System.Web.Mvc;


namespace AskanioPhotoSite.WebUI.Controllers
{
    public class CultureController : BaseController
    {
        // GET: Language
        public ActionResult ChangeCulture(string language)
        {
            if (language != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }

            HttpCookie cookie = new HttpCookie("CurrentUICulture");
            cookie.Value = language;
            Response.Cookies.Add(cookie);

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}