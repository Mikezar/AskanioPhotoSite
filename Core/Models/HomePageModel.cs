using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Models
{
    public class HomePageModel
    {
        public Photo RandomPhoto { get; set; }
        public Photo[] BackgroundCovers { get; set; }
    }
}
