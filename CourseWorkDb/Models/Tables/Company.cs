using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseWorkDb.Models.Tables
{
    public partial class Company
    {
        public Company()
        {
            Output = new HashSet<Output>();
            Release = new HashSet<Release>();
        }

        public int Id { get; set; }

        [Display(Name = "Название предприятия")]
        [Required(ErrorMessage = "Укажите название предприятия")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "Минимальная длина названия организации должна быть 5 символов")]
        public string CompanyName { get; set; }

        [Display(Name = "Форма собственности")]
        [Required(ErrorMessage = "Укажите форму собственности")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Минимальная длина формы собственности должна быть 3 символа")]
        public string FormOfOwnership { get; set; }

        [Display(Name = "Вид деятельности")]
        [Required(ErrorMessage = "Укажите вид деятелности")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Минимальная длина вида деятельности 3 символа")]
        public string ActivityType { get; set; }

        [Display(Name = "ФИО директора")]
        [Required(ErrorMessage = "Укажите ФИО директора")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Минимальная длина ФИО директора должна быть 5 символов")]
        public string HeadName { get; set; }


        public virtual ICollection<Output> Output { get; set; }
        public virtual ICollection<Release> Release { get; set; }
    }
}
