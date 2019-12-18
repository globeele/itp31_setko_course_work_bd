using System.ComponentModel.DataAnnotations;

namespace CourseWorkDb.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} является обязательным полем для заполнения")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} является обязательным полем для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
