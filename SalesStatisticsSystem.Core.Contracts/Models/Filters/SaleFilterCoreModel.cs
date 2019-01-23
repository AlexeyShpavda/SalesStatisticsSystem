using System;
using SalesStatisticsSystem.Core.Contracts.Models.Filters.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models.Filters
{
    public class SaleFilterCoreModel : PagedListParameterCoreModel
    {
        public DateTime? DateFrom { get; set; } = null;
        public DateTime? DateTo { get; set; } = null;

        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }

        public string ProductName { get; set; }

        public decimal? SumFrom { get; set; } = null;
        public decimal? SumTo { get; set; } = null;

        public string ManagerLastName { get; set; }
    }
}