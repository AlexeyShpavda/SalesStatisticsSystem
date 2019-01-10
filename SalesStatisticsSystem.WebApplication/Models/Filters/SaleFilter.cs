namespace SalesStatisticsSystem.WebApplication.Models.Filters
{
    public class SaleFilter
    {
        public string DateFrom { get; set; } = null;
        public string DateTo { get; set; } = null;

        public string CustomerFirstName { get; set; } = null;
        public string CustomerLastName { get; set; } = null;

        public string ProductName { get; set; } = null;

        public string SumFrom { get; set; } = null;
        public string SumTo { get; set; } = null;

        public string ManagerLastName { get; set; } = null;
    }
}