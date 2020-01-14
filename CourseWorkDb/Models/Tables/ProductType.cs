using System.ComponentModel.DataAnnotations;

namespace CourseWorkDb.Models.Tables
{
    public partial class ProductType
    {
       
        
        [Display(Name = "Номер")]
        public int Id { get; set; }

        [Display(Name = "Вид продукции")]
        [Required(ErrorMessage = "Укажите вид продукции")]
        [StringLength(50, ErrorMessage = "Максимальная длина вида продукции должна быть 50 символов")]
        public string ProductionType { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}