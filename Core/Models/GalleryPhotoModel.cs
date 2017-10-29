using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
   public class GalleryPhotoModel
    {
        public int Id { get; set; }

        public string Thumbnail { get; set; }

        public string Photo { get; set; }

        public string Description { get; set; }
    }
}
