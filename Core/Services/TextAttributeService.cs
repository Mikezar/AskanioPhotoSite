using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Helpers;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;
using AskanioPhotoSite.Data.Storage;

namespace AskanioPhotoSite.Core.Services
{
    public class TextAttributeService : BaseService<TextAttributes>
    {
        public TextAttributeService(IStorage storage) : base (storage) { }

        public override IEnumerable<TextAttributes> GetAll()
        {
            return _storage.GetRepository<TextAttributes>().GetAll().ToList();
        }

        public override TextAttributes GetOne(int id)
        {
            return GetAll().SingleOrDefault(x => x.Id == id);
        }

        public override TextAttributes AddOne(object obj)
        {
            var model = (TextAttributeModel)obj;

            var text = new TextAttributes()
            {
                Id = 0,
                WatermarkFont = model.WatermarkFont,
                WatermarkFontSize = model.WatermarkFontSize.Value,
                WatermarkText = model.WatermarkText,
                SignatureFont = model.SignatureFont,
                SignatureFontSize = model.SignatureFontSize.Value,
                SignatureText = model.SignatureText,
                StampFont = model.StampFont,
                StampFontSize = model.StampFontSize.Value,
                StampText = model.StampText
            };

            var updated = _storage.GetRepository<TextAttributes>().AddOne(text);
            _storage.Commit();
            return updated;
        }

        public override TextAttributes[] AddMany(object[] obj)
        {
            throw new NotImplementedException();
        }

        public override TextAttributes UpdateOne(object obj)
        {
            var model = (TextAttributeModel)obj;

            var text = GetOne(model.Id);

            text.WatermarkFont = model.WatermarkFont;
            text.WatermarkFontSize = model.WatermarkFontSize.Value;
            text.WatermarkText = model.WatermarkText;
            text.SignatureFont = model.SignatureFont;
            text.SignatureFontSize = model.SignatureFontSize.Value;
            text.SignatureText = model.SignatureText;
            text.StampFont = model.StampFont;
            text.StampFontSize = model.StampFontSize.Value;
            text.StampText = model.StampText;

            var updated = _storage.GetRepository<TextAttributes>().UpdateOne(text);
            _storage.Commit();
            return updated;
        }

        public override void DeleteOne(int id)
        {
            _storage.GetRepository<TextAttributes>().DeleteOne(id);
            _storage.Commit();
        }
    }
}
