using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Models
{
    public class PhotoModel
    {
        public int Id { get; set; }

        public string TitleRu { get; set; }

        public string TitleEng { get; set; }

        public string DescriptionRu { get; set; }

        public string DescriptionEng { get; set; }

        public string PhotoPath { get; set; }

        public string ThumbnailPath { get; set; }

        public Album Album { get; set; }

        public int Order { get; set; }
    }
}
