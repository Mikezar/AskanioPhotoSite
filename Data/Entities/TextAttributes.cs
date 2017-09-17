using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Data.Entities
{
    public class TextAttributes : Entity
    {
        public int Id { get; set; }
        public string WatermarkFont { get; set; }
        public int WatermarkFontSize { get; set; }
        public string WatermarkText { get; set; }


        public string SignatureFont { get; set; }
        public int SignatureFontSize { get; set; }
        public string SignatureText { get; set; }



        public string StampFont { get; set; }
        public int StampFontSize { get; set; }
        public string StampText { get; set; }
    }
}
