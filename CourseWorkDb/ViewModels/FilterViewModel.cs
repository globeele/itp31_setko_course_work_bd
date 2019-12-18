using CourseWorkDb.Models.Tables;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CourseWorkDb.ViewModels
{
    public class FilterViewModel
    {
        public SelectList Companies { get; private set; }
        public int? SelectedCompany { get; private set; }
        public SelectList Years { get; private set; }
        public int? SelectedYear { get; private set; }

        public FilterViewModel(List<Company> companies, int? company, List<Year> years, int? year)
        {
            companies.Insert(0, new Company { CompanyName = "Все", Id = 0 });
            Companies = new SelectList(companies, "Id", "Name", company);
            SelectedCompany = company;

            years.Insert(0, new Year { Name = "Все", Number = 0 });
            Years = new SelectList(years, "Number", "Name", year);
            SelectedYear = year;
        }


        public class Year
        {
            public int Number { get; set; }
            public string Name { get; set; }
        }
    }
}
