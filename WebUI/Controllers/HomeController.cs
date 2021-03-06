﻿using System.Web.Mvc;
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
            var model = new HomePageModel()
            {
                RandomPhoto = _photoService.GetRandomPhoto(),
                BackgroundCovers = _photoService.GetBackgroundPhotos().ToArray()
            };

            if(Request.Browser.IsMobileDevice)
            {
                return View("Index_m", model);
            }

            return View(model);
        }

        public ActionResult GetRandomPhoto()
        {
            var photo = _photoService.GetRandomPhoto();

            var model = new SideBarModel()
            {
                Photo = photo,
                Tags = _tagService.GetAll()
            };

            if (Request.Browser.IsMobileDevice)
            {
                return PartialView("~/Views/Gallery/_Mobile.cshtml", model);
            }

            return PartialView("~/Views/Shared/_SideBar.cshtml", model);
        }

        public ActionResult GenerateTagCloud()
        {
            var cloud = _tagService.GenerateTagCloud();

            return PartialView("~/Views/Shared/_Cloud.cshtml", cloud);
        }     
    }
}