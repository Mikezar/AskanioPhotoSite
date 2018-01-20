using AskanioPhotoSite.Core.Convertors.Abstract;
using System.Linq;
using System.Collections.Generic;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Convertors.Concrete
{
    public class PhotoConverter : IPhotoConverter
    {
        public Photo ConvertTo(PhotoUploadModel model)
        {
            return new Photo()
            {
                Id = model.Id,
                AlbumId = model.Album?.Id ?? 0,
                TitleRu = model.TitleRu,
                TitleEng = model.TitleEng,
                DescriptionRu = model.DescriptionRu,
                DescriptionEng = model.DescriptionEng,
                PhotoPath = model.PhotoPath,
                ThumbnailPath = model.ThumbnailPath,
                FileName = model.FileName,
                CreationDate = model.CreationDate,
                ShowRandom = model.ShowRandom,
                IsForBackground = model.IsForBackground,
                Order = model.Order
            };
        }

        public IEnumerable<Photo> ConvertToIEnumerable(PhotoUploadModel[] models)
        {
            return models.Select(x => ConvertTo(x));
        }
    }
}
