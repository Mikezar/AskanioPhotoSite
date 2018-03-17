using AskanioPhotoSite.Core.Infrastructure.ImageHandler;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using System.Collections.Generic;
using System.Web;

namespace AskanioPhotoSite.Core.Services.Abstract
{
    public interface IPhotoService
    {
        IEnumerable<Photo> GetAll();
        Photo GetOne(int id);
        Photo[] AddMany(PhotoUploadModel[] models);
        Photo UpdateOne(Photo entity);
        Photo UpdateOne(PhotoUploadModel model, HttpPostedFileBase file, IImageProcessor processor);
        void DeleteOne(int id);
        void DeleteMany(int[] id);
        IEnumerable<Photo> GetOrphans();
        IEnumerable<Photo> GetRandomPhotos();
        Photo GetRandomPhoto();
        IEnumerable<Photo> GetBackgroundPhotos();
        GalleryPhotoViewModel GetNextPrevPhoto(int id, bool isNext, bool includeTag);
        void ChangeOrder(PhotoSortModel model);
        PhotoUploadListModel Upload(IEnumerable<HttpPostedFileBase> files, PhotoUploadListModel listModel, IImageProcessor processor);
    }
}
