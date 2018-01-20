using System.Web.Mvc;
using System.Linq;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Services.Abstract;

namespace AskanioPhotoSite.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPhotoService _photoService;
        private readonly ITagService _tagService;

        public HomeController(IPhotoService photoService, ITagService tagService)
        {
            _photoService = photoService;
            _tagService = tagService;
        }

        public ActionResult Index()
        {
            return View(new HomePageModel()
            {
                RandomPhoto = _photoService.GetRandomPhoto(),
                BackgroundCovers = _photoService.GetBackgroundPhotos().ToArray()
            });
        }

        public ActionResult GetRandomPhoto()
        {
            var photo = _photoService.GetRandomPhoto();

            return PartialView("~/Views/Shared/_SideBar.cshtml", photo);
        }

        public ActionResult GenerateTagCloud()
        {
            var cloud = _tagService.GenerateTagCloud();

            return PartialView("~/Views/Shared/_Cloud.cshtml", cloud);
        }     
    }
}