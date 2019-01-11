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

        public CustomerFilter CustomerFilter { get; set; }

        public ProductFilter ProductFilter { get; set; }

        public decimal? SumFrom { get; set; } = null;
        public decimal? SumTo { get; set; } = null;

        public ManagerFilter ManagerFilter { get; set; }

        public SaleFilter()
        {
            CustomerFilter = new CustomerFilter();

            ProductFilter = new ProductFilter();

            ManagerFilter = new ManagerFilter();
        }
    }
}