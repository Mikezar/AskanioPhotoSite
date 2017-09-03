using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Models;

namespace AskanioPhotoSite.Core.Services.Extensions
{
   public static class PhotoToTagServiceExtension
    {
        public static int GetRelatedPhotoCount(this IEnumerable<PhotoToTag> photoToTags, IEnumerable<Photo> photos, int tagId)
        {
            return photoToTags.Where(r => photos.Select(t => t.Id).
                Contains(r.PhotoId)).Where(f => f.TagId == tagId).Count();
        }

        public static IEnumerable<PhotoToTag> GetRelatedTags(this IEnumerable<PhotoToTag> photoToTags, int photoId)
        {
            return photoToTags.Where(x => x.PhotoId == photoId);
        }
    }
}
