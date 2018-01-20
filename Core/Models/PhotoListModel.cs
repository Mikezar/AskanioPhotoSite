using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Models
{
   public class PhotoListModel
    {
        public PhotoListModel()
        {
            Photos = new List<PhotoModel>();
        }

        public List<PhotoModel> Photos{ get; set; }

        public bool ShowOrderArrows { get; set; }

        public string ReturnUrl { get; set; }
    }
}
