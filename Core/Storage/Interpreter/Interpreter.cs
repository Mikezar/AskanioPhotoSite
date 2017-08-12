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

            if (entity.GetType() == typeof(Album))
            {
                return source.Split('@').Select(x =>
                {
                    var albumFields = x.Split('~');
                    return new Album()
                    {
                        Id = Convert.ToInt32(albumFields[0]),
                        ParentId = Convert.ToInt32(albumFields[1]),
                        TitleRu = albumFields[2],
                        TitleEng = albumFields[3],
                        DescriptionRu = albumFields[4],
                        DescriptionEng = albumFields[5],
                    };
                }).Cast<TEntity>();
            }
            else if (entity.GetType() == typeof(Photo))
            {
                return source.Split('@').Select(x =>
                {
                    var albumFields = x.Split('~');
                    return new Photo()
                    {
                        Id = Convert.ToInt32(albumFields[0]),
                        TitleRu = albumFields[1],
                        TitleEng = albumFields[2],
                        DescriptionRu = albumFields[3],
                        DescriptionEng = albumFields[4],
                        ThumbnailPath = albumFields[5],
                        PhotoPath = albumFields[6],
                    };
                }).Cast<TEntity>();
            }
            else return new List<TEntity>();
        }
    }
}
