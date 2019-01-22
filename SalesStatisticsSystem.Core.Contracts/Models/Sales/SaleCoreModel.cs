using System;
using SalesStatisticsSystem.Core.Contracts.Models.Sales.Abstract;

namespace SalesStatisticsSystem.Core.Contracts.Models.Sales
{
    public class SaleCoreModel : CoreModel
    {
        public DateTime Date { get; set; }

        public CustomerCoreModel Customer { get; set; }

        public ProductCoreModel Product { get; set; }

        public decimal Sum { get; set; }

        public ManagerCoreModel Manager { get; set; }

        public SaleCoreModel()
        {
        }

        public SaleCoreModel(DateTime date, CustomerCoreModel customer, ProductCoreModel product, decimal sum,
            ManagerCoreModel manager)
        {
            Date = date;
            Customer = customer;
            Product = product;
            Sum = sum;
            Manager = manager;
        }
    }
}