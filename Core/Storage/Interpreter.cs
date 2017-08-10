using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage.Interpreter;

namespace AskanioPhotoSite.Core.Storage
{
    public class Interpreter<TEntity> : IInterpreter<TEntity>
    {
        public IEnumerable<TEntity> InterpreteToEntity(string source, TEntity entity)
        {
            if (string.IsNullOrEmpty(source)) return new List<TEntity>();

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
    }
}
