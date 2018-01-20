using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Repositories;
using AskanioPhotoSite.Data.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AskanioPhotoSite.Core.Services.Providers
{
   public class AlbumProvider : BaseProvider<Album>
    {
        private readonly IRepository<Album> _repositoryAlbum;

        public AlbumProvider(IStorage storage) : base (storage)
        {
            _repositoryAlbum = _storage.GetRepository<Album>(); 
        }

        public override IEnumerable<Album> GetAll()
        {
            return _repositoryAlbum.GetAll().ToList();
        }

        public override Album GetOne(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public override Album AddOne(Album entity)
        {
           return _repositoryAlbum.AddOne(entity);
        }

        public override Album[] AddMany(Album[] entities)
        {
            return _repositoryAlbum.AddMany(entities);
        }

        public override Album UpdateOne(Album entity)
        {
            return _repositoryAlbum.UpdateOne(entity);
        }

        public override void DeleteOne(int id)
        {
            _repositoryAlbum.DeleteOne(id);
        }

        public override void Commit()
        {
            base.Commit();
        }

        public override void DeleteMany(int[] ids)
        {
            _repositoryAlbum.DeleteMany(ids);
        }
    }
}
