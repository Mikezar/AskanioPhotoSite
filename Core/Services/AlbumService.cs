using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Core.Entities;
using AskanioPhotoSite.Core.Storage;

namespace AskanioPhotoSite.Core.Services
{
    public class AlbumService : BaseService<Album>
    {
        public AlbumService(IStorage storage) : base (storage) { }

        public override IEnumerable<Album> GetAll()
        {
            if (Cache == null) Cache = GetCache();

            return Cache.GetEntities<Album>().AsEnumerable();
        }
    }
}
