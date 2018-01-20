using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AskanioPhotoSite.Core.Services.Abstract
{
   public interface IAlbumService
    {
        IEnumerable<Album> GetAll();
        Album GetOne(int id);
        Album AddOne(EditAlbumModel model);
        Album UpdateOne(Album entity);
        Album UpdateOne(EditAlbumModel model);
        void DeleteOne(int id);
        IEnumerable<SelectListItem> GetAvailableAlbumSelectList(IEnumerable<Photo> photos);
        GalleryViewModel FillModel();
    }
}
