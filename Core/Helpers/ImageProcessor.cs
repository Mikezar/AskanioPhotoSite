using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System;
using System.IO;


namespace AskanioPhotoSite.Core.Helpers
{
    public class ImageProcessor
    {
        public static void CreateThumbnail(int maxWidth, int maxHeight, HttpPostedFileBase file, string path)
        {

            var image = Image.FromStream(file.InputStream);
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics thumbGraph = Graphics.FromImage(newImage);


            thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);
            image.Dispose();

            string fileRelativePath = "~/Content/Gallery/Thumbs/" + Path.GetFileName(path);
            newImage.Save(HttpContext.Current.Server.MapPath(fileRelativePath), newImage.RawFormat);
        }

        public static void ImageRotating(HttpPostedFileBase file, string path)
        {
            using (var image = Image.FromStream(file.InputStream))
            {
                if (Array.IndexOf(image.PropertyIdList, 274) > -1)
                {
                    var orientation = (int)image.GetPropertyItem(274).Value[0];
                    switch (orientation)
                    {
                        case 1:
                            // No rotation required.
                            break;
                        case 2:
                            image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            break;
                        case 3:
                            image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            break;
                        case 4:
                            image.RotateFlip(RotateFlipType.Rotate180FlipX);
                            break;
                        case 5:
                            image.RotateFlip(RotateFlipType.Rotate90FlipX);
                            break;
                        case 6:
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            break;
                        case 7:
                            image.RotateFlip(RotateFlipType.Rotate270FlipX);
                            break;
                        case 8:
                            image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            break;
                    }
                    // This EXIF data is now invalid and should be removed.
                    image.RemovePropertyItem(274);
                }
                image.Save(HttpContext.Current.Server.MapPath(path), image.RawFormat);
            }
        }
    }
}
