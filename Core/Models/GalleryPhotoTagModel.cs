using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
   public class GalleryPhotoTagModel
    {
        public GalleryPhotoTagModel()
        {
            Photos = new List<GalleryPhotoModel>();
        }

        public string TagName { get; set; }

        public IEnumerable<GalleryPhotoModel> Photos { get; set; }
    }
}
