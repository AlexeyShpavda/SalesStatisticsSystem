using System;
using System.ComponentModel.DataAnnotations;
using SalesStatisticsSystem.WebApp.Models.Filters.Abstract;

namespace SalesStatisticsSystem.WebApp.Models.Filters
{
    public class SaleFilterModel : PagedListParameter
    {
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; } = null;
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; } = null;

        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }

        public string ProductName { get; set; }

        public decimal? SumFrom { get; set; } = null;
        public decimal? SumTo { get; set; } = null;

        public string ManagerLastName { get; set; }
    }
}