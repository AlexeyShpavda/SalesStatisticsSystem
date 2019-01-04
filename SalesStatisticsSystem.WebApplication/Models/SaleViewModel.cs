using System;

namespace SalesStatisticsSystem.WebApplication.Models
{
    public class SaleViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public CustomerViewModel CustomerViewModel { get; set; }

        public ProductViewModel ProductViewModel { get; set; }

        public double Sum { get; set; }

        public ManagerViewModel ManagerViewModel { get; set; }
    }
}