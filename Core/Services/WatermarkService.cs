using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;

namespace AskanioPhotoSite.Core.Services
{
    public class WatermarkService : BaseService<Watermark>
    {
        public WatermarkService(IStorage storage) : base (storage) { }

        public override IEnumerable<Watermark> GetAll()
        {
            return _storage.GetRepository<Watermark>().GetAll().ToList();
        }

        public override Watermark GetOne(int id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }

        public override Watermark AddOne(object obj)
        {
            var model = (ImageAttrModel)obj;

            var watermark = new Watermark()
            {
                Id = 0,
                PhotoId = model.PhotoId,
                IsWatermarkApplied = model.IsWatermarkApplied,
                IsWatermarkBlack = model.IsWatermarkBlack,
                IsSignatureApplied = model.IsSignatureApplied,
                IsSignatureBlack = model.IsSignatureBlack,
                IsWebSiteTitleApplied = model.IsWebSiteTitleApplied,
                IsWebSiteTitleBlack = model.IsWebSiteTitleBlack,
                IsRightSide = model.IsRightSide
            };

            var updated = _storage.GetRepository<Watermark>().AddOne(watermark);
            _storage.Commit();
            return updated;
        }

        public override Watermark[] AddMany(object[] obj)
        {
            throw new NotImplementedException();
        }

        public override Watermark UpdateOne(object obj)
        {
            throw new NotImplementedException();
        }

        public override void DeleteOne(int id)
        {
            _storage.GetRepository<Watermark>().DeleteOne(id);
            _storage.Commit();
        }
    }
}
