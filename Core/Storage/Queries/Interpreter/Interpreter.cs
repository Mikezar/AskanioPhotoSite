using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage.Transactions;

namespace AskanioPhotoSite.Core.Storage.Queries.Interpreter
{
    public class Interpreter<TEntity> : IInterpreter<TEntity>
    {
        public IEnumerable<TEntity> InterpreteToEntity(string[] lines, TEntity entity)
        {
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
                var result = new List<string>();

                foreach(var entity in entities)
                {

                    entity.GetType().GetProperty("Id").SetValue(entity, counter);
                    counter++;
                    var properties = entity.GetType().GetProperties();
                    var values = properties.Select(x => x.GetValue(entity)).ToArray();
                    var entityString = String.Join($"{Transaction<TEntity>.Field}", values.Select(c => c).ToArray());
                    result.Add(entityString);
                }

            return result;
        }

        public Dictionary<int, string> InterpreteToString(IEnumerable<TEntity> entities)
        {
                var result = new Dictionary<int, string>();

                foreach (var entity in entities)
                {
                    var properties = entity.GetType().GetProperties();
                    var values = properties.Select(x => x.GetValue(entity)).ToArray();
                    var entityString = String.Join($"{Transaction<TEntity>.Field}", values.Select(c => c).ToArray());
                    result.Add((int)entity.GetType().GetProperty("Id").GetValue(entity), entityString);
                }

                return result;
        }

        private IEnumerable<Album> GetAlbumEntities(string[] lines)
        {
            return lines.Select(x =>
            {
                var albumFields = x.Split(Transaction<TEntity>.Field);
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
                var albumFields = x.Split(Transaction<TEntity>.Field);
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
