using System.Web;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System;
using System.IO;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

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


        public static void WatermarkImage(string path, HttpPostedFileBase file, 
            ImageAttrModel attributes, TextAttributes textAttributes)
        {
            if (textAttributes == null)
            {
                textAttributes = new TextAttributes()
                {
                    WatermarkFont = "Bell MT",
                    WatermarkFontSize = 60,
                    WatermarkText = "AlexSilver.Photo@gmail.com",
                    SignatureFont = "Edwardian Script ITC",
                    SignatureFontSize = 43,
                    SignatureText = "© Alexander Serebryakov",
                    StampFont = "Bell MT",
                    StampFontSize = 45,
                    StampText = "www.askanio.ru"
                };
            }

            Random random = new Random();

            var watermarkFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var textFormat = new StringFormat()
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near
            };

            using (Image image = Image.FromStream(file.InputStream))
            {
                using (Graphics imageGraphics = Graphics.FromImage(image))
                {
                    imageGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    if (attributes.IsSignatureApplied)
                    {
                        using (Font signatureFont = new Font(textAttributes.SignatureFont, textAttributes.SignatureFontSize, FontStyle.Regular, GraphicsUnit.Pixel))
                        {
                            //Font adaptedWatermarlFont = CalculateFont(imageGraphics, signature, new Size(image.Width / 4, image.Height / 4), signatureFont);
                            SizeF actualSize = imageGraphics.MeasureString(textAttributes.SignatureText, signatureFont);
                            imageGraphics.DrawString(textAttributes.SignatureText, signatureFont,  
                                attributes.IsSignatureBlack ? Brushes.Black : Brushes.White,
                                attributes.IsRightSide ? new PointF(image.Width - actualSize.Width, image.Height - actualSize.Height) : new PointF(10f, image.Height - actualSize.Height), 
                                textFormat);
                        }
                    }
                    if (attributes.IsWebSiteTitleApplied)
                    {
                        using (Font stampFont = new Font(textAttributes.StampFont, textAttributes.StampFontSize, FontStyle.Regular, GraphicsUnit.Pixel))
                        {
                            // Font adaptedWatermarlFont = CalculateFont(imageGraphics, stamp, new Size(120, 20), stampFont);
                            SizeF actualSize = imageGraphics.MeasureString(textAttributes.StampText, stampFont);
                            imageGraphics.DrawString(textAttributes.StampText, stampFont, 
                                attributes.IsWebSiteTitleBlack ? Brushes.Black : Brushes.White,
                                 attributes.IsRightSide ? new PointF(image.Width - actualSize.Width, image.Height - actualSize.Height - 60f) : new PointF(10f, image.Height - actualSize.Height - 60f),
                                textFormat);
                        }
                    }
                    if (attributes.IsWatermarkApplied)
                    {
                        using (Font watermarkFont = new Font(textAttributes.WatermarkFont, textAttributes.WatermarkFontSize, FontStyle.Regular, GraphicsUnit.Pixel))
                        {
                            // Font adaptedWatermarkFont = CalculateFont(imageGraphics, watermark, new Size(image.Width / 2, image.Height / 2), watermarkFont);
                            SizeF actualSize = imageGraphics.MeasureString(textAttributes.WatermarkText, watermarkFont);
                            imageGraphics.DrawString(textAttributes.WatermarkText, watermarkFont, 
                                new SolidBrush(Color.FromArgb(80, attributes.IsWatermarkBlack? Color.Black : Color.White)),
                                (image.Width / 2) + random.Next(40), (image.Height / 2) + random.Next(40),
                                watermarkFormat);
                        }
                    }

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
                                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                image.RotateFlip(RotateFlipType.Rotate90FlipNone); 
                                break;
                            case 7:
                                image.RotateFlip(RotateFlipType.Rotate270FlipX);
                                break;
                            case 8:
                                image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                image.RotateFlip(RotateFlipType.Rotate90FlipNone); ;
                                break;
                        }
                        // This EXIF data is now invalid and should be removed.
                        image.RemovePropertyItem(274);
                    }

                    image.Save(HttpContext.Current.Server.MapPath(path), image.RawFormat);
                }
            }
        }

        //private static Font CalculateFont(Graphics graphics, string text, Size size, Font font)
        //{
        //    SizeF actualSize = graphics.MeasureString(text, font);
        //    float heightScaleRatio = size.Height / actualSize.Height;
        //    float widthScaleRatio = size.Width / actualSize.Width;
        //    float scaleRatio = (heightScaleRatio < widthScaleRatio) ? scaleRatio = heightScaleRatio : scaleRatio = widthScaleRatio;
        //    float scaleFontSize = font.Size * scaleRatio;
        //    return new Font(font.Name, scaleFontSize);
        //}

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
                            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
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
