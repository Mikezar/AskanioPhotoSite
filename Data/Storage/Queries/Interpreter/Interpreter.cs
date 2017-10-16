using System;
using System.Collections.Generic;
using System.Linq;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Helpers;
using AskanioPhotoSite.Data.Storage.Transactions;

namespace AskanioPhotoSite.Data.Storage.Queries.Interpreter
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
            else if (entity.GetType() == typeof(Tag))
            {
                return GetTagEntities(lines).Cast<TEntity>();
            }
            else if (entity.GetType() == typeof(PhotoToTag))
            {
                return GetPhotoToTagEntities(lines).Cast<TEntity>();
            }
            else if (entity.GetType() == typeof(TextAttributes))
            {
                return GetTextAttributes(lines).Cast<TEntity>();
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
                    var entityString = String.Join($"{Processor<TEntity>.Field}", values.Select(c => c).ToArray());
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
                    var entityString = String.Join($"{Processor<TEntity>.Field}", values.Select(c => c).ToArray());
                    result.Add((int)entity.GetType().GetProperty("Id").GetValue(entity), entityString);
                }

                return result;
        }

        private IEnumerable<Album> GetAlbumEntities(string[] lines)
        {
            return lines.Select(x =>
            {
                var albumFields = x.Split(Processor<TEntity>.Field);
                return new Album()
                {
                    Id = albumFields[0].GetValue(),
                    ParentId = albumFields[1].GetValue(),
                    TitleRu = albumFields[2],
                    TitleEng = albumFields[3],
                    DescriptionRu = albumFields[4],
                    DescriptionEng = albumFields[5],
                    CoverPath = albumFields[6]
                };
            });
        }

        private IEnumerable<Photo> GetPhotoEntities(string[] lines)
        {
            return lines.Select(x =>
            {
                var photoFields = x.Split(Processor<TEntity>.Field);
                return new Photo()
                {
                    Id = photoFields[0].GetValue(),
                    AlbumId = photoFields[1].GetValue(),
                    TitleRu = photoFields[2],
                    TitleEng = photoFields[3],
                    DescriptionRu = photoFields[4],
                    DescriptionEng = photoFields[5],
                    PhotoPath = photoFields[6],
                    ThumbnailPath = photoFields[7],
                    FileName = photoFields[8],
                    CreationDate =  Convert.ToDateTime(photoFields[9]),
                    ShowRandom = Convert.ToBoolean(photoFields[10])
                };
            });
        }

        private IEnumerable<Tag> GetTagEntities(string[] lines)
        {
            return lines.Select(x =>
            {
                var tagFields = x.Split(Processor<TEntity>.Field);
                return new Tag()
                {
                    Id = tagFields[0].GetValue(),
                    TitleRu = tagFields[1],
                    TitleEng = tagFields[2]
                };
            });
        }

        private IEnumerable<PhotoToTag> GetPhotoToTagEntities(string[] lines)
        {
            return lines.Select(x =>
            {
                var tagFields = x.Split(Processor<TEntity>.Field);
                return new PhotoToTag()
                {
                    Id = tagFields[0].GetValue(),
                    PhotoId = tagFields[1].GetValue(),
                    TagId = tagFields[2].GetValue()
                };
            });
        }

        private IEnumerable<TextAttributes> GetTextAttributes(string[] lines)
        {
            return lines.Select(x =>
            {
                var textFields = x.Split(Processor<TEntity>.Field);
                return new TextAttributes()
                {
                    Id = textFields[0].GetValue(),
                    WatermarkFont = textFields[1],
                    WatermarkFontSize = textFields[2].GetValue(),
                    WatermarkText = textFields[3],
                    SignatureFont = textFields[4],
                    SignatureFontSize = textFields[5].GetValue(),
                    SignatureText = textFields[6],
                    StampFont = textFields[7],
                    StampFontSize = textFields[8].GetValue(),
                    StampText = textFields[9]
                };
            });
        }

    }
}
