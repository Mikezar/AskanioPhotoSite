using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AskanioPhotoSite.Core.Entities;

namespace AskanioPhotoSite.Core.Models
{
    public class EditAlbumModel
    {
        public int Id { get; set; }

        [Display(Name = "Основной альбом")]
        public Album ParentAlbum { get; set; }

        [Display(Name = "Альбомы")]
        public IEnumerable<SelectListItem> ParentAlbums { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название альбома на русс. языке")]
        [MaxLength(100, ErrorMessage = "Длина названия альбома не может превышать 100 символов")]
        public string TitleRu { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Введите название альбома на англ. языке")]
        [MaxLength(100, ErrorMessage = "Длина названия альбома не может превышать 100 символов")]
        public string TitleEng { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Введите описание альбома на русс. языке")]
        [MaxLength(500, ErrorMessage = "Длина описания альбома не может превышать 500 символов")]
        public string DescriptionRu { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Введите описание альбома на англ. языке")]
        [MaxLength(500, ErrorMessage = "Длина описания альбома не может превышать 500 символов")]
        public string DescriptionEng { get; set; }
    }
}
