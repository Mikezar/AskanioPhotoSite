using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Helpers;
using System;

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

        public static IEnumerable<Album> GetNoPhotoAlbums(this IEnumerable<Album> albums, IEnumerable<Photo> photos)
        {
            var albumIds = photos.Select(t => t.AlbumId).ToList();
            return albums.Where(x => !albumIds.Contains(x.Id));
        }

        public static IEnumerable<Tuple<int, string>> BuildGraph(this IEnumerable<Album> albums, Album currentAlbum)
        {
            if (currentAlbum == null) return new List<Tuple<int, string>>();

            var graph = GetParents(new List<Album>(), albums, currentAlbum.ParentId);

            return graph.Select(x => new Tuple<int, string>(x.Id, CultureHelper.IsEnCulture() ? x.TitleEng : x.TitleRu))
                        .Union(new List<Tuple<int, string>>(){
                            new Tuple<int, string>(currentAlbum.Id, CultureHelper.IsEnCulture() ? currentAlbum.TitleEng : currentAlbum.TitleRu)});
        }

        public static IEnumerable<Album> GetParents(List<Album> collection, IEnumerable<Album> albums, int parentId)
        {
            var parent = albums.SingleOrDefault(x => x.Id == parentId);

            if (parent != null)
            {
                collection.Insert(0,parent);
                return GetParents(collection, albums, parent.ParentId);
            }
            else
                return collection;
        }

        public static string GetAlbumCover(this IEnumerable<Album> albums, Album currentAlbum)
        {
            string cover = null;

            if(!string.IsNullOrEmpty(currentAlbum.CoverPath))
            {
                cover = currentAlbum.CoverPath;
            }
            else
            {
                var childs = albums.GetChilds(null, currentAlbum.Id);

                if (childs.Any())
                {
                    var withCovers = childs.Where(r => !string.IsNullOrEmpty(r.CoverPath)).ToList();

                    if(withCovers.Any())
                    {
                        int seed = new Random().Next(withCovers.Count() - 1);
                        cover = withCovers.ElementAt(seed)?.CoverPath;
                    }
                }
            }

            return cover;
        }

        public static IEnumerable<Album> GetChilds(this IEnumerable<Album> albums, List<Album> collection, int albumId)
        {
            if (collection == null) collection = new List<Album>();

            var childs = albums.Where(x => x.ParentId == albumId).ToList();
            
            foreach(var child in childs)
            {
                if (child == null) continue;

                collection.Add(child);
                GetChilds(albums, collection, child.Id);
            }

            return collection;
        }
    }
}
