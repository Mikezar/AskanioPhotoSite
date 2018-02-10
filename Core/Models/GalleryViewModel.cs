using System;
using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Models
{
    public class GalleryViewModel
    {
        public GalleryViewModel()
        {
            ParentAlbums = new List<Tuple<int, string>>();
        }

        public IEnumerable<GalleryAlbumModel> Albums { get; set; }

        public IEnumerable<Tuple<int, string>> ParentAlbums { get; set; }
    }
}
