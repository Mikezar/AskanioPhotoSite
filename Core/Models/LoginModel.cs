using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskanioPhotoSite.Core.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [DisplayName("Логин")]
        [MaxLength(20, ErrorMessage = "Поле не может превышать 20 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DisplayName("Пароль")]
        [MaxLength(20, ErrorMessage = "Поле не может превышать 20 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Error { get; set; }

        public string ReturnUrl { get; set; }
    }
}
