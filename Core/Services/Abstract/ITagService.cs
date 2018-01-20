using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Services.Abstract
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAll();
        Tag GetOne(int id);
        Tag AddOne(EditTagModel model);
        Tag UpdateOne(EditTagModel model);
        void DeleteOne(int id);
        IEnumerable<TagCloudModel> GenerateTagCloud();
        GalleryPhotoTagModel ShowPhotoByTag(int id);
        int GetRelatedPhotoCount(int id);
        int[] GetRelatedTags(int photoId);
    }
}
