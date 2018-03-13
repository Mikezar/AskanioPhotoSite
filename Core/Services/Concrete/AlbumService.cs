using System.Collections.Generic;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using System.Linq;
using AskanioPhotoSite.Core.Services.Providers;
using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Core.Services.Abstract;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Services.Extensions;
using AskanioPhotoSite.Core.Helpers;
using System;

namespace AskanioPhotoSite.Core.Services.Concrete
{
    public class AlbumService : IAlbumService
    {
        private readonly IConverterFactory _factory;
        private readonly IAlbumConverter _converter;
        private readonly BaseProvider<Album> _providerAlbum;
        private readonly IPhotoService _photoService;

        public AlbumService(IConverterFactory factory, BaseProvider<Album> providerAlbum, IPhotoService photoService)
        {
            _providerAlbum = providerAlbum;
            _factory = factory;
            _photoService = photoService;
            _converter = _factory.GetConverter<IAlbumConverter>();
        }

        public IEnumerable<Album> GetAll()
        {
            return _providerAlbum.GetAll();
        }

        public Album GetOne(int id)
        {
            var album = _providerAlbum.GetOne(id);

            if (album == null) return null;

            return new Album()
            {
                Id = album.Id,
                ParentId = album.ParentId,
                DescriptionEng = album.DescriptionEng,
                DescriptionRu = album.DescriptionRu,
                TitleEng = album.TitleEng,
                TitleRu = album.TitleRu,
                CoverPath = album.CoverPath,
                ViewPattern = album.ViewPattern
            };
        }

        public Album AddOne(EditAlbumModel model)
        {
           var album =  _providerAlbum.AddOne(_converter.ConvertTo(model));
           _providerAlbum.Commit();
           return album;
        }

        public Album UpdateOne(Album entity)
        {
            var album = _providerAlbum.UpdateOne(entity);
            _providerAlbum.Commit();
            return album;
        }

        public Album UpdateOne(EditAlbumModel model)
        {
            return UpdateOne(_converter.ConvertTo(model));
        }

        public void DeleteOne(int id)
        {
            var photos = _photoService.GetAll().Where(x => x.AlbumId == id);

            if (photos.Count() > 0)
                _photoService.DeleteMany(photos.Select(x => x.Id).ToArray());

            _providerAlbum.DeleteOne(id);
            _providerAlbum.Commit();
        }

        public IEnumerable<SelectListItem> GetAvailableAlbumSelectList(IEnumerable<Photo> photos)
        {
            return _providerAlbum.GetAll().GetNoPhotoAlbums(photos).GetSelectListItem();
        }

        public GalleryViewModel FillModel()
        {
            var albums = GetAll();

            var model = new GalleryViewModel()
            {
                Albums = albums.Where(t => t.ParentId == 0).Select(x => new GalleryAlbumModel()
                {
                    Id = x.Id,
                    Title = CultureHelper.IsEnCulture() ? x.TitleEng : x.TitleRu,
                    Description = CultureHelper.IsEnCulture() ? x.DescriptionEng : x.DescriptionRu,
                    Cover = GetAll().GetAlbumCover(x)
                    //Cover = !string.IsNullOrEmpty(x.CoverPath) ? x.CoverPath : albums.Where(f => f.ParentId == x.Id).SingleOrDefault(r =>
                    //{
                    //    var childs = albums.Where(f => f.ParentId == x.Id);
                    //    return childs.ElementAt(new Random().Next(0, childs.Count())).CoverPath != null;
                    //})?.CoverPath
                })
            };

            return model;
        }
    }
}
