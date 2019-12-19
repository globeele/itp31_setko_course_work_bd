namespace CourseWorkDb.ViewModels
{
    public class ComparisonViewModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Features { get; set; }
        public byte[] Photo { get; set; }
        public string MeasureUnit { get; set; }
        public short Year { get; set; }
        public short Quarter { get; set; }
        public int OutputPlan { get; set; }
        public int OutputFact { get; set; }
        public int ReleasePlan { get; set; }
        public int ReleaseFact{ get; set; }
        public float Excess { get; set; }
    }
}
