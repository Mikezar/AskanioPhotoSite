using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AskanioPhotoSite.WebUI.Code.Files
{
    public class PhotoManager
    {
        /// <summary>
        /// Получение фотографии
        /// </summary>
        /// <param name="photoId">Ключ фотографии</param>
        /// <param name="isOriginalSize">Оригинальный размер (в противном случае постер)</param>
        /// <returns></returns>
        public static byte[] GetPhoto(Int32 photoId, Boolean isOriginalSize)
        {
            // TODO: Реализовать возврат фотографии. Если оригинальный размер, то с учётом настроек наложения водяных знаков, подписей и пр.
            return null;
            //try
            //{
            //    byte[] img = null;
            //    if (photoId != 0)
            //        img = LoadPhoto(photoId, isOriginalSize);
            //    if (img == null)
            //        return GetDefaultPhoto(isOriginalSize);
            //    return img;
            //}
            //catch (Exception ex)
            //{
            //    LogError.SaveError(ex);
            //    return GetDefaultPhoto(isOriginalSize);
            //}
        }
    }
}