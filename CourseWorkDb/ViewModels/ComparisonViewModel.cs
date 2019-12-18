namespace CourseWorkDb.ViewModels
{
    public class ComparisonViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string MeasureUnit { get; set; }
        public short Year { get; set; }
        public short Quarter { get; set; }
        public float ReleasePlan { get; set; }
        public float ReleaseFact{ get; set; }
        public float Excess { get; set; }
    }
}
