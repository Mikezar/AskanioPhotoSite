using System.Linq;
using System.Collections.Generic;
using AskanioPhotoSite.Core.Convertors.Abstract;
using AskanioPhotoSite.Core.Models;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Convertors.Concrete
{
    public class WatermarkConverter : IWatermarkConverter
    {
        public Watermark ConvertTo(ImageAttrModel model)
        {
            return new Watermark()
            {
                Id = model.Id,
                PhotoId = model.PhotoId,
                IsWatermarkApplied = model.IsWatermarkApplied,
                IsWatermarkBlack = model.IsWatermarkBlack,
                IsSignatureApplied = model.IsSignatureApplied,
                IsSignatureBlack = model.IsSignatureBlack,
                IsWebSiteTitleApplied = model.IsWebSiteTitleApplied,
                IsWebSiteTitleBlack = model.IsWebSiteTitleBlack,
                IsRightSide = model.IsRightSide
            };
        }

        public IEnumerable<Watermark> ConvertToIEnumerable(ImageAttrModel[] models)
        {
            return models.Select(x => ConvertTo(x));
        }
    }
}
