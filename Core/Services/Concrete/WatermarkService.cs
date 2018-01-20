using System.Collections.Generic;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Core.Services.Providers;
using AskanioPhotoSite.Core.Services.Abstract;

namespace AskanioPhotoSite.Core.Services.Concrete
{
    public class WatermarkService : IWatermarkService
    {
        private readonly IConverterFactory _factory;
        private readonly IWatermarkConverter _converter;
        private readonly BaseProvider<Watermark> _providerWatermark;

        public WatermarkService(IConverterFactory factory, BaseProvider<Watermark> provider)
        {
            _providerWatermark = provider;
            _factory = factory;
            _converter = _factory.GetConverter<IWatermarkConverter>();
        }

        public IEnumerable<Watermark> GetAll()
        {
            return _providerWatermark.GetAll();
        }

        public Watermark GetOne(int id)
        {
            return _providerWatermark.GetOne(id);
        }

        public Watermark AddOne(ImageAttrModel model)
        {
           var watermark = _providerWatermark.AddOne(_converter.ConvertTo(model));
           _providerWatermark.Commit();
           return watermark;
        }

        public void DeleteOne(int id)
        {
            _providerWatermark.DeleteOne(id);
            _providerWatermark.Commit();
        }
    }
}
