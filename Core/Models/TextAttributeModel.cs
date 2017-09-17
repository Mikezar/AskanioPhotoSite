using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AskanioPhotoSite.Core.Models
{
    public class TextAttributeModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Шрифт водяного знака")]
        public string WatermarkFont { get; set; }

        [Required]
        [Display(Name = "Текст водяного знака")]
        public string WatermarkText { get; set; }

        [Required]
        [Display(Name = "Кегль водяного знака")]
        public int WatermarkFontSize { get; set; }


        [Required]
        [Display(Name = "Шрифт подписи")]
        public string SignatureFont { get; set; }

        [Required]
        [Display(Name = "Текст подписи")]
        public string SignatureText { get; set; }

        [Required]
        [Display(Name = "Кегль подписи")]
        public int SignatureFontSize { get; set; }


        [Required]
        [Display(Name = "Шрифт печати сайта")]
        public string StampFont { get; set; }

        [Required]
        [Display(Name = "Текст печати сайта")]
        public string StampText { get; set; }

        [Required]
        [Display(Name = "Кегль печати сайта")]
        public int StampFontSize { get; set; }
    }
}
