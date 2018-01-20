using System;
using System.Linq;
using AskanioPhotoSite.WebUI.Properties;
using AskanioPhotoSite.Core.Helpers;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Infrastructure.ImageHandler;
using AskanioPhotoSite.Core.Services.Abstract;

namespace AskanioPhotoSite.WebUI.Infrastructure.Files
{
    public class PhotoManager
    {
        private readonly ITextAttributeService _textAttrService;
        private readonly IWatermarkService _watermarkService;

        public PhotoManager(ITextAttributeService textAttrService, IWatermarkService watermarkService)
        {
            _textAttrService = textAttrService;
            _watermarkService = watermarkService;
        }

        /// <summary>
        /// Получение фотографии
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <returns></returns>
        public  byte[] GetPhoto(int photoId, string photoPath)
        {
            try
            {
               var textAttributes = _textAttrService.GetAll().FirstOrDefault();
               var watermark = _watermarkService.GetAll().FirstOrDefault(x => x.PhotoId == photoId);

                if (textAttributes == null)
                    textAttributes = new Data.Entities.TextAttributes()
                    {
                        WatermarkFont = "Bell MT",
                        WatermarkFontSize = 60,
                        WatermarkText = "AlexSilver.Photo@gmail.com",
                        SignatureFont = "Edwardian Script ITC",
                        SignatureFontSize = 43,
                        SignatureText = "© Alexander Serebryakov",
                        StampFont = "Bell MT",
                        StampFontSize = 45,
                        StampText = "www.askanio.ru",
                        Alpha = 80
                    };

               var imageAttributes = new ImageAttrModel(watermark);

               return new ImageProcessor(photoPath, Settings.Default.ThumbPath, textAttributes).Watermark(imageAttributes);
            }
            catch (Exception ex)
            {
                Log.RegisterError(ex);
                return null;
            }
        }
    }
}