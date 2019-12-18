﻿using System.ComponentModel.DataAnnotations;

namespace CourseWorkDb.Models.Tables
{
    public partial class Output
    {
        public int Id { get; set; }

        [Display(Name = "Планируемый выпуск продукции")]
        [Required(ErrorMessage = "Укажите планируемый выпуск продукции")]
        public int OutputPlan { get; set; }

        [Display(Name = "Фактический выпуск продукции")]
        [Required(ErrorMessage = "Укажите фактический выпуск продукции")]
        public int OutputFact { get; set; }

        [Display(Name = "Квартал")]
        [Required(ErrorMessage = "Укажите квартал")]
        [Range(1, 4, ErrorMessage = "Квартал должен быть равен 1, 2, 3 или 4")]
        public short Quarter { get; set; }

        [Display(Name = "Год")]
        [Required(ErrorMessage = "Укажите год")]
        [Range(1970, 2050, ErrorMessage = "Год должен быть равен не менее 1970 и не более 2050")]
        public short Year { get; set; }

        public int CompanyId { get; set; }
        public int ProductId { get; set; }

        public virtual Company Company { get; set; }
        public virtual Product Product { get; set; }
    }
}
