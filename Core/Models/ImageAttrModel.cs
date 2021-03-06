﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Models
{
    public class ImageAttrModel
    {
        public int Id { get; set; }

        public int PhotoId { get; set; }

        public bool IsWatermarkApplied { get; set; }

        public bool IsWatermarkBlack { get; set; }

        public bool IsSignatureApplied { get; set; }

        public bool IsSignatureBlack { get; set; }

        public bool IsWebSiteTitleApplied { get; set; }

        public bool IsWebSiteTitleBlack { get; set; }

        public bool IsRightSide { get; set; }

        public ImageAttrModel() { }

        public ImageAttrModel(Watermark watermark)
        {
            if (watermark != null)
            {
                Id = watermark.Id;
                PhotoId = watermark.PhotoId;
                IsWatermarkApplied = watermark.IsWatermarkApplied;
                IsWatermarkBlack = watermark.IsWatermarkBlack;
                IsSignatureApplied = watermark.IsSignatureApplied;
                IsSignatureBlack = watermark.IsSignatureBlack;
                IsWebSiteTitleApplied = watermark.IsWebSiteTitleApplied;
                IsWebSiteTitleBlack = watermark.IsWebSiteTitleBlack;
                IsRightSide = watermark.IsRightSide;
            }
        }
    }
}
