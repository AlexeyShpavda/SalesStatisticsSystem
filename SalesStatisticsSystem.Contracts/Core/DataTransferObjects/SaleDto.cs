using System;
using SalesStatisticsSystem.Contracts.Core.DataTransferObjects.Abstract;

namespace SalesStatisticsSystem.Contracts.Core.DataTransferObjects
{
    public class SaleDto : DataTransferObject
    {
        public DateTime Date { get; set; }

        public CustomerDto Customer { get; set; }

        public ProductDto Product { get; set; }

        public decimal Sum { get; set; }

        public ManagerDto Manager { get; set; }

        public SaleDto()
        {
        }

        public SaleDto(DateTime date, CustomerDto customer, ProductDto product, decimal sum, ManagerDto manager)
        {
            Date = date;
            Customer = customer;
            Product = product;
            Sum = sum;
            Manager = manager;
        }
    }
}