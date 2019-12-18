using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseWorkDb.Models.Tables
{
    public partial class Product
    {
        public Product()
        {
            Output = new HashSet<Output>();
            Release = new HashSet<Release>();
            ProductType = new HashSet<ProductType>();
        }

        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Наименование продукции")]
        [Required(ErrorMessage = "Укажите наименование продукции")]
        [StringLength(50, ErrorMessage = "Максимальная длина наименования продукции должна быть 50 символов")]
        public string ProductName{ get; set; }

        [Display(Name = "Единица измерения")]
        [Required(ErrorMessage = "Укажите единицу измерения")]
        [StringLength(50, ErrorMessage = "Максимальная длина единицы измерения должна быть 50 символов")]
        public string MeasureUnit { get; set; }

        [Display(Name = "Описание продукта")]
        [Required(ErrorMessage = "Укажите описание продукта")]
        [StringLength(50, ErrorMessage = "Максимальная длина описания продукта должна быть 50 символов")]
        public string Features { get; set; }

        [Display(Name = "Фото продукта")]
        public byte[] Photo { get; set; }
        

        public virtual ICollection<Output> Output { get; set; }
        public virtual ICollection<Release> Release { get; set; }
        public virtual ICollection<ProductType> ProductType { get; set; }
    }
}
