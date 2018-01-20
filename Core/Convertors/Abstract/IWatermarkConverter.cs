using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Convertors.Abstract
{
    public interface IWatermarkConverter : IConverter<Watermark, ImageAttrModel>, IEnumerableConverter<Watermark, ImageAttrModel> { }
}
