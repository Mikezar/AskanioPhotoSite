using System.Collections.Generic;
using System.Linq;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Services.Abstract;
using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Core.Services.Providers;
using AskanioPhotoSite.Core.Helpers;
using AskanioPhotoSite.Core.Services.Extensions;

namespace AskanioPhotoSite.Core.Services.Concrete
{
    public sealed class TagService : ITagService
    {
        private readonly IConverterFactory _factory;
        private readonly ITagConverter _converter;
        private readonly BaseProvider<Photo> _providerPhoto;
        private readonly BaseProvider<Tag> _providerTag;
        private readonly BaseProvider<PhotoToTag> _providerPhotoToTag;

        public TagService(IConverterFactory factory, BaseProvider<Tag> provider,
            BaseProvider<PhotoToTag> providerPhotoToTag, BaseProvider<Photo> providerPhoto)
        {
            _providerTag = provider;
            _providerPhoto = providerPhoto;
            _providerPhotoToTag = providerPhotoToTag;
            _factory = factory;
            _converter = _factory.GetConverter<ITagConverter>();
        }

        public IEnumerable<Tag> GetAll()
        {
            return _providerTag.GetAll(); 
        }

        public Tag GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public Tag AddOne(EditTagModel model)
        {
            var tag = _providerTag.AddOne(_converter.ConvertTo(model));
            _providerTag.Commit();
            return tag;
        }

        public Tag UpdateOne(EditTagModel model)
        {
            var tag = _providerTag.UpdateOne(_converter.ConvertTo(model));
            _providerTag.Commit();
            return tag;
        }

        public void DeleteOne(int id)
        {
            var photoToTags = _providerPhotoToTag.GetAll().Where(x => x.TagId == id).ToList();

            if (photoToTags.Count() > 0)
                _providerPhotoToTag.DeleteMany(photoToTags.Select(x => x.Id).ToArray());
            
            _providerTag.DeleteOne(id);
            _providerTag.Commit();
        }

        public IEnumerable<TagCloudModel> GenerateTagCloud()
        {
            var photos = _providerPhoto.GetAll();

            var cloud = _providerPhotoToTag.GetAll().Join(GetAll(), i => i.TagId, o => o.Id, (i, o) => new
            {
                Id = i.TagId,
                TitleRu = o.TitleRu,
                TitleEng = o.TitleEng,
            }).GroupBy(t => t.Id).Select(x => new TagCloudModel()
            {
                Id = x.Key,
                Count = x.Count(),
                TitleRu = x.Select(r => r.TitleRu).FirstOrDefault(),
                TitleEng = x.Select(r => r.TitleEng).FirstOrDefault(),
            });

            if (CultureHelper.IsEnCulture())
                cloud = cloud.OrderBy(x => x.TitleEng).ToList();
            else
                cloud = cloud.OrderBy(x => x.TitleRu).ToList();

            return cloud;
        }

        public GalleryPhotoTagModel ShowPhotoByTag(int id)
        {
            var tag = GetOne(id);
            var photos = _providerPhoto.GetAll().Where(x =>
               _providerPhotoToTag.GetAll().Where(t => t.TagId == id).Select(r => r.PhotoId).Contains(x.Id));

            if (photos.Count() > 0)
            {
                var model = new GalleryPhotoTagModel()
                {
                    TagName = CultureHelper.IsEnCulture() ? tag.TitleEng : tag.TitleRu,
                    Photos = photos.Select(x => new GalleryPhotoModel()
                    {
                        Id = x.Id,
                        Thumbnail = x.ThumbnailPath,
                        Photo = x.PhotoPath
                    })
                };

                return model;
            }

            return new GalleryPhotoTagModel();
        }

        public int GetRelatedPhotoCount(int id)
        {
            return _providerPhotoToTag.GetAll().GetRelatedPhotoCount(_providerPhoto.GetAll(), id);
        }

        public int[] GetRelatedTags(int photoId)
        {
           return _providerPhotoToTag.GetAll().GetRelatedTags(photoId).Select(x => x.TagId).ToArray();
        }
    }
}
