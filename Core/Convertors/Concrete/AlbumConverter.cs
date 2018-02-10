using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Models;

namespace AskanioPhotoSite.Core.Convertors.Concrete
{
    public class AlbumConverter : IAlbumConverter
    {
        public Album ConvertTo(EditAlbumModel model)
        {
            return new Album()
            {
                Id = model.Id,
                ParentId = model.ParentAlbum?.Id ?? 0,
                TitleRu = model.TitleRu,
                TitleEng = model.TitleEng,
                DescriptionEng = model.DescriptionEng,
                DescriptionRu = model.DescriptionRu,
                ViewPattern = model.ViewPattern ?? 0,
                CoverPath = model.CoverPath
            };
        }
    }
}
