using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Helpers;

namespace AskanioPhotoSite.Core.Services.Extensions
{
    public static class AlbumServiceExtension
    {
        public static IEnumerable<SelectListItem> GetSelectListItem(this IEnumerable<Album> albums)
        {
            return albums.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.TitleRu,
            }).InsertEmptyFirst("None", "0").ToList();
        }

        public static IEnumerable<Album> GetEndNodeAlbums(this IEnumerable<Album> albums)
        {
            return albums.Where(x => albums.isParent(x) == false);
        }

        public static bool isParent(this IEnumerable<Album> albums, Album album)
        {
            return albums.Any(x => x.ParentId == album.Id);
        }
    }
}
