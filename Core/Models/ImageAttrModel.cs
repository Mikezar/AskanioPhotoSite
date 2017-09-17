using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
    public class ImageAttrModel
    {

        public bool IsWatermarkApplied { get; set; }

        public bool IsWatermarkBlack { get; set; }

        public bool IsSignatureApplied { get; set; }

        public bool IsSignatureBlack { get; set; }

        public bool IsWebSiteTitleApplied { get; set; }

        public bool IsWebSiteTitleBlack { get; set; }

        public bool IsRightSide { get; set; }
    }
}
