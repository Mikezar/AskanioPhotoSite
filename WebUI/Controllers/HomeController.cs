using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using AskanioPhotoSite.Core.Models;

namespace AskanioPhotoSite.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginModel()
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if(string.IsNullOrEmpty(model.Login)) ModelState.AddModelError("Login", "Поле логина не может быть пустым");
            if(string.IsNullOrEmpty(model.Password)) ModelState.AddModelError("Password", "Поле пароля не может быть пустым");

            if (!ModelState.IsValid) return View(model);

            FormsAuthentication.SignOut();
            bool success =  FormsAuthentication.Authenticate(model.Login, model.Password);
            if (!success)
            {
                model.Error = "Не верный пароль или логин.";
                return View(model);
            }
            FormsAuthentication.SetAuthCookie(model.Login, false);


            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            return RedirectToAction("Index", "Management");
        }
    }
}