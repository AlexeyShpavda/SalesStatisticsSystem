using System;
using System.ComponentModel.DataAnnotations;

namespace SalesStatisticsSystem.WebApp.Models.Filters
{
    public class SaleFilterModel
    {
        [DataType(DataType.Date)]
        public DateTime? DateFrom { get; set; } = null;
        [DataType(DataType.Date)]
        public DateTime? DateTo { get; set; } = null;

        public CustomerFilterModel CustomerFilter { get; set; }

        public ProductFilterModel ProductFilter { get; set; }

        public decimal? SumFrom { get; set; } = null;
        public decimal? SumTo { get; set; } = null;

        public ManagerFilterModel ManagerFilter { get; set; }

        public SaleFilterModel()
        {
            CustomerFilter = new CustomerFilterModel();

            ProductFilter = new ProductFilterModel();

            ManagerFilter = new ManagerFilterModel();
        }
    }
}