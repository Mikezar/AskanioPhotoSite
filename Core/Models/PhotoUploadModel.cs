using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using AskanioPhotoSite.Data.Entities;

namespace AskanioPhotoSite.Core.Models
{
    public class PhotoUploadModel
    {
        public int Id { get; set; }

        public string PhotoPath { get; set; }

        public string ThumbnailPath { get; set; }

        public string FileName { get; set; }

        [Display(Name = "Название")]
        [MaxLength(100, ErrorMessage = "Длина названия фотографии не может превышать 100 символов")]
        public string TitleRu { get; set; }

        [Display(Name = "Title")]
        [MaxLength(100, ErrorMessage = "Длина названия фотографии не может превышать 100 символов")]
        public string TitleEng { get; set; }

        [Display(Name = "Описание")]
        [MaxLength(500, ErrorMessage = "Длина описания фотографии не может превышать 500 символов")]
        public string DescriptionRu { get; set; }

        [Display(Name = "Description")]
        [MaxLength(500, ErrorMessage = "Длина описания фотографии не может превышать 500 символов")]
        public string DescriptionEng { get; set; }

        [Display(Name = "Альбом")]
        public Album Album{ get; set; }

        public IEnumerable<SelectListItem> Albums { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
    }

}
