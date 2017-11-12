using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AskanioPhotoSite.Core.Helpers;
using AskanioPhotoSite.Core.Services;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;
using AskanioPhotoSite.Core.Models;

namespace AskanioPhotoSite.WebUI.Code.Files
{
    public class PhotoManager
    {
        /// <summary>
        /// Получение фотографии
        /// </summary>
        /// <param name="photoId">Идентификатор фотографии</param>
        /// <returns></returns>
        public static byte[] GetPhoto(int photoId, string photoPath)
        {
            try
            {
               var storage = new Storage();
               BaseService<TextAttributes> textAttrService = new TextAttributeService(storage);
               BaseService<Watermark> watermarkService = new WatermarkService(storage);
               var textAttributes = textAttrService.GetAll().FirstOrDefault();
               var watermark = watermarkService.GetAll().FirstOrDefault(x => x.PhotoId == photoId);

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

               return ImageProcessor.WatermarkImage(photoPath, imageAttributes, textAttributes);
            }
            catch (Exception ex)
            {
                Log.RegisterError(ex);
                return null;
            }
        }
    }
}