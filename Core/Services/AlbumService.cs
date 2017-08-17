using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Core.Storage;

namespace AskanioPhotoSite.Core.Services
{
    public class AlbumService : BaseService<Album>
    {
        public AlbumService(IStorage storage) : base (storage) { }

        public override IEnumerable<Album> GetAll()
        {
            return _storage.GetRepository<Album>().GetAll();
        }

        public override Album GetOne(int id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }

        public override Album AddOne(object obj)
        {
            var model = (EditAlbumModel)obj;

            var album = new Album()
            {
                Id = 0,
                ParentId = model.ParentAlbum.Id,
                TitleRu = model.TitleRu,
                TitleEng = model.TitleEng,
                DescriptionEng = model.DescriptionEng,
                DescriptionRu = model.DescriptionRu
            };

           return _storage.GetRepository<Album>().AddOne(album);
        }

        public override Album UpdateOne(object obj)
        {
            var model = (EditAlbumModel)obj;

            var album = GetOne(model.Id);

            album.DescriptionEng = model.DescriptionEng;
            album.DescriptionRu = model.DescriptionRu;
            album.TitleEng = model.TitleEng;
            album.TitleRu = model.TitleRu;
            album.ParentId = model.ParentAlbum?.Id ?? 0;

            return _storage.GetRepository<Album>().UpdateOne(album);
        }

        public override void DeleteOne(int id)
        {
            _storage.GetRepository<Album>().DeleteOne(id);
        }

        public override IEnumerable<SelectListItem> GetSelectListItem()
        {
            var albums = GetAll().ToList();

            return albums.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.TitleRu
            }).ToList();
        }
    }
}
