using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Convertors.Abstract
{
    public interface ITextAttributeConverter : IConverter<TextAttributes, TextAttributeModel>,
    IConverter<TextAttributeModel, TextAttributes> { }
}
