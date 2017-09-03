using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;

namespace AskanioPhotoSite.Core.Services
{
    public sealed class TagService : BaseService<Tag>
    {
        public TagService(IStorage storage) : base (storage) { }

        public override IEnumerable<Tag> GetAll()
        {
            return _storage.GetRepository<Tag>().GetAll().ToList();
        }

        public override Tag GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override Tag[] AddMany(object[] obj)
        {
            throw new NotImplementedException();
        }

        public override Tag AddOne(object obj)
        {
            var model = (EditTagModel)obj;

            var tag = new Tag()
            {
                Id = 0,
                TitleRu = model.TitleRu,
                TitleEng = model.TitleEng,
            };

            var updated = _storage.GetRepository<Tag>().AddOne(tag);
            _storage.Commit();
            return updated;
        }

        public override Tag UpdateOne(object obj)
        {
            var model = (EditTagModel)obj;

            var tag = GetOne(model.Id);

            tag.TitleEng = model.TitleEng;
            tag.TitleRu = model.TitleRu;

            var updated = _storage.GetRepository<Tag>().UpdateOne(tag);
            _storage.Commit();

            return updated;
        }

        public override void DeleteOne(int id)
        {
            var photoToTagRepository = _storage.GetRepository<PhotoToTag>();

            var photoToTags = photoToTagRepository.GetAll().Where(x => x.TagId == id).ToList();

            if (photoToTags.Count() > 0)
            {
                photoToTagRepository.DeleteMany(photoToTags.Select(x => x.Id).ToArray());
            }

            _storage.GetRepository<Tag>().DeleteOne(id);
            _storage.Commit();
        }
    }
}
