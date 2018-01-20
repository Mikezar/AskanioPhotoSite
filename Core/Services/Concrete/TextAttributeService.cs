using System.Collections.Generic;
using System.Linq;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Core.Services.Abstract;
using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Core.Services.Providers;

namespace AskanioPhotoSite.Core.Services.Concrete
{
    public class TextAttributeService : ITextAttributeService
    {
        private readonly IConverterFactory _factory;
        private readonly ITextAttributeConverter _converter;
        private readonly BaseProvider<TextAttributes> _providerAttr;
        private readonly IPhotoService _photoService;

        public TextAttributeService(IConverterFactory factory, BaseProvider<TextAttributes> provider)
        {
            _providerAttr = provider;
            _factory = factory;
            _converter = _factory.GetConverter<ITextAttributeConverter>();
        }

        public IEnumerable<TextAttributes> GetAll()
        {
            return _providerAttr.GetAll().ToList();
        }

        public TextAttributes GetOne(int id)
        {
            return _providerAttr.GetOne(id);
        }

        public TextAttributes AddOne(TextAttributeModel model)
        {
            var attr = _providerAttr.AddOne(_converter.ConvertTo(model));
            _providerAttr.Commit();
            return attr;
        }

        public TextAttributes UpdateOne(TextAttributeModel model)
        {
            var attr = _providerAttr.UpdateOne(_converter.ConvertTo(model));
            _providerAttr.Commit();
            return attr;
        }

        public void DeleteOne(int id)
        {
            _providerAttr.DeleteOne(id);
            _providerAttr.Commit();
        }
    }
}
