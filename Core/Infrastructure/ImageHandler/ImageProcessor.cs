using System.Web;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System;
using System.IO;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Infrastructure.ImageHandler
{
    public sealed class ImageProcessor : IImageProcessor
    {
        private readonly string _thumbFolder;
        private readonly string _photoPath;
        private readonly TextAttributes TextAttributes;

        public ImageProcessor(string path, string thumbFolder)
        {
            if (string.IsNullOrEmpty(thumbFolder)) throw new ArgumentNullException("thumbPath");
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");

            _thumbFolder = thumbFolder;
            _photoPath = path;
        }

        public ImageProcessor(string path, string thumbPath, TextAttributes attributes) : this(path, thumbPath)
        {
            TextAttributes = attributes;
        }


        public void CreateThumbnail(HttpPostedFileBase file, int maxWidth, int maxHeight, string fileName)
        {
            using (var image = Image.FromStream(file.InputStream))
            {
                var ratioX = (double)maxWidth / image.Width;
                var ratioY = (double)maxHeight / image.Height;
                var ratio = Math.Min(ratioX, ratioY);
                var newWidth = (int)(image.Width * ratio);
                var newHeight = (int)(image.Height * ratio);
                using (var newImage = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics thumbGraph = Graphics.FromImage(newImage))
                    {
                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);

                        string fileRelativePath = _thumbFolder + Path.GetFileName($"{fileName}s{Path.GetExtension(file.FileName).ToLower()}");

                        if (!Directory.Exists(HttpContext.Current.Server.MapPath(_thumbFolder)))
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(_thumbFolder));

                        newImage.Save(HttpContext.Current.Server.MapPath(fileRelativePath), newImage.RawFormat);
                    }
                }
            }
        }

        public byte[] Watermark(ImageAttrModel attributes)
        {
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

            using (Image image = Image.FromFile(HttpContext.Current.Server.MapPath(_photoPath), true))
            {
                using (Graphics imageGraphics = Graphics.FromImage(image))
                {
                    imageGraphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    if (attributes.IsSignatureApplied)
                    {
                        using (Font signatureFont = new Font(TextAttributes.SignatureFont, TextAttributes.SignatureFontSize, FontStyle.Regular, GraphicsUnit.Pixel))
                        {
                            //Font adaptedWatermarkFont = CalculateFont(imageGraphics, signature, new Size(image.Width / 4, image.Height / 4), signatureFont);
                            SizeF actualSize = imageGraphics.MeasureString(TextAttributes.SignatureText, signatureFont);
                            imageGraphics.DrawString(TextAttributes.SignatureText, signatureFont,  
                                attributes.IsSignatureBlack ? Brushes.Black : Brushes.White,
                                attributes.IsRightSide ? new PointF(image.Width - actualSize.Width, image.Height - actualSize.Height) : new PointF(10f, image.Height - actualSize.Height), 
                                textFormat);
                        }
                    }
                    if (attributes.IsWebSiteTitleApplied)
                    {
                        using (Font stampFont = new Font(TextAttributes.StampFont, TextAttributes.StampFontSize, FontStyle.Regular, GraphicsUnit.Pixel))
                        {
                            // Font adaptedWatermarkFont = CalculateFont(imageGraphics, stamp, new Size(120, 20), stampFont);
                            SizeF actualSize = imageGraphics.MeasureString(TextAttributes.StampText, stampFont);
                            imageGraphics.DrawString(TextAttributes.StampText, stampFont, 
                                attributes.IsWebSiteTitleBlack ? Brushes.Black : Brushes.White,
                                 attributes.IsRightSide ? new PointF(image.Width - actualSize.Width, image.Height - actualSize.Height - 60f) : new PointF(10f, image.Height - actualSize.Height - 60f),
                                textFormat);
                        }
                    }
                    if (attributes.IsWatermarkApplied)
                    {
                        using (Font watermarkFont = new Font(TextAttributes.WatermarkFont, TextAttributes.WatermarkFontSize, FontStyle.Regular, GraphicsUnit.Pixel))
                        {
                            // Font adaptedWatermarkFont = CalculateFont(imageGraphics, watermark, new Size(image.Width / 2, image.Height / 2), watermarkFont);
                            SizeF actualSize = imageGraphics.MeasureString(TextAttributes.WatermarkText, watermarkFont);
                            imageGraphics.DrawString(TextAttributes.WatermarkText, watermarkFont, 
                                new SolidBrush(Color.FromArgb(80, attributes.IsWatermarkBlack? Color.Black : Color.White)),
                                (image.Width / 2) + random.Next(40), (image.Height / 2) + random.Next(40),
                                watermarkFormat);
                        }
                    }

                    RotateImage(image);

                    return ImageToByteArray(image);
                }
            }
        }

        public byte[] ImageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
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

        public void RotateImage(Image image)
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
        }
    }
}
