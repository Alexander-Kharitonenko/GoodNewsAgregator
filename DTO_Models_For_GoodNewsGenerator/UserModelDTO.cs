using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO_Models_For_GoodNewsGenerator
{
    public class UserModelDTO
    {
        public Guid Id { get; set; }


        public Guid RoleId { get; set; }
        public virtual RoleModelDTO Roles { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Не указано имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Не указана фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Не указан логин или такой логин уже есть")]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Не указан Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указан повтороно Пароль")]
        [Compare("Password" , ErrorMessage ="Не совпадение паролей")]
        [DataType(DataType.Password)]
        public string PasswordСonfirm { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Не указан Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
