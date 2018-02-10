using System;
using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Models
{
    public class GalleryPhotoListModel
    {
        public GalleryPhotoListModel()
        {
            Photos = new List<GalleryPhotoModel>();
            Albums = new List<Tuple<int, string>>();
        }

        public IEnumerable<GalleryPhotoModel> Photos { get; set; }

        public string AlbumTitle { get; set; }

        public IEnumerable<Tuple<int, string>> Albums { get; set; }
    }
}
