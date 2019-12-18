using System.Collections.Generic;

namespace CourseWorkDb.ViewModels
{
    public class IndexViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
    }
}
