using System.ComponentModel.DataAnnotations;

namespace CourseWorkDb.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} является обязательным полем для заполнения")]
        [StringLength(30, ErrorMessage = "{0} должен содержать минимум {2} и максимум {1} символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "{0} является обязательным полем для заполнения")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string NewPasswordConfirm { get; set; }
    }
}
