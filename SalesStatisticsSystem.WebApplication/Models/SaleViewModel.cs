using System;

namespace SalesStatisticsSystem.WebApplication.Models
{
    public class SaleViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public CustomerViewModel Customer { get; set; }

        public ProductViewModel Product { get; set; }

        public double Sum { get; set; }

        public ManagerViewModel Manager { get; set; }
    }
}