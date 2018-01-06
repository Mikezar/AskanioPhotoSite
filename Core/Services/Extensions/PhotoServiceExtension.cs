using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Models;

namespace AskanioPhotoSite.Core.Services.Extensions
{
   public static class PhotoServiceExtension
    {
        public static int GetPhotoCountInAlbum(this IEnumerable<Photo> photos, int albumId)
        {
            return photos.Where(t => t.AlbumId == albumId).Count();
        }


        public static IEnumerable<Photo> GetOrphans(this BaseService<Photo> photoService)
        {
            return photoService.GetAll().Where(r => r.AlbumId == 0);
        }

        public static IEnumerable<Photo> GetRandomPhotos(this BaseService<Photo> photoService)
        {
            return photoService.GetAll().Where(x => x.ShowRandom == true);
        }

        public static List<PhotoModel> InitPhotoListModel(this IEnumerable<Photo> photos)
        {
            return photos.Select(x => new PhotoModel()
            {
                Id = x.Id,
                DescriptionEng = x.DescriptionEng,
                DescriptionRu = x.DescriptionRu,
                PhotoPath = x.PhotoPath,
                ThumbnailPath = x.ThumbnailPath,
                TitleRu = x.TitleRu,
                TitleEng = x.TitleEng,
                Album = new Album()
            }).ToList();
        }

        public static Photo GetRandomPhoto(this BaseService<Photo> photoService)
        {
            Random random = new Random();

            var photos = photoService.GetAll().Where(x => x.ShowRandom == true).ToList();

            if (photos.Count > 0)
            {
                return photos[random.Next(0, photos.Count)];
            }
            return null;
        }

        public static IEnumerable<Photo> GetBackgroundPhotos(this BaseService<Photo> photoService)
        {
            return photoService.GetAll().Where(x => x.IsForBackground == true);
        }
    }
}
