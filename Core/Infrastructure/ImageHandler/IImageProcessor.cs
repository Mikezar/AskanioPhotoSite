using System.Web;
using System.Drawing;
using AskanioPhotoSite.Core.Models;

namespace AskanioPhotoSite.Core.Infrastructure.ImageHandler
{
    public interface IImageProcessor
    {
        void CreateThumbnail(HttpPostedFileBase file, int maxWidth, int maxHeight, string fileName);
        byte[] Watermark(ImageAttrModel attributes);
        byte[] ImageToByteArray(Image image);
        void RotateImage(Image image);
    }
}
