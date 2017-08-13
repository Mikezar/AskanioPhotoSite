using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Entities
{
    public class Photo : Entity
    {
        public int Id { get; set; }

        public string TitleRu { get; set; }

        public string TitleEng { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEng { get; set; }

        public string PhotoPath { get; set; }

        public string ThumbnailPath { get; set; }

        public int AlbumId { get; set; }
    }
}
