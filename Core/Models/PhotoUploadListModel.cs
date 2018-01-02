using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Models
{
    public class PhotoUploadListModel
    {
        public PhotoUploadListModel()
        {
            Uploads = new List<PhotoUploadModel>();
            ImageAttributes = new ImageAttrModel();
        }

        public List<PhotoUploadModel> Uploads { get; set; }

        public ImageAttrModel ImageAttributes { get; set; }

        public IEnumerable<SelectListItem> Albums { get; set; }

        public int AlbumId { get; set; }
    }
}
