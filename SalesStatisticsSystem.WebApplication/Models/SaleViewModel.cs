using System;

namespace SalesStatisticsSystem.WebApplication.Models
{
    public class SaleViewModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public Customer Customer { get; set; }

        public Product Product { get; set; }

        public double Sum { get; set; }

        public Manager Manager { get; set; }
    }
}