using System;
using System.Linq;
using AskanioPhotoSite.WebUI.Properties;
using AskanioPhotoSite.Core.Helpers;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Infrastructure.ImageHandler;

namespace AskanioPhotoSite.WebUI.Infrastructure.Files
{
    public class PhotoManager
    {
        private readonly BaseService<TextAttributes> _textAttrService;
        private readonly BaseService<Watermark> _watermarkService;

        public PhotoManager(BaseService<TextAttributes> textAttrService, BaseService<Watermark>  watermarkService)
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

                var imageAttributes = new ImageAttrModel();
                if (watermark != null)
                {
                    imageAttributes.PhotoId = photoId;
                    imageAttributes.IsWatermarkApplied = watermark.IsWatermarkApplied;
                    imageAttributes.IsWatermarkBlack = watermark.IsWatermarkBlack;
                    imageAttributes.IsSignatureApplied = watermark.IsSignatureApplied;
                    imageAttributes.IsSignatureBlack = watermark.IsSignatureBlack;
                    imageAttributes.IsWebSiteTitleApplied = watermark.IsWebSiteTitleApplied;
                    imageAttributes.IsWebSiteTitleBlack = watermark.IsWebSiteTitleBlack;
                    imageAttributes.IsRightSide = watermark.IsRightSide;
                }

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