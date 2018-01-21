using System;
using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Convertors.Concrete
{
    public class TextAttributeConverter : ITextAttributeConverter
    {
        public TextAttributes ConvertTo(TextAttributeModel model)
        {
            return new TextAttributes()
            {
                Id = model.Id,
                WatermarkFont = model.WatermarkFont,
                WatermarkFontSize = model.WatermarkFontSize.Value,
                WatermarkText = model.WatermarkText,
                SignatureFont = model.SignatureFont,
                SignatureFontSize = model.SignatureFontSize.Value,
                SignatureText = model.SignatureText,
                StampFont = model.StampFont,
                StampFontSize = model.StampFontSize.Value,
                StampText = model.StampText,
                Alpha = model.Alpha > 255 ? 255 : model.Alpha
            };
        }

        TextAttributeModel IConverter<TextAttributeModel, TextAttributes>.ConvertTo(TextAttributes text)
        {
            if (text == null) return null;

            return new TextAttributeModel()
            {
                Id = text.Id,
                WatermarkFont = text.WatermarkFont,
                WatermarkFontSize = text.WatermarkFontSize,
                WatermarkText = text.WatermarkText,
                SignatureFont = text.SignatureFont,
                SignatureFontSize = text.SignatureFontSize,
                SignatureText = text.SignatureText,
                StampFont = text.StampFont,
                StampFontSize = text.StampFontSize,
                StampText = text.StampText,
                Alpha = text.Alpha == default(int) ? 80 : text.Alpha
            };
        }
    }
}
