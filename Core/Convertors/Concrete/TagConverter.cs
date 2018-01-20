using System.Linq;
using System.Collections.Generic;
using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Convertors.Concrete
{
    public class TagConverter : ITagConverter
    {
        public Tag ConvertTo(EditTagModel model)
        {
            return new Tag()
            {
                Id = model.Id,
                TitleRu = model.TitleRu,
                TitleEng = model.TitleEng,
            };
        }

        public IEnumerable<PhotoToTag> ConvertToIEnumerable(PhotoUploadModel[] models)
        {
            var list = new List<PhotoToTag>();

            foreach (var photo in models)
            {
                foreach (var tagId in photo.RelatedTagIds ?? new int[0])
                {
                    list.Add(new PhotoToTag()
                    {
                        Id = 0,
                        PhotoId = photo.Id,
                        TagId = tagId
                    });
                }
            }
            return list;   
        }
    }
}
