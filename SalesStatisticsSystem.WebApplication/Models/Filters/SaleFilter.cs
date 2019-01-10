using System;
using System.ComponentModel.DataAnnotations;

namespace SalesStatisticsSystem.WebApplication.Models.Filters
{
    public class SaleFilter
    {
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; } = null;
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; } = null;

        public string CustomerFirstName { get; set; } = null;
        public string CustomerLastName { get; set; } = null;

        public string ProductName { get; set; } = null;

        public decimal? SumFrom { get; set; } = null;
        public decimal? SumTo { get; set; } = null;

        public string ManagerLastName { get; set; } = null;
    }
}