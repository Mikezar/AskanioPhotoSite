using System.Collections.Generic;
using AskanioPhotoSite.Core.Enums;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AskanioPhotoSite.Core.Models
{
   public class PhotoListModel
    {
        public PhotoListModel()
        {
            Photos = new List<PhotoModel>();
        }

        public List<PhotoModel> Photos{ get; set; }

        public string ReturnUrl { get; set; }
    }
}
