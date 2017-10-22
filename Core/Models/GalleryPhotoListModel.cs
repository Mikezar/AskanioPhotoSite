using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
    public class GalleryPhotoListModel
    {
        public GalleryPhotoListModel()
        {
            Photos = new List<GalleryPhotoModel>();
        }

        public IEnumerable<GalleryPhotoModel> Photos { get; set; }

        public string AlbumTitle { get; set; }
    }
}
