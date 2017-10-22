using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
    public class GalleryPhotoViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }
    }
}
