using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Convertors.Abstract
{
    public interface IPhotoConverter : IConverter<Photo, PhotoUploadModel>, IEnumerableConverter<Photo, PhotoUploadModel> { }
}
