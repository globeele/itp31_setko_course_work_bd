using System.ComponentModel.DataAnnotations;

namespace CourseWorkDb.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "{0} является обязательным полем для заполнения")]
        [EmailAddress(ErrorMessage = "Текст в поле {0} не является правильной записью email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Пользователь с таким {0} уже существует")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} является обязательным полем для заполнения")]
        [StringLength(30, ErrorMessage = "{0} должна содержать минимум {2} и максимум {1} символов", MinimumLength = 2)]
        [RegularExpression("[А-ЯЁA-Z]{1}[а-яА-ЯёЁa-zA-Z-]{1,30}", ErrorMessage = "{0} должна начинаться с большой буквы и содержать только буквы")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "{0} является обязательным полем для заполнения")]
        [StringLength(30, ErrorMessage = "{0} должно содержать минимум {2} и максимум {1} символов", MinimumLength = 2)]
        [RegularExpression("[А-ЯЁA-Z]{1}[а-яА-ЯёЁa-zA-Z-]{1,30}", ErrorMessage = "{0} должно начинаться с большой буквы и содержать только буквы")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} является обязательным полем для заполнения")]
        [StringLength(30, ErrorMessage = "{0} должно содержать минимум {2} и максимум {1} символов", MinimumLength = 2)]
        [RegularExpression("[А-ЯЁA-Z]{1}[а-яА-ЯёЁa-zA-Z-]{1,30}", ErrorMessage = "{0} должно начинаться с большой буквы и содержать только буквы")]
        [DataType(DataType.Text)]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "{0} является обязательным полем для заполнения")]
        [Phone(ErrorMessage = "Текст в поле {0} не является правильной записью номера телефона")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}
