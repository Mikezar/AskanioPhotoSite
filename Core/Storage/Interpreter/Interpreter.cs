using System;
using System.Collections.Generic;
using System.Linq;
using AskanioPhotoSite.Core.Entities;

namespace AskanioPhotoSite.Core.Storage.Interpreter
{
    public class Interpreter<TEntity> : IInterpreter<TEntity>
    {
        public IEnumerable<TEntity> InterpreteToEntity(string source, TEntity entity)
        {
            if (string.IsNullOrEmpty(source)) return new List<TEntity>();

            var lines = source.Split('\r', '\n').Where(x => x != "").ToArray();

            if (entity.GetType() == typeof(Album))
            {
               return GetAlbumEntities(lines).Cast<TEntity>();
            }
            else if (entity.GetType() == typeof(Photo))
            {
                return GetPhotoEntities(lines).Cast<TEntity>();
            }
            else return new List<TEntity>();
        }

        public IEnumerable<string> InterpreteToString(IEnumerable<TEntity> entities, int maxId)
        {
            int counter = ++maxId;
            if (entities.First().GetType() == typeof(Album))
            {
                var result = new List<string>();

                foreach(var entity in entities)
                {

                    entity.GetType().GetProperty("Id").SetValue(entity, counter);
                    counter++;
                    var properties = entity.GetType().GetProperties();
                    var values = properties.Select(x => x.GetValue(entity)).ToArray();
                    var entityString = String.Join("\t", values.Select(c => c).ToArray());
                    result.Add(entityString);
                }

                return result;
            }

            return null;
        }

        public Dictionary<int, string> InterpreteToString(IEnumerable<TEntity> entities)
        {
            if (entities.First().GetType() == typeof(Album))
            {
                var result = new Dictionary<int, string>();

                foreach (var entity in entities)
                {
                    var properties = entity.GetType().GetProperties();
                    var values = properties.Select(x => x.GetValue(entity)).ToArray();
                    var entityString = String.Join("\t", values.Select(c => c).ToArray());
                    result.Add((int)entity.GetType().GetProperty("Id").GetValue(entity), entityString);
                }

                return result;
            }
            return null;
        }

        private IEnumerable<Album> GetAlbumEntities(string[] lines)
        {
            return lines.Select(x =>
            {
                var albumFields = x.Split('\t');
                return new Album()
                {
                    Id = Convert.ToInt32(albumFields[0]),
                    ParentId = Convert.ToInt32(albumFields[1]),
                    TitleRu = albumFields[2],
                    TitleEng = albumFields[3],
                    DescriptionRu = albumFields[4],
                    DescriptionEng = albumFields[5]
                };
            });
        }

        private IEnumerable<Photo> GetPhotoEntities(string[] lines)
        {
            return lines.Select(x =>
            {
                var albumFields = x.Split('\t');
                return new Photo()
                {
                    Id = Convert.ToInt32(albumFields[0]),
                    TitleRu = albumFields[1],
                    TitleEng = albumFields[2],
                    DescriptionRu = albumFields[3],
                    DescriptionEng = albumFields[4],
                    ThumbnailPath = albumFields[5],
                    PhotoPath = albumFields[6],
                    AlbumId = Convert.ToInt32(albumFields[7]),
                };
            });
        }
    }
}
