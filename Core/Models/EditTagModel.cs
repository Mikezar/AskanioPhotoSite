using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AskanioPhotoSite.Core.Models
{
   public class EditTagModel
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название тэга на русс. языке")]
        [MaxLength(100, ErrorMessage = "Длина названия тэга не может превышать 100 символов")]
        public string TitleRu { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Введите название тэга на англ. языке")]
        [MaxLength(100, ErrorMessage = "Длина названия тэга не может превышать 100 символов")]
        public string TitleEng { get; set; }
    }
}
