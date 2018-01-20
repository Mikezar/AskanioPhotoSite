using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using System.Collections.Generic;

namespace AskanioPhotoSite.Core.Services.Abstract
{
    public interface IWatermarkService
    {
        IEnumerable<Watermark> GetAll();
        Watermark GetOne(int id);
        Watermark AddOne(ImageAttrModel model);
        void DeleteOne(int id);
    }
}
