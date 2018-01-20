using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Convertors.Abstract
{
    public interface ITagConverter : IConverter<Tag, EditTagModel>, 
                                     IEnumerableConverter<PhotoToTag, PhotoUploadModel>
    { 

    }
}
