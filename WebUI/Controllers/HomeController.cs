using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using System.Collections.Generic;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Services.Extensions;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.WebUI.Helpers;


namespace AskanioPhotoSite.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly BaseService<Album> _albumService;

        private readonly BaseService<Photo> _photoService;

        private readonly BaseService<Tag> _tagService;

        private readonly BaseService<PhotoToTag> _photoToTagService;


        public HomeController(BaseService<Album> albumService, BaseService<Photo> photoService,
            BaseService<Tag> tagService, BaseService<PhotoToTag> photoToTagService)
        {
            _albumService = albumService;
            _photoService = photoService;
            _tagService = tagService;
            _photoToTagService = photoToTagService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetRandomPhoto()
        {
            var photo = _photoService.GetRandomPhoto();

            return PartialView("~/Views/Shared/_SideBar.cshtml", photo);
        }

        public ActionResult GenerateTagCloud()
        {
            var photos = _photoService.GetAll();

            var cloud = _photoToTagService.GetAll().Join(_tagService.GetAll(), i => i.TagId, o => o.Id,(i, o) => new
            {
                Id = i.TagId,
                TitleRu = o.TitleRu,
                TitleEng = o.TitleEng,
            }).GroupBy(t => t.Id).Select(x => new TagCloudModel()
            {
                Id = x.Key,
                Count = x.Count(),
                TitleRu = x.Select(r => r.TitleRu).FirstOrDefault(),
                TitleEng = x.Select(r => r.TitleEng).FirstOrDefault(),
            });

            if (CultureHelper.IsEnCulture())
                cloud = cloud.OrderBy(x => x.TitleEng).ToList();
            else
                cloud = cloud.OrderBy(x => x.TitleRu).ToList();

            return PartialView("~/Views/Shared/_Cloud.cshtml", cloud);
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